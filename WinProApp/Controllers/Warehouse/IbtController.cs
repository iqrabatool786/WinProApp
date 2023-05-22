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
using WinProApp.ViewModels.Warehouse.IBT;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Controllers.Warehouse
{
    [Authorize]
    public class IbtController : BasedUserController
    {
        public readonly IbtInfoService _ibtInfoService;
        public readonly CommonService _commonService;
        public readonly StoreService _storeService;
        public readonly UserManager<WinProAppUser> _userManager;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public IbtController(IbtInfoService ibtInfoService, CommonService commonService, StoreService storeService, UserManager<WinProAppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _ibtInfoService = ibtInfoService;
            _commonService = commonService;
            _storeService = storeService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/IBT")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/IBTList")]
        [HttpPost]
        public async Task<IActionResult> IBTList(JQueryDataTableParamModel param)
        {

            var results = _ibtInfoService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                Date= x.Date,
                StrDate = x.Date.ToString("yyyy-MM-dd"),
                FromStoreId=x.FromStoreId,
                FromStoreName = _storeService.GetNameByIdAsync(x.FromStoreId).Result,
                ToStoreId = x.ToStoreId,
                ToStoreName = _storeService.GetNameByIdAsync(x.ToStoreId).Result,
                Description = x.Description,
                Status = x.Status == true ? "Approved" : "OnHold",
                Total = x.Total.Value >0 ? x.Total.Value.ToString("0.00") : "",
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

        [Route("/Warehouse/CreateIBT")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int curretStoreId = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).StoreId; //int.Parse(HttpContext.Session.GetString("currentStore").ToString());
            Thread.Sleep(50);
            var stores = await _storeService.GetByAllAsync();
            var toStores = stores.Where(x=>x.Id != curretStoreId).ToList();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name", curretStoreId);
            ViewBag.ToStores = new SelectList(toStores, "Id", "Name");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            ViewBag.CurretStoreId = curretStoreId;

            return View();
        }


        [Route("/Warehouse/CreateIBT")]
        [HttpPost]
        public async Task<IActionResult> Create(AddViewModel model)
        {
            try
            {

                var IBtInfo = new IbtInfo();
                IBtInfo.Date = model.Date;
                IBtInfo.FromStoreId = model.FromStoreId;
                IBtInfo.ToStoreId = model.ToStoreId;
                IBtInfo.Description = model.Description;
                IBtInfo.Status = model.Status;
                IBtInfo.Total = model.Total;
                IBtInfo.CratedDate = DateTime.Now;
                IBtInfo.CreatedBy = User.Identity.Name;
                IBtInfo.UpdatedDate = DateTime.Now;
                IBtInfo.UpdatedBy = User.Identity.Name;

                var data = await _ibtInfoService.CreateAsync(IBtInfo);

                List<IbtDetails> items = new List<IbtDetails>();

                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] boxnos = Request.Form["ItemBoxNo"];
                string[] qtys = Request.Form["ItemQty"];
                string[] prices = Request.Form["itemPrice"];
                string[] precentx = Request.Form["itemPrecent"];
                string[] rprices = Request.Form["itemRetail"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long productId = 0;
                    int? qty = null;
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


                    items.Add(new IbtDetails()
                    {
                        IbtId = data.Id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        BoxNo = boxnos[i],
                        Qty = qty,
                        Price = price,
                        Precentage = precent,
                        RetailPrice = pricer,
                    });


                }

                await _ibtInfoService.CreateItemsAsync(data.Id, items);
                return new JsonResult(new { id = data.Id });

        }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

}


        [Route("/Warehouse/IBTDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var info = await _ibtInfoService.GetByIdAsync(id);
            var IBtInfo = new EditViewModel();
            IBtInfo.Id= info.Id;
            IBtInfo.Date = info.Date;
            IBtInfo.FromStoreId = info.FromStoreId;
            IBtInfo.ToStoreId = info.ToStoreId;
            IBtInfo.Description = info.Description;
            IBtInfo.Status = info.Status;
            IBtInfo.Total = info.Total;


            int curretStoreId = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).StoreId; //int.Parse(HttpContext.Session.GetString("currentStore").ToString());
            Thread.Sleep(50);
            var stores = await _storeService.GetByAllAsync();
            var toStores = stores.Where(x => x.Id != curretStoreId).ToList();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name", curretStoreId);
            ViewBag.ToStores = new SelectList(toStores, "Id", "Name");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
            ViewBag.CurretStoreId = curretStoreId;

            return View(IBtInfo);
        }


        [Route("/Warehouse/EditIBT/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var info = await _ibtInfoService.GetByIdAsync(id);
            var IBtInfo = new EditViewModel();
            IBtInfo.Id = info.Id;
            IBtInfo.Date = info.Date;
            IBtInfo.FromStoreId = info.FromStoreId;
            IBtInfo.ToStoreId = info.ToStoreId;
            IBtInfo.Description = info.Description;
            IBtInfo.Status = info.Status;
            IBtInfo.Total = info.Total;

            int curretStoreId = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).StoreId; //int.Parse(HttpContext.Session.GetString("currentStore").ToString());
            Thread.Sleep(50);
            var stores = await _storeService.GetByAllAsync();
            var toStores = stores.Where(x => x.Id != curretStoreId).ToList();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name", curretStoreId);
            ViewBag.ToStores = new SelectList(toStores, "Id", "Name");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
            ViewBag.CurretStoreId = curretStoreId;

            return View(IBtInfo);
        }



        [Route("/Warehouse/EditIBT")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            long id = model.Id;
            try
            {

                var IBtInfo = await _ibtInfoService.GetByIdAsync(id);
                IBtInfo.Id = id;
                IBtInfo.Date = model.Date;
                IBtInfo.FromStoreId = model.FromStoreId;
                IBtInfo.ToStoreId = model.ToStoreId;
                IBtInfo.Description = model.Description;
                IBtInfo.Status = model.Status;
                IBtInfo.Total = model.Total;
                IBtInfo.UpdatedDate = DateTime.Now;
                IBtInfo.UpdatedBy = User.Identity.Name;



                List<IbtDetails> items = new List<IbtDetails>();

                string[] Ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] boxnos = Request.Form["ItemBoxNo"];
                string[] qtys = Request.Form["ItemQty"];
                string[] prices = Request.Form["itemPrice"];
                string[] precentx = Request.Form["itemPrecent"];
                string[] rprices = Request.Form["itemRetail"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long itemId = 0;
                    long productId = 0;
                    int? qty = null;
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

                    items.Add(new IbtDetails()
                    {
                        Id = itemId,
                        IbtId = id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        BoxNo = boxnos[i],
                        Qty = qty,
                        Price = price,
                        Precentage = precent,
                        RetailPrice = pricer,
                    });

                }

                await _ibtInfoService.UpdateAsync(IBtInfo, items);
                return new JsonResult(new { id = id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }


        [Route("/Warehouse/GetIBTItems/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetIBTItems(long id)
        {
            List<ItemDetailsViewModel> Items = new List<ItemDetailsViewModel>();

            var rItems = await _ibtInfoService.GetItemsByIbtIdAsync(id);

            foreach (var item in rItems)
            {
                Items.Add(new ItemDetailsViewModel
                {
                    Id = item.Id,
                    IbtId = item.IbtId,
                    ProductId = item.ProductId,
                    Barcode = item.Barcode,
                    DescriptionEnglish=  _commonService.GetProductInfoByIdAsync(item.ProductId).Result.ProductNameEng,
                    DescriptionArabic = _commonService.GetProductInfoByIdAsync(item.ProductId).Result.ProductNameArabic,
                    BoxNo =item.BoxNo,
                    Qty = item.Qty,
                    Price = item.Price,
                    Precentage = item.Precentage,
                    RetailPrice = item.RetailPrice,
                });
            }

            return Json(data: Items);
        }

        [Route("/Warehouse/DeleteIbtItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteIbtItem(long id)
        {
            try
            {
                await _ibtInfoService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/DeleteIBT/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteIBT(long id)
        {
            try
            {
                var result = await _ibtInfoService.GetByIdAsync(id);
                await _ibtInfoService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/ReportCreate/{id}")]
        [HttpPost]
        public async Task<IActionResult> ReportCreate(long id)
        {
            var ibtInfo = await _ibtInfoService.GetByIdAsync(id);
            var storeInfo1 = await _storeService.GetByIdAsync(ibtInfo.FromStoreId);
            var storeInfo2 = await _storeService.GetByIdAsync(ibtInfo.ToStoreId);
            var IbtItems = await _ibtInfoService.GetItemsByIbtIdAsync(id);



            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "IBTReport.pdf";

            FilePath = Path.Combine(FilePath, fileName);

            if (System.IO.File.Exists(FilePath))
                System.IO.File.Delete(FilePath);

            FileInfo newFile = new FileInfo(FilePath);


            var html = "<table style='width:100%; margin-top:0px; padding:0px;border-spacing:0;border-collapse: collapse; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:center; width:80%; border-bottom:1px solid #454545;'>IBT (Inter Branch Transfer)</td>";
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
            html += "<td style='text-align:left; width:50%;'>IBT No: " + ibtInfo.Id.ToString() + "</td>";
            html += "<td style='text-align:right; width:50%;'>Date: " + ibtInfo.Date.ToString("dd/MM/yyyy") + "</td>";
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
            html += "<td style='padding-top:20px;'>Note: " + ibtInfo.Description + "</td>";
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
            html += "<th style='padding:3px; border:1px solid #454545;'>Price</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Retail Price</th>";
            html += "<th style='padding:3px; border:1px solid #454545;'>Total</th>";
            html += "</tr>";

            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in IbtItems)
            {
                decimal tot = 0;
                string strTot = "";
                tot = (data.Qty??0) * (data.Price??0);
                strTot = tot.ToString("0.00");
                totQty += (data.Qty ?? 0);

                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                //html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Barcode + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545;'>" + _commonService.GetProductInfoByIdAsync(data.ProductId).Result.ProductNameEng + "</td>";
                //html += "<td style='padding:3px; border:1px solid #454545;'>" + data.BoxNo + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Qty + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Price + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.RetailPrice + "</td>";
                html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + strTot + "</td>";
                html += "</tr>";
            }

            html += "<tr style='page-break-inside: avoid;'>";
            html += "<td colspan='2' style='padding:3px; border:1px solid #454545;'>TOTAL</td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + totQty.ToString() + "</td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'></td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'></td>";
            html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + ibtInfo.Total.Value.ToString("0.00") + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "</tr>";
            html += "<tr style='page-break-inside: avoid;'>";
            html += "<td style='text-align:left; width:100%;'>";
            html += "<table>";
            html += "<tr>";
            html += "<td style='padding:60px 5px 20px 5px; text-align:center; width:33%;'>----------------------------------------</br>Store Manager Signature</td>";
            html += "<td style='padding:60px 5px 20px 5px; text-align:center; width:33%;'>----------------------------------------</br>Transport Signature</td>";
            html += "<td style='padding:60px 5px 20px 5px; text-align:center; width:33%;'>----------------------------------------</br>Receive Store Signature</td>";
            html += "</tr>";
            html += "</table>";
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
