using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using SSRepository.Data;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSRepository.Repository.BaseRepository;

namespace SSRepository.Repository.Report
{
    public class WalkingCreditAmtRepository : ReportBaseRepository, IWalkingCreditAmtRepository
    {
        public WalkingCreditAmtRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_WalkingCreditAmt";
        }
        public DataTable GetList(string ReportType = "", string PartyMobile = "")
        {
            string LocationFilter = GetLocationFilter();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReportType", ReportType);
                cmd.Parameters.AddWithValue("@PartyMobile", PartyMobile);
                cmd.Parameters.AddWithValue("@LocationFilter", GetFilterData(LocationFilter));

                //Get Output Parametr
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();

            }
            return dt;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>();
            if (GridName == "D")
            {
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "SrNo", Fields = "sno", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Date", Fields = "Entrydt", Width = 15, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Inum", Fields = "Inum", Width = 15, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Amount", Fields = "CreditAmt", Width = 25, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "CreditAmt" });
             }
            else
            {
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "SrNo", Fields = "sno", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Name", Fields = "PartyName", Width = 15, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Mobile", Fields = "PartyMobile", Width = 15, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Amount", Fields = "TotalCreditAmt", Width = 25, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "TotalCreditAmt" });
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "View", Fields = "View", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
            }
            return list;
        }
    }
}
