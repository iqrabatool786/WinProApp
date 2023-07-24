using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class QuotationMaster
    {
        public decimal Id { get; set; }
        public decimal InvoiceNo { get; set; }
        public string? Customerid { get; set; }
        public string? Userid { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? Status { get; set; }
        public string? InsertBy { get; set; }
        public decimal? Amountpaidcash { get; set; }
        public double? Amountpaidbank { get; set; }
        public double? Invoicediscount { get; set; }
        public string? Registerid { get; set; }
        public double? Dueamount { get; set; }
        public decimal InvoiceNoRegisterWise { get; set; }
        public int? Storeid { get; set; }
        public decimal? Vat { get; set; }
        public string? BranchName { get; set; }
        public string? Companyname { get; set; }
        public string? Customername { get; set; }
        public string? Transport { get; set; }
        public string? Backbranch { get; set; }
        public string? BankcardNo { get; set; }
    }
}
