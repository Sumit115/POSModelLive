using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblPromotionLocation_lnk", Schema = "dbo")]
    public partial class TblPromotionLocationLnk : TblBase, IEntity
    {
        [Key]
        public long PkId { get; set; }
        public long FkPromotionId { get; set; }
        public long FKLocationId { get; set; }
        public virtual TblLocationMas FKLocation { get; set; }
    }
}
