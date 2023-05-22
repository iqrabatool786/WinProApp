using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.ViewModels.Warehouse.Asset;
namespace WinProApp.Controllers.Warehouse
{
    public class AssetController : BasedUserController
    {
        public readonly AssetService _assetServices;
        public readonly AssetTypesService _assetTypesServices;
        public readonly AssetLocationService _assetLocationServices;
        public readonly AssetDepartmentService _assetDepartmentServices;
        public readonly PurchaseService _purchaseService;

        public AssetController(AssetService assetServices, AssetTypesService assetTypesServices, AssetLocationService assetLocationServices, AssetDepartmentService assetDepartmentServices, PurchaseService purchaseService)
        {
            _assetServices = assetServices;
            _assetTypesServices = assetTypesServices;
            _assetLocationServices = assetLocationServices;
            _assetDepartmentServices = assetDepartmentServices;
            _purchaseService = purchaseService;
        }

        [Route("/Warehouse/Assets")]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var assetTypes = await _assetTypesServices.GetByAllAsync();
            var assetDeps = await _assetDepartmentServices.GetByAllAsync();
            var assetLocations = await _assetLocationServices.GetByAllAsync();

            ViewBag.AssetTypes = new SelectList(assetTypes, "Id", RequestCulture.Name == "ar" ? "AssetTypeArabic" : "AssetTypeEng");
            ViewBag.AssetDepartments = new SelectList(assetDeps, "Id", RequestCulture.Name == "ar" ? "DepartmentArabic" : "DepartmentEng");
            ViewBag.AssetLocations = new SelectList(assetLocations, "Id", RequestCulture.Name == "ar" ? "LocationArabic" : "LocationEng");
            return View();
        }

