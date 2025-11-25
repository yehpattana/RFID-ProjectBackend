using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RFIDApi.Models
{

    public class RFIDTag
    {

        public string? EPC { get; set; }
        public string? Reader_Name { get; set; }
        public int? ANT_NUM { get; set; }
        public double? RSSI { get; set; }
        public DateTime ReadTime { get; set; }


    }
}