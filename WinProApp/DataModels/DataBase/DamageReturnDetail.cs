
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class DamageReturnDetail
    {
        [Key]
        public long Id { get; set; }
        public long? DamageReturnId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public decimal? QtyDozen { get; set; }
        public decimal? QtyPices { get; set; }
        public decimal? Price { get; set; }
      //  public decimal? Vat { get; set; }
    }
}
