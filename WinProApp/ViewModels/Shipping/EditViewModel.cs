using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Shipping
{
    public class EditViewModel
    {
        [Required]
        public long SupplierId { get; set; }

        [Required]
        public long Id { get; set; }
        [Required]
        public int ToStoreId { get; set; }
        [Required]
        public string? ReferenceNo { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }
        public string? ChargesDescription { get; set; }
        public decimal? Total { get; set; }
        public string? RecordType { get; set; }
        public bool ReceivedStatus { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
