using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.Services.Domain;
using WinProApp.Models;
using WinProApp.ViewModels.Loyalty.CardInfo;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WinProApp.Controllers.Loyalty
{
    [Authorize(Roles = "Administrator,Warehouse,Purchase")]
    public class LoyaltyCardsController : BasedUserController
    {
        public readonly LoyaltyCardsInfoService _loyaltyCardsInfoService;

        public LoyaltyCardsController(LoyaltyCardsInfoService loyaltyCardsInfoService)
        {
            _loyaltyCardsInfoService = loyaltyCardsInfoService;
        }

        [Route("/Merchant/NewCardIssue")]
        [HttpGet]
        public async Task<IActionResult> NewCardIssue()
        {
            return View();
        }

        [Route("/Merchant/GetWithoutCardCustomersList")]
        [HttpPost]
        public IActionResult GetWithoutCardCustomersList(JQueryDataTableParamModel param)
        {
            var results = _loyaltyCardsInfoService.GetCustomersWithoutCardList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                FullName= x.FullName,
                Address= x.Address,
                MobileNumber= x.MobileNumber,
                Email= x.Email,                
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

        [Route("/Merchant/NewCardCreate/{id}")]
        [HttpPost]
        public async Task<IActionResult> NewCardCreate(long id)
        {
            var card = new LoyaltyCardsInfo();
            card.CustomerId= id;
            card.Balance= 0;
            card.CratedDate = DateTime.Now;
            card.CreatedBy = User.Identity.Name;
            card.UpdatedDate = DateTime.Now;
            card.UpdatedBy = User.Identity.Name;

            var data = await _loyaltyCardsInfoService.CreateAsync(card);

            return new JsonResult(new { id = data.Id });
        }


        [Route("/Merchant/PointBalance")]
        [HttpGet]
        public async Task<IActionResult> PointBalance()
        {
            return View();
        }

        [Route("/Merchant/GetCustomersPointBalanceList")]
        [HttpPost]
        public IActionResult GetCustomersPointBalanceList(JQueryDataTableParamModel param)
        {
            var results = _loyaltyCardsInfoService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                CustomerId= x.CustomerId,
                FullName = x.FullName,
                Address = x.Address,
                MobileNumber = x.MobileNumber,
                Email = x.Email,
                Balance= x.Balance,
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


        [Route("/Merchant/PointSystem")]
        [HttpGet]
        public async Task<IActionResult> PointSystem()
        {
            var info = await _loyaltyCardsInfoService.GetByAllPointsAsync();
            ViewBag.Id = 0;
            ViewBag.Amount = "";
            ViewBag.Points = "";
            if(info.Count() > 0)
            {
                var info1 = info.FirstOrDefault();
                ViewBag.Id = info1.Id;
                ViewBag.Amount = info1.Amount;
                ViewBag.Points = info1.Points;
            }
            return View();
        }


        [Route("/Merchant/PointSystemUpdate")]
        [HttpPost]
        public async Task<IActionResult> PointSystemUpdate(AddEditPointSysViewModel model)
        {
            int curId = model.Id;
            if(model.Id== 0)
            {
                var info = new LoyaltyPointSystem();
                info.Amount= model.Amount;
                info.Points = model.Points;

                var data = await _loyaltyCardsInfoService.CreatePointSystemAsync(info);
                curId = data.Id;
            }
            {
                var info = await _loyaltyCardsInfoService.GetPointSystemByIdAsync(curId);
                info.Id= curId;
                info.Amount = model.Amount;
                info.Points = model.Points;

                await _loyaltyCardsInfoService.UpdatePointSystemAsync(info);
            }

            return new JsonResult(new { id = curId });
        }

    }
}
