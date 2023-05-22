using System.ComponentModel.DataAnnotations;

namespace WinProApp.ViewModels.Warehouse.Styles
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameEng { get; set; }
        public string NameArabic { get; set; }
    }
}
