using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Systemmenu
    {
        public decimal? Id { get; set; }
        public string? Modulename { get; set; }
        public string? Displayname { get; set; }
        public string? Systemname { get; set; }
        public int? Entrybyuser { get; set; }
        public DateTime? Entrydate { get; set; }
    }
}
