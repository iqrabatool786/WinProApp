using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Units;

namespace WinProApp.Controllers.Warehouse
{
    public class UnitsController : BasedUserController
    {
        public readonly UnitService _unitServices;

        public UnitsController(UnitService unitServices)
        {
            _unitServices = unitServices;
        }

        [Route("/Warehouse/Units")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/UnitList")]
        public IActionResult UnitList(JQueryDataTableParamModel param)
        {
            var results = _unitServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                NameEng = x.NameEng,
                NameArabic = x.NameArabic,
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

        [Route("/Warehouse/CreateUpdateUnit")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateUnit(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _unitServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _unitServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Units();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _unitServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetUnitInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetUnitInfo(int id)
        {
            try
            {
                var result = await _unitServices.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    NameEng = result.NameEng,
                    NameArabic = result.NameArabic,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteUnit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _unitServices.GetByIdAsync(id);
                await _unitServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/UnitValidateNameEng")]
        public IActionResult UnitValidateNameEng(string NameEng, int Id)
        {
            bool flag = _unitServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/UnitValidateNameArabic")]
        public IActionResult UnitValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _unitServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
