using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WinProApp.DataModels.DataBase
{
    public class TransactionTypes
    {
        [Key]
        public int Id { get; set; }
        public string TrantypeID { get; set; }
        public string TrantypeDisplay { get; set; }
    }
}
