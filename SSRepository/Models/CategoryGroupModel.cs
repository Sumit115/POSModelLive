using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class CategoryGroupModel
    {
        public long PkCategoryGroupId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)]
        public string CategoryGroupName { get; set; }
        public long? FkCategoryGroupId { get; set; } 
        public string? PCategoryGroupName { get; set; }

    }
}
