using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesReturnInvoicePayment
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long ReturnId { get; set; }
        public decimal VatAmout { get; set; }
        public decimal TotalWithoutVat { get; set; }
        public decimal Total { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public long? CashBoxId { get; set; }
        public string? BranchName { get; set; }
    }
}
