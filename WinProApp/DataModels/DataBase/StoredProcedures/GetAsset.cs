namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetAsset
    {
        public long Id { get; set; }
        public int? AssetTypeId { get; set; }
        public int? AssetDepartmentId { get; set; }
        public int? AssetLocationId { get; set; }
        public string? Barcode { get; set; }
        public string? AssetNameEng { get; set; }
        public string? AssetNameArabic { get; set; }
        public string? DesignationOfStaff { get; set; }
        public string? ManufactureName { get; set; }
        public string? WarrentyPeriod { get; set; }
        public string? Temp { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? AssetValue { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
