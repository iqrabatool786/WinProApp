using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class BuyGetQtyDiscountService
    {
        private readonly WinProDbContext _DbContext;

        public BuyGetQtyDiscountService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<BuyGetQtyDiscount> GetByIdAsync(long id)
        {
            return await _DbContext.Set<BuyGetQtyDiscount>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<BuyGetQtyDiscount>> GetByAllAsync()
        {
            return await _DbContext.Set<BuyGetQtyDiscount>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<BuyGetQtyDiscount> CreateAsync(BuyGetQtyDiscount model)
        {
            _DbContext.BuyGetQtyDiscount.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<BuyGetQtyDiscount> UpdateAsync(BuyGetQtyDiscount model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(BuyGetQtyDiscount model)
        {
            _DbContext.BuyGetQtyDiscount.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetBuyGetQtyDiscount>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetBuyGetQtyDiscount> rows = null;
            await _DbContext.LoadStoredProc("sp-GetBuyGetQtyDiscount")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetBuyGetQtyDiscount>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
