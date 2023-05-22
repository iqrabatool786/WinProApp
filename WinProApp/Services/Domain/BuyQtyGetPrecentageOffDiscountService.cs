using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class BuyQtyGetPrecentageOffDiscountService
    {
        private readonly WinProDbContext _DbContext;

        public BuyQtyGetPrecentageOffDiscountService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<BuyQtyGetPrecentageOffDiscount> GetByIdAsync(long id)
        {
            return await _DbContext.Set<BuyQtyGetPrecentageOffDiscount>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<BuyQtyGetPrecentageOffDiscount>> GetByAllAsync()
        {
            return await _DbContext.Set<BuyQtyGetPrecentageOffDiscount>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<BuyQtyGetPrecentageOffDiscount> CreateAsync(BuyQtyGetPrecentageOffDiscount model)
        {
            _DbContext.BuyQtyGetPrecentageOffDiscount.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<BuyQtyGetPrecentageOffDiscount> UpdateAsync(BuyQtyGetPrecentageOffDiscount model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(BuyQtyGetPrecentageOffDiscount model)
        {
            _DbContext.BuyQtyGetPrecentageOffDiscount.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetBuyQtyGetPrecentageOffDiscount>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetBuyQtyGetPrecentageOffDiscount> rows = null;
            await _DbContext.LoadStoredProc("sp-GetBuyQtyGetPrecentageOffDiscount")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetBuyQtyGetPrecentageOffDiscount>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
