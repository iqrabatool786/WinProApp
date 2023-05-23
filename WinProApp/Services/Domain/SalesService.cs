using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System.Data;
using System.Data.Common;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SalesService
    {
        private readonly WinProDbContext _Context;

        public SalesService(WinProDbContext Context)
        {
            _Context = Context;
        }

        public async Task<bool> IsCrNoExist(string CrNo, long Id = 0)
        {
            bool response;
            var exists = await _Context.Set<Supplier>().FirstOrDefaultAsync(s => s.CRNumber == CrNo);
            response = exists != null ? true : false;
            if (Id > 0 && exists != null && Id == exists.Id)
            {
                response = false;
            }
            return response;
        }

        
       

       
        public async Task<List<SalesReport>> GetSalesReport(int storeId, DateTime? fromDt, DateTime? toDt)
        {
            try
            {
                List<SalesReport> rows = null;
                await _Context.LoadStoredProc("sp-GetSalesReport")
                      .AddParam("@StoreId", storeId)
                      .AddParam("@FromDate", fromDt)
                      .AddParam("@ToDate", toDt)

                      .ExecAsync(async r =>
                      {
                          rows = await r.ToListAsync<SalesReport>().ConfigureAwait(false);
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
