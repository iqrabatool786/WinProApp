namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetFlatDiscount
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Title { get; set; }
        public bool IsMembersOnly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedDiscount { get; set; }
        public decimal? StartAmount { get; set; }
        public bool Approved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
