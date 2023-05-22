using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Vat
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal? Percentage { get; set; }
        public bool Status { get; set; }
        public DateTime? VatDate { get; set; }
    }
}
