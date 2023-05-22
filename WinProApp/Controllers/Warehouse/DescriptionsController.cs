using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Descriptions;

namespace WinProApp.Controllers.Warehouse
{
    public class DescriptionsController : BasedUserController
    {
        public readonly DescriptionService _descriptionServices;

        public DescriptionsController(DescriptionService descriptionServices)
        {
            _descriptionServices = descriptionServices;
        }

        [Route("/Warehouse/Descriptions")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/DescriptionList")]
        public IActionResult DescriptionList(JQueryDataTableParamModel param)
        {
            var results = _descriptionServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateDescription")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateDescription(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _descriptionServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _descriptionServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Descriptions();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _descriptionServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetDescriptionInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetDescriptionInfo(int id)
        {
            try
            {
                var result = await _descriptionServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteDescription/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _descriptionServices.GetByIdAsync(id);
                await _descriptionServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DescriptionValidateNameEng")]
        public IActionResult DescriptionValidateNameEng(string NameEng, int Id)
        {
            bool flag = _descriptionServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/DescriptionValidateNameArabic")]
        public IActionResult DescriptionValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _descriptionServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
