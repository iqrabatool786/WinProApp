using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblInsurance
    {
        public decimal Idd { get; set; }
        public decimal? Customerid { get; set; }
        public double? InsuredRecevied { get; set; }
        public double? InsuredReturn { get; set; }
        public double? Deduct { get; set; }
        public string? DeductReason { get; set; }
        public double? Balance { get; set; }
        public DateTime? ReceiveDate { get; set; }
    }
}
