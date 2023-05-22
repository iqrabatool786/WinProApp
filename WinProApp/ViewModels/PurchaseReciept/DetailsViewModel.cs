namespace WinProApp.ViewModels.PurchaseReciept
{
    public class DetailsViewModel
    {
        public long Id { get; set; }
        public long ReceiptId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? SkuId { get; set; }
        public string? SkuCode { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public int? ColorId { get; set; }
        public string? ColorName { get; set; }
        public int? SeassonId { get; set; }
        public string? SessonName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DescriptionId { get; set; }
        public string? Description { get; set; }
        public int? UnitId { get; set; }
        public int? BrandId { get; set; }
        public int? VendorId { get; set; }
        public int? GroupId { get; set; }
        public int? YearId { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public decimal? QtyDozen { get; set; }
        public decimal? QtyPices { get; set; }
        public decimal? Price { get; set; }
        public decimal? ReceiveQtyDozen { get; set; }
        public decimal? ReceiveQtyPices { get; set; }
        public decimal? ReceivePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? BoxRetail { get; set; }
        public string? UnitDescription { get; set; }
        public decimal? Vat { get; set; }
        public decimal? PVat { get; set; }
    }
}
