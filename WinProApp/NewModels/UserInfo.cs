using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class UserInfo
    {
        public int Userid { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Displayname { get; set; }
        public string? Usertype { get; set; }
        public string? Alloweddiscount { get; set; }
        public string? Allowedreturn { get; set; }
        public string? Allowedinvoiceedit { get; set; }
        public string? AllowedGiftInvoice { get; set; }
        public string? Allowsupplierdelete { get; set; }
        public string? Status { get; set; }
        public string? Usercolor { get; set; }
        public bool? Finance { get; set; }
        public bool? Purchase { get; set; }
        public bool? Warehouse { get; set; }
        public bool? Pos { get; set; }
        public bool? Backoffice { get; set; }
        public bool? Hrm { get; set; }
        public bool? PassSave { get; set; }
        public string? Loyltycard { get; set; }
        public string? Donation { get; set; }
        public string? Voucher { get; set; }
    }
}
