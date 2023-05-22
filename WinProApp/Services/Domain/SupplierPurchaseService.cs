using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System.Transactions;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SupplierPurchaseService
    {
        private readonly WinProDbContext _DbContext;
        public readonly CommonService _commonService;
        public SupplierPurchaseService(WinProDbContext DbContext, CommonService commonService)
        {
            _DbContext = DbContext;
            _commonService = commonService;
        }

        public async Task<SupplierPurchase> GetByIdAsync(long id)
        {
            return await _DbContext.Set<SupplierPurchase>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SupplierPurchase>> GetByAllAsync()
        {
            return await _DbContext.Set<SupplierPurchase>().OrderByDescending(x=>x.Id).ToListAsync();
        }

        public async Task<bool> IsInvoiceNoWithSupplierExists(string InvoiceNo, long SupplierId, long Id)
        {
            var exists = await _DbContext.Set<SupplierPurchase>().Where(s => s.InvoiceNo == InvoiceNo && s.SupplierId == SupplierId).ToListAsync();
            if(Id > 0 && exists != null && exists.Count > 0)
            {
                if (exists.FirstOrDefault(x => x.Id == Id) != null)
                {
                    return true;
                }
                return false;
            }
            return exists != null && exists.Count > 0 ? false : true;
        }

        public async Task<long> CreateAsync(SupplierPurchase model)
        {
            try
            {
               await _DbContext.SupplierPurchase.AddAsync(model);
               _DbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public async Task<SupplierPurchase> UpdateAsync(SupplierPurchase model, List<SupplierPurchaseDetails> items)
        {
            foreach (var item in items)
            {
                long itemId = item.Id;
                if (itemId == 0)
                {
                    var infoItem = new SupplierPurchaseDetails();
                    infoItem.SupplierPurchaseId = item.SupplierPurchaseId;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.CategoryId=item.CategoryId;
                    infoItem.DepartmentId = item.DepartmentId;
                    infoItem.SeassonId = item.SeassonId;
                    infoItem.DescriptionId = item.DescriptionId;
                    infoItem.DescriptionEng = item.DescriptionEng;
                    infoItem.DescriptionArabic = item.DescriptionArabic;
                    infoItem.SkuId = item.SkuId;
                    infoItem.SizeId = item.SizeId;
                    infoItem.ColorId = item.ColorId;
                    infoItem.UnitId = item.UnitId;
                    infoItem.BrandId= item.BrandId;
                    infoItem.VendorId= item.VendorId;
                    infoItem.GroupId= item.GroupId;
                    infoItem.YearId= item.YearId;
                    infoItem.QtyDozen = item.QtyDozen;
                    infoItem.QtyPices = item.QtyPices;
                    infoItem.Price = item.Price;
                    infoItem.ExpireDate = item.ExpireDate;
                    infoItem.ProductDate = item.ProductDate;
                    infoItem.FlagPricePerPices = item.FlagPricePerPices;
                    infoItem.UnitBarcode = item.UnitBarcode;
                    infoItem.QtyPerUnit = item.QtyPerUnit;
                    infoItem.UnitCost = item.UnitCost;
                    infoItem.UnitSalePrice = item.UnitSalePrice;
                    infoItem.BoxRetail = item.BoxRetail;
                    infoItem.UnitDescription = item.UnitDescription;
                    infoItem.Vat = item.Vat;
                    infoItem.PVat = item.PVat;
                    
                    try
                    {
                        //_DbContext.SupplierPurchaseDetails.Add(infoItem);
                        _DbContext.Set<SupplierPurchaseDetails>().Add(infoItem);
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                
                }
                else
                {
                    var infoItem = await _DbContext.SupplierPurchaseDetails.FirstOrDefaultAsync(x=>x.Id== itemId);
                    infoItem.Id = itemId;
                    infoItem.SupplierPurchaseId = item.SupplierPurchaseId;
                    infoItem.ProductId = item.ProductId;
                    infoItem.Barcode = item.Barcode;
                    infoItem.DescriptionEng= item.DescriptionEng;
                    infoItem.DescriptionArabic= item.DescriptionArabic;
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
                    infoItem.YearId = item.YearId;
                    infoItem.QtyDozen = item.QtyDozen;
                    infoItem.QtyPices = item.QtyPices;
                    infoItem.Price = item.Price;
                    infoItem.ExpireDate = item.ExpireDate;
                    infoItem.ProductDate = item.ProductDate;
                    infoItem.FlagPricePerPices = item.FlagPricePerPices;
                    infoItem.UnitBarcode = item.UnitBarcode;
                    infoItem.QtyPerUnit = item.QtyPerUnit;
                    infoItem.UnitCost = item.UnitCost;
                    infoItem.UnitSalePrice = item.UnitSalePrice;
                    infoItem.BoxRetail = item.BoxRetail;
                    infoItem.UnitDescription = item.UnitDescription;
                    infoItem.Vat = item.Vat;
                    infoItem.PVat = item.PVat;

                    try
                    {
                        _DbContext.Update(infoItem);
                        //await _DbContext.SaveChangesAsync();
                        //Task.Delay(50);
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
            await _DbContext.SaveChangesAsync();
            Task.Delay(50);
            try
            {
                _DbContext.Update(model);
                await _DbContext.SaveChangesAsync();
                Task.Delay(100);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return model;
        }

        public async Task<long> ConfirmInvoiceAsync(long id, string curUser)
        {
            var invoice = await _DbContext.SupplierPurchase.FirstOrDefaultAsync(x => x.Id == id);
            invoice.Status = true;
            invoice.CratedDate = DateTime.Now;
            invoice.CreatedBy = curUser;
            invoice.UpdatedDate = DateTime.Now;
            invoice.UpdatedBy = curUser;
            await _DbContext.SaveChangesAsync();

            return id;
        }

        public Task<int> DeleteAsync(SupplierPurchase model)
        {
            _DbContext.SupplierPurchase.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(long id)
        {
            var item = _DbContext.SupplierPurchaseDetails.FirstOrDefault(x => x.Id == id);
            if(item != null)
            {
                var allDetails = _DbContext.SupplierPurchaseDetails.Where(x => x.SupplierPurchaseId == item.SupplierPurchaseId && x.Id != id).ToList();
                decimal? totall = Convert.ToDecimal(0);
                if (allDetails.Count > 0)
                {
                    totall = allDetails.Sum(x => (((x.QtyDozen * 12) + (x.QtyPices)) * x.UnitCost));
                }
                var purchaseInvoice = _DbContext.SupplierPurchase.FirstOrDefault(x => x.Id == item.SupplierPurchaseId);
                if(purchaseInvoice != null)
                {
                    var VatTotal = totall * (((await _commonService.GetVatAsync()).Percentage) / 100);
                    purchaseInvoice.Total = totall + VatTotal;
                    purchaseInvoice.VatAmount = VatTotal;
                    _DbContext.Update(purchaseInvoice);
                }
                _DbContext.SupplierPurchaseDetails.Remove(item);
            }
            
            await _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetSupplierPurchase>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetSupplierPurchase> rows = null;
            await _DbContext.LoadStoredProc("sp-GetSupplierPurchase")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSupplierPurchase>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<SupplierPurchaseDetails>> GetItemsByRequestIdAsync(long id)
        {
            return await _DbContext.SupplierPurchaseDetails.Where(s => s.SupplierPurchaseId == id).ToListAsync();
        }

        public async Task<List<SupplierPurchaseDetails>> CreateItemsAsync(long id, IList<SupplierPurchaseDetails> items)
        {
            //List<SupplierPurchaseDetails> details = new List<SupplierPurchaseDetails>();
            //foreach (var item in items)
            //{
            //    var infoItem = new SupplierPurchaseDetails();
            //    infoItem.SupplierPurchaseId = item.SupplierPurchaseId;
            //    infoItem.ProductId = item.ProductId;
            //    infoItem.Barcode = item.Barcode;
            //    infoItem.CategoryId = item.CategoryId;
            //    infoItem.DepartmentId = item.DepartmentId;
            //    infoItem.SeassonId = item.SeassonId;
            //    infoItem.DescriptionId = item.DescriptionId;
            //    infoItem.SkuId = item.SkuId;
            //    infoItem.SizeId = item.SizeId;
            //    infoItem.ColorId = item.ColorId;
            //    infoItem.UnitId = item.UnitId;
            //    infoItem.BrandId = item.BrandId;
            //    infoItem.VendorId = item.VendorId;
            //    infoItem.GroupId = item.GroupId;
            //    infoItem.YearId = item.YearId;
            //    infoItem.DescriptionEng = item.DescriptionEng;
            //    infoItem.DescriptionArabic = item.DescriptionArabic;
            //    infoItem.QtyDozen = item.QtyDozen;
            //    infoItem.QtyPices = item.QtyPices;
            //    infoItem.Price = item.Price;
            //    infoItem.ExpireDate = item.ExpireDate;
            //    infoItem.ProductDate = item.ProductDate;
            //    infoItem.FlagPricePerPices = item.FlagPricePerPices;
            //    infoItem.UnitBarcode = item.UnitBarcode;
            //    infoItem.QtyPerUnit = item.QtyPerUnit;
            //    infoItem.UnitCost = item.UnitCost;
            //    infoItem.UnitSalePrice = item.UnitSalePrice;
            //    infoItem.BoxRetail = item.BoxRetail;
            //    infoItem.UnitDescription = item.UnitDescription;
            //    infoItem.Vat = item.Vat;
            //    infoItem.PVat = item.PVat;

            //    try
            //    {
            //        _DbContext.Set<SupplierPurchaseDetails>().Add(infoItem);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex.InnerException;
            //    }
            //}

           await _DbContext.BulkInsertAsync(items, options => options.BatchSize = 100);

           // await _DbContext.SaveChangesAsync();
            Task.Delay(50);
            List<SupplierPurchaseDetails> allItems = await _DbContext.SupplierPurchaseDetails.Where(x => x.SupplierPurchaseId == id).ToListAsync();

            return allItems;
        }
    }
}
