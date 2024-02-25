using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblProdLot_dtl", Schema = "dbo")]
    public partial class TblProdLotDtl : TblBase, IEntity
    {
       
        [Key]
        public long PkLotId { get; set; }

        public long FKProdID { get; set; }
        public string? LotAlias { get; set; }//=''

        //  public long MasterLotID { get; set; }
        public long? Barcode { get; set; }//16 digit uniq no.
        public string? Batch { get; set; }//cntrltype= L
        public string? Color { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpiryDate { get; set; }//=''
        public decimal? ProdConv1 { get; set; }//=0
        public decimal MRP { get; set; }
        public decimal? LtExtra { get; set; }//=0
        public bool AddLT { get; set; }//=false
        public decimal? SaleRate { get; set; }
        public decimal? PurchaseRate { get; set; }//=Rate
        public long? FkmfgGroupId { get; set; }//=0
                                               // public decimal? CostRate { get; set; }
        public decimal? TradeRate { get; set; }
        public decimal? DistributionRate { get; set; }
        //  public decimal? SuggestedRate { get; set; }
        public string? PurchaseRateUnit { get; set; }//=''
        public string? MRPSaleRateUnit { get; set; }//=''
                                                   // public DateTime? StockDate { get; set; }
                                                   //  public decimal? ExciseRate { get; set; }
        public long InTrnId { get; set; }//=PurchaseId
        public long InTrnFKSeriesID { get; set; }//=PurchaseSeriesId
        public long InTrnsno { get; set; }//=PurchaseSeriesId
        //public long? FksaleTaxId { get; set; }
        //public long? FkpurchaseTaxId { get; set; }
        public string? Remarks { get; set; }
        //public decimal? SaleExciseRate { get; set; }
        //public decimal? PurchaseExciseRate { get; set; }
        // public string LotScheme { get; set; }


    }
}
