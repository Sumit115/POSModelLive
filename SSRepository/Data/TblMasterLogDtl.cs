using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblMasterLog_Dtl", Schema = "dbo")]
    public partial class TblMasterLogDtl : IEntity
    {
        [Key]
        public long PKMasterLogID { get; set; }
        public long? FKFormID { get; set; }
        public long FKID { get; set; }
        public long FKSeriseId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }
        public bool IsDelete { get; set; }
        public string? JsonDetail { get; set; }
        public string? Description { get; set; }
        public long? FKUserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifyDate { get; set; }

        public long? FKLastUserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastModifyDate { get; set; }

    }
}
