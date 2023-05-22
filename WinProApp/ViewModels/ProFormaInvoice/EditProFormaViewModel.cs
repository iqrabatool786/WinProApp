using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.ProFormaInvoice
{
    public class EditProFormaViewModel
    {
        public long Id { get; set; }
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
        public virtual List<ProFormaInvoiceItems> Items { get; set; }
    }
}
