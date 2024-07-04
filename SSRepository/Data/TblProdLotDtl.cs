using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblProdLot_dtl", Schema = "dbo")]
    public partial class TblProdLotDtl :  IEntity
    {

        [Key]
        public long PkLotId { get; set; }
        public long FKProductId { get; set; }
        public string? LotAlias { get; set; }
        public string? Barcode { get; set; }
        public string? Batch { get; set; }
        public string? Color { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<decimal> ProdConv1 { get; set; }
        public decimal MRP { get; set; }
        public Nullable<decimal> LtExtra { get; set; }
        public bool AddLT { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public Nullable<decimal> PurchaseRate { get; set; }
        public Nullable<long> FkmfgGroupId { get; set; }
        public Nullable<decimal> TradeRate { get; set; }
        public Nullable<decimal> DistributionRate { get; set; }
        public string? PurchaseRateUnit { get; set; }
        public string? MRPSaleRateUnit { get; set; }
        public long InTrnId { get; set; }
        public long InTrnFKSeriesID { get; set; }
        public long InTrnsno { get; set; }
        public string? Remarks { get; set; }
        public string? LotName { get; set; }
        public string? LotNo { get; set; }
        public string? Style { get; set; }
        public Nullable<System.DateTime> StockDate { get; set; }
        public Nullable<decimal> CostRate { get; set; }
        public Nullable<long> FKChallanID { get; set; }
        public Nullable<decimal> LT_Extra { get; set; }
        public Nullable<decimal> SuggestedRate { get; set; }
        public Nullable<decimal> ExciseRate { get; set; }
        public Nullable<long> FKSaleTaxID { get; set; }
        public Nullable<long> FKPurchaseTaxID { get; set; }
        public Nullable<long> MasterLotID { get; set; }
        public Nullable<long> PkLotIdtest { get; set; }
    }
}
