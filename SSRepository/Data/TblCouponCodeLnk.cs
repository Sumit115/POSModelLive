using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblCouponCode_lnk", Schema = "dbo")]
    public partial class TblCouponCodeLnk : TblBase, IEntity
    {
        [Key]
        public long PkId { get; set; }

        [Required]
        public long FkCouponId { get; set; }

        [Required]
        [StringLength(12)]
        public string CouponCode { get; set; }

        public long? FkId { get; set; }

        public long? FkSeriesId { get; set; }

       
        // Navigation property
        [ForeignKey("FkCouponId")]
        public virtual TblCouponMas FKCoupon { get; set; }
    }
}
