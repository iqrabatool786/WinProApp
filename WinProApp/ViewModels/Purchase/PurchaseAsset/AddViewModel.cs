using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.Purchase.PurchaseAsset
{
    public class AddViewModel
    {
        [Required]
        public long SupplierId { get; set; }
        [Required]
        public string? InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }
        public string? ChargesDescription { get; set; }
        public decimal? VatAmount { get; set; }
        [Required]
        public decimal? Total { get; set; }
        public string? VatNo { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
