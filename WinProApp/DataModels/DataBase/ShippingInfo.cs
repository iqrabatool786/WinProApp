using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class ShippingInfo
    {
        [Key]
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public int ToStoreId { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }
        public string? ChargesDescription { get; set; }
        public decimal? Total { get; set; }
        public string? RecordType { get; set; }
        public bool ReceivedStatus { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
