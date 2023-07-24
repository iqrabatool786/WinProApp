using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class CustomerCollection
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public decimal? PreviouseBalance { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? RecieptNo { get; set; }
        public string? PaymentType { get; set; }
        public string? BankCheque { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? Status { get; set; }
        public bool? Closed { get; set; }
        public int? StoreId { get; set; }
        public decimal? Vat { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
