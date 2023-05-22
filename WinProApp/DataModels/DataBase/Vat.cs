using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public partial class Vat
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal? Percentage { get; set; }
        public bool Status { get; set; }
        public DateTime? Vat_Date { get; set; }
    }
}
