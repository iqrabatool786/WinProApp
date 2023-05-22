using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class PurchaseAssetService
    {
        private readonly WinProDbContext _DbContext;

        public PurchaseAssetService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<SupplierPurchaseAsset> GetByIdAsync(long id)
        {
            return await _DbContext.Set<SupplierPurchaseAsset>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<long> CreateAsync(SupplierPurchaseAsset model)
        {
            try
            {
                await _DbContext.SupplierPurchaseAsset.AddAsync(model);
                _DbContext.SaveChanges();

                if (model.Status == true)
                {
                    var supplierInfo = _DbContext.Suppliers.FirstOrDefault(x => x.Id == model.SupplierId);
                    supplierInfo.Balance += model.Total;
                    _DbContext.SaveChanges();
                }

                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<SupplierPurchaseAsset> UpdateAsync(SupplierPurchaseAsset model, List<SupplierPurchaseAssetDetails> items)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();

            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId == 0)
                {
                    var infoItem = new SupplierPurchaseAssetDetails();
                    infoItem.PurchaseId = item.PurchaseId;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Vat = item.Vat;
                    infoItem.ExpireDate = item.ExpireDate;
                    infoItem.ProductDate = item.ProductDate;

                    _DbContext.SupplierPurchaseAssetDetails.Add(infoItem);
                    await _DbContext.SaveChangesAsync();
                }
                else
                {
                    var infoItem = await _DbContext.SupplierPurchaseAssetDetails.FirstOrDefaultAsync(x => x.Id == itemId);
                    //  infoItem.Id = itemId;
                    infoItem.PurchaseId = item.PurchaseId;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.Qty = item.Qty;
                    infoItem.Price = item.Price;
                    infoItem.Vat = item.Vat;
                    infoItem.ExpireDate = item.ExpireDate;
                    infoItem.ProductDate = item.ProductDate;

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

            }

            if(model.Status ==true)
            {
                var supplierInfo = _DbContext.Suppliers.FirstOrDefault(x => x.Id == model.SupplierId);
                supplierInfo.Balance += model.Total;
                _DbContext.SaveChanges();
            }
            return model;
        }


        public Task<int> DeleteAsync(SupplierPurchaseAsset model)
        {
            _DbContext.SupplierPurchaseAsset.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.SupplierPurchaseAssetDetails.FirstOrDefault(x => x.Id == id);
            _DbContext.SupplierPurchaseAssetDetails.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetSupplierPurchaseAsset>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetSupplierPurchaseAsset> rows = null;
            await _DbContext.LoadStoredProc("sp-GetSupplierPurchaseAsset")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSupplierPurchaseAsset>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<SupplierPurchaseAssetDetails>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.SupplierPurchaseAssetDetails.Where(s => s.PurchaseId == id).ToListAsync();
        }

        public async Task<List<SupplierPurchaseAssetDetails>> CreateItemsAsync(long id, List<SupplierPurchaseAssetDetails> items)
        {
            foreach (var item in items)
            {
                var infoItem = new SupplierPurchaseAssetDetails();
                infoItem.PurchaseId = item.PurchaseId;
                infoItem.ProductId = item.ProductId;
                infoItem.Barcode = item.Barcode;
                infoItem.DescriptionEng = item.DescriptionEng;
                infoItem.DescriptionArabic = item.DescriptionArabic;
                infoItem.Qty = item.Qty;
                infoItem.Price = item.Price;
                infoItem.Vat = item.Vat;
                infoItem.ExpireDate = item.ExpireDate;
                infoItem.ProductDate = item.ProductDate;                
                try
                {
                    _DbContext.SupplierPurchaseAssetDetails.Add(infoItem);
                    await _DbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            List<SupplierPurchaseAssetDetails> allItems = await _DbContext.SupplierPurchaseAssetDetails.Where(x => x.PurchaseId == id).ToListAsync();

            return allItems;
        }
    }
}
