namespace WinProApp.ViewModels.Warehouse.Asset
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public int? AssetTypeId { get; set; }
        public string? AssetTypeNameEng { get; set; }
        public string? AssetTypeNameArabic { get; set; }
        public int? AssetDepartmentId { get; set; }
        public string? AssetDepartmentNameEng { get; set; }
        public string? AssetDepartmentNameArabic { get; set; }
        public int? AssetLocationId { get; set; }
        public string? AssetLocationNameEng { get; set; }
        public string? AssetLocationNameArabic { get; set; }
        public string? Barcode { get; set; }
        public string? AssetNameEng { get; set; }
        public string? AssetNameArabic { get; set; }
        public string? DesignationOfStaff { get; set; }
        public string? ManufactureName { get; set; }
        public string? WarrentyPeriod { get; set; }
        public string? Temp { get; set; }
        public string? ManufactureDate { get; set; }
        public string? ExpireDate { get; set; }
        public decimal? AssetValue { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
