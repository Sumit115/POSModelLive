using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class UserModel : BaseModel
    {
        public long PkUserId { get; set; }
        public string UserId { get; set; } = null!;

        public string Pwd { get; set; } = null!;

        public long? FkRegId { get; set; }

        public int? Usertype { get; set; }

        [Required(ErrorMessage = "Branch Required")]
        public long? FkBranchId { get; set; }

        public long? FkRoleId { get; set; }

        public DateTime? Expiredt { get; set; }

        public DateTime? ExpirePwddt { get; set; }

        public long FkEmployeeId { get; set; }

        public int IsAdmin { get; set; }
        //public EmployeeModel? EmployeeVM { get; set; }
        //public BranchModel? BranchVM { get; set; }
        //public CompanyModel? CompanyVM { get; set; }
        public string? EmployeeName { get; set; }
        public string? BranchName { get; set; }
        public string? CompanyName { get; set; }

    }

    public class BaseModel
    {
        public BaseModel() {
            FKUserId = 1;
            src = 1;
            DATE_CREATED = DateTime.Now;
            DATE_MODIFIED = DateTime.Now;
        }
        //public long PkId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DATE_CREATED { get; set; }//DateCreated
        public string? DateCreated { get { return DATE_CREATED != null ? DATE_CREATED.Value.ToString("dd/MM/yyyy") : ""; } }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DATE_MODIFIED { get; set; }
        public string? DateModified { get { return DATE_MODIFIED != null ? DATE_MODIFIED.Value.ToString("dd/MM/yyyy") : ""; } }


        public string? UserName { get; set; }
        public long FKUserId { get; set; }
        public int src { get; set; }
    }

     
    public class ddl
    {
        public bool Selected { get; set; }
        public string? Text { get; set; }
        public string? Value { get; set; }
    }
}
