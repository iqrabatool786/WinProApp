using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain.AssetSection
{
    public class AssetLocationService
    {
        private readonly WinProDbContext _DbContext;

        public AssetLocationService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<AssetLocation> GetByIdAsync(long id)
        {
            return await _DbContext.Set<AssetLocation>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<AssetLocation>> GetByAllAsync()
        {
            return await _DbContext.Set<AssetLocation>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<AssetLocation> CreateAsync(AssetLocation model)
        {
            _DbContext.AssetLocation.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<AssetLocation> UpdateAsync(AssetLocation model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(AssetLocation model)
        {
            _DbContext.AssetLocation.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetAssetLocation>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetAssetLocation> rows = null;
            await _DbContext.LoadStoredProc("sp-GetAssetLocation")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetAssetLocation>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.AssetLocation.Where(s => s.LocationEng == strName).Count();
            }
            else
            {
                Count = _DbContext.AssetLocation.Where(s => s.LocationEng == strName && s.Id != id).Count();
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
                Count = _DbContext.AssetLocation.Where(s => s.LocationArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.AssetLocation.Where(s => s.LocationArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