        [Route("/Warehouse/AssetList")]
        [HttpPost]
        public IActionResult AssetList(JQueryDataTableParamModel param)
        {
            var results = _assetServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                AssetTypeId = x.AssetTypeId,
                AssetTypeNameEng= x.AssetTypeId!=null? _assetTypesServices.GetByIdAsync(x.AssetTypeId.Value).Result.AssetTypeEng:null,
                AssetTypeNameArabic = x.AssetTypeId != null ? _assetTypesServices.GetByIdAsync(x.AssetTypeId.Value).Result.AssetTypeArabic : null,
                AssetDepartmentId = x.AssetDepartmentId,
                AssetDepartmentNameEng = x.AssetDepartmentId !=null? _assetDepartmentServices.GetByIdAsync(x.AssetDepartmentId.Value).Result.DepartmentEng : null,
                AssetDepartmentNameArabic = x.AssetDepartmentId != null ? _assetDepartmentServices.GetByIdAsync(x.AssetDepartmentId.Value).Result.DepartmentEng : null,
                AssetLocationId= x.AssetLocationId,
                AssetLocationNameEng=x.AssetLocationId !=null? _assetLocationServices.GetByIdAsync(x.AssetLocationId.Value).Result.LocationEng: null,
                AssetLocationNameArabic=x.AssetLocationId !=null? _assetLocationServices.GetByIdAsync(x.AssetLocationId.Value).Result.LocationArabic : null,
                Barcode =x.Barcode,
                AssetNameEng = x.AssetNameEng,
                AssetNameArabic = x.AssetNameArabic,
                DesignationOfStaff= x.DesignationOfStaff,
                ManufactureName = x.ManufactureName,
                WarrentyPeriod=x.WarrentyPeriod,
                Temp = x.Temp,
                ManufactureDate = x.ManufactureDate !=null? x.ManufactureDate.Value.ToString("yyyy-MM-dd"):null,
                ExpireDate= x.ExpireDate != null ? x.ExpireDate.Value.ToString("yyyy-MM-dd") : null,
                AssetValue=x.AssetValue,
                TotalRecordCount=x.TotalRecordCount,
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

        [Route("/Warehouse/CreateUpdateAsset")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateAsset(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _assetServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.AssetTypeId = model.AssetTypeId;
                info.AssetDepartmentId = model.AssetDepartmentId;
                info.AssetLocationId= model.AssetLocationId;
                info.Barcode = model.Barcode;
                info.AssetNameEng = model.AssetNameEng;
                info.AssetNameArabic = model.AssetNameArabic;
                info.DesignationOfStaff = model.DesignationOfStaff;
                info.ManufactureName = model.ManufactureName;
                info.WarrentyPeriod = model.WarrentyPeriod;
                info.Temp = model.Temp;
                info.ManufactureDate = model.ManufactureDate;
                info.ExpireDate = model.ExpireDate;
                info.AssetValue = model.AssetValue;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _assetServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Asset();
                info.AssetTypeId = model.AssetTypeId;
                info.AssetDepartmentId = model.AssetDepartmentId;
                info.AssetLocationId = model.AssetLocationId;
                info.Barcode = model.Barcode;
                info.AssetNameEng = model.AssetNameEng;
                info.AssetNameArabic = model.AssetNameArabic;
                info.DesignationOfStaff = model.DesignationOfStaff;
                info.ManufactureName = model.ManufactureName;
                info.WarrentyPeriod = model.WarrentyPeriod;
                info.Temp = model.Temp;
                info.ManufactureDate = model.ManufactureDate;
                info.ExpireDate = model.ExpireDate;
                info.AssetValue = model.AssetValue;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _assetServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetAssetInfo/{id}")]
        [HttpGet]
        public async Task<DetailsViewModel> GetAssetInfo(int id)
        {
            try
            {
                var result = await _assetServices.GetByIdAsync(id);

                var info = new DetailsViewModel()
                {
                    Id = result.Id,
                    AssetTypeId = result.AssetTypeId,
                    AssetTypeNameEng = result.AssetTypeId!=null?_assetTypesServices.GetByIdAsync(result.AssetTypeId.Value).Result.AssetTypeEng:"",
                    AssetTypeNameArabic = result.AssetTypeId != null ? _assetTypesServices.GetByIdAsync(result.AssetTypeId.Value).Result.AssetTypeArabic : "",
                    AssetDepartmentId = result.AssetDepartmentId,
                    AssetDepartmentNameEng = result.AssetDepartmentId!=null?_assetDepartmentServices.GetByIdAsync(result.AssetDepartmentId.Value).Result.DepartmentEng:"",
                    AssetDepartmentNameArabic= result.AssetDepartmentId != null ? _assetDepartmentServices.GetByIdAsync(result.AssetDepartmentId.Value).Result.DepartmentArabic : "",
                    AssetLocationId = result.AssetLocationId,
                    AssetLocationNameEng= result.AssetLocationId!=null?_assetLocationServices.GetByIdAsync(result.AssetLocationId.Value).Result.LocationEng:"",
                    AssetLocationNameArabic = result.AssetLocationId != null ? _assetLocationServices.GetByIdAsync(result.AssetLocationId.Value).Result.LocationArabic : "",
                    Barcode = result.Barcode,
                    AssetNameEng = result.AssetNameEng,
                    AssetNameArabic = result.AssetNameArabic,
                    DesignationOfStaff= result.DesignationOfStaff,
                    ManufactureName = result.ManufactureName,
                    WarrentyPeriod = result.WarrentyPeriod,
                    Temp = result.Temp,
                    ManufactureDate = result.ManufactureDate != null ? result.ManufactureDate.Value.ToString("yyyy-MM-dd") : null,
                    ExpireDate = result.ExpireDate != null ? result.ExpireDate.Value.ToString("yyyy-MM-dd") : null,
                    AssetValue = result.AssetValue,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteAsset/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _assetServices.GetByIdAsync(id);
                await _assetServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/AssetNameValidateNameEng")]
        public IActionResult AssetNameValidateNameEng(string NameEng, int Id)
        {
            bool flag = _assetServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/AssetNameValidateNameArabic")]
        public IActionResult AssetNameValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _assetServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/AssetsReLocated")]
        [HttpGet]
        public async Task<IActionResult> AssetsReLocated()
        {
            var assetTypes = await _assetTypesServices.GetByAllAsync();
            var assetDeps = await _assetDepartmentServices.GetByAllAsync();
            var assetLocations = await _assetLocationServices.GetByAllAsync();

            ViewBag.AssetTypes = new SelectList(assetTypes, "Id", RequestCulture.Name == "ar" ? "AssetTypeArabic" : "AssetTypeEng");
            ViewBag.AssetDepartments = new SelectList(assetDeps, "Id", RequestCulture.Name == "ar" ? "DepartmentArabic" : "DepartmentEng");
            ViewBag.AssetLocations = new SelectList(assetLocations, "Id", RequestCulture.Name == "ar" ? "LocationArabic" : "LocationEng");
            return View();
        }

        [Route("/Warehouse/AssetsReLocated")]
        [HttpPost]
        public async Task<IActionResult> AssetsReLocated(AddEditViewModel model)
        {
                var info = await _assetServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.AssetTypeId = model.AssetTypeId;
                info.AssetDepartmentId = model.AssetDepartmentId;
                info.AssetLocationId= model.AssetLocationId;
                info.Barcode = model.Barcode;
                info.AssetNameEng = model.AssetNameEng;
                info.AssetNameArabic = model.AssetNameArabic;
                info.DesignationOfStaff= model.DesignationOfStaff;
                info.ManufactureName = model.ManufactureName;
                //info.WarrentyPeriod = model.WarrentyPeriod;
                //info.Temp = model.Temp;
                //info.ManufactureDate = model.ManufactureDate;
                //info.ExpireDate = model.ExpireDate;
                //info.AssetValue = model.AssetValue;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _assetServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
        }


        [Route("/Warehouse/GetAssetInfoByBarcode")]
        [HttpPost]
        public async Task<IActionResult> GetAssetInfoByBarcode(string Prefix)
        {
            var info = await _assetServices.GetAssetByBarcodeAsync(Prefix);

            var productInfo1 = info.Select(t => new Asset()
            {
                Id = t.Id,
                Barcode = t.Barcode.Trim(),
            }).ToList();

            return Json(data: productInfo1);
        }

        [Route("/Warehouse/GetAssetInfoByNameEng")]
        [HttpPost]
        public async Task<IActionResult> GetAssetInfoByNameEng(string Prefix)
        {
            var info = await _assetServices.GetAssetByNameEngAsync(Prefix);

            var productInfo1 = info.Select(t => new Asset()
            {
                Id = t.Id,
                AssetNameEng = t.AssetNameEng,
            }).ToList();

            return Json(data: productInfo1);
        }

        [Route("/Warehouse/GetAssetInfoByNameArabic")]
        [HttpPost]
        public async Task<IActionResult> GetAssetInfoByNameArabic(string Prefix)
        {
            var info = await _assetServices.GetAssetByNameEngAsync(Prefix);

            var productInfo1 = info.Select(t => new Asset()
            {
                Id = t.Id,
                AssetNameArabic = t.AssetNameArabic,                
            }).ToList();

            return Json(data: productInfo1);
        }

        [Route("/Warehouse/AssetReports")]
        [HttpGet]
        public async Task<IActionResult> AssetReports()
        {
            return View();
        }
    }
}
