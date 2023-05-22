namespace WinProApp.ViewModels.SupplierPurchase
{
    public class SupplierInfo
    {
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
        public bool? Status { get; set; }
    }
}
