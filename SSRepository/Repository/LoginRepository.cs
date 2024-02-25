using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Data;
using System.Reflection;

namespace SSRepository.Repository
{
    public class LoginRepository: ILoginRepository
    {
        protected readonly AppDbContext __dbContext;
        public LoginRepository(AppDbContext dbContext)
        {
            __dbContext = dbContext;
        }

        public UserModel? Login(string UserId, string Pwd)
        { 
            return __dbContext.TblUserMas.Where(x => x.UserId == UserId && x.Pwd == Pwd)
                .Select(x => new UserModel {  
                    PkUserId = x.PkUserId,
                    UserId = x.UserId,
                    FkRegId = x.FkRegId,
                    //CompanyName = x.CompanyName,
                    Usertype = x.Usertype,
                    FkBranchId = x.FkBranchId,
                    //BranchName = x.BranchName,
                    FkRoleId = x.FkRoleId,
                    Expiredt = x.Expiredt,
                    ExpirePwddt = x.ExpirePwddt,
                    IsAdmin = x.IsAdmin,
                    FkEmployeeId = x.FkEmployeeId,
                    //EmployeeName = x.EmployeeName,

                }).FirstOrDefault();
        }
        public void Logout() { 
        
        }


    }
}

