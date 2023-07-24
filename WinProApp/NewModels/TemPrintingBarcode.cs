using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TemPrintingBarcode
    {
        public string? Barcode { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }
        public string? Sku { get; set; }
        public string? Productdate { get; set; }
        public string? Expdate { get; set; }
        public string? Companyname { get; set; }
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
        public string? Field3 { get; set; }
        public string? Field4 { get; set; }
        public double? Vat { get; set; }
        public string? BranchName { get; set; }
        public byte[]? ImageI { get; set; }
    }
}
