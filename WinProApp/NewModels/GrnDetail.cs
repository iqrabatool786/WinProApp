using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class GrnDetail
    {
        public long Id { get; set; }
        public long GrnId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public string? BoxNo { get; set; }
        public int? Qty { get; set; }
        public int? ReceivedQty { get; set; }
        public decimal? Price { get; set; }
        public string? BranchName { get; set; }
        public decimal? Precentage { get; set; }
        public decimal? RetailPrice { get; set; }
    }
}
