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
using WinProApp.DataModels.DataBase.StoredProcedures;
using iText.Html2pdf.Attach.Impl.Layout;

namespace WinProApp.Controllers.Reports
{
    public class SalesReportController : Controller
    {

        public readonly SalesService _salesService;
        public readonly ReportHeadService _reportHeadService;
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

        public SalesReportController(SalesService salesService, ReportHeadService reportHeadService, PurchaseService purchaseService, SupplierPurchaseService supplierPurchaseService, RequestForInfoService requestForInfoService, RequestForQuotationService requestForQuotationService, RequestForPurchaseService requestForPurchaseService, PurchaseOrderService purchaseOrderService, CommonService commonService, ProFormaInvoiceService proFormaInvoiceService, PurchaseRecieptService purchaseRecieptService, ShippingService shippingService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _reportHeadService = reportHeadService;
            _salesService = salesService;
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
        public async Task<IActionResult> CreateSalesReport(int storeId, DateTime? fromDt, DateTime? toDt, string colnames, string pagetype)
        {
            try
            {


                var info = await _salesService.GetSalesReport(storeId, fromDt, toDt);

                if (info.Count <= 0)
                {
                    return Json(null);
                }
                JQueryDataTableParamModel param = new JQueryDataTableParamModel();
                param.sSearch = null;
                param.iDisplayStart = 0;
                param.iDisplayLength = 0;
                param.sSortDir_0 = null;
                var reportHeader = await _reportHeadService.GetList(param);
                GetReportHeads reportHead = new GetReportHeads();
                if (reportHeader != null)
                {
                    reportHead = reportHeader.Where(x => x.StoreId == 7).FirstOrDefault();
                }

                string[] fields = colnames.Split(',');
                string[] strCols = { "Company Name", "Invoice Date", "Invoice No.", "Item Price", "Qty", "Sale Amount", "Discount", "Sales After Discount", "Tax", "Return", "Cash", "Bank" };

                string Title = "Sales Summary Report";
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

                string fileName = "SalesSummaryReport.pdf";

                FilePath = Path.Combine(FilePath, fileName);

                if (System.IO.File.Exists(FilePath))
                    System.IO.File.Delete(FilePath);

                FileInfo newFile = new FileInfo(FilePath);

                var html = "<table style='width:100%; margin-top:0px; padding:0px;border-spacing:0;border-collapse: collapse;'>";
                html += "<tr>"; ;
                html += "<td style='text-align:center; width:100%;'><img src=" + reportHead.Logo + " /></td>";
                html += "<td style='text-align:center; width:100%;'><p style='font-size:13px; padding:0px; margin:0px;'>" + reportHead.Vatnum + "</p></td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td style='text-align:center; width:100%;'><h2 style='font-size:14px; padding:0px; margin:0px;'>" + reportHead.ReportHeaderEng + "</h2>";
                html += "</td>";
                html += "<td style='text-align:center; width:100%;'><h2 style='font-size:14px; padding:0px; margin:0px;'>" + reportHead.ReportHeaderArabic + "</h2>";
                html += "</td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td><p style='font-size:13px; padding:0px; margin:0px;'>Printed:" + DateTime.Now.Date.ToString("dd/MM/yyyy") + "</p>";
                html += "</td>";
                html += "<td><p style='font-size:13px; padding:0px; margin:0px;'>" + fromDt + "</p>";
                html += "</td>";
                html += "<td><p style='font-size:13px; padding:0px; margin:0px;'>" + toDt + "</p>";
                html += "</td>";
                html += "<td><p style='font-size:13px; padding:0px; margin:0px;'>Page 1 of 1</p>";
                html += "</td>";
                html += "</tr>";
                html += "</table>";
                html += "<table style='width:100%; font-size:14px;'>";
                html += "<tr>";
                foreach (var h in strCols)
                {
                    if (fields.Contains(h))
                    {
                        string hText = h;
                        html += "<th style='padding:3px; border:1px solid #454545;'>" + hText + "</th>";
                    }

                }
                html += "</tr>";
                html += "<tr>";
                html += "<th style='padding:3px; color:white; background-color:pink'>Store Number</th>";
                html += "</tr>";
                //      { "Company Name", "Invoice No", "Date", "Other Charges", "Discount", "Vat", "Total" };
                int rowx = 0;
                decimal totalPos = 1;
                int totalQty = info.Select(x => x.Qty).Sum();
                decimal totalSalesAmount = info.Select(x => x.SaleAmount).Sum() ?? 0;
                decimal totalDiscount = info.Select(x => x.Discount).Sum() ?? 0;
                decimal totalSaleAfter = info.Select(x => x.SalesAfterDiscount).Sum() ?? 0;
                decimal totalTax = info.Select(x => x.Tax).Sum() ?? 0;
                decimal totalItemPrice = info.Select(x => x.ItemPrice).Sum() ?? 0;
                decimal totalBank = info.Select(x => x.Bank).Sum() ?? 0;
                int totalReturn = info.Select(x => x.Return).Sum();
                decimal totalCash = info.Select(x => x.Cash).Sum() ?? 0;

                int totQty = 0;
                foreach (var data in info)
                {
                    rowx++;
                    html += "<tr style='page-break-inside: avoid;'>";
                    html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                    foreach (var item in strCols)
                    {


                        if (item == "Invoice Date" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.InvoiceDate.ToString("dd/MM/yyyy") + "</td>";
                        }
                        if (item == "Invoice No." && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.InvoiceNumber + "</td>";
                        }

                        if (item == "Item Price" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.ItemPrice + "</td>";
                        }

                        if (item == "Qty" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Qty + "</td>";
                        }

                        if (item == "Sale Amount" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.SaleAmount + "</td>";
                        }

                        if (item == "Discount" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Discount + "</td>";
                        }
                        if (item == "Sales After Discount" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.SalesAfterDiscount + "</td>";
                        }
                        if (item == "Tax" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Tax + "</td>";
                        }
                        if (item == "Return" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Return + "</td>";
                        }
                        if (item == "Cash" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Cash + "</td>";
                        }
                        if (item == "Bank" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;text-align:right;'>" + data.Bank + "</td>";
                        }

                    }
                    html += "</tr>";
                }


                html += "<tr>";
                html += "<td colspan='" + 3 + "' style='text-align:left; border:1px solid #454545;'>POS</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalItemPrice.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalQty.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalSalesAmount.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalDiscount.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalSaleAfter.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalTax.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalReturn.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalCash.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalBank.ToString("0.00") + "</td>";

                html += "</tr>";

                html += "<tr>";
                html += "<td colspan='" + 3 + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalItemPrice.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalQty.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalSalesAmount.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalDiscount.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalSaleAfter.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalTax.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalReturn.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalCash.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalBank.ToString("0.00") + "</td>";

                html += "</tr>";

                html += "<tr>";
                html += "<td colspan='" + 3 + "' style='text-align:left; border:1px solid #454545;'>Grand Total</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalItemPrice.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalQty.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalSalesAmount.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalDiscount.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalSaleAfter.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalTax.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalReturn.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalCash.ToString("0.00") + "</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalBank.ToString("0.00") + "</td>";

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

                using (var pdfWriter = new iText.Kernel.Pdf.PdfWriter("Output.pdf"))
                {
                    pdfWriter.SetCloseStream(false);
                    if (htmlDoc != null && pdfWriter != null)
                    {
                        using (var document = HtmlConverter.ConvertToDocument(html, pdfWriter))
                        {
                            byte[] byteInfo = workStream.ToArray();
                            workStream.Write(byteInfo, 0, byteInfo.Length);
                        }
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
            catch (Exception e)
            {

                throw;
            }
        }

        #endregion

    }
}
