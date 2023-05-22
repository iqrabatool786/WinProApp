using iText.Commons.Actions.Data;
using iText.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.ViewModels;
using WinProApp.ViewModels.Purchase.PurchaseAsset;

namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Purchase")]
    public class PurchaseAssetController : BasedUserController
    {
        public readonly CommonService _commonService;
        public readonly PurchaseAssetService _purchaseAssetService;
        public readonly AssetService _assetServices;
        public readonly PurchaseService _purchaseService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public PurchaseAssetController(CommonService commonService, PurchaseAssetService purchaseAssetService, AssetService assetServices, PurchaseService purchaseService, IWebHostEnvironment webHostEnvironment)
        {
            _commonService = commonService;
            _purchaseAssetService = purchaseAssetService;
            _assetServices = assetServices;
            _purchaseService = purchaseService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Purchase/PurchaseAsset")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Purchase/PurchaseAssetList")]
        [HttpPost]
        public async Task<IActionResult> PurchaseAssetList(JQueryDataTableParamModel param)
        {

            var results = _purchaseAssetService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                SupplierId = x.SupplierId,
                SupplierName = _purchaseService.GetSupplierByIdAsync(x.SupplierId).Result.CompanyName,
                InvoiceNo = x.InvoiceNo,
                Date = x.Date.ToString("yyyy-MM-dd"),
                AttachedDoc = x.AttachedDoc ?? "",
                TransactionType = x.TransactionType,
                Description = x.Description,
                Status = x.Status == true ? "Approved" : "OnHold",
                Discount = x.Discount,
                OtherCharges = x.OtherCharges,
                VatAmount = x.VatAmount,
                Total = x.Total,
                StrDiscount = x.Discount != null ? x.Discount.Value.ToString("0.00") : "",
                StrOtherCharges = x.OtherCharges != null ? x.OtherCharges.Value.ToString("0.00") : "",
                StrVatAmount = x.VatAmount != null ? x.VatAmount.Value.ToString("0.00") : "",
                StrTotal = x.Total != null ? x.Total.Value.ToString("0.00") : "",
                VatNo = x.VatNo,
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

        [Route("/Purchase/CreateAssetInvoice")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();


            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View();
        }

        [Route("/Purchase/CreateAssetInvoice")]
        [HttpPost]
        public async Task<IActionResult> Create(AddViewModel model, IFormFile InvoiceAssetDoc)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string docName = null;
                if (InvoiceAssetDoc != null)
                {
                    string curDocName = Path.GetFileName(InvoiceAssetDoc.FileName);
                    string curDocExtention = Path.GetExtension(InvoiceAssetDoc.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await InvoiceAssetDoc.CopyToAsync(stream);
                    }

                }


                var invoice = new SupplierPurchaseAsset();
                invoice.SupplierId = model.SupplierId;
                invoice.InvoiceNo = model.InvoiceNo;
                invoice.Date = model.Date;
                invoice.AttachedDoc = docName;
                invoice.TransactionType = model.TransactionType;
                invoice.Description = model.Description;
                invoice.Discount = model.Discount;
                invoice.ChargesDescription = model.ChargesDescription;
                invoice.OtherCharges = model.OtherCharges;
                invoice.VatAmount = model.VatAmount;
                invoice.Total = model.Total;
                invoice.VatNo = model.VatNo;
                invoice.Status = false;// model.Status;
                invoice.CratedDate = DateTime.Now;
                invoice.CreatedBy = User.Identity.Name;
                invoice.UpdatedDate = DateTime.Now;
                invoice.UpdatedBy = User.Identity.Name;

                long curInsertId = await _purchaseAssetService.CreateAsync(invoice);

                List<SupplierPurchaseAssetDetails> items = new List<SupplierPurchaseAssetDetails>();



                string[] ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] barcodes = Request.Form["Barcode"];
                string[] descriptions = Request.Form["ItemDescription"];
                string[] descriptionsab = Request.Form["ItemDescriptionAb"];
                
                string[] qtys = Request.Form["ItemQty"];
                string[] prices = Request.Form["itemCost"];
                string[] expireDates = Request.Form["ItemDate1"];
                string[] productDate = Request.Form["ItemDate2"];
                string[] vats = Request.Form["ItemVat"];


                for (int i = 0; i < descriptions.Length; i++)
                {
                    long? productId = null;

                    decimal? qty = null;
                    decimal? price = null;
                    decimal? vat = null;


                    //if (!string.IsNullOrEmpty(productIds[i]))
                    //{
                    //    productId = int.Parse(productIds[i]);
                    //}

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                   
                    if (!string.IsNullOrEmpty(vats[i]))
                    {
                        vat = decimal.Parse(vats[i]);
                    }
                   

                    items.Add(new SupplierPurchaseAssetDetails()
                    {
                        PurchaseId = curInsertId,
                        ProductId = productId,
                        Barcode = barcodes[i],                        
                        DescriptionEng = descriptions[i],
                        DescriptionArabic = descriptionsab[i],
                        Qty = qty,
                        Price = price,
                        Vat = vat,
                        ExpireDate = !string.IsNullOrEmpty(expireDates[i]) ? DateTime.Parse(expireDates[i]) : null,
                        ProductDate = !string.IsNullOrEmpty(productDate[i]) ? DateTime.Parse(productDate[i]) : null,                        
                    });
                }

                // requiestInfo.RequestItems = items;
                await _purchaseAssetService.CreateItemsAsync(curInsertId, items);


                return new JsonResult(new { id = curInsertId });
            }
            catch
            {
                return View();
            }
        }

        [Route("/Purchase/AssetInvoiceDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var invoice = await _purchaseAssetService.GetByIdAsync(id);
            var model = new DetailsViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                InvoiceNo = invoice.InvoiceNo,
                Date = invoice.Date.ToString("yyyy-MM-dd"),
                AttachedDoc = invoice.AttachedDoc,
                TransactionType = invoice.TransactionType,
                Description = invoice.Description,
                Status = invoice.Status == true ? "Approved" : "OnHold",
                StrDiscount = invoice.Discount != null ? invoice.Discount.Value.ToString("0.00") : "0.00",
                StrOtherCharges = invoice.OtherCharges != null ? invoice.OtherCharges.Value.ToString("0.00") : "0.00",
                StrVatAmount = invoice.VatAmount != null ? invoice.VatAmount.Value.ToString("0.00") : "0.00",
                StrTotal = invoice.Total != null ? invoice.Total.Value.ToString("0.00") : "0.00",
                ChargesDescription = invoice.ChargesDescription,
                CreatedBy = invoice.CreatedBy,
                CratedDate = invoice.CratedDate != null ? invoice.CratedDate.Value.ToString("yyyy-MM-dd") : null,
                UpdatedBy = invoice.UpdatedBy,
                UpdatedDate = invoice.UpdatedDate != null ? invoice.UpdatedDate.Value.ToString("yyyy-MM-dd") : null,
                Items = await _purchaseAssetService.GetItemsByRequestIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditAssetInvoice/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();
           
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
            


            var invoice = await _purchaseAssetService.GetByIdAsync(id);
            if (invoice.Status == true)
            {
                return NotFound();
            }
            var model = new EditViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                InvoiceNo = invoice.InvoiceNo,
                Date = invoice.Date,
                AttachedDoc = invoice.AttachedDoc,
                TransactionType = invoice.TransactionType,
                Description = invoice.Description,
                VatAmount = invoice.VatAmount ?? 0,
                Discount = invoice.Discount ?? 0,
                OtherCharges = invoice.OtherCharges ?? 0,
                ChargesDescription = invoice.ChargesDescription,
                Total = invoice.Total,
                Status = invoice.Status
            };


            return View(model);

        }


        [Route("/Purchase/EditAssetInvoice/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(long id, EditViewModel model, IFormFile InvoiceAssetDoc)
        {
            //long id = model.Id;
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string? docName = null;
                if (InvoiceAssetDoc != null)
                {
                    string curDocName = Path.GetFileName(InvoiceAssetDoc.FileName);
                    string curDocExtention = Path.GetExtension(InvoiceAssetDoc.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await InvoiceAssetDoc.CopyToAsync(stream);
                    }

                }

                var invoice = await _purchaseAssetService.GetByIdAsync(id);
                string? oldDoc = invoice.AttachedDoc;

                invoice.SupplierId = model.SupplierId;
                invoice.InvoiceNo = model.InvoiceNo;
                invoice.Date = model.Date;

                if (docName != null && oldDoc != null)
                {
                    string oldDocPath = System.IO.Path.Combine(uploadPath, oldDoc);
                    if (System.IO.File.Exists(oldDocPath))
                    {
                        System.IO.File.Delete(oldDocPath);
                    }
                }

                if (docName != null)
                {
                    invoice.AttachedDoc = docName;
                }
                else
                {
                    invoice.AttachedDoc = oldDoc;
                }

                invoice.TransactionType = model.TransactionType;
                //   invoice.Description = model.Description;
                invoice.VatAmount = model.VatAmount;
                invoice.Discount = model.Discount;
                invoice.OtherCharges = model.OtherCharges;
                invoice.ChargesDescription = model.ChargesDescription;
                invoice.Total = model.Total;
                invoice.Status = model.Status;

                invoice.UpdatedDate = DateTime.Now;
                invoice.UpdatedBy = User.Identity.Name;

                //var result = await _requestForInfoService.UpdateAsync(requiestInfo);

                List<SupplierPurchaseAssetDetails> items = new List<SupplierPurchaseAssetDetails>();

                string[] ids = Request.Form["ItemId"];
                string[] barcodes = Request.Form["Barcode"];
               // string[] productIds = Request.Form["ItemProductId"];
                string[] descriptions = Request.Form["ItemDescription"];
                string[] descriptionsab = Request.Form["ItemDescriptionAb"];

                string[] qtys = Request.Form["ItemQty"];
                string[] prices = Request.Form["itemCost"];
                string[] expireDates = Request.Form["ItemDate1"];
                string[] productDate = Request.Form["ItemDate2"];
                string[] vats = Request.Form["itemVat"];

                for (int i = 0; i < barcodes.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    long? productId = null;
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? vat = null;

                    //if (!string.IsNullOrEmpty(productIds[i]))
                    //{
                    //    productId = long.Parse(productIds[i]);
                    //}

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                if (!string.IsNullOrEmpty(vats[i]))
                {
                    vat = decimal.Parse(vats[i]);
                }

                items.Add(new SupplierPurchaseAssetDetails()
                    {
                        Id = curItemId,
                        PurchaseId = id,
                        ProductId = productId,
                        Barcode = barcodes[i],
                        DescriptionEng = descriptions[i],
                        DescriptionArabic = descriptionsab[i],
                        Qty = qty,
                        Price = price,
                        Vat = vat,
                        ExpireDate = !string.IsNullOrEmpty(expireDates[i]) ? DateTime.Parse(expireDates[i]) : null,
                        ProductDate = !string.IsNullOrEmpty(productDate[i]) ? DateTime.Parse(productDate[i]) : null,
                    });
                }

                await _purchaseAssetService.UpdateAsync(invoice, items);

                return new JsonResult(new { id = id });
        }
            catch (Exception ex)
            {
                throw ex.InnerException;
                // return View(model);
            }
}



        [Route("/Purchase/DeletePurchaseAssetInvoiceItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(long id)
        {
            try
            {
                await _purchaseAssetService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Purchase/DeleteAssetInvoice/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _purchaseAssetService.GetByIdAsync(id);
                await _purchaseAssetService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/GetPurchaseAssetDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPurchaseAssetDetails(long id)
        {
            var info = await _purchaseAssetService.GetItemsByRequestIdAsync(id);

            var detailsViewModels = new List<ItemDetailsViewModel>();
            if (info != null)
            {
                foreach (var item in info)
                {
                    detailsViewModels.Add(new ItemDetailsViewModel()
                    {
                        Id = item.Id,
                        PurchaseId = item.PurchaseId,
                        ProductId = item.ProductId,
                        Barcode = item.Barcode,
                        DescriptionEng = item.DescriptionEng ?? "",
                        DescriptionArabic = item.DescriptionArabic ?? "",
                        Qty = item.Qty,
                        Price = item.Price,
                        Vat = item.Vat,
                        ProductDate = item.ProductDate != null ? item.ProductDate.Value.ToString("yyyy-MM-dd") : "",
                        ExpireDate = item.ExpireDate != null ? item.ExpireDate.Value.ToString("yyyy-MM-dd") : "",
                    });
                }
            }


            return Json(data: detailsViewModels);
        }
    }
}
