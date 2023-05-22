using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class BankPaymentReciept
    {
        public long Id { get; set; }
        public int? BankId { get; set; }
        public string? ReferenceName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? Purpose { get; set; }
        public string? TransactionType { get; set; }
        public string? CompanyName { get; set; }
        public string? StoreId { get; set; }
        public string? Storename { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
