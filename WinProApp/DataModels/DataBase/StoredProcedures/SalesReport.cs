namespace WinProApp.DataModels.DataBase.StoredProcedures
{
    public class SalesReport
    {
        public int POS { get; set; }
        public string CompanyName { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int InvoiceNumber { get; set; }
        public double ItemPrice { get; set; }
        public int Qty { get; set; }
        public double SaleAmount { get; set; }
        public double Discount { get; set; }
        public double SalesAfterDiscount { get; set; }
        public double Tax { get; set; }
        public double Return { get; set; }
        public double Cash { get; set; }
        public double Bank { get; set; }
       
    }
}
