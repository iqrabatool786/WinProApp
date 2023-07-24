using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblLedger
    {
        public decimal LedgerId { get; set; }
        public decimal? ChartofAccountId { get; set; }
        public decimal? CrAmount { get; set; }
        public decimal? DrAmount { get; set; }
        public decimal? Iid { get; set; }
        public string? SType { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? StoreId { get; set; }
        public string? PerticuterName { get; set; }
        public string? Refarance { get; set; }
        public string? BranchName { get; set; }
    }
}
