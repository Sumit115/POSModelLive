using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblBank_dtl", Schema = "dbo")]
    public partial class TblBankDtl : TblBase
    {
        public int fkformId { get; set; }
        public long fkId { get; set; }
        public string? Bank { get; set; }
        public string? Acno { get; set; }
        public string? Ifsc { get; set; }
        public string? Branch { get; set; }
        public string? HolderName { get; set; }
        public bool? Isprimary { get; set; }
    }
}
