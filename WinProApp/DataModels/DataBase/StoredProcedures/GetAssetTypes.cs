namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetAssetTypes
    {
        public int Id { get; set; }
        public string? AssetTypeEng { get; set; }
        public string? AssetTypeArabic { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
