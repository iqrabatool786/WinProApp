using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Customer
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? CreditLimit { get; set; }
        public string? Crno { get; set; }
        public string? VatNo { get; set; }
        public string? LedgerNo { get; set; }
        public string? BookNo { get; set; }
        public string? Crdocument { get; set; }
        public string? TaxDocument { get; set; }
        public string? OtherDocument { get; set; }
        public string? BranchName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? LoyaltyCardId { get; set; }
        public string? AccountCode { get; set; }
    }
}
