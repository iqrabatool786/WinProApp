using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using WinProApp.Resources;
using Microsoft.AspNetCore.Localization;

namespace WinProApp.ViewModels.Warehouse.Asset
{
    public class AddEditViewModel
    {
        [Required]
        public long Id { get; set; }
        public int? AssetTypeId { get; set; }
        public int? AssetDepartmentId { get; set; }
        public int? AssetLocationId { get; set; }
        public string? Barcode { get; set; }
        [Required]
        [Remote("AssetNameValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string AssetNameEng { get; set; }
        [Required]
        [Remote("AssetNameValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string AssetNameArabic { get; set; }
        public string? DesignationOfStaff { get; set; }
        public string? ManufactureName { get; set; }
        public string? WarrentyPeriod { get; set; }
        public string? Temp { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? AssetValue { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
