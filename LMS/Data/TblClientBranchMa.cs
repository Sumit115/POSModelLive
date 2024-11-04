using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientBranchMa
    {
        public TblClientBranchMa()
        {
            TblClientBranchAppLnks = new HashSet<TblClientBranchAppLnk>();
            TblClientLocMas = new HashSet<TblClientLocMa>();
            TblClientUserMas = new HashSet<TblClientUserMa>();
        }

        public long PkbranchId { get; set; }
        public long FkclientRegId { get; set; }
        public string BranchType { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public string? Alias { get; set; }
        public int No { get; set; }
        public string Password { get; set; } = null!;
        public string? Address { get; set; }
        public string? Station { get; set; }
        public string? Locality { get; set; }
        public string? Pincode { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public long? FknonSaleableId { get; set; }
        public long? FkholdLocationId { get; set; }
        public string? Vendor { get; set; }
        public string? Customer { get; set; }
        public string? Dbname { get; set; }
        public string? Swilalias { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Contact { get; set; }
        public string? AccountNo { get; set; }
        public string? Bank { get; set; }
        public string? Ifsc { get; set; }
        public string? Pan { get; set; }
        public string? ServerName { get; set; }
        public string? DataBaseName { get; set; }
        public string? DbuserName { get; set; }
        public string? Dbpassword { get; set; }
        public bool EnableParty { get; set; }
        public int? WebSocketPort { get; set; }
        public string? Gstno { get; set; }
        public DateTime? Gstdate { get; set; }
        public DateTime? Gstvdate { get; set; }
        public bool Composition { get; set; }
        public DateTime? CompIdate { get; set; }
        public DateTime? CompVdate { get; set; }
        public string? MailId { get; set; }
        public string? MailPwd { get; set; }
        public string? MailServer { get; set; }
        public string? Upi { get; set; }
        public string? MsgApi { get; set; }
        public string? WhatsAppApi { get; set; }
        public string? MailSign { get; set; }
        public int? NoOfUser { get; set; }
        public bool MastersFromHo { get; set; }
        public bool MasterData { get; set; }
        public bool Consolidation { get; set; }
        public long? BranchId { get; set; }
        public string? ImageUrl { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }
        public string? UdyamRegNo { get; set; }
        public DateTime? UdyamRegDate { get; set; }
        public DateTime? UdyamIncDate { get; set; }
        public string? UdyamType { get; set; }
        public string? UdyamCatg { get; set; }

        public virtual TblClientRegMa FkclientReg { get; set; } = null!;
        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual TblClientLocMa? FkholdLocation { get; set; }
        public virtual TblClientLocMa? FknonSaleable { get; set; }
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
        public virtual ICollection<TblClientBranchAppLnk> TblClientBranchAppLnks { get; set; }
        public virtual ICollection<TblClientLocMa> TblClientLocMas { get; set; }
        public virtual ICollection<TblClientUserMa> TblClientUserMas { get; set; }
    }
}
