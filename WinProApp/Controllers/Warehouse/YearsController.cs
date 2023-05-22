using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Years;

namespace WinProApp.Controllers.Warehouse
{
    public class YearsController : Controller
    {
        public readonly YearsService _yearsService;
        public YearsController(YearsService yearsService)
        {
            _yearsService = yearsService;
        }

        [Route("/Warehouse/Years")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/YearsList")]
        public IActionResult SizeList(JQueryDataTableParamModel param)
        {
            var results = _yearsService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                YearName = x.YearName,
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

        [Route("/Warehouse/CreateUpdateYear")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateYear(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _yearsService.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.YearName = model.YearName;



                await _yearsService.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Years();
                info.YearName = model.YearName;

                await _yearsService.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetYearInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetSizeInfo(int id)
        {
            try
            {
                var result = await _yearsService.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    YearName = result.YearName,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteYear/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _yearsService.GetByIdAsync(id);
                await _yearsService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/ValidateYearName")]
        public IActionResult ValidateYearName(int YearName, int Id)
        {
            bool flag = _yearsService.ValidateYear(YearName, Id);

            return Json(data: flag);

        }
    }
}
