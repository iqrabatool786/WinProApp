namespace WinProApp.ViewModels.RequestForInfo
{
    public class RequestForInformationItemViewModel
    {
        public long Id { get; set; }
        public long Rfiid { get; set; }
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public string? Mfcompany { get; set; }
    }
}
