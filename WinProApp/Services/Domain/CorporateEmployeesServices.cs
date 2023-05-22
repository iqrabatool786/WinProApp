using DocumentFormat.OpenXml.Spreadsheet;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class CorporateEmployeesServices
    {
        private readonly WinProDbContext _DbContext;
        public readonly CommonService _commonService;
        public CorporateEmployeesServices(WinProDbContext DbContext, CommonService commonService)
        {
            _DbContext = DbContext;
            _commonService = commonService;
        }

        public async Task<List<GetCompanies>> GetCompaniesListList(JQueryDataTableParamModel param)
        {
            try
            {
                string strSearch = param.sSearch == null ? " " : param.sSearch;
                int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
                int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
                string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

                List<GetCompanies> rows = null;
                await _DbContext.LoadStoredProc("sp-GetCompanies")
                      .AddParam("Search", strSearch)
                      .AddParam("PageIndex", start)
                      .AddParam("PageSize", recordsPerPage)
                      .AddParam("Sort", strSort)
                      .AddParam("IsCorporate", 1)
                      .ExecAsync(async r =>
                      {
                          rows = await r.ToListAsync<GetCompanies>().ConfigureAwait(false);
                      }).ConfigureAwait(false);

                return rows;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<long> CreateAsync(Company model)
        {
            try
            {
                await _DbContext.Company.AddAsync(model);
                _DbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<List<Customers>> CreateCustomersAsync(long id, IList<Customers> items)
        {
            try 
            {
                foreach(var item in items)
                {
                    _DbContext.Set<Customers>().Add(item);
                    await _DbContext.SaveChangesAsync();
                }
                //await _DbContext.BulkInsertAsync(items, options => options.BatchSize = 100);
                List<Customers> allCustomers = await _DbContext.Customers.Where(x => x.CompanyId == id).ToListAsync();

                return allCustomers;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<Company> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Company>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Customers>> GetCustomersByCompanyIdAsync(long id)
        {
            return await _DbContext.Customers.Where(s => s.CompanyId == id).ToListAsync();
        }

        public async Task<Company> UpdateAsync(Company model, List<Customers> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                var cust = itemId == 0 ? new Customers() : await _DbContext.Customers.FirstOrDefaultAsync(x => x.Id == itemId);
                if(cust != null)
                {
                    cust.CompanyId = item.CompanyId;
                    cust.FullName = item.FullName;
                    cust.Address = item.Address;
                    cust.MobileNumber = item.MobileNumber;
                    cust.DiscountAmount = item.DiscountAmount;
                    cust.IsDiscountPercentage = item.IsDiscountPercentage;
                    if(itemId == 0)
                    {
                        cust.CratedDate = item.CratedDate;
                        cust.CreatedBy = item.CreatedBy;
                    }
                    cust.UpdatedDate = item.UpdatedDate;
                    cust.UpdatedBy = item.UpdatedBy;

                    try
                    {
                        if (itemId == 0)
                        {
                            _DbContext.Set<Customers>().Add(cust);
                        }
                           
                        else
                            _DbContext.Update(cust);

                        await _DbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
            
            Task.Delay(50);
            try
            {
                _DbContext.Update(model);
                await _DbContext.SaveChangesAsync();
                Task.Delay(100);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return model;
        }

        public async Task<long> DeleteCompanyAsync(long companyId)
        {
            try
            {
                var allCustomers = await _DbContext.Customers.Where(x => x.CompanyId == companyId).ToListAsync();
                if (allCustomers.Count > 0)
                {
                    foreach (var cust in allCustomers)
                    {
                        _DbContext.Customers.Remove(cust);
                        await _DbContext.SaveChangesAsync();
                    }
                }
                var model = await _DbContext.Company.FirstOrDefaultAsync(x => x.Id == companyId);
                if (model != null)
                {
                    _DbContext.Company.Remove(model);
                    await _DbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return companyId;
        }

        public async Task DeleteCustomerAsync(long id)
        {
            var custInfo = _DbContext.Customers.FirstOrDefault(x => x.Id == id);
            if (custInfo != null)
            {
                _DbContext.Customers.Remove(custInfo);
            }

            await _DbContext.SaveChangesAsync();
        }
    }
}
