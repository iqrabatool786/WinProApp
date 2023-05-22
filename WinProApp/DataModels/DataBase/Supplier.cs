using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public partial class Supplier
    {
        [Key]
        public long Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Bpcode { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OpeningBalance { get; set; }
        public string? VatNo { get; set; }
        public string? BankName { get; set; }
        public string? AccountNo { get; set; }
        public string? CRNumber { get; set; }
        public string? CRDocument { get; set; }
        public string? TaxDocument { get; set; }
        public string? OtherDocument { get; set; }
        public bool? Status { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
    }
}
