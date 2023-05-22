using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class VwServerDb
    {
        public int Id { get; set; }
        public string? ServerName { get; set; }
        public string? UserId { get; set; }
        public string? ServerPwd { get; set; }
        public byte? Status { get; set; }
        public string? Dbname { get; set; }
    }
}
