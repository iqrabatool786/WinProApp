using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WinProApp.DataModels.DataBase
{
    public class SalesTypes
    {
        [Key]
        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeDescription { get; set; }
    }
}
