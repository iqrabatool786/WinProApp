using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class AssetLocation
    {
        [Key]
        public int Id { get; set; }
        public string? LocationEng { get; set; }
        public string? LocationArabic { get; set; }
    }
}
