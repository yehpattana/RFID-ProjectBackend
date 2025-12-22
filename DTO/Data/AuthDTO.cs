namespace RFIDApi.DTO.Data
{
    public class LoginRequest
    {
        public string CompanyCode { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public UserInfo? User { get; set; }
    }

    public class UserInfo
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
    }

    public class CompanyDto
    {
        public string CompanyCode { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
    }
}
