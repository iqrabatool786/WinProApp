using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.Purchase
{
    public class EditSupliersViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string CompanyName { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Bpcode { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OpeningBalance { get; set; }
        [RegularExpression(@"^[0-9]{15}$", ErrorMessage = "VatNo should contain 15 digits")]
        public string? VatNo { get; set; }
        public string? BankName { get; set; }
        public string? AccountNo { get; set; }
        [Required(ErrorMessage = "CRNumber Cannot be Empty!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "CRNumber should contain 10 digits")]
        [Remote(action: "IsCrNumberExists", controller: "Purchase", AdditionalFields = "Id")]
        public string? CRNumber { get; set; }
        public string? Upload { get; set; }
        public string? CRDocument { get; set; }
        public string? TaxDocUpload { get; set; }
        public string? TaxDocument { get; set; }
        public string? OtherDocUpload { get; set; }
        public string? OtherDocument { get; set; }

        public bool? Status { get; set; }
    }
}
