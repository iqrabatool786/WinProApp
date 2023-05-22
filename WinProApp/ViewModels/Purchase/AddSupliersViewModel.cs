using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.Purchase
{
    public class AddSupliersViewModel 
    {
        [Required(ErrorMessage = "CompanyName Cannot be Empty!")]
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
        [Remote(action: "IsCrNumberExists", controller: "Purchase")]
        public string? CRNumber { get; set; }
        public string? Upload { get; set; }
        public string? TaxDocUpload { get; set; }
        public string? OtherDocUpload { get; set; }

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    //if (!string.IsNullOrEmpty(CRNumber) && (CRNumber.Length < 10 || CRNumber.Length > 10))
        //    //{
        //    //    yield return new ValidationResult(
        //    //        $"CRNumber should contain 10 digits",
        //    //        new[] { nameof(this.CRNumber) });
        //    //}

        //    if (!string.IsNullOrEmpty(VatNo) && (VatNo.Length < 10 || VatNo.Length > 10))
        //    {
        //        yield return new ValidationResult(
        //            $"VatNo should contain 10 digits",
        //            new[] { nameof(this.VatNo) });
        //    }
        //}
    }
}
