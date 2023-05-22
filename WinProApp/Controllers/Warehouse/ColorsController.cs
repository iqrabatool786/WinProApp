using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Colors;

namespace WinProApp.Controllers.Warehouse
{
    public class ColorsController : BasedUserController
    {
        public readonly ColorServices _colorServices;
        public ColorsController(ColorServices colorServices)
        {
            _colorServices = colorServices;
        }

        [Route("/Warehouse/Colors")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/ColorList")]
        public IActionResult ColorList(JQueryDataTableParamModel param)
        {
            var results = _colorServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateColor")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateColor(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _colorServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _colorServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Colors();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _colorServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetColorInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetColorInfo(int id)
        {
            try
            {
                var result = await _colorServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteColor/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _colorServices.GetByIdAsync(id);
                await _colorServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/ColorValidateNameEng")]
        public IActionResult ColorValidateNameEng(string NameEng, int Id)
        {
            bool flag = _colorServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/ColorValidateNameArabic")]
        public IActionResult ColorValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _colorServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
