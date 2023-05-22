using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.SupplierPurchase
{
    public class AddViewModel
    {
        public long Id { get; set; }
        public string InvoiceType { get; set; }
        [Required]
        public long SupplierId { get; set; }
        [Required]
        [Remote(action: "IsInvoiceNoWithSupplierExists", controller: "PurchaseInvoice", AdditionalFields = "SupplierId")]
        public string InvoiceNo { get; set; }
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
        public decimal? Total { get; set; }
        public string VatNo { get; set; }
    }
}
