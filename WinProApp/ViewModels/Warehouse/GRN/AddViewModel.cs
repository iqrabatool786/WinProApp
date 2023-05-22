using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Warehouse.GRN
{
    public class AddViewModel
    {
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
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
    }
}
