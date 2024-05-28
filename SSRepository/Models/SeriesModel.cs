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
        public long PkSeriesId { get; set; }
        public string Series { get; set; } //=A
        public long SeriesNo { get; set; }// =0  autocalculation
        public long FkBranchId { get; set; }//=ddl
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

        public string BranchName { get; set; }//=true

    }
}
