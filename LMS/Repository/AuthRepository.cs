using LMS.Data;
using LMS.IRepository;
using LMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LMS.Repository
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(ssodbContext dbContext) : base(dbContext)
        {

        }

        public SignInValidate ValidateUser(SignInModel model)
        {
            SignInValidate entity =new SignInValidate();
            var data = __dbContext.TblClientUserMas.Where(x => x.ErpuserId == model.UserID && x.Pwd == AesOperation.EncryptString(model.Password)).ToList();
            if (data.Any())
            {
                entity = (from cou in __dbContext.TblClientUserMas
                         where cou.PkuserId == data[0].PkuserId
                         select (new SignInValidate
                         {
                             ClientUserId = cou.PkuserId,
                             ClientRegId = cou.FkclientRegId,
                             UserId = cou.UserId??0,
                             UserName = cou.UserName,
                             CompanyName = cou.FkclientReg.ClientName?? "",
                             ConnectionString = cou.FkclientReg.ConnectionString ?? ""

                         })).FirstOrDefault();
            }
            else
            {
                entity.ErrMsg = "Invalid UserId Or Password";
            }
            return entity?? new SignInValidate();
        }

        public void Logout()
        {

        }


    }
}

