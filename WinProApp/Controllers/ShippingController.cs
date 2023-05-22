using iText.Commons.Actions.Data;
using iText.Layout;
using iTextSharp.text.pdf;
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
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels;
using WinProApp.ViewModels.Shipping;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using iTextSharp.text;
using iText.Commons.Actions.Contexts;
using iText.Html2pdf;
using iTextSharp.text.html.simpleparser;
using Document = iTextSharp.text.Document;
using iText.StyledXmlParser.Css.Media;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Attach;
using iText.StyledXmlParser.Node;
using iText.StyledXmlParser.Jsoup;
using Microsoft.AspNetCore.Html;
using System.Security.Policy;
using System.Net;
using ClosedXML.Excel;

namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Purchase, Warehouse")]
    public class ShippingController : BasedUserController
    {
        public readonly CommonService _commonService;
        public readonly ShippingService _shippingService;
        public readonly PurchaseService _purchaseService;
        public readonly StyleServices _styleServices;
        public readonly YearsService _yearsService;
        public readonly StoreService _storeService;
        public readonly ReportHeadService _reportHeadService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public ShippingController(CommonService commonService, ShippingService shippingService, PurchaseService purchaseService, StyleServices styleServices, YearsService yearsService, StoreService storeService, ReportHeadService reportHeadService, IWebHostEnvironment webHostEnvironment)
        {
            _commonService = commonService;
            _shippingService = shippingService;
            _purchaseService = purchaseService;
            _styleServices = styleServices;
            _yearsService = yearsService;
            _storeService = storeService;
            _reportHeadService = reportHeadService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Shipping/ShippingInfo")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Shipping/ShippingInfoList")]
        [HttpPost]
        public async Task<IActionResult> ShippingInfoList(JQueryDataTableParamModel param)
        {

            var results = _shippingService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierId > 0 ? _purchaseService.GetSupplierByIdAsync(x.SupplierId).Result.CompanyName : "",
                ToStoreId = x.ToStoreId,
                StoreName = _storeService.GetByIdAsync(x.ToStoreId).Result.Name,
                StoreVatNo = _storeService.GetByIdAsync(x.ToStoreId).Result.VatNo,
                ReferenceNo = x.ReferenceNo,
                Date = x.Date.ToString("yyyy-MM-dd"),
                AttachedDoc = x.AttachedDoc ?? "",
                Description = x.Description,
                Status = x.Status == true ? "Approved" : "OnHold",
                Discount = x.Discount,
                OtherCharges = x.OtherCharges,
                Total = x.Total,
                StrDiscount = x.Discount != null ? x.Discount.Value.ToString("0.00") : "",
                StrOtherCharges = x.OtherCharges != null ? x.OtherCharges.Value.ToString("0.00") : "",
                StrTotal = x.Total != null ? x.Total.Value.ToString("0.00") : "",
                RecordType = x.RecordType,
                ReceivedStatus=x.ReceivedStatus == true?"Yes":"No",
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


        [Route("/Shipping/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();
            var stores = await _storeService.GetByAllAsync();
            var units = await _commonService.GetUnitsAsync();
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
            ViewBag.Stores = new SelectList(stores, "Id", "Name");
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

        [Route("/Shipping/Import")]
        [HttpGet]
        public async Task<IActionResult> Import()
        {
            var records = await _shippingService.GetPendingShipmentAsync();
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();
            var stores = await _storeService.GetByAllAsync();
            var units = await _commonService.GetUnitsAsync();
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

            ViewBag.RecordsIdList = new SelectList(records, "Id", "Id");
            ViewBag.RecordsReferenceList = new SelectList(records, "Id", "ReferenceNo");
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.Stores = new SelectList(stores, "Id", "Name");
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

        [Route("/Shipping/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(AddViewModel model, IFormFile AttachedDoc)
        {
            try
            {
                long curInsertId = 0;
                string recordType = "Create";
                if (model.Id == 0)
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


                    var invoice = new ShippingInfo();
                    invoice.SupplierId = model.SupplierId;
                    invoice.ToStoreId = model.ToStoreId;
                    invoice.ReferenceNo = model.ReferenceNo;
                    invoice.Date = model.Date;
                    invoice.AttachedDoc = docName;
                    invoice.Description = model.Description;
                    invoice.Discount = model.Discount;
                    invoice.ChargesDescription = model.ChargesDescription;
                    invoice.OtherCharges = model.OtherCharges;
                    invoice.Total = model.Total;
                    invoice.Status = model.Status;
                    invoice.RecordType = model.RecordType;
                    invoice.ReceivedStatus = false;
                    invoice.CratedDate = DateTime.Now;
                    invoice.CreatedBy = User.Identity.Name;
                    invoice.UpdatedDate = DateTime.Now;
                    invoice.UpdatedBy = User.Identity.Name;

                     curInsertId = await _shippingService.CreateAsync(invoice);

                    recordType = invoice.RecordType;
                }
                else
                {
                  //  var invoiceInfo = await _shippingService.GetByIdAsync(model.Id);
                    curInsertId = model.Id;
                    recordType = "Create";
                    await _shippingService.UpdateShipmentTotalAsync(curInsertId, model.Discount, model.OtherCharges, model.Total, User.Identity.Name);
                }

                List<ShippingDetails> items = new List<ShippingDetails>();



                string[] ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] barcodes = Request.Form["Barcode"];
                string[] descriptions = Request.Form["ItemDescription"];
                string[] descriptionsab = Request.Form["ItemDescriptionAb"];
                string[] categories = Request.Form["itemCategory"];
                string[] categoriesText = Request.Form["itemCategoryText"];
                string[] seassons = Request.Form["itemSeassonId"];
                string[] seassonText = Request.Form["itemSeasson"];
                string[] depts = Request.Form["itemDepartmentId"];
                string[] deptText = Request.Form["itemDepartment"];
                string[] skus = Request.Form["itemSkuId"];
                string[] skuText = Request.Form["itemSkuText"];
                string[] sizes = Request.Form["itemSizeId"];
                string[] sizeText = Request.Form["itemSize"];
                string[] colors = Request.Form["itemColorId"];
                string[] colorText = Request.Form["itemColor"];
                string[] brands = Request.Form["itemBrand"];
                string[] vendors = Request.Form["itemVendor"];
                string[] years = Request.Form["itemYear"];
                string[] yearText = Request.Form["itemYearText"];
                string[] groups = Request.Form["itemGroup"];
                string[] unitIds = Request.Form["ItemUnitId"];
                string[] boxNos = Request.Form["ItemBoxNo"];
                string[] qtyBoxs = Request.Form["ItemQtyBox"];
                string[] qtys = Request.Form["ItemQty"];
                string[] prices = Request.Form["itemPrice"];
                string[] productImgs = Request.Form["productImg"];

                string[] sellPrices = null;
                string[] sellVats = null;
                string[] orgPrices = null;
                

                if (recordType == "Receive")
                {
                    sellPrices = Request.Form["ItemSellingPrice"];
                    sellVats = Request.Form["ItemSellingVat"];
                    orgPrices = Request.Form["ItemOrgPrice"];
                    
                }


                for (int i = 0; i < ids.Length; i++)
                {
                    long? productId = null;
                    int? catId = null;
                    int? seassonId = null;
                    int? deptsId = null;
                    int? skusId = null;
                    int? sizesId = null;
                    int? colorsId = null;
                    int? brandId = null;
                    int? yearId = null;
                    int? vendorId = null;
                    int? unitId = null;
                    int? groupId = null;
                    int? qtybox = null;
                    int? qty = null;
                    decimal? price = null;
                    decimal? salePrice = null;
                    decimal? saleVat = null;
                    decimal? orgPrice = null;


                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = int.Parse(productIds[i]);
                    }

                    if (!string.IsNullOrEmpty(categories[i]) && categories[i] != "0")
                    {
                        catId = int.Parse(categories[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(categories[i]) || categories[i] == "0")
                        {
                            string strText0 = categoriesText[i];
                            if (!string.IsNullOrEmpty(strText0))
                            {
                                catId = await _commonService.CheckCreateGetProductCategory(strText0);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(seassons[i]) && seassons[i] != "0")
                    {
                        seassonId = int.Parse(seassons[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(seassons[i]) || seassons[i] == "0")
                        {
                            string strText1 = seassonText[i];
                            if (!string.IsNullOrEmpty(strText1))
                            {
                                seassonId = await _commonService.CheckCreateGetProductSeasson(strText1);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(depts[i]) && depts[i] != "0")
                    {
                        deptsId = int.Parse(depts[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(depts[i]) || depts[i] == "0")
                        {
                            string strText2 = deptText[i];
                            if (!string.IsNullOrEmpty(strText2))
                            {
                                deptsId = await _commonService.CheckCreateGetProductDepartment(strText2);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(skus[i]) && skus[i] != "0")
                    {
                        skusId = int.Parse(skus[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(skus[i]) || skus[i] == "0")
                        {
                            string strText = skuText[i];
                            if (!string.IsNullOrEmpty(strText))
                            {
                                skusId = await _commonService.CheckCreateGetProductSku(strText);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes[i]) && sizes[i] != "0")
                    {
                        sizesId = int.Parse(sizes[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sizes[i]) || sizes[i] == "0")
                        {
                            string strText3 = sizeText[i];
                            if (!string.IsNullOrEmpty(strText3))
                            {
                                sizesId = await _commonService.CheckCreateGetProductSize(strText3);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(colors[i]) && colors[i] != "0")
                    {
                        colorsId = int.Parse(colors[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(colors[i]) || colors[i] == "0")
                        {
                            string strText4 = colorText[i];
                            if (!string.IsNullOrEmpty(strText4))
                            {
                                colorsId = await _commonService.CheckCreateGetProductColor(strText4);
                            }
                        }
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
                        if (string.IsNullOrEmpty(years[i]) || years[i] == "0")
                        {
                            if (!string.IsNullOrEmpty(strYearText))
                            {
                                yearId = await _commonService.CheckCreateGetProductYear(strYearText);
                            }
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

                    if (!string.IsNullOrEmpty(qtyBoxs[i]))
                    {
                        qtybox = int.Parse(qtyBoxs[i]);
                    }
                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = int.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    if (recordType == "Receive")
                    {
                        if (!string.IsNullOrEmpty(sellPrices[i]))
                        {
                            salePrice = decimal.Parse(sellPrices[i]);
                        }

                        if (!string.IsNullOrEmpty(sellVats[i]))
                        {
                            saleVat = decimal.Parse(sellVats[i]);
                        }
                        if (!string.IsNullOrEmpty(orgPrices[i]))
                        {
                            orgPrice = decimal.Parse(orgPrices[i]);
                        }
                    }


                    items.Add(new ShippingDetails()
                    {
                        ShippingId = curInsertId,
                        ProductId = productId,
                        Barcode = barcodes[i],
                        CategoryId = catId,
                        DepartmentId = deptsId,
                        SeassonId = seassonId,
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
                        BoxNo = boxNos[i],
                        QtyPerBox = qtybox,
                        Qty = qty,
                        Price = price,
                        SalePrice = salePrice,
                        SaleVat = saleVat,
                        OriginalPrice = orgPrice,
                        ImageUrl= productImgs[i]
                    });
                }

                // requiestInfo.RequestItems = items;
                await _shippingService.CreateItemsAsync(curInsertId, items);


                return new JsonResult(new { id = curInsertId });
            }
            catch
            {
                return View();
            }
        }


        [Route("/Shipping/Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var invoice = await _shippingService.GetByIdAsync(id);
            var model = new DetailsViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                ToStoreId = invoice.ToStoreId,
                ReferenceNo = invoice.ReferenceNo,
                Date = invoice.Date.ToString("yyyy-MM-dd"),
                AttachedDoc = invoice.AttachedDoc,
                Description = invoice.Description,
                Status = invoice.Status == true ? "Approved" : "OnHold",
                StrDiscount = invoice.Discount != null ? invoice.Discount.Value.ToString("0.00") : "0.00",
                StrOtherCharges = invoice.OtherCharges != null ? invoice.OtherCharges.Value.ToString("0.00") : "0.00",
                StrTotal = invoice.Total != null ? invoice.Total.Value.ToString("0.00") : "0.00",
                ChargesDescription = invoice.ChargesDescription,
                RecordType = invoice.RecordType,
                CreatedBy = invoice.CreatedBy,
                CratedDate = invoice.CratedDate != null ? invoice.CratedDate.Value.ToString("yyyy-MM-dd") : null,
                UpdatedBy = invoice.UpdatedBy,
                UpdatedDate = invoice.UpdatedDate != null ? invoice.UpdatedDate.Value.ToString("yyyy-MM-dd") : null,
            };
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();
            var stores = await _storeService.GetByAllAsync();

            ViewBag.Suppliers = new SelectList(suppliers, "Id", "CompanyName");
            ViewBag.VatPercentage = vat.Percentage ?? 0;
            ViewBag.Stores = new SelectList(stores, "Id", "Name");


            return View(model);
        }


        [Route("/Shipping/Edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var suppliers = await _purchaseService.GetAllSuppliersAsync();
            var vat = await _commonService.GetVatAsync();
            var stores = await _storeService.GetByAllAsync();
            var units = await _commonService.GetUnitsAsync();
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
            ViewBag.Stores = new SelectList(stores, "Id", "Name");
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

            var invoice = await _shippingService.GetByIdAsync(id);
            if (invoice.Status == true)
            {
                return NotFound();
            }
            if (invoice.RecordType == "Receive")
            {
                return NotFound();
            }
            var model = new EditViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                ToStoreId = invoice.ToStoreId,
                ReferenceNo = invoice.ReferenceNo,
                Date = invoice.Date,
                AttachedDoc = invoice.AttachedDoc,
                Description = invoice.Description,
                Discount = invoice.Discount ?? 0,
                OtherCharges = invoice.OtherCharges ?? 0,
                ChargesDescription = invoice.ChargesDescription,
                Total = invoice.Total,
                Status = invoice.Status,
                RecordType = invoice.RecordType,
            };


            return View(model);
        }


        [Route("/Shipping/Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model, IFormFile AttachedDoc)
        {
            long id = model.Id;
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/SupplierInvoice/";

                string docName = null;
                if (AttachedDoc != null)
                {
                    if (AttachedDoc.Length > 0)
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
                }

                var invoice = await _shippingService.GetByIdAsync(id);
                bool oldStatus = invoice.Status;
                string? oldDoc = invoice.AttachedDoc;
                invoice.SupplierId = model.SupplierId;
                invoice.ToStoreId = model.ToStoreId;
                invoice.ReferenceNo = model.ReferenceNo;
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

                invoice.Description = model.Description;
                invoice.Discount = model.Discount;
                invoice.ChargesDescription = model.ChargesDescription;
                invoice.OtherCharges = model.OtherCharges;
                invoice.Total = model.Total;
                if(invoice.Status == true)
                {
                    invoice.Status = true;
                    invoice.ReceivedStatus = model.Status;
                }
                else
                {
                    invoice.Status = model.Status;
                    invoice.ReceivedStatus = false;
                }
                
                invoice.RecordType = model.RecordType;
                invoice.UpdatedDate = DateTime.Now;
                invoice.UpdatedBy = User.Identity.Name;

                //   long curInsertId = await _shippingService.CreateAsync(invoice);

                List<ShippingDetails> items = new List<ShippingDetails>();

                string[] ids = Request.Form["ItemId"];
                string[] productIds = Request.Form["ItemProductId"];
                string[] barcodes = Request.Form["Barcode"];
                string[] descriptions = Request.Form["ItemDescription"];
                string[] descriptionsab = Request.Form["ItemDescriptionAb"];
                string[] categories = Request.Form["itemCategory"];
                string[] categoriesText = Request.Form["itemCategoryText"];
                string[] seassons = Request.Form["itemSeassonId"];
                string[] seassonText = Request.Form["itemSeasson"];
                string[] depts = Request.Form["itemDepartmentId"];
                string[] deptText = Request.Form["itemDepartment"];
                string[] skus = Request.Form["itemSkuId"];
                string[] skuText = Request.Form["itemSkuText"];
                string[] sizes = Request.Form["itemSizeId"];
                string[] sizeText = Request.Form["itemSize"];
                string[] colors = Request.Form["itemColorId"];
                string[] colorText = Request.Form["itemColor"];
                string[] brands = Request.Form["itemBrand"];
                string[] vendors = Request.Form["itemVendor"];
                string[] years = Request.Form["itemYear"];
                string[] yearText = Request.Form["itemYearText"];
                string[] groups = Request.Form["itemGroup"];
                string[] unitIds = Request.Form["ItemUnitId"];
                string[] boxNos = Request.Form["ItemBoxNo"];
                string[] qtyBoxs = Request.Form["ItemQtyBox"];
                string[] qtys = Request.Form["ItemQty"];
                string[] prices = Request.Form["itemPrice"];
                string[] productImgs = Request.Form["productImg"];

                string[] rqtys = null;
                string[] sellPrices = null;
                string[] sellVats = null;
                string[] orgPrices = null;
               

                if (invoice.RecordType == "Receive")
                {
                    sellPrices = Request.Form["ItemSellingPrice"];
                    sellVats = Request.Form["ItemSellingVat"];
                    orgPrices = Request.Form["ItemOrgPrice"];
                }

                if(invoice.Status == true)
                {
                     rqtys = Request.Form["ItemRQty"];
                }
                //    //   


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
                    int? brandId = null;
                    int? yearId = null;
                    int? vendorId = null;
                    int? unitId = null;
                    int? groupId = null;
                    int? qtybox = null;
                    int? qty = null;
                    int? rqty = null;
                    decimal? price = null;
                    decimal? salePrice = null;
                    decimal? saleVat = null;
                    decimal? orgPrice = null;

                    if (!string.IsNullOrEmpty(productIds[i]))
                    {
                        productId = int.Parse(productIds[i]);
                    }

                    if(i < categories.Length)
                    {
                        if (!string.IsNullOrEmpty(categories[i]) && categories[i] != "0")
                        {
                            catId = int.Parse(categories[i]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(categories[i]) || categories[i] == "0")
                            {
                                string strText0 = i < categoriesText.Length ? categoriesText[i] : "";
                                if (!string.IsNullOrEmpty(strText0))
                                {
                                    catId = await _commonService.CheckCreateGetProductCategory(strText0);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(seassons[i]) && seassons[i] != "0")
                    {
                        seassonId = int.Parse(seassons[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(seassons[i]) || seassons[i] == "0")
                        {
                            string strText1 = seassonText[i];
                            if (!string.IsNullOrEmpty(strText1))
                            {
                                seassonId = await _commonService.CheckCreateGetProductSeasson(strText1);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(depts[i]) && depts[i] != "0")
                    {
                        deptsId = int.Parse(depts[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(depts[i]) || depts[i] == "0")
                        {
                            string strText2 = deptText[i];
                            if (!string.IsNullOrEmpty(strText2))
                            {
                                deptsId = await _commonService.CheckCreateGetProductDepartment(strText2);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(skus[i]) && skus[i] != "0")
                    {
                        skusId = int.Parse(skus[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(skus[i]) || skus[i] == "0")
                        {
                            string strText = skuText[i];
                            if (!string.IsNullOrEmpty(strText))
                            {
                                skusId = await _commonService.CheckCreateGetProductSku(strText);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes[i]) && sizes[i] != "0")
                    {
                        sizesId = int.Parse(sizes[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sizes[i]) || sizes[i] == "0")
                        {
                            string strText3 = sizeText[i];
                            if (!string.IsNullOrEmpty(strText3))
                            {
                                sizesId = await _commonService.CheckCreateGetProductSize(strText3);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(colors[i]) && colors[i] != "0")
                    {
                        colorsId = int.Parse(colors[i]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(colors[i]) || colors[i] == "0")
                        {
                            string strText4 = colorText[i];
                            if (!string.IsNullOrEmpty(strText4))
                            {
                                colorsId = await _commonService.CheckCreateGetProductColor(strText4);
                            }
                        }
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
                        if (string.IsNullOrEmpty(years[i]) || years[i] == "0")
                        {
                            if (!string.IsNullOrEmpty(strYearText))
                            {
                                yearId = await _commonService.CheckCreateGetProductYear(strYearText);
                            }
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

                    if (!string.IsNullOrEmpty(qtyBoxs[i]) && qtyBoxs[i].ToLower() != "null")
                    {
                        qtybox = int.Parse(qtyBoxs[i]);
                    }
                    if (!string.IsNullOrEmpty(qtys[i]) && qtys[i].ToLower() != "null")
                    {
                        qty = int.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]) && prices[i].ToLower() != "null")
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    if (invoice.RecordType == "Receive")
                    {
                        if (!string.IsNullOrEmpty(sellPrices[i]))
                        {
                            salePrice = decimal.Parse(sellPrices[i]);
                        }

                        if (!string.IsNullOrEmpty(sellVats[i]))
                        {
                            saleVat = decimal.Parse(sellVats[i]);
                        }
                        if (!string.IsNullOrEmpty(orgPrices[i]))
                        {
                            orgPrice = decimal.Parse(orgPrices[i]);
                        }
                    }

                    if(oldStatus == true)
                    {
                        if (rqtys != null && rqtys.Length > i && !string.IsNullOrEmpty(rqtys[i]))
                        {
                            rqty = int.Parse(rqtys[i]);
                        }
                    }

                    items.Add(new ShippingDetails()
                    {
                        Id = curItemId,
                        ShippingId = id,
                        ProductId = productId,
                        Barcode = barcodes[i],
                        CategoryId = catId,
                        DepartmentId = deptsId,
                        SeassonId = seassonId,
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
                        BoxNo = boxNos[i],
                        QtyPerBox = qtybox,
                        Qty = qty,
                        ReceivedQty=rqty,
                        Price = price,
                        SalePrice = salePrice,
                        SaleVat = saleVat,
                        OriginalPrice = orgPrice,
                        ImageUrl= productImgs[i]
                    });
                }

                //// requiestInfo.RequestItems = items;
                await _shippingService.UpdateAsync(invoice, items);

                return new JsonResult(new { id = id });
        }
            catch (Exception ex)
            {
                var one = ex.Message;
                throw ex.InnerException;
                return View();
    }
}

        [Route("/Shipping/ImageUpload")]
        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile upload)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = webRootPath + "/Docs/Products/";
            string? imgName = null;
            if (upload != null)
            {
                string curImgName = Path.GetFileName(upload.FileName);
                string curImgExtention = Path.GetExtension(upload.FileName);

                imgName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curImgExtention;
                string ImgSavePath = System.IO.Path.Combine(uploadPath, imgName);

                using (var stream = System.IO.File.Create(ImgSavePath))
                {
                    await upload.CopyToAsync(stream);
                    Task.Delay(100);
                }

            }

            return Json(data:imgName);
        }

        [Route("/Shipping/DeleteItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(long id)
        {
            try
            {
                await _shippingService.DeleteItemAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Shipping/Confirm/{id}")]
        [HttpPost]
        public async Task<IActionResult> Confirm(long id)
        {
            try
            {
                await _shippingService.ConfirmInvoiceAsync(id, User.Identity.Name);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Shipping/Delete/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _shippingService.GetByIdAsync(id);
                await _shippingService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Shipping/ChangeStatus/{id}")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            try
            {
                await _shippingService.UpdateStatusToOnHoldAsync(id);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Shipping/GetStoreInfo/{id}")]
        [HttpGet]
        public async Task<StoreInfoViewModel> GetStoreInfo(int id)
        {
            try
            {
                var result = await _storeService.GetByIdAsync(id);

                var info = new StoreInfoViewModel()
                {
                    Id = result.Id,
                    Name = result.Name,
                    StoreCode = result.StoreCode,
                    Address = result.Address,
                    VatNo = result.VatNo,
                };

                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Shipping/DownloadSample")]
        [HttpGet]
        public ActionResult DownloadSample()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Docs/Download/";

            FilePath = System.IO.Path.Combine(FilePath, "sample_shipping_format.xlsx");
            var fs = new FileStream(FilePath, FileMode.Open);

            return File(fs, "application/octet-stream", "sample_format.xlsx");
        }


        [Route("/Shipping/ItemUpload")]
        [HttpPost]
        public async Task<IActionResult> ItemUpload(IFormFile upload)
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
                    string DocSavePath = System.IO.Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await upload.CopyToAsync(stream);
                    }
                }
                Thread.Sleep(1000);
                List<ShippingItemUploadViewModel> model = new List<ShippingItemUploadViewModel>();
                if (docName != null)
                {
                    string docPath = System.IO.Path.Combine(uploadPath, docName);
                    using (var package = new ExcelPackage(new FileInfo(docPath)))
                    {
                        var ws = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= ws.Dimension.End.Row; row++)
                        {
                            string? barcode = ws.Cells[row, 1].Value != null ? ws.Cells[row, 1].Value.ToString() : null;
                            string? strSeasson = ws.Cells[row, 2].Value != null ? ws.Cells[row, 2].Value.ToString() : null;
                            string? strDept = ws.Cells[row, 3].Value != null ? ws.Cells[row, 3].Value.ToString() : null;
                            string? strCat = ws.Cells[row, 4].Value != null ? ws.Cells[row, 4].Value.ToString() : null;
                            string? strSku = ws.Cells[row, 5].Value != null ? ws.Cells[row, 5].Value.ToString() : null;
                            if (strSku != null)
                            {
                                string? strSize = ws.Cells[row, 6].Value != null ? ws.Cells[row, 6].Value.ToString() : null;
                                string? strColor = ws.Cells[row, 7].Value != null ? ws.Cells[row, 7].Value.ToString() : null;
                                string? strUnit = ws.Cells[row, 8].Value != null ? ws.Cells[row, 8].Value.ToString() : null;
                                string? strBrand = ws.Cells[row, 9].Value != null ? ws.Cells[row, 9].Value.ToString() : null;
                                string? strVendor = ws.Cells[row, 10].Value != null ? ws.Cells[row, 10].Value.ToString() : null;
                                string? strGroup = ws.Cells[row, 11].Value != null ? ws.Cells[row, 11].Value.ToString() : null;
                                string? strYear = ws.Cells[row, 12].Value != null ? ws.Cells[row, 12].Value.ToString() : null;
                                string? strDesc1 = ws.Cells[row, 13].Value != null ? ws.Cells[row, 13].Value.ToString() : null;
                                string? strDesc2 = ws.Cells[row, 14].Value != null ? ws.Cells[row, 14].Value.ToString() : null;
                                string? strdt1 = ws.Cells[row, 15].Value != null ? ws.Cells[row, 15].Value.ToString() : null;
                                string? strdt2 = ws.Cells[row, 16].Value != null ? ws.Cells[row, 16].Value.ToString() : null;
                                string? strBoxNo = ws.Cells[row, 17].Value != null ? ws.Cells[row, 17].Value.ToString() : null;
                                string? strBoxQty = ws.Cells[row, 18].Value != null ? ws.Cells[row, 18].Value.ToString() : null;
                                string? strQty = ws.Cells[row, 19].Value != null ? ws.Cells[row, 19].Value.ToString() : null;
                                string? strCost = ws.Cells[row, 20].Value != null ? ws.Cells[row, 20].Value.ToString() : null;
                                string? strTot = ws.Cells[row, 21].Value != null ? ws.Cells[row, 21].Value.ToString() : null;
                                string? strSell = ws.Cells[row, 22].Value != null ? ws.Cells[row, 22].Value.ToString() : null;
                                string? strSellVat = ws.Cells[row, 23].Value != null ? ws.Cells[row, 23].Value.ToString() : null;
                                string? strOrg = ws.Cells[row, 24].Value != null ? ws.Cells[row, 24].Value.ToString() : null;
                                string? strImg = ws.Cells[row, 25].Value != null ? ws.Cells[row, 25].Value.ToString() : null;
                                string? imgName = null;
                                if (strImg != null)
                                {
                                    bool imageExist = URLExists(strImg);
                                    if (imageExist)
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            string curImgExtention = Path.GetExtension(strImg);
                                            imgName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curImgExtention;
                                            string imgSavePath = System.IO.Path.Combine(imageUploadPath, imgName);
                                            client.DownloadFileAsync(new Uri(strImg), imgSavePath);
                                            Thread.Sleep(20);
                                        }
                                    }
                                }

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

                                Thread.Sleep(50);
                                if (!string.IsNullOrEmpty(barcode))
                                {
                                    productId = await _commonService.GetCurrentProductIdByBarcodeAsync(barcode);
                                    if(productId > 0 && imgName == null)
                                    {
                                        imgName = await _commonService.ProductImageById(productId);
                                    }
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
                                Thread.Sleep(20);
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



                                model.Add(new ShippingItemUploadViewModel()
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
                                    ProductDate = strdt1 ?? "",
                                    ExpireDate = strdt2 ?? "",
                                    BoxNo = strBoxNo ?? "",
                                    QtyPerBox = strBoxQty ?? "",
                                    Qty = strQty ?? "",
                                    Price = strCost ?? "",
                                    Total = strTot ?? "",
                                    SalePrice = strSell ?? "",
                                    SaleVat = strSellVat ?? "",
                                    OriginalPrice = strOrg ?? "",
                                    ImageUrl= imgName !=null? imgName:""
                                });
                            }
                        }
                    }
                    return Json(data: model);
                }

                return Json(data: null);
            }
            catch (Exception ex)
            {
                //return Json(data: ex.Message);
                throw ex.InnerException;
            }
        }

        public bool URLExists(string url)
        {
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);
            webRequest.Method = "HEAD";
            try
            {
                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)webRequest.GetResponse())
                {
                    if (response.StatusCode.ToString() == "OK")
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        [Route("/Shipping/GetShippingIds/{type}")]
        [HttpGet]
        public async Task<IActionResult> GetShippingIds(string type)
        {
            var info = _shippingService.GetSelectedShippingInfoByAsync(type).Result.Select(x => new ShippingRecordViewModel()
            {
                Id=x.Id,
                ReferenceNo=x.ReferenceNo
            }).ToList();

            return Json(data:info);
        }

        [Route("/Shipping/GetEditableShippingIds/{type}")]
        [HttpGet]
        public async Task<IActionResult> GetEditableShippingIds(string type)
        {
            var info = _shippingService.GetSelectedShippingInfoByAsync(type).Result.Select(x => new ShippingRecordViewModel()
            {
                Id = x.Id,
                ReferenceNo = x.ReferenceNo
            }).ToList();

            return Json(data: info);
        }


        [Route("/Shipping/GetShippingDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetShippingDetails(long id)
        {
            var info = await _shippingService.GetItemsByRequestIdAsync(id);

            var detailsViewModels = new List<ItemDetailsViewModel>();
            if (info != null)
            {
                foreach (var item in info)
                {
                    decimal totalX = 0;
                    decimal priceX = item.Price ?? 0;
                    int qtyX = item.Qty ?? 0;
                    totalX = priceX * qtyX;
                    detailsViewModels.Add(new ItemDetailsViewModel()
                    {
                        Id = item.Id,
                        ShippingId = item.ShippingId,
                        ProductId = item.ProductId ?? 0,
                        ProductImage = item.ImageUrl??(item.ProductId != null && item.ProductId > 0 ? _commonService.GetProductInfoByIdAsync(item.ProductId.Value).Result.Image : ""),
                        Barcode = item.Barcode,
                        DescriptionEng = item.DescriptionEng ?? "",
                        DescriptionArabic = item.DescriptionArabic ?? "",
                        CategoryId = item.CategoryId ?? 0,
                        CategoryName = item.CategoryId != null && item.CategoryId > 0? _commonService.GetCategoryNameByIdAsync(item.CategoryId).Result:"",
                        ParentCategoryId = item.CategoryId !=null && item.CategoryId !=0 ? _commonService.GetCategoryByIdAsync(item.CategoryId.Value).Result.ParentCategoryId : 0,
                        SkuId = item.SkuId ?? 0,
                        Sku = item.SkuId != null && item.SkuId > 0 ? await _commonService.GetSkuCodeByIdAsync(item.SkuId.Value) : "",
                        SizeId = item.SizeId ?? 0,
                        SizeName = item.SizeId != null && item.SizeId > 0 ? await _commonService.GetSizeNameByIdAsync(item.SizeId.Value) : "",
                        ColorId = item.ColorId ?? 0,
                        ColorName = item.ColorId != null && item.ColorId > 0 ? await _commonService.GetColorNameByIdAsync(item.ColorId.Value) : "",
                        UnitId = item.UnitId ?? 0,
                        BrandId = item.BrandId ?? 0,
                        VendorId = item.VendorId ?? 0,
                        GroupId = item.GroupId ?? 0,
                        DepartmentId = item.DepartmentId ?? 0,
                        DepartmentName = item.DepartmentId != null && item.DepartmentId > 0 ? await _commonService.GetDepartmentNameByIdAsync(item.DepartmentId.Value) : "",
                        SeassonId = item.SeassonId ?? 0,
                        SessonName = item.SeassonId != null && item.SeassonId > 0 ? await _commonService.GetSessonNameByIdAsync(item.SeassonId.Value) : "",
                        YearId = item.YearId ?? 0,
                        YearName = item.YearId != null && item.YearId > 0 ? await _commonService.GetYearByIdAsync(item.YearId.Value) : "",
                        BoxNo = item.BoxNo,
                        QtyPerBox = item.QtyPerBox,
                        Qty = item.Qty,
                        ReceivedQty= item.ReceivedQty,
                        Price = item.Price,
                        Total = totalX,
                        StrSalePrice = item.SalePrice != null ? item.SalePrice.Value.ToString("0.00") : "",
                        StrSaleVat = item.SaleVat != null ? item.SaleVat.Value.ToString("0.00") : "",
                        StrOriginalPrice = item.OriginalPrice != null ? item.OriginalPrice.Value.ToString("0.00") : "",
                    });
                }
            }

            return Json(data: detailsViewModels);
        }


        [Route("/Shipping/GetShippingInfo/{id}")]
        [HttpGet]

        public async Task<IActionResult> GetShippingInfo(long id)
        {
            var invoice = await _shippingService.GetByIdAsync(id);
            var model = new DetailsViewModel()
            {
                Id = id,
                SupplierId = invoice.SupplierId,
                ToStoreId = invoice.ToStoreId,
                ReferenceNo = invoice.ReferenceNo,
                Date = invoice.Date.ToString("yyyy-MM-dd"),
                AttachedDoc = invoice.AttachedDoc,
                Description = invoice.Description,
                Status = invoice.Status == true ? "Approved" : "OnHold",
                StrDiscount = invoice.Discount != null ? invoice.Discount.Value.ToString("0.00") : "0.00",
                StrOtherCharges = invoice.OtherCharges != null ? invoice.OtherCharges.Value.ToString("0.00") : "0.00",
                StrTotal = invoice.Total != null ? invoice.Total.Value.ToString("0.00") : "0.00",
                ChargesDescription = invoice.ChargesDescription,
                RecordType = invoice.RecordType,
                CreatedBy = invoice.CreatedBy,
                CratedDate = invoice.CratedDate != null ? invoice.CratedDate.Value.ToString("yyyy-MM-dd") : null,
                UpdatedBy = invoice.UpdatedBy,
                UpdatedDate = invoice.UpdatedDate != null ? invoice.UpdatedDate.Value.ToString("yyyy-MM-dd") : null,
            };

            return Json(data: model);
        }

        [Route("/Shipping/GetSCategoryAutocomplete")]
        [HttpPost]
        public async Task<IActionResult> GetCategoryListAsync(string Prefix)
        {
            var categories = await _commonService.GetCategorieAutocomplete(Prefix);

            //List<CategoryViewModel> catList = new List<CategoryViewModel>();
            //var parentCats = categories.Where(c => c.ParentCategoryId == 0).ToList();

            //foreach (var catItem in parentCats)
            //{
            //    var curCats = new CategoryViewModel();
            //    curCats.Id = catItem.Id;
            //    curCats.ParentId = catItem.ParentCategoryId;
            //    curCats.CategoryNameEng = catItem.NameEng;
            //    curCats.CategoryNameArabic = catItem.NameArabic;
            //    catList.Add(curCats);

            //    await _commonService.GetCategoryHierarchy(catItem.Id, catList, catList, 0);
            //}

            var catInfo = categories.Select(t => new CategoryViewModel()
            {
                Id = t.Id,
                ParentId = t.ParentCategoryId,
                CategoryNameEng = t.NameEng,
                CategoryNameArabic=t.NameArabic,
            }).ToList();

            return Json(data: catInfo);
        }


        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        [Route("/Shipping/CreateBarcode/{storeId?}")]
        [HttpPost]
        public async Task<IActionResult> CreateBarcode(int storeId, string barcode, decimal price, string desc1, string desc2, string refno, string style)
        {
            //try
            //{
                string webRootPath2 = _webHostEnvironment.WebRootPath;
                string barcodePath = webRootPath2 + "/Images/ProductBarcodes/";
                string logoPath = webRootPath2 + "/Images/Logo/";
                string curLogo = logoPath + "blank.png";
                string arabicFontPath = webRootPath2 + "/Fonts/arial.ttf";

                //  string categoryName = "";

                string productName = desc2 + " " + desc1;

            System.Diagnostics.Debug.WriteLine("Here it is");

            // var storeInfo = await _storeService.GetByIdAsync(storeId);
            //  string storeName = "Finelook / فاين لوك";// storeInfo.Brand??" ";
            string storeName = "Finelook  /";
            string strsub = style;
            string logoImg = "finelook_store_logo.png";
            //if (storeId > 0)
            //{
            //    logoImg = await _reportHeadService.GetByStoreLogoByIdAsync(storeId);
            //}

            if (!string.IsNullOrEmpty(logoImg))
                {
                    curLogo = logoPath + logoImg;
                }


                string curPrice = "SAR " + price.ToString("#,0.00");

                string docName = barcode + ".pdf";
                string docPath = Path.Combine(barcodePath, docName);
                //  string docPath = barcodePath + docName;

                if (System.IO.File.Exists(docPath))
                {
                    System.IO.File.Delete(docPath);
                }
                FileStream file = new FileStream(docPath, System.IO.FileMode.CreateNew);
                iTextSharp.text.Rectangle rec2 = new iTextSharp.text.Rectangle(48, 18);

                iTextSharp.text.pdf.PdfWriter writer = null;
                iTextSharp.text.Document doc = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(48, 18), 5, 5, 1, 1);
                writer = PdfWriter.GetInstance(doc, file);
                doc.Open();

            iTextSharp.text.Image logoImage = null;

            if (System.IO.File.Exists(curLogo))
            {
                System.Drawing.Image logox = System.Drawing.Image.FromFile(curLogo);

                Bitmap b = new Bitmap(logox);

                System.Drawing.Image newLogoImage = resizeImage(b, new Size(60, 40));

                logoImage = iTextSharp.text.Image.GetInstance(newLogoImage, System.Drawing.Imaging.ImageFormat.Png);

                logoImage.SetAbsolutePosition(1.7f, 14f);
                logoImage.ScalePercent(10f);
                // logoImage.ScaleAbsoluteHeight(5f);
                // logoImage.ScaleAbsoluteWidth(20f);

            }

                BaseFont bf0 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED); // BaseFont.CreateFont(arabicFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font EFT_Beigale_Heavy = new iTextSharp.text.Font(bf0, 1.6f);
                iTextSharp.text.Font XEFT_Beigale_Heavy = new iTextSharp.text.Font(bf0, 1.5f);
                ColumnText column = new ColumnText(writer.DirectContent);
                column.SetSimpleColumn(2f, 19f, 36f, 14f);
                // column.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                Paragraph p = new Paragraph(storeName, EFT_Beigale_Heavy);
                p.Alignment = Element.ALIGN_CENTER;
                Paragraph p1 = new Paragraph(strsub, XEFT_Beigale_Heavy);
                p1.Alignment = Element.ALIGN_CENTER;
                column.AddElement(p);
                column.AddElement(p1);
                column.Go();

            BaseFont bfar = BaseFont.CreateFont(arabicFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font EFT_Beigale_Heavyar = new iTextSharp.text.Font(bfar, 1.7f);
            ColumnText columnar = new ColumnText(writer.DirectContent);
            columnar.SetSimpleColumn(2f, 19f, 29f, 14f);
            columnar.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            columnar.AddElement(new Paragraph("فاين لوك", EFT_Beigale_Heavyar));
            columnar.Go();


            PdfContentByte cb2 = writer.DirectContent;
                BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                // Change the font size here
                // cb2.ShowTextAligned= Element.ALIGN_LEFT;
                cb2.SetFontAndSize(bf1, 2f);
                cb2.BeginText();
                cb2.SetTextMatrix(1.0f, 1.0f);
                cb2.ShowText(curPrice);
                cb2.EndText();


                BaseFont bf01 = BaseFont.CreateFont(arabicFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font EFT_Beigale_Heavy2 = new iTextSharp.text.Font(bf01, 1.4f);
                ColumnText column3 = new ColumnText(writer.DirectContent);
                column3.SetSimpleColumn(2f, 5f, 40f, 2f);
                column3.Alignment = Element.ALIGN_CENTER;
                column3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                Paragraph p2 = new Paragraph(productName, EFT_Beigale_Heavy2);
                p2.Alignment = Element.ALIGN_CENTER;
                column3.AddElement(p2);
                column3.Go();


            BaseFont bf2 = BaseFont.CreateFont(arabicFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font EFT_Beigale_Heavy1 = new iTextSharp.text.Font(bf2, 1.1f);
            ColumnText column1 = new ColumnText(writer.DirectContent);
            column1.SetSimpleColumn(2f, 3f, 42f, 1.2f);
            column1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            column1.AddElement(new Paragraph("يشمل ١٥٪ ضريبة القيمة المضافة", EFT_Beigale_Heavy1));
            column1.Go();



            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                bc.TextAlignment = Element.ALIGN_CENTER;
                bc.Code = barcode;
                bc.StartStopText = false;
                bc.CodeType = iTextSharp.text.pdf.Barcode128.CODABAR;
                bc.Extended = true;

                iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb,
                  iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK);

                cb.SetTextMatrix(3.5f, 5.0f);
                img.ScaleToFit(135, 10);
                img.SetAbsolutePosition(4.5f, 4);
                cb.AddImage(img);
                if (System.IO.File.Exists(curLogo))
                {
                    doc.Add(logoImage);
                }
                doc.Close();


                // return new JsonResult(new { docName });
                return Json(data: docName);
            //}
            //catch (Exception e)
            //{
            //    throw e.InnerException;
            //}

        }


        [Route("/Shipping/ShippmentReportCreate/{id}")]
        [HttpPost]
        public async Task<IActionResult> ShippmentReportCreate(long id, string colnames, string pagetype)
        {
            var shippingInfo = await _shippingService.GetByIdAsync(id);
            var shippingItems = await _shippingService.GetItemsByRequestIdAsync(id);
            var storeInfo = await _storeService.GetByIdAsync(shippingInfo.ToStoreId);
            var supplierInfo = await _purchaseService.GetSupplierByIdAsync(shippingInfo.SupplierId);

          //  string selectedFields = Request.Form["PrintColumns"];
          //  { "Barcode","Description English","BoxNo","QtyPerBox","Qty","Price","Item Total" };
            string[] fields = colnames.Split(',');


            string[] strCols = { "Barcode","Description English", "Description Arabic","Department","Seasson","Style","Size","Color","Year","BoxNo","QtyPerBox","Qty","Price","SalePrice","SaleVat","OriginalPrice","Item Total" };

            string Title = "Shipment Dispatch";
            string subTitle = "Receiver Information: " + storeInfo.Name + ", " + storeInfo.Address;
            string OtherInfo = "";
            if (!string.IsNullOrEmpty(storeInfo.Phone))
            {
                OtherInfo += " TEL: " + storeInfo.Phone;
            }
            if (!string.IsNullOrEmpty(storeInfo.VatNo))
            {
                OtherInfo += " VAT No: " + storeInfo.VatNo;
            }


            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Reports/";

            string fileName = "Shipment.pdf";

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
            html += "<td style='text-align:center; width:80%;'><p style='font-size:13px; padding:0px; margin:0px; border-bottom:1px solid #454545;'>" + OtherInfo + "</p></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:left; width:60%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td colspan='2' style='text-align:left; width:100%;'>Supplier Info</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Name</td>";
            html += "<td style='text-align:left; width:70%;'>" + supplierInfo.CompanyName + "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Address</td>";
            html += "<td style='text-align:left; width:70%;'>" + supplierInfo.Address + "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>Telephone</td>";
            html += "<td style='text-align:left; width:70%;'>" + supplierInfo.PhoneNo + "</td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:left; width:30%;'>VAT No</td>";
            html += "<td style='text-align:left; width:70%;'>" + supplierInfo.VatNo + "</td>";
            html += "</tr>";
            html += "</table>";
            html += "</td>";
            html += "<td style='text-align:right; width:40%; text-align:left; font-size:11px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<table style='width:100%; font-size:13px; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;'>";
            html += "<tr>";
            html += "<td style='text-align:right; width:70%;'>Shipment No: <b>" + shippingInfo.ReferenceNo + "</b></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<td style='text-align:right; width:70%;'>Date: " + shippingInfo.Date.ToString("dd/MM/yyyy") + "</td>";
            html += "</tr>";
            html += "</table>";
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
                    if(h == "Seasson") { hText = "Season"; }
                    if (h == "QtyPerBox") { hText = "QPBox"; }
                    if (h == "OriginalPrice") { hText = "Ex. Vat"; }
                    html += "<th style='padding:3px; border:1px solid #454545;'>" + hText + "</th>";
                }

            }
            html += "</tr>";

            int rowx = 0;
            decimal totalAmount = 0;
            int totQty = 0;
            foreach (var data in shippingItems)
            {
                rowx++;
                html += "<tr style='page-break-inside: avoid;'>";
                html += "<td style='padding:3px; width:5%; border:1px solid #454545;'>" + rowx.ToString() + "</td>";
                foreach (var item in strCols)
                {
                    if (item == "Barcode" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.Barcode + "</td>";
                    }

                    if (item == "Description English" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.DescriptionEng + "</td>";
                    }
                    if (item == "Description Arabic" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.DescriptionArabic + "</td>";
                    }

                    if (item == "Department" && fields.Contains(item))
                    {
                        if (data.DepartmentId != null && data.DepartmentId != 0)
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + await _commonService.GetDepartmentNameByIdAsync(data.DepartmentId) + "</td>";
                        }
                        else
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'></td>";
                        }
                    }

                    if (item == "Seasson" && fields.Contains(item))
                    {
                        if (data.SeassonId != null && data.SeassonId !=0)
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + await _commonService.GetSessonNameByIdAsync(data.SeassonId) + "</td>";
                        }
                        else
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'></td>";
                        }
                    }

                    if (item == "Style" && fields.Contains(item))
                    {
                        if (data.SkuId != null && data.SkuId != 0 )
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + await _commonService.GetSkuNameByIdAsync(data.SkuId) + "</td>";
                        }
                        else
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'></td>";
                        }
                    }

                    if (item == "Size" && fields.Contains(item))
                    {
                        if (data.SizeId != null && data.SizeId != 0)
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + await _commonService.GetSizeNameByIdAsync(data.SizeId) + "</td>";
                        }
                        else
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'></td>";
                        }
                    }

                    if (item == "Color" && fields.Contains(item))
                    {
                        if (data.ColorId != null && data.ColorId != 0)
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + await _commonService.GetColorNameByIdAsync(data.ColorId) + "</td>";
                        }
                        else
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'></td>";
                        }
                    }

                    if (item == "Year" && fields.Contains(item))
                    {
                        if (data.YearId != null && data.YearId != 0)
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'>" + await _commonService.GetYearByIdAsync(data.YearId) + "</td>";
                        }
                        else
                        {
                            html += "<td style='padding:3px; border:1px solid #454545;'></td>";
                        }
                    }

                    if (item == "BoxNo" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545;'>" + data.BoxNo + "</td>";
                    }

                    if (item == "QtyPerBox" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.QtyPerBox + "</td>";
                    }

                    if (item == "Qty" && fields.Contains(item))
                    {
                        totQty += (data.Qty ?? 0);
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Qty + "</td>";
                    }

                    if (item == "Price" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.Price + "</td>";
                    }

                    if (item == "SalePrice" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.SalePrice + "</td>";
                    }

                    if (item == "SaleVat" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.SaleVat + "</td>";
                    }

                    if (item == "OriginalPrice" && fields.Contains(item))
                    {
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + data.OriginalPrice + "</td>";
                    }

                    if (item == "Item Total" && fields.Contains(item))
                    {
                        decimal rowTot = (data.Qty ?? 0) * (data.Price ?? 0);
                        html += "<td style='padding:3px; border:1px solid #454545; text-align:right;'>" + rowTot.ToString("0.00") + "</td>";
                    }


                }
        html += "</tr>";
        }

            int colCount = fields.Length;
            //// "Barcode,Description English, Description Arabic,Department,Seasson,Style,Size,Color,Year,BoxNo,QtyPerBox,Qty,Price,SalePrice,SaleVat,OriginalPrice,Item Total"
            html += "<tr style='page-break-inside: avoid;'>";
            html += "<td colspan='" + colCount + "' style='text-align:left; border:1px solid #454545;'>Discount</td>";
            html += "<td style='text-align:right; border:1px solid #454545; text-align:right;'>" + (shippingInfo.Discount != null ? shippingInfo.Discount.Value.ToString("0.00") : "0.00") + "</td>";
            html += "<tr>";
            html += "<td colspan='" + colCount + "' style='text-align:left; border:1px solid #454545;'>Other Charges</td>";
            html += "<td style='text-align:right; border:1px solid #454545; text-align:right;'>" + (shippingInfo.OtherCharges != null ? shippingInfo.OtherCharges.Value.ToString("0.00") : "0.00") + "</td>";
            html += "<tr>";
            //   "SalePrice","SaleVat","OriginalPrice"
            int mergeX = 0;
            if (fields.Contains("SalePrice")){
                mergeX++;
            }
            if (fields.Contains("SaleVat")){
                mergeX++;
            }
            if (fields.Contains("OriginalPrice")){
                mergeX++;
            }
            int mergeColx = mergeX + 2;

            html += "<td colspan='" + (colCount - mergeColx) + "' style='text-align:left; border:1px solid #454545;'>Total</td>";
            html += "<td style='text-align:right; border:1px solid #454545;'>" + totQty.ToString() + "</td>";
            html += "<td colspan='" + (mergeX + 1) + "' style='text-align:left; border:1px solid #454545;'></td>";
            html += "<td style='text-align:right; border:1px solid #454545; text-align:right;'>" + (shippingInfo.Total != null ? shippingInfo.Total.Value.ToString("0.00") : "0.00") + "</td>";
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

            //    var fs = new FileStream(FilePath1, FileMode.Open);
            //  return File(fs, "application/octet-stream", fileName);
            try
            {
                return Json(fileName);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            //TempData["Error"] = "<div style='width:100%; background-color:#f00; padding:5px; color:#fff; text-align:center; z-index:99999;'>Records not found!</div>";
            // return RedirectToAction("SalaryReports");
        }

        #region Export To Excel

        [Route("/Shipping/ExportToExcel/{id}")]
        [HttpGet]
        public async Task<IActionResult> ShippmentReportCreate(long id)
        {
            using (var workbook = new XLWorkbook())
            {
                var invoice = await _shippingService.GetByIdAsync(id);
                var supplierInfo = await _purchaseService.GetSupplierByIdAsync(invoice.SupplierId);
                var storeInfo = await _storeService.GetByIdAsync(invoice.ToStoreId);
                string title = "Shipment Dispatch";
                string supplierTitle = "Supplier Details";
                string storeTitle = "Store Details";
                string shipmentTitle = "Shipment Details";
                var worksheet = workbook.Worksheets.Add("Shipment");
                var currentRow = 11;

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

                worksheet.Cell("B4").Value = supplierInfo.CompanyName;
                worksheet.Cell("B5").Value = supplierInfo.Address;

                worksheet.Cell("G4").Value = "VAT No";
                worksheet.Cell("G4").Style.Font.Bold = true;
                worksheet.Cell("G5").Value = "Telephone";
                worksheet.Cell("G5").Style.Font.Bold = true;

                worksheet.Cell("H4").Value = supplierInfo.VatNo;
                worksheet.Cell("H5").Value = supplierInfo.PhoneNo;

                #endregion

                #region Store Details

                worksheet.Range("A6:D6").Merge();
                worksheet.Range("A6:D6").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A6:D6").Style.Font.Bold = true;
                worksheet.Range("A6:D6").Style.Font.FontSize = 16;

                var storeHeaderCell = worksheet.Cell("A6");
                storeHeaderCell.Value = storeTitle;
                storeHeaderCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                storeHeaderCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell("A7").Value = "Store";
                worksheet.Cell("A7").Style.Font.Bold = true;
                worksheet.Cell("A8").Value = "Address";
                worksheet.Cell("A8").Style.Font.Bold = true;

                worksheet.Cell("B7").Value = storeInfo.Name;
                worksheet.Cell("B8").Value = storeInfo.Address;

                worksheet.Cell("G7").Value = "Code";
                worksheet.Cell("G7").Style.Font.Bold = true;
                worksheet.Cell("G8").Value = "Vat No";
                worksheet.Cell("G8").Style.Font.Bold = true;

                worksheet.Cell("H7").Value = storeInfo.StoreCode;
                worksheet.Cell("H8").Value = storeInfo.VatNo;

                #endregion

                #region Shipment Details

                worksheet.Range("A9:D9").Merge();
                worksheet.Range("A9:D9").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A9:D9").Style.Font.Bold = true;
                worksheet.Range("A9:D9").Style.Font.FontSize = 16;

                var invoiceHeaderCell = worksheet.Cell("A9");
                invoiceHeaderCell.Value = shipmentTitle;
                invoiceHeaderCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                invoiceHeaderCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell("A10").Value = "Reference No";
                worksheet.Cell("A10").Style.Font.Bold = true;
                worksheet.Cell("B10").Value = invoice.ReferenceNo;

                worksheet.Cell("G10").Value = "Date";
                worksheet.Cell("G10").Style.Font.Bold = true;
                worksheet.Cell("H10").Value = invoice.Date.ToString("yyyy-MM-dd");

                #endregion

                #region Shipping Items Details

                #region Header

                worksheet.Row(currentRow).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 1).Value = "No.";
                worksheet.Cell(currentRow, 2).Value = "Barcode";
                worksheet.Cell(currentRow, 3).Value = "Description English";
                worksheet.Cell(currentRow, 4).Value = "Description Arabic";
                worksheet.Cell(currentRow, 5).Value = "Seasson";
                worksheet.Cell(currentRow, 6).Value = "Department";
                worksheet.Cell(currentRow, 7).Value = "Model/SKU";
                worksheet.Cell(currentRow, 8).Value = "Size";
                worksheet.Cell(currentRow, 9).Value = "Color";
                worksheet.Cell(currentRow, 10).Value = "Year";
                worksheet.Cell(currentRow, 11).Value = "Box No";
                worksheet.Cell(currentRow, 12).Value = "Qty per Box";
                worksheet.Cell(currentRow, 13).Value = "Qty";
                worksheet.Cell(currentRow, 14).Value = "Qty Received";
                worksheet.Cell(currentRow, 15).Value = "Price";
                worksheet.Cell(currentRow, 16).Value = "Total";
                worksheet.Cell(currentRow, 17).Value = "Selling Price";
                worksheet.Cell(currentRow, 18).Value = "Selling Vat";
                worksheet.Cell(currentRow, 19).Value = "Org. Price";

                #endregion

                #region Body

                var purchaseItems = await _shippingService.GetItemsByRequestIdAsync(id);
                decimal? totalQtys = 0;
                decimal? grandTotal = 0;
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
                        worksheet.Cell(currentRow, 7).Value = item.SkuId != null && item.SkuId != 0 ? await _commonService.GetSkuCodeByIdAsync(item.SkuId.Value) : "";
                        worksheet.Cell(currentRow, 8).Value = item.SizeId != null && item.SizeId != 0 ? await _commonService.GetSizeNameByIdAsync(item.SizeId.Value) : "";
                        worksheet.Cell(currentRow, 9).Value = item.ColorId != null && item.ColorId != 0 ? await _commonService.GetColorNameByIdAsync(item.ColorId.Value) : "";
                        worksheet.Cell(currentRow, 10).Value = item.YearId != null && item.YearId != 0 ? await _commonService.GetYearByIdAsync(item.YearId.Value) : "";
                        worksheet.Cell(currentRow, 11).Value = item.BoxNo;
                        worksheet.Cell(currentRow, 12).Value = item.QtyPerBox;
                        worksheet.Cell(currentRow, 13).Value = item.Qty;
                        totalQtys += item.Qty;
                        worksheet.Cell(currentRow, 14).Value = item.ReceivedQty;
                        worksheet.Cell(currentRow, 15).Value = item.Price;

                        decimal totalX = 0;
                        decimal priceX = item.Price ?? 0;
                        int qtyX = item.Qty ?? 0;
                        totalX = priceX * qtyX;
                        grandTotal += totalX;

                        worksheet.Cell(currentRow, 16).Value = totalX;
                        worksheet.Cell(currentRow, 17).Value = item.SalePrice != null ? item.SalePrice.Value.ToString("0.00") : "";
                        worksheet.Cell(currentRow, 18).Value = item.SaleVat != null ? item.SaleVat.Value.ToString("0.00") : "";
                        worksheet.Cell(currentRow, 19).Value = item.OriginalPrice != null ? item.OriginalPrice.Value.ToString("0.00") : "";
                    }
                    
                    var newRow = worksheet.Range("H" + currentRow + ":R" + currentRow);

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":R" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Discount";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var discountCell = worksheet.Cell("S" + currentRow);
                    discountCell.Value = invoice.Discount;
                    discountCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":R" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Charges";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var chargesCell = worksheet.Cell("S" + currentRow);
                    chargesCell.Value = invoice.OtherCharges;
                    chargesCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    currentRow++;
                    newRow = worksheet.Range("H" + currentRow + ":L" + currentRow);
                    newRow.Merge();
                    newRow.Value = "Total";
                    newRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    newRow.Style.Font.Bold = true;
                    newRow.Style.Font.FontSize = 13;

                    var totalDznCell = worksheet.Cell("M" + currentRow);
                    totalDznCell.Value = totalQtys;
                    totalDznCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    var totalCell = worksheet.Cell("S" + currentRow);
                    totalCell.Value = grandTotal;
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
                        "ShipmentDispatch_" + id + ".xlsx"
                        );
                }
            }
        }

        #endregion

    }
}
