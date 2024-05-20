using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace SSRepository.Data
{
    [Table("tblProduct_mas", Schema = "dbo")]
    public partial class TblProductMas : TblBase, IEntity
    {
        [Key]
        public long PkProductId { get; set; }

        public string? Alias { get; set; }

        public string? Product { get; set; }

        public string? Brand { get; set; }

        public string? Strength { get; set; }

        public string? Unit1 { get; set; }

        public decimal ProdConv1 { get; set; }

        public string? Unit2 { get; set; }

        public bool SellLoose { get; set; }

        public decimal? ProdConv2 { get; set; }

        public string? Unit3 { get; set; }

        public string? CaseLot { get; set; }

        public string? NameToDisplay { get; set; }

        public string? NameToPrint { get; set; }

        public bool IsExpiryApplied { get; set; }

        public bool IsMfgDateApplied { get; set; }

        public bool IsUniqueIDApplied { get; set; }

        public bool IsColorApplied { get; set; }

        public bool IsBarCodeApplied { get; set; }

        public bool IsBatchApplied { get; set; }

        public bool IsVirtual { get; set; }

        public string? Description { get; set; }

        public long FKProdCatgId { get; set; }

        public long? FKMktGroupID { get; set; }

        public long? FKMfgGroupId { get; set; }

        public string? ShelfID { get; set; }

        public int? MinStock { get; set; }

        public int? MaxStock { get; set; }

        public decimal MRP { get; set; }

        public decimal SaleRate { get; set; }

        public decimal TradeRate { get; set; }

        public decimal DistributionRate { get; set; }

        public decimal SuggestedRate { get; set; }

        public string? MRPSaleRateUnit { get; set; }

        public decimal PurchaseRate { get; set; }

        public decimal CostRate { get; set; }

        public string? PurchaseRateUnit { get; set; }

        public bool AddLT { get; set; }

        public long Barcode { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Length { get; set; }

        public string? WeightUnit { get; set; }

        public string? HeightUnit { get; set; }

        public string? Status { get; set; }

        public DateTime? DiscDate { get; set; }

        public decimal? IncreaseSaleRateBy { get; set; }

        public bool KeepStock { get; set; }

        public bool AllowDecimal { get; set; }

        public int? BestBefore { get; set; }

        public string? BestBeforeUnit { get; set; }

        public string? SKUDefinition { get; set; }

        public decimal TradeDisc { get; set; }

        public int? MinDays { get; set; }

        public int? MaxDays { get; set; }

        public long? FKGenericID1 { get; set; }

        public long? FKGenericID2 { get; set; }

        public int? BoxSize { get; set; }

        public long? FkUnitId { get; set; }

        public string? Genration { get; set; }

        public string? CodingScheme { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        public int Src { get; set; }

        public long FKUserId { get; set; }

        public long? FkBrandId { get; set; }

        public string? Image { get; set; }

        public string? HSNCode { get; set; }

        public long? FKTaxID { get; set; }
    }
}