using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblAddressDtl
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public int Addressfor { get; set; }

    public int AddressforId { get; set; }

    public int? AddressType { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? PostOffice { get; set; }

    public string? Pin { get; set; }

    public string? Country { get; set; }

    public bool? Isprimary { get; set; }
}
