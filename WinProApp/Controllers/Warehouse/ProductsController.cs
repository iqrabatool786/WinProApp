using BarcodeLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iText;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Data;
using System.Drawing;
using System.Text;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels;
using WinProApp.ViewModels.Warehouse.Products;
using static Humanizer.In;
using Image = iTextSharp.text.Image;
using iText.IO.Image;
using System.IO;
using System.Drawing.Drawing2D;
using iText.Kernel.Colors;
using iText.Layout;
using OfficeOpenXml;

namespace WinProApp.Controllers.Warehouse
{
    public class ProductsController : BasedUserController
    {
        public readonly ProductInfoService _productService;
        public readonly CommonService _commonService;
        public readonly StoreService _storeService;
        public readonly ReportHeadService _reportHeadService;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ProductInfoService productService, CommonService commonService, StoreService storeService, ReportHeadService reportHeadService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _commonService = commonService;
            _storeService = storeService;
            _reportHeadService = reportHeadService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Warehouse/Products")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/ProductList")]
        [HttpPost]
        public IActionResult ProductList(JQueryDataTableParamModel param)
        {
            var results = _productService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                CompanyId = x.CompanyId,
                CompanyName= x.CompanyId !=null? _commonService.GetCompanyNameByIdAsync(x.CompanyId.Value).Result:null,
                CategoryId=x.CategoryId,
                CategoryName= x.CategoryId != null ? _commonService.GetCategoryNameByIdAsync(x.CategoryId.Value).Result : null,
                DepartmentId=x.DepartmentId,
                DepartmentName=x.DepartmentId !=null? _commonService.GetDepartmentNameByIdAsync(x.DepartmentId.Value).Result : null,
                SeasonId = x.SeasonId,
                SessionName = x.SeasonId !=null? _commonService.GetSessonNameByIdAsync(x.SeasonId.Value).Result : null,
                DescriptionId=x.DescriptionId,
                Description= x.DescriptionId!=null? _commonService.GetDescriptionByIdAsync(x.DescriptionId.Value).Result : null,
                ColorId=x.ColorId,
                ColorName = x.ColorId != null? _commonService.GetColorNameByIdAsync(x.ColorId.Value).Result : null,
                SizeId=x.SizeId,
                SizeName=x.SizeId !=null ? _commonService.GetSizeNameByIdAsync(x.SizeId.Value).Result : null,
                SkuId=x.SkuId,
                SkuName= x.SkuId !=null ? _commonService.GetSkuNameByIdAsync(x.SkuId.Value).Result : null,
                Unitid=x.Unitid,
                UnitName = x.Unitid !=null? _commonService.GetUnitNameByIdAsync(x.Unitid.Value).Result : null,
                VendorId=x.VendorId,
                VendorName= x.VendorId != null? _commonService.GetVendorNameByIdAsync(x.VendorId.Value).Result : null,
                YearId = x.YearId,
                YearName = x.YearId != null? _commonService.GetYearByIdAsync(x.YearId.Value).Result : null,
                BrandId=x.BrandId,
                BrandName=x.BrandId!=null? _commonService.GetBrandNameByIdAsync(x.BrandId.Value).Result : null,
                GroupId=x.GroupId,
                GroupName=x.GroupId!=null? _commonService.GetGroupNameByIdAsync(x.GroupId.Value).Result : null,
                ProductNameEng =x.ProductNameEng,
                ProductNameArabic=x.ProductNameArabic,
                MfgDate=x.MfgDate !=null? x.MfgDate.Value.ToString("yyyy-MM-dd"):null,
                ExpDate=x.ExpDate !=null? x.ExpDate.Value.ToString("yyyy-MM-dd") : null,
                ProductInitialPrice=x.ProductInitialPrice,
                CostPrice=x.CostPrice,
                SalePrice=x.SalePrice,
                Discount=x.Discount,
                UnitBarcode=x.UnitBarcode,
                UnitDescription=x.UnitDescription,
                QtyPerUnit=x.QtyPerUnit,
                UnitSalePrice=x.UnitSalePrice,
                UnitCost=x.UnitCost,
                BoxNo=x.BoxNo,
                MinQty=x.MinQty,
                PackBarcode=x.PackBarcode,
                PackDescription=x.PackDescription,
                QtyPerPack=x.QtyPerPack,
                PackPrice=x.PackPrice,
                PackCost=x.PackCost,
                OreginalPrice=x.OreginalPrice,
                Vat=x.Vat,
                //CustomField1=x.CustomField1,
                //CustomField2=x.CustomField2,
                //CustomField3=x.CustomField3,
                //CustomField4=x.CustomField4,
                Image=x.Image??"",
                WarrantyPeriod=x.WarrantyPeriod,
                Currentstock=x.Currentstock??0,
                Status=x.Status==true?"Yes":"No",
                CreatedBy=x.CreatedBy,
                CratedDate=x.CratedDate !=null?x.CratedDate.Value.ToString("yyyy-MM-dd"):null,
                UpdatedBy=x.UpdatedBy,
                UpdatedDate=x.UpdatedDate!=null?x.UpdatedDate.Value.ToString("yyyy-MM-dd") : null,
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

        [Route("/Warehouse/CreateProduct")]
        [HttpGet]
        public async Task<IActionResult> Create(System.Globalization.CultureInfo requestCulture)
        {
            var suppliers = await _commonService.GetAllSuppliersAsync();
            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            var categories = await _commonService.GetCategoriesAsync();
            var colors = await _commonService.GetColorsAsync();
            var sizes = await _commonService.GetSizesAsync();
            var departments = await _commonService.GetDepartmentsAsync();
            var seassons = await _commonService.GetSeassonsAsync();
            var vendors = await _commonService.GetVendorsAsync();
            var years = await _commonService.GetAllYearsAsync();
            var descs = await _commonService.GetDescriptionsAsync();
            var brands = await _commonService.GetBrandsAsync();
            var groups = await _commonService.GetGroupsAsync();

            var styles = await _commonService.GetStylesAsync();
            var storeList = await _storeService.GetByAllAsync();

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

            ViewBag.Styles = new SelectList(styles, "Id", "Code");
            ViewBag.Vendores = new SelectList(vendors, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Descriptions = new SelectList(descs, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Brands = new SelectList(brands, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Groups = new SelectList(groups, "Id", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.Years = new SelectList(years, "Id", "YearName");
            ViewBag.Stores = new SelectList(storeList, "Id", "Name");

            return View();
        }

        [Route("/Warehouse/CreateProduct")]
        [HttpPost]
        public async Task<IActionResult> Create(AddViewModel model, IFormFile AttachedDoc)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/Products/";

                string imgName = null;
                if (AttachedDoc != null && AttachedDoc.Length > 0)
                {
                    string curDocName = Path.GetFileName(AttachedDoc.FileName);
                    string curDocExtention = Path.GetExtension(AttachedDoc.FileName);

                    imgName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curDocExtention;
                    string DocSavePath = System.IO.Path.Combine(uploadPath, imgName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await AttachedDoc.CopyToAsync(stream);
                    }

                }


                long curId = 0;

                if (model.Id > 0)
                {
                    curId = model.Id;

                    var product = await _productService.GetByIdAsync(curId);
                    long curStock = product.Currentstock ?? 0;
                    long newStock = model.NewStock ?? 0;
                    long stock = curStock + newStock;
                    string? oldImage = product.Image;
                    product.ProductId = model.ProductId;
                    product.CompanyId = model.CompanyId;
                    product.CategoryId = model.CategoryId;
                    product.DepartmentId = model.DepartmentId;
                    product.SeasonId = model.SeasonId;
                    product.DescriptionId = model.DescriptionId;
                    product.ColorId = model.ColorId;
                    product.SizeId = model.SizeId;
                    product.SkuId = model.SkuId;
                    product.Unitid = model.Unitid;
                    product.BrandId = model.BrandId;
                    product.VendorId = model.VendorId;
                    product.GroupId = model.GroupId;
                    product.YearId = model.YearId;
                    product.ProductNameEng = model.ProductNameEng;
                    product.ProductNameArabic = model.ProductNameArabic;
                    product.MfgDate = model.MfgDate;
                    product.ExpDate = model.ExpDate;
                    product.ProductInitialPrice = model.Price1;
                    product.CostPrice = model.CostPrice;
                    product.SalePrice = model.Price1 + model.Price1Vat;
                    product.Discount = model.Discount1;
                    product.UnitBarcode = model.UnitBarcode;
                    product.UnitDescription = model.UnitDescription;
                    product.QtyPerUnit = model.QtyPerUnit;
                    product.UnitSalePrice = model.UnitSalePrice;
                    product.UnitCost = model.UnitCost;
                    product.BoxNo = model.BoxNo;
                    product.MinQty = model.MinQty;
                    product.PackBarcode = model.PackBarcode;
                    product.PackDescription = model.PackDescription;
                    product.QtyPerPack = model.QtyPerPack;
                    product.PackPrice = model.PackPrice;
                    product.PackCost = model.PackCost;
                    product.OreginalPrice = model.OrgPrice1;
                    product.Vat = model.Price1Vat;
                    //product.CustomField1 = model.CustomField1;
                    //product.CustomField2 = model.CustomField2;
                    //product.CustomField3 = model.CustomField3;
                    //product.CustomField4 = model.CustomField4;
                    if(imgName != null && oldImage != null)
                    {
                        string oldImgPath = uploadPath + oldImage;

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    if (imgName != null)
                    {
                        product.Image = imgName;
                    }
                    else
                    {
                        product.Image = oldImage;
                    }
                    product.WarrantyPeriod = model.WarrantyPeriod;
                    product.Currentstock = stock;
                    product.Status = true;
                    product.UpdatedDate = DateTime.Now;
                    product.UpdatedBy = User.Identity.Name;
                    
                    await _productService.UpdateAsync(product);
                }
                else
                {
                    var product = new ProductInfo();
                    product.ProductId = model.ProductId;
                    product.CompanyId = model.CompanyId;
                    product.CategoryId = model.CategoryId;
                    product.DepartmentId = model.DepartmentId;
                    product.SeasonId = model.SeasonId;
                    product.DescriptionId = model.DescriptionId;
                    product.ColorId = model.ColorId;
                    product.SizeId = model.SizeId;
                    product.SkuId = model.SkuId;
                    product.Unitid = model.Unitid;
                    product.BrandId = model.BrandId;
                    product.VendorId = model.VendorId;
                    product.GroupId = model.GroupId;
                    product.YearId = model.YearId;
                    product.ProductNameEng = model.ProductNameEng;
                    product.ProductNameArabic = model.ProductNameArabic;
                    product.MfgDate = model.MfgDate;
                    product.ExpDate = model.ExpDate;
                    product.ProductInitialPrice = model.Price1;
                    product.CostPrice = model.CostPrice;
                    product.SalePrice = model.Price1 + model.Price1Vat;
                    product.Discount = model.Discount1;
                    product.UnitBarcode = model.UnitBarcode;
                    product.UnitDescription = model.UnitDescription;
                    product.QtyPerUnit = model.QtyPerUnit;
                    product.UnitSalePrice = model.UnitSalePrice;
                    product.UnitCost = model.UnitCost;
                    product.BoxNo = model.BoxNo;
                    product.MinQty = model.MinQty;
                    product.PackBarcode = model.PackBarcode;
                    product.PackDescription = model.PackDescription;
                    product.QtyPerPack = model.QtyPerPack;
                    product.PackPrice = model.PackPrice;
                    product.PackCost = model.PackCost;
                    product.OreginalPrice = model.OrgPrice1;
                    product.Vat = model.Price1Vat;
                    //product.CustomField1 = model.CustomField1;
                    //product.CustomField2 = model.CustomField2;
                    //product.CustomField3 = model.CustomField3;
                    //product.CustomField4 = model.CustomField4;
                    product.Image = imgName;
                    product.WarrantyPeriod = model.WarrantyPeriod;
                    product.Currentstock = model.NewStock;
                    product.Status = true;
                    product.CratedDate = DateTime.Now;
                    product.CreatedBy = User.Identity.Name;
                    product.UpdatedDate = DateTime.Now;
                    product.UpdatedBy = User.Identity.Name;
                    var result = await _productService.CreateAsync(product);
                    curId = result.Id;
                }

                

                var PriceInfo = new ProductPrice();

                PriceInfo.ProductId = curId;
                PriceInfo.Price1 = model.Price1;
                PriceInfo.Price1Vat = model.Price1Vat;
                PriceInfo.Price2 = model.Price2;
                PriceInfo.Price2Vat = model.Price2Vat;
                PriceInfo.Price3 = model.Price3;
                PriceInfo.Price3Vat = model.Price3Vat;
                PriceInfo.OrgPrice1 = model.OrgPrice1;
                PriceInfo.OrgPrice2 = model.OrgPrice2;
                PriceInfo.OrgPrice3 = model.OrgPrice3;
                PriceInfo.Discount1 = model.Discount1;
                PriceInfo.Discount2 = model.Discount2;
                PriceInfo.Discount3 = model.Discount3;

                if(model.PriceId > 0)
                {
                    PriceInfo.Id = model.PriceId??0;
                    await _productService.ProductPriceUpdateAsync(PriceInfo);
                }
                else
                {
                    await _productService.ProductPriceCreateAsync(PriceInfo);
                }


                return new JsonResult(new { id = curId });
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [Route("/Warehouse/DeleteProduct/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _productService.GetByIdAsync(id);
                await _productService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/GetDescriptionData")]
        [HttpPost]
        public async Task<IActionResult> GetDescriptionData(int id)
        {
            try
            {
                var result = await _commonService.GetDescriptionInfoByIdAsync(id);
                var desc = new Descriptions();
                desc.Id= result.Id;
                desc.NameEng = result.NameEng;
                desc.NameArabic = result.NameArabic;

                return Json(data: desc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/GetProductPrice")]
        [HttpPost]
        public async Task<IActionResult> GetProductPrice(long id)
        {
            try
            {
                long recordCount = await _productService.GetProductPriceCountByProductIdAsync(id);
                var model = new ProductPrice();
               
               
                if (recordCount > 0)
                {
                    var result = await _productService.GetProductPriceByProductIdAsync(id);
                    model.Id = result.Id;
                    model.Price1 = result.Price1;
                    model.Price1Vat = result.Price1Vat;
                    model.Price2 = result.Price2;
                    model.Price2Vat = result.Price2Vat;
                    model.Price3 = result.Price3;
                    model.Price3Vat = result.Price3Vat;
                    model.OrgPrice1 = result.OrgPrice1;
                    model.OrgPrice2 = result.OrgPrice2;
                    model.OrgPrice3 = result.OrgPrice3;
                    model.Discount1 = result.Discount1;
                    model.Discount2 = result.Discount2;
                    model.Discount3 = result.Discount3;
                    return Json(data: model);
                }
                else
                {
                    return Json(data: null);
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
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

        [Route("/Warehouse/CreateBarcode/{id}/{storeId?}")]
        [HttpPost]
        public async Task<IActionResult> CreateBarcode(long id, int storeId, decimal price, bool flag)
        {
            //try
            //{
                string webRootPath2 = _webHostEnvironment.WebRootPath;
                string barcodePath = webRootPath2 + "/Images/ProductBarcodes/";
                string logoPath = webRootPath2 + "/Images/Logo/";
                string curLogo = logoPath + "blank.png";
                string arabicFontPath = webRootPath2 + "/Fonts/arial.ttf";

                var productInfo = await _productService.GetByIdAsync(id);
                string barcode = productInfo.ProductId.ToString().Trim();

            string strStyle = "";

            if(productInfo.SkuId > 0)
            {
                strStyle = _commonService.GetStyleCodeByIdAsync(productInfo.SkuId.Value).Result ?? "";
            }

                //  string categoryName = "";

                string productName = productInfo.ProductNameArabic + " " + productInfo.ProductNameEng;


            //if(productInfo.CategoryId != null)
            //{
            //    var catInfo = await _commonService.GetCategoryByIdAsync(productInfo.CategoryId.Value);
            //    categoryName = catInfo.NameArabic + " " + catInfo.NameEng;
            //}

            //   var storeInfo = await _storeService.GetByIdAsync(storeId);
            //  string storeName = storeInfo.Name;
            string logoImg = "finelook_store_logo.png";// await _reportHeadService.GetByStoreLogoByIdAsync(storeId);

                if (!string.IsNullOrEmpty(logoImg))
                {
                    curLogo = logoPath + logoImg;
                }
            // string storeName = "Finelook / فاين لوك";
            string storeName = "Finelook  /";
            string curPrice = "SAR " + price.ToString("#,0.00");

            string strsub = strStyle;

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



                    BaseFont bf0 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);// BaseFont.CreateFont(arabicFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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


                    if (flag == true)
                    {
                        BaseFont bf2 = BaseFont.CreateFont(arabicFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font EFT_Beigale_Heavy1 = new iTextSharp.text.Font(bf2, 1.1f);
                        ColumnText column1 = new ColumnText(writer.DirectContent);
                        column1.SetSimpleColumn(2f, 3f, 42f, 1.2f);
                        column1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        column1.AddElement(new Paragraph("يشمل ١٥٪ ضريبة القيمة المضافة", EFT_Beigale_Heavy1));
                        column1.Go();
                    }



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
            //catch(Exception e)
            //{
            //    throw e.InnerException;
            //}

        }

        [Route("/Warehouse/AddNewProductSku/{code}")]
        [HttpPost]
        public async Task<IActionResult> AddNewProductSku(string code)
        {
            int SkuId = await _commonService.CheckCreateGetProductSku(code);
            return new JsonResult(new { id = SkuId });
        }

        [Route("/Warehouse/ItemUpload")]
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
                    string DocSavePath = Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await upload.CopyToAsync(stream);
                    }
                }
                Thread.Sleep(1000);
                List<ProductInfo> model = new List<ProductInfo>();
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
                            if(!string.IsNullOrEmpty(barcode))
                            {
                                productInfo = await _commonService.GetProductByBarcode(barcode);
                                if(productInfo != null)
                                {
                                    productInfo.SalePrice = ws.Cells[row, 2].Value != null ? Convert.ToDecimal(ws.Cells[row, 2].Value) : productInfo.SalePrice;
                                    productInfo.UnitSalePrice = ws.Cells[row, 3].Value != null ? Convert.ToDecimal(ws.Cells[row, 3].Value) : productInfo.UnitSalePrice;
                                    productInfo.OreginalPrice = ws.Cells[row, 4].Value != null ? Convert.ToDecimal(ws.Cells[row, 4].Value) : productInfo.OreginalPrice;
                                    productInfo.Vat = ws.Cells[row, 5].Value != null ? Convert.ToDecimal(ws.Cells[row, 5].Value) : productInfo.Vat;
                                    productInfo.ProductNameEng = ws.Cells[row, 6].Value != null ? Convert.ToString(ws.Cells[row, 6].Value) : productInfo.ProductNameEng;
                                    productInfo.ProductNameArabic = ws.Cells[row, 7].Value != null ? Convert.ToString(ws.Cells[row, 7].Value) : productInfo.ProductNameArabic;
                                    
                                    productInfo.UpdatedDate = DateTime.Now;
                                    var updatedProduct = await _commonService.UpdateProduct(productInfo);
                                    model.Add(productInfo);
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

        [Route("/Warehouse/DownloadSampleForUpdatePrice")]
        [HttpGet]
        public ActionResult DownloadSampleForUpdatePrice()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Docs/Download/";

            FilePath = System.IO.Path.Combine(FilePath, "UpdateProductPrice_format.xlsx");
            var fs = new FileStream(FilePath, FileMode.Open);

            return File(fs, "application/octet-stream", "UpdateProductPrice_format.xlsx");
        }

        [Route("/Warehouse/UploadItemsFromExcel")]
        [HttpPost]
        public async Task<IActionResult> UploadItemsFromExcel(IFormFile upload)
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
                List<ProductInfo> model = new List<ProductInfo>();
                if (docName != null)
                {
                    string docPath = Path.Combine(uploadPath, docName);
                    using (var package = new ExcelPackage(new FileInfo(docPath)))
                    {
                        var ws = package.Workbook.Worksheets[0];
                        for (int row = 2; row <= ws.Dimension.End.Row; row++)
                        {
                            string? barcode = ws.Cells[row, 1].Value != null ? ws.Cells[row, 1].Value.ToString() : null;
                            string? salePrice = ws.Cells[row, 2].Value != null ? ws.Cells[row, 2].Value.ToString() : null;
                            string? unitSalePrice = ws.Cells[row, 3].Value != null ? ws.Cells[row, 3].Value.ToString() : null;
                            string? originalPrice = ws.Cells[row, 4].Value != null ? ws.Cells[row, 4].Value.ToString() : null;
                            string? vat = ws.Cells[row, 5].Value != null ? ws.Cells[row, 5].Value.ToString() : null;
                            var productInfo = new ProductInfo();
                            if (!string.IsNullOrEmpty(barcode) &&
                                !string.IsNullOrEmpty(salePrice) &&
                                !string.IsNullOrEmpty(unitSalePrice) &&
                                !string.IsNullOrEmpty(originalPrice) &&
                                !string.IsNullOrEmpty(vat))
                            {
                                productInfo.ProductId = barcode;
                                productInfo.SalePrice = Convert.ToDecimal(salePrice);
                                productInfo.UnitSalePrice = Convert.ToDecimal(unitSalePrice);
                                productInfo.OreginalPrice = Convert.ToDecimal(originalPrice);
                                productInfo.Vat = Convert.ToDecimal(vat);

                                productInfo.SeasonId = ws.Cells[row, 6].Value != null ?
                                    await _commonService.CheckCreateGetProductSeasson(ws.Cells[row, 6].Value.ToString()) : null;

                                productInfo.DepartmentId = ws.Cells[row, 7].Value != null ?
                                   await _commonService.CheckCreateGetProductDepartment(ws.Cells[row, 7].Value.ToString()) : null;

                                productInfo.CategoryId = ws.Cells[row, 8].Value != null ?
                                   await _commonService.CheckCreateGetProductSeasson(ws.Cells[row, 8].Value.ToString()) : null;

                                productInfo.SkuId = ws.Cells[row, 9].Value != null ?
                                   await _commonService.CheckCreateGetProductSku(ws.Cells[row, 9].Value.ToString()) : null;

                                productInfo.SizeId = ws.Cells[row, 10].Value != null ?
                                   await _commonService.CheckCreateGetProductSize(ws.Cells[row, 10].Value.ToString()) : null;

                                productInfo.ColorId = ws.Cells[row, 11].Value != null ?
                                   await _commonService.CheckCreateGetProductColor(ws.Cells[row, 11].Value.ToString()) : null;

                                productInfo.Unitid = ws.Cells[row, 12].Value != null ?
                                   await _commonService.CheckCreateGetProductUnit(ws.Cells[row, 12].Value.ToString()) : null;

                                productInfo.BrandId = ws.Cells[row, 13].Value != null ?
                                   await _commonService.CheckCreateGetProductbrand(ws.Cells[row, 13].Value.ToString()) : null;

                                productInfo.VendorId = ws.Cells[row, 14].Value != null ?
                                   await _commonService.CheckCreateGetProductVendor(ws.Cells[row, 14].Value.ToString()) : null;

                                productInfo.GroupId = ws.Cells[row, 15].Value != null ?
                                   await _commonService.CheckCreateGetProductGroup(ws.Cells[row, 15].Value.ToString()) : null;

                                productInfo.YearId = ws.Cells[row, 16].Value != null ?
                                   await _commonService.CheckCreateGetProductYear(ws.Cells[row, 16].Value.ToString()) : null;

                                productInfo.ProductNameEng = ws.Cells[row, 17].Value != null ? ws.Cells[row, 17].Value.ToString() : null;
                                productInfo.ProductNameArabic = ws.Cells[row, 18].Value != null ? ws.Cells[row, 18].Value.ToString() : null;
                                productInfo.MfgDate = ws.Cells[row, 19].Value != null ? Convert.ToDateTime(ws.Cells[row, 19].Value.ToString()) : null;
                                productInfo.ExpDate = ws.Cells[row, 20].Value != null ? Convert.ToDateTime(ws.Cells[row, 20].Value.ToString()) : null;
                                productInfo.ProductInitialPrice = ws.Cells[row, 21].Value != null ? Convert.ToDecimal(ws.Cells[row, 21].Value) : null;
                                
                                productInfo.CostPrice = ws.Cells[row, 22].Value != null ? Convert.ToDecimal(ws.Cells[row, 22].Value) : null;
                                productInfo.Discount = ws.Cells[row, 23].Value != null ? Convert.ToDecimal(ws.Cells[row, 23].Value) : null;
                                productInfo.UnitBarcode = ws.Cells[row, 24].Value != null ? ws.Cells[row, 24].Value.ToString() : null;
                                productInfo.UnitDescription = ws.Cells[row, 25].Value != null ? ws.Cells[row, 25].Value.ToString() : null;
                                productInfo.QtyPerUnit = ws.Cells[row, 26].Value != null ? Convert.ToInt64(ws.Cells[row, 26].Value) : null;
                                productInfo.UnitCost = ws.Cells[row, 27].Value != null ? Convert.ToDecimal(ws.Cells[row, 27].Value) : null;
                                productInfo.BoxNo = ws.Cells[row, 28].Value != null ? Convert.ToString(ws.Cells[row, 28].Value) : null;
                                productInfo.MinQty = ws.Cells[row, 29].Value != null ? Convert.ToString(ws.Cells[row, 29].Value) : null;
                                productInfo.PackBarcode = ws.Cells[row, 30].Value != null ? Convert.ToString(ws.Cells[row, 30].Value) : null;
                                productInfo.PackDescription = ws.Cells[row, 31].Value != null ? Convert.ToString(ws.Cells[row, 31].Value) : null;
                                productInfo.QtyPerPack = ws.Cells[row, 32].Value != null ? Convert.ToInt32(ws.Cells[row, 32].Value) : null;
                                productInfo.PackPrice = ws.Cells[row, 33].Value != null ? Convert.ToDecimal(ws.Cells[row, 33].Value) : null;
                                productInfo.PackCost = ws.Cells[row, 34].Value != null ? Convert.ToDecimal(ws.Cells[row, 34].Value) : null;
                                productInfo.WarrantyPeriod = ws.Cells[row, 35].Value != null ? Convert.ToString(ws.Cells[row, 35].Value) : null;
                                productInfo.WarrantyPeriod = ws.Cells[row, 35].Value != null ? Convert.ToString(ws.Cells[row, 35].Value) : null;
                                productInfo.Currentstock = ws.Cells[row, 36].Value != null ? Convert.ToInt64(ws.Cells[row, 36].Value) : null;
                                productInfo.CratedDate = DateTime.Now;
                                var result = await _productService.CreateAsync(productInfo);
                                model.Add(result);
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

        [Route("/Warehouse/DownloadSampleForUploadItems")]
        [HttpGet]
        public ActionResult DownloadSampleForUploadItems()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string FilePath = webRootPath + "/Docs/Download/";

            FilePath = System.IO.Path.Combine(FilePath, "Upload_Items_sample_format.xlsx");
            var fs = new FileStream(FilePath, FileMode.Open);

            return File(fs, "application/octet-stream", "Upload_Items_sample_format.xlsx");
        }

    }
}
