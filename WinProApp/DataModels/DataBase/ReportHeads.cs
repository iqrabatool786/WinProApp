using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class ReportHeads
    {
        [Key]
        public int Id { get; set; }
        public int? StoreId { get; set; }
        public string? Logo { get; set; }
        public string? ReportHeaderEng { get; set; }
        public string? ReportHeaderArabic { get; set; }
        public string? ReportFooterEng { get; set; }
        public string? ReportFooterArabic { get; set; }
        public bool DefaultStore { get; set; }
    }
}
