using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Groups;

namespace WinProApp.Controllers.Warehouse
{
    public class GroupsController : BasedUserController
    {
        public readonly GroupService _groupServices;

        public GroupsController(GroupService groupServices)
        {
            _groupServices = groupServices;
        }

        [Route("/Warehouse/Groups")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/GroupList")]
        public IActionResult GroupList(JQueryDataTableParamModel param)
        {
            var results = _groupServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateGroup")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateGroup(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _groupServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _groupServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Groups();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _groupServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetGroupInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetGroupInfo(int id)
        {
            try
            {
                var result = await _groupServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteGroup/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _groupServices.GetByIdAsync(id);
                await _groupServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/GroupValidateNameEng")]
        public IActionResult GroupValidateNameEng(string NameEng, int Id)
        {
            bool flag = _groupServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/GroupValidateNameArabic")]
        public IActionResult GroupValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _groupServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
