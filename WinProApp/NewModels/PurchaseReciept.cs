using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class PurchaseReciept
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long SupplierId { get; set; }
        public DateTime? Date { get; set; }
        public string? AttachedDoc { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal? Discount { get; set; }
        public decimal? OtherCharges { get; set; }
        public string? ChargesDescription { get; set; }
        public decimal? InvoiceVatAmount { get; set; }
        public decimal? InvoiceTotal { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? Total { get; set; }
        public int? StoreId { get; set; }
        public bool IsReturn { get; set; }
        public string? BranchName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
