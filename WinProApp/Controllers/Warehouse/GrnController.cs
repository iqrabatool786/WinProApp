using iText.Html2pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.Areas.Identity.Data;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.GRN;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Controllers.Warehouse
{
    public class GrnController : BasedUserController
    {
        public readonly GrnInfoService _grnInfoService;
        public readonly IbtInfoService _ibtInfoService;
        public readonly CommonService _commonService;
        public readonly StoreService _storeService;
        public readonly UserManager<WinProAppUser> _userManager;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public GrnController(GrnInfoService grnInfoService, IbtInfoService ibtInfoService, CommonService commonService, StoreService storeService, UserManager<WinProAppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _grnInfoService = grnInfoService;
            _ibtInfoService = ibtInfoService;
            _commonService = commonService;
            _storeService = storeService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/GRN")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/GRNList")]
        [HttpPost]
        public async Task<IActionResult> GRNList(JQueryDataTableParamModel param)
        {

            var results = _grnInfoService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                IbtId= x.IbtId,
                Date = x.Date,
                StrDate = x.Date.ToString("yyyy-MM-dd"),
                FromStoreId = x.FromStoreId,
                FromStoreName = _storeService.GetNameByIdAsync(x.FromStoreId).Result,
                ToStoreId = x.ToStoreId,
                ToStoreName = _storeService.GetNameByIdAsync(x.ToStoreId).Result,
                Description = x.Description,
                Status = x.Status == true ? "Approved" : "OnHold",
                Total = x.Total.Value > 0 ? x.Total.Value.ToString("0.00") : "",
                TotalRecordCount = x.TotalRecordCount,
            }).ToList();

            int TotalRecordCount = 0;

            if (results.Count > 0)
                TotalRecordCount = results[0].TotalRecordCount;

            return Json(data: new
            {
                param.sEcho,
                iTotalRecords = TotalRecordCount,
                iTotalDisplayRecords = TotalRecordCount,
                aaData = results
            });
        }

        [Route("/Warehouse/CreateGRN")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int curretStoreId = _userManager.Users.FirstOrDefault(x=>x.UserName == User.Identity.Name).StoreId;// int.Parse(HttpContext.Session.GetString("currentStore").ToString());

            var ibts = await _grnInfoService.GetStoreIbtByStoreIdsync(curretStoreId);
            var vat = await _commonService.GetVatAsync();

            ViewBag.IBTs = new SelectList(ibts, "Id", "Id");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View();
        }


        [Route("/Warehouse/CreateGRN")]
        [HttpPost]
        public async Task<IActionResult> Create(AddViewModel model)
        {
            try
            {

                var GrnInfo = new GrnInfo();
                GrnInfo.IbtId= model.IbtId;
                GrnInfo.Date = model.Date;
                GrnInfo.FromStoreId = model.FromStoreId;
                GrnInfo.ToStoreId = model.ToStoreId;
                GrnInfo.Description = model.Description;
                GrnInfo.Status = model.Status;
                GrnInfo.Total = model.Total;
                GrnInfo.CratedDate = DateTime.Now;
                GrnInfo.CreatedBy = User.Identity.Name;
                GrnInfo.UpdatedDate = DateTime.Now;
                GrnInfo.UpdatedBy = User.Identity.Name;

                var data = await _grnInfoService.CreateAsync(GrnInfo);

                List<GrnDetails> items = new List<GrnDetails>();

                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] boxnos = Request.Form["ItemBoxNo"];
                string[] qtys = Request.Form["ItemQty"];
                string[] rqtys = Request.Form["ItemRQty"];
                string[] prices = Request.Form["itemPrice"];
                string[] precentx = Request.Form["itemPrecent"];
                string[] rprices = Request.Form["itemRetail"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long productId = 0;
                    int? qty = null;
                    int? rqty = null;
                    decimal? price = null;
                    decimal? pricer = null;
                    decimal? precent = null;


                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = long.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = int.Parse(qtys[i]);
                    }

                    if (!string.IsNullOrEmpty(rqtys[i]))
                    {
                        rqty = int.Parse(rqtys[i]);
                    }

                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    if (!string.IsNullOrEmpty(precentx[i]))
                    {
                        precent = decimal.Parse(precentx[i]);
                    }

                    if (!string.IsNullOrEmpty(rprices[i]))
                    {
                        pricer = decimal.Parse(rprices[i]);
                    }


                    items.Add(new GrnDetails()
                    {
                        GrnId = data.Id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        BoxNo = boxnos[i],
                        Qty = qty,
                        ReceivedQty=rqty,
                        Price = price,
                        Precentage = precent,
                        RetailPrice = pricer,
                    });


                }

                await _grnInfoService.CreateItemsAsync(data.Id, items);
                return new JsonResult(new { id = data.Id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }


        [Route("/Warehouse/GrnDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var info = await _grnInfoService.GetByIdAsync(id);
            var GrnInfo = new DetailsViewModel();
            GrnInfo.Id = info.Id;
            GrnInfo.IbtId= info.IbtId;
            GrnInfo.Date = info.Date;
            GrnInfo.FromStoreId = info.FromStoreId;
            GrnInfo.ToStoreId = info.ToStoreId;
            GrnInfo.Description = info.Description;
            GrnInfo.Status = info.Status;
            GrnInfo.Total = info.Total;


         ///   int curretStoreId = int.Parse(HttpContext.Session.GetString("currentStore").ToString());

            var vat = await _commonService.GetVatAsync();
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View(GrnInfo);
        }


        [Route("/Warehouse/EditGRN/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var info = await _grnInfoService.GetByIdAsync(id);
            var GrnInfo = new EditViewModel();
            GrnInfo.Id = info.Id;
            GrnInfo.IbtId = info.IbtId;
            GrnInfo.Date = info.Date;
            GrnInfo.FromStoreId = info.FromStoreId;
            GrnInfo.ToStoreId = info.ToStoreId;
            GrnInfo.Description = info.Description;
            GrnInfo.Status = info.Status;
            GrnInfo.Total = info.Total;


          //  int curretStoreId =  int.Parse(HttpContext.Session.GetString("currentStore").ToString()); //await _commonService.GetCurrentStoreId();

            var vat = await _commonService.GetVatAsync();

            ViewBag.VatPercentage = vat.Percentage ?? 0;


            return View(GrnInfo);
        }



        [Route("/Warehouse/EditGRN")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            long id = model.Id;
            try
            {

                var GrnInfo = await _grnInfoService.GetByIdAsync(id);
                GrnInfo.Id = model.Id;
                GrnInfo.IbtId = model.IbtId;
                GrnInfo.Date = model.Date;
                GrnInfo.FromStoreId = model.FromStoreId;
                GrnInfo.ToStoreId = model.ToStoreId;
                GrnInfo.Description = model.Description;
                GrnInfo.Status = model.Status;
                GrnInfo.Total = model.Total;
                GrnInfo.UpdatedDate = DateTime.Now;
                GrnInfo.UpdatedBy = User.Identity.Name;



                List<GrnDetails> items = new List<GrnDetails>();

                string[] Ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] boxnos = Request.Form["ItemBoxNo"];
                string[] qtys = Request.Form["ItemQty"];
                string[] rqtys = Request.Form["ItemRQty"];
                string[] prices = Request.Form["itemPrice"];
                string[] precentx = Request.Form["itemPrecent"];
                string[] rprices = Request.Form["itemRetail"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long itemId = 0;
                    long productId = 0;
                    int? qty = null;
                    int? rqty = null;
                    decimal? price = null;
                    decimal? pricer = null;
                    decimal? precent = null;

                    itemId = long.Parse(Ids[i]);

                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = long.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = int.Parse(qtys[i]);
                    }

                    if (!string.IsNullOrEmpty(rqtys[i]))
                    {
                        rqty = int.Parse(rqtys[i]);
                    }

                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    if (!string.IsNullOrEmpty(precentx[i]))
                    {
                        precent = decimal.Parse(precentx[i]);
                    }

                    if (!string.IsNullOrEmpty(rprices[i]))
                    {
                        pricer = decimal.Parse(rprices[i]);
                    }

                    items.Add(new GrnDetails()
                    {
                        Id = itemId,
                        GrnId = id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        BoxNo = boxnos[i],
                        Qty = qty,
                        ReceivedQty=rqty,
                        Price = price,
                        Precentage = precent,
                        RetailPrice = pricer,
                    });

                }

                await _grnInfoService.UpdateAsync(GrnInfo, items);
                return new JsonResult(new { id = id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }


        [Route("/Warehouse/GetGrnItems/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetGrnItems(long id)
        {
            List<ItemDetailsViewModel> Items = new List<ItemDetailsViewModel>();

            var rItems = await _grnInfoService.GetItemsByIbtIdAsync(id);

            foreach (var item in rItems)
            {
                Items.Add(new ItemDetailsViewModel
                {
                    Id = item.Id,
                    GrnId = item.GrnId,
                    ProductId = item.ProductId,
                    Barcode = item.Barcode,
                    DescriptionEnglish = _commonService.GetProductInfoByIdAsync(item.ProductId).Result.ProductNameEng,
                    DescriptionArabic = _commonService.GetProductInfoByIdAsync(item.ProductId).Result.ProductNameArabic,
                    BoxNo = item.BoxNo,
                    Qty = item.Qty,
                    ReceivedQty= item.ReceivedQty,
                    Price = item.Price,
                    Precentage = item.Precentage,
                    RetailPrice = item.RetailPrice,
                });
            }

            return Json(data: Items);
        }

        [Route("/Warehouse/GetIBTInfoById")]
        [HttpPost]
        public async Task<IActionResult> GetIBTInfoById(long id)
        {
           
            var info = await _ibtInfoService.GetByIdAsync(id);
            var model = new IbtInfoDetailsViewModel();
            model.Id= id;
            model.Total= info.Total;
            model.Description = info.Description;
            model.FromStoreId= info.FromStoreId;
            model.ToStoreId= info.ToStoreId;

            return Json(data: model);
        }


        //[Route("/Warehouse/GetGrnInfoById")]
        //[HttpPost]
        //public async Task<IActionResult> GetGrnInfoById(long id)
        //{

        //    var info = await _ibtInfoService.GetByIdAsync(id);
        //    var model = new DetailsViewModel();
        //    model.Id = id;
        //    model.Total = info.Total;
        //    model.Description = info.Description;
        //    model.FromStoreId = info.FromStoreId;
        //    model.ToStoreId = info.ToStoreId;

        //    return Json(data: model);
        //}

        [Route("/Warehouse/DeleteGrnItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteGrnItem(long id)
        {
            try
            {
                await _grnInfoService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/DeleteGrn/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteGrn(long id)
        {
            try
            {
                var result = await _grnInfoService.GetByIdAsync(id);
                await _grnInfoService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/GrnReportCreate/{id}")]
        [HttpPost]
        public async Task<IActionResult> GrnReportCreate(long id)
        {
            var grnInfo = await _grnInfoService.GetByIdAsync(id);
            var storeInfo1 = await _storeService.GetByIdAsync(grnInfo.FromStoreId);
            var storeInfo2 = await _storeService.GetByIdAsync(grnInfo.ToStoreId);
            var grnItems = await _grnInfoService.GetItemsByIbtIdAsync(id);



            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "GRNReport.pdf";

            FilePath = Path.Combine(FilePath, fileName);

            if (System.IO.File.Exists(FilePath))
                System.IO.File.Delete(FilePath);

            FileInfo newFile = new FileInfo(FilePath);


            var html = "<table style='width:100%; margin-top:0px; padding:0px;border-spacing:0;border-collapse: collapse; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:center; width:80%; border-bottom:1px solid #454545;'>GRN (Good Receive Note)</td>";
            html += "</tr>";
            //html += "<tr>";
            //html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px;'>Date:" + ibtInfo.Date.ToString("dd/MM/yyyy") + "</p></td>";
            //html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'></p></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:center; width:80%;'>";
            html += "<table style='width:100%;'>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>GRN No: " + grnInfo.Id.ToString() + "</td>";
            html += "<td style='text-align:left; width:30%;'>IBT No: " + grnInfo.IbtId.ToString() + "</td>";
            html += "<td style='text-align:right; width:40%;'>Date: " + grnInfo.Date.ToString("dd/MM/yyyy") + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:left; width:50%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td colspan='2' style='text-align:left; width:100%;'>From Store Info</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Name</td>";
            html += "<td style='text-align:left; width:70%;'>" + storeInfo1.Name + "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Address</td>";
            html += "<td style='text-align:left; width:70%;'>" + storeInfo1.Address + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "<td style='text-align:right; width:50%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td colspan='2' style='text-align:left; width:100%;'>To Store Info</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Name</td>";
            html += "<td style='text-align:left; width:70%;'>" + storeInfo2.Name + "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Address</td>";
            html += "<td style='text-align:left; width:70%;'>" + storeInfo2.Address + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='padding-top:20px;'>Note: " + grnInfo.Description + "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='padding-top:30px;'>";
            html += "<table style='width:100%; font-family: Helvetica Neue, Helvetica, Arial, sans-serif; font-size:14px;'>";
            html += "<tr>";
            html += "<th style='padding:3px; width:5%; border:1px solid #454545;'>No</th>";
            //html += "<th style='padding:3px; border:1px solid #454545;'>Barcode</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Description</th>";
            //html += "<th style='padding:3px; border:1px solid #454545;'>Box No</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Qty</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Received Qty</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Price</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Retail Price</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Total</th>";
            html += "</tr>";

            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            int totRQty = 0;
            foreach (var data in grnItems)
            {
                decimal tot = 0;
                string strTot = "";
                tot = (data.Qty ?? 0) * (data.Price ?? 0);
                strTot = tot.ToString("0.00");
                totQty += (data.Qty ?? 0);
                totRQty += (data.ReceivedQty ?? 0);

                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                //html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Barcode + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545;'>" + _commonService.GetProductInfoByIdAsync(data.ProductId).Result.ProductNameEng + "</td>";
                //html += "<td style='padding:3px; border:1px solid #454545;'>" + data.BoxNo + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Qty + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.ReceivedQty + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Price + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.RetailPrice + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + strTot + "</td>";
                html += "</tr>";
            }

            html += "<tr style='page-break-inside: avoid;'>";
            html += "<td colspan='2' style='padding:3px; border:1px solid #454545;'>TOTAL</td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + totQty.ToString() + "</td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + totRQty.ToString() + "</td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'></td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'></td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + grnInfo.Total.Value.ToString("0.00") + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "<tr style='page-break-inside: avoid;'>";
            html += "<td style='text-align:left; width:100%;'>";
            //html += "<table>";
            //html += "<tr>";
            //html += "<td style='padding:60px 5px 20px 5px; text-align:center; width:33%;'>----------------------------------------</br>Store Manager Signature</td>";
            //html += "<td style='padding:60px 5px 20px 5px; text-align:center; width:33%;'>----------------------------------------</br>Transport Signature</td>";
            //html += "<td style='padding:60px 5px 20px 5px; text-align:center; width:33%;'>----------------------------------------</br>Receive Store Signature</td>";
            //html += "</tr>";
            //html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "</table>";

            //  var html = "<h1>Hello World</h1>";

            var workStream = new MemoryStream();

            using (var pdfWriter = new iText.Kernel.Pdf.PdfWriter(workStream))
            {
                pdfWriter.SetCloseStream(false);
                using (var document = HtmlConverter.ConvertToDocument(html, pdfWriter))
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

    }
}
