using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;

namespace WinProApp.Services.Domain
{
    public class SalesService
    {
        private readonly WinProDbContext _Context;

        public SalesService(WinProDbContext Context)
        {
            _Context = Context;
        }

        public async Task<bool> IsCrNoExist(string CrNo, long Id = 0)
        {
            bool response;
            var exists = await _Context.Set<Supplier>().FirstOrDefaultAsync(s => s.CRNumber == CrNo);
            response = exists != null ? true : false;
            if (Id > 0 && exists != null && Id == exists.Id)
            {
                response = false;
            }
            return response;
        }

        public async Task<bool> IsVatNoExist(string VatNo)
        {
            var exists = await _Context.Set<Supplier>().FirstOrDefaultAsync(s => s.VatNo == VatNo);
            return exists != null ? true : false;
        }
        public async Task<Supplier> GetSupplierByIdAsync(long id)
        {
            return await _Context.Set<Supplier>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<long> GetInvoiceCountBySupplierIdAsync(long id)
        {
            long count = _Context.SupplierPurchase.Where(s => s.SupplierId == id).Count();
            return count;
        }

        public async Task<long> GetShipmentCountBySupplierIdAsync(long id)
        {
            long count = _Context.ShippingInfo.Where(s => s.SupplierId == id).Count();
            return count;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _Context.Set<Supplier>().OrderBy(s => s.CompanyName).ToListAsync();
        }

        public async Task<List<Supplier>> GetAllSuppliersWithNamesAndIdsAsync()
        {
            var listSup = new List<Supplier>();

            var allSuppliers = await _Context.Set<Supplier>().OrderBy(s => s.CompanyName).ToListAsync();
            foreach (var supplier in allSuppliers)
            {
                listSup.Add(new Supplier()
                {
                    Id = supplier.Id,
                    CompanyName = supplier.Id + " - " + supplier.CompanyName
                });
            }
            return listSup;
        }

        public Task CreateAsync(Supplier supplier)
        {
            _Context.Suppliers.Add(supplier);
            return _Context.SaveChangesAsync();
        }

        public Task UpdateAsync(Supplier supplier)
        {
            _Context.Update(supplier);
            return _Context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Supplier supplier)
        {
            _Context.Suppliers.Remove(supplier);
            return _Context.SaveChangesAsync();
        }

        public async Task<List<GetSupliers>> GetSupplierList(JQueryDataTableParamModel param)
        {
            string strSearch = param.sSearch == null ? " " : param.sSearch;
            int start = param.iDisplayStart > 0 ? (param.iDisplayLength > 0 ? ((param.iDisplayStart / param.iDisplayLength) + 1) : 1) : 1;
            int recordsPerPage = param.iDisplayLength > 0 ? param.iDisplayLength : 20;
            string strSort = param.sSortDir_0 != null ? param.sSortDir_0 : "desc";

            List<GetSupliers> rows = null;
            await _Context.LoadStoredProc("sp-GetAllSupliers")
                  .AddParam("Search", strSearch)
                  .AddParam("PageIndex", start)
                  .AddParam("PageSize", recordsPerPage)
                  .AddParam("Sort", strSort)
                  .ExecAsync(async r =>
                  {
                      rows = await r.ToListAsync<GetSupliers>().ConfigureAwait(false);
                  }).ConfigureAwait(false);

            return rows;
        }

    }
}
