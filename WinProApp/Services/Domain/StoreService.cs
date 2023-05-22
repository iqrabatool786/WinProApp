using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class StoreService
    {
        private readonly WinProDbContext _DbContext;

        public StoreService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<Store> GetByIdAsync(int id)
        {
            return await _DbContext.Store.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<string> GetNameByIdAsync(int id)
        {
            string? storeName = "";

            if (_DbContext.Store.Where(s => s.Id == id).Count() > 0)
            {
                var storeInfo = await _DbContext.Store.FirstOrDefaultAsync(s => s.Id == id);
                storeName = storeInfo.Name;
            }
                
            return storeName;
        }

        public async Task<List<Store>> GetByAllAsync()
        {
            return await _DbContext.Set<Store>().OrderByDescending(x => x.Name).ToListAsync();
        }

        public async Task<int> GetMaxIdAsync()
        {
            int maxRecordId = 0;
            if (_DbContext.Store.Count() > 0)
            {
                maxRecordId = await _DbContext.Store.MaxAsync(x=>x.Id);
            }
            
            return maxRecordId;
        }

        public async Task<int> GetRegisterCountByIdAsync(int id)
        {
            int regCount = 0;
            regCount = await _DbContext.Register.Where(x => x.StoreId == id).CountAsync();


            return regCount;
        }

        public async Task<Store> CreateAsync(Store model)
        {
            _DbContext.Store.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Store> UpdateAsync(Store model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Store model)
        {
            _DbContext.Store.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetStore>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetStore> rows = null;
            await _DbContext.LoadStoredProc("sp-GetStore")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetStore>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Store.Where(s => s.Name == strName).Count();
            }
            else
            {
                Count = _DbContext.Store.Where(s => s.Name == strName && s.Id != id).Count();
            }

            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

        public bool ValidateNameArabic(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Store.Where(s => s.Name == strName).Count();
            }
            else
            {
                Count = _DbContext.Store.Where(s => s.Name == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
