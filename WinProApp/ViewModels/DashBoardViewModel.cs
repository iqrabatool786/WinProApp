namespace WinProApp.ViewModels
{
    public class DashBoardViewModel
    {
    }

    public class PurchaseDashBoardViewModel
    {
        public int SupplierCount { get; set; }
        public int PurchaseCount { get; set; }
        public int ShipmentCount { get; set; }
        public int RFICount { get; set;}
        public int RFQCount { get; set; }
        public int PRCount { get; set; }
        public int POCount { get; set; }
        public int PaymentCount { get; set; }
        public int PayableCount { get; set; }
        public int ReturnCount { get; set; }
        public int DamageCount { get; set; }

    }

    public class WarehouseDashBoardViewModel
    {
        public int PromotionCount { get; set; }
        public int TotalStock { get; set; }
        public int IBTCount { get; set; }
        public int GRNCount { get; set; }
        public int ReturnCount { get; set; }
        public int DamageCount { get; set; }
    }
}
