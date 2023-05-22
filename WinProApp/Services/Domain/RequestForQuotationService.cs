using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class RequestForQuotationService
    {
        private readonly WinProDbContext _DbContext;

        public RequestForQuotationService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<RequestForQuotation> GetRequestForQuotationByIdAsync(long id)
        {
            return await _DbContext.Set<RequestForQuotation>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<RequestForQuotation>> GetByAllAsync()
        {
            return await _DbContext.RequestForQuotation.OrderByDescending(x=>x.Id).ToListAsync();
        }

        public async Task<List<RequestForQuotationItem>> GetRequestForQuotationItemsByInfoIdAsync(long id)
        {
            return await _DbContext.RequestForQuotationItems.Where(s => s.Rfqid == id).ToListAsync();
        }

        public async Task<RequestForQuotation> CreateAsync(RequestForQuotation model)
        {
            try
            {
                var added = await _DbContext.RequestForQuotation.AddAsync(model);
                await _DbContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<List<RequestForQuotationItem>> CreateItemsAsync(long id, List<RequestForQuotationItem> items)
        {
            foreach (var item in items)
            {
                var infoItem = new RequestForQuotationItem();
                infoItem.Rfqid = id;
                infoItem.Description = item.Description;
                infoItem.Reason = item.Reason;
                infoItem.Qty = item.Qty;
                infoItem.Mfcompany = item.Mfcompany;

                await _DbContext.RequestForQuotationItems.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();
            }

            List<RequestForQuotationItem> allItems = await _DbContext.RequestForQuotationItems.Where(x => x.Rfqid == id).ToListAsync();

            return allItems;
        }

        public async Task<long> UpdateAsync(RequestForQuotation model, List<RequestForQuotationItem> items)
        {

            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var qItem = await _DbContext.RequestForQuotationItems.FirstOrDefaultAsync(x => x.Id == itemId);
                    qItem.Id= itemId;
                    qItem.Description = item.Description;
                    qItem.Reason = item.Reason;
                    qItem.Qty = item.Qty;
                    qItem.Mfcompany = item.Mfcompany;

                    try
                    {
                        _DbContext.Update(qItem);
                        await _DbContext.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        throw ex.InnerException;
                    }
                }
                else
                {
                    var infoItem = new RequestForQuotationItem();
                    infoItem.Rfqid = model.Id;
                    infoItem.Description = item.Description;
                    infoItem.Reason = item.Reason;
                    infoItem.Qty = item.Qty;
                    infoItem.Mfcompany = item.Mfcompany;

                    await _DbContext.RequestForQuotationItems.AddAsync(infoItem);
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

            return model.Id;
        }

        public Task<int> DeleteAsync(RequestForQuotation model)
        {
            var itemInfo = _DbContext.RequestForQuotationItems.Where(x => x.Id == model.Id);
            foreach (var item in itemInfo)
            {
                long itemId = item.Id;
                var infoItem = _DbContext.RequestForQuotationItems.FirstOrDefault(x => x.Id == itemId);

                _DbContext.RequestForQuotationItems.Remove(infoItem);
                _DbContext.SaveChangesAsync();
            }
            _DbContext.RequestForQuotation.Remove(model);
            return _DbContext.SaveChangesAsync();
        }


        public async Task<List<GetRequestForQuotation>> GetRequestForQuotationList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetRequestForQuotation> rows = null;
            await _DbContext.LoadStoredProc("sp-GetRequestForQuotation")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetRequestForQuotation>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }


        public async Task<int> DeleteItemAsync(long id)
        {
            var itemInfo = await _DbContext.RequestForQuotationItems.FirstOrDefaultAsync(x => x.Id == id);

            _DbContext.RequestForQuotationItems.Remove(itemInfo);
            return await _DbContext.SaveChangesAsync();
        }
    }
}
