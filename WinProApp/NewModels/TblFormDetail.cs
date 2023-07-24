using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblFormDetail
    {
        public decimal FormDetailId { get; set; }
        public decimal? FormId { get; set; }
        public string? FormDetailControlName { get; set; }
        public string? BranchName { get; set; }
    }
}
