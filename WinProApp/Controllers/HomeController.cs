using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels;
using WinProApp.ViewModels.SupplierPurchase;

namespace WinProApp.Controllers
{
    public class HomeController : BasedUserController
    {
        private readonly ILogger<HomeController> _logger;
        public readonly PurchaseService _purchaseService;
        public readonly SupplierPurchaseService _supplierPurchaseService;
        public readonly PurchaseRecieptService _purchaseRecieptService;
        public readonly RequestForInfoService _requestForInfoService;
        public readonly RequestForQuotationService _requestForQuotationService;
        public readonly RequestForPurchaseService _requestForPurchaseService;
        public readonly PurchaseOrderService _purchaseOrderService;
        public readonly CommonService _commonService;
        public readonly ProFormaInvoiceService _proFormaInvoiceService;
        public readonly ShippingService _shippingService;
        public readonly DamageReturnService _damageReturnService;
        public readonly SupplierReturnService _supplierReturnService;
        public readonly IbtInfoService _ibtInfoService;
        public readonly GrnInfoService _grnInfoService;

       public HomeController(ILogger<HomeController> logger, 
           PurchaseService purchaseService, 
           SupplierPurchaseService supplierPurchaseService, 
           PurchaseRecieptService purchaseRecieptService, 
           RequestForInfoService requestForInfoService, 
           RequestForQuotationService requestForQuotationService, 
           RequestForPurchaseService requestForPurchaseService, 
           PurchaseOrderService purchaseOrderService, 
           CommonService commonService, 
           ProFormaInvoiceService proFormaInvoiceService,
           ShippingService shippingService,
           DamageReturnService damageReturnService,
           SupplierReturnService supplierReturnService,
           IbtInfoService ibtInfoService,
          GrnInfoService grnInfoService
           )
        {
            _logger = logger;
            _purchaseService = purchaseService;
            _supplierPurchaseService = supplierPurchaseService;
            _purchaseRecieptService = purchaseRecieptService;
            _requestForInfoService = requestForInfoService;
            _requestForQuotationService = requestForQuotationService;
            _requestForPurchaseService = requestForPurchaseService;
            _purchaseOrderService = purchaseOrderService;
            _commonService = commonService;
            _proFormaInvoiceService = proFormaInvoiceService;
            _shippingService = shippingService;
            _damageReturnService = damageReturnService;
            _supplierReturnService = supplierReturnService;
            _ibtInfoService = ibtInfoService;
            _grnInfoService= grnInfoService;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/Administrator")]
        public IActionResult AdminDashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/Purchasing")]
        public async Task<IActionResult> PurchasingDashboard()
        {
            var purchaseInfo = await _supplierPurchaseService.GetByAllAsync();
           // decimal totToday = purchaseInfo.Where(x=> x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Select(x=>x.Total).Sum()??0;
            int purchaseCount = purchaseInfo.Where(x => x.Status==true && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();
            ViewBag.TotalToday = purchaseCount;

            var shippingInfo = await _shippingService.GetByAllAsync();
            decimal totShipping = shippingInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day && x.Status == true).Select(x => x.Total).Sum() ?? 0;
            int shippingCount = shippingInfo.Count();
            ViewBag.totShipping = shippingCount;

            var supplierInfo = await _purchaseService.GetAllSuppliersAsync();
            ViewBag.supplierCount = supplierInfo.Count();

            var rfiinfo = await _requestForInfoService.GetByAllAsync();
            ViewBag.rfiinfo = rfiinfo.Where(x=>x.ReqDate.Year == DateTime.Now.Year && x.ReqDate.Month==DateTime.Now.Month && x.ReqDate.Day == DateTime.Now.Day).Count();

            var rfqInfo = await _requestForQuotationService.GetByAllAsync();
            ViewBag.rfqInfo = rfqInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();

            var prInfo = await _requestForPurchaseService.GetByAllAsync();
            ViewBag.prInfo = prInfo.Where(x=> x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();

            var poInfo = await _purchaseOrderService.GetByAllAsync();
            ViewBag.poInfo = poInfo.Where(x=> x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();

            var returnInfo = await _supplierReturnService.GetByAllAsync();
            ViewBag.returnInfo = returnInfo.Where(x=> x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();

            var damageInfo = await _damageReturnService.GetByAllAsync();
            ViewBag.damageInfo = damageInfo.Where(x=> x.Date.Value.Year == DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Day == DateTime.Now.Day).Count();

            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/Warehouse")]
        public async Task<IActionResult> WarehouseDashboard()
        {
            var ibtInfo = await _ibtInfoService.GetByAllAsync();
            var grnService = await _grnInfoService.GetByAllAsync();
            var returnInfo = await _supplierReturnService.GetByAllAsync();
            var damageInfo = await _damageReturnService.GetByAllAsync();

            ViewBag.IBTInfo = ibtInfo.Where(x=> x.Status==true && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();
            ViewBag.GRNInfo = grnService.Where(x => x.Status==true && x.Status == true && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();
            ViewBag.returnInfo = returnInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).Count();
            ViewBag.damageInfo = damageInfo.Where(x => x.Date.Value.Year == DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Day == DateTime.Now.Day).Count();

            return View();
        }

        [Route("/Finance")]
        public IActionResult Finance()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/HRMS")]
        public IActionResult HRMS()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/Ecommerce")]
        public IActionResult Ecommerce()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }


        [Route("/CRM")]
        public IActionResult CRM()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/POS")]
        public IActionResult POS()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }


        [Route("/Purchase/GetPurchasingInfoByDate")]
        [HttpPost]
        public async Task<IActionResult> GetPurchasingInfoByDate(DateTime? fromDt, DateTime? toDt)
        {
            var model = new PurchaseDashBoardViewModel();
            var supplierInfo = await _purchaseService.GetAllSuppliersAsync();
            var purchaseInfo = await _supplierPurchaseService.GetByAllAsync();
            var shippingInfo = await _shippingService.GetByAllAsync();
            var rfiinfo = await _requestForInfoService.GetByAllAsync();
            var rfqInfo = await _requestForQuotationService.GetByAllAsync();
            var prInfo = await _requestForPurchaseService.GetByAllAsync();
            var poInfo = await _purchaseOrderService.GetByAllAsync();
            var returnInfo = await _supplierReturnService.GetByAllAsync();
            var damageInfo = await _damageReturnService.GetByAllAsync();

            int purchaseCount = 0;
            int shippingCount = 0;
            int RFICount = 0;
            int RFQCount = 0;
            int PRCount = 0;
            int POCount = 0;
            int ReturnCount = 0;
            int DamageCount = 0;


            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                purchaseInfo = purchaseInfo.Where(x=> x.Status == true && x.Date >= dt1 && x.Date <= dt2).ToList();
                shippingInfo = shippingInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                rfiinfo = rfiinfo.Where(x => x.ReqDate >= dt1 && x.ReqDate <= dt2).ToList();
                rfqInfo = rfqInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                prInfo = prInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                poInfo = poInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                returnInfo = returnInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                damageInfo = damageInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
            }
            else
            {
                if(fromDt != null && toDt == null)
                {
                    DateTime dt1 = fromDt.Value;

                    purchaseInfo = purchaseInfo.Where(x => x.Status == true && x.Date >= dt1).ToList();
                    shippingInfo = shippingInfo.Where(x => x.Date >= dt1).ToList();
                    rfiinfo = rfiinfo.Where(x => x.ReqDate >= dt1).ToList();
                    rfqInfo = rfqInfo.Where(x => x.Date >= dt1).ToList();
                    prInfo = prInfo.Where(x => x.Date >= dt1).ToList();
                    poInfo = poInfo.Where(x => x.Date >= dt1).ToList();
                    returnInfo = returnInfo.Where(x => x.Date >= dt1).ToList();
                    damageInfo = damageInfo.Where(x => x.Date >= dt1).ToList();
                }
                else
                {
                    if (fromDt == null && toDt != null)
                    {
                        DateTime dt2 = toDt.Value;

                        purchaseInfo = purchaseInfo.Where(x => x.Status == true && x.Date <= dt2).ToList();
                        shippingInfo = shippingInfo.Where(x => x.Date <= dt2).ToList();
                        rfiinfo = rfiinfo.Where(x => x.ReqDate <= dt2).ToList();
                        rfqInfo = rfqInfo.Where(x => x.Date <= dt2).ToList();
                        prInfo = prInfo.Where(x => x.Date <= dt2).ToList();
                        poInfo = poInfo.Where(x => x.Date <= dt2).ToList();
                        returnInfo = returnInfo.Where(x => x.Date <= dt2).ToList();
                        damageInfo = damageInfo.Where(x => x.Date <= dt2).ToList();
                    }
                    else
                    {
                        purchaseInfo = purchaseInfo.Where(x => x.Status == true && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day==DateTime.Now.Day).ToList();
                        shippingInfo = shippingInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        rfiinfo = rfiinfo.Where(x => x.ReqDate.Year == DateTime.Now.Year && x.ReqDate.Month == DateTime.Now.Month && x.ReqDate.Day == DateTime.Now.Day).ToList();
                        rfqInfo = rfqInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        prInfo = prInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        poInfo = poInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        returnInfo = returnInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        damageInfo = damageInfo.Where(x => x.Date.Value.Year == DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Day == DateTime.Now.Day).ToList();
                    }
                }
            }

            model.SupplierCount = supplierInfo.Count();
            model.PurchaseCount = purchaseInfo.Count();
            model.ShipmentCount = shippingInfo.Count();
            model.RFICount = rfiinfo.Count();
            model.RFQCount = rfqInfo.Count();
            model.PRCount= prInfo.Count();
            model.POCount= poInfo.Count();
            model.ReturnCount= returnInfo.Count();
            model.DamageCount= damageInfo.Count();

            return Json(data:model);
        }


        [Route("/Purchase/GetWarehouseInfoByDate")]
        [HttpPost]
        public async Task<IActionResult> GetWarehouseInfoByDate(DateTime? fromDt, DateTime? toDt)
        {
            var model = new WarehouseDashBoardViewModel();

            var ibtInfo = await _ibtInfoService.GetByAllAsync();
            var grnInfo = await _grnInfoService.GetByAllAsync();
            var returnInfo = await _supplierReturnService.GetByAllAsync();
            var damageInfo = await _damageReturnService.GetByAllAsync();


            if (fromDt != null && toDt != null)
            {
                DateTime dt1 = fromDt.Value;
                DateTime dt2 = toDt.Value;

                ibtInfo = ibtInfo.Where(x => x.Status == true && x.Date >= dt1 && x.Date <= dt2).ToList();
                grnInfo = grnInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                returnInfo = returnInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
                damageInfo = damageInfo.Where(x => x.Date >= dt1 && x.Date <= dt2).ToList();
            }
            else
            {
                if (fromDt != null && toDt == null)
                {
                    DateTime dt1 = fromDt.Value;

                    ibtInfo = ibtInfo.Where(x => x.Status == true && x.Date >= dt1).ToList();
                    grnInfo = grnInfo.Where(x => x.Date >= dt1).ToList();
                    returnInfo = returnInfo.Where(x => x.Date >= dt1).ToList();
                    damageInfo = damageInfo.Where(x => x.Date >= dt1).ToList();
                }
                else
                {
                    if (fromDt == null && toDt != null)
                    {
                        DateTime dt2 = toDt.Value;

                        ibtInfo = ibtInfo.Where(x => x.Status == true && x.Date <= dt2).ToList();
                        grnInfo = grnInfo.Where(x => x.Date <= dt2).ToList();
                        returnInfo = returnInfo.Where(x => x.Date <= dt2).ToList();
                        damageInfo = damageInfo.Where(x => x.Date <= dt2).ToList();
                    }
                    else
                    {
                        ibtInfo = ibtInfo.Where(x => x.Status == true && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        grnInfo = grnInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        returnInfo = returnInfo.Where(x => x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
                        damageInfo = damageInfo.Where(x => x.Date.Value.Year == DateTime.Now.Year && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Day == DateTime.Now.Day).ToList();
                    }
                }
            }

            model.PromotionCount = 0;
            model.TotalStock = 0;
            model.IBTCount = ibtInfo.Count();
            model.GRNCount = grnInfo.Count();
            model.ReturnCount = returnInfo.Count();
            model.DamageCount = damageInfo.Count();



            return Json(data:model);
        }


        [Route("/Merchant")]
        public IActionResult Merchant()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        public IActionResult Index2()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            return View();
        }

        [Route("/Identity/Account/Login")]
        public IActionResult AccountLogin()
        {
            return Redirect("/Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}