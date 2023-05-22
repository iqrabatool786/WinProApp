using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class TblUserPermission
    {
        public decimal? UserId { get; set; }
        public decimal? FormDetailId { get; set; }
        public decimal PermissionId { get; set; }
        public bool? BVisible { get; set; }
    }
}
