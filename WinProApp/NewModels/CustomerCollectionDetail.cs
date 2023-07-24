using System;
using System.Collections.Generic;

namespace WinProApp.NewModels
{
    public partial class CustomerCollectionDetail
    {
        public long Id { get; set; }
        public long CollectionId { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentDesc { get; set; }
        public decimal? Amount { get; set; }
        public string? BranchName { get; set; }
    }
}
