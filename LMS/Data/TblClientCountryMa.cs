using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientCountryMa
    {
        public TblClientCountryMa()
        {
            TblClientStateMas = new HashSet<TblClientStateMa>();
        }

        public long PkcountryId { get; set; }
        public string Country { get; set; } = null!;
        public string? Capital { get; set; }
        public long FkuserId { get; set; }
        public DateTime DateModified { get; set; }
        public long FkcreatedById { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual TblClientUserMa FkcreatedBy { get; set; } = null!;
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
        public virtual ICollection<TblClientStateMa> TblClientStateMas { get; set; }
    }
}
