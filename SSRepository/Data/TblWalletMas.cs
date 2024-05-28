using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblWallet_mas", Schema = "dbo")]
    public partial class TblWalletMas: TblBase
    {
        [Key]
        public long PkId { get; set; }
        public long FkAccountId { get; set; }

        public decimal Cr { get; set; }

        public decimal Dr { get; set; }

        public decimal BalAmount { get; set; }

    }
}
 