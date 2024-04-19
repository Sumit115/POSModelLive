using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblAccount_dtl", Schema = "dbo")]
    public partial class TblAccountDtl : TblBase, IEntity
    {
        [Key]
        public long PKAccountDtlId { get; set; }
        public long FkAccountId { get; set; }
        public long FKLocationID { get; set; }
        public decimal? OpBal { get; set; }
        public decimal? CurrBal { get; set; }
        public DateTime? CurrBalDate { get; set; }

    }
}
