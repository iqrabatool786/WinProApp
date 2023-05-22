namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetVendors
    {
        public int Id { get; set; }
        public string NameEng { get; set; }
        public string NameArabic { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
