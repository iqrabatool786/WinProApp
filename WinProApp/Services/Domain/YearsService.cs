using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class YearsService
    {
        private readonly WinProDbContext _DbContext;

        public YearsService(WinProDbContext context)
        {
            _DbContext = context;
        }

        public async Task<Years> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Years>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Years>> GetByAllAsync(long id)
        {
            return await _DbContext.Set<Years>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Years> CreateAsync(Years model)
        {
            var yearInfo = await _DbContext.Set<Years>().Where(x => x.YearName == model.YearName).ToListAsync();
            if (yearInfo.Count() == 0)
            {
                _DbContext.Years.Add(model);
                await _DbContext.SaveChangesAsync();
                return model;
            }
            else
            {
                var outmodel = await _DbContext.Set<Years>().FirstOrDefaultAsync(x => x.YearName == model.YearName);
                return outmodel;
            }
            
        }

        public async Task<Years> UpdateAsync(Years model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Years model)
        {
            _DbContext.Years.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetYears>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetYears> rows = null;
            await _DbContext.LoadStoredProc("sp-GetYears")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetYears>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateYear(int? strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Years.Where(s => s.YearName == strName).Count();
            }
            else
            {
                Count = _DbContext.Years.Where(s => s.YearName == strName && s.Id != id).Count();
            }

            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

        
    }
}
