using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Option;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Repository.Option
{
    public class ImportRepository : BaseRepository, IImportRepository
    {
        public ImportRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public string SaveData(List<TranDetails> model)
        {
            string Error = "";
            setDefaultBeforeSave(model);

            Error = ValidateData(model);
            if (Error == "")
            { 
                SaveData(model,ref Error);
            }
            return Error;
        }
        public void setDefaultBeforeSave(List<TranDetails> model)
        {
            if (model != null)
            {
                // var _branch = new BranchRepository(__dbContext).GetSingleRecord(model.FKLocationID);
                //  __dbContext.Dispose();
                 foreach (var item in model)
                {
                    item.RateUnit = !string.IsNullOrEmpty(item.RateUnit) ? item.RateUnit : "1";
                    item.SchemeDiscType = item.TradeDiscType = item.LotDiscType = "R";
                    item.SaleRate = item.TradeRate = item.DistributionRate = item.MRP;
                    item.ModeForm = 0;
                    item.Color = "NA";
                }

            }

        }

        public virtual string ValidateData(List<TranDetails> objmodel)
        {
            string Error = "";
            try
            {


            }
            catch (Exception ex) { Error = ex.Message; }
            return Error;
        }

        public void SaveData(List<TranDetails> JsonData, ref string ErrMsg)
        {

            var aa = JsonConvert.SerializeObject(JsonData);


            using (SqlConnection con = new SqlConnection(conn))
            {
                //con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("usp_ImportStock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JsonData", JsonConvert.SerializeObject(JsonData)); 
                cmd.Parameters.Add(new SqlParameter("@ErrMsg", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "ErrMsg", DataRowVersion.Default, null));
                cmd.ExecuteNonQuery();
                ErrMsg = Convert.ToString(cmd.Parameters["@ErrMsg"].Value);
                con.Close();
            }
        }

    }
}
