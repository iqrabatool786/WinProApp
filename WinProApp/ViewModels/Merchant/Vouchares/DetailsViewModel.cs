namespace WinProApp.ViewModels.Merchant.Vouchares
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string IsMembersOnly { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Approved { get; set; }
    }
}
