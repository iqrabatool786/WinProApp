using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Merchant.FlatDiscount
{
    public class DetailViewModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Title { get; set; }
        public string IsMembersOnly { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedDiscount { get; set; }
        public decimal? StartAmount { get; set; }
        public string Approved { get; set; }
    }
   
}
