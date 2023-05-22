using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class VoidMaster1
    {
        public int Id { get; set; }
        public long? Customerid { get; set; }
        public decimal? InvoiceAmountExvat { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? Vatmount { get; set; }
        public decimal? Invoicetotal { get; set; }
        public decimal? Totalqty { get; set; }
        public string? VoucherNo { get; set; }
        public decimal? VoucherAmount { get; set; }
        public decimal? Radiumpoint { get; set; }
        public decimal? Donation { get; set; }
        public decimal? Cashamount { get; set; }
        public decimal? Bankamount { get; set; }
        public decimal? Balanceamount { get; set; }
        public decimal? Pointearn { get; set; }
        public long? Cashboxid { get; set; }
        public long? Shiftid { get; set; }
        public string? Createby { get; set; }
        public DateTime? Createdate { get; set; }
        public int? Storeid { get; set; }
        public string? Statuss { get; set; }
    }
}
