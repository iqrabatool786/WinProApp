using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class BankReconcile
    {
        public int Id { get; set; }
        public int? BankId { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
