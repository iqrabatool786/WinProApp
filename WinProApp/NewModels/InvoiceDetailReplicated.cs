using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class InvoiceDetailReplicated
    {
        public decimal? InvoiceNo { get; set; }
        public string? ProductId { get; set; }
        public double? Saleprice { get; set; }
        public double? Qty { get; set; }
        public DateTime? Replicateddate { get; set; }
        public string? Replicatedby { get; set; }
        public string? ReplicationStatus { get; set; }
        public string? BranchName { get; set; }
    }
}
