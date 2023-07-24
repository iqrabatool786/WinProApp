using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class IbtDetail
    {
        public long Id { get; set; }
        public long IbtId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public string? BoxNo { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public string? BranchName { get; set; }
        public decimal? Precentage { get; set; }
        public decimal? RetailPrice { get; set; }
    }
}
