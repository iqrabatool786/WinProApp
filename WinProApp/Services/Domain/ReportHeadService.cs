using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class ReportHeadService
    {
        private readonly WinProDbContext _DbContext;

        public ReportHeadService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<ReportHeads> GetByIdAsync(int id)
        {
            return await _DbContext.ReportHeads.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ReportHeads> GetByStoreIdAsync(int id)
        {
            try
            {
                return await _DbContext.ReportHeads.FirstOrDefaultAsync(s => s.StoreId == id);

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<string> GetByStoreLogoByIdAsync(int id)
        {
            string logoName = "";
            if (_DbContext.ReportHeads.Where(x => x.Id == id).Count() > 0)
            {
                var info = await _DbContext.ReportHeads.FirstOrDefaultAsync(s => s.StoreId == id);
                logoName = info.Logo;
                return logoName;
            }
            else
            {
                return logoName;
            }
        }

        public async Task<string> GetStoreNameByIdAsync(int id)
        {
            string storeName = _DbContext.Store.FirstOrDefaultAsync(s => s.Id == id)?.Result?.Name;
            return storeName;
        }

        public async Task<List<ReportHeads>> GetByAllAsync()
        {
            return await _DbContext.Set<ReportHeads>().OrderByDescending(x => x.ReportHeaderEng).ToListAsync();
        }


        public async Task<ReportHeads> CreateAsync(ReportHeads model)
        {
            if (model.DefaultStore == true)
            {
                if (_DbContext.ReportHeads.Where(x => x.DefaultStore == true).Count() > 0)
                {
                    var curInfo = await _DbContext.ReportHeads.FirstOrDefaultAsync(x => x.DefaultStore == true);
                    curInfo.DefaultStore = false;
                    await _DbContext.SaveChangesAsync();
                }
            }
            _DbContext.ReportHeads.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<ReportHeads> UpdateAsync(ReportHeads model)
        {
                if (_DbContext.ReportHeads.Where(x => x.DefaultStore == true).Count() > 0 && model.DefaultStore == true)
                {
                    var curInfo = await _DbContext.ReportHeads.FirstOrDefaultAsync(x => x.DefaultStore == true);
                    curInfo.DefaultStore=false; 
                    await _DbContext.SaveChangesAsync();
                }


            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(ReportHeads model)
        {
            _DbContext.ReportHeads.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetReportHeads>> GetList(JQueryDataTableParamModel param)
        {
            try
            {
                string strSearch = param.sSearch == null ? " " : param.sSearch;
                int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
                int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
                string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

                List<GetReportHeads> rows = null;
                await _DbContext.LoadStoredProc("sp-GetReportHeads")
                      .AddParam("Search", strSearch)
                      .AddParam("PageIndex", start)
                      .AddParam("PageSize", recordsPerPage)
                      .AddParam("Sort", strSort)
                      .ExecAsync(async r =>
                      {
                          rows = await r.ToListAsync<GetReportHeads>().ConfigureAwait(false);
                      }).ConfigureAwait(false);

                return rows;
            }
            catch (Exception e)
            {

                throw;
            }
            
        }
    }
}
