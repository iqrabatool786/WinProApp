using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.SupplierPurchase
{
    public class EditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long SupplierId { get; set; }
        [Required]
        [Remote(action: "IsInvoiceNoWithSupplierExists", controller: "PurchaseInvoice", AdditionalFields = "SupplierId, Id")]
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
        public string? VatNo { get; set; }

        public virtual List<SupplierPurchaseDetails> Items { get; set; }
    }
}
