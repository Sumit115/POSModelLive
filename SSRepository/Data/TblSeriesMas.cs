using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblSeries_mas", Schema = "dbo")]
    public partial class TblSeriesMas : TblBase,IEntity
    {
        [Key]
        public long PkSeriesId { get; set; }
        public string? Series { get; set; }
        public long SeriesNo { get; set; }
        public long FkBranchId { get; set; }
        public string? BillingRate { get; set; }
        public string? TranAlias { get; set; }
        public string? FormatName { get; set; }
        public string? ResetNoFor { get; set; }
        public bool AllowWalkIn { get; set; }
        public bool AutoApplyPromo { get; set; }
        public bool RoundOff { get; set; }
        public bool DefaultQty { get; set; }
        public bool AllowZeroRate { get; set; }
        public bool AllowFreeQty { get; set; }
        public Nullable<long> FKLocationID { get; set; }
 
        public string? DocumentType { get; set; }

    }
}
