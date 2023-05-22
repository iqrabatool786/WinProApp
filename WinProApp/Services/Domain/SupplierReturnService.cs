using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SupplierReturnService
    {
        private readonly WinProDbContext _DbContext;

        public SupplierReturnService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<SupplierReturn> GetByIdAsync(long id)
        {
            return await _DbContext.Set<SupplierReturn>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SupplierReturn>> GetByAllAsync()
        {
            return await _DbContext.Set<SupplierReturn>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<List<SupplierPurchase>> GetInvoiceByInvoiceIdAsync(long supplierId)
        {
            var info = from r in _DbContext.PurchaseReciept
                       from i in _DbContext.SupplierPurchase
                       where r.SupplierId == supplierId && i.Id == r.InvoiceId
                       select i;

            return await info.ToListAsync();
        }

        public async Task<SupplierReturn> CreateAsync(SupplierReturn model)
        {
            _DbContext.SupplierReturn.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<SupplierReturn> UpdateAsync(SupplierReturn model, List<SupplierReturnDetail> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var infoItem = await _DbContext.SupplierReturnDetail.FirstOrDefaultAsync(x => x.Id == itemId);
                    infoItem.ReturnId = model.Id;
                    infoItem.Barcode = item.Barcode;
                    infoItem.QtyDozen = item.QtyDozen;
                    infoItem.QtyPices = item.QtyPices;
                    infoItem.Price = item.Price;
                    infoItem.Vat = item.Vat;
                    await _DbContext.SaveChangesAsync();
                }
                if(itemId == 0)
                {
                    var infoItem = new SupplierReturnDetail();

                    infoItem.ReturnId = model.Id;
                    infoItem.Barcode = item.Barcode;
                    infoItem.QtyDozen = item.QtyDozen;
                    infoItem.QtyPices = item.QtyPices;
                    infoItem.Price = item.Price;
                    infoItem.Vat = item.Vat;

                    await _DbContext.SupplierReturnDetail.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

            }

            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            if (model.Status == true)
            {
                var supplierInfo = await _DbContext.Suppliers.FirstOrDefaultAsync(x => x.Id == model.SupplierId);
                supplierInfo.Balance -= model.Total;
                await _DbContext.SaveChangesAsync();
            }

            return model;
        }

        public Task<int> DeleteAsync(SupplierReturn model)
        {
            var items = _DbContext.SupplierReturnDetail.Where(x => x.Id == model.Id).ToList();
            foreach(var item in items)
            {
                _DbContext.SupplierReturnDetail.Remove(item);
                _DbContext.SaveChanges();
            }

            _DbContext.SupplierReturn.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.SupplierReturnDetail.FirstOrDefault(x => x.Id == id);
            _DbContext.SupplierReturnDetail.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetSupplierReturn>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetSupplierReturn> rows = null;
            await _DbContext.LoadStoredProc("sp-GetSupplierReturn")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSupplierReturn>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<SupplierReturnDetail>> GetItemsByReturnIdAsync(long id)
        {
            return await _DbContext.SupplierReturnDetail.Where(s => s.ReturnId == id).ToListAsync();
        }

        public async Task<List<SupplierReturnDetail>> CreateItemsAsync(long id, List<SupplierReturnDetail> items)
        {
            foreach (var item in items)
            {
                var infoItem = new SupplierReturnDetail();
                infoItem.ReturnId = id;
                infoItem.ProductId= item.ProductId;
                infoItem.Barcode = item.Barcode;
                infoItem.QtyDozen = item.QtyDozen;
                infoItem.QtyPices = item.QtyPices;
                infoItem.Price = item.Price;
                infoItem.Vat = item.Vat;

                await _DbContext.SupplierReturnDetail.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();
            }

            List<SupplierReturnDetail> allItems = await _DbContext.SupplierReturnDetail.Where(x => x.ReturnId == id).ToListAsync();

            return allItems;
        }
    }
}
