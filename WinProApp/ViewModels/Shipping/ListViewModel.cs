namespace WinProApp.ViewModels.Shipping
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int ToStoreId { get; set; }
        public string? StoreName { get; set; }
        public string? StoreVatNo { get; set; }
        public string? ReferenceNo { get; set; }
        public string Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }
        public string? ChargesDescription { get; set; }
        public decimal? Total { get; set; }
        public string? StrDiscount { get; set; }
        public string? StrOtherCharges { get; set; }
        public string? StrTotal { get; set; }
        public string? RecordType { get; set; }
        public string ReceivedStatus { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public string? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public string? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }

    }

    public class StoreInfoViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? StoreCode { get; set; }
        public string? Address { get; set; } = null!;
        public string? VatNo { get; set; }

    }

    public class ShippingRecordViewModel
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; } = null!;
    }
}
