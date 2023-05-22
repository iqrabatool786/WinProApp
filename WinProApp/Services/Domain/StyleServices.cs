using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class StyleServices
    {
        private readonly WinProDbContext _DbContext;

        public StyleServices(WinProDbContext context)
        {
            _DbContext = context;
        }

        public async Task<Styles> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Styles>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Styles>> GetByAllAsync(long id)
        {
            return await _DbContext.Set<Styles>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Styles> CreateAsync(Styles model)
        {
            _DbContext.Styles.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Styles> UpdateAsync(Styles model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Styles model)
        {
            _DbContext.Styles.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetStyles>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetStyles> rows = null;
            await _DbContext.LoadStoredProc("sp-GetStyles")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetStyles>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Styles.Where(s => s.NameEng == strName).Count();
            }
            else
            {
                Count = _DbContext.Styles.Where(s => s.NameEng == strName && s.Id != id).Count();
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
                Count = _DbContext.Styles.Where(s => s.NameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Styles.Where(s => s.NameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

        public bool ValidateCode(string strCode, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Styles.Where(s => s.Code == strCode).Count();
            }
            else
            {
                Count = _DbContext.Styles.Where(s => s.Code == strCode && s.Id != id).Count();
            }

            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
