using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class IbtDetails
    {
        [Key]
        public long Id { get; set; }
        public long IbtId { get; set; }
        public long ProductId { get; set; }
        public string Barcode { get; set; }
        public string? BoxNo { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Precentage { get; set; }
        public decimal? RetailPrice { get; set; }
    }
}
