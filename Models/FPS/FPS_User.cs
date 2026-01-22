using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models.FPS
{
    [Table("FPS_User")]
    public class FPS_User
    {
        [Key] [Required]
        public string UserName { get; set; } = null!;

        public string? Password { get; set; }

        public string? Department { get; set; }

        public string? Email { get; set; }

        public string? EmployeeId { get; set; }

        public string? GroupMenuUser { get; set; }

        public byte[]? UserImage { get; set; }

        public byte[]? Signature { get; set; }

        public short? ReportDisplay { get; set; }

        public bool? InActive { get; set; }

        public string? CustomerId { get; set; }

        public string? VendorId { get; set; }

        public bool? AppCostingAccount { get; set; }

        public bool? AppCosting { get; set; }

        public bool? AppForecast { get; set; }

        public byte[]? UserSign { get; set; }
    }
}
