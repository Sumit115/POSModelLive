using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblUnit_mas", Schema = "dbo")]
    public partial class TblUnitMas : TblBase,IEntity
    {
       [Key]
        public long PkUnitId { get; set; }
        public string? UnitName { get; set; }

    }
}
