using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain.AssetSection
{ 
    public class AssetTypesService
    {
        private readonly WinProDbContext _DbContext;

        public AssetTypesService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<AssetTypes> GetByIdAsync(int id)
        {
            return await _DbContext.Set<AssetTypes>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<AssetTypes>> GetByAllAsync()
        {
            return await _DbContext.Set<AssetTypes>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<AssetTypes> CreateAsync(AssetTypes model)
        {
            _DbContext.AssetTypes.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<AssetTypes> UpdateAsync(AssetTypes model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(AssetTypes model)
        {
            _DbContext.AssetTypes.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetAssetTypes>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetAssetTypes> rows = null;
            await _DbContext.LoadStoredProc("sp-GetAssetTypes")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetAssetTypes>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.AssetTypes.Where(s => s.AssetTypeEng == strName).Count();
            }
            else
            {
                Count = _DbContext.AssetTypes.Where(s => s.AssetTypeEng == strName && s.Id != id).Count();
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
                Count = _DbContext.AssetTypes.Where(s => s.AssetTypeArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.AssetTypes.Where(s => s.AssetTypeArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
