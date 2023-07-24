using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesInvoiceReturnItem
    {
        public long Id { get; set; }
        public long ReturnId { get; set; }
        public long ProductId { get; set; }
        public string? Barcode { get; set; }
        public decimal? ItemAmount { get; set; }
        public int Qty { get; set; }
        public decimal? ItemTotal { get; set; }
        public string? BranchName { get; set; }
    }
}
