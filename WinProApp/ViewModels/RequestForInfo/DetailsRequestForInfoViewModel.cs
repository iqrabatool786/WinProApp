using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.RequestForInfo
{
    public class DetailsRequestForInfoViewModel
    {
        public long Id { get; set; }
        public string ReqDate { get; set; }
        public string Requester { get; set; }
        public string? Department { get; set; }
        public string? Note { get; set; }
        public string Approved { get; set; }
        public string ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public virtual List<RequestForInformationItem> Items { get; set; }
    }
}
