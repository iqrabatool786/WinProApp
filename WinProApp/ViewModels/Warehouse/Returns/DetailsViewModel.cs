namespace WinProApp.ViewModels.Warehouse.Returns
{
    public class DetailsViewModel
    {
        public long Id { get; set; }
        public long? ReturnId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public string? DescriptionEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public decimal? QtyDozen { get; set; }
        public decimal? QtyPices { get; set; }
        public decimal? Price { get; set; }
        public decimal? Vat { get; set; }
    }
}
