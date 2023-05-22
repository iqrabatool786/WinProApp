namespace WinProApp.ViewModels.Merchant.GetPrecentageOff
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public long BuyProductId { get; set; }
        public string BuyBarcode { get; set; }
        public int BuyQty { get; set; }
        public decimal OffPrecentage { get; set; }
        public string IsMembersOnly { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DiscountType { get; set; }
        public string Approved { get; set; }
    }
}
