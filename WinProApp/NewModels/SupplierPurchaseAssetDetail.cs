using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SupplierPurchaseAssetDetail
    {
        public long Id { get; set; }
        public long? PurchaseId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Vat { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ProductDate { get; set; }
    }
}
