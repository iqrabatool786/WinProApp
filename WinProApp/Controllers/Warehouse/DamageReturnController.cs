using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels;
using WinProApp.ViewModels.Warehouse.DamageReturns;

namespace WinProApp.Controllers.Warehouse
{
    public class DamageReturnController : BasedUserController
    {
        public readonly CommonService _commonService;
        public readonly DamageReturnService _damageReturnService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        public readonly PurchaseService _purchaseService;
        public readonly StoreService _storeService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public DamageReturnController(CommonService commonService, DamageReturnService damageReturnService, SupplierPurchaseService supplierPurchaseService, PurchaseService purchaseService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _commonService = commonService;
            _damageReturnService = damageReturnService;
            _supplierPurchaseService = supplierPurchaseService;
            _purchaseService = purchaseService;
            _storeService = storeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/DamageReturns")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/DamageReturnList")]
        [HttpPost]
        public async Task<IActionResult> ReturnList(JQueryDataTableParamModel param)
        {

            var results = _damageReturnService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
             //   InvoiceId = x.InvoiceId,
           //     SupplierId = x.SupplierId,
           //     SupplierName = _purchaseService.GetSupplierByIdAsync(x.SupplierId.Value).Result.CompanyName,
                Date = x.Date != null ? x.Date.Value.ToString("yyyy-MM-dd") : "",
                AttachedDoc = x.AttachedDoc ?? "",
                TransactionType = x.TransactionType,
                Description = x.Description,
                Status = x.Status == true ? "Approved" : "OnHold",
          //      VatAmount = x.VatAmount,
                Total = x.Total,
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

        [Route("/Warehouse/DamageReturnCreate")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var stores = await _storeService.GetByAllAsync();
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();

            ViewBag.Stores = new SelectList(stores, "Id", "Name");
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View();
        }


        [Route("/Warehouse/DamageReturnCreate")]
        [HttpPost]
        public async Task<IActionResult> DamageReturnCreate(AddViewModel model, IFormFile ReturneDoc)
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

                var ReturnInfo = new DamageReturn();
              //  ReturnInfo.SupplierId = model.SupplierId;
              //  ReturnInfo.InvoiceId = model.InvoiceId;
                ReturnInfo.Date = model.Date;
                ReturnInfo.AttachedDoc = docName;
                ReturnInfo.Description = model.Description;
                ReturnInfo.Status = model.Status;
              //  ReturnInfo.VatAmount = model.VatAmount;
                ReturnInfo.Total = model.Total;
                ReturnInfo.StoreId = model.StoreId;
                ReturnInfo.CratedDate = DateTime.Now;
                ReturnInfo.CreatedBy = User.Identity.Name;
                ReturnInfo.UpdatedDate = DateTime.Now;
                ReturnInfo.UpdatedBy = User.Identity.Name;

                var data = await _damageReturnService.CreateAsync(ReturnInfo);

                List<DamageReturnDetail> items = new List<DamageReturnDetail>();

                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] qtyDozens = Request.Form["QtyDozen"];
                string[] qtyPices = Request.Form["QtyPices"];
                string[] prices = Request.Form["itemCost"];
             //   string[] vats = Request.Form["itemVat"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long productId = 0;
                    decimal? qtyd = null;
                    decimal? qtyp = null;
                    decimal? price = null;
                  //  decimal? vat = null;

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

                    //if (!string.IsNullOrEmpty(vats[i]))
                    //{
                    //    vat = decimal.Parse(vats[i]);
                    //}

