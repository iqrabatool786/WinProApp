using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Store
    {
        public int Id { get; set; }
        public string? StoreCode { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? GlAccount { get; set; }
        public string? Brand { get; set; }
        public string? Brandcode { get; set; }
        public string? StoreType { get; set; }
        public string? Office { get; set; }
        public string? City { get; set; }
        public string? VatNo { get; set; }
        public string? Crno { get; set; }
        public string? BranchName { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
        public string? FooterA { get; set; }
        public string? FooterE { get; set; }
    }
}
