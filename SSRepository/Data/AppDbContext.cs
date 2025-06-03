using LMS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.SqlServer.Server;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SSRepository.Data
{
    public partial class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor)
            : base(options)
        {
            _contextAccessor = contextAccessor;
        }

        public virtual DbSet<TblImgRemarkMas> TblImgRemarkMas { get; set; } = null!;
        public virtual DbSet<TblBranchMas> TblBranchMas { get; set; } = null!;
        public virtual DbSet<TblCompany> TblCompanies { get; set; } = null!;
        public virtual DbSet<TblEmployeeMas> TblEmployeeMas { get; set; } = null!;
        public virtual DbSet<TblReferByMas> TblReferByMas { get; set; } = null!;
        public virtual DbSet<TblUserMas> TblUserMas { get; set; } = null!;

        public virtual DbSet<TblUserLocLnk> TblUserLocLnk { get; set; } = null!;
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
        public virtual DbSet<TblProductQTYBarcode> TblProductQTYBarcode { get; set; } = null!;
        public virtual DbSet<TblPromotionMas> TblPromotionMas { get; set; } = null!;
        public virtual DbSet<TblPromotionLocationLnk> TblPromotionLocationLnk { get; set; } = null!;
        public virtual DbSet<TblPromotionLnk> TblPromotionLnk { get; set; } = null!;
        public virtual DbSet<TblRecipeMas> TblRecipeMas { get; set; } = null!;
        public virtual DbSet<TblRecipeDtl> TblRecipeDtl { get; set; } = null!;
        public virtual DbSet<TblRoleMas> TblRoleMas { get; set; } = null!;
        public virtual DbSet<TblRoleDtl> TblRoleDtl { get; set; } = null!;
        public virtual DbSet<TblUnitMas> TblUnitMas { get; set; } = null!;
        public virtual DbSet<TblJobWorktrn> TblJobWorktrn { get; set; } = null!;
        public virtual DbSet<TblJobWorkdtl> TblJobWorkdtl { get; set; } = null!;
        public virtual DbSet<TblMasterLogDtl> TblMasterLogDtl { get; set; } = null!;
        public virtual DbSet<TblEWayDtlLnk> TblEWayDtlLnk { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Local
                //optionsBuilder.UseSqlServer("server=DESKTOP-OM06UCD\\SQLEXPRESS;database=Jp_POSModel;Trusted_Connection=True;TrustServerCertificate=True");

                //Live https://jaipursoft.com/
                // optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=jaipursoftdata;uid=jaipurdatauser;pwd=btof5zxmgjlyusvnehdc;TrustServerCertificate=True");

                //Live https://phulera.jaipursoft.com/
                //optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=phulera;uid=phuleradb;pwd=Pg4@es2#;TrustServerCertificate=True");

                //New https://annu.jaipursoft.com/
                //optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=annujaipurdb;uid=anjpdbus;pwd=R2#sd#S$;TrustServerCertificate=True");

                //New https://smdress.jaipursoft.com/
                //optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=smdress;uid=smdressdb;pwd=SM#$Dress21;TrustServerCertificate=True");

                optionsBuilder.UseSqlServer(_contextAccessor.HttpContext.Session.GetString("ConnectionString"));

                //General.Encrypt(_contextAccessor.HttpContext.Session.GetString("ConnectionString"))


            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TblFormMas>(entity =>
            {
                // Table name and primary key
                entity.ToTable("tblForm_Mas");
                entity.HasKey(e => e.PKFormID);

                // Columns
                entity.Property(e => e.PKFormID).IsRequired();
                entity.Property(e => e.FKMasterFormID);
                entity.Property(e => e.SeqNo).HasDefaultValue(0);
                entity.Property(e => e.FormName).HasMaxLength(200);
                entity.Property(e => e.ShortName).HasMaxLength(100);
                entity.Property(e => e.ShortCut).HasMaxLength(100);
                entity.Property(e => e.ToolTip).HasMaxLength(500);
                entity.Property(e => e.Image).HasMaxLength(200);
                entity.Property(e => e.FormType).HasMaxLength(1).IsFixedLength();
                entity.Property(e => e.WebURL).HasMaxLength(200);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Self-referencing foreign key
                entity.HasOne(e => e.ParentForm)
                      .WithMany(e => e.ChildForms)
                      .HasForeignKey(e => e.FKMasterFormID)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TblLocationMas>(entity =>
            {
                entity.HasKey(e => e.PkLocationID);

                entity.ToTable("tblLocation_mas");
                entity.Property(e => e.PkLocationID)
            .HasColumnName("PkLocationID")
            .IsRequired();

                entity.Property(e => e.Location)
                    .HasColumnName("Location")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Alias)
                    .HasColumnName("Alias")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsBillingLocation)
                    .HasColumnName("IsBillingLocation")
                    .HasDefaultValue(false);

                entity.Property(e => e.IsAllProduct)
                    .HasColumnName("IsAllProduct")
                    .HasDefaultValue(false);

                entity.Property(e => e.IsAllCustomer)
                    .HasColumnName("IsAllCustomer")
                    .HasDefaultValue(false);

                entity.Property(e => e.IsAllVendor)
                    .HasColumnName("IsAllVendor")
                    .HasDefaultValue(false);

                entity.Property(e => e.Address)
                    .HasColumnName("Address")
                    .HasMaxLength(500)
                    .IsUnicode(true);

                entity.Property(e => e.FkStationID)
                    .HasColumnName("FkStationID");

                entity.Property(e => e.FkLocalityID)
                    .HasColumnName("FkLocalityID");

                entity.Property(e => e.Pincode)
                    .HasColumnName("Pincode")
                    .HasMaxLength(10);

                entity.Property(e => e.Phone1)
                    .HasColumnName("Phone1")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Phone2)
                    .HasColumnName("Phone2")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasColumnName("Fax")
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasColumnName("Website")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsDifferentTax)
                    .HasColumnName("IsDifferentTax")
                    .HasDefaultValue(false);

                entity.Property(e => e.FkAccountID)
                    .HasColumnName("FkAccountID");

                entity.Property(e => e.FkBranchID)
                    .HasColumnName("FkBranchID")
                    .IsRequired();

                entity.Property(e => e.IsAllCostCenter)
                    .HasColumnName("IsAllCostCenter")
                    .HasDefaultValue(false);

                entity.Property(e => e.IsAllAccount)
                    .HasColumnName("IsAllAccount")
                    .HasDefaultValue(false);

                entity.Property(e => e.FkCityId)
                    .HasColumnName("FkCityId")
                    .IsRequired();

                entity.Property(e => e.State)
                    .HasColumnName("State")
                    .HasMaxLength(100);

                entity.HasOne(e => e.branchMas)
                    .WithMany()
                    .HasForeignKey(e => e.FkBranchID)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion

                entity.HasOne(e => e.accountMas)
                    .WithMany()
                    .HasForeignKey(e => e.FkAccountID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.stationMas)
                    .WithMany()
                    .HasForeignKey(e => e.FkStationID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.localityMas)
                    .WithMany()
                    .HasForeignKey(e => e.FkLocalityID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UserMas)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblUserLocLnk>(entity =>
            {
                entity.HasKey(e => new { e.FKUserID, e.FKLocationID });

                entity.Property(e => e.FKUserID)
                    .HasColumnType("bigint")
                    .IsRequired();

                entity.Property(e => e.FKLocationID)
                    .HasColumnType("bigint")
                    .IsRequired();

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKUser) // Navigation property
               .WithMany(l => l.LocationUsers)  // Navigation collection in TblUserMas
               .HasForeignKey(e => e.FKUserID) // FK in TblUserLocLnk
               .HasPrincipalKey(l => l.PkUserId) // Maps to correct PK in TblUserMas
               .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(e => e.FKLocation) // Navigation property
                .WithMany(l => l.LocationUsers)  // Navigation collection in TblLocationMas
                .HasForeignKey(e => e.FKLocationID) // FK in TblUserLocLnk
                .HasPrincipalKey(l => l.PkLocationID) // Maps to correct PK in TblLocationMas
                .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<TblAccountGroupMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKAccountGroupMas)
                       .WithMany()
                       .HasForeignKey(e => e.FkAccountGroupId)
                       .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<TblAccountMas>(entity =>
            {

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKLocality)
                   .WithMany()
                   .HasForeignKey(e => e.FkLocalityId)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKStation)
                   .WithMany()
                   .HasForeignKey(e => e.FkStationId)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKBank)
                 .WithMany()
                 .HasForeignKey(e => e.FKBankID)
                 .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKAccountGroupMas)
                      .WithMany()
                      .HasForeignKey(e => e.FkAccountGroupId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<TblBankMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblUserMas>(entity =>
            {
                entity.Property(e => e.CreationDate)
      .HasColumnType("datetime")
      .ValueGeneratedOnAdd();

                entity.HasOne(e => e.FkEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.FkEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FkBranch)
                   .WithMany()
                   .HasForeignKey(e => e.FkBranchId)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FkRole)
                   .WithMany()
                   .HasForeignKey(e => e.FkRoleId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblCustomerMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKAccount)
                       .WithMany()
                       .HasForeignKey(e => e.FkAccountID)
                       .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKCity)
                      .WithMany()
                      .HasForeignKey(e => e.FkCityId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblVendorMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKAccount)
                       .WithMany()
                       .HasForeignKey(e => e.FkAccountID)
                       .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKCity)
                       .WithMany()
                       .HasForeignKey(e => e.FkCityId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblBrandMas>(entity =>
            {

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<TblProductMas>(entity =>
            {
                entity.HasOne(e => e.FkBrand)
                    .WithMany()
                    .HasForeignKey(e => e.FkBrandId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FkUnit)
                   .WithMany()
                   .HasForeignKey(e => e.FkUnitId)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FkCategory)
                  .WithMany()
                  .HasForeignKey(e => e.FKProdCatgId)
                  .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<TblPromotionMas>(entity =>
            {
                entity.HasOne(e => e.FkBrand)
                    .WithMany()
                    .HasForeignKey(e => e.FkBrandId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<TblUnitMas>(entity =>
            {

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<TblEWayDtlLnk>(entity =>
            {
                entity.HasKey(e => new { e.FKID, e.FkSeriesId });
            });

            modelBuilder.Entity<TblCategoryGroupMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKCategoryGroupMas)
                   .WithMany()
                   .HasForeignKey(e => e.FkCategoryGroupId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblCategoryMas>(entity =>
            {
                entity.HasOne(e => e.FKCategoryGroupMas)
                  .WithMany()
                  .HasForeignKey(e => e.FkCategoryGroupId)
                  .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblCityMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblCountryMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblStateMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKCountry)
                   .WithMany()
                   .HasForeignKey(e => e.FkCountryId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblReferByMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKCity)
                   .WithMany()
                   .HasForeignKey(e => e.FkCityId)
                   .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<TblDistrictMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKState)
                 .WithMany()
                 .HasForeignKey(e => e.FkStateId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblStationMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKDistrict)
                 .WithMany()
                 .HasForeignKey(e => e.FkDistrictId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblZoneMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);
                 
            });

            modelBuilder.Entity<TblRegionMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKZone)
                 .WithMany()
                 .HasForeignKey(e => e.FkZoneId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblAreaMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKRegion)
                 .WithMany()
                 .HasForeignKey(e => e.FkRegionId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblLocalityMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FKArea)
                 .WithMany()
                 .HasForeignKey(e => e.FkAreaId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TblAccountLocLnk>(entity =>
            { 
                entity.Property(e => e.FKLocationID)
                    .HasColumnType("bigint")
                    .IsRequired(); 
            });

            modelBuilder.Entity<TblPromotionLocationLnk>(entity =>
            { 
                entity.Property(e => e.FKLocationId)
                    .HasColumnType("bigint")
                    .IsRequired(); 
            });

            modelBuilder.Entity<TblSeriesMas>(entity =>
            {
                entity.Property(e => e.FKLocationID)
                    .HasColumnType("bigint")
                    .IsRequired();
            });
            modelBuilder.Entity<TblBranchMas>(entity =>
            {
                entity.HasOne(e => e.FKUser)
                    .WithMany()
                    .HasForeignKey(e => e.FKUserID)
                    .OnDelete(DeleteBehavior.Restrict); 

                entity.HasOne(e => e.FKCity)
                      .WithMany()
                      .HasForeignKey(e => e.FkCityId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
