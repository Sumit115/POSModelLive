using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SSRepository.Models
{
    public class UserModel : BaseModel
    {
        public UserModel()
        {
            MenuList = new List<MenuModel>(); 
        }
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
        public List<MenuModel> MenuList { get; set; }


    }

    public class BaseModel
    {
        public BaseModel() {
        }

        public long FKUserID { get; set; }
        public string? UserName { get; set; }

        public string? DATE_MODIFIED { get; set; }
    }

     
    public class ddl
    {
        public bool Selected { get; set; }
        public string? Text { get; set; }
        public string? Value { get; set; }
        public string? Value2 { get; set; }
    }
    public class MenuModel
    {
        public long PKFormID { get; set; }
        public Nullable<long> FKMasterFormID { get; set; }
        public int SeqNo { get; set; }
        public string? FormName { get; set; }
        public string? ShortName { get; set; }
        public string? ShortCut { get; set; }
        public string? ToolTip { get; set; }
        public string? Image { get; set; }
        public string? FormType { get; set; }
        public string? WebURL { get; set; }
        public bool IsActive { get; set; }
        public bool IsAccess { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreate { get; set; }
        public bool IsPrint { get; set; }
        public bool IsBrowse { get; set; }
        public List<MenuModel>? SubMenu { get; set; }
    }
}
