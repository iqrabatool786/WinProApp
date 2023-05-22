using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.Merchant.SelectedItem
{
    public class AddEditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required]
        public bool IsMembersOnly { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string DiscountType { get; set; }
      
        public decimal? DiscountAmount { get; set; }
        public decimal? StartAmount { get; set; }
        public bool Approved { get; set; }
    }
}
