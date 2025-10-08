using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Models
{
    public class CouponModel : BaseModel
    {
        public long PKID { get; set; }

        [Required]
        public long NoOfCoupon { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; } 
        public List<CouponCodeLnkModel>? CouponCodes { get; set; }
    }

  
}
