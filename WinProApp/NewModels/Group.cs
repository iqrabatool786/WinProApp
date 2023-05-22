using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Group
    {
        public int Id { get; set; }
        public string NameEng { get; set; } = null!;
        public string NameArabic { get; set; } = null!;
    }
}
