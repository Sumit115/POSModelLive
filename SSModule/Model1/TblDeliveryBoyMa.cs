using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblDeliveryBoyMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? Marital { get; set; }

    public string? Gender { get; set; }

    public string? Dob { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Aadhar { get; set; }

    public string? Pan { get; set; }

    public string? Gstno { get; set; }

    public string? Passport { get; set; }

    public string? AadharCard { get; set; }

    public string? PanCard { get; set; }

    public string? Signature { get; set; }

    public string? AadharCardBack { get; set; }

    public int? IsAadharVerify { get; set; }

    public int? IsPanVerify { get; set; }

    public int? Status { get; set; }

    public int? FkBranchId { get; set; }

    public string? Address { get; set; }
}
