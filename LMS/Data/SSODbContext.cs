using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Data
{
    public partial class ssodbContext : DbContext
    {
        

        public ssodbContext(DbContextOptions<ssodbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblClientAppMa> TblClientAppMas { get; set; } = null!;
        public virtual DbSet<TblClientBranchAppLnk> TblClientBranchAppLnks { get; set; } = null!;
        public virtual DbSet<TblClientBranchAppUserLnk> TblClientBranchAppUserLnks { get; set; } = null!;
        public virtual DbSet<TblClientBranchMa> TblClientBranchMas { get; set; } = null!;
        public virtual DbSet<TblClientCountryMa> TblClientCountryMas { get; set; } = null!;
        public virtual DbSet<TblClientDistrictMa> TblClientDistrictMas { get; set; } = null!;
        public virtual DbSet<TblClientLocMa> TblClientLocMas { get; set; } = null!;
        public virtual DbSet<TblClientOtpDtl> TblClientOtpDtls { get; set; } = null!;
        public virtual DbSet<TblClientRegAppLnk> TblClientRegAppLnks { get; set; } = null!;
        public virtual DbSet<TblClientRegMa> TblClientRegMas { get; set; } = null!;
        public virtual DbSet<TblClientRoleRightDtl> TblClientRoleRightDtls { get; set; } = null!;
        public virtual DbSet<TblClientStateMa> TblClientStateMas { get; set; } = null!;
        public virtual DbSet<TblClientStationMa> TblClientStationMas { get; set; } = null!;
        public virtual DbSet<TblClientSystemDef> TblClientSystemDefs { get; set; } = null!;
        public virtual DbSet<TblClientUserDeviceDtl> TblClientUserDeviceDtls { get; set; } = null!;
        public virtual DbSet<TblClientUserLogDtl> TblClientUserLogDtls { get; set; } = null!;
        public virtual DbSet<TblClientUserMa> TblClientUserMas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=ssodb;uid=ssodbuser;pwd=Su#2fg!3;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<TblClientAppMa>(entity =>
            {
                entity.HasKey(e => e.PkappId);

                entity.ToTable("tblClientApp_Mas", "dbo");

                entity.Property(e => e.PkappId).HasColumnName("PKAppID");

                entity.Property(e => e.AppName).HasMaxLength(50);

                entity.Property(e => e.AppVersion).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<TblClientBranchAppLnk>(entity =>
            {
                entity.HasKey(e => e.PkbranchAppId);

                entity.ToTable("tblClientBranchApp_Lnk", "dbo");

                entity.Property(e => e.PkbranchAppId).HasColumnName("PKBranchAppID");

                entity.Property(e => e.FkappId).HasColumnName("FKAppID");

                entity.Property(e => e.FkbranchId).HasColumnName("FKBranchID");

                entity.HasOne(d => d.Fkapp)
                    .WithMany(p => p.TblClientBranchAppLnks)
                    .HasForeignKey(d => d.FkappId);

                entity.HasOne(d => d.Fkbranch)
                    .WithMany(p => p.TblClientBranchAppLnks)
                    .HasForeignKey(d => d.FkbranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientAppBranch_Lnk_tblClientBranch_Mas");
            });

            modelBuilder.Entity<TblClientBranchAppUserLnk>(entity =>
            {
                entity.HasKey(e => e.PkbranchAppUserId);

                entity.ToTable("tblClientBranchAppUser_Lnk", "dbo");

                entity.Property(e => e.PkbranchAppUserId).HasColumnName("PKBranchAppUserID");

                entity.Property(e => e.FkbranchAppId).HasColumnName("FKBranchAppID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.HasOne(d => d.FkbranchApp)
                    .WithMany(p => p.TblClientBranchAppUserLnks)
                    .HasForeignKey(d => d.FkbranchAppId)
                    .HasConstraintName("FK_tblClientAppBranchUser_Lnk_tblClientAppBranch_Lnk");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientBranchAppUserLnks)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientAppBranchUser_Lnk_tblClientUser_Mas");
            });

            modelBuilder.Entity<TblClientBranchMa>(entity =>
            {
                entity.HasKey(e => e.PkbranchId);

                entity.ToTable("tblClientBranch_Mas", "dbo");

                entity.Property(e => e.PkbranchId).HasColumnName("PKBranchID");

                entity.Property(e => e.AccountNo).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Alias).HasMaxLength(30);

                entity.Property(e => e.Bank).HasMaxLength(50);

                entity.Property(e => e.Branch).HasMaxLength(100);

                entity.Property(e => e.BranchId).HasColumnName("BranchID");

                entity.Property(e => e.BranchType).HasMaxLength(1);

                entity.Property(e => e.CompIdate)
                    .HasColumnType("date")
                    .HasColumnName("CompIDate");

                entity.Property(e => e.CompVdate)
                    .HasColumnType("date")
                    .HasColumnName("CompVDate");

                entity.Property(e => e.Contact).HasMaxLength(100);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Customer).HasMaxLength(100);

                entity.Property(e => e.DataBaseName).HasMaxLength(50);

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.Dbname)
                    .HasMaxLength(100)
                    .HasColumnName("DBName");

                entity.Property(e => e.Dbpassword)
                    .HasMaxLength(50)
                    .HasColumnName("DBPassword");

                entity.Property(e => e.DbuserName)
                    .HasMaxLength(50)
                    .HasColumnName("DBUserName");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(20);

                entity.Property(e => e.FkclientRegId).HasColumnName("FKClientRegID");

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkholdLocationId).HasColumnName("FKHoldLocationID");

                entity.Property(e => e.FknonSaleableId).HasColumnName("FKNonSaleableID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.Gstdate)
                    .HasColumnType("date")
                    .HasColumnName("GSTDate");

                entity.Property(e => e.Gstno)
                    .HasMaxLength(15)
                    .HasColumnName("GSTNo");

                entity.Property(e => e.Gstvdate)
                    .HasColumnType("date")
                    .HasColumnName("GSTVDate");

                entity.Property(e => e.Ifsc)
                    .HasMaxLength(50)
                    .HasColumnName("IFSC");

                entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");

                entity.Property(e => e.Locality).HasMaxLength(50);

                entity.Property(e => e.MailId).HasMaxLength(100);

                entity.Property(e => e.MailPwd).HasMaxLength(50);

                entity.Property(e => e.MailServer).HasMaxLength(100);

                entity.Property(e => e.MastersFromHo).HasColumnName("MastersFromHO");

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.MsgApi).HasColumnName("MsgAPI");

                entity.Property(e => e.Pan)
                    .HasMaxLength(10)
                    .HasColumnName("PAN");

                entity.Property(e => e.Password).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Pincode).HasMaxLength(20);

                entity.Property(e => e.ServerName).HasMaxLength(50);

                entity.Property(e => e.Station).HasMaxLength(50);

                entity.Property(e => e.Swilalias)
                    .HasMaxLength(30)
                    .HasColumnName("SWILAlias");

                entity.Property(e => e.UdyamIncDate).HasColumnType("datetime");

                entity.Property(e => e.UdyamRegDate).HasColumnType("datetime");

                entity.Property(e => e.UdyamRegNo).HasMaxLength(100);

                entity.Property(e => e.Upi)
                    .HasMaxLength(100)
                    .HasColumnName("UPI");

                entity.Property(e => e.Vendor).HasMaxLength(100);

                entity.Property(e => e.Website).HasMaxLength(50);

                entity.Property(e => e.WhatsAppApi).HasColumnName("WhatsAppAPI");

                entity.HasOne(d => d.FkclientReg)
                    .WithMany(p => p.TblClientBranchMas)
                    .HasForeignKey(d => d.FkclientRegId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientBranch_Mas_tblClientReg_Mas");

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.TblClientBranchMaFkcreatedBies)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientBranch_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.FkholdLocation)
                    .WithMany(p => p.TblClientBranchMaFkholdLocations)
                    .HasForeignKey(d => d.FkholdLocationId)
                    .HasConstraintName("FK_tblClientBranch_Mas_tblClientLoc_Mas_HoldLoc");

                entity.HasOne(d => d.FknonSaleable)
                    .WithMany(p => p.TblClientBranchMaFknonSaleables)
                    .HasForeignKey(d => d.FknonSaleableId)
                    .HasConstraintName("FK_tblClientBranch_Mas_tblClientLoc_Mas_NonSaleableLoc");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientBranchMaFkusers)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientBranch_Mas_tblClientUser_Mas");
            });

            modelBuilder.Entity<TblClientCountryMa>(entity =>
            {
                entity.HasKey(e => e.PkcountryId);

                entity.ToTable("tblClientCountry_Mas", "dbo");

                entity.Property(e => e.PkcountryId).HasColumnName("PKCountryID");

                entity.Property(e => e.Capital).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.TblClientCountryMaFkcreatedBies)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientCountry_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientCountryMaFkusers)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientCountry_Mas_tblClientUser_Mas");
            });

            modelBuilder.Entity<TblClientDistrictMa>(entity =>
            {
                entity.HasKey(e => e.PkdistrictId);

                entity.ToTable("tblClientDistrict_Mas", "dbo");

                entity.Property(e => e.PkdistrictId).HasColumnName("PKDistrictID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkstateId).HasColumnName("FKStateID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.TblClientDistrictMaFkcreatedBies)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientDistrict_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.Fkstate)
                    .WithMany(p => p.TblClientDistrictMas)
                    .HasForeignKey(d => d.FkstateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientDistrict_Mas_tblClientState_Mas");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientDistrictMaFkusers)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientDistrict_Mas_tblClientUser_Mas");
            });

            modelBuilder.Entity<TblClientLocMa>(entity =>
            {
                entity.HasKey(e => e.PklocationId);

                entity.ToTable("tblClientLoc_Mas", "dbo");

                entity.Property(e => e.PklocationId).HasColumnName("PKLocationID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Alias).HasMaxLength(50);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE_MODIFIED");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(20);

                entity.Property(e => e.FkbranchId).HasColumnName("FKBranchID");

                entity.Property(e => e.FkclientRegId).HasColumnName("FKClientRegID");

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.Locality).HasMaxLength(50);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Pincode).HasMaxLength(20);

                entity.Property(e => e.PostingAc).HasMaxLength(100);

                entity.Property(e => e.Station).HasMaxLength(50);

                entity.Property(e => e.StockLocation).HasMaxLength(50);

                entity.Property(e => e.TblClientRegMasPkclientRegId).HasColumnName("TblClientRegMasPKClientRegID");

                entity.Property(e => e.Website).HasMaxLength(50);

                entity.HasOne(d => d.Fkbranch)
                    .WithMany(p => p.TblClientLocMas)
                    .HasForeignKey(d => d.FkbranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbClientLoc_Mas_tblClientBranch_Mas");

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.TblClientLocMaFkcreatedBies)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientLoc_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientLocMaFkusers)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientLoc_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.TblClientRegMasPkclientReg)
                    .WithMany(p => p.TblClientLocMas)
                    .HasForeignKey(d => d.TblClientRegMasPkclientRegId);
            });

            modelBuilder.Entity<TblClientOtpDtl>(entity =>
            {
                entity.HasKey(e => new { e.Email, e.MobileNo });

                entity.ToTable("tblClientOTP_Dtl", "dbo");

                entity.Property(e => e.MobileNo).HasMaxLength(100);

                entity.Property(e => e.EmailOtp)
                    .HasMaxLength(6)
                    .HasColumnName("EmailOTP");

                entity.Property(e => e.Expiry).HasColumnType("datetime");

                entity.Property(e => e.MobileOtp)
                    .HasMaxLength(6)
                    .HasColumnName("MobileOTP");
            });

            modelBuilder.Entity<TblClientRegAppLnk>(entity =>
            {
                entity.HasKey(e => e.PkregAppId);

                entity.ToTable("tblClientRegApp_Lnk", "dbo");

                entity.Property(e => e.PkregAppId).HasColumnName("PKRegAppID");

                entity.Property(e => e.FkappId).HasColumnName("FKAppID");

                entity.Property(e => e.FkclientRegId).HasColumnName("FKClientRegID");

                entity.Property(e => e.ValidTill).HasColumnType("date");

                entity.HasOne(d => d.Fkapp)
                    .WithMany(p => p.TblClientRegAppLnks)
                    .HasForeignKey(d => d.FkappId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientApp_Lnk_tblClientApp_Mas");

                entity.HasOne(d => d.FkclientReg)
                    .WithMany(p => p.TblClientRegAppLnks)
                    .HasForeignKey(d => d.FkclientRegId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientApp_Lnk_tblClientReg_Mas");
            });

            modelBuilder.Entity<TblClientRegMa>(entity =>
            {
                entity.HasKey(e => e.PkclientRegId);

                entity.ToTable("tblClientReg_Mas", "dbo");

                entity.Property(e => e.PkclientRegId).HasColumnName("PKClientRegID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.BusinessDetail).HasMaxLength(500);

                entity.Property(e => e.ClientName).HasMaxLength(100);

                entity.Property(e => e.ConnectionString).HasMaxLength(300);

                entity.Property(e => e.Contact).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(200);

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.DomainName).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(20);

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.Gstno)
                    .HasMaxLength(30)
                    .HasColumnName("GSTNo");

                entity.Property(e => e.InstallationDate).HasColumnType("date");

                entity.Property(e => e.Locality).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.PanNo).HasMaxLength(10);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Pincode).HasMaxLength(20);

                entity.Property(e => e.RegNo).HasMaxLength(10);

                entity.Property(e => e.Sqldbsize)
                    .HasMaxLength(50)
                    .HasColumnName("SQLDBSize");

                entity.Property(e => e.State).HasMaxLength(200);

                entity.Property(e => e.Station).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(1);

                entity.Property(e => e.Turnover).HasMaxLength(100);

                entity.Property(e => e.ValidTill).HasColumnType("date");

                entity.Property(e => e.Version).HasMaxLength(100);

                entity.Property(e => e.Website).HasMaxLength(100);
            });

            modelBuilder.Entity<TblClientRoleRightDtl>(entity =>
            {
                entity.HasKey(e => e.PkroleRightId);

                entity.ToTable("tblClientRoleRight_Dtl", "dbo");

                entity.Property(e => e.PkroleRightId).HasColumnName("PKRoleRightID");

                entity.Property(e => e.FkformId).HasColumnName("FKFormID");

                entity.Property(e => e.FkroleId).HasColumnName("FKRoleID");
            });

            modelBuilder.Entity<TblClientStateMa>(entity =>
            {
                entity.HasKey(e => e.PkstateId);

                entity.ToTable("tblClientState_Mas", "dbo");

                entity.Property(e => e.PkstateId).HasColumnName("PKStateID");

                entity.Property(e => e.Capital).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(2);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.FkcountryId).HasColumnName("FKCountryID");

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(1);

                entity.HasOne(d => d.Fkcountry)
                    .WithMany(p => p.TblClientStateMas)
                    .HasForeignKey(d => d.FkcountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientState_Mas_tblClientState_Mas");

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.TblClientStateMaFkcreatedBies)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientState_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientStateMaFkusers)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientState_Mas_tblClientUser_Mas");
            });

            modelBuilder.Entity<TblClientStationMa>(entity =>
            {
                entity.HasKey(e => e.PkstationId);

                entity.ToTable("tblClientStation_Mas", "dbo");

                entity.Property(e => e.PkstationId).HasColumnName("PKStationID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkdistrictId).HasColumnName("FKDistrictID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.Station).HasMaxLength(50);

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.TblClientStationMaFkcreatedBies)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientStation_Mas_tblClientUser_Mas");

                entity.HasOne(d => d.Fkdistrict)
                    .WithMany(p => p.TblClientStationMas)
                    .HasForeignKey(d => d.FkdistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientStation_Mas_tblClientDistrict_Mas");

                entity.HasOne(d => d.Fkuser)
                    .WithMany(p => p.TblClientStationMaFkusers)
                    .HasForeignKey(d => d.FkuserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblClientStation_Mas_tblClientUser_Mas");
            });

            modelBuilder.Entity<TblClientSystemDef>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblClientSystemDef", "dbo");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.FkclientRegId).HasColumnName("FKClientRegID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.Pkid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PKID");

                entity.Property(e => e.SysDefKey).HasMaxLength(50);

                entity.Property(e => e.SysDefValue).HasMaxLength(2000);
            });

            modelBuilder.Entity<TblClientUserDeviceDtl>(entity =>
            {
                entity.HasKey(e => e.Pkid);

                entity.ToTable("tblClientUserDevice_Dtl", "dbo");

                entity.Property(e => e.Pkid).HasColumnName("PKID");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(100)
                    .HasColumnName("DeviceID");

                entity.Property(e => e.Expiry).HasColumnType("datetime");

                entity.Property(e => e.FkappId).HasColumnName("FKAppID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");
            });

            modelBuilder.Entity<TblClientUserLogDtl>(entity =>
            {
                entity.HasKey(e => e.PkuserLogId);

                entity.ToTable("tblClientUserLog_Dtl", "dbo");

                entity.Property(e => e.PkuserLogId).HasColumnName("PKUserLogID");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(100)
                    .HasColumnName("DeviceID");

                entity.Property(e => e.FkappId).HasColumnName("FKAppID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.LogStatus).HasMaxLength(1);

                entity.Property(e => e.LoginTime).HasColumnType("datetime");

                entity.Property(e => e.LogoffTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblClientUserMa>(entity =>
            {
                entity.HasKey(e => e.PkuserId);

                entity.ToTable("TblClientUser_Mas", "dbo");

                entity.Property(e => e.PkuserId).HasColumnName("PKUserID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Modified");

                entity.Property(e => e.ErpuserId)
                    .HasMaxLength(100)
                    .HasColumnName("ERPUserID");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.FkbranchId).HasColumnName("FKBranchID");

                entity.Property(e => e.FkclientRegId).HasColumnName("FKClientRegID");

                entity.Property(e => e.FkcreatedById).HasColumnName("FKCreatedByID");

                entity.Property(e => e.FkuserId).HasColumnName("FKUserID");

                entity.Property(e => e.MailId)
                    .HasMaxLength(100)
                    .HasColumnName("MailID");

                entity.Property(e => e.MailPwd)
                    .HasMaxLength(50)
                    .HasColumnName("MailPWD");

                entity.Property(e => e.MailServer).HasMaxLength(100);

                entity.Property(e => e.MobileNo).HasMaxLength(20);

                entity.Property(e => e.MsgApi).HasColumnName("MsgAPI");

                entity.Property(e => e.Pwd).HasMaxLength(100);

                entity.Property(e => e.PwdExpDate).HasColumnType("date");

                entity.Property(e => e.RefreshTokenExpiry).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'A')");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'E')");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName).HasMaxLength(30);

                entity.Property(e => e.WhatsAppApi).HasColumnName("WhatsAppAPI");

                entity.HasOne(d => d.Fkbranch)
                    .WithMany(p => p.TblClientUserMas)
                    .HasForeignKey(d => d.FkbranchId)
                    .HasConstraintName("FK_tblClientUser_Mas_tblClientBranch_Mas");

                entity.HasOne(d => d.FkclientReg)
                    .WithMany(p => p.TblClientUserMas)
                    .HasForeignKey(d => d.FkclientRegId)
                    .HasConstraintName("FK_tblClientUser_Mas_tblClientReg_Mas");

                entity.HasOne(d => d.FkcreatedBy)
                    .WithMany(p => p.InverseFkcreatedBy)
                    .HasForeignKey(d => d.FkcreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedBy_tblClientUser_Mas_tblClientUser_Mas");

                entity.HasMany(d => d.Fklocations)
                    .WithMany(p => p.Fkusers)
                    .UsingEntity<Dictionary<string, object>>(
                        "TblClientUserLocLnk",
                        l => l.HasOne<TblClientLocMa>().WithMany().HasForeignKey("FklocationId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_tblClientUserLoc_Lnk_tblClientLoc_Mas"),
                        r => r.HasOne<TblClientUserMa>().WithMany().HasForeignKey("FkuserId").HasConstraintName("FK_tblClientUserLoc_Lnk_tblClientUser_Mas"),
                        j =>
                        {
                            j.HasKey("FkuserId", "FklocationId");

                            j.ToTable("tblClientUserLoc_Lnk", "dbo");

                            j.IndexerProperty<long>("FkuserId").HasColumnName("FKUserID");

                            j.IndexerProperty<long>("FklocationId").HasColumnName("FKLocationID");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
