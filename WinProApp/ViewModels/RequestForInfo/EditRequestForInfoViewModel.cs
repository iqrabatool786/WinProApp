using System.ComponentModel.DataAnnotations;
using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.RequestForInfo
{
    public class EditRequestForInfoViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required(ErrorMessageResourceName = "The Date field is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Requester is required")]
        public string Requester { get; set; } = null!;
        public string? Department { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public virtual List<RequestForInformationItem> Items { get; set; }
    }
}
