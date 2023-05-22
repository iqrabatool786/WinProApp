using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SeassonService
    {
        private readonly WinProDbContext _DbContext;

        public SeassonService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<Seassons> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Seassons>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Seassons>> GetByAllAsync()
        {
            return await _DbContext.Set<Seassons>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Seassons> CreateAsync(Seassons model)
        {
            _DbContext.Seassons.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Seassons> UpdateAsync(Seassons model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Seassons model)
        {
            _DbContext.Seassons.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetSeassons>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetSeassons> rows = null;
            await _DbContext.LoadStoredProc("sp-GetSeassons")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSeassons>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Seassons.Where(s => s.NameEng == strName).Count();
            }
            else
            {
                Count = _DbContext.Seassons.Where(s => s.NameEng == strName && s.Id != id).Count();
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
                Count = _DbContext.Seassons.Where(s => s.NameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Seassons.Where(s => s.NameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
