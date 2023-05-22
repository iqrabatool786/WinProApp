using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Sizes;

namespace WinProApp.Controllers.Warehouse
{
    public class SizesController : BasedUserController
    {
        public readonly SizeServices _sizeServices;
        public SizesController(SizeServices sizeServices)
        {
            _sizeServices = sizeServices;
        }

        [Route("/Warehouse/Sizes")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/SizeList")]
        public IActionResult SizeList(JQueryDataTableParamModel param)
        {
            var results = _sizeServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateSize")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateSize(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _sizeServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _sizeServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Sizes();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _sizeServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetSizeInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetSizeInfo(int id)
        {
            try
            {
                var result = await _sizeServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteSize/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _sizeServices.GetByIdAsync(id);
                await _sizeServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/SizeValidateNameEng")]
        public IActionResult SizeValidateNameEng(string NameEng, int Id)
        {
            bool flag = _sizeServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/SizeValidateNameArabic")]
        public IActionResult SizeValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _sizeServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
