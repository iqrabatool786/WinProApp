using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesInvoiceVoucher
    {
        public long Id { get; set; }
        public int IssueStoreId { get; set; }
        public long InvoiceId { get; set; }
        public DateTime IssueDate { get; set; }
        public long CashBoxId { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal Amount { get; set; }
        public int? UsedStoreId { get; set; }
        public long? UsedInvoiceId { get; set; }
        public DateTime? UsedDate { get; set; }
        public bool IsUsed { get; set; }
        public string? VocherType { get; set; }
    }
}
