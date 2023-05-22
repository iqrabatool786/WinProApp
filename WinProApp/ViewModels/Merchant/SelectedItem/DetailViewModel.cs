namespace WinProApp.ViewModels.Merchant.SelectedItem
{
    public class DetailViewModel
    {
        public long Id { get; set; }
        public int StoreId { get; set; }
        public string IsMembersOnly { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedDiscount { get; set; }
        public decimal? StartAmount { get; set; }
        public string Approved { get; set; }

    }

    public class ProductBarcodeAutoComplete
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string BarcodeWithName { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionArabic { get; set; }
    }

    public class ProductInfoViewModel
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionArabic { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public string OrgPrice { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
