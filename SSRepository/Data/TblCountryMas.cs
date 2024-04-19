using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblCountry_mas", Schema = "dbo")]
    public partial class TblCountryMas : TblBase, IEntity
    {
        [Key]
        public long PkCountryId { get; set; }

         public string? CountryName { get; set; }
        public string? CapitalName { get; set; }
    }
}
