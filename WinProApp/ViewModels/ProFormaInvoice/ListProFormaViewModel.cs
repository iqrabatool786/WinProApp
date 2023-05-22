namespace WinProApp.ViewModels.ProFormaInvoice
{
    public class ListProFormaViewModel
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal? ShippingAmount { get; set; }
        public string? DeliveryDate { get; set; }
        public string? Remarks { get; set; }
        public string Approved { get; set; }
        public string? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public string UpdatedDate { get; set; }

        public string? totQty { get; set; }
        public string? totPrice { get; set; }
        public string? totTax { get; set; }
        public string? totToatal { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
