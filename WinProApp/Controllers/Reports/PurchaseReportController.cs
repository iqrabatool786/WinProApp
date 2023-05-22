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
//using System.Web.Mvc;

namespace WinProApp.Controllers.Reports
{
    [Authorize(Roles = "Administrator,Purchase")]
    public class PurchaseReportController : BasedUserController
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

        public PurchaseReportController(PurchaseService purchaseService, SupplierPurchaseService supplierPurchaseService, RequestForInfoService requestForInfoService, RequestForQuotationService requestForQuotationService, RequestForPurchaseService requestForPurchaseService, PurchaseOrderService purchaseOrderService, CommonService commonService, ProFormaInvoiceService proFormaInvoiceService, PurchaseRecieptService purchaseRecieptService, ShippingService shippingService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _purchaseService = purchaseService;
            _supplierPurchaseService= supplierPurchaseService;
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

        [Route("/Purchase/CreateReport/{type?}")]
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

        #region Supplier Summary

        [Route("/Purchase/CreateSupplierReport")]
        [HttpPost]
        public async Task<IActionResult> CreateSupplierReport(string type, decimal? amount, string colnames, string pagetype, string reportType)
        {
            var supplierInfo = await _purchaseService.GetAllSuppliersAsync();

            if(type == "equal")
            {
                supplierInfo = supplierInfo.Where(x=>x.Balance == amount).ToList();
            }

            if (type == "less")
            {
                supplierInfo = supplierInfo.Where(x => x.Balance <= amount).ToList();
            }

            if (type == "greter")
            {
                supplierInfo = supplierInfo.Where(x => x.Balance >= amount).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };

            string Title = "Supplier Summary";
            string subTitle = "Created Date: " + DateTime.Now.ToString("dd/MM/yyyy");

            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";
            if (reportType == "pdf")
            {
                string fileName = "Suppliers.pdf";

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
                //html += "<tr>";
                //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
                //html += "</tr>";
                html += "<tr>";
                html += "<td>";
                html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
                html += "<tr>";
                html += "<td style='text-align:left; width:60%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
                //html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
                //html += "<tr>";
                //html += "<td colspan='2' style='text-align:left; width:100%;'>Supplier Info</td>";
                //html += "</tr>";
                //html += "<tr>";
                //html += "<td style='text-align:left; width:30%;'>Name</td>";
                //html += "<td style='text-align:left; width:70%;'>" + supplierInfo.CompanyName + "</td>";
                //html += "</tr>";
                //html += "<tr>";
                //html += "<td style='text-align:left; width:30%;'>Address</td>";
                //html += "<td style='text-align:left; width:70%;'>" + supplierInfo.Address + "</td>";
                //html += "</tr>";
                //html += "<tr>";
                //html += "<td style='text-align:left; width:30%;'>Telephone</td>";
                //html += "<td style='text-align:left; width:70%;'>" + supplierInfo.PhoneNo + "</td>";
                //html += "</tr>";
                //html += "<tr>";
                //html += "<td style='text-align:left; width:30%;'>VAT No</td>";
                //html += "<td style='text-align:left; width:70%;'>" + supplierInfo.VatNo + "</td>";
                //html += "</tr>";
                //html += "</table>";
                //html += "</td>";
                //html += "<td style='text-align:right; width:40%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
                //html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
                //html += "<tr>";
                //html += "<td style='text-align:right; width:70%;'>Shipment No: <b>" + shippingInfo.ReferenceNo + "</b></td>";
                //html += "</tr>";
                //html += "<tr>";
                //html += "<td style='text-align:right; width:70%;'>Date: " + shippingInfo.Date.ToString("dd/MM/yyyy") + "</td>";
                //html += "</tr>";
                //html += "</table>";
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
                //  { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };
                int rowx = 0;
                decimal totalAmount = 0;
                int totQty = 0;
                foreach (var data in supplierInfo)
                {
                    rowx++;
                    html += "<tr style='page-break-inside: avoid;'>";
                    html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                    foreach (var item in strCols)
                    {
                        if (item == "Company Name" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.CompanyName + "</td>";
                        }

                        if (item == "BPcode" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Bpcode + "</td>";
                        }
                        if (item == "Address" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Address + "</td>";
                        }

                        if (item == "Phone" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.PhoneNo + "</td>";
                        }

                        if (item == "Fax" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Fax + "</td>";
                        }

                        if (item == "Email" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Email + "</td>";
                        }

                        if (item == "Website" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Website + "</td>";
                        }

                        if (item == "VatNo" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.VatNo + "</td>";
                        }

                        if (item == "Bank" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.BankName + "</td>";
                        }

                        if (item == "Account No" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.AccountNo + "</td>";
                        }

                        if (item == "CR Number" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + data.CRNumber + "</td>";
                        }

                        if (item == "Opening Balance" && fields.Contains(item))
                        {
                            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.OpeningBalance + "</td>";
                        }

                        if (item == "Balance" && fields.Contains(item))
                        {
                            totalAmount += (data.Balance ?? 0);
                            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Balance + "</td>";
                        }

                    }
                    html += "</tr>";
                }

                int colCount = fields.Length;

                html += "<td colspan='" + (colCount) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
                html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString() + "</td>";
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
            }
            else
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SupplierList");
                    var currentRow = 8;

                    #region Report Header

                    worksheet.Range("A1:T1").Merge();
                    worksheet.Range("A1:T1").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range("A1:T1").Style.Font.Bold = true;
                    worksheet.Range("A1:T1").Style.Font.FontSize = 20;

                    var headerCell = worksheet.Cell("A1");
                    headerCell.Value = Title;
                    headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    #endregion

                    #region Suppliers List

                    #region Header

                    int cols = 1;
                    worksheet.Row(currentRow).Style.Font.Bold = true;
                    foreach (var h in strCols)
                    {
                        if (fields.Contains(h))
                        {
                            worksheet.Cell(currentRow, cols).Value = h;
                            cols++;
                        }
                    }

                    #endregion

                    #region Body

                    int rowx = 0;
                    int colsData = 0;
                    decimal totalAmount = 0;
                    foreach (var data in supplierInfo)
                    {
                        rowx++;
                        colsData++;
                        currentRow++;
                        foreach (var item in strCols)
                        {
                            if (item == "Company Name" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.CompanyName;
                            }
                            else if (item == "BPcode" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.Bpcode;
                            }
                            else if (item == "Address" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.Address;
                            }
                            else if (item == "Phone" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.PhoneNo;
                            }
                            else if (item == "Fax" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.Fax;
                            }
                            else if (item == "Email" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.Email;
                            }
                            else if (item == "Website" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.Website;
                            }
                            else if (item == "VatNo" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.VatNo;
                            }
                            else if (item == "Bank" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.BankName;
                            }
                            else if (item == "Account No" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.AccountNo;
                            }
                            else if (item == "CR Number" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.CRNumber;
                            }
                            else if (item == "Opening Balance" && fields.Contains(item))
                            {
                                worksheet.Cell(currentRow, colsData).Value = data.OpeningBalance;
                            }
                            else if (item == "Balance" && fields.Contains(item))
                            {
                                totalAmount += (data.Balance ?? 0);
                                worksheet.Cell(currentRow, colsData).Value = data.Balance;
                            }
                        }
                    }
                    if (fields.Contains("Balance"))
                    {
                        //var one = currentRow;
                        //currentRow++;
                        //var newRow = worksheet.Range(currentRow + "1", currentRow + "" + (colsData - 1));
                        //newRow.Merge();
                        //newRow.Value = "Total";
                        //newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        //newRow.Style.Font.Bold = true;
                        //newRow.Style.Font.FontSize = 13;

                        //var befVatTotalCell = worksheet.Cell(currentRow, colsData);
                        //befVatTotalCell.Value = totalAmount;
                        //befVatTotalCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    #endregion

                    #endregion

                    string handle = Guid.NewGuid().ToString();
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        //var content = stream.ToArray();

                        //stream.Position = 0;
                        //TempData[handle] = stream.ToArray();

                        //var filex = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                        //string FilePath1 = webRootPath + "/Reports/";
                        //string excelFileName = "SupplierList.xlsx";
                        //FilePath1 = Path.Combine(FilePath1, excelFileName);

                        //using (var fileStream = System.IO.File.Create(FilePath1))
                        //{
                        //    await filex.FileStream.CopyToAsync(fileStream);
                        //}

                        //try
                        //{
                        //    return Json(excelFileName);
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex.InnerException;
                        //}
                        //return File(
                        //    content,
                        //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        //    "SupplierList.xlsx"
                        //    );

                        try
                        {
                            return new JsonResult(Convert.ToBase64String(stream.ToArray(), 0, stream.ToArray().Length), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }

                    }
                }
            }
        }

