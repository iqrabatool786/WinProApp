using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Boxopen
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? UserId { get; set; }
        public decimal? OpenCount { get; set; }
        public decimal? Storeid { get; set; }
        public decimal? Posno { get; set; }
        public string? BranchName { get; set; }
    }
}
