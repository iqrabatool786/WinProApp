using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Departments;

namespace WinProApp.Controllers.Warehouse
{
    public class DepartmentsController : BasedUserController
    {
        public readonly DepartmentServices _departmentServices;
        public DepartmentsController(DepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }

        [Route("/Warehouse/Departments")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/DepartmentList")]
        public IActionResult DepartmentList(JQueryDataTableParamModel param)
        {
            var results = _departmentServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateDepartment(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _departmentServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _departmentServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Departments();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _departmentServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetDepartmentInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetDepartmentInfo(int id)
        {
            try
            {
                var result = await _departmentServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteDepartment/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _departmentServices.GetByIdAsync(id);
                await _departmentServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DepartmentValidateNameEng")]
        public IActionResult DepartmentValidateNameEng(string NameEng, int Id)
        {
            bool flag = _departmentServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/DepartmentValidateNameArabic")]
        public IActionResult DepartmentValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _departmentServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
