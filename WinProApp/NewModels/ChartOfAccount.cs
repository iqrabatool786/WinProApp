using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class ChartOfAccount
    {
        public int Id { get; set; }
        public string AccountCode { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public bool IsDetail { get; set; }
        public string? PaccountCode { get; set; }
        public int AccountLevel { get; set; }
        public string? BranchName { get; set; }
    }
}
