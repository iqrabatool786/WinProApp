﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class DiscountVouchares
    {
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsMembersOnly { get; set; }
        public bool Approved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
