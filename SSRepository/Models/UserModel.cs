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
        }

        public string? CreateDate { get; set; }//DateCreated

        public string? ModifiDate { get; set; }


        public string? CreateBy { get; set; }
        public string? ModifiBy { get; set; }
        public long FKUserId { get; set; }
        public long FKCreatedByID { get; set; }
    }

     
    public class ddl
    {
        public bool Selected { get; set; }
        public string? Text { get; set; }
        public string? Value { get; set; }
    }
}
