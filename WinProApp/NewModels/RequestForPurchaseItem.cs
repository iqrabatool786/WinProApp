using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class RequestForPurchaseItem
    {
        public long Id { get; set; }
        public long PrId { get; set; }
        public string? PartNo { get; set; }
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public string? Mfcompany { get; set; }
        public string? BranchName { get; set; }
    }
}
