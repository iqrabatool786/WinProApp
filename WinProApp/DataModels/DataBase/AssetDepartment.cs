using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class AssetDepartment
    {
        [Key]
        public int Id { get; set; }
        public string? DepartmentEng { get; set; }
        public string? DepartmentArabic { get; set; }
    }
}
