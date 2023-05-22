using System;
using System.Collections.Generic;

namespace WinProApp.DataModels.DataBase
{
    public partial class RequestForPurchaseItem
    {
        public long Id { get; set; }
        public long Prid { get; set; }
        public string? PartNo { get; set; }
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public string? Mfcompany { get; set; }
    }
}
