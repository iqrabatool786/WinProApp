using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class UserBalance
    {
        public int Id { get; set; }
        public string? Userid { get; set; }
        public double? Amount { get; set; }
        public string? Transactiontype { get; set; }
        public DateTime? Tdate { get; set; }
        public string? TransactionDesc { get; set; }
    }
}
