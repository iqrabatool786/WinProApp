using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public partial class RequestForQuotationItem
    {
        [Key]
        public long Id { get; set; }
        public long Rfqid { get; set; }
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public string? Mfcompany { get; set; }
    }
}
