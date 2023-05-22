namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetIbtInfo
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int FromStoreId { get; set; }
        public int ToStoreId { get; set; }
        public string? Description { get; set; }
        public decimal? Total { get; set; }
        public bool Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
