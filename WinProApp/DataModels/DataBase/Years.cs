using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WinProApp.DataModels.DataBase
{
    public class Years
    {
        [Key]
        public int Id { get; set; }
        public int YearName { get; set; }
    }
}
