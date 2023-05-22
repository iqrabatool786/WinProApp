using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WinProApp.Resources;

namespace WinProApp.ViewModels.Warehouse.Years
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Remote("SizeValidateNameEng", "Warehouse", AdditionalFields = "Id", ErrorMessageResourceType = typeof(WebResource), ErrorMessageResourceName = "AlreadyExists")]
        public int YearName { get; set; }

    }
}
