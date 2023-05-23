namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class SalesReport
    {
        public decimal? POS { get; set; }
        public string CompanyName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long InvoiceNumber { get; set; }
        public decimal? ItemPrice { get; set; }
        public int Qty { get; set; }
        public decimal? SaleAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SalesAfterDiscount { get; set; }
        public decimal? Tax { get; set; }
        public int Return { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Bank { get; set; }
       
    }
}
