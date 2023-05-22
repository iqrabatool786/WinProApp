namespace WinProApp.ViewModels.Warehouse.Products
{
    public class EditViewModel
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public long? CompanyId { get; set; }
        public int? CategoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SeasonId { get; set; }
        public int? DescriptionId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int? SkuId { get; set; }
        public int? Unitid { get; set; }
        public int? VendorId { get; set; }
        public int? YearId { get; set; }
        public int? BrandId { get; set; }
        public int? GroupId { get; set; }
        public string? ProductNameEng { get; set; }
        public string? ProductNameArabic { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpDate { get; set; }
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
        public bool Status { get; set; }
    }
}
