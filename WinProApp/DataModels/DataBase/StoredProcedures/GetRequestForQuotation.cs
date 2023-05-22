namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetRequestForQuotation
    {
        public long Id { get; set; }
        public long Rfiid { get; set; }
        public DateTime Date { get; set; }
        public DateTime? RequireDate { get; set; }
        public string Requester { get; set; } = null!;
        public string? Department { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
