﻿using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class GroupService
    {
        private readonly WinProDbContext _DbContext;

        public GroupService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<Groups> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Groups>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Groups>> GetByAllAsync()
        {
            return await _DbContext.Set<Groups>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Groups> CreateAsync(Groups model)
        {
            _DbContext.Groups.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Groups> UpdateAsync(Groups model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Groups model)
        {
            _DbContext.Groups.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetGroups>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetGroups> rows = null;
            await _DbContext.LoadStoredProc("sp-GetGroups")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetGroups>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public bool ValidateNameEng(string strName, int id)
        {
            bool flag = false;
            int Count = 0;
            if (id == 0)
            {
                Count = _DbContext.Groups.Where(s => s.NameEng == strName).Count();
            }
            else
            {
                Count = _DbContext.Groups.Where(s => s.NameEng == strName && s.Id != id).Count();
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
                Count = _DbContext.Groups.Where(s => s.NameArabic == strName).Count();
            }
            else
            {
                Count = _DbContext.Groups.Where(s => s.NameArabic == strName && s.Id != id).Count();
            }
            if (Count == 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
