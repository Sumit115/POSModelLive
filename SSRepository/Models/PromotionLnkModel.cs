using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public partial class PromotionLnkModel : BaseModel
    {
        public long PkId { get; set; }
        public long FkPromotionId { get; set; }
        public long FkLinkId { get; set; }

        public string? PromotionName { get; set; }
        public string? CategoryName { get; set; }

        public int Mode { get; set; }

    }
}
