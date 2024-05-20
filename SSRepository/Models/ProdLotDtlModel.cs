using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class ProdLotDtlModel : BaseModel
    {
        public long PkLotId { get; set; }
        [Required(ErrorMessage = "Select SubSection Required")]
        public long FKProductId { get; set; }
        [Required(ErrorMessage = "Alias Required")]
        public string? LotAlias { get; set; }//=''

        [Required(ErrorMessage = "Article Name Required")]
        public string? LotName { get; set; }//=''

        [Required(ErrorMessage = "Article No Required")]
        public string? LotNo { get; set; }//=''
        //  public long MasterLotID { get; set; }
        public string? Barcode { get; set; }//16 digit uniq no.

        [Required(ErrorMessage = "Size Required")]
        public string Batch { get; set; }//cntrltype= L
        [Required(ErrorMessage = "Color Required")]
        public string Color { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MfgDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }//=''
        public decimal? ProdConv1 { get; set; }//=0

        [Required(ErrorMessage = "MRP Required")]
        public decimal MRP { get; set; }
        public decimal? LtExtra { get; set; }//=0
        public bool AddLT { get; set; }//=false
        [Required(ErrorMessage = "Sale Rate Required")]
        public decimal? SaleRate { get; set; }
        [Required(ErrorMessage = "Purchase Rate Required")]
        public decimal? PurchaseRate { get; set; }//=Rate
        public long? FkmfgGroupId { get; set; }//=0
                                               // public decimal? CostRate { get; set; }
        [Required(ErrorMessage = "Trade Rate Required")]
        public decimal? TradeRate { get; set; }
        [Required(ErrorMessage = "Distribution Rate Required")]
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


        //Other
        public string? ProductName { get; set; }

    }
}
