using System.ComponentModel.DataAnnotations;
namespace WinProApp.ViewModels.Warehouse
{
    public class AddEditStoreViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? StoreCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? GlAccount { get; set; }
        public string? Brand { get; set; }
        public string? Brandcode { get; set; }
        public string? StoreType { get; set; }
        public string? Office { get; set; }
        public string? City { get; set; }
        public string? VatNo { get; set; }
        public string? CRNo { get; set; }
    }
}
