using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Merchant.FlatDiscount;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WinProApp.Controllers.Merchant
{
    [Authorize(Roles = "Administrator,Warehouse,Purchase")]
    public class FlatDiscountController : BasedUserController
    {
        public readonly FlatDiscountService _flatDiscountService;
        public readonly StoreService _storeService;
        public FlatDiscountController(FlatDiscountService flatDiscountService, StoreService storeService)
        {
            _flatDiscountService = flatDiscountService;
            _storeService = storeService;
        }

        [Route("/Merchant/ManageFlatDiscount")]
        [HttpGet]
        public async Task<IActionResult> ManageFlatDiscount()
        {
            var storeInfo = await _storeService.GetByAllAsync();
            ViewBag.StoreInfo = new SelectList(storeInfo ,"Id", "Name");
            return View();
        }

        [Route("/Merchant/GetFlatDiscountList")]
        [HttpPost]
        public IActionResult GetFlatDiscountList(JQueryDataTableParamModel param)
        {
            var results = _flatDiscountService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                StoreId= x.StoreId,
                StoreName = x.StoreId > 0?_storeService.GetByIdAsync(x.StoreId).Result.Name:"-All-",
                Title = x.Title,
                IsMembersOnly = x.IsMembersOnly == true?"Members Only":"All",
                StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                DiscountPercentage = x.DiscountPercentage!=null? x.DiscountPercentage.Value.ToString("0.00"):"0.00",
                FixedDiscount = x.FixedDiscount!=null?x.FixedDiscount.Value.ToString("0.00") : "0.00",
                StartAmount = x.StartAmount!=null?x.StartAmount.Value.ToString("0.00") : "0.00",
                Approved = x.Approved==true?"Yes":"No",
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

        [Route("/Merchant/CreateUpdateFlatDiscount")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateFlatDiscount(AddEditViewModel model)
        {
           int insertId = 0;
           if(model.Id == 0)
           {
                var info = new FlatDiscount();
                info.StoreId = model.StoreId;
                info.Title = model.Title;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                if(model.DiscountType == "precentage")
                {
                    info.DiscountPercentage = model.DiscountAmount;
                    info.FixedDiscount = 0;
                }
                else
                {
                    info.FixedDiscount = model.DiscountAmount;
                    info.DiscountPercentage = 0;
                }
                info.StartAmount = model.StartAmount;
                info.Approved = model.Approved;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _flatDiscountService.CreateAsync(info);
                insertId = data.Id;
            }
            else
            {
                var info = await _flatDiscountService.GetByIdAsync(model.Id);
                info.StoreId = model.StoreId;
                info.Title = model.Title;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                if (model.DiscountType == "precentage")
                {
                    info.DiscountPercentage = model.DiscountAmount;
                    info.FixedDiscount = 0;
                }
                else
                {
                    info.FixedDiscount = model.DiscountAmount;
                    info.DiscountPercentage = 0;
                }
                info.StartAmount = model.StartAmount;
                info.Approved = model.Approved;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _flatDiscountService.UpdateAsync(info);
                insertId = data.Id;
            }

            return new JsonResult(new { id = insertId });
        }

        
        [Route("/Merchant/GetFlatDiscountInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetFlatDiscountInfo(int id)
        {
            var info = await _flatDiscountService.GetByIdAsync(id);
            var model = new DetailViewModel();
            model.Id = id;
            model.StoreId = info.StoreId;
            model.Title = info.Title;
            model.IsMembersOnly = info.IsMembersOnly==true?"true":"false";
            model.StartDate= info.StartDate.ToString("yyyy-MM-dd");
            model.EndDate= info.EndDate.ToString("yyyy-MM-dd");
            model.DiscountType = info.DiscountPercentage != null ? (info.DiscountPercentage.Value > 0 ? "precentage" : "fixed") : "fixed";
            model.DiscountPercentage = info.DiscountPercentage;
            model.FixedDiscount = info.FixedDiscount;
            model.StartAmount = info.StartAmount;
            model.Approved = info.Approved==true?"true":"false";

            return Json(data:model);

        }

        [Route("/Merchant/DeleteFlatDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteFlatDiscount(int id)
        {
            var info = await _flatDiscountService.GetByIdAsync(id);
            await _flatDiscountService.DeleteAsync(info);
            return new JsonResult(new { id = id });
        }

    }
}
