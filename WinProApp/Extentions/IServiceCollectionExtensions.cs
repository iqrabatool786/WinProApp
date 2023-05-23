using WinProApp.Services.Domain;
using Microsoft.Extensions.DependencyInjection;
using WinProApp.Services.Domain.AssetSection;

namespace WinProApp.Extentions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<CommonService>();
            services.AddScoped<PurchaseService>();
            services.AddScoped<RequestForInfoService>();
            services.AddScoped<RequestForQuotationService>();
            services.AddScoped<RequestForPurchaseService>();
            services.AddScoped<PurchaseOrderService>();
            services.AddScoped<ProFormaInvoiceService>();
            services.AddScoped<StoreService>();
            services.AddScoped<RegisterService>();
            services.AddScoped<SupplierPurchaseService>();
            services.AddScoped<CorporateEmployeesServices>();
            services.AddScoped<PurchaseRecieptService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<CategoryServices>();
            services.AddScoped<ColorServices>();
            services.AddScoped<DepartmentServices>();
            services.AddScoped<SizeServices>();
            services.AddScoped<StyleServices>();
            services.AddScoped<BrandService>();
            services.AddScoped<DescriptionService>();
            services.AddScoped<GroupService>();
            services.AddScoped<SeassonService>();
            services.AddScoped<UnitService>();
            services.AddScoped<VendorService>();
            services.AddScoped<YearsService>();
            services.AddScoped<ProductInfoService>();
            services.AddScoped<AssetService>();
            services.AddScoped<AssetAssignService>();
            services.AddScoped<AssetDepartmentService>();
            services.AddScoped<AssetLocationService>();
            services.AddScoped<AssetTypesService>();
            services.AddScoped<ReportHeadService>();
            services.AddScoped<SupplierReturnService>();
            services.AddScoped<DamageReturnService>();
            services.AddScoped<PurchaseAssetService>();
            services.AddScoped<ShippingService>();
            services.AddScoped<IbtInfoService>();
            services.AddScoped<GrnInfoService>();
            services.AddScoped<FlatDiscountService>();
            services.AddScoped<SelectedItemDiscountService>();
            services.AddScoped<BuyGetQtyDiscountService>();
            services.AddScoped<BuyGetLowestDiscountService>();
            services.AddScoped<BuyQtyGetPrecentageOffDiscountService>();
            services.AddScoped<BuyQtyGetAmountOffDiscountService>();
            services.AddScoped<DiscountVouchersService>();
            services.AddScoped<LoyaltyCardsInfoService>();
            services.AddScoped<SalesService>();
        }
    }
}
