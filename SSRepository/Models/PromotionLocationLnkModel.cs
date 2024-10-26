using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public partial class PromotionLocationLnkModel : BaseModel
    {
        public long PkId { get; set; }
        public long FkPromotionId { get; set; }
        public long FkLocationId { get; set; } 
        public string? PromotionName { get; set; }
        public string? LocationName { get; set; }

        public int Mode { get; set; }

    }
}
