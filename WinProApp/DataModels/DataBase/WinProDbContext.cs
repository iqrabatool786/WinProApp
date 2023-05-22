using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WinProApp.Areas.Identity.Data;
using WinProApp.DataModels.DataBase.StoredProcedures;

namespace WinProApp.DataModels.DataBase
{
    public partial class WinProDbContext : DbContext
    {
        public WinProDbContext()
        {
        }

        public WinProDbContext(DbContextOptions<WinProDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProFormaInvoice> ProFormaInvoice { get; set; } = null!;
        public virtual DbSet<ProFormaInvoiceItems> ProFormaInvoiceItems { get; set; } = null!;
        public virtual DbSet<PurchaseOrders> PurchaseOrders { get; set; } = null!;
        public virtual DbSet<PurchaseOrderItems> PurchaseOrderItems { get; set; } = null!;
        public virtual DbSet<RequestForInformation> RequestForInformation { get; set; } = null!;
        public virtual DbSet<RequestForInformationItem> RequestForInformationItems { get; set; } = null!;
        public virtual DbSet<RequestForPurchases> RequestForPurchases { get; set; } = null!;
        public virtual DbSet<RequestForPurchaseItem> RequestForPurchaseItems { get; set; } = null!;
        public virtual DbSet<RequestForQuotation> RequestForQuotation { get; set; } = null!;
        public virtual DbSet<RequestForQuotationItem> RequestForQuotationItems { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Vat> Vat { get; set; } = null!;
        public virtual DbSet<Store> Store { get; set; } = null!;
        public virtual DbSet<Register> Register { get; set; } = null!;
        public virtual DbSet<SalesTypes> SalesTypes { get; set; } = null!;
        public virtual DbSet<TransactionTypes> TransactionTypes { get; set; } = null!;
        public virtual DbSet<SupplierPurchase> SupplierPurchase { get; set; } = null!;
        public virtual DbSet<SupplierPurchaseDetails> SupplierPurchaseDetails { get; set; } = null!;
        public virtual DbSet<PurchaseReciept> PurchaseReciept { get; set; } = null!;
        public virtual DbSet<PurchaseRecieptDetails> PurchaseRecieptDetails { get; set; } = null!;
        public virtual DbSet<Customers> Customers { get; set; } = null!;

        public virtual DbSet<Categories> Categories { get; set; } = null!;
        public virtual DbSet<Colors> Colors { get; set; } = null!;
        public virtual DbSet<Departments> Departments { get; set; } = null!;
        public virtual DbSet<Sizes> Sizes { get; set; } = null!;
        public virtual DbSet<Styles> Styles { get; set; } = null!;

        public virtual DbSet<Brands> Brands { get; set; } = null!;
        public virtual DbSet<Descriptions> Descriptions { get; set; } = null!;
        public virtual DbSet<Groups> Groups { get; set; } = null!;
        public virtual DbSet<Seassons> Seassons { get; set; } = null!;
        public virtual DbSet<Units> Units { get; set; } = null!;
        public virtual DbSet<Vendors> Vendors { get; set; } = null!;
        public virtual DbSet<ProductInfo> ProductInfo { get; set; } = null!;
        public virtual DbSet<ProductPrice> ProductPrice { get; set; } = null!;
        public virtual DbSet<Years> Years { get; set; } = null!;

        public virtual DbSet<AssetTypes> AssetTypes { get; set; } = null!;
        public virtual DbSet<AssetLocation> AssetLocation { get; set; } = null!;
        public virtual DbSet<AssetDepartment> AssetDepartment { get; set; } = null!;
        public virtual DbSet<Asset> Asset { get; set; } = null!;
        public virtual DbSet<AssetAssign> AssetAssign { get; set; } = null!;

        public virtual DbSet<ReportHeads> ReportHeads { get; set; } = null!;
        public virtual DbSet<SupplierReturn> SupplierReturn { get; set; } = null!;
        public virtual DbSet<SupplierReturnDetail> SupplierReturnDetail { get; set; } = null!;
        public virtual DbSet<DamageReturn> DamageReturn { get; set; } = null!;
        public virtual DbSet<DamageReturnDetail> DamageReturnDetail { get; set; } = null!;
        public virtual DbSet<SupplierPurchaseAsset> SupplierPurchaseAsset { get; set; } = null!;
        public virtual DbSet<SupplierPurchaseAssetDetails> SupplierPurchaseAssetDetails { get; set; } = null!;
        public virtual DbSet<ShippingInfo> ShippingInfo { get; set; } = null!;
        public virtual DbSet<ShippingDetails> ShippingDetails { get; set; } = null!;
        public virtual DbSet<IbtInfo> IbtInfo { get; set; } = null!;
        public virtual DbSet<IbtDetails> IbtDetails { get; set; } = null!;
        public virtual DbSet<GrnInfo> GrnInfo { get; set; } = null!;
        public virtual DbSet<GrnDetails> GrnDetails { get; set; } = null!;
        public virtual DbSet<tblStock> tblStock { get; set; } = null!;
        public virtual DbSet<FlatDiscount> FlatDiscount { get; set; } = null!;
        public virtual DbSet<SelectedItemDiscount> SelectedItemDiscount { get; set; } = null!;
        public virtual DbSet<SelectedItemDiscountItems> SelectedItemDiscountItems { get; set; } = null!;
        public virtual DbSet<BuyGetQtyDiscount> BuyGetQtyDiscount { get; set; } = null!;
        public virtual DbSet<BuyGetLowestDiscount> BuyGetLowestDiscount { get; set; } = null!;
        public virtual DbSet<BuyQtyGetPrecentageOffDiscount> BuyQtyGetPrecentageOffDiscount { get; set; } = null!;
        public virtual DbSet<BuyQtyGetAmountOffDiscount> BuyQtyGetAmountOffDiscount { get; set; } = null!;
        public virtual DbSet<DiscountVouchares> DiscountVouchares { get; set; } = null!;
        public virtual DbSet<DiscountVouchareItems> DiscountVouchareItems { get; set; } = null!;
        public virtual DbSet<LoyaltyCardsInfo> LoyaltyCardsInfo { get; set; } = null!;
        public virtual DbSet<LoyaityPointInfo> LoyaityPointInfo { get; set; } = null!;
        public virtual DbSet<LoyaltyPointSystem> LoyaltyPointSystem { get; set; } = null!;


        public virtual DbSet<GetSupliers> GetSupliers { get; set; } = null!;
        public virtual DbSet<GetRequestForInformation> GetRequestForInformation { get; set; } = null!;
        public virtual DbSet<GetRequestForQuotation> GetRequestForQuotation { get; set; } = null!;
        public virtual DbSet<GetRequestForPurchase> GetRequestForPurchase { get; set; } = null!;
        public virtual DbSet<GetPurchaseOrder> GetPurchaseOrder { get; set; } = null!;
        public virtual DbSet<GetProFormaInvoice> GetProFormaInvoice { get; set; } = null!;
        public virtual DbSet<GetStore> GetStore { get; set; } = null!;
        public virtual DbSet<GetRegister> GetRegister { get; set; } = null!;
        public virtual DbSet<GetSupplierPurchase> GetSupplierPurchase { get; set; } = null!;
        public virtual DbSet<GetPurchaseReciept> GetPurchaseReciept { get; set; } = null!;
        public virtual DbSet<GetCustomers> GetCustomers { get; set; } = null!;
        public virtual DbSet<GetCategories> GetCategories { get; set; } = null!;
        public virtual DbSet<GetColors> GetColors { get; set; } = null!;
        public virtual DbSet<GetDepartments> GetDepartments { get; set; } = null!;
        public virtual DbSet<GetSizes> GetSizes { get; set; } = null!;
        public virtual DbSet<GetStyles> GetStyles { get; set; } = null!;

        public virtual DbSet<GetBrands> GetBrands { get; set; } = null!;
        public virtual DbSet<GetDescriptions> GetDescriptions { get; set; } = null!;
        public virtual DbSet<GetGroups> GetGroups { get; set; } = null!;
        public virtual DbSet<GetSeassons> GetSeassons { get; set; } = null!;
        public virtual DbSet<GetUnits> GetUnits { get; set; } = null!;
        public virtual DbSet<GetVendors> GetVendors { get; set; } = null!;
        public virtual DbSet<GetProductInfo> GetProductInfo { get; set; } = null!;


        public virtual DbSet<GetAssetTypes> GetAssetTypes { get; set; } = null!;
        public virtual DbSet<GetAssetLocation> GetAssetLocation { get; set; } = null!;
        public virtual DbSet<GetAssetDepartment> GetAssetDepartment { get; set; } = null!;
        public virtual DbSet<GetAsset> GetAsset { get; set; } = null!;
        public virtual DbSet<GetAssetAssign> GetAssetAssign { get; set; } = null!;

        public virtual DbSet<GetReportHeads> GetReportHeads { get; set; } = null!;

        public virtual DbSet<GetSupplierReturn> GetSupplierReturn { get; set; } = null!;
        public virtual DbSet<GetDamageReturn> GetDamageReturn { get; set; } = null!;
        public virtual DbSet<GetSupplierPurchaseAsset> GetSupplierPurchaseAsset { get; set; } = null!;
        public virtual DbSet<GetShippingInfo> GetShippingInfo { get; set; } = null!;
        public virtual DbSet<GetIbtInfo> GetIbtInfo { get; set; } = null!;
        public virtual DbSet<GetGrnInfo> GetGrnInfo { get; set; } = null!;
        public virtual DbSet<GetFlatDiscount> GetFlatDiscount { get; set; } = null!;
        public virtual DbSet<GetSelectedItemDiscount> GetSelectedItemDiscount { get; set; } = null!;
        public virtual DbSet<GetBuyGetQtyDiscount> GetBuyGetQtyDiscount { get; set; } = null!;
        public virtual DbSet<GetBuyGetLowestDiscount> GetBuyGetLowestDiscount { get; set; } = null!;
        public virtual DbSet<GetBuyQtyGetPrecentageOffDiscount> GetBuyQtyGetPrecentageOffDiscount { get; set; } = null!;
        public virtual DbSet<GetBuyQtyGetAmountOffDiscount> GetBuyQtyGetAmountOffDiscount { get; set; } = null!;
        public virtual DbSet<GetDiscountVouchares> GetDiscountVouchares { get; set; } = null!;
        public virtual DbSet<GetLoyaltyCardsInfo> GetLoyaltyCardsInfo { get; set; } = null!;
        public virtual DbSet<GetLoyaityPointInfo> GetLoyaityPointInfo { get; set; } = null!;
        public virtual DbSet<Company> Company { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                string connString = builder.Build().GetSection("ConnectionStrings").GetSection("WinProAppContextConnection").Value;
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var decimalProps = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
