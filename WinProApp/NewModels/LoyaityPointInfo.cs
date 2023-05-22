using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class LoyaityPointInfo
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long InvoiceId { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal? PointEarn { get; set; }
        public decimal? AddMoney { get; set; }
        public decimal? PointRadium { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
    }
}
