using Microsoft.Build.Framework;
using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.PurchaseReciept
{
    public class EditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long InvoiceId { get; set; }
        [Required]
        public long SupplierId { get; set; }
        [Required]
        public string? InvoiceNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? InvoiceDoc { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }
        public string? ChargesDescription { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? Total { get; set; }
        public int? StoreId { get; set; }
        public bool IsReturn { get; set; }
        public virtual List<PurchaseRecieptDetails> Items { get; set; }
    }
}
