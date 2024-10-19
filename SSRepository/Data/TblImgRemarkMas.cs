using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblImgRemark_mas", Schema = "dbo")]
    public partial class TblImgRemarkMas : TblBase, IEntity
    {
        [Key]
        public long FKID { get; set; }
        public long FKSeriesId { get; set; }
        public long FkFormId { get; set; }
        public string? Image { get; set; }
        public string? Remark { get; set; }

    }
}
