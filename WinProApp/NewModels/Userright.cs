using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Userright
    {
        public int? Userid { get; set; }
        public int? Moduleid { get; set; }
        public int? Entryby { get; set; }
        public DateTime? Entrydate { get; set; }
        public string? BranchName { get; set; }
    }
}
