using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models
{
    [Table("RFID_RFIDTag")]
    public class RFIDTag
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [StringLength(4000)] // max length of NVARCHAR(MAX) is 4000 in SQL Server
        public string? EPC { get; set; }

        [Required] // Marked as required, cannot be null
        [DataType(DataType.DateTime)] // For date-time formatting
        public DateTime ReadTime { get; set; }

        [Required] // Marked as required, cannot be null
        public int IsActive { get; set; }

    }
}