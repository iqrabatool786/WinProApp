using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class Descriptions
    {
        [Key]
        public int Id { get; set; }
        public string NameEng { get; set; }
        public string NameArabic { get; set; }
    }
}
