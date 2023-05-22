using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class FlatDiscountService
    {
        private readonly WinProDbContext _DbContext;

        public FlatDiscountService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<FlatDiscount> GetByIdAsync(int id)
        {
            return await _DbContext.Set<FlatDiscount>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<FlatDiscount>> GetByAllAsync()
        {
            return await _DbContext.Set<FlatDiscount>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<FlatDiscount> CreateAsync(FlatDiscount model)
        {
            _DbContext.FlatDiscount.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<FlatDiscount> UpdateAsync(FlatDiscount model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(FlatDiscount model)
        {
            _DbContext.FlatDiscount.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetFlatDiscount>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetFlatDiscount> rows = null;
            await _DbContext.LoadStoredProc("sp-GetFlatDiscount")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetFlatDiscount>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
