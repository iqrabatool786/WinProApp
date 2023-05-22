using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using WinProApp.Resources;
using Microsoft.AspNetCore.Localization;

namespace WinProApp.ViewModels.Warehouse.AssetAssign
{
    public class AddEditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long? AssetId { get; set; }
        [Required]
        public string? TypeName { get; set; }
        public string? LocationName { get; set; }
        public string? DepartmentName { get; set; }
        public string AssignTo { get; set; }
        public bool Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
