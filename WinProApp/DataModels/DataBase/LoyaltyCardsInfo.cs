using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WinProApp.DataModels.DataBase
{
    public class LoyaltyCardsInfo
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public decimal? Balance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
