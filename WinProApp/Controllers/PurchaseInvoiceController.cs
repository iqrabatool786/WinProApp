using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.Commons.Actions.Data;
using iText.Html2pdf;
using iText.Layout;
using iText.StyledXmlParser.Jsoup;
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
using WinProApp.ViewModels.SupplierPurchase;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Purchase")]
    public class PurchaseInvoiceController : BasedUserController
    {
        public readonly CommonService _commonService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        public readonly PurchaseService _purchaseService;
        public readonly StyleServices _styleServices;
        public readonly YearsService _yearsService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public PurchaseInvoiceController(CommonService commonService, SupplierPurchaseService supplierPurchaseService, PurchaseService purchaseService, StyleServices styleServices, YearsService yearsService, IWebHostEnvironment webHostEnvironment)
        {
            _commonService = commonService;
            _supplierPurchaseService = supplierPurchaseService;
            _purchaseService = purchaseService;
            _styleServices = styleServices;
            _yearsService = yearsService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Purchase/PurchaseInvoices")]
        [HttpGet]
        public IActionResult PurchaseInvoices()
        {
            return View();
        }

        [Route("/Purchase/PurchaseInvoiceList")]
        [HttpPost]
        public async Task<IActionResult> PurchaseInvoiceList(JQueryDataTableParamModel param)
        {

            var results = _supplierPurchaseService.GetList(param).Result.Select(x => new ListViewModel()
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
                StrDiscount = x.Discount!=null ? x.Discount.Value.ToString("0.00") : "",
                StrOtherCharges=x.OtherCharges!=null?x.OtherCharges.Value.ToString("0.00") : "",
                StrVatAmount=x.VatAmount!=null?x.VatAmount.Value.ToString("0.00") : "",
                StrTotal=x.Total!=null?x.Total.Value.ToString("0.00") : "",
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

        public System.Globalization.CultureInfo GetRequestCulture()
        {
            return RequestCulture;
        }

        [AllowAnonymous, AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsInvoiceNoWithSupplierExists(string InvoiceNo, long SupplierId, long Id = 0)
        {
            if (!string.IsNullOrEmpty(InvoiceNo))
            {
                var response = await _supplierPurchaseService.IsInvoiceNoWithSupplierExists(InvoiceNo, SupplierId, Id);
                if (response) return Json(response);
                else return Json("InvoiceNo already exists with this supplier");
            }
            else return Json(true);
        }

        [Route("/Purchase/CreateInvoice")]
        [HttpGet]
        public async Task<IActionResult> CreateInvoice(System.Globalization.CultureInfo requestCulture)
        {
            var suppliers = await _purchaseService.GetAllSuppliersWithNamesAndIdsAsync();
            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            var categories = await _commonService.GetCategoriesAsync();
            var colors = await _commonService.GetColorsAsync();
            var sizes = await _commonService.GetSizesAsync();
            var departments = await _commonService.GetDepartmentsAsync();
            var seassons = await _commonService.GetSeassonsAsync();
            var vendors = await _commonService.GetVendorsAsync();
            var descs = await _commonService.GetDescriptionsAsync();
            var brands = await _commonService.GetBrandsAsync();
            var groups = await _commonService.GetGroupsAsync();

            var styles = await _commonService.GetStylesAsync();

            List<CategoryViewModel> catList = new List<CategoryViewModel>();
            var parentCats = categories.Where(c => c.ParentCategoryId == 0).ToList();

            foreach(var catItem in parentCats)
            {
               var curCats = new CategoryViewModel();
                curCats.Id= catItem.Id;
                curCats.ParentId = catItem.ParentCategoryId;
                curCats.CategoryNameEng = catItem.NameEng;
                curCats.CategoryNameArabic = catItem.NameArabic;
                catList.Add(curCats);

              await _commonService.GetCategoryHierarchy(catItem.Id, catList, catList, 0);
            }

            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.Units = new SelectList(units, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
            ViewBag.Categories = new SelectList(catList, "Id", RequestCulture.Name == "ar" ? "CategoryNameArabic" : "CategoryNameEng");
            ViewBag.Colors = new SelectList(colors, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Sizes = new SelectList(sizes, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Departments = new SelectList(departments, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Seassons = new SelectList(seassons, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");

            ViewBag.Styles = new SelectList(styles, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Vendores = new SelectList(vendors, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Descriptions = new SelectList(descs, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Brands = new SelectList(brands, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Groups = new SelectList(groups, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");

            return View();
        }

        [Route("/Purchase/CreateInvoice")]
        [HttpPost]
        public async Task<IActionResult> CreateInvoice(AddViewModel model, IFormFile AttachedDoc)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string docName = null;
                if (AttachedDoc != null && AttachedDoc.Length > 0)
                {
                    string curDocName = Path.GetFileName(AttachedDoc.FileName);
                    string curDocExtention = Path.GetExtension(AttachedDoc.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await AttachedDoc.CopyToAsync(stream);
                    }
                }

                var invoice = new SupplierPurchase();
                invoice.SupplierId = model.SupplierId;
                invoice.InvoiceNo = model.InvoiceNo;
                invoice.Date = model.Date;
                invoice.AttachedDoc = docName;
                invoice.TransactionType = model.TransactionType;
               // invoice.Description = model.Description;
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

                long curInsertId = await _supplierPurchaseService.CreateAsync(invoice);

                List<SupplierPurchaseDetails> items = new List<SupplierPurchaseDetails>();

                string[] ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] barcodes = Request.Form["Barcode"];
                string[] descriptions = Request.Form["ItemDescription"];
                string[] descriptionsab = Request.Form["ItemDescriptionAb"];
                string[] categories = Request.Form["itemCategory"];
                string[] seassons = Request.Form["itemSeasson"];
                string[] depts = Request.Form["itemDepartment"];
                string[] skus = Request.Form["itemSku"];
                string[] skuText = Request.Form["itemSkuText"];
                string[] sizes = Request.Form["itemSize"];
                string[] colors = Request.Form["itemColor"];
                string[] descriptionIds = Request.Form["itemDescriptionId"];
                string[] brands = Request.Form["itemBrand"];
                string[] vendors = Request.Form["itemVendor"];
                string[] years = Request.Form["itemYear"];
                string[] yearText = Request.Form["itemYearText"];
                string[] groups = Request.Form["itemGroup"];
                string[] unitIds = Request.Form["ItemUnitId"];

                string[] qtyDozens = Request.Form["QtyDozen"];
                string[] qtys = Request.Form["QtyPices"];
                string[] prices = Request.Form["itemCost"];
                string[] expireDates = Request.Form["ExpireDate"];
                string[] productDate = Request.Form["ProductDate"];

                string[] unitCosts = Request.Form["itemCost"];
                string[] unitSalePrices = Request.Form["itemSelling"];

                string[] vats = Request.Form["SellingVat"];
                string[] pVats = Request.Form["ProductVat"];

                string[] categoriesText = null;
                string[] seassonsText = null;
                string[] deptsText = null;
                string[] sizesText = null;
                string[] colorsText = null;

                for (int i = 0; i < ids.Length; i++)
                {
                    long? productId = null;
                    int? catId = null;
                    int? seassonId = null;
                    int? deptsId = null;
                    int? skusId = null;
                    int? sizesId = null;
                    int? colorsId = null;
                    int? descriptionId = null;
                    int? brandId = null;
                    int? yearId = null;
                    int? vendorId = null;
                    int? unitId = null;
                    int? groupId = null;

                    decimal? qtyDozen = null;
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? saleprice = null;
                    int? flagPricePerPicesX = null;
                    decimal? qtyPerUnitsX = null;
                    decimal? unitCost = null;
                    decimal? unitSalePrice = null;
                    decimal? boxRetail = null;
                    decimal? vat = null;
                    decimal? pvat = null;

                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = int.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(categories[i]))
                    {
                        catId = int.Parse(categories[i]);
                    }

                    if (!string.IsNullOrEmpty(seassons[i]))
                    {
                        seassonId = int.Parse(seassons[i]);
                    }

                    if (!string.IsNullOrEmpty(depts[i]))
                    {
                        deptsId = int.Parse(depts[i]);
                    }

                    if (!string.IsNullOrEmpty(skus[i]) && skus[i] != "0")
                    {
                        skusId = int.Parse(skus[i]);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(skuText[i]))
                        {
                            string strText = skuText[i];
                            if (!string.IsNullOrEmpty(strText))
                            {
                                var style = new Styles();
                                style.Code = strText;
                                style.NameEng = strText;
                                style.NameArabic = strText;

                                var styleResult = await _styleServices.CreateAsync(style);
                                skusId = styleResult.Id;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes[i]))
                    {
                        sizesId = int.Parse(sizes[i]);
                    }

                    if (!string.IsNullOrEmpty(colors[i]))
                    {
                        colorsId = int.Parse(colors[i]);
                    }

                    if (!string.IsNullOrEmpty(descriptionIds[i]))
                    {
                        descriptionId = int.Parse(descriptionIds[i]);
                    }
                    if (!string.IsNullOrEmpty(brands[i]))
                    {
                        brandId = int.Parse(brands[i]);
                    }
                    if (!string.IsNullOrEmpty(years[i]) && years[i] != "0")
                    {
                        yearId = int.Parse(years[i]);
                    }
                    else
                    {
                        string strYearText = yearText[i];
                        if (!string.IsNullOrEmpty(strYearText))
                        {
                            var yearx = new Years();
                            yearx.YearName = int.Parse(strYearText);
                            var styleResult = await _yearsService.CreateAsync(yearx);
                            yearId = styleResult.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(vendors[i]))
                    {
                        vendorId = int.Parse(vendors[i]);
                    }

                    if (!string.IsNullOrEmpty(unitIds[i]))
                    {
                        unitId = int.Parse(unitIds[i]);
                    }

                    if (!string.IsNullOrEmpty(groups[i]))
                    {
                        groupId = int.Parse(groups[i]);
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
                    if (!string.IsNullOrEmpty(vats[i]))
                    {
                        vat = decimal.Parse(vats[i]);
                    }
                    if (!string.IsNullOrEmpty(pVats[i]))
                    {
                        pvat = decimal.Parse(pVats[i]);
                    }

                    items.Add(new SupplierPurchaseDetails()
                    {
                        SupplierPurchaseId = curInsertId,
                        ProductId = productId,
                        Barcode = barcodes[i],
                        CategoryId = catId,
                        DepartmentId = deptsId,
                        SeassonId = seassonId,
                        DescriptionId = descriptionId,
                        SkuId = skusId,
                        SizeId = sizesId,
                        ColorId = colorsId,
                        UnitId = unitId,
                        BrandId = brandId,
                        VendorId = vendorId,
                        GroupId = groupId,
                        YearId = yearId,
                        DescriptionEng = descriptions[i],
                        DescriptionArabic = descriptionsab[i],
                        QtyDozen = qtyDozen,
                        QtyPices = qty,
                        Price = price,
                        ExpireDate = !string.IsNullOrEmpty(expireDates[i]) ? DateTime.Parse(expireDates[i]) : null,
                        ProductDate = !string.IsNullOrEmpty(productDate[i]) ? DateTime.Parse(productDate[i]) : null,
                        FlagPricePerPices = flagPricePerPicesX,
                        UnitBarcode = null,// unitBarcodes[i],
                        QtyPerUnit = qtyPerUnitsX,
                        UnitCost = unitCost,
                        UnitSalePrice = saleprice,
                        BoxRetail = boxRetail,
                        UnitDescription = descriptions[i],// unitDescriptions[i],
                        Vat = vat,
                        PVat = pvat,
                    });
                }
                await _supplierPurchaseService.CreateItemsAsync(curInsertId, items);
                return new JsonResult(new { id = curInsertId });
            }
            catch
            {
                return View();
            }
        }

        [Route("/Purchase/InvoiceDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> InvoiceDetails(long id)
        {
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var invoice = await _supplierPurchaseService.GetByIdAsync(id);
            var vat = await _commonService.GetVatAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
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
                StrDiscount=invoice.Discount != null?invoice.Discount.Value.ToString("0.00"):"0.00",
                StrOtherCharges = invoice.OtherCharges != null ? invoice.OtherCharges.Value.ToString("0.00") : "0.00",
                StrVatAmount = invoice.VatAmount != null ? invoice.VatAmount.Value.ToString("0.00") : "0.00",
                StrTotal = invoice.Total != null ? invoice.Total.Value.ToString("0.00") : "0.00",
                ChargesDescription=invoice.ChargesDescription,
                CreatedBy = invoice.CreatedBy,
                CratedDate = invoice.CratedDate !=null? invoice.CratedDate.Value.ToString("yyyy-MM-dd"):null,
                UpdatedBy = invoice.UpdatedBy,
                UpdatedDate = invoice.UpdatedDate !=null?invoice.UpdatedDate.Value.ToString("yyyy-MM-dd"):null,
                Items = await _supplierPurchaseService.GetItemsByRequestIdAsync(id)
            };
            return View(model);
        }


        [Route("/Purchase/EditInvoice/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditInvoice(long id)
        {
            var suppliers = await _purchaseService.GetAllSuppliersWithNamesAndIdsAsync();
            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            var categories = await _commonService.GetCategoriesAsync();
            var colors = await _commonService.GetColorsAsync();
            var sizes = await _commonService.GetSizesAsync();
            var departments = await _commonService.GetDepartmentsAsync();
            var seassons = await _commonService.GetSeassonsAsync();
            var vendors = await _commonService.GetVendorsAsync();
            var descs = await _commonService.GetDescriptionsAsync();
            var brands = await _commonService.GetBrandsAsync();
            var groups = await _commonService.GetGroupsAsync();

            var styles = await _commonService.GetStylesAsync();

            List<CategoryViewModel> catList = new List<CategoryViewModel>();
            var parentCats = categories.Where(c => c.ParentCategoryId == 0).ToList();

            foreach (var catItem in parentCats)
            {
                var curCats = new CategoryViewModel();
                curCats.Id = catItem.Id;
                curCats.ParentId = catItem.ParentCategoryId;
                curCats.CategoryNameEng = catItem.NameEng;
                curCats.CategoryNameArabic = catItem.NameArabic;
                catList.Add(curCats);

                await _commonService.GetCategoryHierarchy(catItem.Id, catList, catList, 0);
            }

            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.Units = new SelectList(units, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
            ViewBag.Categories = new SelectList(catList, "Id", RequestCulture.Name == "ar" ? "CategoryNameArabic" : "CategoryNameEng");
            ViewBag.Colors = new SelectList(colors, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Sizes = new SelectList(sizes, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Departments = new SelectList(departments, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Seassons = new SelectList(seassons, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Styles = new SelectList(styles, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Vendores = new SelectList(vendors, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Descriptions = new SelectList(descs, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Brands = new SelectList(brands, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Groups = new SelectList(groups, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");

            var invoice = await _supplierPurchaseService.GetByIdAsync(id);
            if(invoice.Status == true)
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
                VatAmount= invoice.VatAmount??0,
                Discount = invoice.Discount ?? 0,
                OtherCharges = invoice.OtherCharges ?? 0,
                ChargesDescription= invoice.ChargesDescription,
                Total= invoice.Total,
                Status = invoice.Status,
                Items = await _supplierPurchaseService.GetItemsByRequestIdAsync(id)
            };

            return View(model);
        }


        [Route("/Purchase/EditInvoice")]
        [HttpPost]
        public async Task<IActionResult> EditInvoice(EditViewModel model, IFormFile InvoiceDoc)
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

                var invoice = await _supplierPurchaseService.GetByIdAsync(id);
                string? oldDoc = invoice.AttachedDoc;
                //   bool oldApproved = invoice.Status;
                invoice.Id = id;
                invoice.SupplierId = model.SupplierId;
                invoice.InvoiceNo = model.InvoiceNo;
                invoice.Date = model.Date;

                if(docName != null && oldDoc != null)
                {
                    string oldDocPath = System.IO.Path.Combine(uploadPath, oldDoc);
                    if (System.IO.File.Exists(oldDocPath))
                    {
                        System.IO.File.Delete(oldDocPath);
                    }
                }

                if(docName != null)
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

                List<SupplierPurchaseDetails> items = new List<SupplierPurchaseDetails>();

                string[] ids = Request.Form["ItemId"];
                string[] barcodes = Request.Form["Barcode"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] descriptions = Request.Form["ItemDescription"];
                string[] descriptionsab = Request.Form["ItemDescriptionAb"];
                string[] categories = Request.Form["itemCategory"];
                string[] seassons = Request.Form["itemSeasson"];
                string[] depts = Request.Form["itemDepartment"];
                string[] skus = Request.Form["itemSku"];
                string[] skuText = Request.Form["itemSkuText"];
                string[] sizes = Request.Form["itemSize"];
                string[] colors = Request.Form["itemColor"];
                string[] descriptionIds = Request.Form["itemDescriptionId"];
                string[] brands = Request.Form["itemBrand"];
                string[] vendors = Request.Form["itemVendor"];
                string[] years = Request.Form["itemYear"];
                string[] yearText = Request.Form["itemYearText"];
                string[] groups = Request.Form["itemGroup"];
                string[] unitIds = Request.Form["ItemUnitId"];

                string[] qtyDozens = Request.Form["QtyDozen"];
                string[] qtys = Request.Form["QtyPices"];
                string[] prices = Request.Form["itemCost"];
                string[] expireDates = Request.Form["ExpireDate"];
                string[] productDate = Request.Form["ProductDate"];

                string[] unitCosts = Request.Form["itemCost"];
                string[] unitSalePrices = Request.Form["itemSelling"];

                string[] vats = Request.Form["SellingVat"];
                string[] pVats = Request.Form["ProductVat"];

                string[] categoriesText = Request.Form["itemCategoryText"];
                string[] seassonsText = Request.Form["itemSeassonText"];
                string[] deptsText = Request.Form["itemDepartmentText"];
                string[] sizesText = Request.Form["itemSizText"];
                string[] colorsText = Request.Form["itemColorText"];


                for (int i = 0; i < ids.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    long? productId = null;
                    int? catId = null;
                    int? seassonId = null;
                    int? deptsId = null;
                    int? skusId = null;
                    int? sizesId = null;
                    int? colorsId = null;
                    int? descriptionId = null;
                    int? brandId = null;
                    int? yearId = null;
                    int? vendorId = null;
                    int? unitId = null;
                    int? groupId = null;

                    decimal? qtyDozen = null;
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? saleprice = null;
                    int? flagPricePerPicesX = null;
                    decimal? qtyPerUnitsX = null;
                    decimal? unitCost = null;
                    decimal? unitSalePrice = null;
                    decimal? boxRetail = null;
                    decimal? vat = null;
                    decimal? pvat = null;

                    
                    if (!string.IsNullOrEmpty(productIds[i]) && productIds[i].ToLower() != "null")
                    {
                        productId = long.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(categories[i]) && categories[i].ToLower() != "null")
                    {
                        catId = int.Parse(categories[i]);
                    }

                    if (!string.IsNullOrEmpty(seassons[i]) && seassons[i].ToLower() != "null")
                    {
                        seassonId = int.Parse(seassons[i]);
                    }

                    if (!string.IsNullOrEmpty(depts[i]) && depts[i].ToLower() != "null")
                    {
                        deptsId = int.Parse(depts[i]);
                    }

                    if (!string.IsNullOrEmpty(skus[i]) && skus[i] != "0" && skus[i].ToLower() != "null")
                    {
                        skusId = int.Parse(skus[i]);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(skuText[i]))
                        {
                            string strText = skuText[i];
                            if (!string.IsNullOrEmpty(strText) && strText.ToLower() != "null")
                            {
                                var style = new Styles();
                                style.Code = strText;
                                style.NameEng = strText;
                                style.NameArabic = strText;

                                var styleResult = await _styleServices.CreateAsync(style);
                                skusId = styleResult.Id;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes[i]) && sizes[i].ToLower() != "null")
                    {
                        sizesId = int.Parse(sizes[i]);
                    }

                    if (!string.IsNullOrEmpty(colors[i]) && colors[i].ToLower() != "null")
                    {
                        colorsId = int.Parse(colors[i]);
                    }

                    if (!string.IsNullOrEmpty(descriptionIds[i]) && descriptionIds[i].ToLower() != "null")
                    {
                        descriptionId = int.Parse(descriptionIds[i]);
                    }
                    if (!string.IsNullOrEmpty(brands[i]) && brands[i].ToLower() != "null")
                    {
                        brandId = int.Parse(brands[i]);
                    }
                    if (!string.IsNullOrEmpty(years[i]) && years[i] != "0" && years[i].ToLower() != "null")
                    {
                        yearId = int.Parse(years[i]);
                    }
                    else
                    {
                        string strYearText = yearText[i];
                        if (!string.IsNullOrEmpty(strYearText) && strYearText.ToLower() != "null")
                        {
                            var yearx = new Years();
                            yearx.YearName = int.Parse(strYearText);
                            var styleResult = await _yearsService.CreateAsync(yearx);
                            yearId = styleResult.Id;
                        }
                    }

                    if (!string.IsNullOrEmpty(vendors[i]) && vendors[i].ToLower() != "null")
                    {
                        vendorId = int.Parse(vendors[i]);
                    }

                    if (!string.IsNullOrEmpty(unitIds[i]) && unitIds[i].ToLower() != "null")
                    {
                        unitId = int.Parse(unitIds[i]);
                    }

                    if (!string.IsNullOrEmpty(groups[i]) && groups[i].ToLower() != "null")
                    {
                        groupId = int.Parse(groups[i]);
                    }

                    if (!string.IsNullOrEmpty(qtyDozens[i]) && qtyDozens[i].ToLower() != "null")
                    {
                        qtyDozen = decimal.Parse(qtyDozens[i]);
                    }

                    if (!string.IsNullOrEmpty(qtys[i]) && qtys[i].ToLower() != "null")
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]) && prices[i].ToLower() != "null")
                    {
                        price = decimal.Parse(prices[i]);
                    }
                    if (!string.IsNullOrEmpty(unitSalePrices[i]) && unitSalePrices[i].ToLower() != "null")
                    {
                        saleprice = decimal.Parse(unitSalePrices[i]);
                    }
                    if (!string.IsNullOrEmpty(unitCosts[i]) && unitCosts[i].ToLower() != "null")
                    {
                        unitCost = decimal.Parse(unitCosts[i]);
                    }
                    if (!string.IsNullOrEmpty(vats[i]) && vats[i].ToLower() != "null")
                    {
                        vat = decimal.Parse(vats[i]);
                    }
                    if (!string.IsNullOrEmpty(pVats[i]) && pVats[i].ToLower() != "null")
                    {
                        pvat = decimal.Parse(pVats[i]);
                    }

                    if ((catId == null || catId == 0) && !string.IsNullOrEmpty(categoriesText[i]) && categoriesText[i].ToLower() != "null")
                    {
                        catId = await _commonService.CheckCreateGetProductCategory(categoriesText[i]);
                    }

                    if ((seassonId == null || seassonId == 0) && !string.IsNullOrEmpty(seassonsText[i]) && seassonsText[i].ToLower() != "null")
                    {
                        seassonId = await _commonService.CheckCreateGetProductSeasson(seassonsText[i]);
                    }

                    if ((deptsId == null || deptsId == 0) && !string.IsNullOrEmpty(deptsText[i]) && deptsText[i].ToLower() != "null")
                    {
                        deptsId = await _commonService.CheckCreateGetProductDepartment(deptsText[i]);
                    }

                    if ((sizesId == null || sizesId == 0) && !string.IsNullOrEmpty(sizesText[i]) && sizesText[i].ToLower() != "null")
                    {
                        sizesId = await _commonService.CheckCreateGetProductSize(sizesText[i]);
                    }

                    if ((colorsId == null || colorsId == 0) && !string.IsNullOrEmpty(colorsText[i]) && colorsText[i].ToLower() != "null")
                    {
                        colorsId = await _commonService.CheckCreateGetProductColor(colorsText[i]);
                    }

                    items.Add(new SupplierPurchaseDetails()
                    {
                        Id = curItemId,
                        SupplierPurchaseId = id,
                        ProductId= productId,
                        Barcode = barcodes[i],
                        CategoryId = catId,
                        DepartmentId = deptsId,
                        SeassonId = seassonId,
                        DescriptionId = descriptionId,
                        SkuId = skusId,
                        SizeId = sizesId,
                        ColorId = colorsId,
                        BrandId = brandId,
                        VendorId = vendorId,
                        GroupId = groupId,
                        UnitId = unitId,
                        YearId= yearId,
                        DescriptionEng = descriptions[i],
                        DescriptionArabic = descriptionsab[i],
                        QtyDozen = qtyDozen,
                        QtyPices = qty,
                        Price = price,
                        ExpireDate = !string.IsNullOrEmpty(expireDates[i]) ? DateTime.Parse(expireDates[i]) : null,
                        ProductDate = !string.IsNullOrEmpty(productDate[i]) ? DateTime.Parse(productDate[i]) : null,
                        FlagPricePerPices = flagPricePerPicesX,
                        UnitBarcode = null,// unitBarcodes[i],
                        QtyPerUnit = qtyPerUnitsX,
                        UnitCost = unitCost,
                        UnitSalePrice = saleprice,
                        BoxRetail = boxRetail,
                        UnitDescription = descriptions[i],// unitDescriptions[i],
                        Vat = vat,
                        PVat = pvat,
                    });
                }

                await _supplierPurchaseService.UpdateAsync(invoice, items);

                return new JsonResult(new { id = id });
        }
            catch (Exception ex)
            {
                throw ex.InnerException;
                // return View(model);
            }
}


        [Route("/Purchase/DeletePurchaseInvoiceItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(long id)
        {
            try
            {
                await _supplierPurchaseService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Purchase/ConfirmInvoice/{id}")]
        [HttpPost]
        public async Task<IActionResult> ConfirmInvoice(long id)
        {
            try
            {
                await _supplierPurchaseService.ConfirmInvoiceAsync(id, User.Identity.Name);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeletePurchaseInvoice/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _supplierPurchaseService.GetByIdAsync(id);
                await _supplierPurchaseService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Purchase/GetSupplierInfo/{id}")]
        [HttpGet]
        public async Task<SupplierInfo> GetSupplierInfo(int id)
        {
            try
            {
                var result = await _purchaseService.GetSupplierByIdAsync(id);

                var info = new SupplierInfo()
                {
                    Id = result.Id,
                    CompanyName = result.CompanyName,
                    Bpcode= result.Bpcode,
                    Address = result.Address,
                    VatNo = result.VatNo,
                    Balance = result.Balance,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Purchase/GetParentCategoryId")]
        [HttpPost]
        public async Task<IActionResult> GetParentCategoryId(int id)
        {
            int Id = await _commonService.GetParentCategoryIdAsync(id);
            return Json(data: Id);
        }

        [Route("/Purchase/GetSupplierBpCodes")]
        [HttpPost]
        public async Task<IActionResult> GetSupplierBpCodes(string Prefix)
        {
            var info = await _commonService.GetSupplierBpCodesAsync(Prefix);

            var productInfo1 = info.Select(t => new BpCodeViewModel()
            {
                SupplierId = t.Id,
                Code = t.Bpcode.Trim(),
            }).ToList();

            return Json(data: productInfo1);
        }

        [Route("/Purchase/GetBarcodes")]
        [HttpPost]
        public async Task<IActionResult> GetBarcodes(string Prefix)
        {
            var info = await _commonService.GetProductInfoAsync(Prefix);

            var productInfo1 = info.Select(t => new BarcodeViewModel()
            {
                Id = t.Id,
                ProductId = t.ProductId.Trim(),
            }).ToList();

            return Json(data: productInfo1);
        }

        [Route("/Purchase/GetSkuModels")]
        [HttpPost]
        public async Task<IActionResult> GetSkuModels(string Prefix)
        {
            var info = await _commonService.GetSkuStylefoAsync(Prefix);

            var skuInfo = info.Select(t => new StyleSkuViewModel()
            {
                Id = t.Id,
                Code = t.Code.Trim(),
            }).ToList();

            return Json(data: skuInfo);
        }


        [Route("/Purchase/GetProductYears")]
        [HttpPost]
        public async Task<IActionResult> GetProductYears(string Prefix)
        {
            int? yearx = null;
            if (!string.IsNullOrEmpty(Prefix))
            {
                yearx = int.Parse(Prefix);
            }
              

            var info = await _commonService.GetYearsAsync(yearx);

            var skuInfo = info.Select(t => new YearsViewModel()
            {
                Id = t.Id,
                YearName = t.YearName,
            }).ToList();

            return Json(data: skuInfo);
        }


        [Route("/Purchase/GetDepartments")]
        [HttpPost]
        public async Task<IActionResult> GetDepartments(string Prefix)
        {
            var info = await _commonService.GetDepartmentsInfoAsync(Prefix);

            var info2 = info.Select(t => new AutocompleteViewModel()
            {
                Id = t.Id,
                Name = t.NameEng.Trim(),
            }).ToList();

            return Json(data: info2);
        }

        [Route("/Purchase/GetSeassons")]
        [HttpPost]
        public async Task<IActionResult> GetSeassons(string Prefix)
        {
            var info = await _commonService.GetSeassonsInfoAsync(Prefix);

            var info2 = info.Select(t => new AutocompleteViewModel()
            {
                Id = t.Id,
                Name = t.NameEng.Trim(),
            }).ToList();

            return Json(data: info2);
        }

        [Route("/Purchase/GetColors")]
        [HttpPost]
        public async Task<IActionResult> GetColors(string Prefix)
        {
            var info = await _commonService.GetColorsInfoAsync(Prefix);

            var info2 = info.Select(t => new AutocompleteViewModel()
            {
                Id = t.Id,
                Name = t.NameEng.Trim(),
            }).ToList();

            return Json(data: info2);
        }

        [Route("/Purchase/GetSizes")]
        [HttpPost]
        public async Task<IActionResult> GetSizes(string Prefix)
        {
            var info = await _commonService.GetSizesInfoAsync(Prefix);

            var info2 = info.Select(t => new AutocompleteViewModel()
            {
                Id = t.Id,
                Name = t.NameEng.Trim(),
            }).ToList();

            return Json(data: info2);
        }


        [Route("/Purchase/GetProductInfoById")]
        [HttpPost]
        public async Task<IActionResult> GetProductInfoById(long id)
        {
            var info = await _commonService.GetProductInfoByIdAsync(id);

            var productInfo = new ProductInfoViewModel();

            productInfo.Id = info.Id;
            productInfo.ProductId = info.ProductId.Trim();
            productInfo.CompanyId = info.CompanyId;
            productInfo.CategoryId = info.CategoryId;
            productInfo.ColorId = info.ColorId;
            productInfo.SizeId = info.SizeId;
            productInfo.SeasonId = info.SeasonId;
            productInfo.DepartmentId = info.DepartmentId;
            productInfo.SkuId = info.SkuId;
            productInfo.SkuCode = info.SkuId !=null?await _commonService.GetSkuCodeByIdAsync(info.SkuId) : null;
            productInfo.Unitid = info.Unitid;
            productInfo.DescriptionId = info.DescriptionId;
            productInfo.BrandId = info.BrandId;
            productInfo.VendorId = info.VendorId;
            productInfo.GroupId = info.GroupId;
            productInfo.YearId = info.YearId??0;
            productInfo.YearName = info.YearId !=null? await _commonService.GetYearByIdAsync(info.YearId) : null;
            productInfo.ProductName = RequestCulture.Name == "ar" ? info.ProductNameArabic : info.ProductNameEng;
            productInfo.ProductNameEng = info.ProductNameEng;
            productInfo.ProductNameArabic= info.ProductNameArabic;
            productInfo.MfgDate = info.MfgDate != null ? info.MfgDate.Value.ToString("yyyy-MM-dd") : "";
            productInfo.ExpDate = info.ExpDate != null ? info.ExpDate.Value.ToString("yyyy-MM-dd") : "";
            productInfo.ProductInitialPrice = info.ProductInitialPrice??0;
            productInfo.CostPrice = info.CostPrice??0;
            productInfo.SalePrice = info.SalePrice??0;
            productInfo.Discount = info.Discount??0;
            productInfo.OreginalPrice = info.OreginalPrice??0;
            productInfo.Currentstock = info.Currentstock;
            productInfo.Vat = info.Vat;
            productInfo.UnitCost = info.CostPrice;
            productInfo.UnitSalePrice = info.UnitSalePrice;
            productInfo.QtyPerUnit = info.QtyPerUnit;
            productInfo.UnitBarcode=info.UnitBarcode;
            productInfo.UnitDescription=info.UnitDescription;
            productInfo.BoxNo=info.BoxNo;
            productInfo.MinQty=info.MinQty;
            productInfo.PackBarcode=info.PackBarcode;
            productInfo.PackDescription=info.PackDescription;
            productInfo.QtyPerPack=info.QtyPerPack;
            productInfo.PackPrice=info.PackPrice;
            productInfo.PackCost=info.PackCost;
            productInfo.WarrantyPeriod = info.WarrantyPeriod;
            productInfo.Currentstock = info.Currentstock??0;
            productInfo.Image = info.Image??"";
            productInfo.CustomField1 = info.CustomField1;
            productInfo.CustomField2 = info.CustomField2;
            productInfo.CustomField3 = info.CustomField3;
            productInfo.CustomField4 = info.CustomField4;

            return Json(data: productInfo);
        }

        [Route("/Purchase/GetProductIdByBarcode")]
        [HttpPost]
        public async Task<IActionResult> GetProductIdByBarcode(string code)
        {
            long Id = await _commonService.GetProductIdByBarcodeAsync(code);
            return Json(data: Id);
        }

        [Route("/Purchase/GetSkuIdBySkuCode")]
        [HttpPost]
        public async Task<IActionResult> GetSkuIdBySkuCode(string code)
        {
            long Id = await _commonService.GetStyleIdByCodeAsync(code);
            return Json(data: Id);
        }

        [Route("/Purchase/GetYearIdByYearName")]
        [HttpPost]
        public async Task<IActionResult> GetYearIdByYearName(int yearname)
        {
            long Id = await _commonService.GetYearIdByNameAsync(yearname);
            return Json(data: Id);
        }

        [Route("/Purchase/DownloadSample")]
        [HttpGet]
        public ActionResult DownloadSample()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Docs/Download/";

            FilePath = System.IO.Path.Combine(FilePath, "sample_format.xlsx");
            var fs = new FileStream(FilePath, FileMode.Open);

            return File(fs, "application/octet-stream", "sample_format.xlsx");
        }


        [Route("/Purchase/InvoiceItemUpload")]
        [HttpPost]
        public async Task<IActionResult> InvoiceItemUpload(IFormFile upload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/Upload/";

                string docName = null;
                if (upload != null && upload.Length > 0)
                {
                    string curDocName = Path.GetFileName(upload.FileName);
                    string curDocExtention = Path.GetExtension(upload.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await upload.CopyToAsync(stream);
                    }
                }
                Thread.Sleep(1000);
                List<ExcelDataViewModel> model = new List<ExcelDataViewModel>();
                if (docName != null)
                {
                    string docPath = System.IO.Path.Combine(uploadPath, docName);
                    using (var package = new ExcelPackage(new FileInfo(docPath)))
                    {
                        var ws = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= ws.Dimension.End.Row; row++)
                        {
                            string? barcode = ws.Cells[row, 1].Value !=null? ws.Cells[row, 1].Value.ToString():null;
                            string? desc = ws.Cells[row, 2].Value != null ? ws.Cells[row, 2].Value.ToString() : null;
                            string? strSeasson = ws.Cells[row, 3].Value != null ? ws.Cells[row, 3].Value.ToString() : null;
                            string? strDept = ws.Cells[row, 4].Value != null ? ws.Cells[row, 4].Value.ToString() : null;
                            string? strCat = ws.Cells[row, 5].Value != null ? ws.Cells[row, 5].Value.ToString() : null;
                            string? strSku = ws.Cells[row, 6].Value != null ? ws.Cells[row, 6].Value.ToString() : null;
                            string? strSize = ws.Cells[row, 7].Value != null ? ws.Cells[row, 7].Value.ToString() : null;
                            string? strColor = ws.Cells[row, 8].Value != null ? ws.Cells[row, 8].Value.ToString() : null;
                            string? strUnit = ws.Cells[row, 9].Value != null ? ws.Cells[row, 9].Value.ToString() : null;
                            string? strBrand = ws.Cells[row, 10].Value != null ? ws.Cells[row, 10].Value.ToString() : null;
                            string? strVendor = ws.Cells[row, 11].Value != null ? ws.Cells[row, 11].Value.ToString() : null;
                            string? strGroup = ws.Cells[row, 12].Value != null ? ws.Cells[row, 12].Value.ToString() : null;
                            string? strYear = ws.Cells[row, 13].Value != null ? ws.Cells[row, 13].Value.ToString() : null;
                            string? strDesc1 = ws.Cells[row, 14].Value != null ? ws.Cells[row, 14].Value.ToString() : null;
                            string? strDesc2 = ws.Cells[row, 15].Value != null ? ws.Cells[row, 15].Value.ToString() : null;
                            string? strdt1 = ws.Cells[row, 16].Value != null ? ws.Cells[row, 16].Value.ToString() : null;
                            string? strdt2 = ws.Cells[row, 17].Value != null ? ws.Cells[row, 17].Value.ToString() : null;
                            string? strQtyD = ws.Cells[row, 18].Value != null ? ws.Cells[row, 18].Value.ToString() : null;
                            string? strQtyP = ws.Cells[row, 19].Value != null ? ws.Cells[row, 19].Value.ToString() : null;
                            string? strCost = ws.Cells[row, 20].Value != null ? ws.Cells[row, 20].Value.ToString() : null;
                            string? strSellPrice = ws.Cells[row, 21].Value != null ? ws.Cells[row, 21].Value.ToString() : null;
                            string? strSellVat = ws.Cells[row, 22].Value != null ? ws.Cells[row, 22].Value.ToString() : null;
                            string? strOrg = ws.Cells[row, 23].Value != null ? ws.Cells[row, 23].Value.ToString() : null;
                            string? strTotal = ws.Cells[row, 24].Value != null ? ws.Cells[row, 24].Value.ToString() : null;


                            if (strSku != null)
                            {

                                long productId = 0;
                                int seassonId = 0;
                                int deptId = 0;
                                int catId = 0;
                                int ParentCatId = 0;
                                int skuId = 0;
                                int sizeId = 0;
                                int colorId = 0;
                                int unitId = 0;
                                int brandId = 0;
                                int vendorId = 0;
                                int groupId = 0;
                                int yearId = 0;

                                DateTime? dt1 = strdt1 != null?DateTime.Parse(strdt1):null;
                                DateTime? dt2 = strdt2 != null ? DateTime.Parse(strdt2) : null;


                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(barcode))
                                {
                                    productId = await _commonService.GetCurrentProductIdByBarcodeAsync(barcode);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strSeasson))
                                {
                                    seassonId = await _commonService.GetSessonIdByNameAsync(strSeasson);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strDept))
                                {
                                    deptId = await _commonService.GetDepartmentIdByNameAsync(strDept);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strCat))
                                {
                                    catId = await _commonService.GetCategoryIdByNameAsync(strCat);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strSku))
                                {
                                    skuId = await _commonService.CheckCreateGetProductSku(strSku);
                                }
                                Thread.Sleep(50);
                                if (!string.IsNullOrEmpty(strSize))
                                {
                                    sizeId = await _commonService.GetSizeIdByNameAsync(strSize);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strColor))
                                {
                                    colorId = await _commonService.GetColorIdByNameAsync(strColor);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strUnit))
                                {
                                    unitId = await _commonService.GetUnitIdByNameAsync(strUnit);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strBrand))
                                {
                                    brandId = await _commonService.GetBrandIdByNameAsync(strBrand);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strVendor))
                                {
                                    vendorId = await _commonService.GetVendorIdByNameAsync(strVendor);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strGroup))
                                {
                                    groupId = await _commonService.GetGroupIdByNameAsync(strGroup);
                                }
                                Thread.Sleep(20);
                                if (!string.IsNullOrEmpty(strYear))
                                {
                                    yearId = await _commonService.GetYearIdByNameAsync(strYear);
                                }
                                Thread.Sleep(20);
                                if (catId > 0)
                                {
                                    ParentCatId = await _commonService.GetParentCategoryIdAsync(catId);
                                }
                                Thread.Sleep(20);


                                model.Add(new ExcelDataViewModel()
                                {
                                    Barcode = barcode ?? "",
                                    ProductId = productId,
                                    SessonName = strSeasson ?? "",
                                    SeassonId = seassonId,
                                    DepartmentId = deptId,
                                    DepartmentName = strDept ?? "",
                                    CategoryId = catId,
                                    CategoryName = strCat ?? "",
                                    ParentCategoryId = ParentCatId,
                                    SkuId = skuId,
                                    Sku = strSku ?? "",
                                    SizeId = sizeId,
                                    SizeName = strSize ?? "",
                                    ColorId = colorId,
                                    ColorName = strColor ?? "",
                                    UnitId = unitId,
                                    UnitName = strUnit ?? "",
                                    BrandId = brandId,
                                    BrandName = strBrand ?? "",
                                    VendorId = vendorId,
                                    VendorName = strVendor ?? "",
                                    GroupId = groupId,
                                    GroupName = strGroup ?? "",
                                    YearId = yearId,
                                    YearName = strYear ?? "",
                                    DescriptionEng = strDesc1 ?? "",
                                    DescriptionArabic = strDesc2 ?? "",
                                    ProductDate = dt1 != null? dt1.Value.ToString("yyyy-MM-dd"):"",
                                    ExpireDate = dt2 != null ? dt2.Value.ToString("yyyy-MM-dd") : "",
                                    QtyDozen = strQtyD?? "",
                                    QtyPices = strQtyP?? "",
                                    Cost = strCost??"",
                                    SalePrice = strSellPrice??"",
                                    VatSale = strSellVat??"",
                                    OrgPrice = strOrg??"",
                                    Total = strTotal??""
                                });
                            }
                        }
                    }
                    return Json(data: model);
                }

                return Json(data: null);
            }
            catch(Exception ex)
            {
                //return Json(data: ex.Message);
                throw ex.InnerException;
            }
        }

        [Route("/Purchase/GetPurchaseDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPurchaseDetails(long id)
        {
            var info = await _supplierPurchaseService.GetItemsByRequestIdAsync(id);
            
            var detailsViewModels= new List<InvoiceDetailsViewModel>();
            if (info != null)
            {
                foreach(var item in info)
                {
                    detailsViewModels.Add(new InvoiceDetailsViewModel()
                    {
                        Id = item.Id,
                        SupplierPurchaseId = item.SupplierPurchaseId,
                        ProductId= item.ProductId,
                        Barcode=item.Barcode,
                        DescriptionId=item.DescriptionId,
                        DescriptionEng=item.DescriptionEng??"",
                        DescriptionArabic=item.DescriptionArabic??"",
                        CategoryId=item.CategoryId,
                        CategoryName=item.CategoryId !=null && item.CategoryId != 0? await _commonService.GetCategoryNameByIdAsync(item.CategoryId.Value):"",
                        ParentCategoryId = item.CategoryId !=null && item.CategoryId != 0? _commonService.GetCategoryByIdAsync(item.CategoryId.Value).Result.ParentCategoryId:null,
                        SkuId = item.SkuId,
                        SkuName= item.SkuId !=null && item.SkuId !=0 ? await _commonService.GetSkuCodeByIdAsync(item.SkuId.Value) : "",
                        SizeId= item.SizeId,
                        SizeName=item.SizeId != null && item.SizeId != 0 ? await _commonService.GetSizeNameByIdAsync(item.SizeId.Value) : "",
                        ColorId= item.ColorId,
                        ColorName=item.ColorId !=null && item.ColorId !=0 ? await _commonService.GetColorNameByIdAsync(item.ColorId.Value) : "",
                        UnitId=item.UnitId,
                        BrandId=item.BrandId,
                        VendorId=item.VendorId,
                        GroupId=item.GroupId,
                        DepartmentId=item.DepartmentId,
                        DepartmentName= item.DepartmentId!=null && item.DepartmentId !=0 ? await _commonService.GetDepartmentNameByIdAsync(item.DepartmentId.Value) : "",
                        SeassonId=item.SeassonId,
                        SeassonName = item.SeassonId !=null && item.SeassonId != 0 ? await _commonService.GetSessonNameByIdAsync(item.SeassonId.Value) : "",
                        YearId=item.YearId,
                        YearText= item.YearId != null && item.YearId != 0 ? await _commonService.GetYearByIdAsync(item.YearId.Value): "",
                        ProductDate =item.ProductDate !=null? item.ProductDate.Value.ToString("yyyy-MM-dd"):"",
                        ExpireDate= item.ExpireDate != null ? item.ExpireDate.Value.ToString("yyyy-MM-dd") : "",
                        QtyDozen=item.QtyDozen,
                        Qtypices=item.QtyPices,
                        Price=item.Price,
                        UnitCost=item.UnitCost,
                        UnitSalePrice=item.UnitSalePrice,
                        Vat=item.Vat,
                        PVat=item.PVat,
                    });
                }
            }

            return Json(data: detailsViewModels);
        }

        [Route("/Purchase/GenerateBarcodes")]
        [HttpPost]
        public async Task<IActionResult> GenerateBarcodes(List<GenerateBarcodeViewModel> barCodesData)
        {
            try
            {
                var newBarcodes = new List<GenerateBarcodeViewModel>();
                foreach (var data in barCodesData)
                {
                    var barCode = new GenerateBarcodeViewModel();
                    barCode.NewBarCode = "7";
                    if (!string.IsNullOrEmpty(data.Department))
                    {
                        var deptId = await _commonService.CheckCreateGetProductDepartment(data.Department);
                        barCode.Department = deptId.ToString();
                        barCode.NewBarCode += deptId > 0 ? deptId.ToString() : "0";
                    }
                    else
                    {
                        barCode.Department = "0";
                        barCode.NewBarCode += "0";
                    }

                    //if (!string.IsNullOrEmpty(data.CatId))
                    //{
                    //    var cat = "";
                    //    if (Convert.ToInt32(data.CatId) > 0)
                    //    {
                    //        cat = data.CatId.ToString();
                    //        if (Convert.ToInt32(data.CatId) < 10)
                    //        {
                    //            cat = "0" + cat;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        cat = "00";
                    //    }
                    //    barCode.CatId = data.CatId.ToString();
                    //    barCode.NewBarCode += cat;
                    //} else {
                    //    barCode.CatId = "0";
                    //    barCode.NewBarCode += "00";
                    //}

                    if (!string.IsNullOrEmpty(data.SubCatId))
                    {
                        var parentId = await _commonService.GetParentCategoryIdAsync(Convert.ToInt32(data.SubCatId));
                        var cat = "";
                        if (parentId > 0)
                        {
                            cat = parentId.ToString();
                            if (parentId < 10)
                            {
                                cat = "0" + cat;
                            }
                        }
                        else
                        {
                            cat = "00";
                        }

                        barCode.CatId = parentId.ToString();
                        barCode.NewBarCode += cat;

                        var subcat = "";
                        if (Convert.ToInt32(data.SubCatId) > 0)
                        {
                            subcat = data.SubCatId.ToString();
                            if (Convert.ToInt32(data.SubCatId) < 10)
                            {
                                subcat = "0" + subcat;
                            }
                        }
                        else
                        {
                            subcat = "00";
                        }
                        barCode.SubCatId = data.SubCatId.ToString();
                        barCode.NewBarCode += subcat;
                    }
                    else
                    {
                        barCode.SubCatId = "0";
                        barCode.NewBarCode += "00";
                    }

                    if (!string.IsNullOrEmpty(data.SKU))
                    {
                        var skuId = await _commonService.CheckCreateGetProductSku(data.SKU);
                        var skuBarCode = "";
                        if (skuId < 10)
                        {
                            skuBarCode = "00000" + skuId;
                        }
                        else
                        {
                            if (skuId < 100)
                            {
                                skuBarCode = "0000" + skuId;
                            }
                            else
                            {
                                if (skuId < 1000)
                                {
                                    skuBarCode = "000" + skuId;
                                }
                                else
                                {
                                    if (skuId < 10000)
                                    {
                                        skuBarCode = "00" + skuId;
                                    }
                                    else
                                    {
                                        if (skuId < 100000)
                                        {
                                            skuBarCode = "0" + skuId;
                                        }
                                        else
                                        {
                                            skuBarCode = skuId.ToString();
                                        }
                                    }
                                }
                            }
                        }

                        barCode.SKU = skuId.ToString();
                        barCode.NewBarCode += skuBarCode;
                    }
                    else
                    {
                        barCode.SKU = "0";
                        barCode.NewBarCode += "000000";
                    }

                    if (!string.IsNullOrEmpty(data.Color))
                    {
                        var colorId = await _commonService.CheckCreateGetProductColor(data.Color);
                        var color = "";
                        if (Convert.ToInt32(colorId) > 0)
                        {
                            color = colorId.ToString();
                            if (Convert.ToInt32(colorId) < 10)
                            {
                                color = "0" + color;
                            }
                        }
                        else
                        {
                            color = "00";
                        }
                        barCode.Color = colorId.ToString();
                        barCode.NewBarCode += color;
                    }
                    else
                    {
                        barCode.Color = "0";
                        barCode.NewBarCode += "00";
                    }


                    if (!string.IsNullOrEmpty(data.Size))
                    {
                        var sizeId = await _commonService.CheckCreateGetProductSize(data.Size);
                        var size = "";
                        if (Convert.ToInt32(sizeId) > 0)
                        {
                            size = sizeId.ToString();
                            if (Convert.ToInt32(sizeId) < 10)
                            {
                                size = "0" + size;
                            }
                        }
                        else
                        {
                            size = "00";
                        }
                        barCode.Size = sizeId.ToString();
                        barCode.NewBarCode += size;
                    }
                    else
                    {
                        barCode.Size = "0";
                        barCode.NewBarCode += "00";
                    }

                    if (!string.IsNullOrEmpty(data.Year))
                    {
                        barCode.Year = data.Year;
                        barCode.NewBarCode += data.Year;
                    }
                    else
                    {
                        barCode.Year = "0";
                        barCode.NewBarCode += "00";
                    }


                    if (!string.IsNullOrEmpty(data.Season))
                    {
                        var seasonId = await _commonService.CheckCreateGetProductSeasson(data.Season);
                        barCode.Season = seasonId.ToString();
                        barCode.NewBarCode += seasonId > 0 ? seasonId.ToString() : "0";
                    }
                    else
                    {
                        barCode.Season = "0";
                        barCode.NewBarCode += "0";
                    }

                    barCode.NewBarCode += "1";

                    barCode.NewBarCode = await CheckBarCode(barCode.NewBarCode);
                    newBarcodes.Add(barCode);
                }
                return Json(data: newBarcodes);
            }
            catch(Exception ex)
            {
                var one = ex.Message;
                throw ex.InnerException;
            }
            
        }

        public async Task<string> CheckBarCode(string code)
        {
            try
            {
                long Id = await _commonService.GetProductIdByBarcodeAsync(code);
                string newCode = code;
                var lastNo1 = code[code.Length - 1];
                var strNewCode1 = code.Substring(0, code.Length - 1);
                if (Id > 0)
                {
                    var lastNo = code[code.Length - 1];
                    var strNewCode = code.Substring(0, code.Length - 1);
                    if (Convert.ToInt32(lastNo.ToString()) < 9)
                    {
                        newCode = strNewCode + (Convert.ToInt64(lastNo.ToString()) + 1);
                    }
                    await CheckBarCode(newCode);
                    return newCode;
                }
                else
                {
                    return newCode;
                }
            }
            catch(Exception ex)
            {
                var one = ex.Message;
                throw ex.InnerException;
            }
        }

        #region Export To Excel

        [Route("/Purchase/ExportToExcel/{id}")]
        [HttpGet]
        public async Task<IActionResult> ShippmentReportCreate(long id)
        {
            using (var workbook = new XLWorkbook())
            {
                var invoice = await _supplierPurchaseService.GetByIdAsync(id);
                var supplierInfo = await _purchaseService.GetSupplierByIdAsync(invoice.SupplierId);
                string title = "Purchase Invoice Report";
                string supplierTitle = "Supplier Details";
                string invoiceTitle = "Invoice Details";
                var worksheet = workbook.Worksheets.Add("PurchaseInvoice");
                var currentRow = 8;

                #region Report Header

                worksheet.Range("A1:T1").Merge();
                worksheet.Range("A1:T1").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:T1").Style.Font.Bold = true;
                worksheet.Range("A1:T1").Style.Font.FontSize = 20;

                var headerCell = worksheet.Cell("A1");
                headerCell.Value = title;
                headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                #endregion

                #region Supplier Info

                worksheet.Column("A").AdjustToContents();
                worksheet.Column("B").AdjustToContents();
                worksheet.Column("G").AdjustToContents();
                worksheet.Column("H").AdjustToContents();

                worksheet.Range("A3:D3").Merge();
                worksheet.Range("A3:D3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A3:D3").Style.Font.Bold = true;
                worksheet.Range("A3:D3").Style.Font.FontSize = 16;

                var supplierHeaderCell = worksheet.Cell("A3");
                supplierHeaderCell.Value = supplierTitle;
                supplierHeaderCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                supplierHeaderCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell("A4").Value = "Name";
                worksheet.Cell("A4").Style.Font.Bold = true;
                worksheet.Cell("A5").Value = "Address";
                worksheet.Cell("A5").Style.Font.Bold = true;

                worksheet.Cell("B4").Value = supplierInfo != null ? supplierInfo.CompanyName : "";
                worksheet.Cell("B5").Value = supplierInfo != null ? supplierInfo.Address : "";

                worksheet.Cell("G4").Value = "VAT No";
                worksheet.Cell("G4").Style.Font.Bold = true;
                worksheet.Cell("G5").Value = "Telephone";
                worksheet.Cell("G5").Style.Font.Bold = true;

                worksheet.Cell("H4").Value = supplierInfo != null ? supplierInfo.VatNo : "";
                worksheet.Cell("H5").Value = supplierInfo != null ? supplierInfo.PhoneNo : "";

                #endregion

                #region Invoice Details

                worksheet.Range("A6:D6").Merge();
                worksheet.Range("A6:D6").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A6:D6").Style.Font.Bold = true;
                worksheet.Range("A6:D6").Style.Font.FontSize = 16;

                var invoiceHeaderCell = worksheet.Cell("A6");
                invoiceHeaderCell.Value = invoiceTitle;
                invoiceHeaderCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                invoiceHeaderCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell("A7").Value = "Invoice No";
                worksheet.Cell("A7").Style.Font.Bold = true;
                worksheet.Cell("B7").Value = invoice.InvoiceNo;

                worksheet.Cell("G7").Value = "Date";
                worksheet.Cell("G7").Style.Font.Bold = true;
                worksheet.Cell("H7").Value = invoice.Date.ToString("yyyy-MM-dd");

                #endregion

                #region Purchase Items Details

                #region Header

                worksheet.Row(currentRow).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 1).Value = "No.";
                worksheet.Cell(currentRow, 2).Value = "Barcode";
                worksheet.Cell(currentRow, 3).Value = "Description English";
                worksheet.Cell(currentRow, 4).Value = "Description Arabic";
                worksheet.Cell(currentRow, 5).Value = "Season";
                worksheet.Cell(currentRow, 6).Value = "Department";
                worksheet.Cell(currentRow, 7).Value = "Category";
                worksheet.Cell(currentRow, 8).Value = "Model/SKU";
                worksheet.Cell(currentRow, 9).Value = "Size";
                worksheet.Cell(currentRow, 10).Value = "Color";
                worksheet.Cell(currentRow, 11).Value = "Year";
                worksheet.Cell(currentRow, 12).Value = "ProductDate";
                worksheet.Cell(currentRow, 13).Value = "ExpiryDate";
                worksheet.Cell(currentRow, 14).Value = "QtyDozen";
                worksheet.Cell(currentRow, 15).Value = "QtyPieces";
                worksheet.Cell(currentRow, 16).Value = "Org.Price";
                worksheet.Cell(currentRow, 17).Value = "VAT Sale";
                worksheet.Cell(currentRow, 18).Value = "Selling";
                worksheet.Cell(currentRow, 19).Value = "Cost";
                worksheet.Cell(currentRow, 20).Value = "Total";

                #endregion

                #region Body

                var purchaseItems = await _supplierPurchaseService.GetItemsByRequestIdAsync(id);
                decimal? totalDzns = 0;
                decimal? totalPcs = 0;

                if (purchaseItems.Count > 0)
                {
                    int i = 0;
                    foreach (var item in purchaseItems)
                    {
                        currentRow++;
                        i++;
                        worksheet.Cell(currentRow, 1).Value = i;
                        worksheet.Cell(currentRow, 2).Value = item.Barcode;
                        worksheet.Cell(currentRow, 3).Value = item.DescriptionEng;
                        worksheet.Cell(currentRow, 4).Value = item.DescriptionArabic;
                        worksheet.Cell(currentRow, 5).Value = item.SeassonId != null && item.SeassonId != 0 ? await _commonService.GetSessonNameByIdAsync(item.SeassonId.Value) : "";
                        worksheet.Cell(currentRow, 6).Value = item.DepartmentId != null && item.DepartmentId != 0 ? await _commonService.GetDepartmentNameByIdAsync(item.DepartmentId.Value) : "";
                        worksheet.Cell(currentRow, 7).Value = item.CategoryId != null && item.CategoryId != 0 ? await _commonService.GetCategoryNameByIdAsync(item.CategoryId.Value) : "";
                        worksheet.Cell(currentRow, 8).Value = item.SkuId != null && item.SkuId != 0 ? await _commonService.GetSkuCodeByIdAsync(item.SkuId.Value) : "";
                        worksheet.Cell(currentRow, 9).Value = item.SizeId != null && item.SizeId != 0 ? await _commonService.GetSizeNameByIdAsync(item.SizeId.Value) : "";
                        worksheet.Cell(currentRow, 10).Value = item.ColorId != null && item.ColorId != 0 ? await _commonService.GetColorNameByIdAsync(item.ColorId.Value) : "";
                        worksheet.Cell(currentRow, 11).Value = item.YearId != null && item.YearId != 0 ? await _commonService.GetYearByIdAsync(item.YearId.Value) : "";
                        worksheet.Cell(currentRow, 12).Value = item.ProductDate != null ? item.ProductDate.Value.ToString("yyyy-MM-dd") : "";
                        worksheet.Cell(currentRow, 13).Value = item.ExpireDate != null ? item.ExpireDate.Value.ToString("yyyy-MM-dd") : "";
                        worksheet.Cell(currentRow, 14).Value = item.QtyDozen;
                        totalDzns += item.QtyDozen;
                        worksheet.Cell(currentRow, 15).Value = item.QtyPices;
                        totalPcs += item.QtyPices;
                        worksheet.Cell(currentRow, 16).Value = (item.UnitSalePrice - item.Vat);
                        worksheet.Cell(currentRow, 17).Value = item.Vat;
                        worksheet.Cell(currentRow, 18).Value = item.UnitSalePrice;
                        worksheet.Cell(currentRow, 19).Value = item.UnitCost;
                        worksheet.Cell(currentRow, 20).Value = ((((item.QtyDozen > 0) ? item.QtyDozen * 12 : 0) + item.QtyPices) * item.UnitCost);
                    }

                    currentRow++;
                    var newRow = worksheet.Range("H" + currentRow + ":S" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Total Before VAT";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var befVatTotalCell = worksheet.Cell("T" + currentRow);
                    befVatTotalCell.Value = invoice.Total - invoice.VatAmount;
                    befVatTotalCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":S" + currentRow);
                    newRow.Merge();
                    newRow.Value = "VAT Total";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var vatTotalCell = worksheet.Cell("T" + currentRow);
                    vatTotalCell.Value = invoice.VatAmount;
                    vatTotalCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":S" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Discount";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var discountCell = worksheet.Cell("T" + currentRow);
                    discountCell.Value = invoice.Discount;
                    discountCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":S" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Charges";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var chargesCell = worksheet.Cell("T" + currentRow);
                    chargesCell.Value = invoice.OtherCharges;
                    chargesCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":M" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Total";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var totalDznCell = worksheet.Cell("N" + currentRow);
                    totalDznCell.Value = totalDzns;
                    totalDznCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    var totalPcsCell = worksheet.Cell("O" + currentRow);
                    totalPcsCell.Value = totalPcs;
                    totalPcsCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    var totalCell = worksheet.Cell("T" + currentRow);
                    totalCell.Value = invoice.Total;
                    totalCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                }

                #endregion

                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PurcahseInvoice_" + id + ".xlsx"
                        );
                }
            }
        }

        #endregion
    }
}
