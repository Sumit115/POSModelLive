using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SSRepository.Repository.Report
{
    public class ReportBaseRepository : BaseRepository
    {
         public string GetSP = "";
        public ReportBaseRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
      
        
        public DataTable GetList(string FromDate, string ToDate, string ReportType , string TranAlias, DataTable ProductFilter=null  , DataTable CustomerFilter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@ReportType", ReportType);
                cmd.Parameters.AddWithValue("@TranAlias", TranAlias);
                //SqlParameter param = new SqlParameter("@userdefinedtabletypeparameter", SqlDbType.Structured)
                //{
                //    TypeName = "dbo.userdefinedtabletype",
                //    Value = dt
                //};
                cmd.Parameters.AddWithValue("@ProductFilter", ProductFilter);
                cmd.Parameters.AddWithValue("@CustomerFilter", CustomerFilter); 
                //Get Output Parametr
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();

            }
            return dt;
        }
          

    }
}
