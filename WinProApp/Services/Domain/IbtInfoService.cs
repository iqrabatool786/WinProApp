using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class IbtInfoService
    {
        private readonly WinProDbContext _DbContext;

        public IbtInfoService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<IbtInfo> GetByIdAsync(long id)
        {
            return await _DbContext.IbtInfo.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<IbtInfo>> GetByAllAsync()
        {
            return await _DbContext.Set<IbtInfo>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<List<IbtInfo>> GetToStoreIbtByAllAsync(int id)
        {
            if (id > 0)
            {
                return await _DbContext.Set<IbtInfo>().Where(x => x.ToStoreId == id && x.Status==true).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                return await _DbContext.Set<IbtInfo>().Where(x=>x.Status==true).OrderByDescending(x => x.Id).ToListAsync();
            }
        }

        public async Task<IbtInfo> CreateAsync(IbtInfo model)
        {
            _DbContext.IbtInfo.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<IbtInfo> UpdateAsync(IbtInfo model, List<IbtDetails> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var infoItem = await _DbContext.IbtDetails.FirstOrDefaultAsync(x => x.Id == itemId);
                    infoItem.IbtId = model.Id;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.BoxNo = item.BoxNo;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Precentage = item.Precentage;
                    infoItem.RetailPrice = item.RetailPrice;
                    await _DbContext.SaveChangesAsync();
                }
                if (itemId == 0)
                {
                    var infoItem = new IbtDetails();
                    infoItem.IbtId = model.Id;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.BoxNo = item.BoxNo;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Precentage = item.Precentage;
                    infoItem.RetailPrice = item.RetailPrice;
                    await _DbContext.IbtDetails.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

                if (model.Status == true)
                {
                    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    productInfo.Currentstock -= item.Qty;
                    await _DbContext.SaveChangesAsync();
                }

            }

            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }


        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.IbtDetails.FirstOrDefault(x => x.Id == id);
            _DbContext.IbtDetails.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(IbtInfo model)
        {
            _DbContext.IbtInfo.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetIbtInfo>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "asc";

            List<GetIbtInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetIbtInfo")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetIbtInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }


        public async Task<List<IbtDetails>> GetItemsByIbtIdAsync(long id)
        {
            return await _DbContext.IbtDetails.Where(s => s.IbtId == id).ToListAsync();
        }

        public async Task<List<IbtDetails>> CreateItemsAsync(long id, List<IbtDetails> items)
        {
            var IbtInfo = await _DbContext.IbtInfo.FirstOrDefaultAsync(x=>x.Id == id);
            foreach (var item in items)
            {
                var infoItem = new IbtDetails();
                infoItem.IbtId = id;
                infoItem.ProductId = item.ProductId;
                infoItem.Barcode = item.Barcode;
                infoItem.BoxNo = item.BoxNo;
                infoItem.Qty = item.Qty;
                infoItem.Price = item.Price;
                infoItem.Precentage = item.Precentage;
                infoItem.RetailPrice = item.RetailPrice;

                _DbContext.IbtDetails.Add(infoItem);
                await _DbContext.SaveChangesAsync();

                if (IbtInfo.Status == true)
                {
                    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    productInfo.Currentstock -= item.Qty;
                    await _DbContext.SaveChangesAsync();
                }
            }

            List<IbtDetails> allItems = await _DbContext.IbtDetails.Where(x => x.IbtId == id).ToListAsync();

            return allItems;
        }

    }
}
