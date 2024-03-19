using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblGridStructer", Schema = "dbo")]
    public partial class TblGridStructer : TblNon, IEntity
    {
        [Key]
        public long PkGridId { get; set; }

        public long FkUserId { get; set; }

        public long FkFormId { get; set; }

        [MaxLength(20)]
        public string GridName { get; set; }
        public string JsonData { get; set; }

    }
}