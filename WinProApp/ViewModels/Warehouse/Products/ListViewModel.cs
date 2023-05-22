namespace WinProApp.ViewModels.Warehouse.Products
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public long? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? SeasonId { get; set; }
        public string? SessionName { get; set; }
        public int? DescriptionId { get; set; }
        public string? Description { get; set; }
        public int? ColorId { get; set; }
        public string? ColorName { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public int? SkuId { get; set; }
        public string? SkuName { get; set; }
        public int? Unitid { get; set; }
        public string? UnitName { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public int? YearId { get; set; }
        public string? YearName { get; set; }

        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? ProductNameEng { get; set; }
        public string? ProductNameArabic { get; set; }
        public string? MfgDate { get; set; }
        public string? ExpDate { get; set; }
        public decimal? ProductInitialPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Discount { get; set; }
        public string? UnitBarcode { get; set; }
        public string? UnitDescription { get; set; }
        public long? QtyPerUnit { get; set; }
        public decimal? UnitSalePrice { get; set; }
        public decimal? UnitCost { get; set; }
        public string? BoxNo { get; set; }
        public string? MinQty { get; set; }
        public string? PackBarcode { get; set; }
        public string? PackDescription { get; set; }
        public int? QtyPerPack { get; set; }
        public decimal? PackPrice { get; set; }
        public decimal? PackCost { get; set; }
        public decimal? OreginalPrice { get; set; }
        public decimal? Vat { get; set; }
        public string? CustomField1 { get; set; }
        public string? CustomField2 { get; set; }
        public string? CustomField3 { get; set; }
        public string? CustomField4 { get; set; }
        public string? Image { get; set; }
        public string? WarrantyPeriod { get; set; }
        public long? Currentstock { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public string? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public string? UpdatedDate { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
