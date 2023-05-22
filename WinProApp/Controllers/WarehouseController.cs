using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Warehouse;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Warehouse")]
    public class WarehouseController : BasedUserController
    {
        public readonly StoreService _storeService;
        public readonly RegisterService _registerService;

        public WarehouseController(StoreService storeService, RegisterService registerService)
        {
            _storeService = storeService;
            _registerService = registerService;
        }

        #region Store Management

        [Route("/Warehouse/Store")]
        [HttpGet]
        public async Task<IActionResult> Store()
        {
            int curMaxId = 0;
            curMaxId = await _storeService.GetMaxIdAsync();
            curMaxId += 1;
            ViewBag.MaxId = curMaxId.ToString();
            return View();
        }

        [Route("/Warehouse/StoreList")]
        public IActionResult StoreList(JQueryDataTableParamModel param)
        {
            var results = _storeService.GetList(param).Result.Select(x => new ListStoreViewModel()
            {
                Id = x.Id,
                StoreCode= x.StoreCode,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                GlAccount = x.GlAccount,
                Brand = x.Brand,
                Brandcode = x.Brandcode,
                StoreType = x.StoreType,
                Office = x.Office,
                City = x.City,
                VatNo= x.VatNo,
                CRNo= x.CRNo,
                TotalRecordCount = x.TotalRecordCount
            }).ToList();

            int TotalRecordCount = 0;

            if (results.Count > 0)
                TotalRecordCount = results[0].TotalRecordCount;

            return Json(data: new
            {
                param.sEcho,
                iTotalRecords = TotalRecordCount,
                iTotalDisplayRecords = TotalRecordCount,
                aaData = results
            });
        }

        [Route("/Warehouse/CreateUpdateStore")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateStore(AddEditStoreViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _storeService.GetByIdAsync(model.Id);
                info.Id= model.Id;
                info.StoreCode = model.StoreCode;
                info.Name = model.Name;
                info.Address = model.Address;
                info.Phone = model.Phone;
                info.GlAccount = model.GlAccount;
                info.Brand = model.Brand;
                info.Brandcode = model.Brandcode;
                info.StoreType = model.StoreType;
                info.Office = model.Office;
                info.City = model.City;
                info.VatNo= model.VatNo;
                info.CRNo= model.CRNo;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _storeService.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Store();
                info.StoreCode = model.StoreCode;
                info.Name = model.Name;
                info.Address = model.Address;
                info.Phone = model.Phone;
                info.GlAccount = model.GlAccount;
                info.Brand = model.Brand;
                info.Brandcode = model.Brandcode;
                info.StoreType = model.StoreType;
                info.Office = model.Office;
                info.City = model.City;
                info.VatNo = model.VatNo;
                info.CRNo = model.CRNo;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

               await _storeService.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetStoreInfo/{id}")]
        [HttpGet]
        public async Task<AddEditStoreViewModel> GetStoreInfo(int id)
        {
            try
            {
                var result = await _storeService.GetByIdAsync(id);

                var info = new AddEditStoreViewModel()
                {
                    Id = result.Id,
                    StoreCode = result.StoreCode,
                    Name = result.Name,
                    Address = result.Address,
                    Phone = result.Phone,
                    GlAccount = result.GlAccount,
                    Brand = result.Brand,
                    Brandcode = result.Brandcode,
                    StoreType = result.StoreType,
                    Office = result.Office,
                    City = result.City,
                    VatNo = result.VatNo,
                    CRNo = result.CRNo,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/GetStoreInfoList")]
        [HttpGet]
        public IActionResult GetStoreInfoList()
        {
            try
            {
                var result = _storeService.GetByAllAsync().Result.Select(x => new ListStoreViewModel()
                {
                    Id = x.Id,
                    StoreCode = x.StoreCode,
                    Name = x.Name,
                    Address = x.Address,
                    Phone = x.Phone,
                    GlAccount = x.GlAccount,
                    Brand = x.Brand,
                    Brandcode = x.Brandcode,
                    StoreType = x.StoreType,
                    Office = x.Office,
                    City = x.City,
                    VatNo = x.VatNo,
                    CRNo = x.CRNo,
                }).ToList();

                return Json(data: result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteStore/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                int regCount = await _storeService.GetRegisterCountByIdAsync(id);
                if (regCount > 0)
                {
                    var result = await _storeService.GetByIdAsync(id);
                    await _storeService.DeleteAsync(result);

                    return new JsonResult(new { id = id });
                }
                else
                {
                    return new JsonResult(new { id = 0 });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion


        #region Register Management

        [Route("/Warehouse/Register")]
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var stores = await _storeService.GetByAllAsync();
            ViewBag.Stores = new SelectList(stores, "Id", "Name");
            return View();
        }

        [Route("/Warehouse/RegisterList")]
        public IActionResult RegisterList(JQueryDataTableParamModel param)
        {
            var results = _registerService.GetList(param).Result.Select(x => new ListRegisterViewModel()
            {
                Id = x.Id,
                StoreName = _storeService.GetNameByIdAsync(x.StoreId).Result,
                CashTill = x.CashTill,
                TotalRecordCount = x.TotalRecordCount
            }).ToList();

            int TotalRecordCount = 0;

            if (results.Count > 0)
                TotalRecordCount = results[0].TotalRecordCount;

            return Json(data: new
            {
                param.sEcho,
                iTotalRecords = TotalRecordCount,
                iTotalDisplayRecords = TotalRecordCount,
                aaData = results
            });
        }

        [Route("/Warehouse/CreateUpdateRegister")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateRegister(AddEditRegisterViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _registerService.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.StoreId= model.StoreId;
                info.CashTill = model.CashTill;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _registerService.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Register();
                info.StoreId = model.StoreId;
                info.CashTill = model.CashTill;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _registerService.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetRegisterInfo/{id}")]
        [HttpGet]
        public async Task<DetailsRegisterViewModel> GetRegisterInfo(int id)
        {
            try
            {
                var result = await _registerService.GetByIdAsync(id);

                var info = new DetailsRegisterViewModel()
                {
                    Id = result.Id,
                    StoreId= result.StoreId,
                    StoreName = await _storeService.GetNameByIdAsync(result.StoreId),
                    CashTill = result.CashTill,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteRegister/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteRegister(int id)
        {
            try
            {
                var result = await _registerService.GetByIdAsync(id);
                await _registerService.DeleteAsync(result);

                return new JsonResult(new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion
    }
}
