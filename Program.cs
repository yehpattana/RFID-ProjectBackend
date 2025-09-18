using Microsoft.EntityFrameworkCore;
using RFIDApi.Models;
using Microsoft.AspNetCore.SignalR;
using RFIDApi.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// เพิ่ม DbContext
builder.Services.AddDbContext<RFIDDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RFIDDbConnection")));

// เพิ่ม SignalR
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