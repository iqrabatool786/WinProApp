using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;
namespace WinProApp.Services.Domain
{
    public class PurchaseOrderService
    {
        private readonly WinProDbContext _DbContext;

        public PurchaseOrderService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<PurchaseOrders> GetByIdAsync(long id)
        {
            return await _DbContext.Set<PurchaseOrders>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<PurchaseOrders>> GetByAllAsync()
        {
            return await _DbContext.Set<PurchaseOrders>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<PurchaseOrders> GetByRequestIdAsync(long id)
        {
            return await _DbContext.Set<PurchaseOrders>().FirstOrDefaultAsync(s => s.Prid == id);
        }

        public async Task<PurchaseOrders> CreateAsync(PurchaseOrders model)
        {
            _DbContext.PurchaseOrders.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<PurchaseOrders> UpdateAsync(PurchaseOrders model, List<PurchaseOrderItems> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId == 0)
                {
                    var infoItem = new PurchaseOrderItems();
                    infoItem.Poid = model.Id;
                    infoItem.PartNo = item.PartNo;
                    infoItem.Description = item.Description;
                    infoItem.Unit = item.Unit;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Tax = item.Tax;
                    infoItem.Total = item.Total;

                    await _DbContext.PurchaseOrderItems.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

            }

            _DbContext.Update(model);
           await _DbContext.SaveChangesAsync();

            return model;
        }

        public Task<int> DeleteAsync(PurchaseOrders model)
        {
            _DbContext.PurchaseOrders.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.PurchaseOrderItems.FirstOrDefault(x => x.Id == id);
            _DbContext.PurchaseOrderItems.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetPurchaseOrder>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetPurchaseOrder> rows = null;
            await _DbContext.LoadStoredProc("sp-GetPurchaseOrder")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetPurchaseOrder>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<PurchaseOrderItems>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.PurchaseOrderItems.Where(s => s.Poid == id).ToListAsync();
        }

        public async Task<List<PurchaseOrderItems>> CreateItemsAsync(long id, List<PurchaseOrderItems> items)
        {
            foreach (var item in items)
            {
                var infoItem = new PurchaseOrderItems();
                infoItem.Poid = id;
                infoItem.PartNo = item.PartNo;
                infoItem.Description = item.Description;
                infoItem.Unit = item.Unit;
                infoItem.Qty = item.Qty;
                infoItem.Price = item.Price;
                infoItem.Tax = item.Tax;
                infoItem.Total = item.Total;

                await _DbContext.PurchaseOrderItems.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();
            }

            List<PurchaseOrderItems> allItems = await _DbContext.PurchaseOrderItems.Where(x => x.Poid == id).ToListAsync();

            return allItems;
        }
       
    }
}
