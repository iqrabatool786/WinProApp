﻿using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class InvoiceHoldDetail
    {
        public decimal? InvoiceNo { get; set; }
        public string ProductId { get; set; } = null!;
        public double? Saleprice { get; set; }
        public double? Qty { get; set; }
        public string? Productname { get; set; }
        public string? Decsrive { get; set; }
        public double? Total { get; set; }
        public double? Costprice { get; set; }
        public double? TotalCost { get; set; }
        public string? Storeid { get; set; }
        public double? Freeqty { get; set; }
        public double? Freeprice { get; set; }
    }
}
