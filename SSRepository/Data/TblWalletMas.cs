using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblWallet_mas", Schema = "dbo")]
    public partial class TblWalletMas: TblBase
    {
        public int fkformId { get; set; }
        public long fkId { get; set; }

        public decimal Cr { get; set; }

        public decimal Dr { get; set; }

        public decimal BalAmount { get; set; }

    }
}