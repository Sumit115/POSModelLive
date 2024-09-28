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
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        //protected readonly AppDbContext __dbContext;
        //public LoginRepository(AppDbContext dbContext)
        //{
        //    __dbContext = dbContext;
        //}
        public LoginRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public UserModel? Login(string UserId, string Pwd)
        {
            return __dbContext.TblUserMas.Where(x => x.UserId == UserId && x.Pwd == Pwd)
                .Select(x => new UserModel
                {
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
        public string usp_Dashboard(string UserId, string Pwd)
        {
            DataSet ds = new DataSet();
            string data = "";
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_Dashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@Pwd", Pwd);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                data = Convert.ToString(ds.Tables[0].Rows[0]["JsonData"]);
                con.Close();
            }
            return data;
        }
        public UserModel LoginV2(string UserId, string Pwd)
        {

            UserModel data = new UserModel();

            string ErrMsg = "";
            string dd = usp_Dashboard(UserId, Pwd);
            if (dd != null)
            {
                List<UserModel> aa = JsonConvert.DeserializeObject<List<UserModel>>(dd);
                if (aa != null)
                {
                    data = aa[0]; 
                }
            }

            return data;
        }

        public void Logout()
        {

        }


    }
}

