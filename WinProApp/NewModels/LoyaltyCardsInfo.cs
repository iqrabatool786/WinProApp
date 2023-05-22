using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class LoyaltyCardsInfo
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public decimal? Balance { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
    }
}
