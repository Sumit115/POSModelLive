using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientStationMa
    {
        public long PkstationId { get; set; }
        public string Station { get; set; } = null!;
        public long FkdistrictId { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual TblClientDistrictMa Fkdistrict { get; set; } = null!;
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
    }
}
