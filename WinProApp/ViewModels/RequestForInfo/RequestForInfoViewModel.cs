namespace WinProApp.ViewModels.RequestForInfo
{
    public class RequestForInfoViewModel
    {
        public long Id { get; set; }
        public string ReqDate { get; set; }
        public string Requester { get; set; }
        public string? Department { get; set; }
        public string? Note { get; set; }
        public string Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
