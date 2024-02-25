using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblCategory_mas", Schema = "dbo")]
    public partial class TblCategoryMas : TblBase,IEntity
    {
        [Key]
        public long  PkCategoryId { get; set; }
      
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
