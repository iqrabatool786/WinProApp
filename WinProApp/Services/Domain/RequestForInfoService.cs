using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class RequestForInfoService
    {
        private readonly WinProDbContext _DbContext;

        public RequestForInfoService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<RequestForInformation> GetRequestForInfoByIdAsync(long id)
        {
            return await _DbContext.Set<RequestForInformation>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<RequestForInformation>> GetByAllAsync()
        {
            return await _DbContext.Set<RequestForInformation>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<List<RequestForInformationItem>> GetRequestForInfoItemsByInfoIdAsync(long id)
        {
            return await _DbContext.RequestForInformationItems.Where(s => s.Rfiid == id).ToListAsync();
        }

        public async Task<RequestForInformation> CreateAsync(RequestForInformation model)
        {
            try
            {
               var added = await _DbContext.RequestForInformation.AddAsync(model);
              await _DbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<List<RequestForInformationItem>> CreateItemsAsync(long id, List<RequestForInformationItem> items)
        {
            foreach (var item in items)
            {
                var infoItem = new RequestForInformationItem();
                infoItem.Rfiid = id;
                infoItem.Description = item.Description;
                infoItem.Reason = item.Reason;
                infoItem.Qty = item.Qty;
                infoItem.Mfcompany = item.Mfcompany;

               await _DbContext.RequestForInformationItems.AddAsync(infoItem);
               await _DbContext.SaveChangesAsync();
            }

            List<RequestForInformationItem> allItems = await _DbContext.RequestForInformationItems.Where(x=>x.Rfiid == id).ToListAsync();

            return allItems;
        }

        public async Task<RequestForInformation> UpdateAsync(RequestForInformation model, List<RequestForInformationItem> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var infoItem = _DbContext.RequestForInformationItems.FirstOrDefault(x => x.Id == itemId);
                    infoItem.Rfiid = model.Id;
                    infoItem.Description = item.Description;
                    infoItem.Reason = item.Reason;
                    infoItem.Qty = item.Qty;
                    infoItem.Mfcompany = item.Mfcompany;

                    try
                    {
                        _DbContext.Update(infoItem);
                        await _DbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                }
                else
                {
                    var infoItem = new RequestForInformationItem();
                    infoItem.Rfiid = model.Id;
                    infoItem.Description = item.Description;
                    infoItem.Reason = item.Reason;
                    infoItem.Qty = item.Qty;
                    infoItem.Mfcompany = item.Mfcompany;

                    await _DbContext.RequestForInformationItems.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

            }

            try
            {
                _DbContext.Update(model);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            //var result = await _DbContext.RequestForInformation.Where(x=>x.Id==model.Id).ToListAsync();

            return model;
        }

        public Task<int> DeleteAsync(RequestForInformation model)
        {
            var itemInfo = _DbContext.RequestForInformationItems.Where(x=> x.Id== model.Id);
            foreach (var item in itemInfo)
            {
                long itemId = item.Id;
                var infoItem = _DbContext.RequestForInformationItems.FirstOrDefault(x => x.Id == itemId);

                _DbContext.RequestForInformationItems.Remove(infoItem);
                _DbContext.SaveChangesAsync();
            }
            _DbContext.RequestForInformation.Remove(model);
            return _DbContext.SaveChangesAsync();
        }


        public async Task<List<GetRequestForInformation>> GetRequestForInformationList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetRequestForInformation> rows = null;
            await _DbContext.LoadStoredProc("sp-GetRequestForInformation")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetRequestForInformation>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }


        public async Task<int> DeleteItemAsync(long id)
        {
            var itemInfo = await _DbContext.RequestForInformationItems.FirstOrDefaultAsync(x => x.Id == id);

            _DbContext.RequestForInformationItems.Remove(itemInfo);
            return await _DbContext.SaveChangesAsync();
        }
    }
}
