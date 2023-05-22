using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Style
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameEng { get; set; } = null!;
        public string NameArabic { get; set; } = null!;
    }
}