                    items.Add(new DamageReturnDetail()
                    {
                        DamageReturnId = data.Id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        QtyDozen = qtyd,
                        QtyPices = qtyp,
                        Price = price,
                   //     Vat = vat,
                    });

                }

                await _damageReturnService.CreateItemsAsync(data.Id, items);
                return new JsonResult(new { id = data.Id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }


        [Route("/Warehouse/DamageReturnDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var info = await _damageReturnService.GetByIdAsync(id);
            var ReturnInfo = new EditViewModel();
            ReturnInfo.Id = info.Id;
          //  ReturnInfo.SupplierId = info.SupplierId;
          //  ReturnInfo.InvoiceId = info.InvoiceId;
            ReturnInfo.Date = info.Date;
            ReturnInfo.AttachedDoc = info.AttachedDoc;
            ReturnInfo.Description = info.Description;
            ReturnInfo.Status = info.Status;
         //   ReturnInfo.VatAmount = info.VatAmount;
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


        [Route("/Warehouse/DamageReturnEdit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var info = await _damageReturnService.GetByIdAsync(id);
            var ReturnInfo = new EditViewModel();
            ReturnInfo.Id = info.Id;
           // ReturnInfo.SupplierId = info.SupplierId;
          //  ReturnInfo.InvoiceId = info.InvoiceId;
            ReturnInfo.Date = info.Date;
            ReturnInfo.AttachedDoc = info.AttachedDoc;
            ReturnInfo.Description = info.Description;
            ReturnInfo.Status = info.Status;
          //  ReturnInfo.VatAmount = info.VatAmount;
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



        [Route("/Warehouse/DamageReturnEdit")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model, IFormFile ReturneDoc)
        {
            long id = model.Id;
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

                var ReturnInfo = await _damageReturnService.GetByIdAsync(id);
                string? oldDoc = ReturnInfo.AttachedDoc;
                if (oldDoc != null && docName != null)
                {
                    string oldDocPath = System.IO.Path.Combine(uploadPath, oldDoc);
                    if (System.IO.File.Exists(oldDocPath))
                    {
                        System.IO.File.Delete(oldDocPath);
                    }
                }
                ReturnInfo.Id = id;
             //   ReturnInfo.SupplierId = model.SupplierId;
            //    ReturnInfo.InvoiceId = model.InvoiceId;
                ReturnInfo.Date = model.Date;
                ReturnInfo.AttachedDoc = docName;
                ReturnInfo.Description = model.Description;
                ReturnInfo.Status = model.Status;
             //   ReturnInfo.VatAmount = model.VatAmount;
                ReturnInfo.Total = model.Total;
                ReturnInfo.StoreId = model.StoreId;
                ReturnInfo.CratedDate = DateTime.Now;
                ReturnInfo.CreatedBy = User.Identity.Name;
                ReturnInfo.UpdatedDate = DateTime.Now;
                ReturnInfo.UpdatedBy = User.Identity.Name;



                List<DamageReturnDetail> items = new List<DamageReturnDetail>();

                string[] Ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];
                string[] qtyDozens = Request.Form["QtyDozen"];
                string[] qtyPices = Request.Form["QtyPices"];
                string[] prices = Request.Form["itemCost"];
              //  string[] vats = Request.Form["itemVat"];

                for (int i = 0; i < itembarcode.Length; i++)
                {
                    long itemId = 0;
                    long productId = 0;
                    decimal? qtyd = null;
                    decimal? qtyp = null;
                    decimal? price = null;
                  //  decimal? vat = null;

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

                    //if (!string.IsNullOrEmpty(vats[i]))
                    //{
                    //    vat = decimal.Parse(vats[i]);
                    //}

                    items.Add(new DamageReturnDetail()
                    {
                        Id = itemId,
                        DamageReturnId = id,
                        ProductId = productId,
                        Barcode = itembarcode[i],
                        QtyDozen = qtyd,
                        QtyPices = qtyp,
                        Price = price,
                     //   Vat = vat,
                    });

                }

                await _damageReturnService.UpdateAsync(ReturnInfo, items);
                //  await _supplierReturnService.CreateItemsAsync(data.Id, items);
                return new JsonResult(new { id = id });

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }

        

        [Route("/Warehouse/GetDamageReturnItems/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDamageReturnItems(long id)
        {
            List<DetailsViewModel> Items = new List<DetailsViewModel>();

            var rItems = await _damageReturnService.GetItemsByReturnIdAsync(id);

            foreach (var item in rItems)
            {
                Items.Add(new DetailsViewModel
                {
                    Id = item.Id,
                    DamageReturnId = item.DamageReturnId,
                    ProductId = item.ProductId,
                    Barcode = item.Barcode,
                    DescriptionEnglish = await _commonService.GetProductNameEnglishByBarcodeAsync(item.Barcode),
                    DescriptionArabic = await _commonService.GetProductNameArabicByBarcodeAsync(item.Barcode),
                    QtyDozen = item.QtyDozen,
                    QtyPices = item.QtyPices,
                    Price = item.Price,
                   // Vat = item.Vat,
                });
            }

            return Json(data: Items);
        }

        [Route("/Warehouse/DeleteDamageReturnItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteDamageReturnItem(long id)
        {
            try
            {
                await _damageReturnService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/DeleteDamageReturn/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteDamageReturn(long id)
        {
            try
            {
                var result = await _damageReturnService.GetByIdAsync(id);
                await _damageReturnService.DeleteAsync(result);

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
