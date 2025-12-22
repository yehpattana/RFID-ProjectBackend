using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using RFIDApi.Models.Context;
using RFIDApi.DTO.Data;
using RFIDApi.Models.FPS;
using RFIDApi.Models.System;
using RFIDApi.Service.Tenant;

namespace FPSHandheld.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SystemDBContext _sysDbContext;

        private readonly ITenantService _tenantService;
        private readonly IConfiguration _configuration;

        public AuthController(
            SystemDBContext sysDbContext,

            ITenantService tenantService,
            IConfiguration configuration)
        {
            _sysDbContext = sysDbContext;

            _tenantService = tenantService;
            _configuration = configuration;
        }

        /// <summary>
        /// Get list of companies for login dropdown
        /// </summary>
        [HttpGet("companies")]
        public async Task<ActionResult<List<CompanyDto>>> GetCompanies()
        {
            var allCompanies = await _sysDbContext._Companies.ToListAsync();

            var companies = allCompanies
                .Where(c => !string.IsNullOrEmpty(c.DBName))
                .Select(c => new CompanyDto
                {
                    CompanyCode = c.CompanyCode,
                    CompanyName = c.CompanyName
                })
                .ToList();

            return Ok(companies);
        }

        /// <summary>
        /// Login with company, username and password
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validate company exists
                var company = await _sysDbContext._Companies
                    .FirstOrDefaultAsync(c => c.CompanyCode == request.CompanyCode);

                if (company == null)
                {
                    return Ok(new LoginResponse
                    {
                        Success = false,
                        Message = "Company not found"
                    });
                }

                // Connect to company database using company-specific credentials

                var conn = _configuration.GetConnectionString(company.CompanyCode);

                var options = new DbContextOptionsBuilder<FPSDbContext>()
                    .UseSqlServer(conn)
                    .Options;


                using var companyDb = new FPSDbContext(options);


                // Validate user (InActive = false or null means active)
                var user = await companyDb.users
                    .FirstOrDefaultAsync(u =>
                        u.UserName == request.UserName &&
                        u.Password == request.Password &&
                        (u.InActive == false || u.InActive == null));

                if (user == null)
                {
                    return Ok(new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    });
                }

                // Generate JWT token
                var token = GenerateJwtToken(user, company);

                return Ok(new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    User = new UserInfo
                    {
                        UserName = user.UserName,
                        FullName = user.UserName,
                        CompanyCode = request.CompanyCode,
                        CompanyName = company.CompanyName
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        private string GenerateJwtToken(FPS_User user, SYS_Company company)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("CompanyCode", company.CompanyCode),
                new Claim("FullName", user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
