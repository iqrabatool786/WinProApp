﻿using WinProApp.DataModels.DataBase;

namespace WinProApp.ViewModels.RequestForPurchase
{
    public class DetailsRequestForPurchaseViewModel
    {
        public long Id { get; set; }
        public long Rfqid { get; set; }
        public string Date { get; set; }
        public string RequireDate { get; set; }
        public string Requester { get; set; } = null!;
        public string? Department { get; set; }
        public string? Note { get; set; }
        public string Approved { get; set; }
        public string ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public string UpdatedDate { get; set; }

        public virtual List<RequestForPurchaseItem> Items { get; set; }
    }
}
