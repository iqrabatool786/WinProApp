using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblAccount
    {
        public decimal AccountId { get; set; }
        public decimal? FamilyId { get; set; }
        public string? AccountName { get; set; }
        public string? Accounttype { get; set; }
    }
}
