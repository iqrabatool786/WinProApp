using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Warehouse.Returns
{
    public class AddViewModel
    {
        [Required]
        public long SupplierId { get; set; }
        public long? InvoiceId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? VatAmount { get; set; }
        [Required]
        public decimal? Total { get; set; }
        public int? StoreId { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public string? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public string? UpdatedDate { get; set; }
    }
}
