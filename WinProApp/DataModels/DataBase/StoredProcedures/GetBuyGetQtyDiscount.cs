﻿namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class GetBuyGetQtyDiscount
    {
        public long Id { get; set; }
        public int StoreId { get; set; }
        public long BuyProductId { get; set; }
        public string BuyBarcode { get; set; }
        public int BuyQty { get; set; }
        public long GetProductId { get; set; }
        public string GetProductBarcode { get; set; }
        public int GetQty { get; set; }
        public bool IsMembersOnly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Approved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
