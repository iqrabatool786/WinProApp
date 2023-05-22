using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class GrnInfoService
    {
        private readonly WinProDbContext _DbContext;

        public GrnInfoService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<GrnInfo> GetByIdAsync(long id)
        {
            return await _DbContext.GrnInfo.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<GrnInfo>> GetByAllAsync()
        {
            return await _DbContext.Set<GrnInfo>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<IbtInfo>> GetStoreIbtByStoreIdsync(int id)
        {
            if (id > 0)
            {
                var info = from b1 in _DbContext.IbtInfo
                           where b1.ToStoreId == id && b1.Status == true
                           from g1 in _DbContext.GrnInfo
                           where g1.IbtId != b1.Id && b1.Status == true
                           select b1;


                return await info.OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                var info = from b1 in _DbContext.IbtInfo
                           where !_DbContext.GrnInfo.Any(x=>x.IbtId == b1.Id) && b1.Status == true
                           select b1;

                return await info.OrderByDescending(x => x.Id).ToListAsync();
            }
        }

        public async Task<GrnInfo> CreateAsync(GrnInfo model)
        {
            _DbContext.GrnInfo.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<GrnInfo> UpdateAsync(GrnInfo model, List<GrnDetails> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var infoItem = await _DbContext.GrnDetails.FirstOrDefaultAsync(x => x.Id == itemId);
                    infoItem.GrnId = model.Id;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.BoxNo = item.BoxNo;
                    infoItem.Qty = item.Qty;
                    infoItem.ReceivedQty = item.ReceivedQty;
                    infoItem.Price = item.Price;
                    infoItem.Precentage = item.Precentage;
                    infoItem.RetailPrice = item.RetailPrice;
                    await _DbContext.SaveChangesAsync();
                }
                if (itemId == 0)
                {
                    var infoItem = new GrnDetails();
                    infoItem.GrnId = model.Id;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.BoxNo = item.BoxNo;
                    infoItem.Qty = item.Qty;
                    infoItem.ReceivedQty = item.ReceivedQty;
                    infoItem.Price = item.Price;
                    infoItem.Precentage = item.Precentage;
                    infoItem.RetailPrice = item.RetailPrice;
                    await _DbContext.GrnDetails.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

                if (model.Status == true)
                {
                    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    productInfo.Currentstock += item.Qty;
                    await _DbContext.SaveChangesAsync();
                }

            }

            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }


        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.GrnDetails.FirstOrDefault(x => x.Id == id);
            _DbContext.GrnDetails.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(GrnInfo model)
        {
            _DbContext.GrnInfo.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetGrnInfo>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetGrnInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetGrnInfo")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetGrnInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }


        public async Task<List<GrnDetails>> GetItemsByIbtIdAsync(long id)
        {
            return await _DbContext.GrnDetails.Where(s => s.GrnId == id).ToListAsync();
        }

        public async Task<List<GrnDetails>> CreateItemsAsync(long id, List<GrnDetails> items)
        {
            var IbtInfo = await _DbContext.GrnInfo.FirstOrDefaultAsync(x => x.Id == id);
            foreach (var item in items)
            {
                var infoItem = new GrnDetails();
                infoItem.GrnId = id;
                infoItem.ProductId = item.ProductId;
                infoItem.Barcode = item.Barcode;
                infoItem.BoxNo = item.BoxNo;
                infoItem.Qty = item.Qty;
                infoItem.ReceivedQty= item.ReceivedQty;
                infoItem.Price = item.Price;
                infoItem.Precentage = item.Precentage;
                infoItem.RetailPrice = item.RetailPrice;

                _DbContext.GrnDetails.Add(infoItem);
                await _DbContext.SaveChangesAsync();

                if (IbtInfo.Status == true)
                {
                    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    productInfo.Currentstock += item.Qty;
                    await _DbContext.SaveChangesAsync();
                }
            }

            List<GrnDetails> allItems = await _DbContext.GrnDetails.Where(x => x.GrnId == id).ToListAsync();

            return allItems;
        }
    }
}
