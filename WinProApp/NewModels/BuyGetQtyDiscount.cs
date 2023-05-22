using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class BuyGetQtyDiscount
    {
        public long Id { get; set; }
        public int StoreId { get; set; }
        public long BuyProductId { get; set; }
        public string BuyBarcode { get; set; } = null!;
        public int BuyQty { get; set; }
        public long GetProductId { get; set; }
        public string GetProductBarcode { get; set; } = null!;
        public int GetQty { get; set; }
        public bool IsMembersOnly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Approved { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }
    }
}
