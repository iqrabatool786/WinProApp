namespace WinProApp.ViewModels.PurchaseReciept
{
    public class DetailsUpdateViewModel
    {
        public long Id { get; set; }
        public long ReceiptId { get; set; }
        public long ProductId { get; set; }
        public string ItemBarcode { get; set; }
        public decimal? QtyDozen { get; set; }
        public decimal? QtyPices { get; set; }
        public decimal? Price { get; set; }
        public decimal? ReceiveQtyDozen { get; set; }
        public decimal? ReceiveQtyPices { get; set; }
        public decimal? ReceivePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? BoxRetail { get; set; }
        public decimal? OrgPrice { get; set; }
        public decimal? Vat { get; set; }
        public decimal? PVat { get; set; }
    }
}
