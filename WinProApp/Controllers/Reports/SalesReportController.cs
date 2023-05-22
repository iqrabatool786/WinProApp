using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.ProFormaInvoice;
using WinProApp.ViewModels.Purchase;
using WinProApp.ViewModels.PurchaseOrder;
using WinProApp.ViewModels.RequestForInfo;
using WinProApp.ViewModels.RequestForPurchase;
using WinProApp.ViewModels.RequestForQuotation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using iText.Html2pdf;
using iText.StyledXmlParser.Jsoup;
using Org.BouncyCastle.Asn1.X9;
using ClosedXML.Excel;
using System.Reflection.Metadata;

namespace WinProApp.Controllers.Reports
{
    public class SalesReportController : Controller
    {
        public readonly PurchaseService _purchaseService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        public readonly RequestForInfoService _requestForInfoService;
        public readonly RequestForQuotationService _requestForQuotationService;
        public readonly RequestForPurchaseService _requestForPurchaseService;
        public readonly PurchaseOrderService _purchaseOrderService;
        public readonly CommonService _commonService;
        public readonly ProFormaInvoiceService _proFormaInvoiceService;
        public readonly PurchaseRecieptService _purchaseRecieptService;
        public readonly ShippingService _shippingService;
        public readonly StoreService _storeService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SalesReportController(PurchaseService purchaseService, SupplierPurchaseService supplierPurchaseService, RequestForInfoService requestForInfoService, RequestForQuotationService requestForQuotationService, RequestForPurchaseService requestForPurchaseService, PurchaseOrderService purchaseOrderService, CommonService commonService, ProFormaInvoiceService proFormaInvoiceService, PurchaseRecieptService purchaseRecieptService, ShippingService shippingService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _purchaseService = purchaseService;
            _supplierPurchaseService = supplierPurchaseService;
            _requestForInfoService = requestForInfoService;
            _requestForQuotationService = requestForQuotationService;
            _requestForPurchaseService = requestForPurchaseService;
            _purchaseOrderService = purchaseOrderService;
            _commonService = commonService;
            _proFormaInvoiceService = proFormaInvoiceService;
            _purchaseRecieptService = purchaseRecieptService;
            _shippingService = shippingService;
            _storeService = storeService;
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("/Sales/CreateReport/{type?}")]
        [HttpGet]
        public async Task<IActionResult> Index(string type)
        {
            ViewBag.Type = type;
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var stores = await _storeService.GetByAllAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.Stores = new SelectList(stores, "Id", "Name");

            return View();
        }

