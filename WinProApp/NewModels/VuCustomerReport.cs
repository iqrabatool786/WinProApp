using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class VuCustomerReport
    {
        public long Id { get; set; }
        public string? Customerid { get; set; }
        public string? Name { get; set; }
        public string? Mobileno { get; set; }
        public string? Address { get; set; }
        public double? Balance { get; set; }
        public string? Phoneno { get; set; }
        public string? Email { get; set; }
        public string? Transp { get; set; }
        public string? Companyname { get; set; }
        public double? Creditlimit { get; set; }
        public double? Openingbalance { get; set; }
        public DateTime? Entrydate { get; set; }
        public string? Image { get; set; }
        public DateTime? Entrytime { get; set; }
        public decimal Iddd { get; set; }
        public byte[]? CustomerPicture { get; set; }
        public string? Ledgerbook { get; set; }
        public string? Pageno { get; set; }
        public string? City { get; set; }
        public string? Crno { get; set; }
        public string? Buildingno { get; set; }
        public string? Invpers { get; set; }
        public string? Projectcode { get; set; }
        public string? Logo { get; set; }
        public string? HeaderText { get; set; }
        public string? Vatnum { get; set; }
        public string? BranchName { get; set; }
    }
}
