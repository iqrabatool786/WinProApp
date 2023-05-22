using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.NewModels;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.CorporateEmployees;
using Company = WinProApp.DataModels.DataBase.Company;

namespace WinProApp.Controllers.Merchant
{
    public class CorporateEmployeeController : Controller
    {
        public readonly CorporateEmployeesServices _corporateEmployeeService;
        public CorporateEmployeeController(CorporateEmployeesServices corporateEmployeeService) 
        {
            _corporateEmployeeService = corporateEmployeeService;
        }

        [Route("/Merchant/CorporateEmployees")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Merchant/CorporateEmployeesList")]
        [HttpPost]
        public async Task<IActionResult> CorporateEmployeesList(JQueryDataTableParamModel param)
        {
            var results = _corporateEmployeeService.GetCompaniesListList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                Address = x.Address,
                PhoneNo = x.PhoneNo,
                Fax = x.Fax,
                Email = x.Email,
                Website = x.Website,
                Status = x.Status,
                InsertBy = x.InsertBy,
                Bpcode = x.Bpcode,
                Balance = x.Balance,
                OpeningBalance = x.OpeningBalance,
                VatNo = x.VatNo,
                BankName = x.BankName,
                BankAccountNumber = x.BankAccountNumber,
                CratedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedDate = DateTime.Now,
                UpdatedBy = User.Identity.Name
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

        [Route("/Merchant/CreateCorporateEmployee")]
        [HttpGet]
        public async Task<IActionResult> CreateCorporateEmployee(System.Globalization.CultureInfo requestCulture)
        {
            return View();
        }

        [Route("/Merchant/CreateCorporateEmployee")]
        [HttpPost]
        public async Task<IActionResult> CreateCorporateEmployee(ListViewModel model)
        {
            try
            {
                var company = new Company();
                company.CompanyName = model.CompanyName;
                company.Address = model.Address;
                company.PhoneNo = model.PhoneNo;
                company.Fax = model.Fax;
                company.Email = model.Email;
                company.Website = model.Website;
                company.Status = model.Status;
                company.InsertBy = model.InsertBy;
                company.Bpcode = model.Bpcode;
                company.Balance = model.Balance;
                company.OpeningBalance = model.OpeningBalance;
                company.VatNo = model.VatNo;
                company.BankName = model.BankName;
                company.BankAccountNumber = model.BankAccountNumber;
                company.IsCorporate = true;
                company.CratedDate = DateTime.Now;
                company.CreatedBy = User.Identity.Name;
                company.UpdatedDate = DateTime.Now;
                company.UpdatedBy = User.Identity.Name;

                long curInsertId = await _corporateEmployeeService.CreateAsync(company);
                //long curInsertId = 0;
                List<Customers> customerss = new List<Customers>();

                string[] ids = Request.Form["CustomerId"];
                string[] custNames = Request.Form["Custname"];
                string[] custAddressses = Request.Form["CustAddress"];
                string[] custMobiles = Request.Form["CustMobile"];
                string[] custDiscounts = Request.Form["CustDiscount"];
                string[] CustIsDiscPercentage = Request.Form["CustIsDiscPerc"];

                for (int i = 0; i < ids.Length; i++)
                {
                    var custInfo = new Customers();
                    custInfo.CompanyId = Convert.ToInt16(curInsertId);
                    custInfo.FullName = custNames[i];
                    custInfo.Address = custAddressses[i];
                    custInfo.MobileNumber = custMobiles[i];
                    custInfo.DiscountAmount = Convert.ToDecimal(custDiscounts[i]);
                    custInfo.IsDiscountPercentage = Convert.ToBoolean(CustIsDiscPercentage[i]);
                    custInfo.CratedDate = DateTime.Now;
                    custInfo.CreatedBy = User.Identity.Name;
                    custInfo.UpdatedDate = DateTime.Now;
                    custInfo.UpdatedBy = User.Identity.Name;
                    customerss.Add(custInfo);
                }

                await _corporateEmployeeService.CreateCustomersAsync(curInsertId, customerss);
                return new JsonResult(new { id = curInsertId });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        [Route("/Merchant/EditCorporateEmployee/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditCorporateEmployee(long id)
        {
            var company = await _corporateEmployeeService.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            var model = new EditViewModel();
            model.Id = id;
            model.CompanyName = company.CompanyName;
            model.Address = company.Address;
            model.PhoneNo = company.PhoneNo;
            model.Fax = company.Fax;
            model.Email = company.Email;
            model.Website = company.Website;
            model.Status = company.Status;
            model.InsertBy = company.InsertBy;
            model.Bpcode = company.Bpcode;
            model.Balance = company.Balance;
            model.OpeningBalance = company.OpeningBalance;
            model.VatNo = company.VatNo;
            model.BankName = company.BankName;
            model.BankAccountNumber = company.BankAccountNumber;
            model.CratedDate = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
            model.UpdatedDate = DateTime.Now;
            model.UpdatedBy = User.Identity.Name;
            model.Customers = await _corporateEmployeeService.GetCustomersByCompanyIdAsync(id);

            return View(model);
        }


        [Route("/Merchant/EditCorporateEmployee")]
        [HttpPost]
        public async Task<IActionResult> EditCorporateEmployee(EditViewModel model)
        {
            try
            {
                var company = await _corporateEmployeeService.GetByIdAsync(model.Id);
                company.CompanyName = model.CompanyName;
                company.Address = model.Address;
                company.PhoneNo = model.PhoneNo;
                company.Fax = model.Fax;
                company.Email = model.Email;
                company.Website = model.Website;
                company.Status = model.Status;
                company.InsertBy = model.InsertBy;
                company.Bpcode = model.Bpcode;
                company.Balance = model.Balance;
                company.OpeningBalance = model.OpeningBalance;
                company.VatNo = model.VatNo;
                company.BankName = model.BankName;
                company.BankAccountNumber = model.BankAccountNumber;
                company.UpdatedDate = DateTime.Now;
                company.UpdatedBy = User.Identity.Name;

                List<Customers> customerss = new List<Customers>();
                string[] ids = Request.Form["CustomerId"];
                string[] custNames = Request.Form["Custname"];
                string[] custAddressses = Request.Form["CustAddress"];
                string[] custMobiles = Request.Form["CustMobile"];
                string[] custDiscounts = Request.Form["CustDiscount"];
                string[] CustIsDiscPercentage = Request.Form["CustIsDiscPerc"];

                for (int i = 0; i < ids.Length; i++)
                {
                    var custInfo = new Customers();
                    custInfo.CompanyId = Convert.ToInt16(model.Id);
                    custInfo.Id = Convert.ToInt16(ids[i]);
                    custInfo.FullName = custNames[i];
                    custInfo.Address = custAddressses[i];
                    custInfo.MobileNumber = custMobiles[i];
                    custInfo.DiscountAmount = Convert.ToDecimal(custDiscounts[i]);
                    custInfo.IsDiscountPercentage = Convert.ToBoolean(CustIsDiscPercentage[i]);
                    if (Convert.ToInt16(ids[i]) == 0)
                    {
                        custInfo.CratedDate = DateTime.Now;
                        custInfo.CreatedBy = User.Identity.Name;
                    }
                    custInfo.UpdatedDate = DateTime.Now;
                    custInfo.UpdatedBy = User.Identity.Name;
                    customerss.Add(custInfo);
                }

                await _corporateEmployeeService.UpdateAsync(company, customerss);
                return new JsonResult(new { id = model.Id });
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [Route("/Merchant/ViewCorporateEmployee/{id}")]
        [HttpGet]
        public async Task<IActionResult> ViewCorporateEmployee(long id)
        {
            var company = await _corporateEmployeeService.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            var model = new EditViewModel();
            model.Id = id;
            model.CompanyName = company.CompanyName;
            model.Address = company.Address;
            model.PhoneNo = company.PhoneNo;
            model.Fax = company.Fax;
            model.Email = company.Email;
            model.Website = company.Website;
            model.Status = company.Status;
            model.InsertBy = company.InsertBy;
            model.Bpcode = company.Bpcode;
            model.Balance = company.Balance;
            model.OpeningBalance = company.OpeningBalance;
            model.VatNo = company.VatNo;
            model.BankName = company.BankName;
            model.BankAccountNumber = company.BankAccountNumber;
            model.CratedDate = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
            model.UpdatedDate = DateTime.Now;
            model.UpdatedBy = User.Identity.Name;
            model.Customers = await _corporateEmployeeService.GetCustomersByCompanyIdAsync(id);

            return View(model);
        }

        [Route("/Merchant/DeleteCompany/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteCompany(long id)
        {
            try
            {
                await _corporateEmployeeService.DeleteCompanyAsync(id);
                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Merchant/GetCustomersByCompanyId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCustomersByCompanyId(long id)
        {
            var info = await _corporateEmployeeService.GetCustomersByCompanyIdAsync(id);
            List<Customers> customerss = new List<Customers>();
            if (info != null)
            {
                foreach (var item in info)
                {
                    var custInfo = new Customers();
                    custInfo.Id = Convert.ToInt16(item.Id);
                    custInfo.FullName = item.FullName;
                    custInfo.Address = item.Address;
                    custInfo.MobileNumber = item.MobileNumber;
                    custInfo.DiscountAmount = item.DiscountAmount;
                    custInfo.IsDiscountPercentage = item.IsDiscountPercentage;
                    customerss.Add(custInfo);
                }
            }

            return Json(data: customerss);
        }

        [Route("/Merchant/DeleteCustomer/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            try
            {
                await _corporateEmployeeService.DeleteCustomerAsync(id);

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
