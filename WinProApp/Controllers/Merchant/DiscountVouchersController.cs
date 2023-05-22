using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Merchant.Vouchares;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WinProApp.Controllers.Merchant
{
    [Authorize(Roles = "Administrator,Warehouse,Purchase")]
    public class DiscountVouchersController : BasedUserController
    {
        public readonly DiscountVouchersService _discountVouchersService;
        public readonly StoreService _storeService;
        public readonly ProductInfoService _productInfoService;
        public readonly CommonService _commonService;

        public DiscountVouchersController(DiscountVouchersService discountVouchersService, StoreService storeService, ProductInfoService productInfoService, CommonService commonService)
        {
            _discountVouchersService = discountVouchersService;
            _storeService = storeService;
            _productInfoService = productInfoService;
            _commonService = commonService;
        }

        [Route("/Merchant/VoucherDiscount")]
        [HttpGet]
        public async Task<IActionResult> VoucherDiscount()
        {
            var storeInfo = await _storeService.GetByAllAsync();
            ViewBag.StoreInfo = new SelectList(storeInfo, "Id", "Name");
            return View();
        }

        [Route("/Merchant/GetVoucherDiscountList")]
        [HttpPost]
        public IActionResult GetVoucherDiscountList(JQueryDataTableParamModel param)
        {
            var results = _discountVouchersService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                StoreName = x.StoreId > 0 ? _storeService.GetByIdAsync(x.StoreId).Result.Name : "-All-",
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

        [Route("/Merchant/CreateUpdateVouchers")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateVouchers(AddEditViewModel model)
        {
            int insertId = 0;

            string[] itemIds = Request.Form["ItemId"];
            string[] endAmounts = Request.Form["EndAmount"];
            string[] voucherAmounts = Request.Form["VoucharAmount"];
            bool flag = false;

            int itemId = 0;
            decimal amount1 = 0;
            decimal amount2 = 0;
            int curCount = 0;

            var currentData = await _discountVouchersService.GetByAllAsync();

            if (currentData != null)
            {
                if(model.Id == 0)
                {
                    if(model.StoreId== 0)
                    {
                        curCount = currentData.Where(x=>x.EndDate >= model.StartDate).Count();
                        
                    }
                    else
                    {
                        curCount = currentData.Where(x => x.EndDate >= model.StartDate && (x.StoreId == 0 || x.StoreId == model.StoreId)).Count();
                    }
                }
                else
                {
                    if (model.StoreId == 0)
                    {
                        curCount = currentData.Where(x => x.EndDate >= model.StartDate && x.Id != model.Id).Count();

                    }
                    else
                    {
                        curCount = currentData.Where(x => x.EndDate >= model.StartDate && x.Id != model.Id && (x.StoreId == 0 || x.StoreId == model.StoreId)).Count();
                    }
                }
            }

            if (curCount > 0)
            {
                throw new Exception("Already Exists!");
            }

            List<DiscountVouchareItems> items = new List<DiscountVouchareItems>();


            if (model.Id == 0)
            {
                var info = new DiscountVouchares();
                info.StoreId = model.StoreId;
                //info.MinAmount = model.MinAmount;
                //info.MaxAmount = model.MinAmount;
                //info.Amount = model.Amount;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                info.Approved = model.Approved;
                info.CratedDate = DateTime.Now;
                info.CreatedBy = User.Identity.Name;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _discountVouchersService.CreateAsync(info);
                insertId = data.Id;

                for (int i = 0; i < itemIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(endAmounts[i]))
                    {
                        itemId = int.Parse(itemIds[i]);
                        amount1 = decimal.Parse(endAmounts[i]);
                        amount2 = decimal.Parse(voucherAmounts[i]);

                        if (amount1 >= 0 && amount2 >= 0)
                        {
                            items.Add(new DiscountVouchareItems
                            {
                                DiscountVoucherId = insertId,
                                MaxAmount = amount1,
                                Amount = amount2,
                            });
                        }
                    }
                    
                }
                await _discountVouchersService.CreateItemsAsync(items);
            }
            else
            {
                var info = await _discountVouchersService.GetByIdAsync(model.Id);
                info.StoreId = model.StoreId;
                info.StoreId = model.StoreId;
                info.IsMembersOnly = model.IsMembersOnly;
                info.StartDate = model.StartDate;
                info.EndDate = model.EndDate;
                info.Approved = model.Approved;
                info.UpdatedDate = DateTime.Now;
                info.UpdatedBy = User.Identity.Name;

                var data = await _discountVouchersService.UpdateAsync(info);
                insertId = data.Id;

                for (int i = 0; i < itemIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(endAmounts[i]))
                    {
                        itemId = int.Parse(itemIds[i]);
                        amount1 = decimal.Parse(endAmounts[i]);
                        amount2 = decimal.Parse(voucherAmounts[i]);

                        if (amount1 >= 0 && amount2 >= 0)
                        {
                            items.Add(new DiscountVouchareItems
                            {
                                Id = itemId,
                                DiscountVoucherId = insertId,
                                MaxAmount = amount1,
                                Amount = amount2,
                            });
                        }
                    }
                }
                await _discountVouchersService.UpdateItemsAsync(items);
            }

            return new JsonResult(new { id = insertId });
        }


        [Route("/Merchant/GetVoucherDiscountInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetVoucherDiscountInfo(int id)
        {
            var info = await _discountVouchersService.GetByIdAsync(id);
            var model = new DetailsViewModel();
            model.Id = id;
            model.StoreId = info.StoreId;
            model.IsMembersOnly = info.IsMembersOnly == true ? "true" : "false";
            model.StartDate = info.StartDate.ToString("yyyy-MM-dd");
            model.EndDate = info.EndDate.ToString("yyyy-MM-dd");
            model.Approved = info.Approved == true ? "true" : "false";

            return Json(data: model);

        }

        [Route("/Merchant/GetVoucherDiscountItemInfo/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetVoucherDiscountItemInfo(int id)
        {
            var info = await _discountVouchersService.GetAllItemsByIdAsync(id);
            List<DiscountVouchareItems> model = new List<DiscountVouchareItems>();
            foreach(var item in info)
            {
                model.Add(new DiscountVouchareItems
                {
                    Id = item.Id,
                    DiscountVoucherId= item.DiscountVoucherId,
                    MaxAmount= item.MaxAmount,
                    Amount= item.Amount,
                });
            }

            return Json(data: model);

        }


        [Route("/Merchant/DeleteVoucherDiscount/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteVoucherDiscount(int id)
        {
            var items = await _discountVouchersService.GetAllItemsByIdAsync(id);
            await _discountVouchersService.DeleteItemsAsync(items);
            var info = await _discountVouchersService.GetByIdAsync(id);
            await _discountVouchersService.DeleteAsync(info);
            return new JsonResult(new { id = id });

        }
    }
}
