using RFIDApi.Service.Tenant;

namespace RFIDApi.Service.DBConnect
{
    public class TenantService : ITenantService
    {

        private readonly ILogger<TenantService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantService(ILogger<TenantService> logger,IConfiguration configuration,IHttpContextAccessor httpContextAccessor) { 
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCompany()
        {
            var company = _httpContextAccessor.HttpContext?
                .User?
                .Claims?
                .FirstOrDefault(c => c.Type == "CompanyCode")?
                .Value;

            if (string.IsNullOrWhiteSpace(company))
                throw new Exception("Company claim not found");

            return company;
        }
        public string GetConnectionString(string company)
        {
            try
            {

                return _configuration.GetConnectionString(company) ?? throw new Exception($"ConnectionString for {company} not found");

            }
            catch (Exception ex) { 
                return ex.Message;
            }
        }
    }
}
