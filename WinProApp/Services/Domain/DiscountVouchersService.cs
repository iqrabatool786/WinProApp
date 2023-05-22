using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class DiscountVouchersService
    {
        private readonly WinProDbContext _DbContext;

        public DiscountVouchersService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<DiscountVouchares> GetByIdAsync(long id)
        {
            return await _DbContext.Set<DiscountVouchares>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<DiscountVouchares>> GetByAllAsync()
        {
            return await _DbContext.Set<DiscountVouchares>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<DiscountVouchares> CreateAsync(DiscountVouchares model)
        {
            _DbContext.DiscountVouchares.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<DiscountVouchares> UpdateAsync(DiscountVouchares model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<List<DiscountVouchareItems>> CreateItemsAsync(List<DiscountVouchareItems> model)
        {
           await _DbContext.BulkInsertAsync(model);

            return model;
        }

        public async Task<List<DiscountVouchareItems>> UpdateItemsAsync(List<DiscountVouchareItems> model)
        {
           await _DbContext.BulkInsertOrUpdateAsync(model);

            return model;
        }

        public async Task<List<DiscountVouchareItems>> GetAllItemsByIdAsync(int id)
        {
            return await _DbContext.Set<DiscountVouchareItems>().Where(x=>x.DiscountVoucherId == id).OrderBy(x => x.Id).ToListAsync();
        }

        public Task<int> DeleteAsync(DiscountVouchares model)
        {
            _DbContext.DiscountVouchares.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task<int> DeleteItemsAsync(List<DiscountVouchareItems> model)
        {
            _DbContext.BulkDeleteAsync(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetDiscountVouchares>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetDiscountVouchares> rows = null;
            await _DbContext.LoadStoredProc("sp-GetDiscountVouchares")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetDiscountVouchares>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
