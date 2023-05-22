using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class GroupOfCompany
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? FunctionalStatus { get; set; }
    }
}
