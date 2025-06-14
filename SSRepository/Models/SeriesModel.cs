using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class SeriesModel : BaseModel
    {
        public long PKID { get; set; }
        public string Series { get; set; } //=A
        public long SeriesNo { get; set; }// =0  autocalculation
                                          // public long FkBranchId { get; set; }//=ddl
        public string BillingRate { get; set; }//=MRP/SaleRate/TradeRate/DistributionRate/PurchaseRate
        public string TranAlias { get; set; }//=SORD  ddl
        public string? FormatName { get; set; }//=''
        public string? ResetNoFor { get; set; }//=''
        public bool AllowWalkIn { get; set; }//=true
        public bool AutoApplyPromo { get; set; }//=true
        public bool RoundOff { get; set; }//=true
        public bool DefaultQty { get; set; }//=true
        public bool AllowZeroRate { get; set; }//=true
        public bool AllowFreeQty { get; set; }//=true
        public char TaxType { get; set; } = 'I';
        public string? BranchName { get; set; }//=true
        public string? BranchStateName { get; set; }//=true
        public string DocumentType { get; set; }
        public string? TranAliasName { get; set; }//=SORD  ddl
        public long? FKLocationID { get; set; }
        public string? Location { get; set; }
        public string? PaymentMode { get; set; } =  "Cash";
    }
}
