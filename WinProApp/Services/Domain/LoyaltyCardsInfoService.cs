using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class LoyaltyCardsInfoService
    {
        private readonly WinProDbContext _DbContext;

        public LoyaltyCardsInfoService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<LoyaltyCardsInfo> GetByIdAsync(long id)
        {
            return await _DbContext.Set<LoyaltyCardsInfo>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<LoyaltyCardsInfo>> GetByAllAsync()
        {
            return await _DbContext.Set<LoyaltyCardsInfo>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<LoyaltyPointSystem>> GetByAllPointsAsync()
        {
            return await _DbContext.Set<LoyaltyPointSystem>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<LoyaltyPointSystem> GetPointSystemByIdAsync(long id)
        {
            return await _DbContext.Set<LoyaltyPointSystem>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<LoyaltyCardsInfo> CreateAsync(LoyaltyCardsInfo model)
        {
            _DbContext.LoyaltyCardsInfo.Add(model);
            await _DbContext.SaveChangesAsync();

            long Id = model.Id;
            var customer = _DbContext.Customers.Find(model.CustomerId);
            customer.LoyaltyCardId = Id;
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public async Task<LoyaltyPointSystem> CreatePointSystemAsync(LoyaltyPointSystem model)
        {
            _DbContext.LoyaltyPointSystem.Add(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public async Task<LoyaltyPointSystem> UpdatePointSystemAsync(LoyaltyPointSystem model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public async Task<List<GetLoyaltyCardsInfo>> GetCustomersWithoutCardList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetLoyaltyCardsInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetCustomersWithoutLoyaltyCard")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetLoyaltyCardsInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<GetLoyaltyCardsInfo>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetLoyaltyCardsInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetLoyaltyCardsInfo")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetLoyaltyCardsInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<GetLoyaityPointInfo>> GetPointInfoList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetLoyaityPointInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetLoyaltyPointInfo")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetLoyaityPointInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
