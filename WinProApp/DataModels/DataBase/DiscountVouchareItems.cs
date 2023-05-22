using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class DiscountVouchareItems
    {
        [Key]
        public int Id { get; set; }
        public int DiscountVoucherId { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal Amount { get; set; }
    }
}
