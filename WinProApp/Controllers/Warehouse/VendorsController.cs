using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels.Warehouse.Vendors;

namespace WinProApp.Controllers.Warehouse
{
    public class VendorsController : BasedUserController
    {
        public readonly VendorService _vendorServices;

        public VendorsController(VendorService vendorServices)
        {
            _vendorServices = vendorServices;
        }

        [Route("/Warehouse/Vendors")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Warehouse/VendorList")]
        public IActionResult SizeList(JQueryDataTableParamModel param)
        {
            var results = _vendorServices.GetList(param).Result.Select(x => new ListViewModel()
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

        [Route("/Warehouse/CreateUpdateVendor")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateVendor(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _vendorServices.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _vendorServices.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Vendors();
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _vendorServices.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetVendorInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetVendorInfo(int id)
        {
            try
            {
                var result = await _vendorServices.GetByIdAsync(id);

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

        [Route("/Warehouse/DeleteVendor/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _vendorServices.GetByIdAsync(id);
                await _vendorServices.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/VendorValidateNameEng")]
        public IActionResult VendorValidateNameEng(string NameEng, int Id)
        {
            bool flag = _vendorServices.ValidateNameEng(NameEng, Id);

            return Json(data: flag);

        }

        [Route("/Warehouse/VendorValidateNameArabic")]
        public IActionResult VendorValidateNameArabic(string NameArabic, int Id)
        {
            bool flag = _vendorServices.ValidateNameEng(NameArabic, Id);

            return Json(data: flag);

        }
    }
}
