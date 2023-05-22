using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblChartofAccount
    {
        public decimal ChartAccountId { get; set; }
        public decimal? AccountId { get; set; }
        public string Cname { get; set; } = null!;
        public decimal? Ob { get; set; }
        public string? CrDr { get; set; }
        public string? Descriptoin { get; set; }
        public string? Bank { get; set; }
        public string? Acc { get; set; }
        public string? Vatno { get; set; }
        public string? Crno { get; set; }
    }
}
