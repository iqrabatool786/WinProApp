using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain.AssetSection
{
    public class AssetDepartmentService
    {
        private readonly WinProDbContext _DbContext;

        public AssetDepartmentService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<AssetDepartment> GetByIdAsync(int id)
        {
            return await _DbContext.Set<AssetDepartment>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<AssetDepartment>> GetByAllAsync()
        {
            return await _DbContext.Set<AssetDepartment>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<AssetDepartment> CreateAsync(AssetDepartment model)
        {
            _DbContext.AssetDepartment.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<AssetDepartment> UpdateAsync(AssetDepartment model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(AssetDepartment model)
        {
            _DbContext.AssetDepartment.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetAssetDepartment>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetAssetDepartment> rows = null;
            await _DbContext.LoadStoredProc("sp-GetAssetDepartment")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetAssetDepartment>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.AssetDepartment.Where(s => s.DepartmentEng == strName).Count();
            }
            else
            {
                Count = _DbContext.AssetDepartment.Where(s => s.DepartmentEng == strName && s.Id != id).Count();
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
                Count = _DbContext.AssetDepartment.Where(s => s.DepartmentArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.AssetDepartment.Where(s => s.DepartmentArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
