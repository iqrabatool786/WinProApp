using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.PurchaseOrder
{
    public class EditViewModel
    {
        public long Id { get; set; }
        public long? Prid { get; set; }
        public DateTime Date { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal? ShippingAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Remarks { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public virtual List<PurchaseOrderItems> Items { get; set; }
    }
}
