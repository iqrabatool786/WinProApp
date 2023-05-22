using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.ViewModels.Warehouse.AssetDepartment;

namespace WinProApp.Controllers.Warehouse
{
    public class AssetDepartmentController : BasedUserController
    {
        public readonly AssetDepartmentService _assetDepartmentServices;

        public AssetDepartmentController(AssetDepartmentService assetDepartmentServices)
        {
            _assetDepartmentServices = assetDepartmentServices;
        }

        [Route("/Warehouse/AssetDepartments")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/AssetDepartmentList")]
        public IActionResult AssetDepartmentList(JQueryDataTableParamModel param)
        {
            var results = _assetDepartmentServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                DepartmentEng = x.DepartmentEng,
                DepartmentArabic = x.DepartmentArabic,
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

        [Route("/Warehouse/CreateUpdateAssetDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateAssetDepartment(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _assetDepartmentServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.DepartmentEng = model.DepartmentEng;
                info.DepartmentArabic = model.DepartmentArabic;

                await _assetDepartmentServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new AssetDepartment();
                info.DepartmentEng = model.DepartmentEng;
                info.DepartmentArabic = model.DepartmentArabic;

                await _assetDepartmentServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetAssetDepartmentInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetAssetDepartmentInfo(int id)
        {
            try
            {
                var result = await _assetDepartmentServices.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    DepartmentEng = result.DepartmentEng,
                    DepartmentArabic = result.DepartmentArabic,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteAssetDepartment/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _assetDepartmentServices.GetByIdAsync(id);
                await _assetDepartmentServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/AssetDepartmentValidateNameEng")]
        public IActionResult AssetDepartmentValidateNameEng(string NameEng, int Id)
        {
            bool flag = _assetDepartmentServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/AssetDepartmentValidateNameArabic")]
        public IActionResult AssetDepartmentValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _assetDepartmentServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
