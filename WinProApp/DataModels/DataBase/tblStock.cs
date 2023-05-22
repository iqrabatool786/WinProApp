using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class tblStock
    {
        [Key]
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        public decimal? IbtInQty { get; set; }
        public decimal? IbtOutQty { get; set; }
        public decimal? SaleInQty { get; set; }
        public decimal? SaleOutQty { get; set; }
        public decimal? PurInQty { get; set; }
        public decimal? PurOutQty { get; set; }
        public decimal? DamageQty { get; set; }
        public string? SalesType { get; set; }
        public DateTime? Date { get; set; }
        public string? BarCode { get; set; }
        public decimal? ShipInQty { get; set; }
        public decimal? ShipOutQty { get; set; }
        public decimal? ShortInQty { get; set; }
        public int? StoreID { get; set; }
    }
}
