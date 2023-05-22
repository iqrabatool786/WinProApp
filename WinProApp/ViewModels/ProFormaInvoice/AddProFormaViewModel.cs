namespace WinProApp.ViewModels.ProFormaInvoice
{
    public class AddProFormaViewModel
    {
        public long? Pfid { get; set; }
        public DateTime Date { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal? ShippingAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Remarks { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
    }
}
