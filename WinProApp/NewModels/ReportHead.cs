using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class ReportHead
    {
        public int Id { get; set; }
        public int? StoreId { get; set; }
        public string? Logo { get; set; }
        public string? ReportHeaderEng { get; set; }
        public string? ReportHeaderArabic { get; set; }
        public string? ReportFooterEng { get; set; }
        public string? ReportFooterArabic { get; set; }
        public bool DefaultStore { get; set; }
        public string? Vatnum { get; set; }
        public string? Storename { get; set; }
    }
}
