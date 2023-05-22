using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class RegisterService
    {
        private readonly WinProDbContext _DbContext;

        public RegisterService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<Register> GetByIdAsync(long id)
        {
            return await _DbContext.Set<Register>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Register>> GetByAllAsync(long id)
        {
            return await _DbContext.Set<Register>().OrderByDescending(x=>x.Id).ToListAsync();
        }

        public async Task<Register> CreateAsync(Register model)
        {
            try
            {
                _DbContext.Register.Add(model);
                await _DbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<Register> UpdateAsync(Register model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(Register model)
        {
            _DbContext.Register.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetRegister>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetRegister> rows = null;
            await _DbContext.LoadStoredProc("sp-GetRegister")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetRegister>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }


    }
}
