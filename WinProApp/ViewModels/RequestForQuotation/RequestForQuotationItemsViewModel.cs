namespace WinProApp.ViewModels.RequestForQuotation
{
    public class RequestForQuotationItemsViewModel
    {
        public long Id { get; set; }
        public long Rfqid { get; set; }
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public string? Mfcompany { get; set; }
    }
}
