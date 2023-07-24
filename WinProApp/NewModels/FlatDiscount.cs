using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class FlatDiscount
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Title { get; set; } = null!;
        public bool IsMembersOnly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedDiscount { get; set; }
        public decimal? StartAmount { get; set; }
        public bool Approved { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string? BranchName { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
    }
}
