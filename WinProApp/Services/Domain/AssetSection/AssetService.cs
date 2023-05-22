using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain.AssetSection
{
    public class AssetService
    {
        private readonly WinProDbContext _DbContext;

        public AssetService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<Asset> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Asset>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Asset>> GetByAllAsync()
        {
            return await _DbContext.Set<Asset>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Asset> CreateAsync(Asset model)
        {
            _DbContext.Asset.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Asset> UpdateAsync(Asset model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Asset model)
        {
            _DbContext.Asset.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetAsset>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetAsset> rows = null;
            await _DbContext.LoadStoredProc("sp-GetAsset")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetAsset>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<Asset>> GetAssetByBarcodeAsync(string Prefix)
        {
            return await _DbContext.Set<Asset>().Where(x => x.Barcode.Contains(Prefix)).OrderBy(x => x.AssetNameEng).Take(20).ToListAsync();
        }

        public async Task<List<Asset>> GetAssetByNameEngAsync(string Prefix)
        {
            return await _DbContext.Set<Asset>().Where(x => x.AssetNameEng.Contains(Prefix)).OrderBy(x => x.AssetNameEng).Take(20).ToListAsync();
        }

        public async Task<List<Asset>> GetAssetByNameArabicAsync(string Prefix)
        {
            return await _DbContext.Set<Asset>().Where(x => x.AssetNameArabic.Contains(Prefix)).OrderBy(x => x.AssetNameArabic).Take(20).ToListAsync();
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Asset.Where(s => s.AssetNameEng == strName).Count();
            }
            else
            {
                Count = _DbContext.Asset.Where(s => s.AssetNameEng == strName && s.Id != id).Count();
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
                Count = _DbContext.Asset.Where(s => s.AssetNameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Asset.Where(s => s.AssetNameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

       
    }
}
