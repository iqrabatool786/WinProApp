using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinProApp.DataModels.DataBase
{
    public class LoyaltyPointSystem
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Points { get; set; }
    }
}
