using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Customers;
using WinProApp.DataModels.DataBase;


namespace WinProApp.Controllers
{
    [Authorize]
    public class CustomersController : BasedUserController
    {
        public readonly CustomerService _customerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomersController(CustomerService customerService, IWebHostEnvironment webHostEnvironment)
        {
            _customerService = customerService;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Finance/Customers")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Finance/Reports/CustomerList")]
        [HttpGet]
        public IActionResult CustomerList()
        {
            return View();
        }

        [Route("/Finance/CustomersList")]
        public IActionResult CustomersList(JQueryDataTableParamModel param)
        {
            var results = _customerService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                FullName = x.FullName,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                MobileNumber = x.MobileNumber,
                Email = x.Email,
                CompanyName = x.CompanyName,
                Balance = x.Balance,
                OpeningBalance = x.OpeningBalance,
                CreditLimit=x.CreditLimit,
                CRNo = x.CRNo,
                VatNo = x.VatNo,
                LedgerNo=x.LedgerNo,
                BookNo=x.BookNo,
                CRDocument=x.CRDocument,
                TaxDocument=x.TaxDocument,
                OtherDocument=x.OtherDocument,
                CreatedBy= x.CreatedBy,
                CratedDate=x.CratedDate !=null?x.CratedDate.Value.ToString("yyyy-MM-dd"):null,
                UpdatedBy= x.UpdatedBy,
                UpdatedDate=x.UpdatedDate != null ? x.UpdatedDate.Value.ToString("yyyy-MM-dd") : null,
                TotalRecordCount=x.TotalRecordCount,
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

        [Route("/Finance/CustomerDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var info = await _customerService.GetByIdAsync(id);
            var model = new ViewModels.Customers.DetailsViewModel()
            {
                Id = id,
                FullName = info.FullName,
                Address = info.Address,
                PhoneNumber = info.PhoneNumber,
                MobileNumber = info.MobileNumber,
                Email = info.Email,
                CompanyName = info.CompanyName,
                Balance = info.Balance,
                OpeningBalance = info.OpeningBalance,
                CreditLimit=info.CreditLimit,
                CRNo = info.CRNo,
                VatNo = info.VatNo,
                LedgerNo = info.LedgerNo,
                BookNo = info.BookNo,
                CRDocument = info.CRDocument,
                TaxDocument = info.TaxDocument,
                OtherDocument = info.OtherDocument,
                CreatedBy = info.CreatedBy,
                CratedDate = info.CratedDate != null ? info.CratedDate.ToString("yyyy-MM-dd") : null,
                UpdatedBy = info.UpdatedBy,
                UpdatedDate = info.UpdatedDate != null ? info.UpdatedDate.ToString("yyyy-MM-dd") : null,
            };
            return View(model);
        }

        [Route("/Finance/CreateCustomer")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Route("/Finance/CreateCustomer")]
        [HttpPost]
        public async Task<IActionResult> Create(ViewModels.Customers.AddViewModel model, IFormFile Upload, IFormFile TaxDocUpload, IFormFile OtherDocUpload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Images/Customers/"; ;

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


                var customer = new Customers();

                customer.FullName = model.FullName;
                customer.Address = model.Address;
                customer.MobileNumber = model.PhoneNumber;
                customer.MobileNumber = model.MobileNumber;
                customer.Email = model.Email;
                customer.CompanyName = model.CompanyName;
                customer.Balance = model.Balance;
                customer.OpeningBalance = model.OpeningBalance;
                customer.CreditLimit = model.CreditLimit;
                customer.CRNo = model.CRNo;
                customer.VatNo = model.VatNo;
                customer.LedgerNo = model.LedgerNo;
                customer.BookNo = model.BookNo;
                customer.CRDocument = docName;
                customer.TaxDocument = taxDocName;
                customer.OtherDocument = otherDocName;
                customer.CreatedBy = User.Identity.Name;
                customer.CratedDate = DateTime.Now;
                customer.UpdatedBy = User.Identity.Name;
                customer.UpdatedDate = DateTime.Now;

                await _customerService.CreateAsync(customer);
                return RedirectToAction("Customers", "Finance");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Finance/EditCustomer/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var info = await _customerService.GetByIdAsync(id);
            var model = new ViewModels.Customers.EditViewModel()
            {
                Id = id,
                FullName = info.FullName,
                Address = info.Address,
                PhoneNumber = info.PhoneNumber,
                MobileNumber = info.MobileNumber,
                Email = info.Email,
                CompanyName = info.CompanyName,
                Balance = info.Balance,
                OpeningBalance = info.OpeningBalance,
                CreditLimit=info.CreditLimit,
                CRNo = info.CRNo,
                VatNo = info.VatNo,
                LedgerNo = info.LedgerNo,
                BookNo = info.BookNo,
                CRDocument = info.CRDocument,
                TaxDocument = info.TaxDocument,
                OtherDocument = info.OtherDocument,
            };
            return View(model);
        }

        [Route("/Finance/EditCustomer/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(long id, ViewModels.Customers.EditViewModel model, IFormFile Upload, IFormFile TaxDocUpload, IFormFile OtherDocUpload)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = webRootPath + "/Images/Customers/";

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

                var customer = await _customerService.GetByIdAsync(id);
                string? crdoc = null;
                string? taxDoc = null;
                string? otherDoc = null;
                if (customer.CRDocument != null && docName != null)
                {
                    crdoc = System.IO.Path.Combine(uploadPath, customer.CRDocument);
                    if (System.IO.File.Exists(crdoc))
                    {
                        System.IO.File.Delete(crdoc);
                    }
                }
                if (customer.TaxDocument != null && taxDocName != null)
                {
                    taxDoc = System.IO.Path.Combine(uploadPath, customer.TaxDocument);
                }
                if (customer.OtherDocument != null && otherDocName != null)
                {
                    otherDoc = System.IO.Path.Combine(uploadPath, customer.OtherDocument);
                }

                customer.Id = id;
                customer.FullName = model.FullName;
                customer.Address = model.Address;
                customer.PhoneNumber = model.PhoneNumber;
                customer.MobileNumber = model.MobileNumber;
                customer.Email = model.Email;
                customer.CompanyName = model.CompanyName;
                customer.Balance = model.Balance;
                customer.OpeningBalance = model.OpeningBalance;
                customer.CreditLimit= model.CreditLimit;
                customer.CRNo = model.CRNo;
                customer.VatNo = model.VatNo;
                customer.LedgerNo = model.LedgerNo;
                customer.BookNo = model.BookNo;
                if (!string.IsNullOrEmpty(docName))
                {
                    customer.CRDocument = docName;
                }
                if (!string.IsNullOrEmpty(taxDocName))
                {
                    customer.TaxDocument = taxDocName;
                }
                if (!string.IsNullOrEmpty(otherDocName))
                {
                    customer.OtherDocument = otherDocName;
                }

                customer.UpdatedBy = User.Identity.Name;
                customer.UpdatedDate = DateTime.Now;

                await _customerService.UpdateAsync(customer);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Finance/DeleteCustomer/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _customerService.GetByIdAsync(id);
                await _customerService.DeleteAsync(result);

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
