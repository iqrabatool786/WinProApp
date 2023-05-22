namespace WinProApp.ViewModels.Warehouse
{
    public class ListStoreViewModel
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string GlAccount { get; set; }
        public string Brand { get; set; }
        public string Brandcode { get; set; }
        public string StoreType { get; set; }
        public string Office { get; set; }
        public string City { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? VatNo { get; set; }
        public string? CRNo { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
