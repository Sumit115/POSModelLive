using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblBrand_mas", Schema = "dbo")]
    public partial class TblBrandMas : TblBase,IEntity
    {
       [Key]
        public long PkBrandId { get; set; }
        public string? BrandName { get; set; }

    }
}
