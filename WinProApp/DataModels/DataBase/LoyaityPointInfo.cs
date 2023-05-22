using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class LoyaityPointInfo
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long InvoiceId { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? PointEarn { get; set; }
        public decimal? AddMoney { get; set; }
        public decimal? PointRadium { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
    }
}
