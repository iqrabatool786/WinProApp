using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class PurchaseOrderItem
    {
        public long Id { get; set; }
        public long Poid { get; set; }
        public string? PartNo { get; set; }
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
    }
}
