using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Data
{
    public class TblBase
    {
        //[Key]
        //public long PkId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        public long FKCreatedByID { get; set; }
        public long FKUserID { get; set; }
        public virtual TblUserMas FKUser { get; set; }


    }
    public class TblNon
    {
        

    }
}
