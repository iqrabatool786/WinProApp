using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;
namespace WinProApp.Services.Domain
{
    public class ProFormaInvoiceService
    {
        private readonly WinProDbContext _DbContext;

        public ProFormaInvoiceService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<ProFormaInvoice> GetByIdAsync(long id)
        {
            return await _DbContext.Set<ProFormaInvoice>().FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<ProFormaInvoice> CreateAsync(ProFormaInvoice model)
        {
            _DbContext.ProFormaInvoice.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<ProFormaInvoice> UpdateAsync(ProFormaInvoice model, List<ProFormaInvoiceItems> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId == 0)
                {
                    var infoItem = new ProFormaInvoiceItems();
                    infoItem.Pfid = model.Id;
                    infoItem.PartNo = item.PartNo;
                    infoItem.Description = item.Description;
                    infoItem.Unit = item.Unit;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Tax = item.Tax;
                    infoItem.Total = item.Total;

                    await _DbContext.ProFormaInvoiceItems.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

            }

            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public Task<int> DeleteAsync(ProFormaInvoice model)
        {
            _DbContext.ProFormaInvoice.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.ProFormaInvoiceItems.FirstOrDefault(x => x.Id == id);
            _DbContext.ProFormaInvoiceItems.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetProFormaInvoice>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetProFormaInvoice> rows = null;
            await _DbContext.LoadStoredProc("sp-GetProFormaInvoice")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetProFormaInvoice>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<ProFormaInvoiceItems>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.ProFormaInvoiceItems.Where(s => s.Pfid == id).ToListAsync();
        }

        public async Task<List<ProFormaInvoiceItems>> CreateItemsAsync(long id, List<ProFormaInvoiceItems> items)
        {
            foreach (var item in items)
            {
                var infoItem = new ProFormaInvoiceItems();
                infoItem.Pfid = id;
                infoItem.PartNo = item.PartNo;
                infoItem.Description = item.Description;
                infoItem.Unit = item.Unit;
                infoItem.Qty = item.Qty;
                infoItem.Price = item.Price;
                infoItem.Tax = item.Tax;
                infoItem.Total = item.Total;

                await _DbContext.ProFormaInvoiceItems.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();
            }

            List<ProFormaInvoiceItems> allItems = await _DbContext.ProFormaInvoiceItems.Where(x => x.Pfid == id).ToListAsync();

            return allItems;
        }
    }
}
