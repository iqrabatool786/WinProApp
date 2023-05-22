using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class SupplierPurchaseAssetDetails
    {
        [Key]
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        public long? ProductId { get; set; }
        public string Barcode { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Vat { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ProductDate { get; set; }       
    }
}
