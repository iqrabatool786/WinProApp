using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class ColorServices
    {
        private readonly WinProDbContext _DbContext;

        public ColorServices(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<Colors> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Colors>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Colors>> GetByAllAsync()
        {
            return await _DbContext.Set<Colors>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Colors> CreateAsync(Colors model)
        {
            _DbContext.Colors.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Colors> UpdateAsync(Colors model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Colors model)
        {
            _DbContext.Colors.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetColors>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetColors> rows = null;
            await _DbContext.LoadStoredProc("sp-GetColors")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetColors>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Colors.Where(s => s.NameEng == strName).Count();
            }
            else
            {
                Count = _DbContext.Colors.Where(s => s.NameEng == strName && s.Id != id).Count();
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
                Count = _DbContext.Colors.Where(s => s.NameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Colors.Where(s => s.NameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
