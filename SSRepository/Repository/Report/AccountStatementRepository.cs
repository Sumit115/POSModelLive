using Azure;
using SSRepository.Data;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using System.Xml.Linq;
using static SSRepository.Repository.BaseRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Timers;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Numerics;

namespace SSRepository.Repository.Report
{
    public class AccountStatementRepository : ReportBaseRepository, IAccountStatementRepository
    {


        public AccountStatementRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_AccountStatement";
        }
        public DataTable GetList(long FKAccountID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.AddWithValue("@FKAccountID", FKAccountID); 
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
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Cr",    Fields="CreditAmt",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Dr",    Fields="DebitAmt",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Narration",    Fields="VoucherNarration",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }

    }
}
