namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetAssetAssign
    {
        public long Id { get; set; }
        public long? AssetId { get; set; }
        public string? AssignTo { get; set; }
        public bool Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
