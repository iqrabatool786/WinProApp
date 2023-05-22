using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Services.Domain
{
    public class ShippingService
    {
        private readonly WinProDbContext _DbContext;

        public ShippingService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<ShippingInfo> GetByIdAsync(long id)
        {
            return await _DbContext.Set<ShippingInfo>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<ShippingInfo>> GetByAllAsync()
        {
            return await _DbContext.Set<ShippingInfo>().OrderByDescending(s => s.Id).ToListAsync();
        }

        public async Task<List<ShippingInfo>> GetPendingShipmentAsync()
        {
            return await _DbContext.Set<ShippingInfo>().Where(s => s.Status == false).ToListAsync();
        }

        public async Task<long> UpdateStatusToOnHoldAsync(long id)
        {
            var info = await _DbContext.ShippingInfo.FirstOrDefaultAsync(x => x.Id == id);
            info.Status = false;
            info.UpdatedDate= DateTime.UtcNow;
            await _DbContext.SaveChangesAsync();

            return id;
        }

        public async Task<long> CreateAsync(ShippingInfo model)
        {
            try
            {
                await _DbContext.ShippingInfo.AddAsync(model);
                _DbContext.SaveChanges();

                if (model.Status == true)
                {
                    var supplierInfo = await _DbContext.Suppliers.FirstOrDefaultAsync(x => x.Id == model.SupplierId);
                    supplierInfo.Balance += model.Total;
                    await _DbContext.SaveChangesAsync();
                }

                return model.Id;

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<ShippingInfo> UpdateAsync(ShippingInfo model, List<ShippingDetails> items)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            Task.Delay(100);

            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId == 0)
                {
                    var infoItem = new ShippingDetails();
                    infoItem.ShippingId = item.ShippingId;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.DescriptionEng= item.DescriptionEng;
                    infoItem.DescriptionArabic= item.DescriptionArabic;
                    infoItem.CategoryId = item.CategoryId;
                    infoItem.DepartmentId = item.DepartmentId;
                    infoItem.SeassonId = item.SeassonId;
                    infoItem.SkuId = item.SkuId;
                    infoItem.SizeId = item.SizeId;
                    infoItem.ColorId = item.ColorId;
                    infoItem.UnitId = item.UnitId;
                    infoItem.BrandId = item.BrandId;
                    infoItem.VendorId = item.VendorId;
                    infoItem.YearId = item.YearId;
                    infoItem.GroupId = item.GroupId;
                    infoItem.BoxNo = item.BoxNo;
                    infoItem.QtyPerBox = item.QtyPerBox;
                    infoItem.Qty= item.Qty;
                    infoItem.ReceivedQty = item.ReceivedQty;
                    infoItem.Price = item.Price;
                    infoItem.SalePrice = item.SalePrice;
                    infoItem.SaleVat = item.SaleVat;
                    infoItem.OriginalPrice = item.OriginalPrice;
                    infoItem.ImageUrl = item.ImageUrl;

                    _DbContext.ShippingDetails.Add(infoItem);
                    await _DbContext.SaveChangesAsync();

                    //if (model.ReceivedStatus == true)
                    //{
                    //    int? qty = item.Qty ?? 0;

                    //    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    //    if (item.Price != null)
                    //    {
                    //        productInfo.CostPrice = item.Price;
                    //        productInfo.UnitCost = item.Price;
                    //    }
                    //    if (item.SalePrice != null)
                    //    {
                    //        productInfo.SalePrice = item.SalePrice;
                    //    }
                    //    if (item.SaleVat != null)
                    //    {
                    //        productInfo.ProductInitialPrice = ((item.SalePrice ?? 0) - (item.SaleVat ?? 0));
                    //        productInfo.Vat = item.SaleVat;
                    //    }
                    //    productInfo.Currentstock += qty;
                    //    await _DbContext.SaveChangesAsync();
                    //}
                }
                else
                {
                    var infoItem = await _DbContext.ShippingDetails.FirstOrDefaultAsync(x => x.Id == itemId);
                    //  infoItem.Id = itemId;
                    infoItem.ShippingId = item.ShippingId;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.DescriptionEng = item.DescriptionEng;
                    infoItem.DescriptionArabic = item.DescriptionArabic;
                    infoItem.CategoryId = item.CategoryId;
                    infoItem.DepartmentId = item.DepartmentId;
                    infoItem.SeassonId = item.SeassonId;
                    infoItem.SkuId = item.SkuId;
                    infoItem.SizeId = item.SizeId;
                    infoItem.ColorId = item.ColorId;
                    infoItem.UnitId = item.UnitId;
                    infoItem.BrandId = item.BrandId;
                    infoItem.VendorId = item.VendorId;
                    infoItem.YearId = item.YearId;
                    infoItem.GroupId = item.GroupId;
                    infoItem.BoxNo = item.BoxNo;
                    infoItem.QtyPerBox = item.QtyPerBox;
                    infoItem.Qty = item.Qty;
                    infoItem.ReceivedQty = item.ReceivedQty;
                    infoItem.Price = item.Price;
                    infoItem.SalePrice = item.SalePrice;
                    infoItem.SaleVat = item.SaleVat;
                    infoItem.OriginalPrice = item.OriginalPrice;
                    infoItem.ImageUrl = item.ImageUrl;

                    try
                    {
                        _DbContext.Update(infoItem);
                        await _DbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }

                    //if (model.ReceivedStatus == true)
                    //{
                    //    int? qty = item.Qty ?? 0;

                    //    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    //    if (item.Price != null)
                    //    {
                    //        productInfo.CostPrice = item.Price;
                    //        productInfo.UnitCost = item.Price;
                    //    }
                    //    if(item.SalePrice !=null)
                    //    {
                    //        productInfo.SalePrice = item.SalePrice;
                    //    }
                    //    if(item.SaleVat != null)
                    //    {
                    //        productInfo.ProductInitialPrice = ((item.SalePrice ?? 0) - (item.SaleVat ?? 0));
                    //        productInfo.Vat = item.SaleVat;
                    //    }
                    //    productInfo.Currentstock += qty;
                        
                    //    await _DbContext.SaveChangesAsync();
                    //}
                }

            }

            if (model.ReceivedStatus == true)
            {
                var supplierInfo = await _DbContext.Suppliers.FirstOrDefaultAsync(x => x.Id == model.SupplierId);
                supplierInfo.Balance += model.Total;
                await _DbContext.SaveChangesAsync();
            }

            return model;
        }

        public async Task<long> UpdateShipmentTotalAsync(long id, decimal? discount, decimal? charges, decimal? total, string curUser)
        {
            var invoice = await _DbContext.ShippingInfo.FirstOrDefaultAsync(x => x.Id == id);
            invoice.Discount += discount??0;
            invoice.OtherCharges += charges??0;
            invoice.Total += total ?? 0;
            invoice.CratedDate = DateTime.Now;
            invoice.CreatedBy = curUser;
            invoice.UpdatedDate = DateTime.Now;
            invoice.UpdatedBy = curUser;
            await _DbContext.SaveChangesAsync();

            return id;
        }

        public async Task<long> ConfirmInvoiceAsync(long id, string curUser)
        {
            var invoice = await _DbContext.ShippingInfo.FirstOrDefaultAsync(x => x.Id == id);
            invoice.Status = true;
            invoice.CratedDate = DateTime.Now;
            invoice.CreatedBy = curUser;
            invoice.UpdatedDate = DateTime.Now;
            invoice.UpdatedBy = curUser;
            await _DbContext.SaveChangesAsync();

            return id;
        }

        public Task<int> DeleteAsync(ShippingInfo model)
        {
            _DbContext.ShippingInfo.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public Task DeleteItemAsync(long id)
        {
            var item = _DbContext.ShippingDetails.FirstOrDefault(x => x.Id == id);
            _DbContext.ShippingDetails.Remove(item);

            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetShippingInfo>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetShippingInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetShippingInfo")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetShippingInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<ShippingDetails>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.ShippingDetails.Where(s => s.ShippingId == id).ToListAsync();
        }

        public async Task<List<ShippingInfo>> GetSelectedShippingInfoByAsync(string type)
        {
            if (type == "pending")
            {
                return await _DbContext.ShippingInfo.Where(s => s.Status == true && s.ReceivedStatus==false).ToListAsync();
            }
            else
            {
                if (type == "custom")
                {
                    return await _DbContext.ShippingInfo.Where(s => s.ReceivedStatus == false).ToListAsync();
                }
                else
                {
                    return await _DbContext.ShippingInfo.Where(s => s.Status == false).ToListAsync();
                }                
            }
            
        }

        public async Task<List<ShippingDetails>> CreateItemsAsync(long id, List<ShippingDetails> items)
        {
            var shippingInfo = await _DbContext.ShippingInfo.FirstOrDefaultAsync(s => s.Id == id);    
            foreach (var item in items)
            {
                var infoItem = new ShippingDetails();
                infoItem.ShippingId = item.ShippingId;
                infoItem.ProductId = item.ProductId;
                infoItem.Barcode = item.Barcode;
                infoItem.CategoryId = item.CategoryId;
                infoItem.DepartmentId = item.DepartmentId;
                infoItem.SeassonId = item.SeassonId;
                infoItem.SkuId = item.SkuId;
                infoItem.SizeId = item.SizeId;
                infoItem.ColorId = item.ColorId;
                infoItem.UnitId = item.UnitId;
                infoItem.BrandId = item.BrandId;
                infoItem.VendorId = item.VendorId;
                infoItem.YearId = item.YearId;
                infoItem.GroupId = item.GroupId;
                infoItem.DescriptionEng = item.DescriptionEng;
                infoItem.DescriptionArabic = item.DescriptionArabic;
                infoItem.BoxNo = item.BoxNo;
                infoItem.QtyPerBox = item.QtyPerBox;
                infoItem.Qty = item.Qty;
                infoItem.ReceivedQty= item.ReceivedQty;
                infoItem.Price = item.Price;
                infoItem.SalePrice= item.SalePrice;
                infoItem.SaleVat= item.SaleVat;
                infoItem.OriginalPrice= item.OriginalPrice;
                infoItem.ImageUrl = item.ImageUrl;
                try
                {
                    _DbContext.ShippingDetails.Add(infoItem);
                    await _DbContext.SaveChangesAsync();

                    //if (shippingInfo.Status == true)
                    //{
                    //    int? qty = item.Qty ?? 0;

                    //    var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                    //    if (item.Price != null)
                    //    {
                    //        productInfo.CostPrice = item.Price;
                    //        productInfo.UnitCost = item.Price;
                    //    }
                    //    if (item.SalePrice != null)
                    //    {
                    //        productInfo.SalePrice = item.SalePrice;
                    //    }
                    //    if (item.SaleVat != null)
                    //    {
                    //        productInfo.ProductInitialPrice = ((item.SalePrice ?? 0) - (item.SaleVat ?? 0));
                    //        productInfo.Vat = item.SaleVat;
                    //    }
                    //    productInfo.Currentstock += qty;
                    //    await _DbContext.SaveChangesAsync();
                    //}
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            List<ShippingDetails> allItems = await _DbContext.ShippingDetails.Where(x => x.ShippingId == id).ToListAsync();

            return allItems;
        }
    }
}
