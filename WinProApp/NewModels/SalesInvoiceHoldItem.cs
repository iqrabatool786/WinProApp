using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesInvoiceHoldItem
    {
        public long Id { get; set; }
        public long HoldId { get; set; }
        public long ProductId { get; set; }
        public string? Barcode { get; set; }
        public decimal AmountExVat { get; set; }
        public decimal ItemDiscount { get; set; }
        public decimal VatAmount { get; set; }
        public int Qty { get; set; }
        public decimal ItemTotalDiscount { get; set; }
        public decimal ItemTotalVat { get; set; }
        public decimal ItemTotalExVat { get; set; }
        public decimal ItemTotalInVat { get; set; }
        public decimal ItemOrgSalePrice { get; set; }
        public decimal ItemOrgSaleVat { get; set; }
        public string? BranchName { get; set; }
    }
}
