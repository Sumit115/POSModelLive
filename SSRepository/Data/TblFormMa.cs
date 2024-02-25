using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblForm_mas", Schema = "dbo")]
    public partial class TblFormMas  
    {
        [Key]
        public long PkFormId { get; set; }

        public string Caption { get; set; }

        public string Url { get; set; }

        public int FkMasterFormId { get; set; } 
    }
}