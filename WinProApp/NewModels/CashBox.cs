using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class CashBox
    {
        public long Id { get; set; }
        public string CurrentIp { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public decimal? OpeningBalance { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Bank { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Voucher { get; set; }
        public decimal? RadiumPoint { get; set; }
        public decimal? TaxTotal { get; set; }
        public decimal? TaxCash { get; set; }
        public decimal? TaxBank { get; set; }
        public decimal? CloseOpeningBalance { get; set; }
        public decimal? CloseCash { get; set; }
        public decimal? CloseBank { get; set; }
        public decimal? CloseCredit { get; set; }
        public string? BranchName { get; set; }
        public DateTime OpenAt { get; set; }
        public DateTime? CloseAt { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
