using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models.System
{
    [Table("SYS_Company")]
    public class SYS_Company
    {
        [Key] [Required]    
        public string CompanyCode { get; set; }

        public string? CompanyName { get; set; }

        public string? DBName { get; set; }

        public string? UserDB { get; set; }

        public string? PassDB { get; set; }

        public string? APPPath { get; set; }

        public bool ShowInWeb { get; set; }
    }
}
