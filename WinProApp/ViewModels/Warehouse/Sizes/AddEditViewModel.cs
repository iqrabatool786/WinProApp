using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WinProApp.Resources;

namespace WinProApp.ViewModels.Warehouse.Sizes
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("SizeValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameEng { get; set; }
        [Required]
        [Remote("SizeValidateNameArabic", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameArabic { get; set; }
    }
}
