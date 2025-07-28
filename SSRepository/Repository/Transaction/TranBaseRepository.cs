using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class TranBaseRepository : BaseRepository
    {
        //public string TranType = "";
        //public string TranAlias = "";
        //public string StockFlag = "";
        //public bool PostInAc = false;
        public string SPAddUpd = "";
        public string SPList = "";
        public string SPById = "";

        public TranBaseRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {

        }

        public string Create(TransactionModel model)
        {
            string Error = "";

            Error = ValidateData(model);
            if (Error == "")
            {
                setVoucherDetails(model);

                long Id = 0;
                long SeriesNo = 0;
                //  var aa = JsonConvert.SerializeObject(model);
                SaveData(model, ref Id, ref Error, ref SeriesNo);
                //Error = "p";
            }
            return Error;
        }

        public string ValidateData(TransactionModel objmodel)
        {

            string Error = "";
            try
            {
                if (objmodel.FkPartyId <= 0 || objmodel.FkPartyId == null)
                    throw new Exception("Party Detail Required");

                if (objmodel.FKSeriesId <= 0 || objmodel.FKSeriesId == null)
                    throw new Exception("Series Required");

                //
                setDefaultBeforeSave(objmodel);

                if (objmodel.ExtProperties.TranType == "S" && objmodel.IsTranChange && objmodel.TranAlias != "LINV" && objmodel.TranAlias != "LORD")
                    setPromotion(objmodel);

                //CalculateExe(objmodel);


                if (objmodel.TranDetails != null)
                {
                    foreach (var item in objmodel.TranDetails.Where(x => x.FkProductId > 0 && x.ModeForm != 2))
                    {
                        //if (string.IsNullOrEmpty(item.Color))
                        //{
                        //    throw new Exception("Color Required on Product " + item.Product);
                        //}

                        if (objmodel.TranAlias == "PINV" || objmodel.TranAlias == "PJ_R")
                        {
                            if (item.ModeForm != 0 && (objmodel.TranAlias == "PINV" || objmodel.TranAlias == "PJ_R"))
                            {
                                var _check = __dbContext.TblSalesInvoicedtl.Where(x => x.FkLotId == item.FkLotId && x.FkProductId == item.FkProductId).FirstOrDefault();
                                if (_check != null) { throw new Exception("Product Not Update After Sale :" + item.Product); }
                            }
                            if (item.CodingScheme == "fixed")
                            {
                                if (objmodel.UniqIdDetails != null)
                                {
                                    var _bQty = objmodel.UniqIdDetails.Where(x => x.SrNo == item.SrNo).ToList();
                                    if (_bQty.Count > 0) { throw new Exception("Product (" + item.Product + ")  Barcode Not Allowed"); }
                                }
                            }
                            else if (item.CodingScheme == "Lot")
                            {
                                if (objmodel.UniqIdDetails != null)
                                {
                                    var _bQty = objmodel.UniqIdDetails.Where(x => x.SrNo == item.SrNo).ToList();
                                    if (_bQty.Count > 1) { throw new Exception("Only 1 Barcode Required Product (" + item.Product + ") "); }
                                }
                            }
                            else
                            {
                                if (objmodel.UniqIdDetails != null)
                                {
                                    var _bQty = objmodel.UniqIdDetails.Where(x => x.SrNo == item.SrNo).ToList();
                                    if (_bQty.Count > (item.Qty + item.FreeQty)) { throw new Exception("Product (" + item.Product + ") Qty & Barcode Qty Not Match"); }
                                }
                            }
                        }

                        if (objmodel.UniqIdDetails != null && (objmodel.ExtProperties.StockFlag == "O"))
                        {
                            var _bQty = objmodel.UniqIdDetails.Where(x => x.SrNo == item.SrNo).ToList();
                            if (item.CodingScheme == "Unique")
                            {
                                if (_bQty.Count != (item.Qty + item.FreeQty)) { throw new Exception("Product (" + item.Product + ") Qty & Barcode Qty Not Match"); }
                            }
                        }

                        if (string.IsNullOrEmpty(item.Batch))
                        {
                            throw new Exception("Size Required on Product " + item.Product);
                        }
                        CalculateExe(objmodel, item);
                    }

                }

                if (objmodel.TranReturnDetails != null)
                {
                    foreach (var item in objmodel.TranReturnDetails.Where(x => x.FkProductId > 0 && x.ModeForm != 2))
                    {
                        //For JobWork
                        if (objmodel.TranAlias == "PJ_I" && objmodel.UniqIdReturnDetails != null)
                        {
                            var _bQty = objmodel.UniqIdReturnDetails.Where(x => x.SrNo == item.SrNo).ToList();
                            if (item.CodingScheme == "Unique")
                            {
                                if (_bQty.Count != (item.Qty + item.FreeQty)) { throw new Exception("Product (" + item.Product + ") Qty & Barcode Qty Not Match"); }
                            }
                        }
                        if (string.IsNullOrEmpty(item.Batch))
                        {
                            throw new Exception("Size Required on Product " + item.Product);
                        }
                        CalculateExe(objmodel, item);
                    }

                }

                if (objmodel.FKPostAccID <= 0 && (objmodel.Cheque || objmodel.Credit))
                {
                    throw new Exception("Account Not Found");
                }

                //if (objmodel.CreditCard && (objmodel.CreditCardAmt < 0 || objmodel.CreditCardNo == "" || objmodel.CreditCardDate == null || objmodel.FKBankCreditCardID == null))
                //    throw new Exception("Please Enter Valid Card Detail");

                //if (objmodel.Cheque && (objmodel.ChequeAmt < 0 || objmodel.ChequeNo == "" || objmodel.ChequeDate == null || objmodel.FKBankChequeID == null))
                //    throw new Exception("Please Enter Valid Cheque Detail");

                //if (objmodel.Credit && (objmodel.CreditAmt < 0 || objmodel.CreditDate == null || objmodel.FKPostAccID == null))
                //    throw new Exception("Please Enter Valid Credit Detail");


                //if (objmodel.VoucherDetails != null)
                //{
                //    if (objmodel.VoucherDetails.ToList().Sum(x => x.CreditAmt) != objmodel.VoucherDetails.ToList().Sum(x => x.DebitAmt))
                //        throw new Exception("Please Enter Valid Amount");
                //}

                if (objmodel.TrnStatus.Trim() == "I" || objmodel.TrnStatus.Trim() == "C")
                    throw new Exception("Invalid Request");


                Error = ValidData(objmodel);
                SetGridTotal(objmodel);
                SetPaymentDetail(objmodel);
                //set Promotion Invoice Value
                if (objmodel.ExtProperties.TranType == "S" && objmodel.IsTranChange && objmodel.TranAlias != "LINV" && objmodel.TranAlias != "LORD")
                    setPromotion_InvoiceValue(objmodel);
                objmodel.IsTranChange = false;


            }
            catch (Exception ex) { Error = ex.Message; }
            return Error;
        }

        public virtual string ValidData(TransactionModel objmodel)
        {
            string Error = "";
            try
            {


            }
            catch (Exception ex) { Error = ex.Message; }
            return Error;
        }


        public enum AccountId
        {
            PURCHASE_TAXABLE_GOODS = 1,
            SALES_TAXABLE_GOODS = 2,
            SGST_INPUT = 3,
            SGST_OUTPUT = 4,
            CGST_INPUT = 5,
            CGST_OUTPUT = 6,
            IGST_INPUT = 7,
            IGST_OUTPUT = 8,
            CASH_IN_HAND = 9,
            BANK_ACCOUNTS = 10,
            ROUND_OFF_AC = 11,
            Walking_Customer = 12,
        }
        public void setDefaultBeforeSave(TransactionModel model)
        {
            model.FreightType = string.IsNullOrEmpty(model.FreightType) ? "FOB_Paid(Add)" : model.FreightType;
            model.PaymentMode = string.IsNullOrEmpty(model.PaymentMode) ? "C" : model.PaymentMode;
            if (model.EWayDetail != null)
            {
                model.EWayDetail.SupplyType = string.IsNullOrEmpty(model.EWayDetail.SupplyType) ? "Supply" : model.EWayDetail.SupplyType;
                model.EWayDetail.TransMode = string.IsNullOrEmpty(model.EWayDetail.TransMode) ? "Road" : model.EWayDetail.TransMode;
                model.EWayDetail.VehicleType = string.IsNullOrEmpty(model.EWayDetail.VehicleType) ? "Regular" : model.EWayDetail.VehicleType;
            }
            if (model.TranDetails != null)
            {
                // var _branch = new BranchRepository(__dbContext).GetSingleRecord(model.FKLocationID);
                //  __dbContext.Dispose();
                model.SgstAmt = 0;
                foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
                {
                    item.RateUnit = !string.IsNullOrEmpty(item.RateUnit) ? item.RateUnit : "1";
                    item.SchemeDiscType = item.TradeDiscType = item.LotDiscType = "R";

                    if (model.BranchStateName == model.PartyStateName)
                    {
                        item.SCRate = Math.Round(item.GstRate / 2, 2);
                        item.SCAmt = Math.Round(item.GstAmt / 2, 2);
                        model.SgstAmt += item.SCAmt;
                    }
                    else
                    {
                        item.ICRate = Math.Round(item.GstRate, 2);
                        item.ICAmt = Math.Round(item.GstAmt, 2);
                    }
                }

            }

        }
        public void setVoucherDetails(TransactionModel model)
        {

            //    model.VoucherDetails[rowIndex].DebitAmt = 0;
            //    model.VoucherDetails[rowIndex].VoucherAmt = model.VoucherDetails[rowIndex].CreditAmt;
            //}
            //        if (fieldName == "Debit")
            //        {
            //            model.VoucherDetails[rowIndex].CreditAmt = 0;
            //            model.VoucherDetails[rowIndex].VoucherAmt = -model.VoucherDetails[rowIndex].DebitAmt;
            //Voucher Detail
            model.VoucherDetails = new List<VoucherDetails>();
            if (model.TranAlias == "PORD" || model.TranAlias == "PINV")
            {
                if (model.Credit || model.Cheque)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 1,
                        FkAccountId = (int)model.FKPostAccID,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = ((decimal)model.CreditAmt + (decimal)model.ChequeAmt),
                        VoucherAmt = ((decimal)model.CreditAmt + (decimal)model.ChequeAmt),
                    });
                }

                if (model.Cash)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 2,
                        FkAccountId = (long)AccountId.CASH_IN_HAND,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = (decimal)model.CashAmt,
                        VoucherAmt = (decimal)model.CashAmt,
                    });
                }
                if (model.CreditCard)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 3,
                        FkAccountId = (long)AccountId.BANK_ACCOUNTS,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = (decimal)model.CreditCardAmt,
                        VoucherAmt = (decimal)model.CreditCardAmt,
                    });
                }

                model.VoucherDetails.Add(new VoucherDetails()
                {
                    FKSeriesId = model.FKSeriesId,
                    SrNo = 4,
                    FkAccountId = (long)AccountId.PURCHASE_TAXABLE_GOODS,
                    FKLocationID = model.FKLocationID,
                    DebitAmt = model.GrossAmt,
                    VoucherAmt = -model.GrossAmt,
                });
                if (model.RoundOfDiff > 0)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 5,
                        FkAccountId = (long)AccountId.ROUND_OFF_AC,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = model.RoundOfDiff,
                        VoucherAmt = model.RoundOfDiff,
                    });
                }
                if (model.BranchStateName == model.PartyStateName)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 6,
                        FkAccountId = (long)AccountId.CGST_INPUT,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = model.TaxAmt / 2,
                        VoucherAmt = -model.TaxAmt / 2,
                    });
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 7,
                        FkAccountId = (long)AccountId.SGST_INPUT,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = model.TaxAmt / 2,
                        VoucherAmt = -model.TaxAmt / 2,
                    });

                }
                else
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 6,
                        FkAccountId = (long)AccountId.IGST_INPUT,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = model.TaxAmt,
                        VoucherAmt = -model.TaxAmt,
                    });
                }


            }
            else//For Sales
            {
                if (model.Credit || model.Cheque)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 1,
                        FkAccountId = (int)model.FKPostAccID,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = ((decimal)model.CreditAmt + (decimal)model.ChequeAmt),
                        VoucherAmt = -((decimal)model.CreditAmt + (decimal)model.ChequeAmt),
                    });
                }

                if (model.Cash)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 2,
                        FkAccountId = (long)AccountId.CASH_IN_HAND,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = (decimal)model.CashAmt,
                        VoucherAmt = -(decimal)model.CashAmt,
                    });
                }
                if (model.CreditCard)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 3,
                        FkAccountId = (long)AccountId.BANK_ACCOUNTS,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = (decimal)model.CreditCardAmt,
                        VoucherAmt = -(decimal)model.CreditCardAmt,
                    });
                }

                model.VoucherDetails.Add(new VoucherDetails()
                {
                    FKSeriesId = model.FKSeriesId,
                    SrNo = 4,
                    FkAccountId = (long)AccountId.SALES_TAXABLE_GOODS,
                    FKLocationID = model.FKLocationID,
                    CreditAmt = model.GrossAmt,
                    VoucherAmt = model.GrossAmt,
                });
                if (model.RoundOfDiff > 0)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 5,
                        FkAccountId = (long)AccountId.ROUND_OFF_AC,
                        FKLocationID = model.FKLocationID,
                        DebitAmt = model.RoundOfDiff,
                        VoucherAmt = -model.RoundOfDiff,
                    });
                }
                if (model.BranchStateName == model.PartyStateName)
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 6,
                        FkAccountId = (long)AccountId.CGST_OUTPUT,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = model.TaxAmt / 2,
                        VoucherAmt = model.TaxAmt / 2,
                    });
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 7,
                        FkAccountId = (long)AccountId.SGST_OUTPUT,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = model.TaxAmt / 2,
                        VoucherAmt = model.TaxAmt / 2,
                    });

                }
                else
                {
                    model.VoucherDetails.Add(new VoucherDetails()
                    {
                        FKSeriesId = model.FKSeriesId,
                        SrNo = 6,
                        FkAccountId = (long)AccountId.IGST_OUTPUT,
                        FKLocationID = model.FKLocationID,
                        CreditAmt = model.TaxAmt,
                        VoucherAmt = model.TaxAmt,
                    });
                }

            }
        }

        public void SaveData(TransactionModel JsonData, ref long Id, ref string ErrMsg, ref long SeriesNo)
        {
            try
            {
                string oldJsonData = "";
                if (JsonData.PkId > 0)
                    oldJsonData = GetData(JsonData.PkId, JsonData.FKSeriesId, ref ErrMsg);

                var aa = JsonConvert.SerializeObject(JsonData);
                //JsonData.FKReferById = JsonData.FKReferById > 0 ? JsonData.FKReferById : null;
                //JsonData.FKSalesPerId = JsonData.FKSalesPerId > 0 ? JsonData.FKSalesPerId : null;

                using (SqlConnection con = new SqlConnection(conn))
                {
                    //con.Open();
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand(SPAddUpd, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JsonData", JsonConvert.SerializeObject(JsonData));

                    cmd.Parameters.Add(new SqlParameter("@OutParam", SqlDbType.BigInt, 20, ParameterDirection.Output, false, 0, 10, "OutParam", DataRowVersion.Default, null));
                    cmd.Parameters.Add(new SqlParameter("@SeriesNo", SqlDbType.BigInt, 20, ParameterDirection.Output, false, 0, 10, "SeriesNo", DataRowVersion.Default, null));
                    cmd.Parameters.Add(new SqlParameter("@ErrMsg", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "ErrMsg", DataRowVersion.Default, null));
                    cmd.ExecuteNonQuery();
                    Id = Convert.ToInt64(cmd.Parameters["@OutParam"].Value);
                    SeriesNo = cmd.Parameters["@SeriesNo"].Value != null ? Convert.ToInt64(cmd.Parameters["@SeriesNo"].Value) : 0;
                    ErrMsg = Convert.ToString(cmd.Parameters["@ErrMsg"].Value);
                    con.Close();
                }

                if (string.IsNullOrEmpty(ErrMsg) && JsonData.PkId > 0)
                {
                    TransactionModel oldModel = JsonConvert.DeserializeObject<List<TransactionModel>>(oldJsonData).ToList().FirstOrDefault();

                    AddMasterLog(JsonData.ExtProperties.FKFormID, JsonData.PkId, JsonData.FKSeriesId, oldModel.EntryDate, false, JsonConvert.SerializeObject(oldModel), oldModel.EntryNo.ToString(), JsonData.FKUserId, JsonData.ModifiedDate, oldModel.FKUserId, oldModel.ModifiedDate);
                    SaveClientData();
                }
                JsonData.PkId = Id;
            }
            catch (Exception ex) { throw ex; }
        }

        public DataTable GetList(string FromDate, string ToDate, string SeriesFilter, string DocumentType, string LocationFilter, string StateFilter = "", string StatusFilter = "")
        {
            LocationFilter = string.IsNullOrEmpty(LocationFilter) ? GetLocationFilter() : LocationFilter;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(SPList, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@SeriesFilter", SeriesFilter);
                cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                cmd.Parameters.AddWithValue("@LocationFilter", GetFilterData(LocationFilter));
                if (SeriesFilter == "SORD" || SeriesFilter == "SINV")
                    cmd.Parameters.AddWithValue("@StateFilter", GetFilterData(StateFilter));
                if (!string.IsNullOrEmpty(StatusFilter))
                    cmd.Parameters.AddWithValue("@StatusFilter", StatusFilter);
                //Get Output Parametr
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();

            }
            return dt;
        }

        public string GetData(long PkId, long FkSeriesId, ref string ErrMsg)
        {
            DataSet ds = new DataSet();
            string data = "";
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(SPById, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PkId", PkId);
                cmd.Parameters.AddWithValue("@FkSeriesId", FkSeriesId);
                cmd.Parameters.Add(new SqlParameter("@JsonData", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "JsonData", DataRowVersion.Default, null));
                cmd.Parameters.Add(new SqlParameter("@ErrMsg", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "ErrMsg", DataRowVersion.Default, null));
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                data = Convert.ToString(cmd.Parameters["@JsonData"].Value);
                ErrMsg = Convert.ToString(cmd.Parameters["@ErrMsg"].Value);
                con.Close();
            }
            return data;
        }
        public TransactionModel GetSingleRecord(long PkId, long FkSeriesId)
        {

            TransactionModel data = new TransactionModel();
            if (PkId > 0)
            {
                string ErrMsg = "";
                string dd = GetData(PkId, FkSeriesId, ref ErrMsg);
                if (dd != null)
                {
                    List<TransactionModel> aa = JsonConvert.DeserializeObject<List<TransactionModel>>(dd);
                    if (aa != null)
                    {
                        data = aa[0];
                        data.TrnStatus = data.TrnStatus.Trim().Replace("\0", "").Replace(" ", "");
                        data.IsTranChange = true;
                        if (data.BranchDetails != null)
                        {
                            if (data.BranchDetails.Count > 0)
                            {
                                data.Branch = data.BranchDetails.FirstOrDefault();
                            }
                        }
                        if (data.EWayDetails != null)
                        {
                            if (data.EWayDetails.Count > 0)
                            {
                                data.EWayDetail = data.EWayDetails.FirstOrDefault();
                            }
                        }
                        CalculateExe_For_Update(data);
                        SetSeries(data, data.FKSeriesId);
                        if (data.FKBankThroughBankID > 0)
                            SetBankThroughBank(data, (long)data.FKBankThroughBankID);
                    }

                }
            }
            return data;
        }
        public void CalculateExe_For_Update(TransactionModel model)
        {
            foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            {

                item.GstRate = Math.Round(item.ICRate > 0 ? item.ICRate : (item.SCRate * 2), 2);
                item.GstAmt = Math.Round(item.ICAmt > 0 ? item.ICAmt : (item.SCAmt * 2), 2);
            }

            // model.TranDetails = model.TranDetails.Where(x => x.FkProductId > 0).ToList();
        }


        public object ColumnChange(TransactionModel model, int rowIndex, string fieldName, bool IsReturn)
        {
            try
            {
                if ((IsReturn ? model.TranReturnDetails.Count : model.TranDetails.Count) <= rowIndex)
                    throw new Exception("Invalid");


                switch (fieldName)
                {
                    case "Product":
                        if ((IsReturn ? model.TranReturnDetails[rowIndex].FkId : model.TranDetails[rowIndex].FkId) == 0)
                        {
                            TranDetailDefault(model, (IsReturn ? model.TranReturnDetails[rowIndex] : model.TranDetails[rowIndex]));
                            GetSetProduct(model, (IsReturn ? model.TranReturnDetails[rowIndex] : model.TranDetails[rowIndex]), "", 0, model.FKOrderID, model.FKOrderSrID);
                        }

                        break;
                    case "Batch":
                    case "Color":
                        setBatch(model, (IsReturn ? model.TranReturnDetails[rowIndex] : model.TranDetails[rowIndex]));
                        break;
                    case "TradeDisc":
                        model.TranDetails[rowIndex].TradeDiscAmt = 0;
                        break;
                    case "Delete":
                        if (IsReturn)
                        {
                            model.TranReturnDetails[rowIndex].ModeForm = 2;
                            model.UniqIdReturnDetails = model.UniqIdReturnDetails.Where(x => x.SrNo != model.TranReturnDetails[rowIndex].SrNo).ToList();
                        }
                        else
                        {
                            model.TranDetails[rowIndex].ModeForm = 2;
                            model.UniqIdDetails = model.UniqIdDetails.Where(x => x.SrNo != model.TranDetails[rowIndex].SrNo).ToList();
                        }
                        break;
                    case "ProductReturn":
                        setReturnProduct(model, (IsReturn ? model.TranReturnDetails[rowIndex] : model.TranDetails[rowIndex]));
                        break;
                    case "Inum":
                        setInvoiceinfo(model, (IsReturn ? model.TranReturnDetails[rowIndex] : model.TranDetails[rowIndex]));
                        break;
                    case "Qty":
                    case "FreeQty":
                    default:
                        break;
                }

                CalculateExe(model, (IsReturn ? model.TranReturnDetails[rowIndex] : model.TranDetails[rowIndex]));
                SetGridTotal(model);
                SetPaymentDetail(model);
                model.IsTranChange = true;
            }
            catch (Exception ex) { }
            return model;
        }

        public void TranDetailDefault(TransactionModel model, TranDetails detail)
        {

            if (detail.SrNo == 0)
            {
                var _checkSrNo = model.TranDetails.ToList();
                if (_checkSrNo.Count > 0)
                {
                    detail.SrNo = _checkSrNo.Max(x => x.SrNo) + 1;
                }
                else
                {
                    detail.SrNo = 1;
                }
            }

            if (detail.FKLocationID == 0)
                detail.FKLocationID = model.FKLocationID;

            if (detail.Qty == 0)
                detail.Qty = 1;

            if (detail.FKLocationID == 0)
                detail.FKLocationID = model.FKLocationID;

            if (detail.PromotionType == null)
            {
                detail.PromotionType = "";
                detail.FkPromotionId = null;
                detail.PromotionName = "";
            }

            if (detail.ReturnTypeID == 0)
                detail.ReturnTypeID = 2;
        }

        public object ApplyRateDiscount(TransactionModel model, string type, decimal discount)
        {

            if (type == "LIR")
            {
                foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
                {
                    item.TradeDisc = 0;
                    item.TradeDiscAmt = discount;
                }
                CalculateExe(model);
            }
            else if (type == "LIP")
            {
                foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
                {
                    item.TradeDisc = discount;
                    item.TradeDiscAmt = 0;
                }
                CalculateExe(model);
            }
            else if (type == "F")
            {
                foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
                {
                    item.Rate = discount;
                }
                CalculateExe(model);
            }

            SetGridTotal(model);
            SetPaymentDetail(model);
            model.IsTranChange = true;
            return model;
        }

        public void GetSetProduct(TransactionModel model, TranDetails detail, string Barcode = "", long LotId = 0, long? FKOrderID = 0, long? FKOrderSrID = 0)
        {

            DataTable dtProduct = GetProduct(Barcode, detail.FKLocationID, detail.FkProductId, LotId, "", false, model.TranAlias, model.FkPartyId, FKOrderID, FKOrderSrID);

            SetProduct(model, detail, dtProduct);
        }

        public DataTable GetProduct(string Barcode, Int64 FKLocationID, long ProductId, long LotId, string ProductName, bool ItemByBarcode, String TranAlias, Int64 FKPartyID = 0, long? FKOrderID = 0, long? FKOrderSrID = 0)
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetProductDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", Barcode);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@LotId", LotId);
                cmd.Parameters.AddWithValue("@ProductName", ProductName);
                cmd.Parameters.AddWithValue("@FKOrderID", FKOrderID);
                cmd.Parameters.AddWithValue("@FKOrderSrID", FKOrderSrID);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();
            }
            return dt;
        }
        public DataTable GetProductReturn(string Barcode, Int64 FKLocationID, long ProductId, long LotId, string ProductName, bool ItemByBarcode, String TranAlias, Int64 FKPartyID = 0, long? FKOrderID = 0, long? FKOrderSrID = 0)
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetProductDetailReturn", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Barcode", Barcode);
                cmd.Parameters.AddWithValue("@FKPartyID", FKPartyID);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();
            }
            return dt;
        }

        private void SetProduct(TransactionModel model, TranDetails detail, DataTable dtProduct)
        {
            if (dtProduct.Rows.Count > 0)
            {
                detail.FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
                detail.FkLotId = Convert.ToInt64(dtProduct.Rows[0]["PkLotId"].ToString());
                detail.Color = dtProduct.Rows[0]["Color"].ToString();
                detail.Batch = dtProduct.Rows[0]["Batch"].ToString();

                detail.FkBrandId = Convert.ToInt64(dtProduct.Rows[0]["FkBrandId"].ToString());
                detail.FKProdCatgId = Convert.ToInt64(dtProduct.Rows[0]["FKProdCatgId"].ToString());
                detail.Product = dtProduct.Rows[0]["Product"].ToString();
                detail.CodingScheme = dtProduct.Rows[0]["CodingScheme"].ToString();

                detail.MRP = Convert.ToDecimal(dtProduct.Rows[0]["MRP"].ToString());
                detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["SaleRate"].ToString());
                detail.TradeRate = Convert.ToDecimal(dtProduct.Rows[0]["TradeRate"].ToString());
                detail.DistributionRate = Convert.ToDecimal(dtProduct.Rows[0]["DistributionRate"].ToString());

                detail.Rate = Convert.ToDecimal(dtProduct.Rows[0][model.BillingRate].ToString());
                detail.FKOrderID = Convert.ToInt64(dtProduct.Rows[0]["FkOrderId"].ToString()); ;
                detail.FKOrderSrID = Convert.ToInt64(dtProduct.Rows[0]["FKOrderSrID"].ToString()); ;
                detail.OrderSrNo = Convert.ToInt64(dtProduct.Rows[0]["OrderSrNo"].ToString()); ;
                if (model.TranAlias == "PORD" || model.TranAlias == "PINV" || (model.TranAlias == "PJ_R" && detail.TranType == "I"))
                {
                    detail.FkLotId = 0;
                }

                if (model.FkPartyId > 0 && model.ExtProperties.TranType == "S" && model.TranAlias != "LORD" && model.TranAlias != "LINV")
                {
                    var _cust = new CustomerRepository(__dbContext, _contextAccessor).GetSingleRecord(model.FkPartyId);
                    detail.TradeDisc = _cust.Disc;
                }

            }
            else
            {
                detail.FkProductId = 0;
            }
        }

        private void setBatch(TransactionModel model, TranDetails detail)
        {

            DataTable dtProduct = GetProduct("", detail.FKLocationID, detail.FkProductId, detail.FkLotId, "", false, model.ExtProperties.TranAlias);
            if (dtProduct.Rows.Count > 0)
            {
                detail.FkLotId = Convert.ToInt64(dtProduct.Rows[0]["PkLotId"].ToString()); ;
                detail.Color = dtProduct.Rows[0]["Color"].ToString();
                detail.Batch = dtProduct.Rows[0]["Batch"].ToString();
                detail.MRP = Convert.ToDecimal(dtProduct.Rows[0]["MRP"].ToString());
                detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["SaleRate"].ToString());
                detail.TradeRate = Convert.ToDecimal(dtProduct.Rows[0]["TradeRate"].ToString());
                detail.DistributionRate = Convert.ToDecimal(dtProduct.Rows[0]["DistributionRate"].ToString());
                detail.Rate = Convert.ToDecimal(dtProduct.Rows[0][model.BillingRate].ToString());
            }
        }
        public object BarcodeScan(TransactionModel model, string Barcode, bool isCalGridTotal, bool IsReturn)
        {
            if (IsReturn)
            {
                DataTable dtProduct = GetProductReturn(Barcode, model.FKLocationID, 0, 0, "", false, model.TranAlias, model.FkPartyId, model.FKOrderID, model.FKOrderSrID);
                if (dtProduct.Rows.Count > 0)
                {
                    if (model.FkPartyId <= 0)
                    {
                        model.FkPartyId = Convert.ToInt64(dtProduct.Rows[0]["FkPartyId"].ToString());
                        model.PartyName = dtProduct.Rows[0]["PartyName"].ToString();
                        model.PartyMobile = dtProduct.Rows[0]["PartyMobile"].ToString();
                        model.PartyAddress = dtProduct.Rows[0]["PartyAddress"].ToString();
                        model.PartyDob = dtProduct.Rows[0]["PartyDob"].ToString();
                        model.PartyMarriageDate = dtProduct.Rows[0]["PartyMarriageDate"].ToString();
                    }

                    long FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
                    long FkLotId = Convert.ToInt64(dtProduct.Rows[0]["PkLotId"].ToString());
                    var drdetail = model.TranReturnDetails.Where(x => x.FkProductId == FkProductId && x.FkLotId == FkLotId && x.ModeForm != 2)
                        .FirstOrDefault();
                    if (drdetail != null)
                    {
                        int rowIndex = model.TranReturnDetails.FindIndex(a => a.FkProductId == FkProductId && a.FkLotId == FkLotId && a.ModeForm != 2);

                        SetProduct(model, drdetail, dtProduct);


                        CalculateExe(model, model.TranReturnDetails[rowIndex]);
                        // Check Product UNique/Lot/PRoduct
                        var _check = model.UniqIdDetails.ToList().Where(x => x.Barcode == Barcode).FirstOrDefault();
                        if (_check == null)
                        {
                            model.UniqIdDetails.Add(new BarcodeUniqVM() { SrNo = model.TranReturnDetails[rowIndex].SrNo, Barcode = Barcode });
                        }
                    }
                    else
                    {
                        var detail = new TranDetails();
                        TranDetailDefault(model, detail);
                        SetProduct(model, detail, dtProduct);
                        detail.Barcode = "Barcode";
                        detail.BarcodeTest = Barcode;

                        detail.FKInvoiceID = Convert.ToInt64(dtProduct.Rows[0]["FKInvoiceID"].ToString());
                        detail.FKInvoiceSrID = Convert.ToInt64(dtProduct.Rows[0]["FKSeriesId"].ToString());
                        detail.FKInvoiceID_Text = dtProduct.Rows[0]["FKInvoiceID_Text"].ToString();

                        CalculateExe(model, detail);
                        model.TranReturnDetails.Add(detail);

                        // Check Product UNique/Lot/PRoduct
                        model.UniqIdReturnDetails.Add(new BarcodeUniqVM() { SrNo = detail.SrNo, Barcode = Barcode });

                    }
                }
                else { throw new Exception("Data Not Found"); }

            }
            else
            {
                DataTable dtProduct = GetProduct(Barcode, model.FKLocationID, 0, 0, "", false, model.TranAlias, model.FkPartyId, model.FKOrderID, model.FKOrderSrID);
                if (dtProduct.Rows.Count > 0)
                {
                    long FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
                    long FkLotId = Convert.ToInt64(dtProduct.Rows[0]["PkLotId"].ToString());
                    var drdetail = model.TranDetails.Where(x => x.FkProductId == FkProductId && x.FkLotId == FkLotId && x.ModeForm != 2
                    && model.ExtProperties.DocumentType != "C"
                    )
                        .FirstOrDefault();
                    if (drdetail != null)
                    {
                        if (drdetail.CodingScheme == "Unique" && model.UniqIdDetails.ToList().Where(x => x.Barcode == Barcode).ToList().Count > 0)
                        { }
                        else
                        {
                            int rowIndex = model.TranDetails.FindIndex(a => a.FkProductId == FkProductId && a.FkLotId == FkLotId && a.ModeForm != 2);
                            SetProduct(model, drdetail, dtProduct);


                            model.TranDetails[rowIndex].Qty += 1;
                            CalculateExe(model, model.TranDetails[rowIndex]);
                            // Check Product UNique/Lot/PRoduct
                            var _check = model.UniqIdDetails.ToList().Where(x => x.Barcode == Barcode).FirstOrDefault();
                            if (_check == null)
                            {
                                model.UniqIdDetails.Add(new BarcodeUniqVM() { SrNo = model.TranDetails[rowIndex].SrNo, Barcode = Barcode });
                            }
                        }
                    }
                    else
                    {
                        var detail = new TranDetails();
                        TranDetailDefault(model, detail);
                        SetProduct(model, detail, dtProduct);
                        detail.Barcode = "Barcode";
                        detail.BarcodeTest = Barcode;

                        CalculateExe(model, detail);
                        model.TranDetails.Add(detail);

                        // Check Product UNique/Lot/PRoduct
                        model.UniqIdDetails.Add(new BarcodeUniqVM() { SrNo = detail.SrNo, Barcode = Barcode });

                    }
                }
                else { throw new Exception("Data Not Found"); }
            }
            if (isCalGridTotal)
            {
                SetGridTotal(model);
                SetPaymentDetail(model);
            }
            return model;
        }

        public object ProductTouch(TransactionModel model, long PkProductId)
        {
            var detail = new TranDetails();

            model.TranDetails.Add(detail);
            detail.FkProductId = PkProductId;
            TranDetailDefault(model, detail);
            GetSetProduct(model, detail, "", 0, model.FKOrderID, model.FKOrderSrID);
            CalculateExe(model, detail);
            SetGridTotal(model);
            SetPaymentDetail(model);
            return model;
        }

        public object ApplyPromotion(TransactionModel objmodel)
        {
            if (objmodel.ExtProperties.TranType == "S" && objmodel.IsTranChange && objmodel.TranAlias != "LINV" && objmodel.TranAlias != "LORD")
            {
                setPromotion(objmodel);

                SetGridTotal(objmodel);
                setPromotion_InvoiceValue(objmodel);

                // objmodel.IsTranChange = false;
            }
            return objmodel;
        }

        #region Promotion
        public void removePromotion(TransactionModel model)
        {
            model.TranDetails.Where(x => x.LinkSrNo > 0 || x.PromotionType == "IVFP").ToList().ForEach(x => x.ModeForm = 2);
            model.TranDetails.Where(x => x.PromotionType == "PFPT" || x.PromotionType == "CFPT" || x.PromotionType == "BFPT" || x.PromotionType == "CFRT" || x.PromotionType == "PFRT" || x.PromotionType == "CFQT" || x.PromotionType == "PFQT").ToList().ForEach(x => { x.PromotionType = "";x.FkPromotionId = null;x.PromotionName = ""; });
            model.TranDetails.Where(x => x.PromotionType == "PFQT" || x.PromotionType == "CFQT" || x.PromotionType == "BFQT").ToList().ForEach(x => { x.FreeQty = 0; x.PromotionType = ""; x.FkPromotionId = null; x.PromotionName = ""; });
            model.TranDetails.Where(x => x.PromotionType == "PTDT" || x.PromotionType == "CTDT" || x.PromotionType == "BTDT" || x.PromotionType == "XOXT" || x.PromotionType == "CFRT" || x.PromotionType == "PFRT" || x.PromotionType == "CFQT" || x.PromotionType == "PFQT").ToList().ForEach(x => { x.TradeDisc = 0; x.TradeDiscAmt = 0; x.PromotionType = ""; x.FkPromotionId = null; x.PromotionName = ""; });
            model.TranDetails.Where(x => !string.IsNullOrEmpty(x.PromotionType)).ToList().ForEach(x => { x.PromotionType = ""; x.FkPromotionId = null; x.PromotionName = ""; });
            //model.TranDetails.Where(x => !string.IsNullOrEmpty(x.PromotionType)).ToList().ForEach(x => x.PromotionType = "");
        }
        public void setPromotion(TransactionModel model)
        {
            removePromotion(model);
            if (model.TranDetails != null)
            {
                DateTime Cdt = DateTime.Now;

                var _entityPromotion = (from cou in __dbContext.TblPromotionMas
                                        join _locationLnk in __dbContext.TblPromotionLocationLnk on cou.PkPromotionId equals (Int64?)_locationLnk.FkPromotionId into _locationmpLnk
                                        from locationLnk in _locationmpLnk.DefaultIfEmpty()
                                            //join _location in __dbContext.TblLocationMas on cou.FKLocationId equals (Int64?)_location.PkLocationID into _locationmp
                                            //from location in _locationmp.DefaultIfEmpty()
                                        where cou.PromotionDuring == ((model.TranAlias == "PINV" || model.TranAlias == "PORD") ? "Purchase" : "Sales")
                                        && (cou.FkCustomerId == null || cou.FkCustomerId == model.FkPartyId)
                                        && (locationLnk.FKLocationId == null || locationLnk.FKLocationId == model.FKLocationID)
                                        && ((cou.PromotionFromDt == null && cou.PromotionToDt == null) || ((cou.PromotionFromDt != null ? cou.PromotionFromDt.Value : Cdt).Date <= Cdt.Date && (cou.PromotionToDt != null ? cou.PromotionToDt.Value : Cdt).Date >= Cdt.Date))
                                        // && ((string.IsNullOrEmpty(cou.PromotionFromTime) && string.IsNullOrEmpty(cou.PromotionToTime)) || ((!string.IsNullOrEmpty(cou.PromotionFromTime) ? TimeSpan.Parse(cou.PromotionFromTime) : Cdt.TimeOfDay) <= Cdt.TimeOfDay && (!string.IsNullOrEmpty(cou.PromotionToTime) ? TimeSpan.Parse(cou.PromotionToTime) : Cdt.TimeOfDay) >= Cdt.TimeOfDay))
                                       && cou.PromotionApplyOn != "Invoice Value"
                                        orderby cou.SequenceNo ascending
                                        select new
                                        {
                                            cou,
                                        }).ToList();

                //var _entityPromotion = __dbContext.TblPromotionMas.Where(x => x.PromotionDuring == "Sales"
                //&& (x.FkCustomerId == null || x.FkCustomerId == model.FkPartyId)
                //&& (x.FKLocationId == null || x.FKLocationId == model.FKLocationID)
                //&& ((x.PromotionFromDt == null && x.PromotionToDt == null) || ((x.PromotionFromDt != null ? x.PromotionFromDt.Value : Cdt).Date <= Cdt.Date && (x.PromotionToDt != null ? x.PromotionToDt.Value : Cdt).Date >= Cdt.Date))
                //).ToList().Where(x => ((string.IsNullOrEmpty(x.PromotionFromTime) && string.IsNullOrEmpty(x.PromotionToTime)) || ((!string.IsNullOrEmpty(x.PromotionFromTime) ? TimeSpan.Parse(x.PromotionFromTime) : Cdt.TimeOfDay) <= Cdt.TimeOfDay && (!string.IsNullOrEmpty(x.PromotionToTime) ? TimeSpan.Parse(x.PromotionToTime) : Cdt.TimeOfDay) >= Cdt.TimeOfDay))
                //).OrderBy(x=>x.SequenceNo).ToList();

                if (_entityPromotion.Count > 0)
                {
                    foreach (var _itemPromo in _entityPromotion)
                    {
                        var itemPromo = _itemPromo.cou;
                        if (itemPromo.PromotionApplyOn == "Product" || itemPromo.PromotionApplyOn == "Category" || itemPromo.PromotionApplyOn == "Brand")
                        {
                            var _lst = new List<TranDetails>();
                            if (itemPromo.PromotionApplyOn == "Product")
                            {
                                //    _lst = model.TranDetails.Where(x => x.Qty >= itemPromo.PromotionApplyQty && x.FkProductId == itemPromo.FKProdID && (x.LinkSrNo <= 0 || x.LinkSrNo == null) && string.IsNullOrEmpty(x.PromotionType)).ToList();
                                _lst = (from cou in model.TranDetails
                                        join promotionLnk in __dbContext.TblPromotionLnk on cou.FkProductId equals (Int64?)promotionLnk.FkLinkId //into _locationmpLnk
                                        where promotionLnk.FkPromotionId == itemPromo.PkPromotionId && cou.ModeForm != 2
                                        && cou.Qty >= itemPromo.PromotionApplyQty
                                         && (cou.LinkSrNo <= 0 || cou.LinkSrNo == null) && string.IsNullOrEmpty(cou.PromotionType)
                                        orderby cou.Rate descending
                                        select cou).ToList();
                            }
                            else if (itemPromo.PromotionApplyOn == "Category")
                            {
                                //     _lst = model.TranDetails.Where(x => x.Qty >= itemPromo.PromotionApplyQty && x.FKProdCatgId == itemPromo.FkProdCatgId && (x.LinkSrNo <= 0 || x.LinkSrNo == null) && string.IsNullOrEmpty(x.PromotionType)).ToList();
                                _lst = (from cou in model.TranDetails
                                        join promotionLnk in __dbContext.TblPromotionLnk on cou.FKProdCatgId equals (Int64?)promotionLnk.FkLinkId //into _locationmpLnk
                                        where promotionLnk.FkPromotionId == itemPromo.PkPromotionId && cou.ModeForm != 2
                                        && cou.Qty >= itemPromo.PromotionApplyQty
                                         && (cou.LinkSrNo <= 0 || cou.LinkSrNo == null) && string.IsNullOrEmpty(cou.PromotionType)
                                        orderby cou.Rate descending
                                        select cou).ToList();
                            }
                            else if (itemPromo.PromotionApplyOn == "Brand")
                            {
                                //   _lst = model.TranDetails.Where(x => x.Qty >= itemPromo.PromotionApplyQty && x.FkBrandId == itemPromo.FkBrandId && (x.LinkSrNo <= 0 || x.LinkSrNo == null) && string.IsNullOrEmpty(x.PromotionType)).ToList();
                                _lst = (from cou in model.TranDetails
                                        join promotionLnk in __dbContext.TblPromotionLnk on cou.FkBrandId equals (Int64?)promotionLnk.FkLinkId //into _locationmpLnk
                                        where promotionLnk.FkPromotionId == itemPromo.PkPromotionId && cou.ModeForm != 2
                                        && cou.Qty >= itemPromo.PromotionApplyQty
                                         && (cou.LinkSrNo <= 0 || cou.LinkSrNo == null) && string.IsNullOrEmpty(cou.PromotionType)
                                        orderby cou.Rate descending
                                        select cou).ToList();
                            }
                            if (_lst.Count > 0)
                            {
                                foreach (var item in _lst)
                                {
                                    if (itemPromo.Promotion == "Free Product" && itemPromo.FkPromotionProdId > 0 && itemPromo.PromotionQty > 0)
                                    {
                                        decimal qty = Decimal.Truncate(item.Qty / (decimal)itemPromo.PromotionApplyQty) * (decimal)itemPromo.PromotionQty;
                                        DataTable dtProduct = GetProduct("", model.FKLocationID, (long)itemPromo.FkPromotionProdId, 0, "", false, model.TranAlias, model.FkPartyId, model.FKOrderID, model.FKOrderSrID);
                                        if (dtProduct.Rows.Count > 0)
                                        {
                                            var detail = new TranDetails();
                                            TranDetailDefault(model, detail);
                                            SetProduct(model, detail, dtProduct);
                                            detail.LinkSrNo = item.SrNo;
                                            detail.Qty = 0;
                                            detail.FreeQty = qty;
                                            CalculateExe(model, detail);
                                            model.TranDetails.Add(detail);

                                            if (itemPromo.PromotionApplyOn == "Product") { item.PromotionType = "PFPT"; }
                                            else if (itemPromo.PromotionApplyOn == "Category") { item.PromotionType = "CFPT"; }
                                            else if (itemPromo.PromotionApplyOn == "Brand") { item.PromotionType = "BFPT"; }
                                          
                                            item.FkPromotionId = itemPromo.PkPromotionId;
                                            item.PromotionName = itemPromo.PromotionName;
                                            //break;
                                        }
                                    }
                                    else if (itemPromo.Promotion == "Free Qty" && itemPromo.PromotionQty > 0)
                                    {
                                        decimal qty = Decimal.Truncate(item.Qty / (decimal)itemPromo.PromotionApplyQty) * (decimal)itemPromo.PromotionQty;
                                        item.FreeQty = (decimal)itemPromo.PromotionQty;
                                        if (itemPromo.PromotionApplyOn == "Product") { item.PromotionType = "PFQT"; }
                                        else if (itemPromo.PromotionApplyOn == "Category") { item.PromotionType = "CFQT"; }
                                        else if (itemPromo.PromotionApplyOn == "Brand") { item.PromotionType = "BFQT"; }
                                       
                                        item.FkPromotionId = itemPromo.PkPromotionId;
                                        item.PromotionName = itemPromo.PromotionName;

                                        //  CalculateExe(item);
                                    }
                                    else if (itemPromo.Promotion == "Trade Discount" && itemPromo.PromotionAmt > 0)
                                    {
                                        item.TradeDisc = (decimal)itemPromo.PromotionAmt;
                                        item.TradeDiscAmt = 0;
                                        CalculateExe(model, item);
                                        if (itemPromo.PromotionApplyOn == "Product") { item.PromotionType = "PTDT"; }
                                        else if (itemPromo.PromotionApplyOn == "Category") { item.PromotionType = "CTDT"; }
                                        else if (itemPromo.PromotionApplyOn == "Brand") { item.PromotionType = "BTDT"; }

                                        item.FkPromotionId = itemPromo.PkPromotionId;
                                        item.PromotionName = itemPromo.PromotionName;

                                    }
                                    else if (itemPromo.Promotion == "Flat Rate" && item.Rate >= itemPromo.PromotionApplyAmt && itemPromo.PromotionApplyAmt2 >= item.Rate)
                                    {
                                        var dic = item.Rate - itemPromo.PromotionAmt;
                                        if (dic > 0)
                                        {
                                            item.TradeDisc = (decimal)(dic / item.Rate) * 100;
                                        }
                                        else
                                        {
                                            item.TradeDisc = 100;
                                        }
                                        item.TradeDiscAmt = 0;
                                        CalculateExe(model, item);
                                        if (itemPromo.PromotionApplyOn == "Product") { item.PromotionType = "PFRT"; }
                                        else if (itemPromo.PromotionApplyOn == "Category") { item.PromotionType = "CFRT"; }

                                        item.FkPromotionId = itemPromo.PkPromotionId;
                                        item.PromotionName = itemPromo.PromotionName;

                                    }
                                    else if (itemPromo.Promotion == "Flat Qty" && item.Qty >= itemPromo.PromotionApplyQty && itemPromo.PromotionApplyQty2 >= item.Qty)
                                    {
                                        var dic = item.Rate - itemPromo.PromotionAmt;
                                        if (dic > 0)
                                        {
                                            item.TradeDisc = (decimal)(dic / item.Rate) * 100;
                                        }
                                        else
                                        {
                                            item.TradeDisc = 100;
                                        }
                                        item.TradeDiscAmt = 0;
                                        CalculateExe(model, item);
                                        if (itemPromo.PromotionApplyOn == "Product") { item.PromotionType = "PFQT"; }
                                        else if (itemPromo.PromotionApplyOn == "Category") { item.PromotionType = "CFQT"; }

                                        item.FkPromotionId = itemPromo.PkPromotionId;
                                        item.PromotionName = itemPromo.PromotionName;

                                    }
                                }
                            }

                        }
                        else if (itemPromo.PromotionApplyOn == "XonX" && itemPromo.PromotionQty > 0)
                        {

                            var _lstForApplyPromoCommon = (from cou in model.TranDetails
                                                           join promotionLnk in __dbContext.TblPromotionLnk on cou.FKProdCatgId equals (Int64?)promotionLnk.FkLinkId //into _locationmpLnk
                                                           where promotionLnk.FkPromotionId == itemPromo.PkPromotionId && cou.Qty > 0 && cou.ModeForm != 2
                                                        && (cou.LinkSrNo <= 0 || cou.LinkSrNo == null) && string.IsNullOrEmpty(cou.PromotionType)
                                                           orderby cou.Rate descending
                                                           select new
                                                           {
                                                               cou,
                                                           }).ToList();
                            //  var disc = 100 / (decimal)itemPromo.PromotionQty;
                            int PromotionQty = (int)Decimal.Truncate(Convert.ToDecimal(itemPromo.PromotionQty) + Convert.ToDecimal(itemPromo.PromotionApplyQty));
                            int l = (int)Decimal.Truncate(Convert.ToDecimal(_lstForApplyPromoCommon.Count) / PromotionQty);
                            if (l > 0)
                            {
                                for (int i = 0; i < l; i++)
                                {
                                    //int PromotionQty = Convert.ToInt32(itemPromo.PromotionQty);
                                    var _lstForApplyPromo = _lstForApplyPromoCommon.Skip(i * PromotionQty).Take(PromotionQty).Select(x => x.cou).ToList();
                                    decimal maxRate = Convert.ToDecimal(_lstForApplyPromo[0].Rate);
                                    decimal disc = (decimal)100 - ((maxRate / (decimal)(_lstForApplyPromo.Sum(x => x.Rate))) * 100);
                                    foreach (var _ap in _lstForApplyPromo)
                                    {
                                        _ap.TradeDisc = disc;
                                        _ap.TradeDiscAmt = 0;
                                        _ap.PromotionType = "XOXT";
                                        CalculateExe(model, (TranDetails)_ap);
                                        _ap.FkPromotionId = itemPromo.PkPromotionId;
                                        _ap.PromotionName = itemPromo.PromotionName;

                                    }
                                }
                            }

                        }

                    }
                }

            }
        }
        public void setPromotion_InvoiceValue(TransactionModel model)
        {
            if (model.TranDetails != null)
            {
                DateTime Cdt = DateTime.Now;

                var _entityPromotion = (from cou in __dbContext.TblPromotionMas
                                        join _locationLnk in __dbContext.TblPromotionLocationLnk on cou.PkPromotionId equals (Int64?)_locationLnk.FkPromotionId into _locationmpLnk
                                        from locationLnk in _locationmpLnk.DefaultIfEmpty()
                                            //join _location in __dbContext.TblLocationMas on cou.FKLocationId equals (Int64?)_location.PkLocationID into _locationmp
                                            //from location in _locationmp.DefaultIfEmpty()
                                        where cou.PromotionDuring == ((model.TranAlias == "PINV" || model.TranAlias == "PORD") ? "Purchase" : "Sales")
                                        && (cou.FkCustomerId == null || cou.FkCustomerId == model.FkPartyId)
                                        && (locationLnk.FKLocationId == null || locationLnk.FKLocationId == model.FKLocationID)
                                        && ((cou.PromotionFromDt == null && cou.PromotionToDt == null) || ((cou.PromotionFromDt != null ? cou.PromotionFromDt.Value : Cdt).Date <= Cdt.Date && (cou.PromotionToDt != null ? cou.PromotionToDt.Value : Cdt).Date >= Cdt.Date))
                                        // && ((string.IsNullOrEmpty(cou.PromotionFromTime) && string.IsNullOrEmpty(cou.PromotionToTime)) || ((!string.IsNullOrEmpty(cou.PromotionFromTime) ? TimeSpan.Parse(cou.PromotionFromTime) : Cdt.TimeOfDay) <= Cdt.TimeOfDay && (!string.IsNullOrEmpty(cou.PromotionToTime) ? TimeSpan.Parse(cou.PromotionToTime) : Cdt.TimeOfDay) >= Cdt.TimeOfDay))
                                       && cou.PromotionApplyOn == "Invoice Value" && cou.PromotionAmt <= model.NetAmt
                                        orderby cou.SequenceNo ascending
                                        select new
                                        {
                                            cou,
                                        }).FirstOrDefault();

                //var _entityPromotion = __dbContext.TblPromotionMas.Where(x => x.PromotionDuring == "Sales"
                //&& (x.FkCustomerId == null || x.FkCustomerId == model.FkPartyId)
                //&& (x.FKLocationId == null || x.FKLocationId == model.FKLocationID)
                //&& ((x.PromotionFromDt == null && x.PromotionToDt == null) || ((x.PromotionFromDt != null ? x.PromotionFromDt.Value : Cdt).Date <= Cdt.Date && (x.PromotionToDt != null ? x.PromotionToDt.Value : Cdt).Date >= Cdt.Date))
                //).ToList().Where(x => ((string.IsNullOrEmpty(x.PromotionFromTime) && string.IsNullOrEmpty(x.PromotionToTime)) || ((!string.IsNullOrEmpty(x.PromotionFromTime) ? TimeSpan.Parse(x.PromotionFromTime) : Cdt.TimeOfDay) <= Cdt.TimeOfDay && (!string.IsNullOrEmpty(x.PromotionToTime) ? TimeSpan.Parse(x.PromotionToTime) : Cdt.TimeOfDay) >= Cdt.TimeOfDay))
                //).OrderBy(x=>x.SequenceNo).ToList();

                if (_entityPromotion != null)
                {
                    var itemPromo = _entityPromotion.cou;
                    if (itemPromo != null)
                    {
                        int l = (int)Decimal.Truncate(Convert.ToDecimal(model.NetAmt) / (decimal)itemPromo.PromotionApplyAmt);

                        // var itemPromo = _itemPromo.cou;
                        // if (itemPromo.PromotionApplyOn == "Product" || itemPromo.PromotionApplyOn == "Category" || itemPromo.PromotionApplyOn == "Brand")
                        // {
                        if (l > 0 && (itemPromo.Promotion == "Free Product") && itemPromo.FkPromotionProdId > 0 && itemPromo.PromotionQty > 0)
                        {
                            decimal qty = (decimal)itemPromo.PromotionQty * l;

                            DataTable dtProduct = GetProduct("", model.FKLocationID, (long)itemPromo.FkPromotionProdId, 0, "", false, model.TranAlias, model.FkPartyId, model.FKOrderID, model.FKOrderSrID);
                            if (dtProduct.Rows.Count > 0)
                            {
                                var detail = new TranDetails();
                                TranDetailDefault(model, detail);
                                SetProduct(model, detail, dtProduct);
                                detail.Qty = 0;
                                detail.FreeQty = qty;
                                detail.Barcode = "Barcode";
                                detail.PromotionType = "IVFP";
                                CalculateExe(model, detail);
                                model.TranDetails.Add(detail);

                                //break;
                            }
                        }
                        else if (itemPromo.Promotion == "Free Point" && itemPromo.PromotionAmt > 0)
                        {
                            model.FreePoint = (decimal)itemPromo.PromotionAmt * l;
                        }
                        // } 
                    }
                }
            }
        }

        #endregion

        public void setReturnProduct(TransactionModel model, TranDetails detail)
        {
            var _invoice = (from saledtl in __dbContext.TblSalesInvoicedtl
                            join sale in __dbContext.TblSalesInvoicetrn on saledtl.FkId equals sale.PkId
                            join series in __dbContext.TblSeriesMas on sale.FKSeriesId equals series.PkSeriesId
                            join cou in __dbContext.TblProductMas on saledtl.FkProductId equals cou.PkProductId
                            where sale.FkPartyId == model.FkPartyId
                            //   && saledtl.SrNo == detail.InvoiceSrNo
                            && sale.FKSeriesId == detail.FKInvoiceSrID
                            && saledtl.FkId == detail.FKInvoiceID
                            && saledtl.FkProductId == detail.FkProductId
                            select (new
                            {
                                sale,
                                saledtl,
                                series,
                                cou
                            }
                           )).FirstOrDefault();


            if (_invoice != null)
            {
                TranDetailDefault(model, detail);
                detail.FkProductId = _invoice.saledtl.FkProductId;
                detail.FkLotId = _invoice.saledtl.FkLotId;
                detail.Color = _invoice.saledtl.Color;
                detail.Batch = _invoice.saledtl.Batch;
                detail.MRP = _invoice.saledtl.MRP != null ? Convert.ToDecimal(_invoice.saledtl.MRP) : 0;
                detail.SaleRate = _invoice.saledtl.SaleRate != null ? Convert.ToDecimal(_invoice.saledtl.SaleRate) : 0;
                detail.TradeRate = 0;
                detail.DistributionRate = 0;
                detail.Rate = _invoice.saledtl.Rate != null ? Convert.ToDecimal(_invoice.saledtl.Rate) : 0;
                detail.InvoiceDate = _invoice.sale.EntryDate.Date;
                detail.FKInvoiceID_Text = _invoice.series.Series + _invoice.sale.EntryNo;
                detail.FKInvoiceSrID = _invoice.sale.FKSeriesId;
            }
        }

        public void setInvoiceinfo(TransactionModel model, TranDetails detail)
        {

            var _invoice = (from sale in __dbContext.TblSalesInvoicetrn
                            join c in __dbContext.TblSeriesMas on sale.FKSeriesId equals c.PkSeriesId
                            where sale.FkPartyId == model.FkPartyId && sale.PkId == detail.FKInvoiceID
                            orderby sale.EntryDate
                            select (new
                            {
                                FKInvoiceID = sale.PkId,
                                Inum = c.Series + sale.EntryNo,
                                sale.EntryDate,
                                sale.FKSeriesId,
                            }
                           )).FirstOrDefault();

            if (_invoice != null)
            {
                TranDetailDefault(model, detail);
                detail.InvoiceDate = _invoice.EntryDate.Date;
                detail.FKInvoiceID_Text = _invoice.Inum;
                detail.FKInvoiceSrID = _invoice.FKSeriesId;
            }
        }
        public object AutoFillLastRecord(TransactionModel model)
        {
            if (model.TranDetails != null)
            {
                var tranDetail = model.TranDetails.ToList().OrderByDescending(x => x.SrNo).FirstOrDefault();
                var detail = new TranDetails();
                model.TranDetails.Add(detail);
                TranDetailDefault(model, detail);
                detail.FkProductId = tranDetail.FkProductId;
                detail.Qty = tranDetail.Qty;
                detail.FkLotId = tranDetail.FkLotId;
                detail.Color = tranDetail.Color;
                detail.Batch = tranDetail.Batch;

                detail.FkBrandId = tranDetail.FkBrandId;
                detail.FKProdCatgId = tranDetail.FKProdCatgId;
                detail.Product = tranDetail.Product;
                detail.CodingScheme = tranDetail.CodingScheme;

                detail.MRP = tranDetail.MRP;
                detail.SaleRate = tranDetail.SaleRate;
                detail.TradeRate = tranDetail.TradeRate;
                detail.DistributionRate = tranDetail.DistributionRate;

                detail.Rate = tranDetail.Rate;
                detail.TradeDisc = tranDetail.TradeDisc;
                //detail.FKOrderID = tranDetail.FKOrderID;
                //detail.FKOrderSrID = tranDetail.FKOrderSrID;
                //detail.OrderSrNo = tranDetail.OrderSrNo; 
                //if (model.TranAlias == "PORD" || model.TranAlias == "PINV")
                //{
                //    detail.FkLotId = 0;
                //}


                CalculateExe(model, detail);
                SetGridTotal(model);
                SetPaymentDetail(model);
            }

            return model;
        }

        public void CalculateExe(TransactionModel model)
        {
            foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            {
                item.FKLocationID = model.FKLocationID;

                CalculateExe(model, item);
            }

            // model.TranDetails = model.TranDetails.Where(x => x.FkProductId > 0).ToList();
        }

        public void CalculateExe(TransactionModel model, TranDetails item)
        {

            decimal GrossAmt = item.Rate * item.Qty;
            if (item.TradeDisc > 0)
            {
                item.TradeDiscAmt = Math.Round(GrossAmt * (item.TradeDisc / 100), 2);
            }
            else if (item.TradeDiscAmt > 0)
            {
                item.TradeDisc = Math.Round((item.TradeDiscAmt / GrossAmt) * 100, 2);
            }

            item.GrossAmt = Math.Round((GrossAmt - item.TradeDiscAmt), 2);
            item.GstRate = (item.GrossAmt / item.Qty) < 1000 ? 5 : 12;
            if (model.TaxType == 'E')
            {
                item.TaxableAmt = item.GrossAmt;
            }
            else
            {
                item.TaxableAmt = Math.Round(((item.GrossAmt * 100) / (100 + item.GstRate)), 2);
            }
            item.GstAmt = Math.Round((item.TaxableAmt * item.GstRate) / 100, 2);
            item.NetAmt = Math.Round(item.TaxableAmt + item.GstAmt, 2);
        }

        public void SetGridTotal(TransactionModel model)
        {
            model.GrossAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.TaxableAmt), 2);// + Math.Round(model.TranReturnDetails.Where(x => x.ModeForm != 2).Sum(x => x.TaxableAmt), 2);
            model.TaxAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.GstAmt), 2);//+ Math.Round(model.TranReturnDetails.Where(x => x.ModeForm != 2).Sum(x => x.GstAmt), 2);
            model.CashDiscountAmt = 0;
            if (model.CashDiscType == "R" && model.CashDiscount > 0 && model.CashDiscount <= model.GrossAmt)
            {
                model.CashDiscountAmt = Math.Round(model.CashDiscount, 2);
            }
            else if (model.CashDiscType == "P" && model.CashDiscount > 0 && model.CashDiscount <= 100)
            {
                model.CashDiscountAmt = Math.Round((model.GrossAmt * model.CashDiscount / 100), 2);
            }
            else
            {
                model.CashDiscount = 0;
            }
            model.TradeDiscAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.TradeDiscAmt), 2);// + Math.Round(model.TranReturnDetails.Where(x => x.ModeForm != 2).Sum(x => x.TradeDiscAmt), 2);
            model.TotalDiscount = model.CashDiscountAmt + model.TradeDiscAmt;
            decimal NetAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.NetAmt), 2);// + Math.Round(model.TranReturnDetails.Where(x => x.ModeForm != 2).Sum(x => x.NetAmt), 2);
            model.NetAmt = Math.Round((NetAmt - model.CashDiscountAmt) + model.Shipping + model.OtherCharge - (model.RoundOfDiff >= 1 ? model.RoundOfDiff : 0), 2);
            if (model.RoundOfDiff < 1)
            {
                model.RoundOfDiff = model.NetAmt - Math.Floor(model.NetAmt);
                model.NetAmt = Math.Round(model.NetAmt - model.RoundOfDiff, 2);
            }
            model.NetAmtIn = Math.Round(model.TranReturnDetails.Where(x => x.ModeForm != 2).Sum(x => x.NetAmt), 2);
            model.NetAmtOut = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.NetAmt), 2);

        }

        public void SetPaymentDetail(TransactionModel model)
        {
            decimal _remAmt = model.NetAmt;
            if (model.CreditCard || model.Cheque || model.Credit || model.Cash)
            {
                if (model.CreditCard && model.CreditCardAmt > 0)
                {
                    if ((_remAmt - (decimal)model.CreditCardAmt) < 0)
                    {
                        model.CreditCardAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.CreditCardAmt;
                }
                else
                {
                    model.CreditCard = false;
                    model.CreditCardAmt = 0;
                    model.CreditCardNo = "";
                    model.CreditCardDate = null;
                    model.FKBankCreditCardID = null;
                }


                if (model.Cheque && model.ChequeAmt > 0 && _remAmt > 0)
                {
                    if ((_remAmt - (decimal)model.ChequeAmt) < 0)
                    {
                        model.ChequeAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.ChequeAmt;
                }
                else
                {
                    model.Cheque = false;
                    model.ChequeAmt = 0;
                    model.ChequeNo = "";
                    model.ChequeDate = null;
                    model.FKBankChequeID = null;
                }

                if (model.Credit && model.CreditAmt > 0 && _remAmt > 0)
                {
                    if ((_remAmt - (decimal)model.CreditAmt) < 0)
                    {
                        model.CreditAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.CreditAmt;
                }
                else
                {
                    model.Credit = false;
                    model.CreditAmt = 0;
                    model.CreditDate = null;
                }

                if (model.Cash && model.CashAmt > 0 && _remAmt > 0)
                {
                    if ((_remAmt - (decimal)model.CashAmt) < 0)
                    {
                        model.CashAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.CashAmt;
                }
                else
                {
                    model.Cash = false;
                    model.CashAmt = 0;
                }

                if (_remAmt > 0)
                {
                    model.Cash = true;
                    model.CashAmt += _remAmt;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.PaymentModeDefault))
                {
                    if (model.PaymentModeDefault == "Cash")
                    {
                        model.Cash = true;
                        model.CashAmt = _remAmt;
                    }
                    else if (model.PaymentModeDefault == "Credit")
                    {
                        model.Credit = true;
                        model.CreditAmt = _remAmt;
                    }
                    else if (model.PaymentModeDefault == "Cheque")
                    {
                        model.Cheque = true;
                        model.ChequeAmt = _remAmt;
                    }
                    else if (model.PaymentModeDefault == "Card")
                    {
                        model.CreditCard = true;
                        model.CreditCardAmt = _remAmt;
                    }

                }
                else
                {
                    model.Cash = true;
                    model.CashAmt = _remAmt;
                }
            }
        }


        public object PaymentDetail(TransactionModel model)
        {
            SetPaymentDetail(model);
            return model;
        }

        public object FooterChange(TransactionModel model, string fieldName)
        {
            //if (fieldName == "CashDiscType" || fieldName == "CashDiscount")
            //{
            //    if (model.CashDiscType == "LIR")
            //    {
            //        foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            //        {
            //            item.TradeDisc = 0;
            //            item.TradeDiscAmt = model.CashDiscount;
            //        }
            //        model.CashDiscount = 0;
            //        model.CashDiscountAmt = 0;
            //        CalculateExe(model);
            //    }
            //    else if (model.CashDiscType == "LIP")
            //    {
            //        foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            //        {
            //            item.TradeDisc = model.CashDiscount;
            //            item.TradeDiscAmt = 0;
            //        }
            //        model.CashDiscount = 0;
            //        model.CashDiscountAmt = 0;
            //        CalculateExe(model);
            //    }
            //    else if (model.CashDiscType == "F")
            //    {
            //        foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            //        {
            //            item.Rate = model.CashDiscount;
            //        }
            //        model.CashDiscount = 0;
            //        model.CashDiscountAmt = 0;
            //        CalculateExe(model);
            //    }


            //}

            SetGridTotal(model);
            SetPaymentDetail(model);
            return model;
        }

        public List<PartyModel> PartyList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "")
        {
            if (TranAlias == "LINV" || TranAlias == "LORD")
            {
                LocationRepository rep = new LocationRepository(__dbContext, _contextAccessor);
                var lst = rep.GetList(pageSize, pageNo, search).ToList()
                    .Select(cou => new PartyModel()
                    {
                        PKID = cou.PKID,
                        Email = cou.Email,
                        Mobile = cou.Phone1,
                        Address = cou.Address,
                        StateName = cou.State,
                        FkCityId = cou.FkCityId,
                        //  City = city.CityName, 
                        Pin = cou.Pincode,
                        Name = cou.Location,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.DATE_MODIFIED
                    }).ToList();
                return lst;
            }
            else if (TranAlias == "PINV" || TranAlias == "PORD" || TranAlias == "PJ_O" || TranAlias == "PJ_R" || TranAlias == "PJ_I")
            {
                VendorRepository rep = new VendorRepository(__dbContext, _contextAccessor);
                return rep.GetList(pageSize, pageNo, search);
            }
            else
            {
                CustomerRepository rep = new CustomerRepository(__dbContext, _contextAccessor);
                return rep.GetList(pageSize, pageNo, search);
            }
        }

        public object SetParty(TransactionModel model, long FkPartyId)
        {
            var vendor = new PartyModel();
            if (model.ExtProperties.TranAlias == "LINV" || model.ExtProperties.TranAlias == "LORD")
                vendor = GetLocation(FkPartyId);
            else if (model.ExtProperties.TranAlias == "PINV" || model.ExtProperties.TranAlias == "PORD" || model.ExtProperties.TranAlias == "PJ_O" || model.ExtProperties.TranAlias == "PJ_R" || model.ExtProperties.TranAlias == "PJ_I")
                vendor = GetVendor(FkPartyId);
            else
                vendor = GetCustomer(FkPartyId, "");

            if (vendor != null)
            {
                model.PartyAddress = vendor.Address == null ? "" : vendor.Address.ToString();
                model.PartyName = vendor.Name;
                model.PartyGSTN = vendor.Gstno == null ? "" : vendor.Gstno.ToString();
                model.PartyMobile = vendor.Mobile;
                model.PartyStateName = vendor.StateName;
                model.PartyCredit = 0;
                model.FkPartyId = FkPartyId;
                model.FKPostAccID = vendor.FkAccountID;
                model.Account = vendor.AccountName;
            }
            model.IsTranChange = true;

            return model;
        }
        public object GetParty(TransactionModel model, string Mobile)
        {
            var vendor = new PartyModel();
            if (model.ExtProperties.TranAlias == "LINV" || model.ExtProperties.TranAlias == "LORD")
                vendor = GetLocation(0);
            else if (model.ExtProperties.TranAlias == "PINV" || model.ExtProperties.TranAlias == "PORD" || model.ExtProperties.TranAlias == "PJ_O" || model.ExtProperties.TranAlias == "PJ_R" || model.ExtProperties.TranAlias == "PJ_I")
                vendor = GetVendor(0);
            else
                vendor = GetCustomer(0, Mobile);

            if (vendor != null)
            {
                model.PartyAddress = vendor.Address == null ? "" : vendor.Address.ToString();
                model.PartyName = vendor.Name;
                model.PartyGSTN = vendor.Gstno == null ? "" : vendor.Gstno.ToString();
                model.PartyMobile = vendor.Mobile;
                model.PartyStateName = vendor.StateName;
                model.PartyCredit = 0;
                model.FkPartyId = vendor.PKID;
                model.FKPostAccID = vendor.FkAccountID;
                model.Account = vendor.Name;
            }
            model.IsTranChange = true;

            return model;
        }

        public PartyModel? GetVendor(long PkVendorId)
        {
            PartyModel? data = (from cou in __dbContext.TblVendorMas
                                where cou.PkVendorId == PkVendorId
                                select (new PartyModel
                                {
                                    PKID = cou.PkVendorId,
                                    Name = cou.Name,
                                    Mobile = cou.Mobile,
                                    Address = cou.Address,
                                    Gstno = cou.Gstno,
                                    StateName = cou.StateName,
                                    FkAccountID = cou.FkAccountID,
                                    AccountName = cou.FKAccount.Account,
                                }
                               )).FirstOrDefault();
            return data;
        }
        public PartyModel? GetCustomer(long PkId, string Mobile)
        {
            PartyModel? data = (from cou in __dbContext.TblCustomerMas
                                where (cou.PkCustomerId == PkId || cou.Mobile == Mobile)
                                select (new PartyModel
                                {
                                    PKID = cou.PkCustomerId,
                                    Name = cou.Name,
                                    Mobile = cou.Mobile,
                                    Address = cou.Address,
                                    Gstno = cou.Gstno,
                                    StateName = cou.StateName,
                                    FkAccountID = cou.FkAccountID,
                                    AccountName = cou.FKAccount.Account,
                                }
                               )).FirstOrDefault();
            return data;
        }
        public PartyModel? GetLocation(long PkLocationID)
        {
            PartyModel? data = (from cou in __dbContext.TblLocationMas
                                where cou.PkLocationID == PkLocationID
                                select (new PartyModel
                                {
                                    PKID = cou.PkLocationID,
                                    Name = cou.Location,
                                    Mobile = cou.Phone1,
                                    Address = cou.Address,
                                    //Gstno = cou.gst,
                                    StateName = cou.State,
                                    FkAccountID = cou.FkAccountID,
                                }
                               )).FirstOrDefault();
            return data;
        }

        public object SetSeries(TransactionModel model, long FKSeriesId)
        {
            var obj = GetSeries(FKSeriesId);
            if (obj != null)
            {
                model.SeriesName = obj.Series == null ? "" : obj.Series.ToString();
                model.FKLocationID = (long)obj.FKLocationID;
                model.FKSeriesId = FKSeriesId;
                model.BillingRate = obj.BillingRate;
                model.BranchStateName = obj.BranchStateName;
                model.TaxType = obj.TaxType;
                model.PaymentModeDefault = obj.PaymentMode;

            }
            model.IsTranChange = true;

            return model;
        }

        public SeriesModel? GetSeries(long FKSeriesId)
        {
            SeriesModel? data = (from cou in __dbContext.TblSeriesMas
                                 join location in __dbContext.TblLocationMas on cou.FKLocationID equals location.PkLocationID
                                 join branch in __dbContext.TblBranchMas on location.FkBranchID equals branch.PkBranchId
                                 where cou.PkSeriesId == FKSeriesId
                                 select (new SeriesModel
                                 {
                                     PKID = cou.PkSeriesId,
                                     Series = cou.Series,
                                     FKLocationID = cou.FKLocationID,
                                     BillingRate = cou.BillingRate,
                                     TaxType = cou.TaxType,
                                     BranchStateName = branch.State,
                                     PaymentMode = cou.PaymentMode,

                                 }
                                )).FirstOrDefault();
            return data;
        }
        public object SetBankThroughBank(TransactionModel model, long FKBankThroughBankID)
        {
            var obj = GetBank(FKBankThroughBankID);
            if (obj != null)
            {
                model.BankThroughBankName = obj.BankName == null ? "" : obj.BankName.ToString();
                model.FKBankThroughBankID = FKBankThroughBankID;
            }
            model.IsTranChange = true;

            return model;
        }

        public BankModel? GetBank(long BankId)
        {
            BankModel? data = (from cou in __dbContext.TblBankMas
                               where cou.PkBankId == BankId
                               select (new BankModel
                               {
                                   PKID = cou.PkBankId,
                                   BankName = cou.BankName,
                                   IFSCCode = cou.IFSCCode,
                               }
                                )).FirstOrDefault();
            return data;
        }
        public List<ProdLotDtlModel> Get_ProductLotDtlList(int PKProductId, string Batch, string Color)
        {

            List<ProdLotDtlModel> data = (from cou in __dbContext.TblProdLotDtl
                                          where cou.FKProductId == PKProductId
                                          && cou.Batch == (!string.IsNullOrEmpty(Batch) ? Batch : cou.Batch)
                                          && cou.Color == (!string.IsNullOrEmpty(Color) ? Color : cou.Color)
                                          // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                          orderby cou.PkLotId
                                          select (new ProdLotDtlModel
                                          {
                                              PkLotId = cou.PkLotId,
                                              FKProductId = cou.FKProductId,
                                              LotAlias = cou.LotAlias,
                                              Barcode = cou.Barcode,
                                              Batch = cou.Batch,
                                              Color = cou.Color,
                                              MfgDate = cou.MfgDate,
                                              ExpiryDate = cou.ExpiryDate,
                                              ProdConv1 = cou.ProdConv1,
                                              MRP = cou.MRP,
                                              LtExtra = cou.LtExtra,
                                              AddLT = cou.AddLT,
                                              SaleRate = cou.SaleRate,
                                              PurchaseRate = cou.PurchaseRate,
                                              FkmfgGroupId = cou.FkmfgGroupId,
                                              TradeRate = cou.TradeRate,
                                              DistributionRate = cou.DistributionRate,
                                              PurchaseRateUnit = cou.PurchaseRateUnit,
                                              MRPSaleRateUnit = cou.MRPSaleRateUnit,
                                              InTrnId = cou.InTrnId,
                                              InTrnFKSeriesID = cou.InTrnFKSeriesID,
                                              InTrnsno = cou.InTrnsno,
                                              Remarks = cou.Remarks,
                                              //FKUserId = cou.FKUserID,
                                              //
                                              //ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                              //CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                                          }
                                         )).ToList();
            return data;
        }

        public ProdLotDtlModel Get_ProductLotDtl_SingleRecord(long PkLotId)
        {
            ProdLotDtlModel data = new ProdLotDtlModel();
            var entity = (from odr in __dbContext.TblProdLotDtl
                              //join cust in __dbContext.TblCustomerMas on odr.FkPartyId equals cust.PkCustomerId
                          where odr.PkLotId == PkLotId
                          select new { odr }).FirstOrDefault();
            if (entity != null)
            {
                data.PkLotId = entity.odr.PkLotId;
                data.FKProductId = entity.odr.FKProductId;
                data.LotAlias = entity.odr.LotAlias;
                data.Barcode = entity.odr.Barcode;
                data.Batch = entity.odr.Batch;
                data.Color = entity.odr.Color;
                data.MfgDate = entity.odr.MfgDate;
                data.ExpiryDate = entity.odr.ExpiryDate;
                data.ProdConv1 = entity.odr.ProdConv1;
                data.MRP = entity.odr.MRP;
                data.LtExtra = entity.odr.LtExtra;
                data.AddLT = entity.odr.AddLT;
                data.SaleRate = entity.odr.SaleRate;
                data.PurchaseRate = entity.odr.PurchaseRate;
                data.FkmfgGroupId = entity.odr.FkmfgGroupId;
                data.TradeRate = entity.odr.TradeRate;
                data.DistributionRate = entity.odr.DistributionRate;
                data.PurchaseRateUnit = entity.odr.PurchaseRateUnit;
                data.MRPSaleRateUnit = entity.odr.MRPSaleRateUnit;
                data.InTrnId = entity.odr.InTrnId;
                data.InTrnFKSeriesID = entity.odr.InTrnFKSeriesID;
                data.InTrnsno = entity.odr.InTrnsno;
                data.Remarks = entity.odr.Remarks;
                //data.ModifiDate = entity.odr.ModifiedDate.ToString("dd-MMM-yyyy");
                //data.CreateDate = entity.odr.CreationDate.ToString("dd-MMM-yyyy");
                //data.FKCreatedByID = entity.odr.FKCreatedByID;
                //data.FKUserId = entity.odr.FKUserID;
            }
            return data;
        }


        public List<ProductModel> ProductList(int pageSize, int pageNo = 1, string search = "", long FkPartyId = 0, long FkInvoiceId = 0, DateTime? InvoiceDate = null)
        {
            ProductRepository rep = new ProductRepository(__dbContext, _contextAccessor);
            if (!string.IsNullOrWhiteSpace(search) && search.Length > 3)
            {
                if (FkPartyId > 0 || FkInvoiceId > 0)
                    return rep.GetListByPartyId_InSaleInvoice(pageSize, pageNo, search, FkPartyId, FkInvoiceId, InvoiceDate);
                else
                    return rep.GetList(pageSize, pageNo, search);
            }
            else { return new List<ProductModel>(); }
        }

        public List<ProdLotDtlModel> ProductBatchList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProdLotDtlModel> data = (from cou in __dbContext.TblProdLotDtl
                                          where cou.FKProductId == PKProductId
                                          && (EF.Functions.Like(cou.Batch.Trim().ToLower(), Convert.ToString(search) + "%"))
                                          orderby cou.PkLotId
                                          select (new ProdLotDtlModel
                                          {
                                              PkLotId = cou.PkLotId,
                                              Batch = cou.Batch,
                                              Color = cou.Color,
                                              MRP = cou.MRP,
                                              SaleRate = cou.SaleRate,
                                              PurchaseRate = cou.PurchaseRate,
                                              FkmfgGroupId = cou.FkmfgGroupId,
                                              TradeRate = cou.TradeRate,
                                              DistributionRate = cou.DistributionRate,
                                              PurchaseRateUnit = cou.PurchaseRateUnit,
                                              MRPSaleRateUnit = cou.MRPSaleRateUnit,
                                              Remarks = cou.Remarks
                                          }
                                         )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<ProdLotDtlModel> ProductColorList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0, string TranAlias = "", string Batch = "")
        {
            List<ProdLotDtlModel> data = new List<ProdLotDtlModel>();

            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            if (TranAlias == "")
            {
                data = (from cou in __dbContext.TblProdLotDtl
                        where cou.FKProductId == PKProductId
                        && (EF.Functions.Like(cou.Color.Trim().ToLower(), Convert.ToString(search) + "%"))
                        && (cou.Batch == Batch || Batch == "")
                        orderby cou.PkLotId
                        select (new ProdLotDtlModel
                        {
                            PkLotId = cou.PkLotId,
                            Batch = cou.Batch,
                            Color = cou.Color,
                            MRP = cou.MRP,
                            SaleRate = cou.SaleRate,
                            PurchaseRate = cou.PurchaseRate,
                            FkmfgGroupId = cou.FkmfgGroupId,
                            TradeRate = cou.TradeRate,
                            DistributionRate = cou.DistributionRate,
                            PurchaseRateUnit = cou.PurchaseRateUnit,
                            MRPSaleRateUnit = cou.MRPSaleRateUnit,
                            Remarks = cou.Remarks
                        }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                data = (from cou in __dbContext.TblProdLotDtl
                        where cou.FKProductId == PKProductId
                 && (EF.Functions.Like(cou.Color.Trim().ToLower(), Convert.ToString(search) + "%"))
                        group cou.Color by cou.Color into g
                        select (new ProdLotDtlModel
                        {
                            Color = g.Key,
                        }
                )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            return data;
        }

        public List<ProdLotDtlModel> ProductMRPList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0, string Batch = "", string Color = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProdLotDtlModel> data = (from cou in __dbContext.TblProdLotDtl
                                          where cou.FKProductId == PKProductId
                                          && (cou.Batch == Batch || Batch == "")
                                          && (cou.Color == Color || Color == "")
                                          && (EF.Functions.Like(cou.MRP.ToString().Trim().ToLower(), Convert.ToString(search) + "%"))
                                          orderby cou.PkLotId
                                          select (new ProdLotDtlModel
                                          {
                                              PkLotId = cou.PkLotId,
                                              Batch = cou.Batch,
                                              Color = cou.Color,
                                              MRP = cou.MRP,
                                              SaleRate = cou.SaleRate,
                                              PurchaseRate = cou.PurchaseRate,
                                              FkmfgGroupId = cou.FkmfgGroupId,
                                              TradeRate = cou.TradeRate,
                                              DistributionRate = cou.DistributionRate,
                                              PurchaseRateUnit = cou.PurchaseRateUnit,
                                              MRPSaleRateUnit = cou.MRPSaleRateUnit,
                                              Remarks = cou.Remarks
                                          }
                                         )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object InvoiceList(long FkPartyId, DateTime? InvoiceDate = null)
        {
            SalesInvoiceRepository rep = new SalesInvoiceRepository(__dbContext, _contextAccessor);
            return rep.InvoiceListByPartyId_Date(FkPartyId, InvoiceDate);

        }
        public List<BankModel> BankList()
        {
            BankRepository rep = new BankRepository(__dbContext, _contextAccessor);
            return rep.GetList(1000, 1);
        }

        public List<SeriesModel> SeriesList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {
            SeriesRepository rep = new SeriesRepository(__dbContext, _contextAccessor);
            return rep.GetList(pageSize, pageNo, search, TranAlias, DocumentType);
        }


        public List<AccountMasModel> AccountList()
        {
            AccountMasRepository rep = new AccountMasRepository(__dbContext, _contextAccessor);
            return rep.GetList(1000, 1);
        }

        public object VoucherColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            try
            {
                if (fieldName == "Account")
                {
                    setAccountinfo(model, model.VoucherDetails[rowIndex]);
                }

                if (fieldName == "Delete")
                {
                    model.VoucherDetails[rowIndex].ModeForm = 2;
                }

                if (fieldName == "Credit")
                {
                    model.VoucherDetails[rowIndex].DebitAmt = 0;
                    model.VoucherDetails[rowIndex].VoucherAmt = model.VoucherDetails[rowIndex].CreditAmt;
                }
                if (fieldName == "Debit")
                {
                    model.VoucherDetails[rowIndex].CreditAmt = 0;
                    model.VoucherDetails[rowIndex].VoucherAmt = -model.VoucherDetails[rowIndex].DebitAmt;
                }
                model.VoucherDetails[rowIndex].CurrentBalance = model.VoucherDetails[rowIndex].Balance + model.VoucherDetails[rowIndex].VoucherAmt;
                VoucherCalculateExe(model);
            }
            catch (Exception ex) { }
            return model;
        }
        public void setAccountinfo(TransactionModel model, VoucherDetails? detail)
        {
            if (detail != null)
            {
                var account = new AccountMasRepository(__dbContext, _contextAccessor).GetSingleRecord(detail.FkAccountId);
                if (account != null)
                {
                    var series = new SeriesRepository(__dbContext, _contextAccessor).GetSingleRecord(model.FKSeriesId);
                    decimal Balance = 0;
                    var _chek = model.VoucherDetails.ToList().Where(x => x.FkAccountId == detail.FkAccountId && x.SrNo > 0 && x.ModeForm != 2).ToList();
                    if (_chek.Count > 1)
                    {
                        Balance = _chek.OrderByDescending(x => x.SrNo).Skip(1).FirstOrDefault().CurrentBalance;
                    }
                    else
                    {
                        Balance = __dbContext.TblWalletMas.Where(x => x.FkAccountId == detail.FkAccountId).FirstOrDefault().BalAmount;
                    }

                    detail.ModeForm = 0;
                    // detail.SrNo = model.VoucherDetails.ToList().Count > 0 ? model.VoucherDetails.ToList().Max(x => x.SrNo) + 1 : 1;
                    detail.VoucherAmt = 0;
                    detail.CreditAmt = 0;
                    detail.DebitAmt = 0;
                    detail.CurrentBalance = detail.Balance = Balance;
                    detail.Location = series.BranchName;
                    detail.AccountGroupName = account.AccountGroupName;
                    detail.AccountName_Text = account.Account;
                    detail.FKLocationID = model.FKLocationID;

                }
            }
        }

        public void VoucherCalculateExe(TransactionModel model)
        {
            model.NetAmt = model.VoucherDetails.Where(x => x.ModeForm != 2 && x.FkAccountId > 0).ToList().Sum(x => x.CreditAmt);


            // model.TranDetails = model.TranDetails.Where(x => x.FkProductId > 0).ToList();
        }


        public object Get_CategorySizeList_ByProduct(long PKProductId, string search = "")
        {
            ProductRepository rep = new ProductRepository(__dbContext, _contextAccessor);
            var data = new object();
            if (PKProductId > 0)
            {
                long CategoryId = rep.GetSingleRecord(PKProductId).FKProdCatgId;

                data = (from cou in __dbContext.TblCategorySizeLnk
                        where cou.FkCategoryId == CategoryId
                         && (EF.Functions.Like(cou.Size.Trim().ToLower(), Convert.ToString(search) + "%"))

                        orderby cou.PkId
                        select (new
                        {
                            cou.PkId,
                            cou.Size,
                        }
                       )).ToList();
            }
            return data;
        }

        public long SaveWalkingCustomer(WalkingCustomerModel model)
        {
            TblWalkingCustomerMas Tbl = new TblWalkingCustomerMas();
            Tbl.Name = model.Name;
            Tbl.Mobile = model.Mobile;
            Tbl.Dob = model.Dob;
            Tbl.MarriageDate = model.MarriageDate;
            Tbl.Address = model.Address;
            Tbl.FkLocationId = model.FkLocationId;
            Tbl.CreationDate = Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKCreatedByID = Tbl.FKUserID = model.FKUserID;
            //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
            __dbContext.Add(Tbl);
            __dbContext.SaveChanges();

            return Tbl.PkId;

            //AddImagesAndRemark(obj.PkcountryId, obj.FKCustomerID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }

        public WalkingCustomerModel GeWalkingCustomer_byMobile(string Mobile)
        {

            WalkingCustomerModel data = new WalkingCustomerModel();

            data = (from cou in __dbContext.TblWalkingCustomerMas
                    where cou.Mobile == Mobile
                    select (new WalkingCustomerModel
                    {
                        PkId = cou.PkId,
                        Name = cou.Name,
                        Mobile = cou.Mobile,
                        Dob = cou.Dob,
                        MarriageDate = cou.MarriageDate,
                        Address = cou.Address,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                    })).FirstOrDefault();

            return data;
        }

        public object BarcodeList(TransactionModel model, int rowIndex, bool IsReturn)
        {
            ProductRepository rep = new ProductRepository(__dbContext, _contextAccessor);
            if (!IsReturn)
            {
                model.UniqIdDetails = model.UniqIdDetails == null ? new List<BarcodeUniqVM>() : model.UniqIdDetails;// JsonConvert.DeserializeObject<List<BarcodeVM>>(dd);

                var lst = (from cou in __dbContext.TblProductQTYBarcode
                           where cou.FkLotID == model.TranDetails[rowIndex].FkLotId && cou.FkProductId == model.TranDetails[rowIndex].FkProductId
                           && ((cou.TranOutId == model.PkId || cou.TranOutId == null)
                           && (cou.TranOutSeriesId == model.FKSeriesId || cou.TranOutSeriesId == null)
                           && (cou.TranOutSrNo == model.TranDetails[rowIndex].SrNo || cou.TranOutSrNo == null)
                           )
                           select new
                           {
                               Barcode = cou.Barcode,
                               // IsPrint = model.UniqIdDetails.Find(u => u.Barcode == cou.Barcode)?.Name = "CBA";,
                               IsPrint = false,// (model.UniqIdDetails.ToList().Where(x => x.Barcode == cou.Barcode && x.SrNo == 1).ToList().Count>0) ? true : false,
                           }).ToList();
                var data = new List<object>();
                foreach (var d in lst)
                {
                    bool IsPrint = (model.UniqIdDetails.ToList().Where(x => x.Barcode == d.Barcode && x.SrNo == model.TranDetails[rowIndex].SrNo).ToList().Count > 0) ? true : false;
                    data.Add(new
                    {
                        Barcode = d.Barcode,
                        IsPrint = IsPrint,
                        SrNo = model.TranDetails[rowIndex].SrNo,
                    });
                }
                return data;
            }
            else
            {
                model.UniqIdReturnDetails = model.UniqIdReturnDetails == null ? new List<BarcodeUniqVM>() : model.UniqIdReturnDetails;// JsonConvert.DeserializeObject<List<BarcodeVM>>(dd);

                var lst = (from cou in __dbContext.TblProductQTYBarcode
                           where cou.FkLotID == model.TranReturnDetails[rowIndex].FkLotId && cou.FkProductId == model.TranReturnDetails[rowIndex].FkProductId
                           && ((cou.TranOutId == model.PkId || cou.TranOutId == null)
                           && (cou.TranOutSeriesId == model.FKSeriesId || cou.TranOutSeriesId == null)
                           && (cou.TranOutSrNo == model.TranReturnDetails[rowIndex].SrNo || cou.TranOutSrNo == null)
                           )
                           select new
                           {
                               Barcode = cou.Barcode,
                               // IsPrint = model.UniqIdReturnDetails.Find(u => u.Barcode == cou.Barcode)?.Name = "CBA";,
                               IsPrint = false,// (model.UniqIdReturnDetails.ToList().Where(x => x.Barcode == cou.Barcode && x.SrNo == 1).ToList().Count>0) ? true : false,
                           }).ToList();
                var data = new List<object>();
                foreach (var d in lst)
                {
                    bool IsPrint = (model.UniqIdReturnDetails.ToList().Where(x => x.Barcode == d.Barcode && x.SrNo == model.TranReturnDetails[rowIndex].SrNo).ToList().Count > 0) ? true : false;
                    data.Add(new
                    {
                        Barcode = d.Barcode,
                        IsPrint = IsPrint,
                        SrNo = model.TranReturnDetails[rowIndex].SrNo,
                    });
                }
                return data;
            }
        }

        public List<ColumnStructure> TrandtlColumnList(string TranType)
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>();
            if (TranType == "P")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="ArticalNo",       Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Size",            Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="C"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Color",           Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="MRP",             Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Purchase Rate",   Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Sale Rate",       Fields="SaleRate",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Trade Rate",      Fields="TradeRate",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Distribution Rate",Fields="DistributionRate",    Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="QTY",             Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Free Qty",        Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Disc %",          Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Disc Amt",        Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Disc Type",       Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Gross Amt",       Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="GST Rate",        Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="GST Amount",      Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Net Amount",      Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Barcode",         Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Del",             Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Due Qty",         Fields="DueQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},

                };
            }
            else if (TranType == "R")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="InvoiceDate",  Fields="InvoiceDate",         Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="D1"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="FKInvoiceID",  Fields="FKInvoiceID_Text",    Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            else if (TranType == "R2")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="FKInvoiceID",  Fields="FKInvoiceID_Text",    Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            else if (TranType == "SORD")
            {

                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="C"  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Due Qty",         Fields="DueQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" },
              
                };
            }
            else if (TranType == "Walkingdtl")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Due Qty",         Fields="DueQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="PromotionName",      Fields="PromotionName",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" },

                };
            }
            else
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Due Qty",         Fields="DueQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="PromotionName",      Fields="PromotionName",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" },

                };
            }
            return list.OrderBy(x => x.Orderby).ToList();
        }

        public string SaveInvoiceBilty(long FkUserId, long FkID, long FKSeriesId, long FkFormId, string BiltyNo, string Image)
        {
            string error = "";
            TblSalesInvoicetrn Tbl = new TblSalesInvoicetrn();
            TblImgRemarkMas TblImg = new TblImgRemarkMas();

            var _entity = __dbContext.TblSalesInvoicetrn.Find(FkID);
            var _entityImg = __dbContext.TblImgRemarkMas.Where(x => x.FKID == FkID && x.FKSeriesId == FKSeriesId && x.FkFormId == FkFormId).FirstOrDefault();
            if (_entity != null)
            {
                Tbl = _entity;
                Tbl.BiltyNo = BiltyNo;
                __dbContext.Update(Tbl);
                if (_entityImg != null)
                {
                    TblImg = _entityImg;
                    TblImg.FKID = FkID;
                    TblImg.FKSeriesId = FKSeriesId;
                    TblImg.FkFormId = FkFormId;
                    TblImg.Image = Image;
                    __dbContext.Update(TblImg);
                }
                else
                {
                    TblImg.FKID = FkID;
                    TblImg.FKSeriesId = FKSeriesId;
                    TblImg.FkFormId = FkFormId;
                    TblImg.Image = Image;
                    TblImg.FKCreatedByID = FkUserId;
                    TblImg.FKUserID = FkUserId;
                    TblImg.CreationDate = DateTime.Now;
                    TblImg.ModifiedDate = DateTime.Now;

                    __dbContext.Add(TblImg);
                }
                __dbContext.SaveChanges();
            }

            else { error = "data not found"; }
            return error;
        }
        public object GetInvoiceBilty(long FkID, long FKSeriesId, long FkFormId)
        {
            var data = new object();

            var _entity = __dbContext.TblSalesInvoicetrn.Find(FkID);
            var _entityImg = __dbContext.TblImgRemarkMas.Where(x => x.FKID == FkID && x.FKSeriesId == FKSeriesId && x.FkFormId == FkFormId).FirstOrDefault();
            if (_entity != null)
            {

                if (_entityImg != null)
                {
                    data = new
                    {
                        _entity.BiltyNo,
                        _entityImg.Image
                    };
                }
                else
                {
                    data = new
                    {
                        _entity.BiltyNo,
                        Image = ""
                    };
                }
            }


            return data;
        }
        public object GetInvoiceShippingDetail(long FkID, long FKSeriesId)
        {
            var data = new TransactionModel();
            DataSet ds = new DataSet();
            string JsonData = "";
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_SalesInvoiceShippingDetailById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PkId", FkID);
                cmd.Parameters.AddWithValue("@FkSeriesId", FKSeriesId);
                cmd.Parameters.Add(new SqlParameter("@JsonData", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "JsonData", DataRowVersion.Default, null));
                cmd.Parameters.Add(new SqlParameter("@ErrMsg", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "ErrMsg", DataRowVersion.Default, null));
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                JsonData = Convert.ToString(cmd.Parameters["@JsonData"].Value);
                // ErrMsg = Convert.ToString(cmd.Parameters["@ErrMsg"].Value);
                con.Close();
            }
            if (!string.IsNullOrEmpty(JsonData))
            {
                List<TransactionModel> aa = JsonConvert.DeserializeObject<List<TransactionModel>>(JsonData);
                if (aa != null)
                {
                    data = aa[0];
                    if (data.EWayDetails != null)
                    {
                        if (data.EWayDetails.Count > 0)
                        {
                            data.EWayDetail = data.EWayDetails.FirstOrDefault();
                        }
                    }
                    if (data.FKBankThroughBankID > 0)
                        SetBankThroughBank(data, (long)data.FKBankThroughBankID);
                }

            }
            return data;
        }
        public string SaveInvoiceShippingDetail(TransactionModel JsonData)
        {
            string error = "";
            try
            {

                using (SqlConnection con = new SqlConnection(conn))
                {
                    //con.Open();
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("usp_SalesInvoiceShippingDetailAddUpd", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JsonData", JsonConvert.SerializeObject(JsonData));
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex) { error = ex.Message; }

            return error;
        }

        //AddImagesAndRemark(obj.PkcountryId, obj.FKCustomerID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());

        public object GetPrintData(long PkId, long FkSeriesId)
        {
            var model = GetSingleRecord(PkId, FkSeriesId);
            SeriesRepository repSeries = new SeriesRepository(__dbContext, _contextAccessor);
            var FormatName = repSeries.GetSingleRecord(model.FKSeriesId).FormatName;
            FormatName = string.IsNullOrEmpty(FormatName) ? "Wholesale" : FormatName;

            return new { model = model, FormatName = FormatName };
        }
        public object Get_CategoryList(int pageSize, int pageNo = 1, string search = "")
        {
            CategoryRepository repository = new CategoryRepository(__dbContext, _contextAccessor);
            return repository.CustomList((int)Handler.en_CustomFlag.CustomDrop, pageSize, pageNo, search);

        }

        public List<TranDetails> Get_ProductInfo_FromFile(List<TranDetails> dtlList)
        {
            //foreach (var detail in dtlList)
            //{

            //    if (!int.TryParse(dr["Qty"]?.ToString().Trim(), out int qty))
            //        error += $" Qty is zero.";

            //    if (!decimal.TryParse(dr["MRP"]?.ToString().Trim(), out decimal mrp))
            //        error += $" MRP is zero.";


            //    if (string.IsNullOrWhiteSpace(dr["Barcode"]?.ToString()))
            //    {
            //        error += $" Artical is blank.";
            //    }
            //    else if (!IsAlphanumeric(dr["Barcode"]?.ToString()))
            //    {
            //        error += $"Barcode Must Be Alphanumeric. ";
            //    }
            //    else if (tranList.Where(x => x.Barcode?.ToString().ToLower() == dr["Barcode"]?.ToString().ToLower().Trim()).Count() > 0)
            //    {
            //        error += $" Duplicate Barcode. ";
            //    }

            //    log += "\n Product " + DateTime.Now.ToString("HH:mm:ss");


            //    if (error != "")
            //        validationErrors.Add($"Row {srNo} {error}");
            //    else
            //    {
            //        var item = new TranDetails
            //        {
            //            SrNo = srNo,
            //            Barcode = dr["Barcode"]?.ToString().Trim(),
            //            ProductDisplay = dr["Artical"]?.ToString().Trim(),
            //            Batch = dr["Size"]?.ToString().Trim(),
            //            Color = dr["Color"]?.ToString().Trim(),
            //            Qty = qty,
            //            MRP = mrp,
            //            SaleRate = mrp,
            //            TradeRate = mrp,
            //            DistributionRate = mrp,
            //            FkLotId = 0
            //        };
            //        DataTable dtProduct = GetProduct("", 0, 0, 0, dr["Artical"]?.ToString(), false, "");
            //        if (dtProduct.Rows.Count > 0)
            //        {

            //            item.FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
            //            item.FkBrandId = Convert.ToInt64(dtProduct.Rows[0]["FkBrandId"].ToString());
            //            item.FKProdCatgId = Convert.ToInt64(dtProduct.Rows[0]["FKProdCatgId"].ToString());
            //            item.SubCategoryName = dtProduct.Rows[0]["CategoryName"].ToString();
            //            item.Product = dtProduct.Rows[0]["Product"].ToString();
            //            item.CodingScheme = dtProduct.Rows[0]["CodingScheme"].ToString();
            //        }
            //        else
            //        {
            //            var Category = __dbContext.TblCategoryMas.Where(x => x.CategoryName == dr["SubSection"].ToString()).SingleOrDefault();
            //            if (Category != null)
            //            {
            //                item.FKProdCatgId = Category.PkCategoryId;
            //                item.SubCategoryName = Category.CategoryName;
            //            }
            //            else
            //                validationErrors.Add($"Row {srNo} Category : {dr["SubSection"].ToString()}");

            //        }
            //        tranList.Add(item);

            //    }
            //}
            //return tranList;

            return null;

        }

        public bool IsAlphanumeric(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
        }
        public object BindImportData(TransactionModel model, List<TranDetails> details)
        {
            try
            {

                model.TranDetails = new List<TranDetails>();
                if (details != null)
                {
                    int SrNo = 0;
                    foreach (var item in details)
                    {
                        SrNo++;
                        if (item.FkProductId > 0)
                        {
                            var detail = item;
                            detail.SrNo = SrNo;
                            CalculateExe(model, detail);

                            if (!string.IsNullOrEmpty(detail.Barcode))
                                model.UniqIdDetails.Add(new BarcodeUniqVM() { SrNo = detail.SrNo, Barcode = detail.Barcode });

                            detail.Barcode = "Barcode";
                            model.TranDetails.Add(detail);
                        }
                        else
                        {
                            InsertProduct(item);
                            DataTable dtProduct = GetProduct("", model.FKLocationID, 0, 0, item.ProductDisplay, false, model.TranAlias);
                            if (dtProduct.Rows.Count > 0)
                            {
                                var detail = new TranDetails();
                                TranDetailDefault(model, detail);
                                SetProduct(model, detail, dtProduct);
                                detail.SrNo = SrNo;
                                detail.Qty = item.Qty;
                                CalculateExe(model, detail);


                                if (!string.IsNullOrEmpty(detail.Barcode))
                                    model.UniqIdDetails.Add(new BarcodeUniqVM() { SrNo = detail.SrNo, Barcode = detail.Barcode });

                                detail.Barcode = "Barcode";
                                model.TranDetails.Add(detail);
                            }

                            // Check Product UNique/Lot/PRoduct

                        }
                    }
                }
                else
                    throw new Exception("Invalid Request");

            }
            catch (Exception ex) { }
            return model;
        }

        private void InsertProduct(TranDetails detail)
        {
            if (__dbContext.TblProductMas.Where(u => u.Product == detail.ProductDisplay).FirstOrDefault() == null)
            {
                TblProductMas Tbl = new TblProductMas();
                var data = __dbContext.TblProductMas.OrderByDescending(u => u.PkProductId).FirstOrDefault();
                if (data != null)
                {
                    Tbl.PkProductId = data.PkProductId + 1;
                }
                else
                {
                    Tbl.PkProductId = 1;
                }
                Tbl.Product = detail.ProductDisplay;
                Tbl.NameToDisplay = detail.ProductDisplay;
                Tbl.NameToPrint = detail.ProductDisplay;
                //Tbl.Image = model.Image;
                //Tbl.Alias = model.Alias;
                //Tbl.Strength = model.Strength;
                //Tbl.Barcode = detail.Barcode;
                //Tbl.Status = model.Status;
                Tbl.FKProdCatgId = detail.FKProdCatgId;
                //Tbl.FKTaxID = model.FKTaxID;
                //Tbl.HSNCode = model.HSNCode;
                Tbl.FkBrandId = null;
                //Tbl.ShelfID = model.ShelfID;
                //Tbl.TradeDisc = model.TradeDisc;
                //Tbl.MinStock = model.MinStock;
                //Tbl.MaxStock = model.MaxStock;
                //Tbl.MinDays = model.MinDays;
                //Tbl.MaxDays = model.MaxDays;
                Tbl.CaseLot = "";
                //Tbl.BoxSize = model.BoxSize;
                //Tbl.Description = model.Description;
                Tbl.Unit1 = "";
                //Tbl.ProdConv1 = model.ProdConv1;
                Tbl.Unit2 = "";
                //Tbl.ProdConv2 = model.ProdConv2;
                Tbl.Unit3 = "";
                Tbl.MRP = detail.MRP;
                Tbl.MRPSaleRateUnit = "";
                Tbl.SaleRate = detail.MRP;
                Tbl.TradeRate = detail.MRP;
                Tbl.DistributionRate = detail.MRP;
                Tbl.PurchaseRate = detail.MRP;
                Tbl.PurchaseRateUnit = "";
                //Tbl.KeepStock = model.KeepStock;
                //Tbl.Genration = model.Genration;
                Tbl.CodingScheme = GetSysDefaultsByKey("CodingScheme");
                Tbl.FkUnitId = null;
                Tbl.ModifiedDate = DateTime.Now;
                Tbl.FKUserID = GetUserID();
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;


                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
                SaveClientData();
                //AddImagesAndRemark(obj.PkcountryId, obj.FKProductID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
            }
        }

    }
}



