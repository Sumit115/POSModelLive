using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientDistrictMa
    {
        public TblClientDistrictMa()
        {
            TblClientStationMas = new HashSet<TblClientStationMa>();
        }

        public long PkdistrictId { get; set; }
        public string District { get; set; } = null!;
        public long FkstateId { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual TblClientStateMa Fkstate { get; set; } = null!;
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
        public virtual ICollection<TblClientStationMa> TblClientStationMas { get; set; }
    }
}
