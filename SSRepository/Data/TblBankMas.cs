using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblBank_mas", Schema = "dbo")]
    public partial class TblBankMas : TblBase,IEntity
    {
       [Key]
        public long PkBankId { get; set; }
        public string BankName { get; set; }
        public string? IFSCCode { get; set; }  
    }
}
