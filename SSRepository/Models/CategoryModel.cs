using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class CategoryModel : BaseModel
    {
        public long PkCategoryId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)] 
        public string Category { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Section Group")]  
        public long FkCategoryGroupId { get; set; }
        public string? GroupName { get; set; }

        public List<CategorySizeLnkModel>? CategorySize_lst { get; set; }

    }

}
