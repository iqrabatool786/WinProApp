using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WinProApp.Resources;

namespace WinProApp.ViewModels.Warehouse.Categories
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("CategoryValidateNameEng", "Warehouse", AdditionalFields = "Id,ParentCategoryId", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameEng { get; set; }
        [Required]
        [Remote("CategoryValidateNameArabic", "Warehouse", AdditionalFields = "Id,ParentCategoryId", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public string NameArabic { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
