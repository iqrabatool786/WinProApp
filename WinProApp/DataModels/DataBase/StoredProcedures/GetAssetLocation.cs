namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetAssetLocation
    {
        public int Id { get; set; }
        public string? LocationEng { get; set; }
        public string? LocationArabic { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
