using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class CategoryServices
    {
        private readonly WinProDbContext _DbContext;

        public CategoryServices(WinProDbContext context)
        {
            _DbContext = context;
        }

        public async Task<Categories> GetByIdAsync(int id)
        {
            return await _DbContext.Set<Categories>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> GetCountByIdAsync(int id)
        {
            return await _DbContext.Categories.Where(s => s.Id == id).CountAsync();
        }

        public async Task<List<Categories>> GetByAllAsync()
        {
            return await _DbContext.Set<Categories>().OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<int> GetSubCategoryCountByIdAsync(int id)
        {
            int count =  await _DbContext.Categories.Where(s =>s.ParentCategoryId == id).CountAsync();
            return count;
        }

        public async Task<Categories> CreateAsync(Categories model)
        {
            _DbContext.Categories.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Categories> UpdateAsync(Categories model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Categories model)
        {
            _DbContext.Categories.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetCategories>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "ASC";

            List<GetCategories> rows = null;
            await _DbContext.LoadStoredProc("sp-GetCategories")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetCategories>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id, int parentId)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Categories.Where(s => s.NameEng == strName && s.ParentCategoryId == parentId).Count();
            }
            else
            {
                Count = _DbContext.Categories.Where(s => s.NameEng == strName && s.ParentCategoryId == parentId && s.Id != id).Count();
            }

            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

        public bool ValidateNameArabic(string strName, int id, int parentId)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Categories.Where(s => s.NameArabic == strName && s.ParentCategoryId == parentId).Count();
            }
            else
            {
                Count = _DbContext.Categories.Where(s => s.NameArabic == strName && s.ParentCategoryId == parentId && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }

       
    }
}
