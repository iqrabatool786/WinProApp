using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class SelectedItemDiscountItems
    {
        [Key]
        public long Id { get; set; }
        public long SelectedItemDiscountId { get; set; }
        public long ProductId { get; set; }
        public string Barcode { get; set; }
        public int? DiscountPercentage { get; set; }
        public decimal? DiscountFixed { get; set; }
        public decimal? NewPrice { get; set; }
    }
}
