using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.ProFormaInvoice;
using WinProApp.ViewModels.Purchase;
using WinProApp.ViewModels.PurchaseOrder;
using WinProApp.ViewModels.RequestForInfo;
using WinProApp.ViewModels.RequestForPurchase;
using WinProApp.ViewModels.RequestForQuotation;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Purchase")]
    public class PurchaseController : BasedUserController
    {
        public readonly PurchaseService _purchaseService;
        public readonly RequestForInfoService _requestForInfoService;
        public readonly RequestForQuotationService _requestForQuotationService;
        public readonly RequestForPurchaseService _requestForPurchaseService;
        public readonly PurchaseOrderService _purchaseOrderService;
        public readonly CommonService _commonService;
        public readonly ProFormaInvoiceService _proFormaInvoiceService;
        public readonly PurchaseRecieptService _purchaseRecieptService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PurchaseController(PurchaseService purchaseService,
           RequestForInfoService requestForInfoService,
           RequestForQuotationService requestForQuotationService,
           RequestForPurchaseService requestForPurchaseService,
           PurchaseOrderService purchaseOrderService,
           CommonService commonService,
           ProFormaInvoiceService proFormaInvoiceService,
           PurchaseRecieptService purchaseRecieptService,
           IWebHostEnvironment webHostEnvironment
           )
        {
            _purchaseService = purchaseService;
            _requestForInfoService = requestForInfoService;
            _requestForQuotationService = requestForQuotationService;
            _requestForPurchaseService = requestForPurchaseService;
            _purchaseOrderService = purchaseOrderService;
            _commonService = commonService;
            _proFormaInvoiceService = proFormaInvoiceService;
            _purchaseRecieptService = purchaseRecieptService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: PurchaseController
        [Route("/Purchase")]
        public ActionResult Index()
        {
            return View();
        }


        #region Supliers Management

        [Route("/Purchase/Suppliers")]
        [HttpGet]
        public IActionResult Suppliers()
        {
            return View();
        }

        [Route("/Purchase/SuppliersList")]
        public IActionResult SuppliersList(JQueryDataTableParamModel param)
        {

            var results = _purchaseService.GetSupplierList(param).Result.Select(x => new SupliersViewModel()
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                Address = x.Address,
                PhoneNo = x.PhoneNo,
                Fax = x.Fax,
                Email = x.Email,
                Website = x.Website,
                Bpcode = x.Bpcode,
                Balance = x.Balance,
                OpeningBalance = x.OpeningBalance,
                VatNo = x.VatNo,
                BankName = x.BankName,
                AccountNo = x.AccountNo,
                CRNumber = x.CRNumber,
                CRDocument = x.CRDocument,
                TaxDocument= x.TaxDocument,
                OtherDocument= x.OtherDocument,
                Status = x.Status,
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

        [AllowAnonymous, AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsCrNumberExists(string? CRNumber, long Id = 0)
        {
            if (!string.IsNullOrEmpty(CRNumber))
            {
                var response = await _purchaseService.IsCrNoExist(CRNumber, Id);
                if (!response) return Json(true);
                else return Json("CRNumber already exists");
            }
            else return Json(true);
        }

        [Route("/Purchase/IsVatNoExists/{VatNo}")]
        [HttpGet]
        public async Task<IActionResult> IsVatNoExists(string? VatNo)
        {
            try
            {
                if (!string.IsNullOrEmpty(VatNo))
                {
                    var response = await _purchaseService.IsVatNoExist(VatNo);
                    if (response) return Json(true);
                    else return Json(false);
                }
                else return Json(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        // GET: PurchaseController/CreateSupplier
        [Route("/Purchase/CreateSupplier")]
        [HttpGet]
        public IActionResult CreateSupplierAsync()
        {
            return View();
        }

        // POST: PurchaseController/Create
        [Route("/Purchase/CreateSupplier")]
        [HttpPost]
        public async Task<IActionResult> CreateSupplierAsync(AddSupliersViewModel model, IFormFile Upload, IFormFile TaxDocUpload, IFormFile OtherDocUpload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/Suppliers/";

                string docName = null;
                string taxDocName = null;
                string otherDocName = null;
                if (Upload != null && Upload.Length > 0)
                {
                    string curImgName = Path.GetFileName(Upload.FileName);
                    string curImgExtention = Path.GetExtension(Upload.FileName);

                    docName = "cr_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + curImgExtention;
                    string ImageSavePath = Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(ImageSavePath))
                    {
                        await Upload.CopyToAsync(stream);
                    }
                }

                if (TaxDocUpload != null && TaxDocUpload.Length > 0)
                {
                    string curTaxDocImgName = Path.GetFileName(TaxDocUpload.FileName);
                    string curTaxDocImgExtention = Path.GetExtension(TaxDocUpload.FileName);

                    taxDocName = "tax_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + curTaxDocImgExtention;
                    string taxDocSavePath = Path.Combine(uploadPath, taxDocName);

                    using (var stream = System.IO.File.Create(taxDocSavePath))
                    {
                        await TaxDocUpload.CopyToAsync(stream);
                    }
                }

                if (OtherDocUpload != null && OtherDocUpload.Length > 0)
                {
                    string curOtherDocImgName = Path.GetFileName(OtherDocUpload.FileName);
                    string curOtherDocImgExtention = Path.GetExtension(OtherDocUpload.FileName);

                    otherDocName = "other_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + curOtherDocImgExtention;
                    string OtherDocImageSavePath = Path.Combine(uploadPath, otherDocName);

                    using (var stream = System.IO.File.Create(OtherDocImageSavePath))
                    {
                        await OtherDocUpload.CopyToAsync(stream);
                    }
                }

                var supplier = new Supplier();
                supplier.CompanyName = model.CompanyName;
                supplier.Address = model.Address;
                supplier.PhoneNo = model.PhoneNo;
                supplier.Fax = model.Fax;
                supplier.Email = model.Email;
                supplier.Website = model.Website;
                supplier.Bpcode = model.Bpcode;
                supplier.Balance = model.Balance;
                supplier.OpeningBalance = model.OpeningBalance;
                supplier.VatNo = model.VatNo;
                supplier.BankName = model.BankName;
                supplier.AccountNo = model.AccountNo;
                supplier.CRNumber = model.CRNumber;
                supplier.CRDocument = docName;
                supplier.TaxDocument = taxDocName;
                supplier.OtherDocument= otherDocName;
                supplier.Status = true;
                supplier.CratedDate = DateTime.Now;
                supplier.CreatedBy = User.Identity.Name;
                supplier.UpdatedDate = DateTime.Now;
                supplier.UpdatedBy = User.Identity.Name;

                //return View();
                await _purchaseService.CreateAsync(supplier);
                return RedirectToAction(nameof(Suppliers));
            }
            catch
            {
                return View();
            }
        }


        // GET: PurchaseController/Create
        [Route("/Purchase/EditSupplier/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditSupplier(long id)
        {
            var info = await _purchaseService.GetSupplierByIdAsync(id);
            var model = new EditSupliersViewModel()
            {
                Id = id,
                CompanyName = info.CompanyName,
                Address = info.Address,
                PhoneNo = info.PhoneNo,
                Fax = info.Fax,
                Email = info.Email,
                Website = info.Website,
                Bpcode = info.Bpcode,
                Balance = info.Balance,
                OpeningBalance = info.OpeningBalance,
                VatNo = info.VatNo,
                BankName = info.BankName,
                AccountNo = info.AccountNo,
                CRNumber = info.CRNumber,
                CRDocument = info.CRDocument,
                TaxDocument=info.TaxDocument,
                OtherDocument= info.OtherDocument,
                Status = info.Status
            };
            return View(model);

        }

        [Route("/Purchase/EditSupplier/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditSupplier(long id, EditSupliersViewModel model, IFormFile Upload, IFormFile TaxDocUpload, IFormFile OtherDocUpload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Docs/Suppliers/";

                string docName = null;
                string taxDocName = null;
                string otherDocName = null;
                if (Upload != null && Upload.Length > 0)
                {
                    string curImgName = Path.GetFileName(Upload.FileName);
                    string curImgExtention = Path.GetExtension(Upload.FileName);

                    docName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curImgExtention;
                    string DocSavePath = Path.Combine(uploadPath, docName);

                    using (var stream = System.IO.File.Create(DocSavePath))
                    {
                        await Upload.CopyToAsync(stream);
                    }

                }

                if (TaxDocUpload != null && TaxDocUpload.Length > 0)
                {
                    string curTaxDocImgName = Path.GetFileName(TaxDocUpload.FileName);
                    string curTaxDocImgExtention = Path.GetExtension(TaxDocUpload.FileName);

                    taxDocName = "tax_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + curTaxDocImgExtention;
                    string taxDocSavePath = Path.Combine(uploadPath, taxDocName);

                    using (var stream = System.IO.File.Create(taxDocSavePath))
                    {
                        await TaxDocUpload.CopyToAsync(stream);
                    }
                }

                if (OtherDocUpload != null && OtherDocUpload.Length > 0)
                {
                    string curOtherDocImgName = Path.GetFileName(OtherDocUpload.FileName);
                    string curOtherDocImgExtention = Path.GetExtension(OtherDocUpload.FileName);

                    otherDocName = "other_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + curOtherDocImgExtention;
                    string OtherDocImageSavePath = Path.Combine(uploadPath, otherDocName);

                    using (var stream = System.IO.File.Create(OtherDocImageSavePath))
                    {
                        await OtherDocUpload.CopyToAsync(stream);
                    }
                }

                var supplier = _purchaseService.GetSupplierByIdAsync(id).Result;
                string curDoc = null;
                string curTaxDoc=null;
                string curOtherDoc=null;
                if (supplier.CRDocument != null)
                {
                    curDoc = Path.Combine(uploadPath, supplier.CRDocument);
                }
                if (supplier.TaxDocument != null)
                {
                    curTaxDoc = Path.Combine(uploadPath, supplier.TaxDocument);
                }
                if (supplier.OtherDocument != null)
                {
                    curOtherDoc = Path.Combine(uploadPath, supplier.OtherDocument);
                }

                supplier.Id = model.Id;
                supplier.CompanyName = model.CompanyName;
                supplier.Address = model.Address;
                supplier.PhoneNo = model.PhoneNo;
                supplier.Fax = model.Fax;
                supplier.Email = model.Email;
                supplier.Website = model.Website;
                supplier.Bpcode = model.Bpcode;
                supplier.Balance = model.Balance;
                supplier.OpeningBalance = model.OpeningBalance;
                supplier.VatNo = model.VatNo;
                supplier.BankName = model.BankName;
                supplier.AccountNo = model.AccountNo;
                supplier.CRNumber = model.CRNumber;
                if (!string.IsNullOrEmpty(docName))
                {
                    if (System.IO.File.Exists(curDoc))
                    {
                        System.IO.File.Delete(curDoc);
                    }
                    supplier.CRDocument = docName;
                }
                if (!string.IsNullOrEmpty(taxDocName))
                {
                    if (System.IO.File.Exists(curTaxDoc))
                    {
                        System.IO.File.Delete(curTaxDoc);
                    }
                    supplier.TaxDocument = taxDocName;
                }
                if (!string.IsNullOrEmpty(otherDocName))
                {
                    if (System.IO.File.Exists(curOtherDoc))
                    {
                        System.IO.File.Delete(curOtherDoc);
                    }
                    supplier.OtherDocument = otherDocName;
                }

                supplier.Status = true;
                supplier.UpdatedDate = DateTime.Now;
                supplier.UpdatedBy = User.Identity.Name;

                await _purchaseService.UpdateAsync(supplier);

                // return RedirectToAction(nameof(Suppliers));
                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeleteSupplier/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                long invoiceCount = await _purchaseService.GetInvoiceCountBySupplierIdAsync(id);
                long shipmentCount = await _purchaseService.GetShipmentCountBySupplierIdAsync(id);

                if (invoiceCount == 0 && shipmentCount == 0)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string docPath = webRootPath + "/Docs/Suppliers/";

                    var supplier = await _purchaseService.GetSupplierByIdAsync(id);
                    string? crdoc = supplier.CRDocument;
                    string? taxdoc = supplier.TaxDocument;
                    string? otherdoc = supplier.OtherDocument;

                    await _purchaseService.DeleteAsync(supplier);

                    if (!string.IsNullOrEmpty(crdoc))
                    {
                        string curdoc1 = Path.Combine(docPath, crdoc);
                        if (System.IO.File.Exists(curdoc1))
                        {
                            System.IO.File.Delete(curdoc1);
                        }
                    }

                    if (!string.IsNullOrEmpty(taxdoc))
                    {
                        string curdoc2 = Path.Combine(docPath, taxdoc);
                        if (System.IO.File.Exists(curdoc2))
                        {
                            System.IO.File.Delete(curdoc2);
                        }
                    }

                    if (!string.IsNullOrEmpty(otherdoc))
                    {
                        string curdoc2 = Path.Combine(docPath, otherdoc);
                        if (System.IO.File.Exists(curdoc2))
                        {
                            System.IO.File.Delete(curdoc2);
                        }
                    }

                    return new JsonResult(new { id=id });
                }
                else
                {
                    id = 0;
                    return new JsonResult(new { id=id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

        #region Requiest for informations

        [Route("/Purchase/RequestForInfo")]
        [HttpGet]
        public IActionResult RequestForInfo()
        {
            return View();
        }

        [Route("/Purchase/RequestForInfoList")]
        public IActionResult RequestForInfoList(JQueryDataTableParamModel param)
        {

            var results = _requestForInfoService.GetRequestForInformationList(param).Result.Select(x => new RequestForInfoViewModel()
            {
                Id = x.Id,
                ReqDate = x.ReqDate.ToString("yyyy-MM-dd"),
                Requester = x.Requester,
                Department = x.Department,
                Note = x.Note,
                Approved = x.Approved == true ? "Yes" : "No",
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


        [Route("/Purchase/CreateRequestForInfo")]
        [HttpGet]
        public IActionResult CreateRequestForInfo()
        {
            return View();
        }

        [Route("/Purchase/CreateRequestForInfo")]
        [HttpPost]
        public async Task<IActionResult> CreateRequestForInfo(AddRequestForInfoViewModel model)
        {
            try
            {
                var requiestInfo = new RequestForInformation();
                requiestInfo.ReqDate = model.Date;
                requiestInfo.Requester = model.Requester;
                requiestInfo.Department = model.Department;
                requiestInfo.Note = model.Note;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    requiestInfo.Approved = model.Approved;
                    if (model.Approved == true)
                    {
                        requiestInfo.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    requiestInfo.Approved = false;
                }
                requiestInfo.CratedDate = DateTime.Now;
                requiestInfo.CreatedBy = User.Identity.Name;
                requiestInfo.UpdatedDate = DateTime.Now;
                requiestInfo.UpdatedBy = User.Identity.Name;

                var result = await _requestForInfoService.CreateAsync(requiestInfo);

                List<RequestForInformationItem> items = new List<RequestForInformationItem>();

                string[] descriptions = Request.Form["Description"];
                string[] reasons = Request.Form["Reason"];
                string[] qtys = Request.Form["Qty"];
                string[] mf = Request.Form["MFCompany"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    decimal? qty = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }

                    items.Add(new RequestForInformationItem() { Description = descriptions[i], Reason = reasons[i], Qty = qty, Mfcompany = mf[i] });
                }

                // requiestInfo.RequestItems = items;
                await _requestForInfoService.CreateItemsAsync(result.Id, items);



                return RedirectToAction(nameof(RequestForInfo));
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
                //return View();
            }
        }


        [Route("/Purchase/RequestForInfoDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> RequestForInfoDetails(long id)
        {
            var info = await _requestForInfoService.GetRequestForInfoByIdAsync(id);
            var model = new DetailsRequestForInfoViewModel()
            {
                Id = id,
                ReqDate = info.ReqDate.ToString("yyyy-MM-dd"),
                Requester = info.Requester,
                Department = info.Department,
                Note = info.Note,
                Approved = info.Approved == true ? "Yes" : "No",
                CreatedBy = info.CreatedBy,
                CratedDate = info.CratedDate.ToString("yyyy-MM-dd"),
                UpdatedBy = info.UpdatedBy,
                UpdatedDate = info.UpdatedDate.ToString("yyyy-MM-dd"),
                Items = await _requestForInfoService.GetRequestForInfoItemsByInfoIdAsync(id)
            };
            return View(model);

        }

        [Route("/Purchase/EditRequestForInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditRequestForInfo(long id)
        {
            var info = await _requestForInfoService.GetRequestForInfoByIdAsync(id);
            var model = new EditRequestForInfoViewModel()
            {
                Id = id,
                Date = info.ReqDate,
                Requester = info.Requester,
                Department = info.Department,
                Note = info.Note,
                Approved = info.Approved,
                Items = await _requestForInfoService.GetRequestForInfoItemsByInfoIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditRequestForInfo/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditRequestForInfo(long id, EditRequestForInfoViewModel model)
        {
            //try
            //{
                var requiestInfo = await _requestForInfoService.GetRequestForInfoByIdAsync(id);
                bool oldApproved = requiestInfo.Approved;
                requiestInfo.Id = id;
                requiestInfo.ReqDate = model.Date;
                requiestInfo.Requester = model.Requester;
                requiestInfo.Department = model.Department;
                requiestInfo.Note = model.Note;

                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {

                    requiestInfo.Approved = model.Approved;
                    if (model.Approved == true && oldApproved == false)
                    {
                        requiestInfo.ApprovedDate = DateTime.Now;
                        requiestInfo.ApprovedBy = User.Identity.Name;
                    }
                }
                else
                {
                    requiestInfo.Approved = false;
                }

                requiestInfo.UpdatedDate = DateTime.Now;
                requiestInfo.UpdatedBy = User.Identity.Name;

                // var result = await _requestForInfoService.UpdateAsync(requiestInfo);

                List<RequestForInformationItem> items = new List<RequestForInformationItem>();

                string[] ids = Request.Form["ItemId"];
                string[] descriptions = Request.Form["Description"];
                string[] reasons = Request.Form["Reason"];
                string[] qtys = Request.Form["Qty"];
                string[] mf = Request.Form["MFCompany"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    decimal? qty = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }

                    items.Add(new RequestForInformationItem() { Id = curItemId, Rfiid = id, Description = descriptions[i], Reason = reasons[i], Qty = qty, Mfcompany = mf[i] });
                }

                // requiestInfo.RequestItems = items;
                await _requestForInfoService.UpdateAsync(requiestInfo, items);

                return new JsonResult(new { id });
            //}
            //catch (Exception ex)
            //{
            //    throw ex.InnerException;
            //    // return View();
            //}
        }



        [Route("/Purchase/DeleteRequestForInfoItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequestForInfoItem(long id)
        {
            try
            {
                await _requestForInfoService.DeleteItemAsync(id);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeleteRequestForInfo/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequestForInfo(long id)
        {
            try
            {
                var result = await _requestForInfoService.GetRequestForInfoByIdAsync(id);
                await _requestForInfoService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

        #region RequestForQuotation

        [Route("/Purchase/RequestForQuotation")]
        [HttpGet]
        public IActionResult RequestForQuotation()
        {
            return View();
        }

        [Route("/Purchase/RequestForQuotationList")]
        public IActionResult RequestForQuotationList(JQueryDataTableParamModel param)
        {

            var results = _requestForQuotationService.GetRequestForQuotationList(param).Result.Select(x => new RequestForQuotationViewModel()
            {
                Id = x.Id,
                Rfiid = x.Rfiid,
                Date = x.Date.ToString("yyyy-MM-dd"),
                RequireDate = x.RequireDate != null ? x.RequireDate.Value.ToString("yyyy-MM-dd") : "",
                Requester = x.Requester,
                Department = x.Department,
                Note = x.Note,
                Approved = x.Approved == true ? "Yes" : "No",
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


        [Route("/Purchase/CreateRequestForQuotation/{id}")]
        [HttpGet]
        public async Task<IActionResult> CreateRequestForQuotation(long id)
        {
            var info = await _requestForInfoService.GetRequestForInfoByIdAsync(id);
            var model = new AddRequestForQuotationViewModel();
            model.Rfiid = id;
            model.Requester = info.Requester;
            model.Department = info.Department;
            model.Date = info.ReqDate;

            ViewBag.Items = await _requestForInfoService.GetRequestForInfoItemsByInfoIdAsync(id);

            return View(model);
        }

        [Route("/Purchase/CreateRequestForQuotation/{id}")]
        [HttpPost]
        public async Task<IActionResult> CreateRequestForQuotation(long id, AddRequestForQuotationViewModel model)
        {
            try
            {
                var requiestInfo = new RequestForQuotation();
                requiestInfo.Rfiid = id;
                requiestInfo.Date = model.Date;
                requiestInfo.RequireDate = model.RequireDate;
                requiestInfo.Requester = model.Requester;
                requiestInfo.Department = model.Department;
                requiestInfo.Note = model.Note;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    requiestInfo.Approved = model.Approved;
                    if (model.Approved == true)
                    {
                        requiestInfo.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    requiestInfo.Approved = false;
                }
                requiestInfo.CratedDate = DateTime.Now;
                requiestInfo.CreatedBy = User.Identity.Name;
                requiestInfo.UpdatedDate = DateTime.Now;
                requiestInfo.UpdatedBy = User.Identity.Name;

                var result = await _requestForQuotationService.CreateAsync(requiestInfo);

                List<RequestForQuotationItem> items = new List<RequestForQuotationItem>();


                string[] descriptions = Request.Form["Description"];
                string[] reasons = Request.Form["Reason"];
                string[] qtys = Request.Form["Qty"];
                string[] mf = Request.Form["MFCompany"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    decimal? qty = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }

                    items.Add(new RequestForQuotationItem() { Rfqid = id, Description = descriptions[i], Reason = reasons[i], Qty = qty, Mfcompany = mf[i] });
                }

                // requiestInfo.RequestItems = items;
                await _requestForQuotationService.CreateItemsAsync(result.Id, items);



                return RedirectToAction(nameof(RequestForQuotation));
            }
            catch
            {
                return View();
            }
        }

        [Route("/Purchase/RequestForQuotationDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> RequestForQuotationDetails(long id)
        {
            var info = await _requestForQuotationService.GetRequestForQuotationByIdAsync(id);
            var model = new DetailsRequestForQuotationViewModel()
            {
                Id = id,
                Rfiid = info.Rfiid,
                Date = info.Date.ToString("yyyy-MM-dd"),
                RequireDate = info.RequireDate != null ? info.RequireDate.Value.ToString("yyyy-MM-dd") : "",
                Requester = info.Requester,
                Department = info.Department,
                Note = info.Note,
                Approved = info.Approved == true ? "Yes" : "No",
                CreatedBy = info.CreatedBy,
                CratedDate = info.CratedDate.ToString("yyyy-MM-dd"),
                UpdatedBy = info.UpdatedBy,
                UpdatedDate = info.UpdatedDate.ToString("yyyy-MM-dd"),
                Items = await _requestForQuotationService.GetRequestForQuotationItemsByInfoIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditRequestForQuotation/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditRequestForQuotation(long id)
        {
            var info = await _requestForQuotationService.GetRequestForQuotationByIdAsync(id);
            var model = new EditRequestForQuotationViewModel()
            {
                Id = id,
                Rfiid = info.Rfiid,
                Date = info.Date,
                RequireDate = info.RequireDate,
                Requester = info.Requester,
                Department = info.Department,
                Note = info.Note,
                Approved = info.Approved,
                Items = await _requestForQuotationService.GetRequestForQuotationItemsByInfoIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditRequestForQuotation/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditRequestForQuotation(long id, EditRequestForQuotationViewModel model)
        {
            try
            {
                var requiestInfo = await _requestForQuotationService.GetRequestForQuotationByIdAsync(id);
                bool oldApproved = requiestInfo.Approved;
                requiestInfo.Date = model.Date;
                requiestInfo.RequireDate = model.RequireDate;
                requiestInfo.Requester = model.Requester;
                requiestInfo.Department = model.Department;
                requiestInfo.Note = model.Note;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    requiestInfo.Approved = model.Approved;
                    if (model.Approved == true && oldApproved == false)
                    {
                        requiestInfo.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    requiestInfo.Approved = false;
                }

                requiestInfo.UpdatedDate = DateTime.Now;
                requiestInfo.UpdatedBy = User.Identity.Name;

                // var result = await _requestForInfoService.UpdateAsync(requiestInfo);

                List<RequestForQuotationItem> items = new List<RequestForQuotationItem>();

                string[] ids = Request.Form["ItemId"];
                string[] descriptions = Request.Form["Description"];
                string[] reasons = Request.Form["Reason"];
                string[] qtys = Request.Form["Qty"];
                string[] mf = Request.Form["MFCompany"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    decimal? qty = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }

                    items.Add(new RequestForQuotationItem() { Id = curItemId, Rfqid = id, Description = descriptions[i], Reason = reasons[i], Qty = qty, Mfcompany = mf[i] });
                }

                // requiestInfo.RequestItems = items;
                await _requestForQuotationService.UpdateAsync(requiestInfo, items);

                return new JsonResult(new { id });
            }
            catch
            {
                return View();
            }
        }



        [Route("/Purchase/DeleteRequestForQuotationItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRRequestForQuotationItem(long id)
        {
            try
            {
                await _requestForQuotationService.DeleteItemAsync(id);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeleteRequestForQuotation/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequestForQuotation(long id)
        {
            try
            {
                var result = await _requestForQuotationService.GetRequestForQuotationByIdAsync(id);
                await _requestForQuotationService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

        #region RequestForPurchase

        [Route("/Purchase/RequestForPurchase")]
        [HttpGet]
        public IActionResult RequestForPurchase()
        {
            return View();
        }

        [Route("/Purchase/RequestForPurchaseList")]
        public IActionResult RequestForPurchaseList(JQueryDataTableParamModel param)
        {

            var results = _requestForPurchaseService.GetList(param).Result.Select(x => new RequestForPurchaseViewModel()
            {
                Id = x.Id,
                Rfqid = x.Rfqid,
                Date = x.Date.ToString("yyyy-MM-dd"),
                RequireDate = x.RequireDate != null ? x.RequireDate.Value.ToString("yyyy-MM-dd") : "",
                Requester = x.Requester,
                Department = x.Department,
                Note = x.Note,
                Approved = x.Approved == true ? "Yes" : "No",
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


        [Route("/Purchase/CreateRequestForPurchase/{id}")]
        [HttpGet]
        public async Task<IActionResult> CreateRequestForPurchase(long id)
        {
            var info = await _requestForPurchaseService.GetByIdAsync(id);
            var model = new AddRequestForPurchaseViewModelcs();
            model.Rfqid = id;
            model.Requester = info.Requester;
            model.Department = info.Department;
            model.Date = info.Date;

            ViewBag.Items = await _requestForPurchaseService.GetByIdAsync(id);

            return View(model);
        }

        [Route("/Purchase/CreateRequestForPurchase/{id}")]
        [HttpPost]
        public async Task<IActionResult> CreateRequestForPurchase(long id, AddRequestForPurchaseViewModelcs model)
        {
            try
            {
                var requiestInfo = new RequestForPurchases();
                requiestInfo.Rfqid = id;
                requiestInfo.Date = model.Date;
                requiestInfo.RequireDate = model.RequireDate;
                requiestInfo.Requester = model.Requester;
                requiestInfo.Department = model.Department;
                requiestInfo.Note = model.Note;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    requiestInfo.Approved = model.Approved;
                    if (model.Approved == true)
                    {
                        requiestInfo.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    requiestInfo.Approved = false;
                }
                requiestInfo.CratedDate = DateTime.Now;
                requiestInfo.CreatedBy = User.Identity.Name;
                requiestInfo.UpdatedDate = DateTime.Now;
                requiestInfo.UpdatedBy = User.Identity.Name;

                var result = await _requestForPurchaseService.CreateAsync(requiestInfo);

                List<RequestForPurchaseItem> items = new List<RequestForPurchaseItem>();


                string[] ids = Request.Form["ItemId"];
                string[] partNos = Request.Form["PartNo"];
                string[] descriptions = Request.Form["Description"];
                string[] reasons = Request.Form["Reason"];
                string[] qtys = Request.Form["Qty"];
                string[] prices = Request.Form["Price"];
                string[] mf = Request.Form["MFCompany"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    decimal? qty = null;
                    decimal? price = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    items.Add(new RequestForPurchaseItem() { Prid = id, PartNo = partNos[i], Description = descriptions[i], Reason = reasons[i], Qty = qty, Price = price, Mfcompany = mf[i] });
                }

                // requiestInfo.RequestItems = items;
                await _requestForPurchaseService.CreateItemsAsync(result.Id, items);



                return RedirectToAction(nameof(RequestForQuotation));
            }
            catch
            {
                return View();
            }
        }

        [Route("/Purchase/RequestForPurchaseDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> RequestForPurchaseDetails(long id)
        {
            var info = await _requestForPurchaseService.GetByIdAsync(id);
            var model = new DetailsRequestForPurchaseViewModel()
            {
                Id = id,
                Rfqid = info.Rfqid,
                Date = info.Date.ToString("yyyy-MM-dd"),
                RequireDate = info.RequireDate != null ? info.RequireDate.Value.ToString("yyyy-MM-dd") : "",
                Requester = info.Requester,
                Department = info.Department,
                Note = info.Note,
                Approved = info.Approved == true ? "Yes" : "No",
                CreatedBy = info.CreatedBy,
                CratedDate = info.CratedDate.ToString("yyyy-MM-dd"),
                UpdatedBy = info.UpdatedBy,
                UpdatedDate = info.UpdatedDate.ToString("yyyy-MM-dd"),
                Items = await _requestForPurchaseService.GetItemsByRequestIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditRequestForPurchase/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditRequestForPurchase(long id)
        {
            var info = await _requestForPurchaseService.GetByIdAsync(id);
            var model = new EditRequestForPurchaseViewModel()
            {
                Id = id,
                Rfqid = info.Rfqid,
                Date = info.Date,
                RequireDate = info.RequireDate,
                Requester = info.Requester,
                Department = info.Department,
                Note = info.Note,
                Approved = info.Approved,
                Items = await _requestForPurchaseService.GetItemsByRequestIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditRequestForPurchase/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditRequestForPurchase(long id, EditRequestForPurchaseViewModel model)
        {
            try
            {
                var requiestInfo = await _requestForPurchaseService.GetByIdAsync(id);
                bool oldApproved = requiestInfo.Approved;
                requiestInfo.Date = model.Date;
                requiestInfo.RequireDate = model.RequireDate;
                requiestInfo.Requester = model.Requester;
                requiestInfo.Department = model.Department;
                requiestInfo.Note = model.Note;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    requiestInfo.Approved = model.Approved;
                    if (model.Approved == true && oldApproved == false)
                    {
                        requiestInfo.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    requiestInfo.Approved = false;
                }

                requiestInfo.UpdatedDate = DateTime.Now;
                requiestInfo.UpdatedBy = User.Identity.Name;

                // var result = await _requestForInfoService.UpdateAsync(requiestInfo);

                List<RequestForPurchaseItem> items = new List<RequestForPurchaseItem>();

                string[] ids = Request.Form["ItemId"];
                string[] partNos = Request.Form["PartNo"];
                string[] descriptions = Request.Form["Description"];
                string[] reasons = Request.Form["Reason"];
                string[] qtys = Request.Form["Qty"];
                string[] prices = Request.Form["Price"];
                string[] mf = Request.Form["MFCompany"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    decimal? qty = null;
                    decimal? price = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }

                    items.Add(new RequestForPurchaseItem() { Id = curItemId, Prid = id, PartNo = partNos[i], Description = descriptions[i], Reason = reasons[i], Qty = qty, Price = price, Mfcompany = mf[i] });
                }

                // requiestInfo.RequestItems = items;
                await _requestForPurchaseService.UpdateAsync(requiestInfo, items);

                return new JsonResult(new { id });
            }
            catch
            {
                return View();
            }
        }



        [Route("/Purchase/DeleteRequestForPurchaseItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequestForPurchaseItem(long id)
        {
            try
            {
                await _requestForPurchaseService.DeleteItemAsync(id);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeleteRequestForPurchase/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRequestForPurchase(long id)
        {
            try
            {
                var result = await _requestForPurchaseService.GetByIdAsync(id);
                await _requestForPurchaseService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

        #region Purchase Orders
        [Route("/Purchase/PurchaseOrders")]
        [HttpGet]
        public IActionResult PurchaseOrders()
        {
            return View();
        }

        [Route("/Purchase/PurchaseOrdersList")]
        public IActionResult PurchaseOrdersList(JQueryDataTableParamModel param)
        {

            var results = _purchaseOrderService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                Prid = x.Prid,
                Date = x.Date.ToString("yyyy-MM-dd"),
                ShippingAddress = x.ShippingAddress,
                ShippingMethod = x.ShippingMethod,
                ShippingAmount = x.ShippingAmount,
                DeliveryDate = x.DeliveryDate != null ? x.DeliveryDate.Value.ToString("yyyy-MM-dd") : "",
                Remarks = x.Remarks,
                Approved = x.Approved == true ? "Yes" : "No",
                ApprovedDate = x.ApprovedDate != null ? x.ApprovedDate.Value.ToString("yyyy-MM-dd") : "",
                ApprovedBy = x.ApprovedBy,
                totQty = _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Qty) > 0 ? _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Qty).Value.ToString("0.00") : "0.00",
                totPrice = _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Price) > 0 ? _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Price).Value.ToString("0.00") : "0.00",
                totTax = _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax) > 0 ? _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax).Value.ToString("0.00") : "0.00",
                totToatal = _purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax) + x.ShippingAmount > 0 ? (_purchaseOrderService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax) + x.ShippingAmount).Value.ToString("0.00") : "0.00",
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


        [Route("/Purchase/CreatePurchaseOrder")]
        [HttpGet]
        public async Task<IActionResult> CreatePurchaseOrder()
        {
            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            ViewBag.Units = new SelectList(units, RequestCulture.Name == "ar" ? "NameArabic" : "NameEng", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View();
        }

        [Route("/Purchase/CreatePurchaseOrder")]
        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder(AddViewModel model)
        {
            try
            {
                var order = new PurchaseOrders();
                order.Date = model.Date;
                order.ShippingAddress = model.ShippingAddress;
                order.ShippingMethod = model.ShippingMethod;
                order.ShippingAmount = model.ShippingAmount;
                order.DeliveryDate = model.DeliveryDate;
                order.DeliveryDate = model.DeliveryDate;
                order.Remarks = model.Remarks;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    order.Approved = model.Approved;
                    if (model.Approved == true)
                    {
                        order.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    order.Approved = false;
                }
                order.CratedDate = DateTime.Now;
                order.CreatedBy = User.Identity.Name;
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = User.Identity.Name;

                var result = await _purchaseOrderService.CreateAsync(order);

                List<PurchaseOrderItems> items = new List<PurchaseOrderItems>();



                string[] partNos = Request.Form["PartNo"];
                string[] descriptions = Request.Form["Description"];
                string[] units = Request.Form["Unit"];
                string[] qtys = Request.Form["Qty"];
                string[] prices = Request.Form["Price"];
                string[] taxes = Request.Form["Tax"];
                string[] totals = Request.Form["Total"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? tax = null;
                    decimal? total = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                    if (!string.IsNullOrEmpty(taxes[i]))
                    {
                        tax = decimal.Parse(taxes[i]);
                    }
                    if (!string.IsNullOrEmpty(totals[i]))
                    {
                        total = decimal.Parse(totals[i]);
                    }

                    items.Add(new PurchaseOrderItems() { Poid = result.Id, PartNo = partNos[i], Description = descriptions[i], Unit = units[i], Qty = qty, Price = price, Tax = tax, Total = total });
                }

                // requiestInfo.RequestItems = items;
                await _purchaseOrderService.CreateItemsAsync(result.Id, items);



                return RedirectToAction(nameof(RequestForQuotation));
            }
            catch
            {
                return View();
            }
        }

        [Route("/Purchase/PurchaseOrderDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> PurchaseOrderDetails(long id)
        {
            var info = await _purchaseOrderService.GetByIdAsync(id);
            var model = new DetailsViewModel()
            {
                Id = id,
                Prid = info.Prid,
                Date = info.Date.ToString("yyyy-MM-dd"),
                ShippingAddress = info.ShippingAddress,
                ShippingMethod = info.ShippingMethod,
                ShippingAmount = info.ShippingAmount,
                DeliveryDate = info.DeliveryDate != null ? info.DeliveryDate.Value.ToString("yyyy-MM-dd") : "",
                Remarks = info.Remarks,
                Approved = info.Approved == true ? "Yes" : "No",
                CreatedBy = info.CreatedBy,
                CratedDate = info.CratedDate.ToString("yyyy-MM-dd"),
                UpdatedBy = info.UpdatedBy,
                UpdatedDate = info.UpdatedDate.ToString("yyyy-MM-dd"),
                Items = await _purchaseOrderService.GetItemsByRequestIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditPurchaseOrder/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditPurchaseOrder(long id)
        {
            var info = await _purchaseOrderService.GetByIdAsync(id);
            var model = new EditViewModel()
            {
                Id = id,
                Prid = info.Prid,
                Date = info.Date,
                ShippingAddress = info.ShippingAddress,
                ShippingMethod = info.ShippingMethod,
                ShippingAmount = info.ShippingAmount,
                DeliveryDate = info.DeliveryDate,
                Remarks = info.Remarks,
                Approved = info.Approved,
                Items = await _purchaseOrderService.GetItemsByRequestIdAsync(id)
            };

            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            ViewBag.Units = new SelectList(units, RequestCulture.Name == "ar" ? "NameArabic" : "NameEng", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View(model);

        }


        [Route("/Purchase/EditPurchaseOrder/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditPurchaseOrder(long id, EditViewModel model)
        {
            try
            {
                var order = await _purchaseOrderService.GetByIdAsync(id);
                bool oldApproved = order.Approved;
                order.Date = model.Date;
                order.ShippingAddress = model.ShippingAddress;
                order.ShippingMethod = model.ShippingMethod;
                order.ShippingAmount = model.ShippingAmount;
                order.DeliveryDate = model.DeliveryDate;
                order.DeliveryDate = model.DeliveryDate;
                order.Remarks = model.Remarks;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    order.Approved = model.Approved;
                    if (model.Approved == true && oldApproved == false)
                    {
                        order.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    order.Approved = oldApproved;
                }

                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = User.Identity.Name;

                // var result = await _requestForInfoService.UpdateAsync(requiestInfo);

                List<PurchaseOrderItems> items = new List<PurchaseOrderItems>();

                string[] ids = Request.Form["ItemId"];
                string[] partNos = Request.Form["PartNo"];
                string[] descriptions = Request.Form["Description"];
                string[] units = Request.Form["Unit"];
                string[] qtys = Request.Form["Qty"];
                string[] prices = Request.Form["Price"];
                string[] taxes = Request.Form["Tax"];
                string[] totals = Request.Form["Total"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? tax = null;
                    decimal? total = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                    if (!string.IsNullOrEmpty(taxes[i]))
                    {
                        tax = decimal.Parse(taxes[i]);
                    }
                    if (!string.IsNullOrEmpty(totals[i]))
                    {
                        total = decimal.Parse(totals[i]);
                    }

                    items.Add(new PurchaseOrderItems() { Id = curItemId, Poid = id, PartNo = partNos[i], Description = descriptions[i], Unit = units[i], Qty = qty, Price = price, Tax = tax, Total = total });
                }

                await _purchaseOrderService.UpdateAsync(order, items);

                return new JsonResult(new { id });
            }
            catch
            {
                return View();
            }
        }



        [Route("/Purchase/DeletePurchaseOrderItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeletePurchaseOrderItem(long id)
        {
            try
            {
                await _purchaseOrderService.DeleteItemAsync(id);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeletePurchaseOrder/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeletePurchaseOrder(long id)
        {
            try
            {
                var result = await _purchaseOrderService.GetByIdAsync(id);
                await _purchaseOrderService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

        #region ProFormaInvoice

        [Route("/Purchase/ProFormaInvoices")]
        [HttpGet]
        public IActionResult ProFormaInvoices()
        {
            return View();
        }

        [Route("/Purchase/ProFormaInvoicesList")]
        public IActionResult ProFormaInvoicesList(JQueryDataTableParamModel param)
        {

            var results = _proFormaInvoiceService.GetList(param).Result.Select(x => new ListProFormaViewModel()
            {
                Id = x.Id,
                Date = x.Date.ToString("yyyy-MM-dd"),
                ShippingAddress = x.ShippingAddress,
                ShippingMethod = x.ShippingMethod,
                ShippingAmount = x.ShippingAmount,
                DeliveryDate = x.DeliveryDate != null ? x.DeliveryDate.Value.ToString("yyyy-MM-dd") : "",
                Remarks = x.Remarks,
                Approved = x.Approved == true ? "Yes" : "No",
                ApprovedDate = x.ApprovedDate != null ? x.ApprovedDate.Value.ToString("yyyy-MM-dd") : "",
                ApprovedBy = x.ApprovedBy,
                totQty = _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Qty) > 0 ? _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Qty).Value.ToString("0.00") : "0.00",
                totPrice = _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Price) > 0 ? _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Price).Value.ToString("0.00") : "0.00",
                totTax = _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax) > 0 ? _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax).Value.ToString("0.00") : "0.00",
                totToatal = _proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax) + x.ShippingAmount > 0 ? (_proFormaInvoiceService.GetItemsByRequestIdAsync(x.Id).Result.Sum(t => t.Tax) + x.ShippingAmount).Value.ToString("0.00") : "0.00",
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


        [Route("/Purchase/CreateProFormaInvoice")]
        [HttpGet]
        public async Task<IActionResult> CreateProFormaInvoice()
        {
            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            ViewBag.Units = new SelectList(units, RequestCulture.Name == "ar" ? "NameArabic" : "NameEng", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View();
        }

        [Route("/Purchase/CreateProFormaInvoice")]
        [HttpPost]
        public async Task<IActionResult> CreateProFormaInvoice(AddProFormaViewModel model)
        {
            try
            {
                var order = new ProFormaInvoice();
                order.Date = model.Date;
                order.ShippingAddress = model.ShippingAddress;
                order.ShippingMethod = model.ShippingMethod;
                order.ShippingAmount = model.ShippingAmount;
                order.DeliveryDate = model.DeliveryDate;
                order.DeliveryDate = model.DeliveryDate;
                order.Remarks = model.Remarks;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    order.Approved = model.Approved;
                    if (model.Approved == true)
                    {
                        order.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    order.Approved = false;
                }
                order.CratedDate = DateTime.Now;
                order.CreatedBy = User.Identity.Name;
                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = User.Identity.Name;

                var result = await _proFormaInvoiceService.CreateAsync(order);

                List<ProFormaInvoiceItems> items = new List<ProFormaInvoiceItems>();



                string[] partNos = Request.Form["PartNo"];
                string[] descriptions = Request.Form["Description"];
                string[] units = Request.Form["Unit"];
                string[] qtys = Request.Form["Qty"];
                string[] prices = Request.Form["Price"];
                string[] taxes = Request.Form["Tax"];
                string[] totals = Request.Form["Total"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? tax = null;
                    decimal? total = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                    if (!string.IsNullOrEmpty(taxes[i]))
                    {
                        tax = decimal.Parse(taxes[i]);
                    }
                    if (!string.IsNullOrEmpty(totals[i]))
                    {
                        total = decimal.Parse(totals[i]);
                    }

                    items.Add(new ProFormaInvoiceItems() { Pfid = result.Id, PartNo = partNos[i], Description = descriptions[i], Unit = units[i], Qty = qty, Price = price, Tax = tax, Total = total });
                }

                // requiestInfo.RequestItems = items;
                await _proFormaInvoiceService.CreateItemsAsync(result.Id, items);



                return RedirectToAction(nameof(RequestForQuotation));
            }
            catch
            {
                return View();
            }
        }

        [Route("/Purchase/ProFormaInvoiceDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> ProFormaInvoiceDetails(long id)
        {
            var info = await _proFormaInvoiceService.GetByIdAsync(id);
            var model = new DetailsProFormaViewModel()
            {
                Id = id,
                Date = info.Date.ToString("yyyy-MM-dd"),
                ShippingAddress = info.ShippingAddress,
                ShippingMethod = info.ShippingMethod,
                ShippingAmount = info.ShippingAmount,
                DeliveryDate = info.DeliveryDate != null ? info.DeliveryDate.Value.ToString("yyyy-MM-dd") : "",
                Remarks = info.Remarks,
                Approved = info.Approved == true ? "Yes" : "No",
                CreatedBy = info.CreatedBy,
                CratedDate = info.CratedDate.ToString("yyyy-MM-dd"),
                UpdatedBy = info.UpdatedBy,
                UpdatedDate = info.UpdatedDate.ToString("yyyy-MM-dd"),
                Items = await _proFormaInvoiceService.GetItemsByRequestIdAsync(id)
            };
            return View(model);

        }


        [Route("/Purchase/EditProFormaInvoice/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditProFormaInvoice(long id)
        {
            var info = await _proFormaInvoiceService.GetByIdAsync(id);
            var model = new EditProFormaViewModel()
            {
                Id = id,
                Date = info.Date,
                ShippingAddress = info.ShippingAddress,
                ShippingMethod = info.ShippingMethod,
                ShippingAmount = info.ShippingAmount,
                DeliveryDate = info.DeliveryDate,
                Remarks = info.Remarks,
                Approved = info.Approved,
                Items = await _proFormaInvoiceService.GetItemsByRequestIdAsync(id)
            };

            var units = await _commonService.GetUnitsAsync();
            var vat = await _commonService.GetVatAsync();
            ViewBag.Units = new SelectList(units, RequestCulture.Name == "ar" ? "NameArabic" : "NameEng", RequestCulture.Name == "ar" ? "NameArabic" : "NameEng");
            ViewBag.VatPercentage = vat.Percentage ?? 0;

            return View(model);

        }


        [Route("/Purchase/EditProFormaInvoice/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditProFormaInvoice(long id, EditProFormaViewModel model)
        {
            try
            {
                var order = await _proFormaInvoiceService.GetByIdAsync(id);
                bool oldApproved = order.Approved;
                order.Date = model.Date;
                order.ShippingAddress = model.ShippingAddress;
                order.ShippingMethod = model.ShippingMethod;
                order.ShippingAmount = model.ShippingAmount;
                order.DeliveryDate = model.DeliveryDate;
                order.DeliveryDate = model.DeliveryDate;
                order.Remarks = model.Remarks;
                if (User.IsInRole("Administrator") || User.IsInRole("Purchase") || User.IsInRole("Manager"))
                {
                    order.Approved = model.Approved;
                    if (model.Approved == true && oldApproved == false)
                    {
                        order.ApprovedDate = DateTime.Now;
                    }
                }
                else
                {
                    order.Approved = oldApproved;
                }

                order.UpdatedDate = DateTime.Now;
                order.UpdatedBy = User.Identity.Name;


                List<ProFormaInvoiceItems> items = new List<ProFormaInvoiceItems>();

                string[] ids = Request.Form["ItemId"];
                string[] partNos = Request.Form["PartNo"];
                string[] descriptions = Request.Form["Description"];
                string[] units = Request.Form["Unit"];
                string[] qtys = Request.Form["Qty"];
                string[] prices = Request.Form["Price"];
                string[] taxes = Request.Form["Tax"];
                string[] totals = Request.Form["Total"];

                for (int i = 0; i < descriptions.Length; i++)
                {
                    long curItemId = long.Parse(ids[i]);
                    decimal? qty = null;
                    decimal? price = null;
                    decimal? tax = null;
                    decimal? total = null;

                    if (!string.IsNullOrEmpty(qtys[i]))
                    {
                        qty = decimal.Parse(qtys[i]);
                    }
                    if (!string.IsNullOrEmpty(prices[i]))
                    {
                        price = decimal.Parse(prices[i]);
                    }
                    if (!string.IsNullOrEmpty(taxes[i]))
                    {
                        tax = decimal.Parse(taxes[i]);
                    }
                    if (!string.IsNullOrEmpty(totals[i]))
                    {
                        total = decimal.Parse(totals[i]);
                    }

                    items.Add(new ProFormaInvoiceItems() { Id = curItemId, Pfid = id, PartNo = partNos[i], Description = descriptions[i], Unit = units[i], Qty = qty, Price = price, Tax = tax, Total = total });
                }

                await _proFormaInvoiceService.UpdateAsync(order, items);

                return new JsonResult(new { id });
            }
            catch
            {
                return View();
            }
        }



        [Route("/Purchase/DeleteProFormaInvoiceItem/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProFormaInvoiceItem(long id)
        {
            try
            {
                await _proFormaInvoiceService.DeleteItemAsync(id);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Purchase/DeleteProFormaInvoice/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteProFormaInvoice(long id)
        {
            try
            {
                var result = await _proFormaInvoiceService.GetByIdAsync(id);
                await _proFormaInvoiceService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

    }
}
