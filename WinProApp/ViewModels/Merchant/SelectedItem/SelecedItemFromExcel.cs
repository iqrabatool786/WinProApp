namespace WinProApp.ViewModels.Merchant.SelectedItem
{
    public class SelecedItemFromExcel
    {
        public long ProductId { get; set; }
        public string Barcode { get; set; }
        public int? DiscountPercentage { get; set; }
        public decimal? DiscountFixed { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
    }
}
