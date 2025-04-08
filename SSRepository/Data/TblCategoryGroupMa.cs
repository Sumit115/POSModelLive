using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblCategoryGroup_mas", Schema = "dbo")]
    public partial class TblCategoryGroupMas : TblBase,IEntity
    {
        [Key]
        public long PkCategoryGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryGroupName { get; set; } 
        public long? FkCategoryGroupId { get; set; } 
        public virtual TblCategoryGroupMas? FKCategoryGroupMas { get; set; }


    }
}
