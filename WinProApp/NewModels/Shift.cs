using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class Shift
    {
        public long Id { get; set; }
        public int StoreId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime OpenAt { get; set; }
        public DateTime? CloseAt { get; set; }
        public bool Closed { get; set; }
    }
}
