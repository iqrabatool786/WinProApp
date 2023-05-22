using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;
using WinProApp.ViewModels.PurchaseReciept;

namespace WinProApp.Services.Domain
{
    public class PurchaseRecieptService
    {
        private readonly WinProDbContext _DbContext;

        public PurchaseRecieptService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<PurchaseReciept> GetByIdAsync(long id)
        {
            return await _DbContext.Set<PurchaseReciept>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<PurchaseReciept>> GetByAllAsync()
        {
            return await _DbContext.Set<PurchaseReciept>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<PurchaseReciept> CreateAsync(PurchaseReciept model)
        {
            _DbContext.PurchaseReciept.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<PurchaseReciept> UpdateReceiptAsync(PurchaseReciept model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<PurchaseReciept> UpdateAsync(PurchaseReciept model, List<DetailsUpdateViewModel> items)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            Task.Delay(100);

            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    var infoItem = await _DbContext.PurchaseRecieptDetails.FirstOrDefaultAsync(x=>x.Id == itemId);
                    infoItem.ReceiveQtyDozen = item.ReceiveQtyDozen;
                    infoItem.ReceiveQtyPices = item.ReceiveQtyPices;
                    infoItem.ReceivePrice = item.ReceivePrice;
                    infoItem.CostPrice = item.CostPrice;
                    infoItem.SalePrice = item.SalePrice;
                    infoItem.BoxRetail = item.BoxRetail;
                    infoItem.Vat = item.Vat;
                    infoItem.PVat = item.PVat;
                   

                    if(model.Status == true)
                    {
                        var stockInfo = _DbContext.tblStock.FirstOrDefault(x=>x.PurchaseId== model.Id && x.BarCode == item.ItemBarcode);
                        if (stockInfo != null)
                        {
                            stockInfo.PurInQty = (((item.ReceiveQtyDozen ?? 0) * 12) + (item.ReceiveQtyPices ?? 0));
                          // await _DbContext.SaveChangesAsync();
                        }
                    }

                    await _DbContext.SaveChangesAsync();
                }

            }

            if (model.Status == true)
            {
                var supplierInfo = await _DbContext.Suppliers.FirstOrDefaultAsync(x => x.Id == model.SupplierId);
                supplierInfo.Balance += model.Total;
                await _DbContext.SaveChangesAsync();
            }

            return model;
        }

        public async Task<long> UpdateProductInfoAsync(long id, List<DetailsUpdateViewModel> items)
        {
           // var receipt= await _DbContext.PurchaseReciept.FirstOrDefaultAsync(x => x.Id == id);
            //var info = await _DbContext.PurchaseRecieptDetails.Where(x=>x.ReceiptId== id).ToListAsync();
            foreach(var item in items)
            {
                long itemId = item.Id;
                if (itemId > 0)
                {
                    decimal productpriceWithoutVat = ((item.SalePrice ?? 0) - (item.Vat ?? 0));
                   // if (receipt.Status == true)
                   // {
                        decimal totQty1 = 0;
                        decimal totQty = 0;
                        decimal qtyd1 = item.QtyDozen ?? 0;
                        decimal qtyp1 = item.QtyPices ?? 0;
                        decimal qtyd = item.ReceiveQtyDozen ?? 0;
                        decimal qtyp = item.ReceiveQtyPices ?? 0;


                        totQty1 = (qtyd1 * 12) + qtyp1;
                        totQty = (qtyd * 12) + qtyp;

                            var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                            long curStock = productInfo.Currentstock ?? 0;
                            decimal curCost = productInfo.CostPrice ?? 0;
                            decimal newPrice = item.CostPrice ?? 0;
                            decimal pricex = 0;

                            if (curStock > 0 && curCost != newPrice)
                            {
                                pricex = (((curStock * curCost) + (totQty * newPrice)) / (curStock + totQty));
                            }
                            else
                            {
                                pricex = item.CostPrice ?? 0;
                            }

                            productInfo.CostPrice = pricex;
                            productInfo.SalePrice = item.SalePrice;
                            productInfo.UnitCost = item.CostPrice;
                            productInfo.ProductInitialPrice = item.OrgPrice;
                            productInfo.OreginalPrice = item.OrgPrice;
                            productInfo.Currentstock = (curStock + long.Parse(totQty.ToString()));
                            productInfo.Vat = item.Vat;
                            await _DbContext.SaveChangesAsync();
                   // }
                }
            }

            return id;
        }

        public Task<int> DeleteAsync(PurchaseReciept model)
        {
            _DbContext.PurchaseReciept.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.PurchaseRecieptDetails.FirstOrDefault(x => x.Id == id);
            _DbContext.PurchaseRecieptDetails.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetPurchaseReciept>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetPurchaseReciept> rows = null;
            await _DbContext.LoadStoredProc("sp-GetPurchaseReciept")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetPurchaseReciept>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<PurchaseRecieptDetails>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.PurchaseRecieptDetails.Where(s => s.ReceiptId == id).ToListAsync();
        }

        public async Task<List<PurchaseRecieptDetails>> CreateItemsAsync(long id, List<PurchaseRecieptDetails> items)
        {
            foreach (var item in items)
            {
                var infoItem = new PurchaseRecieptDetails();
                infoItem.ReceiptId = id;
                infoItem.Barcode = item.Barcode;
                infoItem.CategoryId = item.CategoryId;
                infoItem.DepartmentId = item.DepartmentId;
                infoItem.SeassonId = item.SeassonId;
                infoItem.DescriptionId = item.DescriptionId;
                infoItem.SkuId = item.SkuId;
                infoItem.SizeId = item.SizeId;
                infoItem.ColorId = item.ColorId;
                infoItem.UnitId = item.UnitId;
                infoItem.BrandId = item.BrandId;
                infoItem.VendorId = item.VendorId;
                infoItem.GroupId = item.GroupId;
                infoItem.ReceiveQtyDozen = item.ReceiveQtyDozen;
                infoItem.ReceiveQtyPices = item.ReceiveQtyPices;
                infoItem.ReceivePrice = item.ReceivePrice;
                infoItem.CostPrice = item.CostPrice;
                infoItem.SalePrice = item.SalePrice;
                infoItem.BoxRetail = item.BoxRetail;
                infoItem.UnitDescription = item.UnitDescription;
                infoItem.Vat = item.Vat;
                infoItem.PVat = item.PVat;

                await _DbContext.PurchaseRecieptDetails.AddAsync(infoItem);
                await _DbContext.SaveChangesAsync();
            }

            List<PurchaseRecieptDetails> allItems = await _DbContext.PurchaseRecieptDetails.Where(x => x.ReceiptId == id).ToListAsync();

            return allItems;
        }
    }
}
