using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public TranBaseRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public string Create(TransactionModel model)
        {
            string Error = "";

            Error = ValidateData(model);
            if (Error == "")
            {
                setDefaultBeforeSave(model);
                CalculateExe(model);
                setGridTotal(model);
                setPaymentDetail(model);
                long Id = 0;
                long SeriesNo = 0;
                SaveData(model, ref Id, ref Error, ref SeriesNo);
            }
            return Error;
        }

        public string ValidateData(TransactionModel objmodel)
        {
            //
            string Error = "";
            try
            {
                if (objmodel.TranDetails != null)
                {
                    foreach (var item in objmodel.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
                    {
                        if (string.IsNullOrEmpty(item.Color))
                        {
                            throw new Exception("Color Required on Product " + item.Product);
                        }
                        if (string.IsNullOrEmpty(item.Batch))
                        {
                            throw new Exception("Batch Required on Product " + item.Product);
                        }
                    }

                }

                Error = ValidData(objmodel);
            }
            catch (Exception ex) { Error = ex.Message; }
            return Error;
        }

        public virtual string ValidData(TransactionModel objmodel)
        {
            string Error = "";
            try
            {
                if (objmodel.CreditCard && (objmodel.CreditCardAmt < 0 || objmodel.CreditCardNo == "" || objmodel.CreditCardDate == null || objmodel.FKBankCreditCardID == null))
                    Error = "Please Enter Valid Card Detail";

                if (objmodel.Cheque && (objmodel.ChequeAmt < 0 || objmodel.ChequeNo == "" || objmodel.ChequeDate == null || objmodel.FKBankChequeID == null))
                    Error = "Please Enter Valid Cheque Detail";

                if (objmodel.Cheque && (objmodel.CreditAmt < 0 || objmodel.CreditDate == null || objmodel.FKPostAccID == null))
                    Error = "Please Enter Valid Credit Detail";


              
            }
            catch (Exception ex) { Error = ex.Message; }
            return Error;
        }
        public void setDefaultBeforeSave(TransactionModel model)
        {
            if (model.TranDetails != null)
            {
                foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
                {
                    item.RateUnit = "1";
                    item.SchemeDiscType = item.TradeDiscType = item.LotDiscType = "R";
                    //if (model.TranAlias == "PORD" || model.TranAlias == "PINV")
                    //{
                    //    item.FkLotId = 0;
                    //}
                }

            }
        }

        public void SaveData(TransactionModel JsonData, ref long Id, ref string ErrMsg, ref long SeriesNo)
        {

            var aa = JsonConvert.SerializeObject(JsonData);

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
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
        }

        public DataTable GetList(string FromDate, string ToDate, string SeriesFilter, string DocumentType)
        {
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
                    data = aa[0];
                }
            }
            else
            {
                //UserLastSeries
            }
            CalculateExe_For_Update(data);
            return data;
        }
        public void CalculateExe_For_Update(TransactionModel model)
        {
            foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            {

                item.GstRate = Math.Round((item.SCRate * 2), 2);
                item.GstAmt = Math.Round((item.SCAmt * 2), 2);
            }

            // model.TranDetails = model.TranDetails.Where(x => x.FkProductId > 0).ToList();
        }

        public object BarcodeScan(TransactionModel model, string barcode)
        {
            try
            {
                var product = new ProductRepository(__dbContext).GetSingleRecord_ByBarcode(barcode);
                if (product != null)
                {

                    //check
                    var detail = new TranDetails();
                    var _old = model.TranDetails.ToList().Where(x => x.FkProductId == product.PkProductId).FirstOrDefault();
                    if (_old == null)
                    {

                        var _checkSrNo = model.TranDetails.ToList().Where(x => x.FkProductId > 0 && x.Qty > 0).ToList();
                        if (_checkSrNo.Count > 0)
                        {
                            detail.SrNo = _checkSrNo.Max(x => x.SrNo) + 1;

                        }
                        else { detail.SrNo = 1; }
                        detail.FkProductId = product.PkProductId;
                        detail.Product = product.Product;
                        detail.Qty = 1;
                        detail.ModeForm = 0;//0=Add,1=Edit,2=Delete 
                        var _lotEntity = __dbContext.TblProdLotDtl.Where(x => x.FKProductId == product.PkProductId).FirstOrDefault();
                        if (_lotEntity != null)
                        {
                            detail.MRP = _lotEntity.MRP;
                            detail.SaleRate = _lotEntity.SaleRate > 0 ? _lotEntity.SaleRate : 0;
                            detail.FkLotId = _lotEntity.PkLotId;
                            detail.Color = _lotEntity.Color;
                            detail.Batch = _lotEntity.Batch;
                        }
                        else
                        {
                            detail.MRP = product.MRP;
                            detail.SaleRate = product.SaleRate;
                            detail.FkLotId = 0;
                            detail.Batch = "";
                            detail.Color = "";
                        }
                        detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                        detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                        detail.TradeDisc = 0;
                        detail.TradeDiscAmt = 0;
                        detail.TradeDiscType = "";
                        detail.Qty = 1;

                        model.TranDetails.Add(detail);
                    }
                    else
                    {
                        int rowIndex = model.TranDetails.FindIndex(a => a.FkProductId == product.PkProductId);
                        model.TranDetails[rowIndex].Qty += 1;
                    }
                }

                CalculateExe(model);
                setGridTotal(model);
                setPaymentDetail(model);
            }
            catch (Exception ex) { }
            return model;
        }

        public object ColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            try
            {
                if (fieldName == "Product")
                {
                    setProductinfo(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "ProductReturn")
                {
                    setReturnProductinfo(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "Batch")
                {
                    setProductinfoByBatch(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "Color")
                {
                    setProductinfoByColor(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "Delete")
                {
                    model.TranDetails[rowIndex].ModeForm = 2;
                }
                if (fieldName == "TradeDisc")
                {
                    model.TranDetails[rowIndex].TradeDiscAmt = 0;
                }
                if (fieldName == "FKInvoiceID")
                {
                    setProductinfoByColor(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "Inum")
                {
                    setInvoiceinfo(model, model.TranDetails[rowIndex]);
                }
                CalculateExe(model);
                setGridTotal(model);
                setPaymentDetail(model);
            }
            catch (Exception ex) { }
            return model;
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

            setGridTotal(model);
            setPaymentDetail(model);
            return model;
        }

        public void setProductinfo(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {
                var product = new ProductRepository(__dbContext).GetSingleRecord(detail.FkProductId);
                if (product != null)
                {
                    var _checkSrNo = model.TranDetails.ToList().Where(x => x.FkProductId > 0 && x.Qty > 0).ToList();
                    if (_checkSrNo.Count > 0)
                    {
                        detail.SrNo = _checkSrNo.Max(x => x.SrNo) + 1;

                    }
                    else { detail.SrNo = 1; }
                    detail.FkProductId = product.PkProductId;
                    detail.Qty = 1;
                    detail.ModeForm = 0;//0=Add,1=Edit,2=Delete 
                    detail.FKLocationID = model.FKLocationID;
                    detail.ReturnTypeID = 2;
                    var _lotEntity = __dbContext.TblProdLotDtl.Where(x => x.FKProductId == product.PkProductId).FirstOrDefault();
                    if (_lotEntity != null)
                    {
                        detail.MRP = _lotEntity.MRP;
                        detail.SaleRate = _lotEntity.SaleRate > 0 ? _lotEntity.SaleRate : 0;
                        detail.FkLotId = _lotEntity.PkLotId;
                        detail.Color = _lotEntity.Color;
                        detail.Batch = _lotEntity.Batch;
                        if (model.TranAlias == "PORD" || model.TranAlias == "PINV")
                        {
                            detail.FkLotId = 0;
                        }
                     }
                    else
                    {
                        detail.MRP = product.MRP;
                        detail.SaleRate = product.SaleRate;
                        detail.FkLotId = 0;
                        detail.Batch = "";
                        detail.Color = "";
                    }
                    detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                    detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    detail.TradeDisc = 0;
                    detail.TradeDiscAmt = 0;
                    detail.TradeDiscType = "";
                    detail.Qty = 1;
                    detail.TradeRate = detail.DistributionRate = detail.SaleRate;

                }
            }
        }
        public void setReturnProductinfo(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {
                var _invoice = (from saledtl in __dbContext.TblSalesInvoicedtl
                                join sale in __dbContext.TblSalesInvoicetrn on saledtl.FkId equals sale.PkId
                                join series in __dbContext.TblSeriesMas on sale.FKSeriesId equals series.PkSeriesId
                                join cou in __dbContext.TblProductMas on saledtl.FkProductId equals cou.PkProductId

                                where sale.FkPartyId == model.FkPartyId
                                && saledtl.SrNo == detail.InvoiceSrNo
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
                    var product = _invoice.cou;

                    detail.FkProductId = product.PkProductId;
                    detail.Qty = 1;
                    detail.ModeForm = 0;//0=Add,1=Edit,2=Delete 

                    detail.MRP = _invoice.saledtl.MRP != null ? Convert.ToDecimal(_invoice.saledtl.MRP) : 0;
                    detail.SaleRate = _invoice.saledtl.SaleRate;
                    detail.FkLotId = 0;
                    detail.Batch = _invoice.saledtl.Batch;
                    detail.Color = _invoice.saledtl.Color;

                    detail.GstRate = (_invoice.saledtl.Rate < 1000 ? 5 : 18);
                    detail.Rate = Math.Round(Convert.ToDecimal(_invoice.saledtl.Rate) * (100 / (100 + (_invoice.saledtl.ICRate != null ? Convert.ToDecimal(_invoice.saledtl.ICRate) : 0))), 2);
                    detail.TradeDisc = 0;
                    detail.TradeDiscAmt = 0;
                    detail.TradeDiscType = "";
                    detail.Qty = 1;

                    detail.InvoiceDate = _invoice.sale.EntryDate.Date;
                    detail.FKInvoiceID_Text = _invoice.series.Series + _invoice.sale.EntryNo;
                    detail.FKInvoiceSrID = _invoice.sale.FKSeriesId;

                }
            }
        }
        public void setProductinfoByBatch(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {

                var _productLot = __dbContext.TblProdLotDtl.Where(x => x.FKProductId == detail.FkProductId && x.PkLotId == detail.FkLotId).FirstOrDefault();
                if (_productLot != null)
                {

                    detail.MRP = _productLot.MRP;
                    detail.SaleRate = _productLot.SaleRate > 0 ? _productLot.SaleRate : 0;
                    detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                    detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    detail.Color = _productLot.Color;
                    detail.Batch = _productLot.Batch;
                }

            }
        }

        public void setProductinfoByColor(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {

                var _productLot = __dbContext.TblProdLotDtl.Where(x => x.FKProductId == detail.FkProductId && x.PkLotId == detail.FkLotId).FirstOrDefault();
                if (_productLot != null)
                {

                    detail.MRP = _productLot.MRP;
                    detail.SaleRate = _productLot.SaleRate > 0 ? _productLot.SaleRate : 0;
                    detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                    detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    detail.Color = _productLot.Color;
                    detail.Batch = _productLot.Batch;
                }

            }
        }

        public void setInvoiceinfo(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
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
                    detail.InvoiceDate = _invoice.EntryDate.Date;
                    detail.FKInvoiceID_Text = _invoice.Inum;
                    detail.FKInvoiceSrID = _invoice.FKSeriesId;
                }


            }
        }

        public void CalculateExe(TransactionModel model)
        {
            foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2 && x.FkProductId > 0))
            {
                decimal amt = item.Rate * item.Qty;
                decimal decAmt = item.TradeDisc > 0 ? (amt * item.TradeDisc / 100) : item.TradeDiscAmt;

                item.GstRate = (item.Rate < 1000 ? 5 : 18); 
                item.TradeDiscAmt = decAmt;
                item.GrossAmt = Math.Round(amt - item.TradeDiscAmt, 2);//Math.Round(item.Rate * item.Qty, 2) ;
                item.GstAmt = Math.Round(item.GrossAmt * item.GstRate / 100, 2);
                item.SCRate = Math.Round(item.GstRate / 2, 2);
                item.SCAmt = Math.Round(item.GstAmt / 2, 2);
                item.NetAmt = Math.Round(item.GrossAmt + item.GstAmt, 2);
                item.FKLocationID = model.FKLocationID;
                item.TaxableAmt = Math.Round(item.Rate - (item.SchemeDiscAmt + item.TradeDiscAmt + item.LotDiscAmt), 2);
            }

            // model.TranDetails = model.TranDetails.Where(x => x.FkProductId > 0).ToList();
        }

        public void setGridTotal(TransactionModel model)
        {
            model.GrossAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.GrossAmt), 2);
            model.TaxAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.GstAmt), 2);
            model.CashDiscountAmt = 0;
            if (model.CashDiscType == "R" && model.CashDiscount > 0 && model.CashDiscount <= model.GrossAmt) { model.CashDiscountAmt = Math.Round(model.CashDiscount, 2); }
            else if (model.CashDiscType == "P" && model.CashDiscount > 0 && model.CashDiscount <= 100) { model.CashDiscountAmt = Math.Round((model.GrossAmt * model.CashDiscount / 100), 2); }
            else { model.CashDiscount = 0; }
            model.TotalDiscount = model.CashDiscountAmt;
            model.NetAmt = Math.Round(model.GrossAmt + model.TaxAmt + model.Shipping + model.OtherCharge - model.RoundOfDiff - model.TotalDiscount, 2);

        }

        public void setPaymentDetail(TransactionModel model)
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
                else { model.CreditCard = false; model.CreditCardAmt = 0; model.CreditCardNo = ""; model.CreditCardDate = null; model.FKBankCreditCardID = null; }


                if (model.Cheque && model.ChequeAmt > 0 && _remAmt > 0)
                {
                    if ((_remAmt - (decimal)model.ChequeAmt) < 0)
                    {
                        model.ChequeAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.ChequeAmt;
                }
                else { model.Cheque = false; model.ChequeAmt = 0; model.ChequeNo = ""; model.ChequeDate = null; model.FKBankChequeID = null; }


                if (model.Credit && model.CreditAmt > 0 && _remAmt > 0)
                {
                    if ((_remAmt - (decimal)model.CreditAmt) < 0)
                    {
                        model.CreditAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.CreditAmt;
                }
                else { model.Credit = false; model.CreditAmt = 0; model.CreditDate = null; model.FKPostAccID = null; }


                if (model.Cash && model.CashAmt > 0 && _remAmt > 0)
                {
                    if ((_remAmt - (decimal)model.CashAmt) < 0)
                    {
                        model.CashAmt = _remAmt;
                    }
                    _remAmt -= (decimal)model.CashAmt;
                }
                else { model.Cash = false; model.CashAmt = 0; }

                if (_remAmt > 0) { model.Cash = true; model.CashAmt += _remAmt; }
            }
            else { model.Cash = true; model.CashAmt = _remAmt; }
        }

        public object PaymentDetail(TransactionModel model)
        {
            setPaymentDetail(model);
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

            setGridTotal(model);
            setPaymentDetail(model);
            return model;
        }

        public List<PartyModel> PartyList(int pageSize, int pageNo = 1, string search = "", string TranType = "")
        {
            if (TranType == "P")
            {
                VendorRepository rep = new VendorRepository(__dbContext);
                return rep.GetList(pageSize, pageNo, search);
            }
            else
            {
                CustomerRepository rep = new CustomerRepository(__dbContext);
                return rep.GetList(pageSize, pageNo, search);
            }
        }

        public object SetParty(TransactionModel model, long FkPartyId)
        {
            var vendor = new PartyModel();
            if (model.ExtProperties.TranAlias == "PINV" || model.ExtProperties.TranAlias == "PORD")
                vendor = GetVendor(FkPartyId);
            else
                vendor = GetCustomer(FkPartyId);

            if (vendor != null)
            {
                model.PartyAddress = vendor.Address == null ? "" : vendor.Address.ToString();
                model.PartyName = vendor.Name;
                model.PartyGSTN = vendor.Gstno == null ? "" : vendor.Gstno.ToString();
                model.PartyMobile = vendor.Mobile;
                model.PartyCredit = 0;
                model.FkPartyId = FkPartyId;

            }
            return model;
        }

        public PartyModel? GetVendor(long PkVendorId)
        {
            PartyModel? data = (from cou in __dbContext.TblVendorMas
                                where cou.PkVendorId == PkVendorId
                                select (new PartyModel
                                {
                                    PkId = cou.PkVendorId,
                                    Name = cou.Name,
                                    Mobile = cou.Mobile,
                                    Address = cou.Address,
                                    Gstno = cou.Gstno
                                }
                               )).FirstOrDefault();
            return data;
        }

        public PartyModel? GetCustomer(long PkId)
        {
            PartyModel? data = (from cou in __dbContext.TblCustomerMas
                                where cou.PkCustomerId == PkId
                                select (new PartyModel
                                {
                                    PkId = cou.PkCustomerId,
                                    Name = cou.Name,
                                    Mobile = cou.Mobile,
                                    Address = cou.Address,
                                    Gstno = cou.Gstno
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
                model.FKLocationID = obj.FkBranchId;
                model.FKSeriesId = FKSeriesId;
            }
            return model;
        }

        public SeriesModel? GetSeries(long FKSeriesId)
        {
            SeriesModel? data = (from cou in __dbContext.TblSeriesMas
                                 where cou.PkSeriesId == FKSeriesId
                                 select (new SeriesModel
                                 {
                                     PkSeriesId = cou.PkSeriesId,
                                     Series = cou.Series,
                                     FkBranchId = cou.FkBranchId
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
                                              //FKCreatedByID = cou.FKCreatedByID,
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
            ProductRepository rep = new ProductRepository(__dbContext);
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

        public List<ProdLotDtlModel> ProductColorList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0, string TranAlias = "")
        {
            List<ProdLotDtlModel> data = new List<ProdLotDtlModel>();

            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            if (TranAlias == "")
            {
                data = (from cou in __dbContext.TblProdLotDtl
                        where cou.FKProductId == PKProductId
                        && (EF.Functions.Like(cou.Color.Trim().ToLower(), Convert.ToString(search) + "%"))
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

        public List<ProdLotDtlModel> ProductMRPList(int pageSize, int pageNo = 1, string search = "", long PKProductId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProdLotDtlModel> data = (from cou in __dbContext.TblProdLotDtl
                                          where cou.FKProductId == PKProductId
                                          where (EF.Functions.Like(cou.MRP.ToString().Trim().ToLower(), Convert.ToString(search) + "%"))
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
            SalesInvoiceRepository rep = new SalesInvoiceRepository(__dbContext);
            return rep.InvoiceListByPartyId_Date(FkPartyId, InvoiceDate);

        }
        public List<BankModel> BankList()
        {
            BankRepository rep = new BankRepository(__dbContext);
            return rep.GetList(1000, 1);
        }

        public List<SeriesModel> SeriesList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {
            SeriesRepository rep = new SeriesRepository(__dbContext);
            return rep.GetList(pageSize, pageNo, search, TranAlias, DocumentType);
        }


        public List<AccountMasModel> AccountList()
        {
            AccountMasRepository rep = new AccountMasRepository(__dbContext);
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
                var account = new AccountMasRepository(__dbContext).GetSingleRecord(detail.FkAccountId);
                if (account != null)
                {
                    var series = new SeriesRepository(__dbContext).GetSingleRecord(model.FKSeriesId);
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

                }
            }
        }

        public void VoucherCalculateExe(TransactionModel model)
        {
            model.NetAmt = model.VoucherDetails.Where(x => x.ModeForm != 2 && x.FkAccountId > 0).ToList().Sum(x => x.CreditAmt);



            // model.TranDetails = model.TranDetails.Where(x => x.FkProductId > 0).ToList();
        }


        public List<CategorySizeLnkModel> Get_CategorySizeList_ByProduct(long PKProductId)
        {
            ProductRepository rep = new ProductRepository(__dbContext);

            long CategoryId = rep.GetSingleRecord(PKProductId).FKProdCatgId;

            List<CategorySizeLnkModel> data = (from cou in __dbContext.TblCategorySizeLnk
                                               where cou.FkCategoryId == CategoryId
                                               orderby cou.PkId
                                               select (new CategorySizeLnkModel
                                               {
                                                   PkId = cou.PkId,
                                                   Size = cou.Size
                                               }
                                              )).ToList();
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
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKCreatedByID = model.FKCreatedByID;
            Tbl.FKUserID = model.FKUserId;
            Tbl.CreationDate = DateTime.Now;
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
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                    })).FirstOrDefault();

            return data;
        }

        public List<ColumnStructure> TrandtlColumnList(string TranType)
        {
            var list = new List<ColumnStructure>();
            if (TranType == "P")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=1 ,  Orderby =1 ,  Heading ="ArticalNo",       Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=2 ,  Orderby =2 ,  Heading ="Size",            Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="C"  },
                    new ColumnStructure{ pk_Id=3 ,  Orderby =3 ,  Heading ="Color",           Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  },
                    new ColumnStructure{ pk_Id=4 ,  Orderby =4 ,  Heading ="MRP",             Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"  },
                    new ColumnStructure{ pk_Id=6 ,  Orderby =6 ,  Heading ="Purchase Rate",   Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=7 ,  Orderby =7 ,  Heading ="Sale Rate",       Fields="SaleRate",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=8 ,  Orderby =8 ,  Heading ="Trade Rate",      Fields="TradeRate",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=9 ,  Orderby =9 ,  Heading ="Distribution Rate",Fields="DistributionRate",    Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=10,  Orderby =10,  Heading ="QTY",             Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=11,  Orderby =11,  Heading ="Free Qty",        Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=12,  Orderby =12,  Heading ="Disc %",          Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=13,  Orderby =13,  Heading ="Disc Amt",        Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=14,  Orderby =14,  Heading ="Disc Type",       Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=15,  Orderby =15,  Heading ="Gross Amt",       Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=16,  Orderby =16,  Heading ="GST Rate",        Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=17,  Orderby =17,  Heading ="GST Amount",      Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=18,  Orderby =18,  Heading ="Net Amount",      Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=19,  Orderby =19,  Heading ="Barcode",         Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  },
                    new ColumnStructure{ pk_Id=20,  Orderby =20,  Heading ="Del",             Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            else if (TranType == "R")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=1,   Orderby =1,  Heading ="InvoiceDate",  Fields="InvoiceDate",         Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="D1"  },
                    new ColumnStructure{ pk_Id=2,   Orderby =2,  Heading ="FKInvoiceID",  Fields="FKInvoiceID_Text",    Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=3,   Orderby =3,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=4,   Orderby =4,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=5,   Orderby =5,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=6,   Orderby =6,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=7,   Orderby =7,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=8,   Orderby =8,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=9,   Orderby =9,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=10,  Orderby =10, Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=12,  Orderby =12, Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=13,  Orderby =13, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=14,  Orderby =14, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=15,  Orderby =15, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=16,  Orderby =16, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=17,  Orderby =17, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=18,  Orderby =18, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  },
                    new ColumnStructure{ pk_Id=19,  Orderby =19, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            else
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=1,   Orderby =1,  Heading ="ArticalNo",    Fields="Product",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=2,   Orderby =2,  Heading ="Size",         Fields="Batch",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=3,   Orderby =3,  Heading ="Color",        Fields="Color",               Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=4,   Orderby =4,  Heading ="MRP",          Fields="MRP",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="CD"  },
                    new ColumnStructure{ pk_Id=5,   Orderby =5,  Heading ="Rate",         Fields="Rate",                Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""},
                    new ColumnStructure{ pk_Id=6,   Orderby =6,  Heading ="QTY",          Fields="Qty",                 Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=7,   Orderby =7,  Heading ="Free Qty",     Fields="FreeQty",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=8,   Orderby =8,  Heading ="Disc %",       Fields="TradeDisc",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"},
                    new ColumnStructure{ pk_Id=9,   Orderby =9,  Heading ="Disc Amt",     Fields="TradeDiscAmt",        Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=10,  Orderby =10, Heading ="Disc Type",    Fields="TradeDiscType",       Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=11,  Orderby =11, Heading ="Gross Amt",    Fields="GrossAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=12,  Orderby =12, Heading ="GST Rate",     Fields="GstRate",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=13,  Orderby =13, Heading ="GST Amount",   Fields="GstAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=14,  Orderby =14, Heading ="Net Amount",   Fields="NetAmt",              Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""   },
                    new ColumnStructure{ pk_Id=15,  Orderby =15, Heading ="Barcode",      Fields="Barcode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  },
                    new ColumnStructure{ pk_Id=16,  Orderby =16, Heading ="Del",          Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            return list.OrderBy(x => x.Orderby).ToList();
        }

    }
}
