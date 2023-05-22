using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Merchant.BuyGetQty
{
    public class AddEditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required]
        public long BuyProductId { get; set; }
        [Required]
        public string BuyBarcode { get; set; }
        [Required]
        public int BuyQty { get; set; }
        [Required]
        public long GetProductId { get; set; }
        [Required]
        public string GetProductBarcode { get; set; }
        [Required]
        public int GetQty { get; set; }

        public bool IsMembersOnly { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool Approved { get; set; }
    }
}
