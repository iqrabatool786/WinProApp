using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;
namespace WinProApp.Services.Domain
{
    public class RequestForPurchaseService
    {
        private readonly WinProDbContext _DbContext;

        public RequestForPurchaseService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<RequestForPurchases> GetByIdAsync(long id)
        {
            return _DbContext.RequestForPurchases.FirstOrDefaultAsync(s => s.Id == id).Result;
        }

        public async Task<List<RequestForPurchases>> GetByAllAsync()
        {
            return await _DbContext.RequestForPurchases.OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<RequestForPurchases> CreateAsync(RequestForPurchases model)
        {
            _DbContext.RequestForPurchases.Add(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public async Task<List<RequestForPurchaseItem>> CreateItemsAsync(long id, List<RequestForPurchaseItem> items)
        {
            foreach (var item in items)
            {
                var infoItem = new RequestForPurchaseItem();
                infoItem.Prid = id;
                infoItem.PartNo = item.PartNo;
                infoItem.Description = item.Description;
                infoItem.Reason = item.Reason;
                infoItem.Qty = item.Qty;
                infoItem.Price = item.Price;
                infoItem.Mfcompany = item.Mfcompany;

                await _DbContext.RequestForPurchaseItems.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();
            }

            List<RequestForPurchaseItem> allItems = await _DbContext.RequestForPurchaseItems.Where(x => x.Prid == id).ToListAsync();

            return allItems;
        }

        public async Task<RequestForPurchases> UpdateAsync(RequestForPurchases model, List<RequestForPurchaseItem> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var infoItem = _DbContext.RequestForPurchaseItems.FirstOrDefault(x => x.Id == itemId);
                    infoItem.PartNo = item.PartNo;
                    infoItem.Description = item.Description;
                    infoItem.Reason = item.Reason;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Mfcompany = item.Mfcompany;

                    _DbContext.Update(infoItem);
                    await _DbContext.SaveChangesAsync();
                }
                else
                {
                    var infoItem = new RequestForPurchaseItem();
                    infoItem.Prid = model.Id;
                    infoItem.PartNo = item.PartNo;
                    infoItem.Description = item.Description;
                    infoItem.Reason = item.Reason;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Mfcompany = item.Mfcompany;

                    await _DbContext.RequestForPurchaseItems.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

            }

            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public Task<int> DeleteAsync(RequestForPurchases model)
        {
            _DbContext.RequestForPurchases.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.RequestForPurchaseItems.FirstOrDefault(x=>x.Id== id);
            _DbContext.RequestForPurchaseItems.Remove(item);
            
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<RequestForPurchaseItem>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.RequestForPurchaseItems.Where(s => s.Prid == id).ToListAsync();
        }

        public async Task<List<GetRequestForPurchase>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetRequestForPurchase> rows = null;
            await _DbContext.LoadStoredProc("sp-GetRequestForPurchase")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetRequestForPurchase>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }
    }
}
