using Microsoft.AspNetCore.Http;
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
        public ImportRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {

        }
        public List<string> SaveData_FromFile(string filePath)
        {
            List<string> validationErrors = new List<string>();
            List<TranDetails> tranList = new List<TranDetails>();

            DataTable dt = new DataTable();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string headerLine = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(headerLine))
                    throw new Exception("Invalid file format (no headers).");

                string[] headers = headerLine.Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Trim());
                }
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue; // ✅ skip empty lines

                    string[] rows = line.Split(',');
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (i < rows.Length)
                            dr[i] = rows[i].Trim();
                        else
                            dr[i] = ""; // ✅ handle missing columns safely
                    }

                    dt.Rows.Add(dr);
                }

                sr.Close();
            }

            if (dt.Rows.Count > 0)
            {
                var cs = GetSysDefaultsByKey("CodingScheme");
                if (!string.IsNullOrEmpty(cs))
                {

                    int srNo = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        srNo++;
                        string error = "";
                        int qty = 0;
                        decimal mrp = 0;

                        if (string.IsNullOrWhiteSpace(dr["Artical"]?.ToString()))
                        {
                            error += $" Artical is blank.";
                        }
                        else if (!IsAlphanumeric(dr["Barcode"]?.ToString()))
                        {
                            error += $"Barcode Must Be Alphanumeric. ";
                        }
                        else if (cs == "Unique" && tranList.Where(x => x.Barcode?.ToString().ToLower() == dr["Barcode"]?.ToString().ToLower().Trim()).Count() > 0)
                        {
                            error += $" Duplicate Barcode {dr["Barcode"]?.ToString()}. ";
                        }
                        else if (string.IsNullOrWhiteSpace(dr["Qty"]?.ToString()))
                        {
                            error += $" Row {srNo}: Qty is blank. ";
                        }
                        else if (!int.TryParse(dr["Qty"]?.ToString(), out qty))
                        {
                            error += $" Row {srNo}: Qty must be a valid integer. ";
                        }
                        else if (qty <= 0)
                        {
                            error += $" Row {srNo}: Qty must be greater than 0. ";
                        }
                        else if (string.IsNullOrWhiteSpace(dr["MRP"]?.ToString()))
                        {
                            error += $" Row {srNo}: MRP is blank. ";
                        }
                        else if (!decimal.TryParse(dr["MRP"]?.ToString(), out mrp))
                        {
                            error += $" Row {srNo}: MRP must be a valid decimal. ";
                        }
                        else if (mrp <= 0)
                        {
                            error += $" Row {srNo}: MRP must be greater than 0. ";
                        }

                        if (error != "")
                            validationErrors.Add($"Row {srNo} {error}");
                        else
                        {
                            tranList.Add(new TranDetails
                            {
                                SrNo = srNo,
                                Barcode = dr["Barcode"]?.ToString().Trim(),
                                Product = dr["Artical"]?.ToString(),
                                SubCategoryName = dr["SubSection"].ToString(),
                                Batch = dr["Size"].ToString(),
                                Qty = qty,          // ✅ already validated integer
                                MRP = mrp,
                                RateUnit = "1",
                                SchemeDiscType = "R",
                                TradeDiscType = "R",
                                LotDiscType = "R",
                                SaleRate = Math.Round(mrp, 2),
                                TradeRate = Math.Round(mrp, 2),
                                DistributionRate = Math.Round(mrp, 2),
                                ModeForm = 0,
                                Color = "NA",
                            });
                        }
                    }
                }
                else
                    throw new Exception("Please Update CodingScheme From System Default");


                if (validationErrors.Count == 0)
                {
                    string Error = "";
                    SaveData(tranList, ref Error);
                    validationErrors.Add(Error);
                }
            }
            else
                throw new Exception("Invalid Data");
            return validationErrors;
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
