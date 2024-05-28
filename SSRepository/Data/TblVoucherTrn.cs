using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblVoucher_trn", Schema = "dbo")]
    public partial class TblVoucherTrn : TblBase, IEntity
    {
        [Key] 
        public long PkVoucherId { get; set; }
        public long FKSeriesId { get; set; }
        public long EntryNo { get; set; }
        public DateTime EntryDate { get; set; }
        public TimeSpan EntryTime { get; set; }
        public string? VoucherNarration { get; set; }
        public bool? DraftMode { get; set; }
        public decimal? VoucherAmt { get; set; }
        public string? VoucherAmtType { get; set; }
        public int? PrintCount { get; set; }
        public string? RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public string? TrnStatus { get; set; }
 
        //public TblEmpUserMas FkcreatedBy { get; set; }
        //public TblSeriesMas Fkseries { get; set; }
        //public TblEmpUserMas Fkuser { get; set; }
        //public ICollection<TblBankReconcilition> TblBankReconcilition { get; set; }
        //public ICollection<TblBankTransaction> TblBankTransaction { get; set; }
    }
}
