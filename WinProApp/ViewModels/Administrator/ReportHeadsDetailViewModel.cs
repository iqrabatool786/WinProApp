﻿namespace WinProApp.ViewModels.Administrator
{
    public class ReportHeadStoreDetailViewModel
    {
        public int Id { get; set; }
        public int? StoreId { get; set; }
        public string? StoreCode { get; set; }
        public string? StoreAddress { get; set; }
        public string? VatNo { get; set; }
        public string? Logo { get; set; }
        public string? ReportHeaderEng { get; set; }
        public string? ReportHeaderArabic { get; set; }
        public string? ReportFooterEng { get; set; }
        public string? ReportFooterArabic { get; set; }
        public bool DefaultStore { get; set; }
    }
}
