using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblCustomer_mas", Schema = "dbo")]
    public partial class TblCustomerMas: TblBasePersion, IEntity
    {
        [Key]
        public long PkCustomerId { get; set; }

    }
}