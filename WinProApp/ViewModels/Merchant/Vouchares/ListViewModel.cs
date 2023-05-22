namespace WinProApp.ViewModels.Merchant.Vouchares
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string IsMembersOnly { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Approved { get; set; }
        public string CreatedBy { get; set; }
        public string CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
