using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Company
    {
        public long Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Status { get; set; }
        public string? InsertBy { get; set; }
        public string? Bpcode { get; set; }
        public decimal? Balance { get; set; }
        public double? OpeningBalance { get; set; }
        public string? BranchName { get; set; }
        public double? VatNo { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
