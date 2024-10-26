using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblPromotion_lnk", Schema = "dbo")]
    public partial class TblPromotionLnk : TblBase, IEntity
    {
        [Key]
        public long PkId { get; set; }
        public long FkPromotionId { get; set; }
        public long FkLinkId { get; set; }
    }
}
