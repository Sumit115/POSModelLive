using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblVendor_mas", Schema = "dbo")]
    public partial class TblVendorMas : TblBasePersion, IEntity
    {
        [Key]
        public long PkVendorId { get; set; }
    }
}
