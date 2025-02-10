using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace SSRepository.Repository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        //protected readonly AppDbContext __dbContext;
        //public LoginRepository(AppDbContext dbContext)
        //{
        //    __dbContext = dbContext;
        //}
        public LoginRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
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


        public string UserMenu(long UserId)
        {
            DataSet ds = new DataSet();
            string data = "";
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_UserMenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PkUserId", UserId);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                data = Convert.ToString(ds.Tables[0].Rows[0]["JsonData"]);
                if (data != null)
                {
                    List<MenuModel> aa = BuildMenuTree(JsonConvert.DeserializeObject<List<MenuModel>>(data), null);
                    data = JsonConvert.SerializeObject(aa);
                }
                else
                {
                    data = "";
                }
                con.Close();
            }
            return data;
        }

        private List<MenuModel> BuildMenuTree(List<MenuModel>? allMenuItems, long? parentId)
        {
            if (allMenuItems != null)
            {
                return allMenuItems
               .Where(m => m.FKMasterFormID == parentId)
               .Select(m => new MenuModel
               {
                   PKFormID = m.PKFormID,
                   FKMasterFormID = m.FKMasterFormID,
                   SeqNo = m.SeqNo,
                   FormName = m.FormName,
                   Image = m.Image,
                   WebURL = m.WebURL,
                   IsAccess = m.IsAccess,
                   IsEdit = m.IsEdit,
                   IsCreate = m.IsCreate,
                   IsPrint = m.IsPrint,
                   IsBrowse = m.IsBrowse,
                   SubMenu = BuildMenuTree(allMenuItems, m.PKFormID) // Recursive call for nested menus
               })
               .ToList();
            }
            else
            { 
                return new List<MenuModel>();
            }

           
        }


        public void Logout()
        {

        }


    }
}

