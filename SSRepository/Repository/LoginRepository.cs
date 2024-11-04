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

        public string usp_Dashboard(long UserId)
        {
            DataSet ds = new DataSet();
            string data = "";
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_ValidateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PkUserId", UserId);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                data = Convert.ToString(ds.Tables[0].Rows[0]["JsonData"]);
                con.Close();
            }
            return data;
        }
        public UserModel ValidateUser(long UserId)
        {

            UserModel data = new UserModel();

            string ErrMsg = "";
            string dd = usp_Dashboard(UserId);
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

