using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Merchant.FlatDiscount
{
    public class AddEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int StoreId { get; set; }
        public bool IsMembersOnly { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string DiscountType { get; set; }
        [Required]
        public decimal? DiscountAmount { get; set; }
        [Required]
        public decimal? StartAmount { get; set; }
        public bool Approved { get; set; }
       
    }
}
