using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class VoucherDetails : BaseModel
    {
        public long FkVoucherId { get; set; }
        public long FKSeriesId { get; set; }
        public long SrNo { get; set; }
        public long FkAccountId { get; set; }
        public long FKLocationID { get; set; }
        public decimal VoucherAmt { get; set; }
        public string? VoucherNarration { get; set; }
        public long? FkTaxId { get; set; }
        public decimal? TaxableAmt { get; set; }
        public long? FkCustomerId { get; set; }
        public long? FkVendorId { get; set; }
        public string? ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        //Extra
        public int ModeForm { get; set; }
        public decimal CreditAmt { get; set; }
        public decimal DebitAmt { get; set; }
        public decimal CurrentBalance { get; set; }
        public string? Location { get; set; }
        public string? AccountGroupName { get; set; }
        public string? AccountName_Text { get; set; }

        public decimal Balance { get; set; }
        public string AccMode { get; set; }


    }
}
