using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class AssetTypes
    {
        [Key]
        public int Id { get; set; }
        public string? AssetTypeEng { get; set; }
        public string? AssetTypeArabic { get; set; }
    }
}
