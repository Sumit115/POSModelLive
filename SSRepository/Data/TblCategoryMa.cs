using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblCategory_mas", Schema = "dbo")]
    public partial class TblCategoryMas : TblBase, IEntity
    {
        [Key]
        public long PkCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        public long FkCategoryGroupId { get; set; }
    }
}
