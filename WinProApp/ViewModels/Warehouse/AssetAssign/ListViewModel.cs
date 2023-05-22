namespace WinProApp.ViewModels.Warehouse.AssetAssign
{
    public class ListViewModel
    {
        public long Id { get; set; }
        public long? AssetId { get; set; }
        public string? AssetNameEng { get; set; }
        public string? AssetNameArabic { get; set; }
        public int? AssetDepartmentId { get; set; }
        public string? AssetDepartmentNameEng { get; set; }
        public string? AssetDepartmentNameArabic { get; set; }
        public int? AssetLocationId { get; set; }
        public string? AssetLocationNameEng { get; set; }
        public string? AssetLocationNameArabic { get; set; }
        public string AssignTo { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
