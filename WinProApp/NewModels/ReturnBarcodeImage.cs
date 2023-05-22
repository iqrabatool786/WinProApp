using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class ReturnBarcodeImage
    {
        public decimal? Returnid { get; set; }
        public byte[]? Image { get; set; }
        public byte[]? ImageI { get; set; }
    }
}
