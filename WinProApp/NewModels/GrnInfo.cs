using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class GrnInfo
    {
        public long Id { get; set; }
        public long IbtId { get; set; }
        public DateTime? Date { get; set; }
        public int? FromStoreId { get; set; }
        public int? ToStoreId { get; set; }
        public string? Description { get; set; }
        public decimal? Total { get; set; }
        public bool? Status { get; set; }
        public string? BranchName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
