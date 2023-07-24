using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class ProductPrice
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public decimal? Price1Vat { get; set; }
        public decimal? Price2Vat { get; set; }
        public decimal? Price3Vat { get; set; }
        public decimal? OrgPrice1 { get; set; }
        public decimal? OrgPrice2 { get; set; }
        public decimal? OrgPrice3 { get; set; }
        public decimal? Discount1 { get; set; }
        public decimal? Discount2 { get; set; }
        public decimal? Discount3 { get; set; }
        public string? BranchName { get; set; }
    }
}
