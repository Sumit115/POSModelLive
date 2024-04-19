using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblZone_mas", Schema = "dbo")]
    public partial class TblZoneMas : TblBase, IEntity
    {
        [Key]
        public long PkZoneId { get; set; }

         public string? ZoneName { get; set; }
        public string? Description { get; set; }
    }
}
