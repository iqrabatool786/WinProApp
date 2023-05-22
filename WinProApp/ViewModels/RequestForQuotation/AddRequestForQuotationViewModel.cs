using Microsoft.Build.Framework;

namespace WinProApp.ViewModels.RequestForQuotation
{
    public class AddRequestForQuotationViewModel
    {
        [Required]
        public long Rfiid { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public DateTime? RequireDate { get; set; }
        public string Requester { get; set; } = null!;
        public string? Department { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }

    }
}
