using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Merchant.SelectedItem;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

namespace WinProApp.Controllers.Merchant
{
    [Authorize(Roles = "Administrator,Warehouse,Purchase")]
    public class SelectedItemDiscountController : BasedUserController
    {
        public readonly SelectedItemDiscountService _selectedItemDiscountService;
        public readonly StoreService _storeService;
        public readonly ProductInfoService _productInfoService;
        public readonly CommonService _commonService;
        public readonly CategoryServices _categoryServices;
        public readonly VendorService _vendorService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public SelectedItemDiscountController(SelectedItemDiscountService selectedItemDiscountService, StoreService storeService, ProductInfoService productInfoService, CommonService commonService, CategoryServices categoryServices, VendorService vendorService, IWebHostEnvironment webHostEnvironment)
        {
            _selectedItemDiscountService = selectedItemDiscountService;
            _storeService = storeService;
            _productInfoService = productInfoService;
            _commonService = commonService;
            _categoryServices = categoryServices;
            _vendorService = vendorService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Merchant/SelectedItemDiscount")]
        [HttpGet]
        public async Task<IActionResult> SelectedItemDiscount()
        {
            var storeInfo = await _storeService.GetByAllAsync();
            var categoryInfo = await _categoryServices.GetByAllAsync();
            var vendorInfo = await _vendorService.GetByAllAsync();

            ViewBag.StoreInfo = new SelectList(storeInfo, "Id", "Name");
            ViewBag.categoryInfo = new SelectList(categoryInfo, "Id", "NameEng");
            ViewBag.vendorInfo = new SelectList(vendorInfo, "Id", "NameEng");
            return View();
        }

        [Route("/Merchant/GetSelectedItemDiscountList")]
        [HttpPost]
        public IActionResult GetSelectedItemDiscountList(JQueryDataTableParamModel param)
        {
            var results = _selectedItemDiscountService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                StoreName = x.StoreId > 0 ? _storeService.GetByIdAsync(x.StoreId).Result.Name : "-All-",
                IsMembersOnly = x.IsMembersOnly == true ? "Members Only" : "All",
                StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                DiscountPercentage = x.DiscountPercentage != null ? x.DiscountPercentage.Value.ToString("0.00") : "0.00",
                FixedDiscount = x.FixedDiscount != null ? x.FixedDiscount.Value.ToString("0.00") : "0.00",
                StartAmount = x.StartAmount != null ? x.StartAmount.Value.ToString("0.00") : "0.00",
                Approved = x.Approved == true ? "Yes" : "No",
                TotalRecordCount = x.TotalRecordCount
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


        [Route("/Merchant/GetProductList")]
        [HttpPost]
        public IActionResult GetProductList(JQueryDataTableParamModel param, int catgoryId, int vendorId)
        {
            var results = _productInfoService.GetFilteredList(param, catgoryId, vendorId).Result.Select(x => new ProductInfoViewModel()
            {
                Id = x.Id,
                Barcode = x.ProductId,
                DescriptionEng = x.ProductNameEng,
                DescriptionArabic = x.ProductNameArabic,
                CategoryId=x.CategoryId,
                CategoryName = x.CategoryId != null && x.CategoryId !=0?_commonService.GetCategoryNameByIdAsync(x.CategoryId).Result : "",
                VendorId= x.VendorId,
                VendorName = x.VendorId !=null && x.VendorId != 0? _commonService.GetVendorNameByIdAsync(x.VendorId).Result :"",
                OrgPrice = x.OreginalPrice!=null?x.OreginalPrice.Value.ToString("0.00"):(x.ProductInitialPrice != null? x.ProductInitialPrice.Value.ToString("0.00"):"0.00"),
                TotalRecordCount = x.TotalRecordCount
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

        [Route("/Merchant/CreateUpdateSelectedItemDiscount")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateSelectedItemDiscount(AddEditViewModel model)
        {
            try
            {
                long insertId = 0;
                string[] itemIds = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] barCodes = Request.Form["ItemBarcode"];
                string[] discPercents = Request.Form["txtDiscPercent"];
                string[] discFixes = Request.Form["txtDiscFixed"];
                string[] newPrices = Request.Form["txtNewPriceProd"];

                long itemId = 0;
                long productId = 0;

                List<SelectedItemDiscountItems> items = new List<SelectedItemDiscountItems>();

                if (itemIds.Length > 0)
                {
                    if (model.Id == 0)
                    {
                        var info = new SelectedItemDiscount();
                        info.StoreId = model.StoreId;
                        info.IsMembersOnly = model.IsMembersOnly;
                        info.StartDate = model.StartDate;
                        info.EndDate = model.EndDate;
                        if (model.DiscountType == "precentage")
                        {
                            info.DiscountPercentage = model.DiscountAmount;
                            info.FixedDiscount = 0;
                        }
                        else
                        {
                            info.FixedDiscount = model.DiscountAmount;
                            info.DiscountPercentage = 0;
                        }
                        info.StartAmount = model.StartAmount;
                        //var one = model.Approved;
                        info.Approved = model.Approved ? true : false;
                        info.CratedDate = DateTime.Now;
                        info.CreatedBy = User.Identity.Name;
                        info.UpdatedDate = DateTime.Now;
                        info.UpdatedBy = User.Identity.Name;

                        var data = await _selectedItemDiscountService.CreateAsync(info);
                        insertId = data.Id;

                        for (int i = 0; i < itemIds.Length; i++)
                        {
                            // itemId = 0;
                            // productId = 0;

                            productId = long.Parse(productIds[i].ToString());

                            items.Add(new SelectedItemDiscountItems()
                            {
                                SelectedItemDiscountId = insertId,
                                ProductId = productId,
                                Barcode = barCodes[i].ToString(),
                                DiscountPercentage = !string.IsNullOrEmpty(discPercents[i].ToString()) ? Convert.ToInt16(discPercents[i].ToString()) : 0,
                                DiscountFixed = !string.IsNullOrEmpty(discFixes[i].ToString()) ? Convert.ToDecimal(discFixes[i].ToString()) : 0,
                                NewPrice = !string.IsNullOrEmpty(newPrices[i].ToString()) ? Convert.ToDecimal(newPrices[i].ToString()) : 0
                            });
                        }
                        await _selectedItemDiscountService.CreateItemsAsync(items);
                    }
                    else
                    {
                        var info = await _selectedItemDiscountService.GetByIdAsync(model.Id);
                        info.StoreId = model.StoreId;
                        info.IsMembersOnly = model.IsMembersOnly;
                        info.StartDate = model.StartDate;
                        info.EndDate = model.EndDate;
                        if (model.DiscountType == "precentage")
                        {
                            info.DiscountPercentage = model.DiscountAmount;
                            info.FixedDiscount = 0;
                        }
                        else
                        {
                            info.FixedDiscount = model.DiscountAmount;
                            info.DiscountPercentage = 0;
                        }
                        info.StartAmount = model.StartAmount;
                        info.Approved = model.Approved;
                        info.UpdatedDate = DateTime.Now;
                        info.UpdatedBy = User.Identity.Name;

                        var data = await _selectedItemDiscountService.UpdateAsync(info);
                        insertId = data.Id;

                        for (int i = 0; i < itemIds.Length; i++)
                        {
                            //  itemId = 0;
                            //   productId = 0;

                            itemId = long.Parse(itemIds[i].ToString());
                            productId = long.Parse(productIds[i].ToString());

                            items.Add(new SelectedItemDiscountItems()
                            {
                                Id = itemId,
                                SelectedItemDiscountId = insertId,
                                ProductId = productId,
                                Barcode = barCodes[i].ToString(),
                            });
                        }
                        await _selectedItemDiscountService.UpdateItemsAsync(items);
                    }

                    return new JsonResult(new { id = insertId });
                }
                else
                {
                    return new JsonResult(new { id = 0 });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [Route("/Merchant/GetSelectedItemDiscountInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetSelectedItemDiscountInfo(long id)
        {
            var info = await _selectedItemDiscountService.GetByIdAsync(id);
            var model = new DetailViewModel();
            model.Id = id;
            model.StoreId = info.StoreId;
            model.IsMembersOnly = info.IsMembersOnly == true ? "true" : "false";
            model.StartDate = info.StartDate.ToString("yyyy-MM-dd");
            model.EndDate = info.EndDate.ToString("yyyy-MM-dd");
            model.DiscountType = info.DiscountPercentage != null ? (info.DiscountPercentage.Value > 0 ? "precentage" : "fixed") : "fixed";
            model.DiscountPercentage = info.DiscountPercentage;
            model.FixedDiscount = info.FixedDiscount;
            model.StartAmount = info.StartAmount;
            model.Approved = info.Approved == true ? "true" : "false";

            return Json(data: model);

        }

        [Route("/Merchant/GetSelectedItemDiscountItems/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetSelectedItemDiscountItems(long id)
        {
            List<SelectedItemDiscountItems> model = new List<SelectedItemDiscountItems>();
            var info = await _selectedItemDiscountService.GetAllItemsByIdAsync(id);

            foreach(var item in info)
            {
                model.Add(new SelectedItemDiscountItems()
                {
                    Id = item.Id,
                    ProductId= item.ProductId,
                    Barcode = item.Barcode,
                });
            }

            return Json(data: model);
        }



            [Route("/Merchant/ValidateSelectedItem")]
        [HttpPost]
        public async Task<IActionResult> ValidateSelectedItem(DateTime startDt, long productId, long recordId)
        {
            int id = 0;
            var info = await _selectedItemDiscountService.GetByAllAsync();

            if(info != null)
            {
                if (recordId == 0)
                {
                    if (info.Where(x => x.EndDate <= startDt && x.Approved == true).Count() > 0)
                    {
                        var info2 = info.Where(x => x.EndDate <= startDt && x.Approved == true);
                        foreach (var item1 in info2)
                        {
                            long Id = item1.Id;
                            var itemInfo = await _selectedItemDiscountService.GetAllItemsByIdAsync(Id);
                            if (itemInfo != null)
                            {
                                if (itemInfo.Where(x => x.ProductId == productId).Count() > 0)
                                {
                                    id = 1; break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (info.Where(x => x.Id != recordId && x.EndDate <= startDt && x.Approved == true).Count() > 0)
                    {
                        var info2 = info.Where(x => x.Id != recordId && x.EndDate <= startDt && x.Approved == true);
                        foreach (var item1 in info2)
                        {
                            long Id = item1.Id;
                            var itemInfo = await _selectedItemDiscountService.GetAllItemsByIdAsync(Id);
                            if (itemInfo != null)
                            {
                                if (itemInfo.Where(x => x.ProductId == productId).Count() > 0)
                                {
                                    id = 1; break;
                                }
                            }
                        }
                    }
                }
            }

            return new JsonResult(new { id = id });
        }



            [Route("/Merchant/GetProductBarcodes")]
        [HttpPost]
        public async Task<IActionResult> GetProductBarcodes(string Prefix)
        {
            var info = await _commonService.GetProductInfoAsync(Prefix);

            var productInfo1 = info.Select(t => new ProductBarcodeAutoComplete()
            {
                Id = t.Id,
                Barcode = t.ProductId.Trim(),
                BarcodeWithName = t.ProductId.Trim() + " -- " + t.ProductNameEng,
            }).ToList();

            return Json(data: productInfo1);
        }

        [Route("/Merchant/DeleteSelectedItemDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteSelectedItemDiscount(long id)
        {
            var info = await _selectedItemDiscountService.GetByIdAsync(id);
            await _selectedItemDiscountService.DeleteAsync(info);
            return new JsonResult(new { id = id });

        }

        [Route("/Merchant/DeleteSelectedItemSingleItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteSelectedItemSingleItem(long id)
        {
            await _selectedItemDiscountService.DeleteSingleItemAsync(id);
            return new JsonResult(new { id = id });

        }

        [Route("/Merchant/ItemUploadForSelectedDiscount")]
        [HttpPost]
        public async Task<IActionResult> ItemUploadForSelectedDiscount(IFormFile upload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/Upload/";
                string imageUploadPath = webRootPath + "/Docs/Products/";

                string docName = null;
                if (upload != null && upload.Length > 0)
                {
                    string curDocName = Path.GetFileName(upload.FileName);
                    string curDocExtention = Path.GetExtension(upload.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await upload.CopyToAsync(stream);
                    }
                }
                Thread.Sleep(1000);
                List<SelecedItemFromExcel> model = new List<SelecedItemFromExcel>();
                if (docName != null)
                {
                    string docPath = Path.Combine(uploadPath, docName);
                    using (var package = new ExcelPackage(new FileInfo(docPath)))
                    {
                        var ws = package.Workbook.Worksheets[0];
                        for (int row = 2; row <= ws.Dimension.End.Row; row++)
                        {
                            string? barcode = ws.Cells[row, 1].Value != null ? ws.Cells[row, 1].Value.ToString() : null;
                            var productInfo = new ProductInfo();
                            var ItemDiscountInfo = new SelecedItemFromExcel();
                            if (!string.IsNullOrEmpty(barcode))
                            {
                                productInfo = await _commonService.GetProductByBarcode(barcode);
                                if (productInfo != null)
                                {
                                    ItemDiscountInfo.ProductId = productInfo.Id;
                                    ItemDiscountInfo.Barcode = productInfo.ProductId;
                                    ItemDiscountInfo.OldPrice = productInfo.OreginalPrice;
                                    var discPer = ws.Cells[row, 2].Value;
                                    var discFix = ws.Cells[row, 3].Value;
                                    ItemDiscountInfo.DiscountPercentage = discPer != null ? Convert.ToInt16(discPer) : 0;
                                    ItemDiscountInfo.DiscountFixed = discFix != null ? Convert.ToDecimal(discFix) : 0;
                                    var newPrice = productInfo.OreginalPrice;
                                    if (discPer != null && Convert.ToInt16(discPer) > 0)
                                    {
                                        var one = Convert.ToDecimal(productInfo.OreginalPrice - (productInfo.OreginalPrice * (Convert.ToInt16(discPer) / 100)));
                                        newPrice = one;
                                        ItemDiscountInfo.DiscountFixed = 0;
                                    }
                                    else if(discFix != null && Convert.ToDecimal(discFix) > 0)
                                    {
                                        if(Convert.ToDecimal(discFix) <= newPrice)
                                        {
                                            newPrice = newPrice - Convert.ToDecimal(discFix);
                                        }
                                        else
                                        {
                                            ItemDiscountInfo.DiscountFixed = 0;
                                        }
                                        ItemDiscountInfo.DiscountPercentage = 0;
                                    }
                                    ItemDiscountInfo.NewPrice = newPrice;

                                    model.Add(ItemDiscountInfo);
                                }
                            }
                        }
                    }
                    return Json(data: model);
                }

                return Json(data: null);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [Route("/Merchant/DownloadSampleForSelectedItems")]
        [HttpGet]
        public ActionResult DownloadSampleForSelectedItems()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Docs/Download/";

            FilePath = System.IO.Path.Combine(FilePath, "Selected_Item_Discount_Format.xlsx");
            var fs = new FileStream(FilePath, FileMode.Open);

            return File(fs, "application/octet-stream", "Selected_Item_Discount_Format.xlsx");
        }

    }
}
