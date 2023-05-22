using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels;
using WinProApp.ViewModels.Warehouse.Returns;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Controllers.Warehouse
{
    [Authorize]
    public class ReturnController : BasedUserController
    {
        public readonly CommonService _commonService;
        public readonly SupplierReturnService _supplierReturnService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        public readonly PurchaseService _purchaseService;
        public readonly StoreService _storeService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public ReturnController(CommonService commonService, SupplierReturnService supplierReturnService, SupplierPurchaseService supplierPurchaseService, PurchaseService purchaseService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _commonService = commonService;
            _supplierReturnService = supplierReturnService;
            _supplierPurchaseService = supplierPurchaseService;
            _purchaseService = purchaseService;
            _storeService= storeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/Returns")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/ReturnList")]
        [HttpPost]
        public async Task<IActionResult> ReturnList(JQueryDataTableParamModel param)
        {

            var results = _supplierReturnService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                InvoiceId = x.InvoiceId,
                SupplierId = x.SupplierId,
                SupplierName = _purchaseService.GetSupplierByIdAsync(x.SupplierId.Value).Result.CompanyName,
                Date = x.Date != null ? x.Date.Value.ToString("yyyy-MM-dd") : "",
                AttachedDoc = x.AttachedDoc ?? "",
                TransactionType = x.TransactionType,
                Description = x.Description,
                Status = x.Status == true ? "Approved" : "OnHold",
                VatAmount = x.VatAmount.Value > 0? x.VatAmount.Value.ToString("0.00"):"",
                Total = x.Total.Value > 0? x.Total.Value.ToString("0.00") : "",
                StoreId = x.StoreId,
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

        [Route("/Warehouse/CreateReturn")]
        [HttpGet]
        public async Task<IActionResult> CreateReturn()
        {
            var stores = await _storeService.GetByAllAsync();
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name");
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View();
        }


        [Route("/Warehouse/CreateReturn")]
        [HttpPost]
        public async Task<IActionResult> CreateReturn(AddViewModel model, IFormFile ReturneDoc)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string? docName = null;
                if (ReturneDoc != null)
                {
                    string curDocName = Path.GetFileName(ReturneDoc.FileName);
                    string curDocExtention = Path.GetExtension(ReturneDoc.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await ReturneDoc.CopyToAsync(stream);
                    }

                }

                var ReturnInfo = new SupplierReturn();
                ReturnInfo.SupplierId= model.SupplierId;
                ReturnInfo.InvoiceId= model.InvoiceId;
                ReturnInfo.Date= model.Date;
                ReturnInfo.AttachedDoc = docName;
                ReturnInfo.Description= model.Description;
                ReturnInfo.Status= model.Status;
                ReturnInfo.VatAmount= model.VatAmount;
                ReturnInfo.Total= model.Total;
                ReturnInfo.StoreId= model.StoreId;
                ReturnInfo.CratedDate = DateTime.Now;
                ReturnInfo.CreatedBy = User.Identity.Name;
                ReturnInfo.UpdatedDate = DateTime.Now;
                ReturnInfo.UpdatedBy = User.Identity.Name;

                var data = await _supplierReturnService.CreateAsync(ReturnInfo);

                List<SupplierReturnDetail> items = new List<SupplierReturnDetail>();

                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] qtyDozens = Request.Form["QtyDozen"];
                string[] qtyPices = Request.Form["QtyPices"];
                string[] prices = Request.Form["itemCost"];
                string[] vats = Request.Form["itemVat"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long productId = 0;
                    decimal? qtyd = null;
                    decimal? qtyp = null;
                    decimal? price = null;
                    decimal? vat = null;

                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = long.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(qtyDozens[i]))
                    {
                        qtyd = decimal.Parse(qtyDozens[i]);
                    }

                    if (!string.IsNullOrEmpty(qtyPices[i]))
                    {
                        qtyp = decimal.Parse(qtyPices[i]);
                    }

                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    if (!string.IsNullOrEmpty(vats[i]))
                    {
                        vat = decimal.Parse(vats[i]);
                    }

                    items.Add(new SupplierReturnDetail()
                    {
                        ReturnId = data.Id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        QtyDozen = qtyd,
                        QtyPices = qtyp,
                        Price = price,
                        Vat = vat,
                    });

                }

                await _supplierReturnService.CreateItemsAsync(data.Id, items);
                return new JsonResult(new { id = data.Id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }

        [Route("/Warehouse/ReturnDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> ReturnDetails(long id)
        {
            var info = await _supplierReturnService.GetByIdAsync(id);
            var ReturnInfo = new EditViewModel();
            ReturnInfo.Id = info.Id;
            ReturnInfo.SupplierId = info.SupplierId;
            ReturnInfo.InvoiceId = info.InvoiceId;
            ReturnInfo.Date = info.Date;
            ReturnInfo.AttachedDoc = info.AttachedDoc;
            ReturnInfo.Description = info.Description;
            ReturnInfo.Status = info.Status;
            ReturnInfo.VatAmount = info.VatAmount;
            ReturnInfo.Total = info.Total;
            ReturnInfo.StoreId = info.StoreId;

            var stores = await _storeService.GetByAllAsync();
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name");
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;



            return View(ReturnInfo);
        }


        [Route("/Warehouse/EditReturn/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var info = await _supplierReturnService.GetByIdAsync(id);
            var ReturnInfo = new EditViewModel();
            ReturnInfo.Id= info.Id;
            ReturnInfo.SupplierId = info.SupplierId;
            ReturnInfo.InvoiceId = info.InvoiceId;
            ReturnInfo.Date = info.Date;
            ReturnInfo.AttachedDoc = info.AttachedDoc;
            ReturnInfo.Description = info.Description;
            ReturnInfo.Status = info.Status;
            ReturnInfo.VatAmount = info.VatAmount;
            ReturnInfo.Total = info.Total;
            ReturnInfo.StoreId = info.StoreId;

            var stores = await _storeService.GetByAllAsync();
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name");
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            

            return View(ReturnInfo);
        }



        [Route("/Warehouse/EditReturn")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model, IFormFile ReturnDoc)
        {
            long id=model.Id;
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string? docName = null;
                if (ReturnDoc != null)
                {
                    string curDocName = Path.GetFileName(ReturnDoc.FileName);
                    string curDocExtention = Path.GetExtension(ReturnDoc.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await ReturnDoc.CopyToAsync(stream);
                    }

                }

                var ReturnInfo = await _supplierReturnService.GetByIdAsync(id);
                string? oldDoc = ReturnInfo.AttachedDoc;
                if (oldDoc != null && docName !=null)
                {
                    string oldDocPath = System.IO.Path.Combine(uploadPath, oldDoc);
                    if(System.IO.File.Exists(oldDocPath))
                    {
                        System.IO.File.Delete(oldDocPath);
                    }
                }
                ReturnInfo.Id= id;
                ReturnInfo.SupplierId = model.SupplierId;
                ReturnInfo.InvoiceId = model.InvoiceId;
                ReturnInfo.Date = model.Date;
                ReturnInfo.AttachedDoc = docName;
                ReturnInfo.Description = model.Description;
                ReturnInfo.Status = model.Status;
                ReturnInfo.VatAmount = model.VatAmount;
                ReturnInfo.Total = model.Total;
                ReturnInfo.StoreId = model.StoreId;
                ReturnInfo.UpdatedDate = DateTime.Now;
                ReturnInfo.UpdatedBy = User.Identity.Name;

               

                List<SupplierReturnDetail> items = new List<SupplierReturnDetail>();

                string[] Ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] qtyDozens = Request.Form["QtyDozen"];
                string[] qtyPices = Request.Form["QtyPices"];
                string[] prices = Request.Form["itemCost"];
                string[] vats = Request.Form["itemVat"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long itemId = 0;
                    long productId = 0;
                    decimal? qtyd = null;
                    decimal? qtyp = null;
                    decimal? price = null;
                    decimal? vat = null;

                    if (!string.IsNullOrEmpty(Ids[i]))
                    {
                        itemId = long.Parse(Ids[i]);
                    }

                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = long.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(qtyDozens[i]))
                    {
                        qtyd = decimal.Parse(qtyDozens[i]);
                    }

                    if (!string.IsNullOrEmpty(qtyPices[i]))
                    {
                        qtyp = decimal.Parse(qtyPices[i]);
                    }

                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    if (!string.IsNullOrEmpty(vats[i]))
                    {
                        vat = decimal.Parse(vats[i]);
                    }

                    items.Add(new SupplierReturnDetail()
                    {
                        Id = itemId,
                        ReturnId = id,
                        ProductId=productId,
                        Barcode = itembarcode[i],
                        QtyDozen = qtyd,
                        QtyPices = qtyp,
                        Price = price,
                        Vat = vat,
                    });

                }

                await _supplierReturnService.UpdateAsync(ReturnInfo, items);
              //  await _supplierReturnService.CreateItemsAsync(data.Id, items);
                return new JsonResult(new { id = id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }



        [Route("/Warehouse/GetInvoiceNoListBySupplier/{supplierId}")]
        [HttpGet]
        public async Task<IActionResult> GetInvoiceNoListBySupplier(long supplierId)
        {
            List<InvoiceIdListViewModel> InvoiceList = new List<InvoiceIdListViewModel>();

            var invoiceInfo = await _supplierReturnService.GetInvoiceByInvoiceIdAsync(supplierId);

            foreach(var item in invoiceInfo)
            {
                InvoiceList.Add(new InvoiceIdListViewModel
                {
                    Id= item.Id,
                    InvoiceNo= item.InvoiceNo
                });
            }

            return Json(data:InvoiceList);
        }


        [Route("/Warehouse/GetReturnItems/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetReturnItems(long id)
        {
            List<DetailsViewModel> Items = new List<DetailsViewModel>();

            var rItems= await _supplierReturnService.GetItemsByReturnIdAsync(id);

            foreach (var item in rItems)
            {
                Items.Add(new DetailsViewModel
                {
                    Id= item.Id,
                    ReturnId = item.ReturnId,
                    ProductId= item.ProductId,
                    Barcode = item.Barcode,
                    DescriptionEnglish = await _commonService.GetProductNameEnglishByBarcodeAsync(item.Barcode),
                    DescriptionArabic =  await _commonService.GetProductNameArabicByBarcodeAsync(item.Barcode),
                    QtyDozen = item.QtyDozen,
                    QtyPices = item.QtyPices,
                    Price = item.Price,
                    Vat = item.Vat,
                });
            }

            return Json(data: Items);
        }

        [Route("/Warehouse/DeleteReturnItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteReturnItem(long id)
        {
            try
            {
                await _supplierReturnService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/DeleteReturn/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteReturn(long id)
        {
            try
            {
                var result = await _supplierReturnService.GetByIdAsync(id);
                await _supplierReturnService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

    }
}
