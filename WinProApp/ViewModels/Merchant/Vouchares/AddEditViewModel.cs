using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Merchant.Vouchares
{
    public class AddEditViewModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required]
        public decimal MinAmount { get; set; }
        [Required]
        public decimal MaxAmount { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public bool IsMembersOnly { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool Approved { get; set; }
    }
}
