using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SSAdmin.Model1;

public partial class PapapizzaContext : DbContext
{
    public PapapizzaContext()
    {
    }

    public PapapizzaContext(DbContextOptions<PapapizzaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAddonMa> TblAddonMas { get; set; }

    public virtual DbSet<TblAddonMa1> TblAddonMas1 { get; set; }

    public virtual DbSet<TblAddressDtl> TblAddressDtls { get; set; }

    public virtual DbSet<TblBankDtl> TblBankDtls { get; set; }

    public virtual DbSet<TblBranch> TblBranches { get; set; }

    public virtual DbSet<TblCategoryMa> TblCategoryMas { get; set; }

    public virtual DbSet<TblCompany> TblCompanies { get; set; }

    public virtual DbSet<TblCompanyBranchLnk> TblCompanyBranchLnks { get; set; }

    public virtual DbSet<TblCustomerMa> TblCustomerMas { get; set; }

    public virtual DbSet<TblDeliveryBoyMa> TblDeliveryBoyMas { get; set; }

    public virtual DbSet<TblEmployeeMa> TblEmployeeMas { get; set; }

    public virtual DbSet<TblFormMa> TblFormMas { get; set; }

    public virtual DbSet<TblLogDtl> TblLogDtls { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblOrderItem> TblOrderItems { get; set; }

    public virtual DbSet<TblOrderPayment> TblOrderPayments { get; set; }

    public virtual DbSet<TblProductAddonLnk> TblProductAddonLnks { get; set; }

    public virtual DbSet<TblProductMa> TblProductMas { get; set; }

    public virtual DbSet<TblProductVariationLnk> TblProductVariationLnks { get; set; }

    public virtual DbSet<TblTableAreaMa> TblTableAreaMas { get; set; }

    public virtual DbSet<TblTableMa> TblTableMas { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserBranchLnk> TblUserBranchLnks { get; set; }

    public virtual DbSet<TblVariationMa> TblVariationMas { get; set; }

    public virtual DbSet<TblVendorMa> TblVendorMas { get; set; }

    public virtual DbSet<TblWalletDetail> TblWalletDetails { get; set; }

    public virtual DbSet<TblWalletStatus> TblWalletStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=papapizza;uid=papapizzauser;pwd=Papa@554piza;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("papapizzauser");

        modelBuilder.Entity<TblAddonMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblAddon__A7C03FF8B835EB6F");

            entity.ToTable("tblAddon_mas", "dbo");

            entity.Property(e => e.AddonName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblAddonMa1>(entity =>
        {
            entity.HasKey(e => e.PkId);

            entity.ToTable("tblAddon_mas");
        });

        modelBuilder.Entity<TblAddressDtl>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblAddre__A7C03FF8046C535E");

            entity.ToTable("tblAddress_dtl", "dbo");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('India')");
            entity.Property(e => e.Isprimary).HasColumnName("isprimary");
            entity.Property(e => e.Line1)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Line2)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pin)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PostOffice)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.State)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblBankDtl>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblBank___A7C03FF8753C6634");

            entity.ToTable("tblBank_dtl", "dbo");

            entity.Property(e => e.Acno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ACno");
            entity.Property(e => e.Bank)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HolderName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ifsc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IFSC");
            entity.Property(e => e.Isprimary).HasColumnName("isprimary");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblBranch>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblBranc__A7C03FF80D00D8E7");

            entity.ToTable("tblBranch", "dbo");

            entity.Property(e => e.Address)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('India')");
            entity.Property(e => e.Email)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.FkCompanyId).HasColumnName("Fk_CompanyId");
            entity.Property(e => e.Mobile)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblCategoryMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblCateg__A7C03FF84B854B2C");

            entity.ToTable("tblCategory_mas", "dbo");

