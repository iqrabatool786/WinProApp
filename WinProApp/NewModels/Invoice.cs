using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Invoice
    {
        public decimal Id { get; set; }
        public decimal InvoiceNo { get; set; }
        public decimal InvoiceNoRegisterWise { get; set; }
        public string? Customerid { get; set; }
        public string? Userid { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Returnremarks { get; set; }
        public string? Status { get; set; }
        public string? InsertBy { get; set; }
        public DateTime? DateInsert { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? DateUpdate { get; set; }
        public double? Amountpaidcash { get; set; }
        public double? Amountpaidbank { get; set; }
        public string? BankcardNo { get; set; }
        public string? Backbranch { get; set; }
        public string? BranchName { get; set; }
        public double? Invoicediscount { get; set; }
        public string? Closed { get; set; }
        public double? ClosingBankAmount { get; set; }
        public double? ClosingCashAmount { get; set; }
        public string? Closedby { get; set; }
        public double? Dueamount { get; set; }
        public double? CashtillopeningBalance { get; set; }
        public int? OpeningBalanceRefrence { get; set; }
        public byte[]? Image { get; set; }
        public string? Registerid { get; set; }
        public int? Storeid { get; set; }
        public DateTime? Closingdate { get; set; }
        public DateTime? RepicationDate { get; set; }
        public string? Replicatedby { get; set; }
        public string? ReplicatedStatus { get; set; }
        public string? Transport { get; set; }
        public decimal? Vat { get; set; }
        public string? Companyname { get; set; }
        public string? Customername { get; set; }
        public byte[]? ImageI { get; set; }
        public decimal? Gt { get; set; }
        public string? Invoicemonth { get; set; }
        public string? Projectname { get; set; }
        public string? Pono { get; set; }
        public string? Saleinvoice { get; set; }
        public string? Projectcode { get; set; }
        public string? Pers { get; set; }
    }
}
