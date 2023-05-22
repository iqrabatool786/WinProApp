namespace WinProApp.ViewModels.Warehouse.IBT
{
    public class ItemDetailsViewModel
    {
        public long Id { get; set; }
        public long IbtId { get; set; }
        public long ProductId { get; set; }
        public string Barcode { get; set; }
        public string DescriptionEnglish { get; set; }
        public string DescriptionArabic { get; set; }
        public string? BoxNo { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Precentage { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? ItemTotal { get; set; }
    }
}
