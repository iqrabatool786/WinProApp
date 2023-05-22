namespace WinProApp.ViewModels.Warehouse.Categories
{
    public class ListViewModel
    {
        public int Id { get; set; }
        public string NameEng { get; set; }
        public string NameArabic { get; set; }
        public int ParentCategoryId { get; set; }
        public string? ParentCategoryNameEng { get; set; }
        public string? ParentCategoryNameArabic { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
