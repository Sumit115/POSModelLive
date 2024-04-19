using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblRegion_mas", Schema = "dbo")]
    public partial class TblRegionMas : TblBase, IEntity
    {
        [Key]
        public long PkRegionId { get; set; }
         
        public string RegionName { get; set; }
        public long FkZoneId { get; set; }
        public string? Description { get; set; }

    }
}
