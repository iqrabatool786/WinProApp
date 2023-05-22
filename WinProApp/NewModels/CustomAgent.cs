using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class CustomAgent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public decimal? CommissionPersent { get; set; }
        public decimal? CommissionAmountDr { get; set; }
        public decimal? CommissionAmountCr { get; set; }
        public decimal? Balance { get; set; }
    }
}
