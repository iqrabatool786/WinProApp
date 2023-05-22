using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.ViewModels.Warehouse.AssetLocation;

namespace WinProApp.Controllers.Warehouse
{
    public class AssetLocationController : BasedUserController
    {
        public readonly AssetLocationService _assetLocationServices;

        public AssetLocationController(AssetLocationService assetLocationServices)
        {
            _assetLocationServices = assetLocationServices;
        }

        [Route("/Warehouse/AssetLocations")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/AssetLocationList")]
        public IActionResult AssetLocationList(JQueryDataTableParamModel param)
        {
            var results = _assetLocationServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                LocationEng = x.LocationEng,
                LocationArabic = x.LocationArabic,
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

        [Route("/Warehouse/CreateUpdateAssetLocation")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateAssetLocation(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _assetLocationServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.LocationEng = model.LocationEng;
                info.LocationArabic = model.LocationArabic;

                await _assetLocationServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new AssetLocation();
                info.LocationEng = model.LocationEng;
                info.LocationArabic = model.LocationArabic;

                await _assetLocationServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetAssetLocationInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetAssetLocationInfo(int id)
        {
            try
            {
                var result = await _assetLocationServices.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    LocationEng = result.LocationEng,
                    LocationArabic = result.LocationArabic,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteAssetLocation/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _assetLocationServices.GetByIdAsync(id);
                await _assetLocationServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/AssetLocationValidateNameEng")]
        public IActionResult AssetLocationValidateNameEng(string NameEng, int Id)
        {
            bool flag = _assetLocationServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/AssetLocationValidateNameArabic")]
        public IActionResult AssetLocationValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _assetLocationServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
