using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SizeServices
    {
        private readonly WinProDbContext _DbContext;

        public SizeServices(WinProDbContext context)
        {
            _DbContext = context;
        }

        public async Task<Sizes> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Sizes>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Sizes>> GetByAllAsync(long id)
        {
            return await _DbContext.Set<Sizes>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Sizes> CreateAsync(Sizes model)
        {
            _DbContext.Sizes.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Sizes> UpdateAsync(Sizes model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Sizes model)
        {
            _DbContext.Sizes.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetSizes>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetSizes> rows = null;
            await _DbContext.LoadStoredProc("sp-GetSizes")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSizes>().ConfigureAwait(false);
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
                Count = _DbContext.Sizes.Where(s => s.NameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Sizes.Where(s => s.NameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

    }
}
