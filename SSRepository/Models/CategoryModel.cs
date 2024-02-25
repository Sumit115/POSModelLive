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

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        //public string? CategoryOnlineName { get; set; }
        //public string? CategoryLogo { get; set; }
        public long? FkCategoryId { get; set; }
        //public int? Statu { get; set; }

        //[NotMapped]
        //public IFormFile ImgUrl { get; set; }
    }
}
