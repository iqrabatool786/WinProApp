using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SelectedItemDiscountItem
    {
        public long Id { get; set; }
        public long SelectedItemDiscountId { get; set; }
        public long ProductId { get; set; }
        public string Barcode { get; set; } = null!;
    }
}
