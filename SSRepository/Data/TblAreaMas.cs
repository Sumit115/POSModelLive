using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblArea_mas", Schema = "dbo")]
    public partial class TblAreaMas : TblBase, IEntity
    {
        [Key]
        public long PkAreaId { get; set; }
         
        public string AreaName { get; set; }
        public long FkRegionId { get; set; }
        public string? Description { get; set; }

    }
}
