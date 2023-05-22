using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class BuyQtyGetAmountOffDiscountService
    {
        private readonly WinProDbContext _DbContext;

        public BuyQtyGetAmountOffDiscountService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<BuyQtyGetAmountOffDiscount> GetByIdAsync(long id)
        {
            return await _DbContext.Set<BuyQtyGetAmountOffDiscount>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<BuyQtyGetAmountOffDiscount>> GetByAllAsync()
        {
            return await _DbContext.Set<BuyQtyGetAmountOffDiscount>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<BuyQtyGetAmountOffDiscount> CreateAsync(BuyQtyGetAmountOffDiscount model)
        {
            _DbContext.BuyQtyGetAmountOffDiscount.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<BuyQtyGetAmountOffDiscount> UpdateAsync(BuyQtyGetAmountOffDiscount model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(BuyQtyGetAmountOffDiscount model)
        {
            _DbContext.BuyQtyGetAmountOffDiscount.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetBuyQtyGetAmountOffDiscount>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetBuyQtyGetAmountOffDiscount> rows = null;
            await _DbContext.LoadStoredProc("sp-GetBuyQtyGetAmountOffDiscount")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetBuyQtyGetAmountOffDiscount>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
