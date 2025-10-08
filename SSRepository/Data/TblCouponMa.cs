using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SSRepository.Data
{
    [Table("tblCoupon_mas", Schema = "dbo")]
    public partial class TblCouponMas : TblBase, IEntity
    {
        [Key]
        public long PkCouponId { get; set; }

        [Required]
        public long NoOfCoupon { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

         

        // Navigation property
        public virtual ICollection<TblCouponCodeLnk> CouponCodes { get; set; }
    }

    
}
