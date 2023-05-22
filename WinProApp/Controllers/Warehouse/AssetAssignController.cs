using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.ViewModels.Warehouse.AssetAssign;

namespace WinProApp.Controllers.Warehouse
{
    public class AssetAssignController : BasedUserController
    {
        public readonly AssetAssignService _assetAssignServices;
        public readonly AssetService _assetServices;
        public readonly AssetTypesService _assetTypesService;
        public readonly AssetLocationService _assetLocationService;
        public readonly AssetDepartmentService _assetDepartmentServices;
       public readonly CommonService _commonServices;


        public AssetAssignController(AssetAssignService assetAssignServices, AssetService assetServices, AssetTypesService assetTypesService, AssetLocationService assetLocationService, AssetDepartmentService assetDepartmentServices,  CommonService commonServices)
        {
            _assetAssignServices = assetAssignServices;
            _assetServices = assetServices;
            _assetTypesService = assetTypesService;
            _assetLocationService = assetLocationService;
            _assetDepartmentServices = assetDepartmentServices;
            _commonServices = commonServices;
        }

        [Route("/Warehouse/AssetAsigns")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var assets = await _assetServices.GetByAllAsync();
            
            ViewBag.Assets = new SelectList(assets, "Id", RequestCulture.Name == "ar" ? "AssetNameEng": "AssetNameArabic");
            return View();
        }

        [Route("/Warehouse/AssetAsignList")]
        public IActionResult AssetAsignList(JQueryDataTableParamModel param)
        {
            var results = _assetAssignServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                AssetId = x.AssetId,
                AssignTo=x.AssignTo,
                AssetNameEng = x.AssetId!=null? _assetServices.GetByIdAsync(x.AssetId.Value).Result.AssetNameEng : null,
                AssetNameArabic = x.AssetId != null ? _assetServices.GetByIdAsync(x.AssetId.Value).Result.AssetNameArabic : null,
                Status=x.Status==true?"Active":"Disable",
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

        [Route("/Warehouse/CreateUpdateAssetAssign")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateAssetAssign(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _assetAssignServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.AssetId = model.AssetId;
                info.AssignTo = model.AssignTo;
                info.Status = model.Status;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _assetAssignServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new AssetAssign();
                info.AssetId = model.AssetId;
                info.AssignTo = model.AssignTo;
                info.Status = model.Status;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                await _assetAssignServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetAssetAsignInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetAssetAsignInfo(int id)
        {
            try
            {
                var result = await _assetAssignServices.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    AssetId = result.AssetId,
                    AssignTo = result.AssignTo,                    
                    Status = result.Status==true?true:false,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }


        [Route("/Warehouse/GetAssetDetails/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAssetDetails(int id)
        {
            try
            {
                var result = await _assetServices.GetByIdAsync(id);

                var info = new AssetInfoViewModel()
                {
                    Id = id,
                    TypeName = result.AssetTypeId!=null?_assetTypesService.GetByIdAsync(result.AssetTypeId.Value).Result.AssetTypeEng:"",
                    LocationName = result.AssetLocationId!=null?_assetLocationService.GetByIdAsync(result.AssetLocationId.Value).Result.LocationEng:"",
                    DepartmentName = result.AssetDepartmentId!=null?_assetDepartmentServices.GetByIdAsync(result.AssetDepartmentId.Value).Result.DepartmentEng:"",
                };
                return Json(data:info);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteAssetAsign/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _assetAssignServices.GetByIdAsync(id);
                await _assetAssignServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }
    }
}
