using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class MasterLogDtlModel
    {
        public long PKMasterLogID { get; set; }
        public long? FKFormID { get; set; }
        public long FKID { get; set; }
        public long FKSeriseId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }
        public bool IsDelete { get; set; }
        public string? JsonDetail { get; set; }
        public string? Description { get; set; }
        public long? FKUserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifyDate { get; set; }

        public long? FKLastUserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastModifyDate { get; set; }
    }
    //public class MasterLogDtlModel
    //{
    //    public long PKMasterLogID { get; set; }
    //    public long? FKFormID { get; set; }
    //    public long FKID { get; set; }
    //    public long FKSeriseId { get; set; }

    //    //[DataType(DataType.Date)]
    //    //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    //public DateTime EntryDate { get; set; }
    //    public string? DATE_ENTRY { get; set; }
    //    public string? TIME_ENTRY { get; set; }

    //    public bool IsDelete { get; set; }
    //    public string? Status { get; set; }
    //    public string? JsonDetail { get; set; }
    //    public string? Description { get; set; }
    //    public long? FKUserId { get; set; }
    //    public string? UserName { get; set; }

    //    //[DataType(DataType.Date)]
    //    //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    //public DateTime? ModifyDate { get; set; }
    //    public string? DATE_MODIFIED { get; set; }
    //    public string? TIME_MODIFIED { get; set; }

    //    public long? FKLastUserId { get; set; }
    //    public string? LastUserName{ get; set; }

    //    //[DataType(DataType.Date)]
    //    //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    //public DateTime? LastModifyDate { get; set; }
    //    public string? DATE_LASTMODIFIED { get; set; }
    //    public string? TIME_LASTMODIFIED { get; set; }
    //    public string? WebUrl { get; set; } 
    //}
}
