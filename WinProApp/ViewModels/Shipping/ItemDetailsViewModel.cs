namespace WinProApp.ViewModels.Shipping
{
    public class ItemDetailsViewModel
    {
        public long Id { get; set; }
        public long ShippingId { get; set; }
        public string? Barcode { get; set; }
        public long? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public int? SkuId { get; set; }
        public string? Sku { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public int? ColorId { get; set; }
        public string? ColorName { get; set; }
        public int? SeassonId { get; set; }
        public string? SessonName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? YearId { get; set; }
        public string? YearName { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }

        public string? BoxNo { get; set; }
        public int? QtyPerBox { get; set; }
        public int? Qty { get; set; }
        public int? ReceivedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? SaleVat { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string? StrSalePrice { get; set; }
        public string? StrSaleVat { get; set; }
        public string? StrOriginalPrice { get; set; }
        public string? ExpireDate { get; set; }
        public string? ProductDate { get; set; }
        public string? ProductImage { get; set; }
    }


    public class BarcodeGenViewModel
    {
        public int CurrentStoreId { get; set; }
        public string? CurrentBarcodeText { get; set; }
        public string? CurrentProductNameEng { get; set; }
        public string? CurrentProductNameArabic { get; set; }
        public string? CurrentProductPrice { get; set; }
    }
}
