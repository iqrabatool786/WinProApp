using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SelectedItemDiscountService
    {
        private readonly WinProDbContext _DbContext;

        public SelectedItemDiscountService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<SelectedItemDiscount> GetByIdAsync(long id)
        {
            return await _DbContext.Set<SelectedItemDiscount>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SelectedItemDiscount>> GetByAllAsync()
        {
            return await _DbContext.Set<SelectedItemDiscount>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<SelectedItemDiscount> CreateAsync(SelectedItemDiscount model)
        {
            _DbContext.SelectedItemDiscount.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<SelectedItemDiscount> UpdateAsync(SelectedItemDiscount model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }


        public Task<int> DeleteAsync(SelectedItemDiscount model)
        {
            _DbContext.SelectedItemDiscount.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<SelectedItemDiscountItems>> CreateItemsAsync(List<SelectedItemDiscountItems> model)
        {
            await _DbContext.BulkInsertAsync(model);

            return model;
        }

        public async Task<List<SelectedItemDiscountItems>> UpdateItemsAsync(List<SelectedItemDiscountItems> model)
        {
            await _DbContext.BulkInsertOrUpdateAsync(model);

            return model;
        }

        public async Task<List<SelectedItemDiscountItems>> GetAllItemsByIdAsync(long id)
        {
            return await _DbContext.Set<SelectedItemDiscountItems>().Where(x => x.SelectedItemDiscountId == id).OrderBy(x => x.Id).ToListAsync();
        }

        public Task<int> DeleteItemsAsync(List<SelectedItemDiscountItems> model)
        {
            _DbContext.BulkDeleteAsync(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<long> DeleteSingleItemAsync(long id)
        {
            var item = _DbContext.Set<SelectedItemDiscountItems>().Find(id);
            _DbContext.Remove(item);
            return id;
        }

        public async Task<List<GetSelectedItemDiscount>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetSelectedItemDiscount> rows = null;
            await _DbContext.LoadStoredProc("sp-GetSelectedItemDiscount")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSelectedItemDiscount>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
