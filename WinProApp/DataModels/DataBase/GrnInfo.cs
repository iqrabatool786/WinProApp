using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class GrnInfo
    {
        [Key]
        public long Id { get; set; }
        public long IbtId { get; set; }
        public DateTime Date { get; set; }
        public int FromStoreId { get; set; }
        public int ToStoreId { get; set; }
        public string? Description { get; set; }
        public decimal? Total { get; set; }
        public bool Status { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTime? CratedDate { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
    }
}
