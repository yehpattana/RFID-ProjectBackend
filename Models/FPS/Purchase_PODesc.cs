using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RFIDApi.Models.FPS
{
    [Table("Purchase_PODesc")]
    
    public class Purchase_PODesc
    {
        [Key]
        [Required]
        public string PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string? PRNo { get; set; }
        public string? CompanyCode { get; set; }
        public int? POType { get; set; }
        public bool ConfirmPO { get; set; }
        public bool ApprovePO { get; set; }
        public DateTime? ApprovePO_Date { get; set; }
        public bool CloseStatus { get; set; }
        public string? CloseBy { get; set; }
        public DateTime? CloseDate { get; set; }
        public bool CancelStatus { get; set; }
        public string? CancelBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public string? ShipMode { get; set; }
        public string? PaymentType { get; set; }
        public int? TermDay { get; set; }
        public string? PaymentDetail { get; set; }
        public string? PaymentTerm { get; set; }
        public string? TermDetail { get; set; }
        public string? Currency { get; set; }
        public double? Rates { get; set; }
        public string? PODesc { get; set; }
        public string? PORemark { get; set; }
        public string? VendorId { get; set; }
        public string? DlvCode { get; set; }
        public string? UserCreate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UserEdit { get; set; }
        public DateTime? EditDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? FinalETA { get; set; }
        public string? StepType { get; set; }
        public int? PreviousStep { get; set; }
        public int? CurrentStep { get; set; }
        public int? ApproveTime { get; set; }
        public double? PONetAmount { get; set; }
        public double? POAmountBeforeVat { get; set; }
        public double? POTotalAmount { get; set; }
    }
}
