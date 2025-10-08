using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class CouponCodeLnkModel : BaseModel
    {
        public long PkId { get; set; }

        [Required]
        public long FkCouponId { get; set; }

        [Required]
        [StringLength(12)]
        public string CouponCode { get; set; }

        public long? FkId { get; set; }
        public long? FkSeriesId { get; set; }

        public int Mode { get; set; } // Optional: for UI operations

        public string? CouponName { get; set; } // Optional: can map master SKUDefinition
    }
}
