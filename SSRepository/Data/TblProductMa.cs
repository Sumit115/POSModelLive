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
        public string Product { get; set; }
        public string? Brand { get; set; }
        public string? Strength { get; set; }
        public string? Unit1 { get; set; }
        public decimal ProdConv1 { get; set; }
        public string? Unit2 { get; set; }
        public Nullable<bool> SellLoose { get; set; }
        public Nullable<decimal> ProdConv2 { get; set; }
        public string? Unit3 { get; set; }
        public string? CaseLot { get; set; }
        public string? NameToDisplay { get; set; }
        public string? NameToPrint { get; set; }
        public Nullable<bool> IsExpiryApplied { get; set; }
        public Nullable<bool> IsMfgDateApplied { get; set; }
        public Nullable<bool> IsUniqueIDApplied { get; set; }
        public Nullable<bool> IsColorApplied { get; set; }
        public Nullable<bool> IsBarCodeApplied { get; set; }
        public Nullable<bool> IsBatchApplied { get; set; }
        public bool IsVirtual { get; set; }
        public string? Description { get; set; }
        public long FKProdCatgId { get; set; }
        public Nullable<long> FKMktGroupID { get; set; }
        public Nullable<long> FKMfgGroupId { get; set; }
        public string? ShelfID { get; set; }
        public Nullable<int> MinStock { get; set; }
        public Nullable<int> MaxStock { get; set; }
        public decimal MRP { get; set; }
        public decimal SaleRate { get; set; }
        public decimal TradeRate { get; set; }
        public decimal DistributionRate { get; set; }
        public decimal SuggestedRate { get; set; }
        public string? MRPSaleRateUnit { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal CostRate { get; set; }
        public string? PurchaseRateUnit { get; set; }
        public Nullable<bool> AddLT { get; set; }
        public string? Barcode { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Length { get; set; }
        public string? WeightUnit { get; set; }
        public string? HeightUnit { get; set; }
        public string? Status { get; set; }
        public Nullable<System.DateTime> DiscDate { get; set; }
        public Nullable<decimal> IncreaseSaleRateBy { get; set; }
        public bool KeepStock { get; set; }
        public bool AllowDecimal { get; set; }
        public Nullable<int> BestBefore { get; set; }
        public string? BestBeforeUnit { get; set; }
        public string? SKUDefinition { get; set; }
        public decimal TradeDisc { get; set; }
        public Nullable<int> MinDays { get; set; }
        public Nullable<int> MaxDays { get; set; }
        public Nullable<long> FKGenericID1 { get; set; }
        public Nullable<long> FKGenericID2 { get; set; }
        public Nullable<int> BoxSize { get; set; }
        public Nullable<long> FkUnitId { get; set; }
        public string? Genration { get; set; }
        public string? CodingScheme { get; set; }
        public Nullable<long> FkBrandId { get; set; }
        public string? Image { get; set; }
        public string? HSNCode { get; set; }
        public Nullable<long> FKTaxID { get; set; }
    
         
    }
}