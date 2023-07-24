using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblExpense
    {
        public decimal Pkid { get; set; }
        public double? BuildingNo { get; set; }
        public double Tantid { get; set; }
        public double? Accountid { get; set; }
        public double? Supplierid { get; set; }
        public decimal? AmountToPay { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Remaining { get; set; }
        public string? Desciprtion { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentArabic { get; set; }
        public string? Suppliername { get; set; }
        public string? BranchName { get; set; }
    }
}
