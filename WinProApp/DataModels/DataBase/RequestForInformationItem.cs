﻿using System;
using System.Collections.Generic;

namespace WinProApp.DataModels.DataBase
{
    public partial class RequestForInformationItem
    {
        public long Id { get; set; }
        public long Rfiid { get; set; }
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public string? Mfcompany { get; set; }
    }

    public partial class AddRequestForInformationItem
    {
        public string Description { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? Qty { get; set; }
        public string? Mfcompany { get; set; }
    }
}
