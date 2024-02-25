using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblEmployee_mas", Schema = "dbo")]
    public partial class TblEmployeeMas : TblBasePersion,IEntity
    {
        [Key]
        public long PkEmployeeId { get; set; }
    }
}
