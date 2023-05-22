using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class ProductInfoService
    {
        private readonly WinProDbContext _DbContext;

        public ProductInfoService(WinProDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<ProductInfo> GetByIdAsync(long id)
        {
            return await _DbContext.Set<ProductInfo>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<ProductInfo>> GetByAllAsync()
        {
            return await _DbContext.Set<ProductInfo>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<ProductInfo> CreateAsync(ProductInfo model)
        {
            _DbContext.ProductInfo.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<ProductInfo> UpdateAsync(ProductInfo model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> DeleteAsync(ProductInfo model)
        {
            _DbContext.ProductInfo.Remove(model);
            return _DbContext.SaveChangesAsync();
        }

        public async Task<List<GetProductInfo>> GetList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetProductInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetProductInfo")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetProductInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        public async Task<List<GetProductInfo>> GetFilteredList(JQueryDataTableParamModel param, int categoryId, int vendorId)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetProductInfo> rows = null;
            await _DbContext.LoadStoredProc("sp-GetProductInfoWithFilter")
                  .AddParam("CategoryId", categoryId)
                  .AddParam("VendorId", vendorId)
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetProductInfo>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

        //-- Product Price
        public async Task<ProductPrice> GetProductPriceByIdAsync(long id)
        {
            return await _DbContext.Set<ProductPrice>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<long> GetProductPriceCountByProductIdAsync(long id)
        {
            long count = await _DbContext.Set<ProductPrice>().Where(x=>x.ProductId== id).CountAsync();
            return count;
        }

        public async Task<ProductPrice> GetProductPriceByProductIdAsync(long id)
        {
            return await _DbContext.Set<ProductPrice>().FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<ProductPrice> ProductPriceCreateAsync(ProductPrice model)
        {
            _DbContext.ProductPrice.Add(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public async Task<ProductPrice> ProductPriceUpdateAsync(ProductPrice model)
        {
            _DbContext.Update(model);
            await _DbContext.SaveChangesAsync();
            return model;
        }

        public Task<int> ProductPriceDeleteAsync(ProductPrice model)
        {
            _DbContext.ProductPrice.Remove(model);
            return _DbContext.SaveChangesAsync();
        }
    }
}
