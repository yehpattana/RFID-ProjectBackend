using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using RFIDApi.Hubs;
using Microsoft.OpenApi.Models;
using RFIDApi.Service.Interface;
using RFIDApi.Service.FPSService;
using RFIDApi.Service.Tenant;
using RFIDApi.Service.DBConnect;
using RFIDApi.Models.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IMasterWarehouseService,MasterWarehouseService>();
builder.Services.AddScoped<IMasterProductOnlineService, MasterProductOnlineService>();
builder.Services.AddScoped<IWarehouseInOutTypeService, WarehouseInOutTypeService>();
builder.Services.AddScoped<IPODetailService, PODetailService>();
builder.Services.AddScoped<IPODescService, PODescService>();
builder.Services.AddScoped<IWarehouseTransactionService, WarehouseTransactionService>();
// เพิ่ม SignalR
// เพิ่ม DbContext
builder.Services.AddDbContext<RFIDDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RFIDDbConnection")));

builder.Services.AddDbContext<SystemDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SystemDBConnection")));

builder.Services.AddDbContext<FPSDbContext>((sp, options) =>
{
    var tenant = sp.GetRequiredService<ITenantService>();
    var config = sp.GetRequiredService<IConfiguration>();

    var company = tenant.GetCompany(); // ❗ ดึงจาก Claim ตอน request
    var conn = config.GetConnectionString(company);

    options.UseSqlServer(conn);
});
// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "FPSHandheld",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "FPSHandheldApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddSignalR();
// ตั้งค่า CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowNextJs", policy =>
//    {
//        policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "https://localhost:7233", "http://localhost:7233", "http://localhost:5090", "https://rfid-pos-ui-bnow.vercel.app", "http://localhost:5173", "http://192.168.1.99:3000", "https://www.ymt-group.com")
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//              .AllowCredentials();
//    });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllWithCredentials", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true) // รับทุก origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
// เพิ่ม Controller support
builder.Services.AddControllers();

// เพิ่ม Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RFID API",
        Version = "v1",
        Description = "API for managing RFID tags and related data"
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ใช้ CORS
app.UseCors("AllowAllWithCredentials");

// เพิ่ม Swagger UI (เฉพาะ Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RFID API V1");
        //c.RoutePrefix = string.Empty; // ทำให้ Swagger อยู่ที่ root
    });
}

// กำหนด SignalR Hub และ Controllers
app.MapHub<RFIDHubs>("/rfidHub");
app.MapControllers();

app.Run();