using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesInvoiceHold
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public decimal InvoiceAmountExVat { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal InvoiceTotal { get; set; }
        public int? TotalQty { get; set; }
        public string? VoucherNo { get; set; }
        public decimal? VoucherAmount { get; set; }
        public decimal? RadiumPoint { get; set; }
        public decimal? Donation { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? BankAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? PointEarn { get; set; }
        public long CashBoxId { get; set; }
        public long ShiftId { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
        public int StoreId { get; set; }
        public bool UnHold { get; set; }
        public string? UnHoldBy { get; set; }
        public DateTime? UnHoldDate { get; set; }
    }
}
