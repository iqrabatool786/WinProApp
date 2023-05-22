namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetLoyaityPointInfo
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public long InvoiceId { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? PointEarn { get; set; }
        public decimal? AddMoney { get; set; }
        public decimal? PointRadium { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
