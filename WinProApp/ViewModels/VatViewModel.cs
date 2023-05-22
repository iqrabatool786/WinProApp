namespace WinProApp.ViewModels
{
    public class VatViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal? Percentage { get; set; }
        public bool Status { get; set; }
        public DateTime? Vat_Date { get; set; }
    }
}
