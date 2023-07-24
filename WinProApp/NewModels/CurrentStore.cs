using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class CurrentStore
    {
        public int Id { get; set; }
        public string StoreCode { get; set; } = null!;
        public string NameEnglish { get; set; } = null!;
        public string? NameArabic { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? VatNo { get; set; }
        public string? Crno { get; set; }
        public string? Logo { get; set; }
        public string? BranchName { get; set; }
        public string? FooterA { get; set; }
        public string? FooterE { get; set; }
    }
}
