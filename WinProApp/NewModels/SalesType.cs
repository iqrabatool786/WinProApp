using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class SalesType
    {
        public int Id { get; set; }
        public string TypeCode { get; set; } = null!;
        public string TypeDescription { get; set; } = null!;
    }
}
