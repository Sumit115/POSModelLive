    using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SSRepository.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBranchMas> TblBranchMas { get; set; } = null!;
        public virtual DbSet<TblCompany> TblCompanies { get; set; } = null!;
        public virtual DbSet<TblEmployeeMas> TblEmployeeMas { get; set; } = null!;
        public virtual DbSet<TblUserMas> TblUserMas { get; set; } = null!;
        public virtual DbSet<TblCustomerMas> TblCustomerMas { get; set; } = null!;
        public virtual DbSet<TblWalkingCustomerMas> TblWalkingCustomerMas { get; set; } = null!;
        public virtual DbSet<TblVendorMas> TblVendorMas { get; set; } = null!;
        public virtual DbSet<TblFormMas> TblFormMas { get; set; } = null!;
        public virtual DbSet<TblGridStructer> TblGridStructer { get; set; } = null!;
        public virtual DbSet<TblCategoryMas> TblCategoryMas { get; set; } = null!;
        public virtual DbSet<TblCategorySizeLnk> TblCategorySizeLnk { get; set; } = null!;
        public virtual DbSet<TblCategoryGroupMas> TblCategoryGroupMas { get; set; } = null!;
        public virtual DbSet<TblProductMas> TblProductMas { get; set; } = null!;
        public virtual DbSet<TblBankMas> TblBankMas { get; set; } = null!;
        public virtual DbSet<TblSeriesMas> TblSeriesMas { get; set; } = null!;
        public virtual DbSet<TblErrorLog> TblErrorLog { get; set; } = null!;
        public virtual DbSet<TblSalesOrdertrn> TblSalesOrdertrn { get; set; } = null!;
        public virtual DbSet<TblSalesOrderdtl> TblSalesOrderdtl { get; set; } = null!;
        public virtual DbSet<TblSalesInvoicetrn> TblSalesInvoicetrn { get; set; } = null!;
        public virtual DbSet<TblSalesInvoicedtl> TblSalesInvoicedtl { get; set; } = null!;
        public virtual DbSet<TblPurchaseOrdertrn> TblPurchaseOrdertrn { get; set; } = null!;
        public virtual DbSet<TblPurchaseOrderdtl> TblPurchaseOrderdtl { get; set; } = null!;
        public virtual DbSet<TblPurchaseInvoicetrn> TblPurchaseInvoicetrn { get; set; } = null!;
        public virtual DbSet<TblPurchaseInvoicedtl> TblPurchaseInvoicedtl { get; set; } = null!;
        public virtual DbSet<TblSalesChallantrn> TblSalesChallantrn { get; set; } = null!;
        public virtual DbSet<TblSalesChallandtl> TblSalesChallandtl { get; set; } = null!;
        public virtual DbSet<TblProdLotDtl> TblProdLotDtl { get; set; } = null!;
        public virtual DbSet<TblProdStockDtl> TblProdStockDtl { get; set; } = null!;
        public virtual DbSet<TblBrandMas> TblBrandMas { get; set; } = null!;
        public virtual DbSet<TblCityMas> TblCityMas { get; set; } = null!;
        public virtual DbSet<TblAccountGroupMas> TblAccountGroupMas { get; set; } = null!;

        public virtual DbSet<TblAccountMas> TblAccountMas { get; set; } = null!;
        public virtual DbSet<TblAccountLicDtl> TblAccountLicDtl { get; set; } = null!;
        public virtual DbSet<TblAccountLocLnk> TblAccountLocLnk { get; set; } = null!;
        public virtual DbSet<TblAccountDtl> TblAccountDtl { get; set; } = null!;
        public virtual DbSet<TblCountryMas> TblCountryMas { get; set; } = null!;
        public virtual DbSet<TblStateMas> TblStateMas { get; set; } = null!;
        public virtual DbSet<TblDistrictMas> TblDistrictMas { get; set; } = null!;
        public virtual DbSet<TblStationMas> TblStationMas { get; set; } = null!;
        public virtual DbSet<TblZoneMas> TblZoneMas { get; set; } = null!;
        public virtual DbSet<TblRegionMas> TblRegionMas { get; set; } = null!;
        public virtual DbSet<TblAreaMas> TblAreaMas { get; set; } = null!;
        public virtual DbSet<TblLocalityMas> TblLocalityMas { get; set; } = null!;

        public virtual DbSet<TblSysDefaults> TblSysDefaults { get; set; } = null!;
        public virtual DbSet<TblLocationMas> TblLocationMas { get; set; } = null!;
        // public virtual DbSet<TblStationMas> TblStationMas { get; set; } = null!;
        public virtual DbSet<TblSalesCrNotetrn> TblSalesCrNotetrn { get; set; } = null!;
        public virtual DbSet<TblSalesCrNotedtl> TblSalesCrNotedtl { get; set; } = null!;
        public virtual DbSet<TblVoucherTrn> TblVoucherTrn { get; set; } = null!;
        public virtual DbSet<TblVoucherDtl> TblVoucherDtl { get; set; } = null!;
        public virtual DbSet<TblWalletMas> TblWalletMas { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               // optionsBuilder.UseSqlServer("server=DESKTOP-OM06UCD\\SQLEXPRESS;database=POSdb1;Trusted_Connection=True;TrustServerCertificate=True");
                optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=jaipursoftdata;uid=jaipurdatauser;pwd=btof5zxmgjlyusvnehdc;TrustServerCertificate=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Ignore<TblBranchMas>();
            //modelBuilder.Ignore<TblCompany>();
            //modelBuilder.Ignore<TblEmployeeMas>();
            //modelBuilder.Ignore<TblUserMas>();
            //modelBuilder.Ignore<TblCustomerMas>();
            //modelBuilder.Ignore<TblVendorMas>();
            //modelBuilder.Ignore<TblFormMas>();
            //modelBuilder.Ignore<TblGridStructer>();
            //modelBuilder.Ignore<TblCategoryMas>();
            //modelBuilder.Ignore<TblProductMas>();
            //modelBuilder.Ignore<TblBankMas>();
            //modelBuilder.Ignore<TblSeriesMas>();
            //modelBuilder.Ignore<TblErrorLog>();
            //modelBuilder.Ignore<TblSalesOrdertrn>();
            //modelBuilder.Ignore<TblSalesOrderdtl>();
            //modelBuilder.Ignore<TblSalesInvoicetrn>();
            //modelBuilder.Ignore<TblSalesInvoicedtl>();
            //modelBuilder.Ignore<TblPurchaseOrdertrn>();
            //modelBuilder.Ignore<TblPurchaseOrderdtl>();
            //modelBuilder.Ignore<TblPurchaseInvoicetrn>();
            //modelBuilder.Ignore<TblPurchaseInvoicedtl>();
            //modelBuilder.Ignore<TblSalesChallantrn>();
            //modelBuilder.Ignore<TblSalesChallandtl>();
            //modelBuilder.Ignore<TblProdLotDtl>();
            //modelBuilder.Ignore<TblProdStockDtl>();

        }
    }
}