            entity.Property(e => e.CategoryLogo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CategoryName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.CategoryOnlineName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblCompany>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblCompa__A7C03FF84B2F5525");

            entity.ToTable("tblCompany", "dbo");

            entity.Property(e => e.Address)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Connection)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('India')");
            entity.Property(e => e.Email)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Gstn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GSTN");
            entity.Property(e => e.LogoImg)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThumbnailImg)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblCompanyBranchLnk>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblCompa__A7C03FF856F90C48");

            entity.ToTable("tblCompanyBranch_lnk", "dbo");

            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.FkCompanyId).HasColumnName("fkCompanyId");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");

            entity.HasOne(d => d.FkBranch).WithMany(p => p.TblCompanyBranchLnks)
                .HasForeignKey(d => d.FkBranchId)
                .HasConstraintName("FK__tblCompan__fkBra__440B1D61");

            entity.HasOne(d => d.FkCompany).WithMany(p => p.TblCompanyBranchLnks)
                .HasForeignKey(d => d.FkCompanyId)
                .HasConstraintName("FK__tblCompan__fkCom__4316F928");
        });

        modelBuilder.Entity<TblCustomerMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblCusto__A7C03FF8150F8FD1");

            entity.ToTable("tblCustomer_Mas", "dbo", tb => tb.HasTrigger("trg_Cust_Insert"));

            entity.Property(e => e.Aadhar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AadharCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AadharCardBack)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Gstno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GSTNO");
            entity.Property(e => e.Marital)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PanCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Passport)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Signature)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblDeliveryBoyMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblDeliv__A7C03FF8849483FD");

            entity.ToTable("tblDeliveryBoy_Mas", "dbo");

            entity.Property(e => e.Aadhar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AadharCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AadharCardBack)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Gstno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GSTNO");
            entity.Property(e => e.Marital)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PanCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Passport)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Signature)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblEmployeeMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblEmplo__A7C03FF8251EF0F0");

            entity.ToTable("tblEmployee_Mas", "dbo");

            entity.Property(e => e.Aadhar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AadharCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AadharCardBack)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Gstno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GSTNO");
            entity.Property(e => e.Marital)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PanCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Passport)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Signature)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblFormMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblForm___A7C03FF8D3953BFC");

            entity.ToTable("tblForm_Mas", "dbo");

            entity.Property(e => e.FkPid).HasColumnName("fk_PId");
            entity.Property(e => e.Icon).HasMaxLength(125);
            entity.Property(e => e.Name)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblLogDtl>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblLog_d__A7C03FF8E8103ACA");

            entity.ToTable("tblLog_dtl", "dbo");

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.FkFormId).HasColumnName("fkFormId");
            entity.Property(e => e.FkId).HasColumnName("fkId");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.PkId);

            entity.ToTable("tblOrder", "dbo");

            entity.Property(e => e.CgstAmt)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CGstAmt");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DueAmt)
                .HasComputedColumnSql("([NetAmt]-[PaidAmt])", false)
                .HasColumnType("decimal(19, 2)");
            entity.Property(e => e.GstAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IgstAmt)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("IGstAmt");
            entity.Property(e => e.NetAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OtherCharge).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Paymode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.SettleAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SgstAmt)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SGstAmt");
            entity.Property(e => e.Shipping).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.Statu)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaxableAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TipAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblOrderItem>(entity =>
        {
            entity.HasKey(e => e.PkId);

            entity.ToTable("tblOrderItem", "dbo");

            entity.Property(e => e.AddonIds).HasMaxLength(500);
            entity.Property(e => e.AddonNames).HasMaxLength(500);
            entity.Property(e => e.CgstAmt)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CGstAmt");
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GstAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IgstAmt)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("IGstAmt");
            entity.Property(e => e.Kotno).HasColumnName("KOTNo");
            entity.Property(e => e.NetAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Qty)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("QTY");
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.SgstAmt)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SGstAmt");
            entity.Property(e => e.Shipping).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.TaxableAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VariationName).HasMaxLength(500);
            entity.Property(e => e.VariationPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblOrderPayment>(entity =>
        {
            entity.HasKey(e => e.PkId);

            entity.ToTable("tblOrderPayment", "dbo");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Paymode).HasMaxLength(50);
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblProductAddonLnk>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblProdu__A7C03FF8858C37C7");

            entity.ToTable("tblProductAddon_lnk", "dbo");

            entity.Property(e => e.FkAddonIds)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fkAddonIds");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblProductMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblProdu__A7C03FF8343EBA1A");

            entity.ToTable("tblProduct_mas", "dbo");

            entity.Property(e => e.Choice)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DesktopType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Gos)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GOS");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblProductVariationLnk>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblProdu__A7C03FF8C4847C43");

            entity.ToTable("tblProductVariation_lnk", "dbo");

            entity.Property(e => e.FkAddonIds)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fkAddonIds");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblTableAreaMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblTable__A7C03FF86497F25F");

            entity.ToTable("tblTableArea_mas", "dbo");

            entity.Property(e => e.AreaName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Areatype)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblTableMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblTable__A7C03FF83B3EB851");

            entity.ToTable("tblTable_mas", "dbo");

            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.TableName)
                .HasMaxLength(125)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblUser__A7C03FF88696CB60");

            entity.ToTable("tblUser", "dbo");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ExpirePwddt).HasColumnType("datetime");
            entity.Property(e => e.Expiredt).HasColumnType("datetime");
            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.FkEmployeeId).HasColumnName("fk_EmployeeId");
            entity.Property(e => e.FkRegId).HasColumnName("fkRegId");
            entity.Property(e => e.FkRoleId).HasColumnName("fkRoleId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pwd)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.UserId)
                .HasMaxLength(120)
                .IsUnicode(false);

            entity.HasOne(d => d.FkReg).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.FkRegId)
                .HasConstraintName("FK__tblUser__fkRegId__3F466844");
        });

        modelBuilder.Entity<TblUserBranchLnk>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblUserB__A7C03FF8F77A3858");

            entity.ToTable("tblUserBranch_lnk", "dbo");

            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.FkUserId).HasColumnName("fkUserId");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");

            entity.HasOne(d => d.FkBranch).WithMany(p => p.TblUserBranchLnks)
                .HasForeignKey(d => d.FkBranchId)
                .HasConstraintName("FK__tblUserBr__fkBra__48CFD27E");

            entity.HasOne(d => d.FkUser).WithMany(p => p.TblUserBranchLnks)
                .HasForeignKey(d => d.FkUserId)
                .HasConstraintName("FK__tblUserBr__fkUse__47DBAE45");
        });

        modelBuilder.Entity<TblVariationMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblVaria__A7C03FF8D287515D");

            entity.ToTable("tblVariation_mas", "dbo");

            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(500);
            entity.Property(e => e.Source)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
            entity.Property(e => e.Unit)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.VariationName)
                .HasMaxLength(125)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblVendorMa>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__tblVendo__A7C03FF8E1C30871");

            entity.ToTable("tblVendor_Mas", "dbo");

            entity.Property(e => e.Aadhar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AadharCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AadharCardBack)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.FkBranchId).HasColumnName("fkBranchId");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Gstno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GSTNO");
            entity.Property(e => e.Marital)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(125)
                .IsUnicode(false);
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Pan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PanCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Passport)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Signature)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("srcId");
        });

        modelBuilder.Entity<TblWalletDetail>(entity =>
        {
            entity.HasKey(e => e.PkId);

            entity.ToTable("tbl_WalletDetail", "dbo");

            entity.Property(e => e.PkId).HasColumnName("pk_Id");
            entity.Property(e => e.ClossingBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ondt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Particulars)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RecFor)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("rec_for");
            entity.Property(e => e.RecForId).HasColumnName("rec_forId");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("src_Id");
            entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblWalletStatus>(entity =>
        {
            entity.HasKey(e => e.PkId);

            entity.ToTable("tbl_WalletStatus", "dbo");

            entity.Property(e => e.PkId).HasColumnName("pk_Id");
            entity.Property(e => e.BalAmount)
                .HasComputedColumnSql("([Cr]-[Dr])", false)
                .HasColumnType("decimal(13, 2)");
            entity.Property(e => e.Cr).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Dr).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.RecFor)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("rec_for");
            entity.Property(e => e.RecForId).HasColumnName("rec_forId");
            entity.Property(e => e.Src).HasColumnName("src");
            entity.Property(e => e.SrcId).HasColumnName("src_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
