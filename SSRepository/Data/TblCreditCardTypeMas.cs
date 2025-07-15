using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblCreditCardType_mas", Schema = "dbo")]
    public partial class TblCreditCardTypeMas : TblBase, IEntity
    {
        [Key]
        public long PkCreditCardTypeId { get; set; }
        public string? CreditCardType { get; set; }
        public long? FkAccountID { get; set; }
        public string? Assembly { get; set; }
        public string? Class { get; set; } 
        public string? Method { get; set; }
        public string? Parameter { get; set; }

        public virtual TblUserMas FKUser { get; set; }
        public virtual TblAccountMas? FKAccount { get; set; }
    }
}