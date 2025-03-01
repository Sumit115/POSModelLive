using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblUserLoc_Lnk", Schema = "dbo")]
    public partial class TblUserLocLnk : IEntity
    {
        public long FKUserID { get; set; }
        public long FKLocationID { get; set; }
        public virtual TblUserMas User { get; set; }
        public virtual TblLocationMas Location { get; set; }



    }
}
