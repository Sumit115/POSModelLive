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


        public DataTable GetList(string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter, string PartyFilter, string LocationFilter, string SeriesFilter)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(ProductFilter))
                {
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                }
                if (!string.IsNullOrEmpty(ProductFilter))
                {
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                }
                if (!string.IsNullOrEmpty(ProductFilter))
                {
                    cmd.Parameters.AddWithValue("@ReportType", ReportType);
                }
                if (!string.IsNullOrEmpty(TranAlias))
                {
                    cmd.Parameters.AddWithValue("@ProductFilter", TranAlias);
                } 
                //SqlParameter param = new SqlParameter("@userdefinedtabletypeparameter", SqlDbType.Structured)
                //{
                //    TypeName = "dbo.userdefinedtabletype",
                //    Value = dt
                //};
                if (!string.IsNullOrEmpty(ProductFilter))
                {
                    cmd.Parameters.AddWithValue("@ProductFilter", GetFilterData(ProductFilter));
                }
                if (!string.IsNullOrEmpty(PartyFilter))
                {
                    cmd.Parameters.AddWithValue("@PartyFilter", GetFilterData(PartyFilter));
                }
                if (!string.IsNullOrEmpty(LocationFilter))
                {
                    cmd.Parameters.AddWithValue("@LocationFilter", GetFilterData(LocationFilter));
                }
                if (!string.IsNullOrEmpty(SeriesFilter))
                {
                    cmd.Parameters.AddWithValue("@SeriesFilter", GetFilterData(SeriesFilter));
                }
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
