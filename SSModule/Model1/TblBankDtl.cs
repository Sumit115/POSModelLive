using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblBankDtl
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public int Bankfor { get; set; }

    public int BankforId { get; set; }

    public string? Bank { get; set; }

    public string? Acno { get; set; }

    public string? Ifsc { get; set; }

    public string? Branch { get; set; }

    public string? HolderName { get; set; }

    public bool? Isprimary { get; set; }
}
