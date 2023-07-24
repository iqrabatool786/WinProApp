using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class InvoiceDetailPosTemp
    {
        public decimal? InvoiceNo { get; set; }
        public string? ProductId { get; set; }
        public double? Saleprice { get; set; }
        public double? Qty { get; set; }
        public string? Productname { get; set; }
        public string? BranchName { get; set; }
    }
}
