using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientLocMa
    {
        public TblClientLocMa()
        {
            TblClientBranchMaFkholdLocations = new HashSet<TblClientBranchMa>();
            TblClientBranchMaFknonSaleables = new HashSet<TblClientBranchMa>();
            Fkusers = new HashSet<TblClientUserMa>();
        }

        public long PklocationId { get; set; }
        public long FkclientRegId { get; set; }
        public string StockLocation { get; set; } = null!;
        public string? Alias { get; set; }
        public bool IsBillingLocation { get; set; }
        public bool IsAllProduct { get; set; }
        public bool IsAllCustomer { get; set; }
        public bool IsAllVendor { get; set; }
        public bool IsAllAccount { get; set; }
        public bool IsAllCostCenter { get; set; }
        public bool IsDifferentTax { get; set; }
        public string? Address { get; set; }
        public string? Station { get; set; }
        public string? Locality { get; set; }
        public string? Pincode { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? PostingAc { get; set; }
        public long FkbranchId { get; set; }
        public long? LocationId { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }
        public long? TblClientRegMasPkclientRegId { get; set; }

        public virtual TblClientBranchMa Fkbranch { get; set; } = null!;
        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
        public virtual TblClientRegMa? TblClientRegMasPkclientReg { get; set; }
        public virtual ICollection<TblClientBranchMa> TblClientBranchMaFkholdLocations { get; set; }
        public virtual ICollection<TblClientBranchMa> TblClientBranchMaFknonSaleables { get; set; }

        public virtual ICollection<TblClientUserMa> Fkusers { get; set; }
    }
}
