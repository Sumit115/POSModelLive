using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class SignInModel
    {
        public string UserID { get; set; } = null!;
         public string Password { get; set; } = null!;
    }

    public class SignInValidate
    {
        public long ClientUserId { get; set; }

        public long ClientRegId { get; set; }

        public long UserId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string ConnectionString { get; set; } = null!;

        public string ErrMsg { get; set; } = null!;


    }
}
