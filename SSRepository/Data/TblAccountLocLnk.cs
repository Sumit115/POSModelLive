using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblAccountLoc_lnk", Schema = "dbo")]
    public partial class TblAccountLocLnk : TblBase, IEntity
    {
        [Key]
        public long PKAccountLocLnkId { get; set; }
        public long FkAccountId { get; set; }
        public long FKLocationID { get; set; } 
        public virtual TblLocationMas FKLocation { get; set; }
    }
}
