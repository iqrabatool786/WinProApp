﻿namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetAssetDepartment
    {
        public int Id { get; set; }
        public string? DepartmentEng { get; set; }
        public string? DepartmentArabic { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
