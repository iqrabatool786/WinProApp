using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    
    public class CustomerService
    {
        private readonly WinProDbContext _DbContext;
        public CustomerService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<Customers> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Customers>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Customers>> GetByAllAsync(long id)
        {
            return await _DbContext.Set<Customers>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Customers> CreateAsync(Customers model)
        {
            _DbContext.Customers.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Customers> UpdateAsync(Customers model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Customers model)
        {
            _DbContext.Customers.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetCustomers>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 10;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetCustomers> rows = null;
            await _DbContext.LoadStoredProc("sp-GetCustomers")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetCustomers>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
