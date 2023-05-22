using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Brands;

namespace WinProApp.Controllers.Warehouse
{
    public class BrandsController : BasedUserController
    {
        public readonly BrandService _brandServices;

        public BrandsController(BrandService brandServices)
        {
            _brandServices = brandServices;
        }

        [Route("/Warehouse/Brands")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/BrandList")]
        public IActionResult BrandList(JQueryDataTableParamModel param)
        {
            var results = _brandServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateBrand")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateBrand(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _brandServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _brandServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Brands();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _brandServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetBrandInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetBrandInfo(int id)
        {
            try
            {
                var result = await _brandServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteBrand/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _brandServices.GetByIdAsync(id);
                await _brandServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/BrandValidateNameEng")]
        public IActionResult BrandValidateNameEng(string NameEng, int Id)
        {
            bool flag = _brandServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/BrandValidateNameArabic")]
        public IActionResult BrandValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _brandServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
