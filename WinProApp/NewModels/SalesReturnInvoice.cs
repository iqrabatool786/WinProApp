using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesReturnInvoice
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public decimal ReturnAmountExVat { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ReturnTotal { get; set; }
        public int? TotalQty { get; set; }
        public string? ReturnVoucherNo { get; set; }
        public decimal? VoucherAmount { get; set; }
        public decimal? PointReduce { get; set; }
        public string? BranchName { get; set; }
        public long CashBoxId { get; set; }
        public long ShiftId { get; set; }
        public string ReturnType { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
        public int StoreId { get; set; }
        public bool Paid { get; set; }
    }
}
