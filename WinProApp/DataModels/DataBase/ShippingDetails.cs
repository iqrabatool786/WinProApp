using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class ShippingDetails
    {
        [Key]
        public long Id { get; set; }
        public long ShippingId { get; set; }
        public long? ProductId { get; set; }
        public string Barcode { get; set; }
        public int? CategoryId { get; set; }
        public int? SkuId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? UnitId { get; set; }
        public int? SeassonId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BrandId { get; set; }
        public int? VendorId { get; set; }
        public int? GroupId { get; set; }
        public int? YearId { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? BoxNo { get; set; }
        public int? QtyPerBox { get; set; }
        public int? Qty { get; set; }
        public int? ReceivedQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? SaleVat { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string? ImageUrl { get; set; }
    }
}
