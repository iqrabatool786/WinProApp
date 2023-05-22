using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class DamageReturnService
    {
        private readonly WinProDbContext _DbContext;

        public DamageReturnService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<DamageReturn> GetByIdAsync(long id)
        {
            return await _DbContext.Set<DamageReturn>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<DamageReturn>> GetByAllAsync()
        {
            return await _DbContext.Set<DamageReturn>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<List<SupplierPurchase>> GetInvoiceByInvoiceIdAsync(long supplierId)
        {
            var info = from r in _DbContext.PurchaseReciept
                       from i in _DbContext.SupplierPurchase
                       where r.SupplierId == supplierId && i.Id == r.InvoiceId
                       select i;

            return await info.ToListAsync();
        }

        public async Task<DamageReturn> CreateAsync(DamageReturn model)
        {
            _DbContext.DamageReturn.Add(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public async Task<DamageReturn> UpdateAsync(DamageReturn model, List<DamageReturnDetail> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;

                if (itemId > 0)
                {
                    var infoItem = await _DbContext.DamageReturnDetail.FirstOrDefaultAsync(x => x.Id == itemId);
                    infoItem.DamageReturnId = model.Id;
                    infoItem.Barcode = item.Barcode;
                    infoItem.QtyDozen = item.QtyDozen;
                    infoItem.QtyPices = item.QtyPices;
                    infoItem.Price = item.Price;
                   // infoItem.Vat = item.Vat;
                    await _DbContext.SaveChangesAsync();
                }
                if (itemId == 0)
                {
                    var infoItem = new DamageReturnDetail();

                    infoItem.DamageReturnId = model.Id;
                    infoItem.Barcode = item.Barcode;
                    infoItem.QtyDozen = item.QtyDozen;
                    infoItem.QtyPices = item.QtyPices;
                    infoItem.Price = item.Price;
                  //  infoItem.Vat = item.Vat;

                    await _DbContext.DamageReturnDetail.AddAsync(infoItem);
                    await _DbContext.SaveChangesAsync();
                }

                //if(model.Status == true)
                //{
                //    decimal totQty = 0;
                //    decimal qtyd = item.QtyDozen ?? 0;
                //    decimal qtyp = item.QtyPices ?? 0;
                //    if (qtyd > 0 && qtyp > 0)
                //    {
                //        totQty = (qtyd * 12) + qtyp;
                //    }
                //    else
                //    {
                //        if (qtyd > 0)
                //        {
                //            totQty = (qtyd * 12);
                //        }
                //        else
                //        {
                //            totQty = qtyp;
                //        }
                //    }

                //    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                //    productInfo.Currentstock -= long.Parse(totQty.ToString());
                //    await _DbContext.SaveChangesAsync();
                //}

            }

            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            return model;
        }

        public Task<int> DeleteAsync(DamageReturn model)
        {
            var items = _DbContext.DamageReturnDetail.Where(x => x.Id == model.Id).ToList();
            foreach (var item in items)
            {
                _DbContext.DamageReturnDetail.Remove(item);
                _DbContext.SaveChanges();
            }

            _DbContext.DamageReturn.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.DamageReturnDetail.FirstOrDefault(x => x.Id == id);
            _DbContext.DamageReturnDetail.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetDamageReturn>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetDamageReturn> rows = null;
            await _DbContext.LoadStoredProc("sp-GetDamageReturn")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetDamageReturn>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<DamageReturnDetail>> GetItemsByReturnIdAsync(long id)
        {
            return await _DbContext.DamageReturnDetail.Where(s => s.DamageReturnId == id).ToListAsync();
        }

        public async Task<List<DamageReturnDetail>> CreateItemsAsync(long id, List<DamageReturnDetail> items)
        {
            var info = await _DbContext.DamageReturn.FirstOrDefaultAsync(x=>x.Id== id);
            
            foreach (var item in items)
            {
                var infoItem = new DamageReturnDetail();
                infoItem.DamageReturnId = id;
                infoItem.ProductId = item.ProductId;
                infoItem.Barcode = item.Barcode;
                infoItem.QtyDozen = item.QtyDozen;
                infoItem.QtyPices = item.QtyPices;
                infoItem.Price = item.Price;
             //   infoItem.Vat = item.Vat;

                await _DbContext.DamageReturnDetail.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();

                //if (info.Status == true)
                //{
                //    decimal totQty = 0;
                //    decimal qtyd = item.QtyDozen ?? 0;
                //    decimal qtyp = item.QtyPices ?? 0;
                //    if (qtyd > 0 && qtyp > 0)
                //    {
                //        totQty = (qtyd * 12) + qtyp;
                //    }
                //    else
                //    {
                //        if (qtyd > 0)
                //        {
                //            totQty = (qtyd * 12);
                //        }
                //        else
                //        {
                //            totQty = qtyp;
                //        }
                //    }

                //    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                //    productInfo.Currentstock -= long.Parse(totQty.ToString());
                //    await _DbContext.SaveChangesAsync();
                //}
            }

            List<DamageReturnDetail> allItems = await _DbContext.DamageReturnDetail.Where(x => x.DamageReturnId == id).ToListAsync();

            return allItems;
        }
    }
}
