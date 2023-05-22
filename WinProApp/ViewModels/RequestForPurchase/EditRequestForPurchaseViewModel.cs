using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.RequestForPurchase
{
    public class EditRequestForPurchaseViewModel
    {
        public long Id { get; set; }
        public long Rfqid { get; set; }
        public DateTime Date { get; set; }
        public DateTime? RequireDate { get; set; }
        public string Requester { get; set; } = null!;
        public string? Department { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }

        public virtual List<RequestForPurchaseItem> Items { get; set; }

    }
}
