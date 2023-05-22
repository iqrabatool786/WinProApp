using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Merchant.GetAmountOff;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WinProApp.Controllers.Merchant
{
    [Authorize(Roles = "Administrator,Warehouse,Purchase")]
    public class GetAmountOffController : BasedUserController
    {
        public readonly BuyQtyGetAmountOffDiscountService _buyQtyGetAmountOffDiscountService;
        public readonly StoreService _storeService;
        public readonly ProductInfoService _productInfoService;
        public readonly CommonService _commonService;

        public GetAmountOffController(BuyQtyGetAmountOffDiscountService buyQtyGetAmountOffDiscountService, StoreService storeService, ProductInfoService productInfoService, CommonService commonService)
        {
            _buyQtyGetAmountOffDiscountService = buyQtyGetAmountOffDiscountService;
            _storeService = storeService;
            _productInfoService = productInfoService;
            _commonService = commonService;
        }

        [Route("/Merchant/GetAmountOffDiscount")]
        [HttpGet]
        public async Task<IActionResult> GetAmountOffDiscount()
        {
            var storeInfo = await _storeService.GetByAllAsync();
            ViewBag.StoreInfo = new SelectList(storeInfo, "Id", "Name");
            return View();
        }

        [Route("/Merchant/GetAmountOffDiscountList")]
        [HttpPost]
        public IActionResult GetAmountOffDiscountList(JQueryDataTableParamModel param)
        {
            var results = _buyQtyGetAmountOffDiscountService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                StoreName = x.StoreId > 0 ? _storeService.GetByIdAsync(x.StoreId).Result.Name : "-All-",
                BuyProductId = x.BuyProductId,
                BuyBarcode = x.BuyBarcode,
                BuyQty = x.BuyQty,
                OffAmount = x.OffAmount,
                IsMembersOnly = x.IsMembersOnly == true ? "Members Only" : "All",
                StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                Approved = x.Approved == true ? "Yes" : "No",
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

        [Route("/Merchant/CreateUpdateGetAmountOffDiscount")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateGetAmountOffDiscount(AddEditViewModel model)
        {
            long insertId = 0;
            if (model.Id == 0)
            {
                var info = new BuyQtyGetAmountOffDiscount();
                info.StoreId = model.StoreId;
                info.BuyProductId = model.BuyProductId;
                info.BuyBarcode = model.BuyBarcode;
                info.BuyQty = model.BuyQty;
                info.OffAmount = model.OffAmount;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                info.Approved = model.Approved;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _buyQtyGetAmountOffDiscountService.CreateAsync(info);
                insertId = data.Id;
            }
            else
            {
                var info = await _buyQtyGetAmountOffDiscountService.GetByIdAsync(model.Id);
                info.StoreId = model.StoreId;
                info.StoreId = model.StoreId;
                info.BuyProductId = model.BuyProductId;
                info.BuyBarcode = model.BuyBarcode;
                info.BuyQty = model.BuyQty;
                info.OffAmount = model.OffAmount;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                info.Approved = model.Approved;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _buyQtyGetAmountOffDiscountService.UpdateAsync(info);
                insertId = data.Id;
            }

            return new JsonResult(new { id = insertId });
        }


        [Route("/Merchant/GetAmountOffDiscountInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAmountOffDiscountInfo(int id)
        {
            var info = await _buyQtyGetAmountOffDiscountService.GetByIdAsync(id);
            var model = new DetailsViewModel();
            model.Id = id;
            model.StoreId = info.StoreId;
            model.BuyProductId = info.BuyProductId;
            model.BuyBarcode = info.BuyBarcode;
            model.BuyQty = info.BuyQty;
            model.OffAmount = info.OffAmount;
            model.IsMembersOnly = info.IsMembersOnly == true ? "true" : "false";
            model.StartDate = info.StartDate.ToString("yyyy-MM-dd");
            model.EndDate = info.EndDate.ToString("yyyy-MM-dd");
            model.Approved = info.Approved == true ? "true" : "false";

            return Json(data: model);

        }


        [Route("/Merchant/DeleteGetAmountOffDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteGetAmountOffDiscount(long id)
        {
            var info = await _buyQtyGetAmountOffDiscountService.GetByIdAsync(id);
            await _buyQtyGetAmountOffDiscountService.DeleteAsync(info);
            return new JsonResult(new { id = id });
        }
    }
}
