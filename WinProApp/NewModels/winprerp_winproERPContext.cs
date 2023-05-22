using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WinProApp.NewModels
{
    public partial class winprerp_winproERPContext : DbContext
    {
        public winprerp_winproERPContext()
        {
        }

        public winprerp_winproERPContext(DbContextOptions<winprerp_winproERPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<AssetAssign> AssetAssigns { get; set; } = null!;
        public virtual DbSet<AssetDepartment> AssetDepartments { get; set; } = null!;
        public virtual DbSet<AssetLocation> AssetLocations { get; set; } = null!;
        public virtual DbSet<AssetType> AssetTypes { get; set; } = null!;
        public virtual DbSet<BankInfo> BankInfos { get; set; } = null!;
        public virtual DbSet<BankPaymentReciept> BankPaymentReciepts { get; set; } = null!;
        public virtual DbSet<BankReconcile> BankReconciles { get; set; } = null!;
        public virtual DbSet<Boxopen> Boxopens { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<BuyGetLowestDiscount> BuyGetLowestDiscounts { get; set; } = null!;
        public virtual DbSet<BuyGetQtyDiscount> BuyGetQtyDiscounts { get; set; } = null!;
        public virtual DbSet<BuyQtyGetAmountOffDiscount> BuyQtyGetAmountOffDiscounts { get; set; } = null!;
        public virtual DbSet<BuyQtyGetPrecentageOffDiscount> BuyQtyGetPrecentageOffDiscounts { get; set; } = null!;
        public virtual DbSet<CashBox> CashBoxes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ChartOfAccount> ChartOfAccounts { get; set; } = null!;
        public virtual DbSet<ClosingHistory> ClosingHistories { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<CurrentStore> CurrentStores { get; set; } = null!;
        public virtual DbSet<CustomAgent> CustomAgents { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerCollection> CustomerCollections { get; set; } = null!;
        public virtual DbSet<CustomerCollectionDetail> CustomerCollectionDetails { get; set; } = null!;
        public virtual DbSet<DamageReturn> DamageReturns { get; set; } = null!;
        public virtual DbSet<DamageReturnDetail> DamageReturnDetails { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Description> Descriptions { get; set; } = null!;
        public virtual DbSet<DiscountVouchare> DiscountVouchares { get; set; } = null!;
        public virtual DbSet<DiscountVouchareItem> DiscountVouchareItems { get; set; } = null!;
        public virtual DbSet<FlatDiscount> FlatDiscounts { get; set; } = null!;
        public virtual DbSet<GrnDetail> GrnDetails { get; set; } = null!;
        public virtual DbSet<GrnInfo> GrnInfos { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<GroupOfCompany> GroupOfCompanies { get; set; } = null!;
        public virtual DbSet<IbtDetail> IbtDetails { get; set; } = null!;
        public virtual DbSet<IbtInfo> IbtInfos { get; set; } = null!;
        public virtual DbSet<ImportCoa> ImportCoas { get; set; } = null!;
        public virtual DbSet<Invoice> Invoices { get; set; } = null!;
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
        public virtual DbSet<InvoiceDetailPosTemp> InvoiceDetailPosTemps { get; set; } = null!;
        public virtual DbSet<InvoiceDetailReplicated> InvoiceDetailReplicateds { get; set; } = null!;
        public virtual DbSet<InvoiceDetailReplicatedTemp> InvoiceDetailReplicatedTemps { get; set; } = null!;
        public virtual DbSet<InvoiceHoldDetail> InvoiceHoldDetails { get; set; } = null!;
        public virtual DbSet<InvoiceHoldMaster> InvoiceHoldMasters { get; set; } = null!;
        public virtual DbSet<InvoicePosTemp> InvoicePosTemps { get; set; } = null!;
        public virtual DbSet<LoyaityPointInfo> LoyaityPointInfos { get; set; } = null!;
        public virtual DbSet<LoyaltyCardsInfo> LoyaltyCardsInfos { get; set; } = null!;
        public virtual DbSet<LoyaltyPointSystem> LoyaltyPointSystems { get; set; } = null!;
        public virtual DbSet<ProFormaInvoice> ProFormaInvoices { get; set; } = null!;
        public virtual DbSet<ProFormaInvoiceItem> ProFormaInvoiceItems { get; set; } = null!;
        public virtual DbSet<ProductInfo> ProductInfos { get; set; } = null!;
        public virtual DbSet<ProductPrice> ProductPrices { get; set; } = null!;
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
        public virtual DbSet<PurchaseReciept> PurchaseReciepts { get; set; } = null!;
        public virtual DbSet<PurchaseRecieptDetail> PurchaseRecieptDetails { get; set; } = null!;
        public virtual DbSet<QuotationDetail> QuotationDetails { get; set; } = null!;
        public virtual DbSet<QuotationMaster> QuotationMasters { get; set; } = null!;
        public virtual DbSet<Register> Registers { get; set; } = null!;
        public virtual DbSet<ReportHead> ReportHeads { get; set; } = null!;
        public virtual DbSet<RequestForInformation> RequestForInformations { get; set; } = null!;
        public virtual DbSet<RequestForInformationItem> RequestForInformationItems { get; set; } = null!;
        public virtual DbSet<RequestForPurchase> RequestForPurchases { get; set; } = null!;
        public virtual DbSet<RequestForPurchaseItem> RequestForPurchaseItems { get; set; } = null!;
        public virtual DbSet<RequestForQuotation> RequestForQuotations { get; set; } = null!;
        public virtual DbSet<RequestForQuotationItem> RequestForQuotationItems { get; set; } = null!;
        public virtual DbSet<ReturnBarcodeImage> ReturnBarcodeImages { get; set; } = null!;
        public virtual DbSet<ReturnInvoice> ReturnInvoices { get; set; } = null!;
        public virtual DbSet<SalesInvoice> SalesInvoices { get; set; } = null!;
        public virtual DbSet<SalesInvoiceHold> SalesInvoiceHolds { get; set; } = null!;
        public virtual DbSet<SalesInvoiceHoldItem> SalesInvoiceHoldItems { get; set; } = null!;
        public virtual DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; } = null!;
        public virtual DbSet<SalesInvoiceReturnItem> SalesInvoiceReturnItems { get; set; } = null!;
        public virtual DbSet<SalesInvoiceVoucher> SalesInvoiceVouchers { get; set; } = null!;
        public virtual DbSet<SalesReturnInvoice> SalesReturnInvoices { get; set; } = null!;
        public virtual DbSet<SalesReturnInvoicePayment> SalesReturnInvoicePayments { get; set; } = null!;
        public virtual DbSet<SalesType> SalesTypes { get; set; } = null!;
        public virtual DbSet<Seasson> Seassons { get; set; } = null!;
        public virtual DbSet<SelectedItemDiscount> SelectedItemDiscounts { get; set; } = null!;
        public virtual DbSet<SelectedItemDiscountItem> SelectedItemDiscountItems { get; set; } = null!;
        public virtual DbSet<Shift> Shifts { get; set; } = null!;
        public virtual DbSet<ShippingDetail> ShippingDetails { get; set; } = null!;
        public virtual DbSet<ShippingInfo> ShippingInfos { get; set; } = null!;
        public virtual DbSet<Size> Sizes { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<Style> Styles { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<SupplierPurchase> SupplierPurchases { get; set; } = null!;
        public virtual DbSet<SupplierPurchaseAsset> SupplierPurchaseAssets { get; set; } = null!;
        public virtual DbSet<SupplierPurchaseAssetDetail> SupplierPurchaseAssetDetails { get; set; } = null!;
        public virtual DbSet<SupplierPurchaseDetail> SupplierPurchaseDetails { get; set; } = null!;
        public virtual DbSet<SupplierReturn> SupplierReturns { get; set; } = null!;
        public virtual DbSet<SupplierReturnDetail> SupplierReturnDetails { get; set; } = null!;
        public virtual DbSet<SystemModule> SystemModules { get; set; } = null!;
        public virtual DbSet<Systemmenu> Systemmenus { get; set; } = null!;
        public virtual DbSet<TblAccount> TblAccounts { get; set; } = null!;
        public virtual DbSet<TblChartofAccount> TblChartofAccounts { get; set; } = null!;
        public virtual DbSet<TblExpense> TblExpenses { get; set; } = null!;
        public virtual DbSet<TblFamilyAccount> TblFamilyAccounts { get; set; } = null!;
        public virtual DbSet<TblForm> TblForms { get; set; } = null!;
        public virtual DbSet<TblFormDetail> TblFormDetails { get; set; } = null!;
        public virtual DbSet<TblInsurance> TblInsurances { get; set; } = null!;
        public virtual DbSet<TblLedger> TblLedgers { get; set; } = null!;
        public virtual DbSet<TblProfression> TblProfressions { get; set; } = null!;
        public virtual DbSet<TblStock> TblStocks { get; set; } = null!;
        public virtual DbSet<TblUserPermission> TblUserPermissions { get; set; } = null!;
        public virtual DbSet<TemPrintingBarcode> TemPrintingBarcodes { get; set; } = null!;
        public virtual DbSet<TransactionType> TransactionTypes { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;
        public virtual DbSet<UserBalance> UserBalances { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;
        public virtual DbSet<UserInfoReturn> UserInfoReturns { get; set; } = null!;
        public virtual DbSet<Userright> Userrights { get; set; } = null!;
        public virtual DbSet<Vat> Vats { get; set; } = null!;
        public virtual DbSet<Vendor> Vendors { get; set; } = null!;
        public virtual DbSet<View1> View1s { get; set; } = null!;
        public virtual DbSet<VoidDetail> VoidDetails { get; set; } = null!;
        public virtual DbSet<VoidDetail1> VoidDetails1 { get; set; } = null!;
        public virtual DbSet<VoidMaster> VoidMasters { get; set; } = null!;
        public virtual DbSet<VoidMaster1> VoidMasters1 { get; set; } = null!;
        public virtual DbSet<VuCustomerReport> VuCustomerReports { get; set; } = null!;
        public virtual DbSet<VwServerDb> VwServerDbs { get; set; } = null!;
        public virtual DbSet<Year> Years { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=192.185.6.199;Database=winprerp_winproERP;User Id=winprerp_aziz1;Password=0@z4nId3;Trusted_Connection=False;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("winprerp_aziz1");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.ToTable("AspNetRoles", "dbo");

                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.ToTable("AspNetRoleClaims", "dbo");

                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.ToTable("AspNetUsers", "dbo");

                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(1),(0)))");

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles", "dbo");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.ToTable("AspNetUserClaims", "dbo");

                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.ToTable("AspNetUserLogins", "dbo");

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.ToTable("AspNetUserTokens", "dbo");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Asset", "dbo");

                entity.Property(e => e.AssetNameArabic).HasMaxLength(100);

                entity.Property(e => e.AssetNameEng).HasMaxLength(100);

                entity.Property(e => e.AssetValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.DesignationOfStaff).HasMaxLength(100);

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.ManufactureDate).HasColumnType("date");

                entity.Property(e => e.ManufactureName).HasMaxLength(50);

                entity.Property(e => e.Temp).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.WarrentyPeriod).HasMaxLength(50);
            });

            modelBuilder.Entity<AssetAssign>(entity =>
            {
                entity.ToTable("AssetAssign", "dbo");

                entity.Property(e => e.AssignTo).HasMaxLength(50);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AssetDepartment>(entity =>
            {
                entity.ToTable("AssetDepartment", "dbo");

                entity.Property(e => e.DepartmentArabic).HasMaxLength(50);

                entity.Property(e => e.DepartmentEng).HasMaxLength(50);
            });

            modelBuilder.Entity<AssetLocation>(entity =>
            {
                entity.ToTable("AssetLocation", "dbo");

                entity.Property(e => e.LocationArabic).HasMaxLength(50);

                entity.Property(e => e.LocationEng).HasMaxLength(50);
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.ToTable("AssetTypes", "dbo");

                entity.Property(e => e.AssetTypeArabic).HasMaxLength(50);

                entity.Property(e => e.AssetTypeEng).HasMaxLength(50);
            });

            modelBuilder.Entity<BankInfo>(entity =>
            {
                entity.ToTable("BankInfo", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BankName).HasMaxLength(50);
            });

            modelBuilder.Entity<BankPaymentReciept>(entity =>
            {
                entity.ToTable("BankPaymentReciept", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Purpose).HasMaxLength(100);

                entity.Property(e => e.ReferenceName).HasMaxLength(100);

                entity.Property(e => e.StoreId)
                    .HasMaxLength(50)
                    .HasColumnName("storeID");

                entity.Property(e => e.Storename).HasMaxLength(50);

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BankReconcile>(entity =>
            {
                entity.ToTable("BankReconcile", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Boxopen>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Boxopen", "winprerp_sa");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OpenCount)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("openCount");

                entity.Property(e => e.Posno)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("POSno");

                entity.Property(e => e.Storeid)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("storeid");

                entity.Property(e => e.UserId)
                    .HasMaxLength(4000)
                    .HasColumnName("UserID");

                entity.Property(e => e.Username).HasMaxLength(4000);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brands", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<BuyGetLowestDiscount>(entity =>
            {
                entity.ToTable("BuyGetLowestDiscount", "dbo");

                entity.Property(e => e.BuyBarcode).HasMaxLength(200);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.GetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GetProductBarcode).HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BuyGetQtyDiscount>(entity =>
            {
                entity.ToTable("BuyGetQtyDiscount", "dbo");

                entity.Property(e => e.BuyBarcode).HasMaxLength(200);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.GetProductBarcode).HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BuyQtyGetAmountOffDiscount>(entity =>
            {
                entity.ToTable("BuyQtyGetAmountOffDiscount", "dbo");

                entity.Property(e => e.BuyBarcode).HasMaxLength(200);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.OffAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BuyQtyGetPrecentageOffDiscount>(entity =>
            {
                entity.ToTable("BuyQtyGetPrecentageOffDiscount", "dbo");

                entity.Property(e => e.BuyBarcode).HasMaxLength(200);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.OffPrecentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CashBox>(entity =>
            {
                entity.ToTable("CashBox", "dbo");

                entity.Property(e => e.Bank).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Cash).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CloseAt).HasColumnType("datetime");

                entity.Property(e => e.CloseBank).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CloseCash).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CloseCredit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CloseOpeningBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Credit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CurrentIp)
                    .HasMaxLength(30)
                    .HasColumnName("CurrentIP");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OpenAt).HasColumnType("datetime");

                entity.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RadiumPoint).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxBank).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxCash).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Updated).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(e => e.Voucher).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<ChartOfAccount>(entity =>
            {
                entity.ToTable("ChartOfAccounts", "dbo");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AccountName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AccountType)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PaccountCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PAccountCode");
            });

            modelBuilder.Entity<ClosingHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Closing_History", "dbo");

                entity.Property(e => e.Cdate)
                    .HasColumnType("datetime")
                    .HasColumnName("cdate");

                entity.Property(e => e.Confirmedamountpaidcash).HasColumnName("confirmedamountpaidcash");

                entity.Property(e => e.ConfirmedcashtillopeningBalance).HasColumnName("confirmedcashtillopeningBalance");

                entity.Property(e => e.Deposited)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("deposited");

                entity.Property(e => e.Discount)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("discount");

                entity.Property(e => e.Donation).HasColumnName("donation");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("id");

                entity.Property(e => e.PostingCash).HasMaxLength(50);

                entity.Property(e => e.Postingstatus)
                    .HasMaxLength(50)
                    .HasColumnName("postingstatus");

                entity.Property(e => e.Registerid)
                    .HasMaxLength(50)
                    .HasColumnName("registerid");

                entity.Property(e => e.Storeid)
                    .HasMaxLength(50)
                    .HasColumnName("storeid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.VatBank).HasColumnName("Vat_bank");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Colors", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company", "dbo");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Bpcode)
                    .HasMaxLength(50)
                    .HasColumnName("BPcode");

                entity.Property(e => e.CompanyName).HasMaxLength(50);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.InsertBy).HasMaxLength(50);

                entity.Property(e => e.PhoneNo).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Website).HasMaxLength(50);
            });

            modelBuilder.Entity<CurrentStore>(entity =>
            {
                entity.ToTable("CurrentStore", "dbo");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Crno)
                    .HasMaxLength(50)
                    .HasColumnName("CRNo");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FooterA).HasMaxLength(4000);

                entity.Property(e => e.FooterE).HasMaxLength(4000);

                entity.Property(e => e.Logo)
                    .HasMaxLength(150)
                    .HasColumnName("logo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEnglish).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.StoreCode).HasMaxLength(50);

                entity.Property(e => e.VatNo).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomAgent>(entity =>
            {
                entity.ToTable("CustomAgent", "dbo");

                entity.Property(e => e.Balance).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CommissionAmountCr)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("CommissionAmountCR");

                entity.Property(e => e.CommissionAmountDr)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("commissionAmountDR");

                entity.Property(e => e.CommissionPersent).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Mobile).HasMaxLength(11);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", "dbo");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BookNo).HasMaxLength(50);

                entity.Property(e => e.CompanyName).HasMaxLength(500);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.Crdocument)
                    .HasMaxLength(150)
                    .HasColumnName("CRDocument");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Crno)
                    .HasMaxLength(50)
                    .HasColumnName("CRNo");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.LedgerNo).HasMaxLength(50);

                entity.Property(e => e.MobileNumber).HasMaxLength(100);

                entity.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OtherDocument).HasMaxLength(150);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.TaxDocument).HasMaxLength(150);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatNo).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerCollection>(entity =>
            {
                entity.ToTable("CustomerCollection", "dbo");

                entity.Property(e => e.AmountPaid).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BankCheque).HasMaxLength(50);

                entity.Property(e => e.BankName).HasMaxLength(50);

                entity.Property(e => e.Closed).HasDefaultValueSql("((0))");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentType).HasMaxLength(50);

                entity.Property(e => e.PreviouseBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RecieptNo).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Vat).HasColumnType("decimal(8, 2)");
            });

            modelBuilder.Entity<CustomerCollectionDetail>(entity =>
            {
                entity.ToTable("CustomerCollectionDetail", "dbo");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentDesc).HasMaxLength(500);

                entity.Property(e => e.PaymentType).HasMaxLength(50);
            });

            modelBuilder.Entity<DamageReturn>(entity =>
            {
                entity.ToTable("DamageReturn", "dbo");

                entity.Property(e => e.AttachedDoc).HasMaxLength(200);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DamageReturnDetail>(entity =>
            {
                entity.ToTable("DamageReturnDetail", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(500);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyDozen).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyPices).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<Description>(entity =>
            {
                entity.ToTable("Descriptions", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<DiscountVouchare>(entity =>
            {
                entity.ToTable("DiscountVouchares", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DiscountVouchareItem>(entity =>
            {
                entity.ToTable("DiscountVouchareItems", "dbo");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaxAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<FlatDiscount>(entity =>
            {
                entity.ToTable("FlatDiscount", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FixedDiscount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.StartAmount).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<GrnDetail>(entity =>
            {
                entity.ToTable("GrnDetails", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(200);

                entity.Property(e => e.BoxNo).HasMaxLength(200);

                entity.Property(e => e.Precentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GrnInfo>(entity =>
            {
                entity.ToTable("GrnInfo", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<GroupOfCompany>(entity =>
            {
                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<IbtDetail>(entity =>
            {
                entity.ToTable("IbtDetails", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(200);

                entity.Property(e => e.BoxNo).HasMaxLength(200);

                entity.Property(e => e.Precentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<IbtInfo>(entity =>
            {
                entity.ToTable("IbtInfo", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ImportCoa>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ImportCOA", "dbo");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceNo, e.InvoiceNoRegisterWise });

                entity.ToTable("Invoice", "dbo");

                entity.Property(e => e.InvoiceNo).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.InvoiceNoRegisterWise).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Backbranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("backbranch");

                entity.Property(e => e.BankcardNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankcardNo");

                entity.Property(e => e.CashtillopeningBalance).HasColumnName("cashtillopeningBalance");

                entity.Property(e => e.Closed)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Closedby)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Closingdate)
                    .HasColumnType("datetime")
                    .HasColumnName("closingdate");

                entity.Property(e => e.Companyname)
                    .HasMaxLength(50)
                    .HasColumnName("companyname");

                entity.Property(e => e.Customerid).HasMaxLength(50);

                entity.Property(e => e.Customername)
                    .HasMaxLength(50)
                    .HasColumnName("customername");

                entity.Property(e => e.DateInsert).HasColumnType("datetime");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.Dueamount).HasColumnName("dueamount");

                entity.Property(e => e.Gt)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("GT");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.InsertBy).HasMaxLength(50);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.Invoicediscount).HasColumnName("invoicediscount");

                entity.Property(e => e.Invoicemonth).HasColumnName("invoicemonth");

                entity.Property(e => e.OpeningBalanceRefrence).HasColumnName("openingBalanceRefrence");

                entity.Property(e => e.Pers)
                    .HasMaxLength(50)
                    .HasColumnName("pers");

                entity.Property(e => e.Pono).HasColumnName("PONo");

                entity.Property(e => e.Projectcode)
                    .HasMaxLength(50)
                    .HasColumnName("projectcode");

                entity.Property(e => e.Projectname).HasColumnName("projectname");

                entity.Property(e => e.Registerid)
                    .HasMaxLength(50)
                    .HasColumnName("REGISTERID");

                entity.Property(e => e.RepicationDate).HasColumnType("datetime");

                entity.Property(e => e.ReplicatedStatus).HasMaxLength(50);

                entity.Property(e => e.Replicatedby).HasMaxLength(50);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.Returnremarks).HasMaxLength(200);

                entity.Property(e => e.Saleinvoice).HasColumnName("saleinvoice");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Transport).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.Userid).HasMaxLength(50);

                entity.Property(e => e.Vat).HasColumnType("numeric(18, 2)");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invoice_detail", "dbo");

                entity.Property(e => e.Costprice)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("costprice");

                entity.Property(e => e.Decsrive).HasColumnName("decsrive");

                entity.Property(e => e.Freeprice).HasColumnName("freeprice");

                entity.Property(e => e.Freeqty).HasColumnName("freeqty");

                entity.Property(e => e.InvoiceNo)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("InvoiceNO");

                entity.Property(e => e.Persd).HasColumnName("persd");

                entity.Property(e => e.Persons).HasColumnName("persons");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Productname).HasColumnName("productname");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Total).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TotalCost).HasColumnName("total_cost");
            });

            modelBuilder.Entity<InvoiceDetailPosTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("invoice_detail_POS_TEMP", "dbo");

                entity.Property(e => e.InvoiceNo)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("InvoiceNO");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Productname).HasColumnName("productname");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");
            });

            modelBuilder.Entity<InvoiceDetailReplicated>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invoice_detail_replicated", "dbo");

                entity.Property(e => e.InvoiceNo)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("InvoiceNO");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Replicatedby).HasMaxLength(50);

                entity.Property(e => e.Replicateddate).HasColumnType("datetime");

                entity.Property(e => e.ReplicationStatus)
                    .HasMaxLength(50)
                    .HasColumnName("replicationStatus");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");
            });

            modelBuilder.Entity<InvoiceDetailReplicatedTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invoice_detail_replicated_TEMP", "dbo");

                entity.Property(e => e.InvoiceNo)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("InvoiceNO");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Replicatedby).HasMaxLength(50);

                entity.Property(e => e.Replicateddate).HasColumnType("datetime");

                entity.Property(e => e.ReplicationStatus)
                    .HasMaxLength(50)
                    .HasColumnName("replicationStatus");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");
            });

            modelBuilder.Entity<InvoiceHoldDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invoice_Hold_Detail", "dbo");

                entity.Property(e => e.Costprice).HasColumnName("costprice");

                entity.Property(e => e.Decsrive)
                    .HasMaxLength(4000)
                    .HasColumnName("decsrive");

                entity.Property(e => e.Freeprice).HasColumnName("freeprice");

                entity.Property(e => e.Freeqty).HasColumnName("freeqty");

                entity.Property(e => e.InvoiceNo)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("InvoiceNO");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Productname)
                    .HasMaxLength(4000)
                    .HasColumnName("productname");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");

                entity.Property(e => e.Storeid)
                    .HasMaxLength(50)
                    .HasColumnName("storeid");

                entity.Property(e => e.TotalCost).HasColumnName("total_cost");
            });

            modelBuilder.Entity<InvoiceHoldMaster>(entity =>
            {
                entity.HasKey(e => e.InvoiceNo)
                    .HasName("PK_Invoice_hold");

                entity.ToTable("Invoice_hold_master", "dbo");

                entity.Property(e => e.InvoiceNo).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Amountpaidcash).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Backbranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("backbranch");

                entity.Property(e => e.BankcardNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankcardNo");

                entity.Property(e => e.CashtillopeningBalance).HasColumnName("cashtillopeningBalance");

                entity.Property(e => e.Closed)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Closedby)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingBankAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ClosingCashAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Closingdate)
                    .HasColumnType("datetime")
                    .HasColumnName("closingdate");

                entity.Property(e => e.Companyname)
                    .HasMaxLength(50)
                    .HasColumnName("companyname");

                entity.Property(e => e.Customerid).HasMaxLength(50);

                entity.Property(e => e.Customername)
                    .HasMaxLength(50)
                    .HasColumnName("customername");

                entity.Property(e => e.DateInsert).HasColumnType("datetime");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.Donation).HasColumnName("donation");

                entity.Property(e => e.Dueamount).HasColumnName("dueamount");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.InsertBy).HasMaxLength(50);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNoRegisterWise).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Invoicediscount).HasColumnName("invoicediscount");

                entity.Property(e => e.OpeningBalanceRefrence).HasColumnName("openingBalanceRefrence");

                entity.Property(e => e.Registerid)
                    .HasMaxLength(50)
                    .HasColumnName("REGISTERID");

                entity.Property(e => e.RepicationDate).HasColumnType("datetime");

                entity.Property(e => e.ReplicatedStatus).HasMaxLength(50);

                entity.Property(e => e.Replicatedby).HasMaxLength(50);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.Returnremarks).HasMaxLength(200);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Transport).HasMaxLength(50);

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.Userid).HasMaxLength(50);

                entity.Property(e => e.Vat).HasColumnType("numeric(18, 3)");
            });

            modelBuilder.Entity<InvoicePosTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Invoice_POS_TEMP", "dbo");

                entity.Property(e => e.Backbranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("backbranch");

                entity.Property(e => e.BankcardNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankcardNo");

                entity.Property(e => e.CashtillopeningBalance).HasColumnName("cashtillopeningBalance");

                entity.Property(e => e.Closed)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Closedby)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerMobileno)
                    .HasMaxLength(200)
                    .HasColumnName("customerMobileno");

                entity.Property(e => e.Customeraddress)
                    .HasMaxLength(500)
                    .HasColumnName("customeraddress");

                entity.Property(e => e.Customerbalance).HasColumnName("customerbalance");

                entity.Property(e => e.Customerid).HasMaxLength(200);

                entity.Property(e => e.Customername)
                    .HasMaxLength(500)
                    .HasColumnName("customername");

                entity.Property(e => e.DateInsert).HasColumnType("datetime");

                entity.Property(e => e.DateUpdate).HasColumnType("datetime");

                entity.Property(e => e.Dueamount).HasColumnName("dueamount");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.InsertBy).HasMaxLength(50);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.InvoiceNoRegisterWise).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Invoicediscount).HasColumnName("invoicediscount");

                entity.Property(e => e.OpeningBalanceRefrence).HasColumnName("openingBalanceRefrence");

                entity.Property(e => e.Registerid)
                    .HasMaxLength(50)
                    .HasColumnName("REGISTERID");

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.Returnremarks).HasMaxLength(200);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.UpdateBy).HasMaxLength(50);

                entity.Property(e => e.Userid).HasMaxLength(50);
            });

            modelBuilder.Entity<LoyaityPointInfo>(entity =>
            {
                entity.ToTable("LoyaityPointInfo", "dbo");

                entity.Property(e => e.AddMoney).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PointEarn).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.PointRadium).HasColumnType("decimal(8, 2)");
            });

            modelBuilder.Entity<LoyaltyCardsInfo>(entity =>
            {
                entity.ToTable("LoyaltyCardsInfo", "dbo");

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<LoyaltyPointSystem>(entity =>
            {
                entity.ToTable("LoyaltyPointSystem", "dbo");

                entity.Property(e => e.Amount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Points).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<ProFormaInvoice>(entity =>
            {
                entity.ToTable("ProFormaInvoice", "dbo");

                entity.Property(e => e.ApprovedBy).HasMaxLength(450);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.ShippingAddress).HasMaxLength(200);

                entity.Property(e => e.ShippingAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ShippingMethod).HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProFormaInvoiceItem>(entity =>
            {
                entity.ToTable("ProFormaInvoiceItems", "dbo");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.PartNo).HasMaxLength(100);

                entity.Property(e => e.Pfid).HasColumnName("PFId");

                entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Tax).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Unit).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductInfo>(entity =>
            {
                entity.ToTable("ProductInfo", "dbo");

                entity.Property(e => e.BoxNo).HasMaxLength(50);

                entity.Property(e => e.CostPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CustomField1).HasMaxLength(100);

                entity.Property(e => e.CustomField2).HasMaxLength(100);

                entity.Property(e => e.CustomField3).HasMaxLength(100);

                entity.Property(e => e.CustomField4).HasMaxLength(100);

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ExpDate).HasColumnType("date");

                entity.Property(e => e.Image)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MfgDate).HasColumnType("date");

                entity.Property(e => e.MinQty).HasMaxLength(50);

                entity.Property(e => e.OreginalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PackBarcode).HasMaxLength(50);

                entity.Property(e => e.PackCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PackPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductId).HasMaxLength(50);

                entity.Property(e => e.ProductInitialPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductNameArabic).HasMaxLength(500);

                entity.Property(e => e.ProductNameEng).HasMaxLength(500);

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitBarcode).HasMaxLength(50);

                entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WarrantyPeriod).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.ToTable("ProductPrice", "dbo");

                entity.Property(e => e.Discount1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Discount2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Discount3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrgPrice1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrgPrice2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrgPrice3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price1Vat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price2Vat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price3Vat).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.ToTable("PurchaseOrders", "dbo");

                entity.Property(e => e.ApprovedBy).HasMaxLength(450);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Prid).HasColumnName("PRId");

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.ShippingAddress).HasMaxLength(200);

                entity.Property(e => e.ShippingAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ShippingMethod).HasMaxLength(200);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.ToTable("PurchaseOrderItems", "dbo");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.PartNo).HasMaxLength(100);

                entity.Property(e => e.Poid).HasColumnName("POId");

                entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Tax).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Unit).HasMaxLength(50);
            });

            modelBuilder.Entity<PurchaseReciept>(entity =>
            {
                entity.ToTable("PurchaseReciept", "dbo");

                entity.Property(e => e.AttachedDoc).HasMaxLength(500);

                entity.Property(e => e.ChargesDescription).HasMaxLength(100);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceVatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OtherCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PurchaseRecieptDetail>(entity =>
            {
                entity.ToTable("PurchaseRecieptDetails", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(500);

                entity.Property(e => e.BoxRetail).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CostPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DescriptionArabic).HasMaxLength(250);

                entity.Property(e => e.DescriptionEng).HasMaxLength(250);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Pvat)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PVat");

                entity.Property(e => e.QtyDozen).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyPices).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReceivePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReceiveQtyDozen).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReceiveQtyPices).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitDescription).HasMaxLength(50);

                entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<QuotationDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Quotation_Detail", "dbo");

                entity.Property(e => e.Costprice).HasColumnName("costprice");

                entity.Property(e => e.Decsrive)
                    .HasMaxLength(4000)
                    .HasColumnName("decsrive");

                entity.Property(e => e.InvoiceNo)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("InvoiceNO");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Productname).HasColumnName("productname");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");

                entity.Property(e => e.TotalCost).HasColumnName("total_cost");
            });

            modelBuilder.Entity<QuotationMaster>(entity =>
            {
                entity.HasKey(e => e.InvoiceNo);

                entity.ToTable("Quotation_master", "dbo");

                entity.Property(e => e.InvoiceNo).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Amountpaidcash).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Backbranch)
                    .HasMaxLength(50)
                    .HasColumnName("backbranch");

                entity.Property(e => e.BankcardNo)
                    .HasMaxLength(50)
                    .HasColumnName("bankcardNo");

                entity.Property(e => e.Companyname)
                    .HasMaxLength(50)
                    .HasColumnName("companyname");

                entity.Property(e => e.Customerid).HasMaxLength(50);

                entity.Property(e => e.Customername)
                    .HasMaxLength(50)
                    .HasColumnName("customername");

                entity.Property(e => e.Dueamount).HasColumnName("dueamount");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.InsertBy).HasMaxLength(50);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNoRegisterWise).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Invoicediscount).HasColumnName("invoicediscount");

                entity.Property(e => e.Registerid)
                    .HasMaxLength(50)
                    .HasColumnName("REGISTERID");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Storeid).HasColumnName("storeid");

                entity.Property(e => e.Transport).HasMaxLength(50);

                entity.Property(e => e.Userid).HasMaxLength(50);

                entity.Property(e => e.Vat).HasColumnType("numeric(18, 3)");
            });

            modelBuilder.Entity<Register>(entity =>
            {
                entity.ToTable("Register", "dbo");

                entity.Property(e => e.CashTill).HasMaxLength(50);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReportHead>(entity =>
            {
                entity.ToTable("ReportHeads", "dbo");

                entity.Property(e => e.Logo).HasMaxLength(200);

                entity.Property(e => e.ReportFooterArabic).HasMaxLength(150);

                entity.Property(e => e.ReportFooterEng).HasMaxLength(300);

                entity.Property(e => e.ReportHeaderArabic)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ReportHeaderEng).HasMaxLength(400);

                entity.Property(e => e.Storename)
                    .HasMaxLength(255)
                    .HasColumnName("storename");

                entity.Property(e => e.Vatnum)
                    .HasMaxLength(255)
                    .HasColumnName("vatnum");
            });

            modelBuilder.Entity<RequestForInformation>(entity =>
            {
                entity.ToTable("RequestForInformation", "dbo");

                entity.Property(e => e.ApprovedBy).HasMaxLength(450);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.ReqDate).HasColumnType("date");

                entity.Property(e => e.Requester).HasMaxLength(150);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RequestForInformationItem>(entity =>
            {
                entity.ToTable("RequestForInformationItems", "dbo");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Mfcompany)
                    .HasMaxLength(100)
                    .HasColumnName("MFCompany");

                entity.Property(e => e.Qty).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Reason).HasMaxLength(150);

                entity.Property(e => e.Rfiid).HasColumnName("RFIId");
            });

            modelBuilder.Entity<RequestForPurchase>(entity =>
            {
                entity.ToTable("RequestForPurchases", "dbo");

                entity.Property(e => e.ApprovedBy).HasMaxLength(450);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Requester).HasMaxLength(150);

                entity.Property(e => e.RequireDate).HasColumnType("date");

                entity.Property(e => e.Rfqid).HasColumnName("RFQId");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RequestForPurchaseItem>(entity =>
            {
                entity.ToTable("RequestForPurchaseItems", "dbo");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Mfcompany)
                    .HasMaxLength(100)
                    .HasColumnName("MFCompany");

                entity.Property(e => e.PartNo).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Reason).HasMaxLength(150);
            });

            modelBuilder.Entity<RequestForQuotation>(entity =>
            {
                entity.ToTable("RequestForQuotation", "dbo");

                entity.Property(e => e.ApprovedBy).HasMaxLength(450);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Requester).HasMaxLength(150);

                entity.Property(e => e.RequireDate).HasColumnType("date");

                entity.Property(e => e.Rfiid).HasColumnName("RFIId");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RequestForQuotationItem>(entity =>
            {
                entity.ToTable("RequestForQuotationItems", "dbo");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Mfcompany)
                    .HasMaxLength(100)
                    .HasColumnName("MFCompany");

                entity.Property(e => e.Qty).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Reason).HasMaxLength(150);

                entity.Property(e => e.Rfqid).HasColumnName("RFQId");
            });

            modelBuilder.Entity<ReturnBarcodeImage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Return_Barcode_Image", "dbo");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Returnid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("returnid");
            });

            modelBuilder.Entity<ReturnInvoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ReturnInvoice", "dbo");

                entity.Property(e => e.Customerid)
                    .HasMaxLength(50)
                    .HasColumnName("customerid");

                entity.Property(e => e.Invoiceno)
                    .HasMaxLength(50)
                    .HasColumnName("invoiceno");

                entity.Property(e => e.Productname).HasColumnName("productname");

                entity.Property(e => e.Productno)
                    .HasMaxLength(50)
                    .HasColumnName("productno");

                entity.Property(e => e.Registerid).HasMaxLength(50);

                entity.Property(e => e.Returnby)
                    .HasMaxLength(50)
                    .HasColumnName("returnby");

                entity.Property(e => e.Returndate)
                    .HasColumnType("datetime")
                    .HasColumnName("returndate");

                entity.Property(e => e.Returndiscount).HasColumnName("returndiscount");

                entity.Property(e => e.Returnid).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.ReturnidRegisterWise).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Returnprice).HasColumnName("returnprice");

                entity.Property(e => e.Returnqty).HasColumnName("returnqty");

                entity.Property(e => e.Returntotal)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("returntotal");

                entity.Property(e => e.Statuss).HasMaxLength(50);

                entity.Property(e => e.Storeid).HasColumnName("storeid");
            });

            modelBuilder.Entity<SalesInvoice>(entity =>
            {
                entity.ToTable("SalesInvoices", "dbo");

                entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BankAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CashAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Donation).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.InvoiceAmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PointEarn).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RadiumPoint).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherNo).HasMaxLength(50);
            });

            modelBuilder.Entity<SalesInvoiceHold>(entity =>
            {
                entity.ToTable("SalesInvoiceHold", "dbo");

                entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BankAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CashAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Donation).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.InvoiceAmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PointEarn).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RadiumPoint).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.UnHoldBy).HasMaxLength(450);

                entity.Property(e => e.UnHoldDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherNo).HasMaxLength(50);
            });

            modelBuilder.Entity<SalesInvoiceHoldItem>(entity =>
            {
                entity.ToTable("SalesInvoiceHoldItems", "dbo");

                entity.Property(e => e.AmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.ItemDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSaleVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalInVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SalesInvoiceItem>(entity =>
            {
                entity.ToTable("SalesInvoiceItems", "dbo");

                entity.Property(e => e.AmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.ItemDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSaleVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalInVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SalesInvoiceReturnItem>(entity =>
            {
                entity.ToTable("SalesInvoiceReturnItems", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.ItemAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotal).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SalesInvoiceVoucher>(entity =>
            {
                entity.ToTable("SalesInvoiceVoucher", "dbo");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.IssueDate).HasColumnType("datetime");

                entity.Property(e => e.UsedDate).HasColumnType("datetime");

                entity.Property(e => e.VocherType)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'Invoice')");
            });

            modelBuilder.Entity<SalesReturnInvoice>(entity =>
            {
                entity.ToTable("SalesReturnInvoices", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.PointReduce).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReturnAmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReturnTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReturnType).HasMaxLength(20);

                entity.Property(e => e.ReturnVoucherNo).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SalesReturnInvoicePayment>(entity =>
            {
                entity.ToTable("SalesReturnInvoicePayment", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalWithoutVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatAmout).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SalesType>(entity =>
            {
                entity.ToTable("SalesTypes", "dbo");

                entity.Property(e => e.TypeCode).HasMaxLength(50);

                entity.Property(e => e.TypeDescription).HasMaxLength(250);
            });

            modelBuilder.Entity<Seasson>(entity =>
            {
                entity.ToTable("Seassons", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<SelectedItemDiscount>(entity =>
            {
                entity.ToTable("SelectedItemDiscount", "dbo");

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FixedDiscount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.StartAmount).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SelectedItemDiscountItem>(entity =>
            {
                entity.ToTable("SelectedItemDiscountItems", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(200);
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shifts", "dbo");

                entity.Property(e => e.CloseAt).HasColumnType("datetime");

                entity.Property(e => e.OpenAt).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(450);
            });

            modelBuilder.Entity<ShippingDetail>(entity =>
            {
                entity.ToTable("ShippingDetails", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.BoxNo).HasMaxLength(100);

                entity.Property(e => e.DescriptionArabic).HasMaxLength(250);

                entity.Property(e => e.DescriptionEng).HasMaxLength(250);

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SaleVat).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ShippingInfo>(entity =>
            {
                entity.ToTable("ShippingInfo", "dbo");

                entity.Property(e => e.AttachedDoc).HasMaxLength(500);

                entity.Property(e => e.ChargesDescription).HasMaxLength(100);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OtherCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RecordType).HasMaxLength(10);

                entity.Property(e => e.ReferenceNo).HasMaxLength(200);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("Sizes", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store", "dbo");

                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.Brandcode).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Crno)
                    .HasMaxLength(50)
                    .HasColumnName("CRNo");

                entity.Property(e => e.FooterA).HasMaxLength(4000);

                entity.Property(e => e.FooterE).HasMaxLength(4000);

                entity.Property(e => e.GlAccount).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(500);

                entity.Property(e => e.StoreCode).HasMaxLength(50);

                entity.Property(e => e.StoreType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatNo).HasMaxLength(50);
            });

            modelBuilder.Entity<Style>(entity =>
            {
                entity.ToTable("Styles", "dbo");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Suppliers", "dbo");

                entity.Property(e => e.AccountCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNo).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BankName).HasMaxLength(50);

                entity.Property(e => e.Bpcode)
                    .HasMaxLength(50)
                    .HasColumnName("BPcode");

                entity.Property(e => e.CompanyName).HasMaxLength(150);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.Crdocument)
                    .HasMaxLength(150)
                    .HasColumnName("CRDocument");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Crnumber)
                    .HasMaxLength(50)
                    .HasColumnName("CRNumber");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OtherDocument).HasMaxLength(150);

                entity.Property(e => e.PhoneNo).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TaxDocument).HasMaxLength(150);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatNo).HasMaxLength(50);

                entity.Property(e => e.Website).HasMaxLength(100);
            });

            modelBuilder.Entity<SupplierPurchase>(entity =>
            {
                entity.ToTable("SupplierPurchase", "dbo");

                entity.Property(e => e.AttachedDoc).HasMaxLength(500);

                entity.Property(e => e.ChargesDescription).HasMaxLength(100);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceNo).HasMaxLength(200);

                entity.Property(e => e.OtherCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatNo).HasMaxLength(50);
            });

            modelBuilder.Entity<SupplierPurchaseAsset>(entity =>
            {
                entity.ToTable("SupplierPurchaseAsset", "dbo");

                entity.Property(e => e.AttachedDoc).HasMaxLength(500);

                entity.Property(e => e.ChargesDescription).HasMaxLength(100);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceNo).HasMaxLength(200);

                entity.Property(e => e.OtherCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatNo).HasMaxLength(50);
            });

            modelBuilder.Entity<SupplierPurchaseAssetDetail>(entity =>
            {
                entity.ToTable("SupplierPurchaseAssetDetails", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.DescriptionArabic).HasMaxLength(250);

                entity.Property(e => e.DescriptionEng).HasMaxLength(250);

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductDate).HasColumnType("date");

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SupplierPurchaseDetail>(entity =>
            {
                entity.ToTable("SupplierPurchaseDetails", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.BoxRetail).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DescriptionArabic).HasMaxLength(250);

                entity.Property(e => e.DescriptionEng).HasMaxLength(250);

                entity.Property(e => e.ExpireDate).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductDate).HasColumnType("date");

                entity.Property(e => e.Pvat)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PVat");

                entity.Property(e => e.QtyDozen).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyPerUnit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyPices).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitBarcode).HasMaxLength(50);

                entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitDescription).HasMaxLength(50);

                entity.Property(e => e.UnitSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SupplierReturn>(entity =>
            {
                entity.ToTable("SupplierReturn", "dbo");

                entity.Property(e => e.AttachedDoc).HasMaxLength(200);

                entity.Property(e => e.CratedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SupplierReturnDetail>(entity =>
            {
                entity.ToTable("SupplierReturnDetail", "dbo");

                entity.Property(e => e.Barcode).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyDozen).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QtyPices).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Vat).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<SystemModule>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SystemModules", "dbo");

                entity.Property(e => e.MenueName).HasMaxLength(500);

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.ModuleName).HasMaxLength(500);

                entity.Property(e => e.ModuleSystemMenueName).HasMaxLength(500);
            });

            modelBuilder.Entity<Systemmenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("systemmenus", "dbo");

                entity.Property(e => e.Displayname)
                    .HasMaxLength(500)
                    .HasColumnName("displayname");

                entity.Property(e => e.Entrybyuser).HasColumnName("entrybyuser");

                entity.Property(e => e.Entrydate)
                    .HasColumnType("datetime")
                    .HasColumnName("entrydate");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("id");

                entity.Property(e => e.Modulename)
                    .HasMaxLength(500)
                    .HasColumnName("modulename");

                entity.Property(e => e.Systemname)
                    .HasMaxLength(500)
                    .HasColumnName("systemname");
            });

            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("tbl_Account", "dbo");

                entity.Property(e => e.AccountId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccountName).HasMaxLength(200);

                entity.Property(e => e.Accounttype)
                    .HasMaxLength(50)
                    .HasColumnName("ACCOUNTTYPE");

                entity.Property(e => e.FamilyId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FamilyID");
            });

            modelBuilder.Entity<TblChartofAccount>(entity =>
            {
                entity.HasKey(e => e.ChartAccountId);

                entity.ToTable("tbl_ChartofAccount", "dbo");

                entity.HasIndex(e => e.Cname, "IX_tbl_ChartofAccount")
                    .IsUnique();

                entity.Property(e => e.ChartAccountId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ChartAccountID");

                entity.Property(e => e.Acc).HasColumnName("acc");

                entity.Property(e => e.AccountId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("AccountID");

                entity.Property(e => e.Bank).HasColumnName("bank");

                entity.Property(e => e.Cname)
                    .HasMaxLength(200)
                    .HasColumnName("CName");

                entity.Property(e => e.CrDr)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("crDR");

                entity.Property(e => e.Crno)
                    .HasMaxLength(50)
                    .HasColumnName("crno");

                entity.Property(e => e.Descriptoin).HasMaxLength(2000);

                entity.Property(e => e.Ob)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("OB");

                entity.Property(e => e.Vatno)
                    .HasMaxLength(50)
                    .HasColumnName("vatno");
            });

            modelBuilder.Entity<TblExpense>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_Expense", "dbo");

                entity.Property(e => e.Accountid).HasColumnName("accountid");

                entity.Property(e => e.AmountToPay).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.BuildingNo).HasColumnName("BuildingNO");

                entity.Property(e => e.Discount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PaidAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PaymentArabic)
                    .HasMaxLength(50)
                    .HasColumnName("paymentArabic");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("paymentDate");

                entity.Property(e => e.Pkid)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PKID");

                entity.Property(e => e.Remaining).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Supplierid).HasColumnName("supplierid");

                entity.Property(e => e.Suppliername).HasMaxLength(50);

                entity.Property(e => e.Tantid).HasColumnName("tantid");
            });

            modelBuilder.Entity<TblFamilyAccount>(entity =>
            {
                entity.HasKey(e => e.FamilyId);

                entity.ToTable("tbl_FamilyAccount", "dbo");

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.FamilyName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblForm>(entity =>
            {
                entity.HasKey(e => e.FormId);

                entity.ToTable("tbl_Forms", "dbo");

                entity.Property(e => e.FormId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FormID");

                entity.Property(e => e.DescrirptiveName).HasMaxLength(50);

                entity.Property(e => e.FromName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblFormDetail>(entity =>
            {
                entity.HasKey(e => e.FormDetailId);

                entity.ToTable("tbl_FormDetails", "dbo");

                entity.Property(e => e.FormDetailId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FormDetailID");

                entity.Property(e => e.FormDetailControlName).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FormID");
            });

            modelBuilder.Entity<TblInsurance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_insurance", "dbo");

                entity.Property(e => e.Customerid).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Deduct).HasColumnName("deduct");

                entity.Property(e => e.DeductReason)
                    .HasMaxLength(4000)
                    .HasColumnName("deduct_reason");

                entity.Property(e => e.Idd)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IDD");

                entity.Property(e => e.InsuredRecevied).HasColumnName("Insured_recevied");

                entity.Property(e => e.InsuredReturn).HasColumnName("insured_return");

                entity.Property(e => e.ReceiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Receive_date");
            });

            modelBuilder.Entity<TblLedger>(entity =>
            {
                entity.HasKey(e => e.LedgerId);

                entity.ToTable("tbl_Ledger", "dbo");

                entity.Property(e => e.LedgerId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("LedgerID");

                entity.Property(e => e.ChartofAccountId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ChartofAccountID");

                entity.Property(e => e.CrAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.DrAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.EntryDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Iid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("IID");

                entity.Property(e => e.Refarance).HasColumnName("refarance");

                entity.Property(e => e.SType)
                    .HasMaxLength(500)
                    .HasColumnName("sType");

                entity.Property(e => e.StoreId)
                    .HasMaxLength(50)
                    .HasColumnName("StoreID");
            });

            modelBuilder.Entity<TblProfression>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_Profression", "dbo");

                entity.Property(e => e.Dept)
                    .HasMaxLength(255)
                    .HasColumnName("dept");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.Pkid)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("pkid");

                entity.Property(e => e.Profression).HasMaxLength(255);

                entity.Property(e => e.SalaryTosaudi)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("salaryTOsaudi");

                entity.Property(e => e.Salaryfromarab)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("salaryfromarab");

                entity.Property(e => e.Salaryfromasia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("salaryfromasia");

                entity.Property(e => e.Salaryfromsaudi)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("salaryfromsaudi");

                entity.Property(e => e.Salarytoarab)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("salarytoarab");

                entity.Property(e => e.Salarytoasia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("salarytoasia");
            });

            modelBuilder.Entity<TblStock>(entity =>
            {
                entity.ToTable("tblStock", "dbo");

                entity.Property(e => e.BarCode).HasMaxLength(200);

                entity.Property(e => e.DamageQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IbtInQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IbtOutQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PurInQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PurOutQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SaleInQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SaleOutQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SalesType).HasMaxLength(50);

                entity.Property(e => e.ShipInQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShipOutQty)
                    .HasColumnType("decimal(18, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShortInQty)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");
            });

            modelBuilder.Entity<TblUserPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.ToTable("tbl_userPermission", "dbo");

                entity.Property(e => e.PermissionId)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PermissionID");

                entity.Property(e => e.BVisible).HasColumnName("bVisible");

                entity.Property(e => e.FormDetailId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FormDetailID");

                entity.Property(e => e.UserId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<TemPrintingBarcode>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TemPrintingBarcode", "dbo");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(1000)
                    .HasColumnName("barcode");

                entity.Property(e => e.Companyname)
                    .HasMaxLength(1000)
                    .HasColumnName("companyname");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Expdate).HasMaxLength(1000);

                entity.Property(e => e.Field1)
                    .HasMaxLength(50)
                    .HasColumnName("field1");

                entity.Property(e => e.Field2)
                    .HasMaxLength(50)
                    .HasColumnName("field2");

                entity.Property(e => e.Field3)
                    .HasMaxLength(50)
                    .HasColumnName("field3");

                entity.Property(e => e.Field4)
                    .HasMaxLength(50)
                    .HasColumnName("field4");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.ImageI).HasColumnName("imageI");

                entity.Property(e => e.Price).HasMaxLength(100);

                entity.Property(e => e.Productdate).HasMaxLength(1000);

                entity.Property(e => e.Sku).HasMaxLength(1000);
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("TransactionTypes", "dbo");

                entity.Property(e => e.TrantypeDisplay).HasMaxLength(50);

                entity.Property(e => e.TrantypeId)
                    .HasMaxLength(50)
                    .HasColumnName("TrantypeID");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Units", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<UserBalance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("User_Balance", "dbo");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Tdate).HasColumnType("datetime");

                entity.Property(e => e.TransactionDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Transactiontype)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Userid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("userid");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("User_info", "dbo");

                entity.Property(e => e.Userid)
                    .ValueGeneratedNever()
                    .HasColumnName("userid");

                entity.Property(e => e.AllowedGiftInvoice).HasMaxLength(50);

                entity.Property(e => e.Alloweddiscount)
                    .HasMaxLength(100)
                    .HasColumnName("alloweddiscount");

                entity.Property(e => e.Allowedinvoiceedit)
                    .HasMaxLength(50)
                    .HasColumnName("allowedinvoiceedit");

                entity.Property(e => e.Allowedreturn)
                    .HasMaxLength(100)
                    .HasColumnName("allowedreturn");

                entity.Property(e => e.Allowsupplierdelete)
                    .HasMaxLength(50)
                    .HasColumnName("allowsupplierdelete");

                entity.Property(e => e.Backoffice).HasColumnName("backoffice");

                entity.Property(e => e.Displayname).HasMaxLength(100);

                entity.Property(e => e.Donation).HasMaxLength(50);

                entity.Property(e => e.Hrm).HasColumnName("HRM");

                entity.Property(e => e.Loyltycard)
                    .HasMaxLength(50)
                    .HasColumnName("loyltycard");

                entity.Property(e => e.PassSave).HasColumnName("pass_save");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Pos).HasColumnName("POS");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Usercolor)
                    .HasMaxLength(50)
                    .HasColumnName("usercolor");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.Usertype)
                    .HasMaxLength(100)
                    .HasColumnName("usertype");

                entity.Property(e => e.Voucher).HasMaxLength(50);

                entity.Property(e => e.Warehouse).HasColumnName("warehouse");
            });

            modelBuilder.Entity<UserInfoReturn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_info_return", "dbo");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Pkid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("pkid");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Userright>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("userrights", "dbo");

                entity.Property(e => e.Entryby).HasColumnName("entryby");

                entity.Property(e => e.Entrydate)
                    .HasColumnType("datetime")
                    .HasColumnName("entrydate");

                entity.Property(e => e.Moduleid).HasColumnName("moduleid");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Vat>(entity =>
            {
                entity.ToTable("Vat", "dbo");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.VatDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Vat_Date");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.ToTable("Vendors", "dbo");

                entity.Property(e => e.NameArabic).HasMaxLength(100);

                entity.Property(e => e.NameEng).HasMaxLength(100);
            });

            modelBuilder.Entity<View1>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_1", "dbo");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .HasColumnName("color");

                entity.Property(e => e.Costprice).HasColumnName("costprice");

                entity.Property(e => e.Field1)
                    .HasMaxLength(50)
                    .HasColumnName("field1");

                entity.Property(e => e.Field2)
                    .HasMaxLength(50)
                    .HasColumnName("field2");

                entity.Property(e => e.Field3)
                    .HasMaxLength(50)
                    .HasColumnName("field3");

                entity.Property(e => e.Field4)
                    .HasMaxLength(50)
                    .HasColumnName("field4");

                entity.Property(e => e.Image)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.Qtyperunit).HasColumnName("qtyperunit");

                entity.Property(e => e.Size)
                    .HasMaxLength(50)
                    .HasColumnName("size");

                entity.Property(e => e.Sku)
                    .HasMaxLength(50)
                    .HasColumnName("sku");

                entity.Property(e => e.Unitbarcode)
                    .HasMaxLength(50)
                    .HasColumnName("unitbarcode");

                entity.Property(e => e.Unitcost).HasColumnName("unitcost");

                entity.Property(e => e.Unitid).HasColumnName("unitid");

                entity.Property(e => e.Usaleprice).HasColumnName("usaleprice");
            });

            modelBuilder.Entity<VoidDetail>(entity =>
            {
                entity.ToTable("VoidDetail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Barcode).HasMaxLength(4000);

                entity.Property(e => e.ItemDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSaleVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalInVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("qty");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoidId).HasColumnName("VoidID");
            });

            modelBuilder.Entity<VoidDetail1>(entity =>
            {
                entity.ToTable("VoidDetails", "winprerp_sa");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmountExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Barcode).HasMaxLength(4000);

                entity.Property(e => e.ItemDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemOrgSaleVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalExVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalInVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemTotalVat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("qty");

                entity.Property(e => e.VatAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoidId).HasColumnName("VoidID");
            });

            modelBuilder.Entity<VoidMaster>(entity =>
            {
                entity.ToTable("VoidMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Balanceamount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("balanceamount");

                entity.Property(e => e.Bankamount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("bankamount");

                entity.Property(e => e.Cashamount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Createby)
                    .HasMaxLength(4000)
                    .HasColumnName("createby");

                entity.Property(e => e.Createdate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdate");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("discount");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("discountAmount");

                entity.Property(e => e.Donation).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceAmountExvat)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("InvoiceAmountEXVat");

                entity.Property(e => e.Invoicetotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("invoicetotal");

                entity.Property(e => e.Pointearn)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("pointearn");

                entity.Property(e => e.Radiumpoint).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Shiftid).HasColumnName("shiftid");

                entity.Property(e => e.Statuss)
                    .HasMaxLength(4000)
                    .HasColumnName("statuss");

                entity.Property(e => e.Totalqty)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("totalqty");

                entity.Property(e => e.Vatmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("vatmount");

                entity.Property(e => e.VoucherAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherNo).HasMaxLength(50);
            });

            modelBuilder.Entity<VoidMaster1>(entity =>
            {
                entity.ToTable("VoidMaster", "winprerp_sa");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Balanceamount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("balanceamount");

                entity.Property(e => e.Bankamount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("bankamount");

                entity.Property(e => e.Cashamount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Createby)
                    .HasMaxLength(4000)
                    .HasColumnName("createby");

                entity.Property(e => e.Createdate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdate");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("discount");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("discountAmount");

                entity.Property(e => e.Donation).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceAmountExvat)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("InvoiceAmountEXVat");

                entity.Property(e => e.Invoicetotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("invoicetotal");

                entity.Property(e => e.Pointearn)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("pointearn");

                entity.Property(e => e.Radiumpoint).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Shiftid).HasColumnName("shiftid");

                entity.Property(e => e.Statuss)
                    .HasMaxLength(4000)
                    .HasColumnName("statuss");

                entity.Property(e => e.Totalqty)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("totalqty");

                entity.Property(e => e.Vatmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("vatmount");

                entity.Property(e => e.VoucherAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VoucherNo).HasMaxLength(50);
            });

            modelBuilder.Entity<VuCustomerReport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vu_CustomerReport", "dbo");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.Buildingno)
                    .HasMaxLength(50)
                    .HasColumnName("buildingno");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Companyname)
                    .HasMaxLength(500)
                    .HasColumnName("companyname");

                entity.Property(e => e.Creditlimit).HasColumnName("creditlimit");

                entity.Property(e => e.Crno)
                    .HasMaxLength(50)
                    .HasColumnName("CRNo");

                entity.Property(e => e.CustomerPicture).HasColumnType("image");

                entity.Property(e => e.Customerid)
                    .HasMaxLength(50)
                    .HasColumnName("customerid");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Entrydate)
                    .HasColumnType("datetime")
                    .HasColumnName("entrydate");

                entity.Property(e => e.Entrytime)
                    .HasColumnType("datetime")
                    .HasColumnName("entrytime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Iddd)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("IDDD");

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .HasColumnName("image");

                entity.Property(e => e.Invpers)
                    .HasMaxLength(50)
                    .HasColumnName("invpers");

                entity.Property(e => e.Ledgerbook)
                    .HasMaxLength(50)
                    .HasColumnName("ledgerbook");

                entity.Property(e => e.Mobileno)
                    .HasMaxLength(100)
                    .HasColumnName("mobileno");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Openingbalance).HasColumnName("openingbalance");

                entity.Property(e => e.Pageno)
                    .HasMaxLength(50)
                    .HasColumnName("pageno");

                entity.Property(e => e.Phoneno)
                    .HasMaxLength(50)
                    .HasColumnName("phoneno");

                entity.Property(e => e.Projectcode).HasColumnName("projectcode");

                entity.Property(e => e.Transp).HasMaxLength(50);

                entity.Property(e => e.Vatnum).HasMaxLength(50);
            });

            modelBuilder.Entity<VwServerDb>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwServerDB", "dbo");

                entity.Property(e => e.Dbname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DBName");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ServerName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ServerPwd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ServerPWD");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<Year>(entity =>
            {
                entity.ToTable("Years", "dbo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
