using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{

    [Table("tblWallet_dtl", Schema = "dbo")]
    public partial class TblWalletdtl:TblBase
    {
        public long fkWalletId { get; set; }

        public string Particulars { get; set; } = null!;

        public string TranType { get; set; } = null!;

        public decimal Amount { get; set; }

        public decimal ClossingBalance { get; set; }
        public string? Remark { get; set; }
    }
}