using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WinProApp.Resources;

namespace WinProApp.ViewModels.Warehouse.Styles
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("StyleValidateCode", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string Code { get; set; }
        [Required]
        [Remote("StyleValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameEng { get; set; }
        [Required]
        [Remote("StyleValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameArabic { get; set; }
    }
}
