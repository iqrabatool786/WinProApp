using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Warehouse.Returns
{
    public class EditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long? SupplierId { get; set; }
        public long? InvoiceId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? Total { get; set; }
        public int? StoreId { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public string? UpdatedDate { get; set; }
    }
}
