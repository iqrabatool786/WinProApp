namespace WinProApp.ViewModels.Customers
{
    public class AddViewModel
    {
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
    }
}
