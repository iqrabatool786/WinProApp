using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.ViewModels.Warehouse.AssetTypes;

namespace WinProApp.Controllers.Warehouse
{
    public class AssetTypesController : BasedUserController
    {
        public readonly AssetTypesService _assetTypesServices;

        public AssetTypesController(AssetTypesService assetTypesServices)
        {
            _assetTypesServices = assetTypesServices;
        }

        [Route("/Warehouse/AssetTypes")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/AssetTypeList")]
        public IActionResult AssetTypeList(JQueryDataTableParamModel param)
        {
            var results = _assetTypesServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                AssetTypeEng = x.AssetTypeEng,
                AssetTypeArabic = x.AssetTypeArabic,
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

        [Route("/Warehouse/CreateUpdateAssetType")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateAssetType(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _assetTypesServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.AssetTypeEng = model.AssetTypeEng;
                info.AssetTypeArabic = model.AssetTypeArabic;

                await _assetTypesServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new AssetTypes();
                info.AssetTypeEng = model.AssetTypeEng;
                info.AssetTypeArabic = model.AssetTypeArabic;

                await _assetTypesServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetAssetTypeInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetAssetTypeInfo(int id)
        {
            try
            {
                var result = await _assetTypesServices.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    AssetTypeEng = result.AssetTypeEng,
                    AssetTypeArabic = result.AssetTypeArabic,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteAssetType/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _assetTypesServices.GetByIdAsync(id);
                await _assetTypesServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/AssetTypeValidateNameEng")]
        public IActionResult AssetTypeValidateNameEng(string NameEng, int Id)
        {
            bool flag = _assetTypesServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/AssetTypeValidateNameArabic")]
        public IActionResult AssetTypeValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _assetTypesServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
