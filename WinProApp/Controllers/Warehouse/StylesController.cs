using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Styles;

namespace WinProApp.Controllers.Warehouse
{
    public class StylesController : BasedUserController
    {
        public readonly StyleServices _styleServices;

        public StylesController(StyleServices styleServices)
        {
            _styleServices = styleServices;
        }

        [Route("/Warehouse/Styles")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/StyleList")]
        public IActionResult StyleList(JQueryDataTableParamModel param)
        {
            var results = _styleServices.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                Code= x.Code,
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

        [Route("/Warehouse/CreateUpdateStyle")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateStyle(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _styleServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.Code= model.Code;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _styleServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Styles();
                info.Code=model.Code;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _styleServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetStyleInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetStyleInfo(int id)
        {
            try
            {
                var result = await _styleServices.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    Code= result.Code,
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

        [Route("/Warehouse/DeleteStyle/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _styleServices.GetByIdAsync(id);
                await _styleServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/StyleValidateNameEng")]
        public IActionResult StyleValidateNameEng(string NameEng, int Id)
        {
            bool flag = _styleServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/StyleValidateNameArabic")]
        public IActionResult StyleValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _styleServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/StyleValidateCode")]
        public IActionResult StyleValidateCode(string Code, int Id)
        {
            bool flag = _styleServices.ValidateCode(Code, Id);

            return Json(data: flag);

        }
    }
}