        [Route("/Purchase/DownloadExcel")]
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
        #endregion

        #region Invoice Report
        [Route("/Purchase/CreateInvoiceReport")]
        [HttpPost]
        public async Task<IActionResult> CreateInvoiceReport(int supplierId, DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var info = await _supplierPurchaseService.GetByAllAsync();

            if(supplierId > 0)
            {
                info = info.Where(x=>x.SupplierId == supplierId).ToList();
            }
           
            if(fromDt != null && toDt!=null)
            {
                DateTime dt =fromDt.Value;
                DateTime dt2=toDt.Value;

                info = info.Where(x=>x.Date >= dt && x.Date <= dt2).ToList();
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

            if(status == "yes")
            {
                info = info.Where(x => x.Status==true).ToList();
            }

            if (status == "no")
            {
                info = info.Where(x => x.Status == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "Company Name", "Invoice No", "Date", "Other Charges", "Discount", "Vat", "Total" };

            string Title = "Invoice Summary";
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

            string fileName = "PurchaseInvoice.pdf";

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
            decimal totalAmount = info.Select(x=>x.Total).Sum()??0;
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


            html += "<td colspan='" + (colCount-3) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
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

        #region Prchase Receive

        [Route("/Purchase/CreateReceiveReport")]
        [HttpPost]
        public async Task<IActionResult> CreateReceiveReport(int supplierId, DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var info = await _purchaseRecieptService.GetByAllAsync();

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

            string Title = "Purchase Receive Summary";
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

            string fileName = "PurchaseReceive.pdf";

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
                        string invoiceNo =_supplierPurchaseService.GetByIdAsync(data.InvoiceId).Result.InvoiceNo??"";
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + invoiceNo + "</td>";
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

        #region RFI
        [Route("/Purchase/CreateRFIReport")]
        [HttpPost]
        public async Task<IActionResult> CreateRFIReport(DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var Info = await _requestForInfoService.GetByAllAsync();

            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.ReqDate >= dt1 && x.ReqDate <= dt2).ToList();
            }

            if (fromDt != null && toDt == null)
            {
                DateTime dt1 = fromDt.Value;

                Info = Info.Where(x => x.ReqDate >= dt1).ToList();
            }

            if (fromDt == null && toDt != null)
            {
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.ReqDate <= dt2).ToList();
            }

            if(status == "yes")
            {
                Info = Info.Where(x => x.Approved == true).ToList();
            }

            if (status == "no")
            {
                Info = Info.Where(x => x.Approved == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "Requester", "Date", "Department", "Note", "Approved", "Approved Date", "Approved By" };

            string Title = "RFI Summary";
            string subTitle = "Created Date: " + DateTime.Now.ToString("dd/MM/yyyy");


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "RFI.pdf";

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
            //html += "<tr>";
            //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
            //html += "</tr>";
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
            //  { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };
            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in Info)
            {
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "Requester" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Requester + "</td>";
                    }

                    if (item == "Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ReqDate.ToString("yyyy-MM-dd") + "</td>";
                    }
                    if (item == "Department" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Department + "</td>";
                    }

                    if (item == "Note" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Note + "</td>";
                    }

                    if (item == "Approved" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.Approved==true?"Yes":"No") + "</td>";
                    }

                    if (item == "Approved Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.ApprovedDate !=null?data.ApprovedDate.Value.ToString("yyyy-MM-dd"):"") + "</td>";
                    }

                    if (item == "Approved By" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ApprovedBy + "</td>";
                    }                   

                }
                html += "</tr>";
            }

            int colCount = fields.Length;

            //html += "<td colspan='" + (colCount) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            //html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString() + "</td>";
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

        #region RFQ
        [Route("/Purchase/CreateRFQReport")]
        [HttpPost]
        public async Task<IActionResult> CreateRFQReport(DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var Info = await _requestForQuotationService.GetByAllAsync();

            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
            }

            if (fromDt != null && toDt == null)
            {
                DateTime dt1 = fromDt.Value;

                Info = Info.Where(x => x.Date >= dt1).ToList();
            }

            if (fromDt == null && toDt != null)
            {
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date <= dt2).ToList();
            }

            if (status == "yes")
            {
                Info = Info.Where(x => x.Approved == true).ToList();
            }

            if (status == "no")
            {
                Info = Info.Where(x => x.Approved == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "RFI ID", "Date", "Requester", "Department", "Note", "Approved", "Approved Date", "Approved By" };

            string Title = "RFQ Summary";
            string subTitle = "Created Date: " + DateTime.Now.ToString("dd/MM/yyyy");


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "RFQ.pdf";

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
            //html += "<tr>";
            //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
            //html += "</tr>";
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
            //  { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };
            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in Info)
            {
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "RFI ID" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Rfiid + "</td>";
                    }

                    if (item == "Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Date.ToString("yyyy-MM-dd") + "</td>";
                    }

                    if (item == "Requester" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Requester + "</td>";
                    }

                    if (item == "Department" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Department + "</td>";
                    }

                    if (item == "Note" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Note + "</td>";
                    }

                    if (item == "Approved" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.Approved == true ? "Yes" : "No") + "</td>";
                    }

                    if (item == "Approved Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.ApprovedDate != null ? data.ApprovedDate.Value.ToString("yyyy-MM-dd") : "") + "</td>";
                    }

                    if (item == "Approved By" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ApprovedBy + "</td>";
                    }

                }
                html += "</tr>";
            }

            int colCount = fields.Length;

            //html += "<td colspan='" + (colCount) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            //html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString() + "</td>";
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

        #region Requiest for purchase
        [Route("/Purchase/CreateRFPReport")]
        [HttpPost]
        public async Task<IActionResult> CreateRFPReport(DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var Info = await _requestForPurchaseService.GetByAllAsync();

            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
            }

            if (fromDt != null && toDt == null)
            {
                DateTime dt1 = fromDt.Value;

                Info = Info.Where(x => x.Date >= dt1).ToList();
            }

            if (fromDt == null && toDt != null)
            {
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date <= dt2).ToList();
            }

            if (status == "yes")
            {
                Info = Info.Where(x => x.Approved == true).ToList();
            }

            if (status == "no")
            {
                Info = Info.Where(x => x.Approved == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "RFQ ID", "Date", "Requester", "Department", "Note", "Approved", "Approved Date", "Approved By" };

            string Title = "Requiest for purchase Summary";
            string subTitle = "Created Date: " + DateTime.Now.ToString("dd/MM/yyyy");


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "RFP.pdf";

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
            //html += "<tr>";
            //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
            //html += "</tr>";
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
            //  { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };
            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in Info)
            {
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "RFQ ID" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Rfqid + "</td>";
                    }

                    if (item == "Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Date.ToString("yyyy-MM-dd") + "</td>";
                    }

                    if (item == "Requester" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Requester + "</td>";
                    }

                    if (item == "Department" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Department + "</td>";
                    }

                    if (item == "Note" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Note + "</td>";
                    }

                    if (item == "Approved" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.Approved == true ? "Yes" : "No") + "</td>";
                    }

                    if (item == "Approved Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.ApprovedDate != null ? data.ApprovedDate.Value.ToString("yyyy-MM-dd") : "") + "</td>";
                    }

                    if (item == "Approved By" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ApprovedBy + "</td>";
                    }

                }
                html += "</tr>";
            }

            int colCount = fields.Length;

            //html += "<td colspan='" + (colCount) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            //html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString() + "</td>";
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

        #region purchase Order
        [Route("/Purchase/CreatePOPReport")]
        [HttpPost]
        public async Task<IActionResult> CreatePOPReport(DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var Info = await _purchaseOrderService.GetByAllAsync();

            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
            }

            if (fromDt != null && toDt == null)
            {
                DateTime dt1 = fromDt.Value;

                Info = Info.Where(x => x.Date >= dt1).ToList();
            }

            if (fromDt == null && toDt != null)
            {
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date <= dt2).ToList();
            }

            if (status == "yes")
            {
                Info = Info.Where(x => x.Approved == true).ToList();
            }

            if (status == "no")
            {
                Info = Info.Where(x => x.Approved == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "PR ID", "Date", "Shipping Address", "Shipping Method", "Shipping Amount", "Delivery Date", "Remarks", "Approved", "Approved Date", "Approved By" };

            string Title = "Purchase Order Summary";
            string subTitle = "Created Date: " + DateTime.Now.ToString("dd/MM/yyyy");


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "PR.pdf";

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
            //html += "<tr>";
            //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
            //html += "</tr>";
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
            //  { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };
            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in Info)
            {
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "PR ID" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Prid + "</td>";
                    }

                    if (item == "Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Date.ToString("yyyy-MM-dd") + "</td>";
                    }

                    if (item == "Shipping Address" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ShippingAddress + "</td>";
                    }

                    if (item == "Shipping Method" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ShippingMethod + "</td>";
                    }

                    if (item == "Shipping Amount" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + (data.ShippingAmount !=null? data.ShippingAmount.Value.ToString("0.00"):"") + "</td>";
                    }
                    if (item == "Delivery Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.DeliveryDate !=null? data.DeliveryDate.Value.ToString("yyyy-MM-dd"):"") + "</td>";
                    }
                    if (item == "Remarks" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Remarks + "</td>";
                    }

                    if (item == "Approved" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.Approved == true ? "Yes" : "No") + "</td>";
                    }

                    if (item == "Approved Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.ApprovedDate != null ? data.ApprovedDate.Value.ToString("yyyy-MM-dd") : "") + "</td>";
                    }

                    if (item == "Approved By" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ApprovedBy + "</td>";
                    }

                }
                html += "</tr>";
            }

            int colCount = fields.Length;

            //html += "<td colspan='" + (colCount) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            //html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString() + "</td>";
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

        #region Shipment
        [Route("/Purchase/CreateShipmentReport")]
        [HttpPost]
        public async Task<IActionResult> CreateShipmentReport(string Type, int SupplierId, int StoreId, DateTime? fromDt, DateTime? toDt, string status, string colnames, string pagetype)
        {
            var Info = await _shippingService.GetByAllAsync();

            if(Type == "create")
            {
                Info = Info.Where(x => x.RecordType == "Create").ToList();
            }

            if (Type == "in")
            {
                Info = Info.Where(x => x.RecordType == "Receive").ToList();
            }

            if (SupplierId > 0)
            {
                Info = Info.Where(x => x.SupplierId == SupplierId).ToList();
            }

            if (StoreId > 0)
            {
                Info = Info.Where(x => x.ToStoreId == StoreId).ToList();
            }

            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
            }

            if (fromDt != null && toDt == null)
            {
                DateTime dt1 = fromDt.Value;

                Info = Info.Where(x => x.Date >= dt1).ToList();
            }

            if (fromDt == null && toDt != null)
            {
                DateTime dt2 = toDt.Value;

                Info = Info.Where(x => x.Date <= dt2).ToList();
            }

            if (status == "yes")
            {
                Info = Info.Where(x => x.Status == true).ToList();
            }

            if (status == "no")
            {
                Info = Info.Where(x => x.Status == false).ToList();
            }

            string[] fields = colnames.Split(',');
            string[] strCols = { "ID", "Reference No", "Date", "Charges Description", "Discount", "Other Charges", "Total", "Received", "Approved" };

            string Title = "Shipment Summary";
            string subTitle = "Created Date: " + DateTime.Now.ToString("dd/MM/yyyy");

            if (Type == "create")
            {
                Title = Title + " [Shipment Entry]";
            }

            if (Type == "in")
            {
                Title = Title + " [Shipment In]";
            }


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "shipment.pdf";

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
            //html += "<tr>";
            //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
            //html += "</tr>";
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
            //  { "Company Name", "BPcode", "Address", "Phone", "Fax", "Email", "Website", "VatNo", "Bank", "Account No", "CR Number", "Opening Balance", "Balance" };
            int rowx = 0;
            decimal totalDiscount = 0;
            decimal totalCharges = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in Info)
            {
                totalDiscount += data.Discount ?? 0;
                totalCharges += data.OtherCharges ?? 0;
                totalAmount += data.Total ?? 0;
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "ID" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Id.ToString() + "</td>";
                    }

                    if (item == "Reference No" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ReferenceNo+ "</td>";
                    }

                    if (item == "Date" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Date.ToString("yyyy-MM-dd") + "</td>";
                    }

                    if (item == "Charges Description" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.ChargesDescription + "</td>";
                    }

                    if (item == "Received" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.ReceivedStatus == true ? "Yes" : "No") + "</td>";
                    }

                    if (item == "Approved" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + (data.Status == true ? "Yes" : "No") + "</td>";
                    }

                    if (item == "Discount" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + (data.Discount != null?data.Discount.Value.ToString("0.00"):"") + "</td>";
                    }

                    if (item == "Other Charges" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + (data.OtherCharges != null ? data.OtherCharges.Value.ToString("0.00") : "") + "</td>";
                    }
                    if (item == "Total" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + (data.Total != null ? data.Total.Value.ToString("0.00") : "") + "</td>";
                    }
                                        

                }
                html += "</tr>";
            }

            int colCount = fields.Length;

            html += "<td colspan='" + (colCount - 2) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalDiscount.ToString() + "</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalCharges.ToString() + "</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totalAmount.ToString() + "</td>";
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
