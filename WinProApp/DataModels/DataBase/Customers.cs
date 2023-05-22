using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WinProApp.DataModels.DataBase
{
    public class Customers
    {
        [Key]
        public long Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? CreditLimit { get; set; }
        public string? CRNo { get; set; }
        public string? VatNo { get; set; }
        public string? LedgerNo { get; set; }
        public string? BookNo { get; set; }
        public string? CRDocument { get; set; }
        public string? TaxDocument { get; set; }
        public string? OtherDocument { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
        public long? LoyaltyCardId { get; set; }
        public int? CompanyId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public bool? IsDiscountPercentage { get; set; }
    }
}
