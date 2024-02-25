
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblLog_dtl", Schema = "dbo")]
    public partial class TblLogDtl : TblBase
    {
        public int? FkId { get; set; }
        public int? FkFormId { get; set; }

        public DateTime? EntryDate { get; set; }

        public string? JsonDetail { get; set; }


        public string? Logtype { get; set; }

        public int PsrcId { get; set; }

        public string? Status { get; set; }

    }
}
