using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class SupplierPurchaseDetails
    {
        [Key]
        public long Id { get; set; }
        public long SupplierPurchaseId { get; set; }
        public long? ProductId { get; set; }
        public string? Barcode { get; set; }
        public int? CategoryId { get; set; }
        public int? SkuId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? UnitId { get; set; }
        public int? SeassonId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DescriptionId { get; set; }
        public int? BrandId { get; set; }
        public int? VendorId { get; set; }
        public int? GroupId { get; set; }
        public int? YearId { get; set; }
        public string? DescriptionEng { get; set; }
        public string? DescriptionArabic { get; set; }
        public decimal? QtyDozen { get; set; }
        public decimal? QtyPices { get; set; }
        public decimal? Price { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ProductDate { get; set; }
        public int? FlagPricePerPices { get; set; }
        public string? UnitBarcode { get; set; }
        public decimal? QtyPerUnit { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? UnitSalePrice { get; set; }
        public decimal? BoxRetail { get; set; }
        public string? UnitDescription { get; set; }
        public decimal? Vat { get; set; }
        public decimal? PVat { get; set; }
        
    }
}
