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
using WinProApp.ViewModels;
using WinProApp.ViewModels.PurchaseReciept;

namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Purchase,Warehouse")]
    public class PurchaseRecieptController : BasedUserController
    {
        public readonly PurchaseRecieptService _purchaseRecieptService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        public readonly PurchaseService _purchaseService;
        public readonly StoreService _storeService;
        public readonly SupplierReturnService _supplierReturnService;
        public readonly CommonService _commonService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public PurchaseRecieptController(PurchaseRecieptService purchaseRecieptService, SupplierPurchaseService supplierPurchaseService, PurchaseService purchaseService, StoreService storeService, CommonService commonService, SupplierReturnService supplierReturnService, IWebHostEnvironment webHostEnvironment)
        {
            _purchaseRecieptService = purchaseRecieptService;
            _supplierPurchaseService = supplierPurchaseService;
            _purchaseService = purchaseService;
            _storeService = storeService;
            _commonService = commonService;
            _supplierReturnService = supplierReturnService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/PurchaseReceipts")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/PurchaseReceiptList")]
        [HttpPost]
        public async Task<IActionResult> PurchaseReceiptList(JQueryDataTableParamModel param)
        {

            var results = _purchaseRecieptService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                InvoiceId= x.InvoiceId,
                InvoiceNo = x.InvoiceId > 0? _supplierPurchaseService.GetByIdAsync(x.InvoiceId).Result.InvoiceNo:"",
                SupplierId = x.SupplierId,
                SupplierName = _purchaseService.GetSupplierByIdAsync(x.SupplierId).Result.CompanyName,
                Date = x.Date !=null? x.Date.Value.ToString("yyyy-MM-dd"):"",
                InvoiceDoc = x.InvoiceId > 0 ? (_supplierPurchaseService.GetByIdAsync(x.InvoiceId).Result.AttachedDoc != null? _supplierPurchaseService.GetByIdAsync(x.InvoiceId).Result.AttachedDoc:"") : "",
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


        [Route("/Warehouse/UpdateReceipt/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateReceipt(long id)
        {
            var vat = await _commonService.GetVatAsync();
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            var invoice = await _purchaseRecieptService.GetByIdAsync(id);
            if (invoice.Status == true)
            {
                return NotFound();
            }
            var model = new EditViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                InvoiceId= invoice.InvoiceId,
                InvoiceNo = _supplierPurchaseService.GetByIdAsync(invoice.InvoiceId).Result.InvoiceNo,
                Date = invoice.Date,
                AttachedDoc = invoice.AttachedDoc,
                InvoiceDoc = _supplierPurchaseService.GetByIdAsync(invoice.InvoiceId).Result.AttachedDoc,
                TransactionType = invoice.TransactionType,
                Description = invoice.Description,
                VatAmount = invoice.VatAmount ?? 0,
                Discount = invoice.Discount ?? 0,
                OtherCharges = invoice.OtherCharges ?? 0,
                ChargesDescription = invoice.ChargesDescription,
                Total = invoice.Total,
                Status = invoice.Status,
                StoreId = invoice.StoreId,
                Items = await _purchaseRecieptService.GetItemsByRequestIdAsync(id),
                IsReturn=false,
            };


            return View(model);

        }


        [Route("/Warehouse/UpdateReceipt")]
        [HttpPost]
        public async Task<IActionResult> UpdateReceipt(EditViewModel model, IFormFile InvoiceDoc)
        {
            long id = model.Id;
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string? docName = null;
                if (InvoiceDoc != null)
                {
                    string curDocName = Path.GetFileName(InvoiceDoc.FileName);
                    string curDocExtention = Path.GetExtension(InvoiceDoc.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await InvoiceDoc.CopyToAsync(stream);
                    }

                }

                var productInvoice = await _supplierPurchaseService.GetByIdAsync(model.InvoiceId);

                var invoice = await _purchaseRecieptService.GetByIdAsync(id);
                string? oldDoc = invoice.AttachedDoc;

                invoice.Id = id;
                invoice.TransactionType = model.TransactionType;

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
                invoice.Discount = model.Discount;
                invoice.OtherCharges = model.OtherCharges;
                invoice.ChargesDescription = model.ChargesDescription;

                //   invoice.Description = model.Description;
                if (model.IsReturn == true)
                {
                    invoice.VatAmount = productInvoice.VatAmount;
                    invoice.Total = productInvoice.Total;
                    invoice.IsReturn = true;
                }
                else
                {
                    invoice.VatAmount = model.VatAmount;
                    invoice.Total = model.Total;
                    invoice.IsReturn = false;
                }

                invoice.Status = model.Status;

                invoice.UpdatedDate = DateTime.Now;
                invoice.UpdatedBy = User.Identity.Name;

              //  var result = await _purchaseRecieptService.UpdateReceiptAsync(invoice);

                List<DetailsUpdateViewModel> items = new List<DetailsUpdateViewModel>();
                List<SupplierReturnDetail> returnItems = new List<SupplierReturnDetail>();

                string[] ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] itembarcode = Request.Form["Barcode"];

                string[] qtyDozens1 = Request.Form["QtyDozen"];
                string[] qtys1 = Request.Form["QtyPices"];

                string[] qtyDozens = Request.Form["itemRQtyD"];
                string[] qtys = Request.Form["itemRQtyP"];

                string[] prices = Request.Form["itemOrgPrice"];

                string[] unitCosts = Request.Form["itemCost"];
                string[] unitSalePrices = Request.Form["itemRetails"];
                string[] orgPrices = Request.Form["itemOrgPrice2"];

                string[] vats = Request.Form["ItemSellingVat"];
                string[] pVats = Request.Form["OrgSellingVat"];

                for (int i = 0; i < ids.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    long productId = 0;
                    decimal? qtyDozen1 = 0;
                    decimal? qty1 = 0;
                    decimal? diffd = 0;
                    decimal? diffp = 0;

                    decimal? qtyDozen = null;
                    decimal? qty = null;
                    decimal? qtyDozenX = null;
                    decimal? qtyX = null;
                    decimal? price = null;
                    decimal? saleprice = null;
                    decimal? unitCost = null;
                    decimal? boxRetail = null;
                    decimal? orgPrice = null;
                    decimal? vat = null;
                    decimal? pvat = null;

                    if (!string.IsNullOrEmpty(productIds[i]) && productIds[i] != "0")
                    {
                        productId = long.Parse(productIds[i]);
                        if(productId == 0)
                        {
                            productId = await _commonService.GetProductIdByBarcodeAsync(itembarcode[i]);
                        }
                    }
                    else
                    {
                        productId = await _commonService.GetProductIdByBarcodeAsync(itembarcode[i]);
                    }

                    if (!string.IsNullOrEmpty(qtyDozens1[i]))
                    {
                        qtyDozen1 = decimal.Parse(qtyDozens1[i]);
                    }

                    if (!string.IsNullOrEmpty(qtys1[i]))
                    {
                        qty1 = decimal.Parse(qtys1[i]);
                    }


                    if (!string.IsNullOrEmpty(qtyDozens[i]))
                    {
                        qtyDozen = decimal.Parse(qtyDozens[i]);
                    }

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                    if (!string.IsNullOrEmpty(unitSalePrices[i]))
                    {
                        saleprice = decimal.Parse(unitSalePrices[i]);
                    }
                    if (!string.IsNullOrEmpty(unitCosts[i]))
                    {
                        unitCost = decimal.Parse(unitCosts[i]);
                    }
                    if (!string.IsNullOrEmpty(orgPrices[i]))
                    {
                        orgPrice = decimal.Parse(orgPrices[i]);
                    }
                    
                    if (!string.IsNullOrEmpty(vats[i]))
                    {
                        vat = decimal.Parse(vats[i]);
                    }
                    if (!string.IsNullOrEmpty(pVats[i]))
                    {
                        pvat = decimal.Parse(pVats[i]);
                    }

                        qtyDozenX = qtyDozen;
                        qtyX = qty;
                    

                    items.Add(new DetailsUpdateViewModel()
                    {
                        Id = curItemId,
                        ReceiptId = id,
                        ProductId=productId,
                        ItemBarcode = itembarcode[i],
                        QtyDozen = qtyDozen1,
                        QtyPices= qty1,
                        ReceiveQtyDozen = qtyDozen,
                        ReceiveQtyPices = qty,
                        ReceivePrice = unitCost,
                        Price = price,
                        CostPrice = unitCost,
                        SalePrice = saleprice,
                        BoxRetail = boxRetail,
                        OrgPrice= orgPrice,
                        Vat = vat,
                        PVat = pvat,
                    });

                    diffd = qtyDozen1 - qtyDozen;
                    diffp = qty1 - qty;

                    if (diffd > 0 || diffp > 0)
                    {
                        returnItems.Add(new SupplierReturnDetail()
                        {
                            ProductId= productId,
                            Barcode = itembarcode[i],
                            QtyDozen = diffd,
                            QtyPices = diffp,
                            Price = unitCost,
                            Vat = pvat,
                        });
                    }

                }

              var purchaseInfo =  await _purchaseRecieptService.UpdateAsync(invoice, items);
                if (model.Status == true)
                {
                    await _purchaseRecieptService.UpdateProductInfoAsync(id, items);
                }

                if(model.IsReturn == true)
                {
                    decimal returnInvTotVat = productInvoice.VatAmount.Value > 0? productInvoice.VatAmount.Value:0;
                    decimal returnRecTotVat = purchaseInfo.VatAmount.Value >0? model.VatAmount.Value:0;
                    decimal returnTotVat = (returnInvTotVat - returnRecTotVat);

                    decimal returnInvTot = productInvoice.Total.Value>0 ? productInvoice.Total.Value:0;
                    decimal returnRecTot = purchaseInfo.Total.Value > 0 ? model.Total.Value:0;
                    decimal returnTot = (returnInvTot - returnRecTot);

                    var returnP = new SupplierReturn();
                    returnP.SupplierId = purchaseInfo.SupplierId;
                    returnP.InvoiceId= purchaseInfo.InvoiceId;
                    returnP.Date = invoice.Date;
                    returnP.Status = false;
                    returnP.VatAmount = returnTotVat;
                    returnP.Total= returnTot;
                    returnP.StoreId = purchaseInfo.StoreId;
                    returnP.CratedDate = DateTime.Now;
                    returnP.CreatedBy = User.Identity.Name;
                    returnP.UpdatedDate = DateTime.Now;
                    returnP.UpdatedBy = User.Identity.Name;

                   var sReturn = await _supplierReturnService.CreateAsync(returnP);

                   await _supplierReturnService.CreateItemsAsync(sReturn.Id, returnItems);

                }

                return new JsonResult(new { id = id });
        }
            catch (Exception ex)
            {
                throw ex.InnerException;
                // return View(model);
            }
}


        [Route("/Warehouse/ViewReceipt/{id}")]
        [HttpGet]
        public async Task<IActionResult> ViewReceipt(long id)
        {
            var suppliers = await _purchaseService.GetAllSuppliersAsync();

            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");

            var invoice = await _purchaseRecieptService.GetByIdAsync(id);

            var model = new EditViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                InvoiceNo = _supplierPurchaseService.GetByIdAsync(invoice.InvoiceId).Result.InvoiceNo,
                Date = invoice.Date,
                AttachedDoc = invoice.AttachedDoc,
                InvoiceDoc= _supplierPurchaseService.GetByIdAsync(invoice.InvoiceId).Result.AttachedDoc,
                TransactionType = invoice.TransactionType,
                Description = invoice.Description,
                VatAmount = invoice.VatAmount ?? 0,
                Discount = invoice.Discount ?? 0,
                OtherCharges = invoice.OtherCharges ?? 0,
                ChargesDescription = invoice.ChargesDescription,
                Total = invoice.Total,
                Status = invoice.Status,
                StoreId = invoice.StoreId,
                Items = await _purchaseRecieptService.GetItemsByRequestIdAsync(id)
            };


            return View(model);

        }


        [Route("/Warehouse/GetReceiptDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetReceiptDetails(long id)
        {
            var info = await _purchaseRecieptService.GetItemsByRequestIdAsync(id);

            var detailsViewModels = new List<DetailsViewModel>();
            if (info != null)
            {
                foreach (var item in info)
                {
                    detailsViewModels.Add(new DetailsViewModel()
                    {
                        Id = item.Id,
                        ReceiptId = item.ReceiptId,
                        ProductId = item.ProductId,
                        Barcode = item.Barcode,
                        DescriptionId = item.DescriptionId,
                        DescriptionEng = item.DescriptionEng ?? "",
                        DescriptionArabic = item.DescriptionArabic ?? "",
                        CategoryId = item.CategoryId,
                        CategoryName = item.CategoryId != null ? await _commonService.GetCategoryNameByIdAsync(item.CategoryId.Value) : "",
                        YearId = item.YearId,
                        SkuId= item.SkuId,
                        SkuCode = item.SkuId != null && item.SkuId !=0? await _commonService.GetSkuCodeByIdAsync(item.SkuId.Value) :"",
                        QtyDozen = item.QtyDozen,
                        QtyPices = item.QtyPices,
                        Price = item.Price,
                        ReceiveQtyDozen = item.ReceiveQtyDozen,
                        ReceiveQtyPices = item.ReceiveQtyPices,
                        ReceivePrice = item.ReceivePrice,
                        CostPrice =item.CostPrice,
                        SalePrice=item.SalePrice,
                        Vat = item.Vat,
                        PVat= item.PVat,
                    });
                }
            }


            return Json(data: detailsViewModels);
        }


    }
}
