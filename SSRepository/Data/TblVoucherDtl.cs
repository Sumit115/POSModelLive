using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblVoucher_dtl", Schema = "dbo")]
    public partial class TblVoucherDtl : TblBase, IEntity
    {
         
        [Key]
        public long FkVoucherId { get; set; }
        public long FKSeriesId { get; set; }
        public long SrNo { get; set; }
        public long FkaccountId { get; set; }
        public long FKLocationID { get; set; }
        public decimal VoucherAmt { get; set; }
        public string? VoucherNarration { get; set; }
        public long? FktaxId { get; set; }
        public decimal? TaxableAmt { get; set; }
        public long? FkcustomerId { get; set; }
        public long? FkvendorId { get; set; }

        //public TblAccountMas Fkaccount { get; set; }
        //public TblCustomerMas Fkcustomer { get; set; }
        //public TblLocMas Fklocation { get; set; }
        //public TblTaxMas Fktax { get; set; }
        //public TblVendorMas Fkvendor { get; set; }
    }

}
