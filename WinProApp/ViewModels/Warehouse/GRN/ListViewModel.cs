namespace WinProApp.ViewModels.Warehouse.GRN
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public long IbtId { get; set; }
        public DateTime Date { get; set; }
        public string StrDate { get; set; }
        public int FromStoreId { get; set; }
        public string FromStoreName { get; set; }
        public int ToStoreId { get; set; }
        public string ToStoreName { get; set; }
        public string? Description { get; set; }
        public string? Total { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
