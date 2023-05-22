namespace WinProApp.ViewModels.SupplierPurchase
{
    public class BarcodeViewModel
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
    }


    public class BpCodeViewModel
    {
        public long SupplierId { get; set; }
        public string Code { get; set; }
    }

    public class StyleSkuViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
    }

    public class YearsViewModel
    {
        public int Id { get; set; }
        public int YearName { get; set; }
    }

    public class AutocompleteViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
