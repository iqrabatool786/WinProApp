using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using WinProApp.Resources;
using Microsoft.AspNetCore.Localization;

namespace WinProApp.ViewModels.Warehouse.AssetDepartment
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("AssetDepartmentValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string DepartmentEng { get; set; }
        [Required]
        [Remote("AssetDepartmentValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string DepartmentArabic { get; set; }
    }
}
