﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSRepository.Data;

#nullable disable

namespace SSRepository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240208161053_mig-k-third")]
    partial class migkthird
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SSRepository.Data.TblSalesInvoicedtl", b =>
                {
                    b.Property<long>("PkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PkId"));

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<long>("FKSeriesId")
                        .HasColumnType("bigint");

                    b.Property<long>("FKUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("FkId")
                        .HasColumnType("bigint");

                    b.Property<long>("FkLotId")
                        .HasColumnType("bigint");

                    b.Property<long>("FkProductId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("FreeQty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("GrossAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ICAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ICRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LotDisc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("NetAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Qty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RateUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SCAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SCRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SchemeDisc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SchemeDiscAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SchemeDiscType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Src")
                        .HasColumnType("int");

                    b.Property<decimal>("TradeDisc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TradeDiscAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TradeDiscType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sno")
                        .HasColumnType("int");

                    b.HasKey("PkId");

                    b.ToTable("tblSalesInvoice_dtl", "dbo");
                });

            modelBuilder.Entity("SSRepository.Data.TblSalesInvoicetrn", b =>
                {
                    b.Property<long>("PkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PkId"));

                    b.Property<bool>("Cash")
                        .HasColumnType("bit");

                    b.Property<decimal>("CashAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CashDiscType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("CashDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CashDiscountAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Cheque")
                        .HasColumnType("bit");

                    b.Property<decimal>("ChequeAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ChequeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChequeNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Credit")
                        .HasColumnType("bit");

                    b.Property<decimal>("CreditAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CreditDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("EntryNo")
                        .HasColumnType("bigint");

                    b.Property<long>("FKBankChequeID")
                        .HasColumnType("bigint");

                    b.Property<long>("FKSeriesId")
                        .HasColumnType("bigint");

                    b.Property<long>("FKUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("FkPartyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("GRDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GRNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("GrossAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("NetAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OtherCharge")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("RoundOfDiff")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SgstAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Shipping")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Src")
                        .HasColumnType("int");

                    b.Property<string>("Statu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TaxAmt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TranAlias")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PkId");

                    b.ToTable("tblSalesInvoice_trn", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
