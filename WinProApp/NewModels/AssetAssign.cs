using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class AssetAssign
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        public string? AssignTo { get; set; }
        public bool? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
