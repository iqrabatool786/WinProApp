using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.Purchase.PurchaseAsset
{
    public class EditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long SupplierId { get; set; }
        [Required]
        public string? InvoiceNo { get; set; }
        [Required]
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
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
