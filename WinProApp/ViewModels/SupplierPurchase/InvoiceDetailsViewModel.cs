namespace WinProApp.ViewModels.SupplierPurchase
{
    public class InvoiceDetailsViewModel
    {
        public long Id { get; set; }
        public long SupplierPurchaseId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public int? SkuId { get; set; }
        public string? SkuName { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public int? ColorId { get; set; }
        public string? ColorName { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public int? SeassonId { get; set; }
        public string? SeassonName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DescriptionId { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public int? BrandId { get; set; }
        public int? VendorId { get; set; }
        public int? GroupId { get; set; }
        public int? YearId { get; set; }
        public string? YearText { get; set; }
        public decimal? QtyDozen { get; set; }
        public decimal? Qtypices { get; set; }
        public decimal? Price { get; set; }
        public string? ExpireDate { get; set; }
        public string? ProductDate { get; set; }
        public int? FlagPricePerPices { get; set; }
        public string? UnitBarcode { get; set; }
        public decimal? QtyPerUnit { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? UnitSalePrice { get; set; }
        public decimal? BoxRetail { get; set; }
        public string? UnitDescription { get; set; }
        public decimal? Vat { get; set; }
        public decimal? PVat { get; set; }
    }
}
