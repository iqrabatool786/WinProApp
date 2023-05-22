using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class ReturnInvoice
    {
        public decimal Returnid { get; set; }
        public decimal? ReturnidRegisterWise { get; set; }
        public string Registerid { get; set; } = null!;
        public string Invoiceno { get; set; } = null!;
        public string? Productno { get; set; }
        public double? Returnqty { get; set; }
        public double? Returnprice { get; set; }
        public string? Returnby { get; set; }
        public string? Customerid { get; set; }
        public DateTime? Returndate { get; set; }
        public string? Statuss { get; set; }
        public double? Storeid { get; set; }
        public double? Returndiscount { get; set; }
        public decimal? Returntotal { get; set; }
        public string? Productname { get; set; }
    }
}
