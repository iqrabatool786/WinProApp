namespace WinProApp.ViewModels.Warehouse.DamageReturns
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public long? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public long? InvoiceId { get; set; }
        public string? Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? Total { get; set; }
        public int? StoreId { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public string? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public string? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }

    public class InvoiceIdListViewModel
    {
        public long Id { get; set;}
        public string? InvoiceNo { get; set; }
    }
}
