using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace SSRepository.Repository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {

        public LoginRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {

        }


        public bool ValidateUser(long UserId)
        {

            UserModel data = new UserRepository(__dbContext, _contextAccessor).GetSingleRecord(UserId);
            if (data != null && data.PKID > 0)
            {
                SysDefaults Sys = GetSysDefaults();

                var result = (from c in __dbContext.TblUserLocLnk
                              where c.FKUserID == UserId
                              select (new
                              {
                                  c.FKLocationID,
                                  c.FKLocation.IsBillingLocation
                              })
                           ).ToList();


                Sys.BillingLocation = string.Join(",", result.Where(x => x.IsBillingLocation == true).Select(r => r.FKLocationID));
                Sys.Location = string.Join(",", result.Select(r => r.FKLocationID));
                Sys.FkRoleId = data.FkRoleId;
                Sys.IsAdmin = data.IsAdmin;
                Sys.EditBatch = data.EditBatch;
                Sys.EditColor = data.EditColor;
                Sys.EditDiscount = data.EditDiscount;
                Sys.EditRate = data.EditRate;
                Sys.EditMRP = data.EditMRP;
                Sys.EditPurRate = data.EditPurRate;
                Sys.EditPurDiscount = data.EditPurDiscount;

                SaveFile("sysdefaults.json", JsonConvert.SerializeObject(Sys));

                RoleModel role = new RoleRepository(__dbContext, _contextAccessor).GetSingleRecord(data.FkRoleId, true);
                SaveFile("menulist.json", JsonConvert.SerializeObject(role.RoleDtls));
                return true;
            }
            else {  return false; }
        }


        private void SaveFile(string FileName, string Data)
        {

            string companyName = _contextAccessor.HttpContext.Session.GetString("CompanyName") ?? "";
            string userId = _contextAccessor.HttpContext.Session.GetString("UserID") ?? "";

            string path = Path.Combine("wwwroot", "Data", companyName, userId); 
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            string filePath = Path.Combine(path, FileName);

            // Always overwrite
            File.WriteAllText(filePath, Data);

            //string filePath = Path.Combine("wwwroot", "Data", companyName, userId, FileName);


            //FileInfo file = new FileInfo(filePath);
            //if (file.Exists)
            //    file.Delete();
            //using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            //{
            //    using (var writer = new StreamWriter(fileStream))
            //    {
            //        writer.Write(Data);
            //    }
            //}

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

