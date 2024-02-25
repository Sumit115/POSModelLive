
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblAddress_dtl", Schema = "dbo")]
    public partial class TblAddressDtl: TblBase
    {
        public int fkformId { get; set; }
        public long fkId { get; set; }
        public int? AddressType { get; set; }
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PostOffice { get; set; }
        public string? Pin { get; set; }
        public string? Country { get; set; }
        public bool? Isprimary { get; set; }
    }
}
