using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using WinProApp.Resources;
using Microsoft.AspNetCore.Localization;

namespace WinProApp.ViewModels.Warehouse.AssetTypes
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("AssetTypeValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string AssetTypeEng { get; set; }
        [Required]
        [Remote("AssetTypeValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string AssetTypeArabic { get; set; }
    }
}
