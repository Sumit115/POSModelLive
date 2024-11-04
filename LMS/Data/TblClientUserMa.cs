using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientUserMa
    {
        public TblClientUserMa()
        {
            InverseFkcreatedBy = new HashSet<TblClientUserMa>();
            TblClientBranchAppUserLnks = new HashSet<TblClientBranchAppUserLnk>();
            TblClientBranchMaFkcreatedBies = new HashSet<TblClientBranchMa>();
            TblClientBranchMaFkusers = new HashSet<TblClientBranchMa>();
            TblClientCountryMaFkcreatedBies = new HashSet<TblClientCountryMa>();
            TblClientCountryMaFkusers = new HashSet<TblClientCountryMa>();
            TblClientDistrictMaFkcreatedBies = new HashSet<TblClientDistrictMa>();
            TblClientDistrictMaFkusers = new HashSet<TblClientDistrictMa>();
            TblClientLocMaFkcreatedBies = new HashSet<TblClientLocMa>();
            TblClientLocMaFkusers = new HashSet<TblClientLocMa>();
            TblClientStateMaFkcreatedBies = new HashSet<TblClientStateMa>();
            TblClientStateMaFkusers = new HashSet<TblClientStateMa>();
            TblClientStationMaFkcreatedBies = new HashSet<TblClientStationMa>();
            TblClientStationMaFkusers = new HashSet<TblClientStationMa>();
            Fklocations = new HashSet<TblClientLocMa>();
        }

        public long PkuserId { get; set; }
        public long FkclientRegId { get; set; }
        public string UserName { get; set; } = null!;
        public string Pwd { get; set; } = null!;
        public string? Type { get; set; }
        public string ErpuserId { get; set; } = null!;
        public string? MobileNo { get; set; }
        public string Status { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public DateTime? PwdExpDate { get; set; }
        public long? FkbranchId { get; set; }
        public string? MailId { get; set; }
        public string? MailPwd { get; set; }
        public string? MailServer { get; set; }
        public string? MsgApi { get; set; }
        public string? WhatsAppApi { get; set; }
        public string? MailSign { get; set; }
        public bool MailVerified { get; set; }
        public bool MobileVerified { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public long? UserId { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual TblClientBranchMa? Fkbranch { get; set; }
        public virtual TblClientRegMa FkclientReg { get; set; }
        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual ICollection<TblClientUserMa> InverseFkcreatedBy { get; set; }
        public virtual ICollection<TblClientBranchAppUserLnk> TblClientBranchAppUserLnks { get; set; }
        public virtual ICollection<TblClientBranchMa> TblClientBranchMaFkcreatedBies { get; set; }
        public virtual ICollection<TblClientBranchMa> TblClientBranchMaFkusers { get; set; }
        public virtual ICollection<TblClientCountryMa> TblClientCountryMaFkcreatedBies { get; set; }
        public virtual ICollection<TblClientCountryMa> TblClientCountryMaFkusers { get; set; }
        public virtual ICollection<TblClientDistrictMa> TblClientDistrictMaFkcreatedBies { get; set; }
        public virtual ICollection<TblClientDistrictMa> TblClientDistrictMaFkusers { get; set; }
        public virtual ICollection<TblClientLocMa> TblClientLocMaFkcreatedBies { get; set; }
        public virtual ICollection<TblClientLocMa> TblClientLocMaFkusers { get; set; }
        public virtual ICollection<TblClientStateMa> TblClientStateMaFkcreatedBies { get; set; }
        public virtual ICollection<TblClientStateMa> TblClientStateMaFkusers { get; set; }
        public virtual ICollection<TblClientStationMa> TblClientStationMaFkcreatedBies { get; set; }
        public virtual ICollection<TblClientStationMa> TblClientStationMaFkusers { get; set; }

        public virtual ICollection<TblClientLocMa> Fklocations { get; set; }
    }
}
