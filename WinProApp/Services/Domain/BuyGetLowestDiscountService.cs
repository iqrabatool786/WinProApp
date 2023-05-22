using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class BuyGetLowestDiscountService
    {
        private readonly WinProDbContext _DbContext;

        public BuyGetLowestDiscountService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<BuyGetLowestDiscount> GetByIdAsync(long id)
        {
            return await _DbContext.Set<BuyGetLowestDiscount>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<BuyGetLowestDiscount>> GetByAllAsync()
        {
            return await _DbContext.Set<BuyGetLowestDiscount>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<BuyGetLowestDiscount> CreateAsync(BuyGetLowestDiscount model)
        {
            _DbContext.BuyGetLowestDiscount.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<BuyGetLowestDiscount> UpdateAsync(BuyGetLowestDiscount model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(BuyGetLowestDiscount model)
        {
            _DbContext.BuyGetLowestDiscount.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetBuyGetLowestDiscount>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetBuyGetLowestDiscount> rows = null;
            await _DbContext.LoadStoredProc("sp-GetBuyGetLowestDiscount")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetBuyGetLowestDiscount>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
