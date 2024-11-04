using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientStateMa
    {
        public TblClientStateMa()
        {
            TblClientDistrictMas = new HashSet<TblClientDistrictMa>();
        }

        public long PkstateId { get; set; }
        public string State { get; set; } = null!;
        public string Capital { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Code { get; set; } = null!;
        public long FkcountryId { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual TblClientCountryMa Fkcountry { get; set; } = null!;
        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
        public virtual ICollection<TblClientDistrictMa> TblClientDistrictMas { get; set; }
    }
}
