namespace WinProApp.ViewModels.Purchase.PurchaseAsset
{
    public class ItemDetailsViewModel
    {
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        public long? ProductId { get; set; }
        public string Barcode { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Vat { get; set; }
        public string? ExpireDate { get; set; }
        public string? ProductDate { get; set; }
    }
}
