using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using WinProApp.Resources;
using Microsoft.AspNetCore.Localization;


namespace WinProApp.ViewModels.Warehouse.Brands
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("BrandValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameEng { get; set; }
        [Required]
        [Remote("BrandValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameArabic { get; set; }
    }
}
