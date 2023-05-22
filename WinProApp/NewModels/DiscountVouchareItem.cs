using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class DiscountVouchareItem
    {
        public int Id { get; set; }
        public int DiscountVoucherId { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal Amount { get; set; }
    }
}
