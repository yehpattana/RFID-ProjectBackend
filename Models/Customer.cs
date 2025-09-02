using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models
{
    [Table("RFID_Customer")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; } // Primary Key with Identity

        [Required]
        [StringLength(255)]
        public string? CustomerName { get; set; } // ชื่อลูกค้า (Required, Maximum length 255)

        [StringLength(20)]
        public string? Phone { get; set; } // เบอร์โทร (Maximum length 20)

        [StringLength(100)]
        public string? Email { get; set; } // อีเมล (Maximum length 100)

        [StringLength(255)]
        public string? Address { get; set; } // ที่อยู่ (Maximum length 255)

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; } = DateTime.Now; // วันที่สร้าง (Default value as current date)
    }
}