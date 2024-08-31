using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblRecipe_mas", Schema = "dbo")]
    public partial class TblRecipeMas : TblBase,IEntity
    {
       [Key]
        public long PkRecipeId { get; set; }
        public string Name { get; set; } 

    }
}
