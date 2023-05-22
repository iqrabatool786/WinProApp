using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Seassons;

namespace WinProApp.Controllers.Warehouse
{
    public class SeassonsController : BasedUserController
    {
        public readonly SeassonService _seassonServices;

        public SeassonsController(SeassonService seassonServices)
        {
            _seassonServices = seassonServices;
        }

        [Route("/Warehouse/Seassons")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/SeassonList")]
        public IActionResult SeassonList(JQueryDataTableParamModel param)
        {
            var results = _seassonServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateSeasson")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateSeasson(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _seassonServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _seassonServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Seassons();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _seassonServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetSeassonInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetSeassonInfo(int id)
        {
            try
            {
                var result = await _seassonServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteSeasson/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _seassonServices.GetByIdAsync(id);
                await _seassonServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/SeassonValidateNameEng")]
        public IActionResult SeassonValidateNameEng(string NameEng, int Id)
        {
            bool flag = _seassonServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/SeassonValidateNameArabic")]
        public IActionResult SeassonValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _seassonServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
