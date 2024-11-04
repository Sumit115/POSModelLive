using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientRegMa
    {
        public TblClientRegMa()
        {
            TblClientBranchMas = new HashSet<TblClientBranchMa>();
            TblClientLocMas = new HashSet<TblClientLocMa>();
            TblClientRegAppLnks = new HashSet<TblClientRegAppLnk>();
            TblClientUserMas = new HashSet<TblClientUserMa>();
        }

        public long PkclientRegId { get; set; }
        public string RegNo { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string? Gstno { get; set; }
        public string? Address { get; set; }
        public string? Station { get; set; }
        public string? Locality { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Pincode { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? BusinessDetail { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? ValidTill { get; set; }
        public string? Version { get; set; }
        public string? ConnectionString { get; set; }
        public string? DomainName { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public string? Sqldbsize { get; set; }
        public string? PanNo { get; set; }
        public string? Turnover { get; set; }

        public virtual ICollection<TblClientBranchMa> TblClientBranchMas { get; set; }
        public virtual ICollection<TblClientLocMa> TblClientLocMas { get; set; }
        public virtual ICollection<TblClientRegAppLnk> TblClientRegAppLnks { get; set; }
        public virtual ICollection<TblClientUserMa> TblClientUserMas { get; set; }
    }
}
