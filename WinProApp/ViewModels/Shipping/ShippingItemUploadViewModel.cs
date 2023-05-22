namespace WinProApp.ViewModels.Shipping
{
    public class ShippingItemUploadViewModel
    {
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
        public string? QtyPerBox { get; set; }
        public string? Qty { get; set; }
        public string? Price { get; set; }
        public string? Total { get; set; }
        public string? SalePrice { get; set; }
        public string? SaleVat { get; set; }
        public string? OriginalPrice { get; set; }
        public string? ExpireDate { get; set; }
        public string? ProductDate { get; set; }
        public string? ImageUrl { get; set; }

    }
}