        #region Sales Report
        [Route("/Sales/CreateSalesReport")]
        [HttpPost]
        public async Task<IActionResult> CreateSalesReport(int supplierId, DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var info = await _supplierPurchaseService.GetByAllAsync();

            if (supplierId > 0)
            {
                info = info.Where(x => x.SupplierId == supplierId).ToList();
            }

            if (fromDt != null && toDt != null)
            {
                DateTime dt = fromDt.Value;
                DateTime dt2 = toDt.Value;

                info = info.Where(x => x.Date >= dt && x.Date <= dt2).ToList();
            }

            if (fromDt != null && toDt == null)
            {
                DateTime dt = fromDt.Value;

                info = info.Where(x => x.Date >= dt).ToList();
            }

            if (fromDt == null && toDt != null)
            {
                DateTime dt2 = toDt.Value;

                info = info.Where(x => x.Date <= dt2).ToList();
            }

            if (status == "yes")
            {
                info = info.Where(x => x.Status == true).ToList();
            }

            if (status == "no")
            {
                info = info.Where(x => x.Status == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "Company Name", "Invoice No", "Date", "Other Charges", "Discount", "Vat", "Total" };

            string Title = "Sales Report";
            string subTitle = "Date: " + DateTime.Now.ToString("dd/MM/yyyy");

            if (fromDt != null && toDt != null)
            {
                subTitle = "From Date: " + fromDt.Value.ToString("dd/MM/yyyy") + " To Date: " + toDt.Value.ToString("dd/MM/yyyy");
            }

            if (fromDt != null && toDt == null)
            {
                subTitle = "From Date: " + fromDt.Value.ToString("dd/MM/yyyy");
            }

            if (fromDt == null && toDt != null)
            {
                subTitle = "To Date: " + toDt.Value.ToString("dd/MM/yyyy");
            }


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "SalesReport.pdf";

            FilePath = Path.Combine(FilePath, fileName);

            if (System.IO.File.Exists(FilePath))
                System.IO.File.Delete(FilePath);

            FileInfo newFile = new FileInfo(FilePath);

            var html = "<table style='width:100%; margin-top:0px; padding:0px;border-spacing:0;border-collapse: collapse; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:center; width:80%; border-bottom:1px solid #454545;'><h2 style='font-size:28px; font-weight:bold; padding:0px; margin:0px;'>" + Title + "</h2></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px;'>" + subTitle + "</p></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:left; width:60%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='padding-top:30px;'>";
            html += "<table style='width:100%; font-family: Helvetica Neue, Helvetica, Arial, sans-serif; font-size:14px;'>";
            html += "<tr>";
            html += "<th style='padding:3px; width:5%; border:1px solid #454545;'>No</th>";
            foreach (var h in strCols)
            {
                if (fields.Contains(h))
                {
                    string hText = h;
                    html += "<th style='padding:3px; border:1px solid #454545;'>" + hText + "</th>";
                }

            }
            html += "</tr>";
            //      { "Company Name", "Invoice No", "Date", "Other Charges", "Discount", "Vat", "Total" };
            int rowx = 0;
            decimal totalAmount = info.Select(x => x.Total).Sum() ?? 0;
            decimal totalOther = info.Select(x => x.OtherCharges).Sum() ?? 0;
            decimal totalDiscount = info.Select(x => x.Discount).Sum() ?? 0;
            decimal totalVat = info.Select(x => x.VatAmount).Sum() ?? 0;
            int totQty = 0;
            foreach (var data in info)
            {
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "Company Name" && fields.Contains(item))
                    {
                        string companyName = _purchaseService.GetSupplierByIdAsync(data.SupplierId).Result.CompanyName;
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + companyName + "</td>";
                    }

                    if (item == "Invoice No" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.InvoiceNo + "</td>";
                    }
                    if (item == "Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Date.ToString("dd/MM/yyyy") + "</td>";
                    }

                    if (item == "Other Charges" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.OtherCharges + "</td>";
                    }

                    if (item == "Discount" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Discount + "</td>";
                    }

                    if (item == "Vat" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.VatAmount + "</td>";
                    }

                    if (item == "Total" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Total + "</td>";
                    }

                }
                html += "</tr>";
            }

            int colCount = fields.Length;


            html += "<td colspan='" + (colCount - 3) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalOther.ToString("0.00") + "</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalDiscount.ToString("0.00") + "</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalVat.ToString("0.00") + "</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString("0.00") + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "</table>";

            //  var html = "<h1>Hello World</h1>";

            var workStream = new MemoryStream();
            //   Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);

            //  HtmlString str = new HtmlString(html);
            iText.StyledXmlParser.Jsoup.Nodes.Document htmlDoc = Jsoup.Parse(html);
            if (pagetype == "Landscape")
            {
                htmlDoc.Head().Append("<style>" +
                        "@page { size: landscape; } "
                        + "</style>");
            }

            using (var pdfWriter = new iText.Kernel.Pdf.PdfWriter(workStream))
            {
                pdfWriter.SetCloseStream(false);
                using (var document = HtmlConverter.ConvertToDocument(htmlDoc.OuterHtml(), pdfWriter))
                {
                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                }
            }




            workStream.Position = 0;

            var filex = new FileStreamResult(workStream, "application/pdf");

            String FilePath1 = webRootPath + "/Reports/";
            FilePath1 = Path.Combine(FilePath1, fileName);

            using (var fileStream = System.IO.File.Create(FilePath1))
            {
                await filex.FileStream.CopyToAsync(fileStream);
            }

            try
            {
                return Json(fileName);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }


            return View();
        }

        #endregion

    }
}
