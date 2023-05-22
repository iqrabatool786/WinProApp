using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Merchant.BuyGetLowest;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WinProApp.Controllers.Merchant
{
    [Authorize(Roles = "Administrator,Warehouse,Purchase")]
    public class BuyGetLowestDiscountController : BasedUserController
    {
        public readonly BuyGetLowestDiscountService _buyGetLowestDiscountService;
        public readonly StoreService _storeService;
        public readonly ProductInfoService _productInfoService;
        public readonly CommonService _commonService;

        public BuyGetLowestDiscountController(BuyGetLowestDiscountService buyGetLowestDiscountService, StoreService storeService, ProductInfoService productInfoService, CommonService commonService)
        {
            _buyGetLowestDiscountService = buyGetLowestDiscountService;
            _storeService = storeService;
            _productInfoService = productInfoService;
            _commonService = commonService;
        }

        [Route("/Merchant/BuyGetLowestDiscount")]
        [HttpGet]
        public async Task<IActionResult> BuyGetLowestDiscount()
        {
            var storeInfo = await _storeService.GetByAllAsync();
            ViewBag.StoreInfo = new SelectList(storeInfo, "Id", "Name");
            return View();
        }

        [Route("/Merchant/GetBuyGetLowestDiscountList")]
        [HttpPost]
        public IActionResult GetBuyGetLowestDiscountList(JQueryDataTableParamModel param)
        {
            var results = _buyGetLowestDiscountService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                StoreName = x.StoreId > 0 ? _storeService.GetByIdAsync(x.StoreId).Result.Name : "-All-",
                BuyProductId = x.BuyProductId,
                BuyBarcode = x.BuyBarcode,
                BuyQty = x.BuyQty,
                GetProductId = x.GetProductId,
                GetProductBarcode = x.GetProductBarcode,
                GetAmount = x.GetAmount,
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

        [Route("/Merchant/CreateUpdateBuyGetLowestDiscount")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateBuyGetLowestDiscount(AddEditViewModel model)
        {
            long insertId = 0;
            if (model.Id == 0)
            {
                var info = new BuyGetLowestDiscount();
                info.StoreId = model.StoreId;
                info.BuyProductId = model.BuyProductId;
                info.BuyBarcode = model.BuyBarcode;
                info.BuyQty = model.BuyQty;
                info.GetProductId = model.GetProductId;
                info.GetProductBarcode = model.GetProductBarcode;
                info.GetAmount = model.GetAmount;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                info.Approved = model.Approved;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _buyGetLowestDiscountService.CreateAsync(info);
                insertId = data.Id;
            }
            else
            {
                var info = await _buyGetLowestDiscountService.GetByIdAsync(model.Id);
                info.StoreId = model.StoreId;
                info.StoreId = model.StoreId;
                info.BuyProductId = model.BuyProductId;
                info.BuyBarcode = model.BuyBarcode;
                info.BuyQty = model.BuyQty;
                info.GetProductId = model.GetProductId;
                info.GetProductBarcode = model.GetProductBarcode;
                info.GetAmount = model.GetAmount;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                info.Approved = model.Approved;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _buyGetLowestDiscountService.UpdateAsync(info);
                insertId = data.Id;
            }

            return new JsonResult(new { id = insertId });
        }


        [Route("/Merchant/GetBuyGetLowestDiscountInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetBuyGetLowestDiscountInfo(int id)
        {
            var info = await _buyGetLowestDiscountService.GetByIdAsync(id);
            var model = new DetailsViewModel();
            model.Id = id;
            model.StoreId = info.StoreId;
            model.BuyProductId = info.BuyProductId;
            model.BuyBarcode = info.BuyBarcode;
            model.BuyQty = info.BuyQty;
            model.GetProductId = info.GetProductId;
            model.GetProductBarcode = info.GetProductBarcode;
            model.GetAmount = info.GetAmount;
            model.IsMembersOnly = info.IsMembersOnly == true ? "true" : "false";
            model.StartDate = info.StartDate.ToString("yyyy-MM-dd");
            model.EndDate = info.EndDate.ToString("yyyy-MM-dd");
            model.Approved = info.Approved == true ? "true" : "false";

            return Json(data: model);

        }


        [Route("/Merchant/DeleteBuyGetLowestDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteBuyGetLowestDiscount(long id)
        {
            var info = await _buyGetLowestDiscountService.GetByIdAsync(id);
            await _buyGetLowestDiscountService.DeleteAsync(info);
            return new JsonResult(new { id = id });

        }
    }
}
