using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class ClosingHistory
    {
        public decimal Id { get; set; }
        public string Registerid { get; set; } = null!;
        public DateTime Cdate { get; set; }
        public int Userid { get; set; }
        public double? Actualamountpaidcash { get; set; }
        public double? Confirmedamountpaidcash { get; set; }
        public double? Actualamountpaidbank { get; set; }
        public double? Confirmedamountpaidbank { get; set; }
        public double? Actualdueamount { get; set; }
        public double? Confirmeddueamount { get; set; }
        public double? ActualcashtillopeningBalance { get; set; }
        public double? ConfirmedcashtillopeningBalance { get; set; }
        public decimal? Deposited { get; set; }
        public string? Postingstatus { get; set; }
        public double? Vat { get; set; }
        public double? Donation { get; set; }
        public double? VatBank { get; set; }
        public string? Storeid { get; set; }
        public string? PostingCash { get; set; }
        public decimal? Discount { get; set; }
    }
}
