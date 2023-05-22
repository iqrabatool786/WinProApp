using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain.AssetSection
{
    public class AssetAssignService
    {
        private readonly WinProDbContext _DbContext;

        public AssetAssignService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<AssetAssign> GetByIdAsync(long id)
        {
            return await _DbContext.Set<AssetAssign>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<AssetAssign>> GetByAllAsync()
        {
            return await _DbContext.Set<AssetAssign>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<AssetAssign> CreateAsync(AssetAssign model)
        {
            _DbContext.AssetAssign.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<AssetAssign> UpdateAsync(AssetAssign model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(AssetAssign model)
        {
            _DbContext.AssetAssign.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetAssetAssign>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetAssetAssign> rows = null;
            await _DbContext.LoadStoredProc("sp-GetAssetAssign")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetAssetAssign>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
