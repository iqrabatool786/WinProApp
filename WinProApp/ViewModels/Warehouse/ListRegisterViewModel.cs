﻿namespace WinProApp.ViewModels.Warehouse
{
    public class ListRegisterViewModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string CashTill { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CratedDate { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime UpdatedDate { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
