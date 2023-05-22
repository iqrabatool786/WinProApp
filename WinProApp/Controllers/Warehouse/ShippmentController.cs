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

namespace WinProApp.Controllers.Warehouse
{
    public class ShippmentController : BasedUserController
    {
        public readonly CommonService _commonService;
        public readonly ShippingService _shippingService;
        public readonly PurchaseService _purchaseService;
        public readonly StoreService _storeService;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ShippmentController(CommonService commonService, ShippingService shippingService, PurchaseService purchaseService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _commonService = commonService;
            _shippingService = shippingService;
            _purchaseService = purchaseService;
            _storeService = storeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/ReceiveShipment")]
        [HttpGet]
        public async Task<IActionResult> ReceiveShipment()
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
    }
}
