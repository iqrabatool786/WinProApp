using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class InvoiceDetail
    {
        public decimal? InvoiceNo { get; set; }
        public string? ProductId { get; set; }
        public double? Saleprice { get; set; }
        public double? Qty { get; set; }
        public string? Productname { get; set; }
        public decimal? Total { get; set; }
        public decimal? Costprice { get; set; }
        public double? TotalCost { get; set; }
        public double? Storeid { get; set; }
        public double? Freeqty { get; set; }
        public double? Freeprice { get; set; }
        public string? Decsrive { get; set; }
        public double? Persd { get; set; }
        public double? Persons { get; set; }
    }
}
