using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblError_Log", Schema = "dbo")]
    public partial class TblErrorLog : TblBase
    {
        [Key]
        public int PkId { get; set; }
        public long LogId { get; set; }
        public string? ErrorMessage { get; set; } 
    }
}
