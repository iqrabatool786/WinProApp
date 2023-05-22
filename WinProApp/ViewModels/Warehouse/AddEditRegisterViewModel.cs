using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Warehouse
{
    public class AddEditRegisterViewModel
    {
        public int Id { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required]
        public string CashTill { get; set; }
    }
}
