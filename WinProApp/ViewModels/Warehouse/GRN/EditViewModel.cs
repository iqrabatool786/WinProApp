using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Warehouse.GRN
{
    public class EditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long IbtId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int FromStoreId { get; set; }
        [Required]
        public int ToStoreId { get; set; }
        public string? Description { get; set; }
        public decimal? Total { get; set; }
        public bool Status { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
