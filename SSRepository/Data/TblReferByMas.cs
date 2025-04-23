using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblReferBy_mas", Schema = "dbo")]
    public partial class TblReferByMas : TblBasePersion,IEntity
    {
        [Key]
        public long PkReferById { get; set; }

        public string Name { get; set; }
        public string? Post { get; set; }
        public decimal? Salary { get; set; }
    }
}
