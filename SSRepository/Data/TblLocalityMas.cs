using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblLocality_mas", Schema = "dbo")]
    public partial class TblLocalityMas : TblBase, IEntity
    {
        [Key]
        public long PkLocalityId { get; set; }
         
        public string LocalityName { get; set; }
        public long FkAreaId { get; set; }
        public string? Description { get; set; }
        public TblAreaMas FKArea { get; set; }

    }
}
