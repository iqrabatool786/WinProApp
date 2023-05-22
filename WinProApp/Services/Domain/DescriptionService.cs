using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class DescriptionService
    {
        private readonly WinProDbContext _DbContext;

        public DescriptionService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<Descriptions> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Descriptions>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Descriptions>> GetByAllAsync()
        {
            return await _DbContext.Set<Descriptions>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Descriptions> CreateAsync(Descriptions model)
        {
            _DbContext.Descriptions.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Descriptions> UpdateAsync(Descriptions model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Descriptions model)
        {
            _DbContext.Descriptions.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetDescriptions>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetDescriptions> rows = null;
            await _DbContext.LoadStoredProc("sp-GetDescriptions")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetDescriptions>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Descriptions.Where(s => s.NameEng == strName).Count();
            }
            else
            {
                Count = _DbContext.Descriptions.Where(s => s.NameEng == strName && s.Id != id).Count();
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
                Count = _DbContext.Descriptions.Where(s => s.NameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Descriptions.Where(s => s.NameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
