
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_RFIDAddress")]
    public class RFIDAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        [ForeignKey("Shop")]
        public string? IP { get; set; }
        
        public bool? IsActive { get; set; }

        // Navigation Property
        public virtual POS_NDS_MasterShop? Shop { get; set; }
    }
}