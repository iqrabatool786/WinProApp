using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using WinProApp.Resources;
using Microsoft.AspNetCore.Localization;


namespace WinProApp.ViewModels.Warehouse.Vendors
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("VendorValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameEng { get; set; }
        [Required]
        [Remote("VendorValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameArabic { get; set; }
    }
}
