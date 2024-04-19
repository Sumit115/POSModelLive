using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System;
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
            CalculateExe(model);
            ValidateData(model);
            setGridTotal(model);
            long Id = 0;
            string Error = "";
            long SeriesNo = 0;
            SaveData(model, ref Id, ref Error, ref SeriesNo);
            return Error;
        }
        public string ValidateData(TransactionModel objmodel)
        {
            //
            ValidData(objmodel);
            return "";
        }
        public virtual string ValidData(TransactionModel objmodel)
        {
            return "";
        }
        public void SaveData(TransactionModel JsonData, ref long Id, ref string ErrMsg, ref long SeriesNo)
        {


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
                SeriesNo = Convert.ToInt64(cmd.Parameters["@SeriesNo"].Value);
                ErrMsg = Convert.ToString(cmd.Parameters["@ErrMsg"].Value);
                con.Close();
            }
        }

        public DataTable GetList(string FromDate, string ToDate, string SeriesFilter = "")
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
                //Get Output Parametr
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();

            }
            return dt;
        }
        public string GetData(long PkId, long FkSeriesId,ref string ErrMsg)
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
        public object ColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            try
            {
                if (fieldName == "Product")
                {
                    setProductinfo(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "Batch"  )
                {
                    setProductinfoByBatch(model, model.TranDetails[rowIndex]);
                }
                if (  fieldName == "Color")
                {
                    setProductinfoByColor(model, model.TranDetails[rowIndex]);
                }
                if (fieldName == "Delete")
                {
                    model.TranDetails[rowIndex].ModeForm = 2;
                }
                CalculateExe(model);
                setGridTotal(model);
            }
            catch (Exception ex) { }
            return model;
        }

        public void setProductinfo(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {
                var product = new ProductRepository(__dbContext).GetSingleRecord(detail.FkProductId);
                if (product != null)
                {
                    detail.FkProductId = product.PkProductId;
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
                    detail.Qty = 1;

                }
            }
        }
        public void setProductinfoByBatch(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {

                var _productLot = __dbContext.TblProdLotDtl.Where(x => x.FKProductId == detail.FkProductId && x.Batch == detail.Batch).FirstOrDefault();
                if (_productLot != null)
                {
                    detail.MRP = _productLot.MRP;
                    detail.SaleRate = _productLot.SaleRate > 0 ? _productLot.SaleRate : 0;
                    detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                    detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    detail.Qty = 1;
                    detail.Color = _productLot.Color;
                    detail.Batch = _productLot.Batch;
                }
                else
                {
                   // detail.Batch = "";
                    detail.FkLotId = 0;
                    var product = new ProductRepository(__dbContext).GetSingleRecord(detail.FkProductId);
                    if (product != null)
                    {
                        detail.MRP = product.MRP;
                        detail.SaleRate = product.SaleRate;
                        detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                        detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    }
                }

            }
        }
        public void setProductinfoByColor(TransactionModel model, TranDetails? detail)
        {
            if (detail != null)
            {

                var _productLot = __dbContext.TblProdLotDtl.Where(x => x.FKProductId == detail.FkProductId && x.Color == detail.Color).FirstOrDefault();
                if (_productLot != null)
                {
                    detail.MRP = _productLot.MRP;
                    detail.SaleRate = _productLot.SaleRate > 0 ? _productLot.SaleRate : 0;
                    detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                    detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    detail.Qty = 1;
                    detail.Color = _productLot.Color;
                    detail.Batch = _productLot.Batch;
                }
                else
                {
                   // detail.Color = "";
                    detail.FkLotId = (!string.IsNullOrEmpty(detail.Batch) & detail.FkLotId > 0) ? detail.FkLotId : 0;
                    var product = new ProductRepository(__dbContext).GetSingleRecord(detail.FkProductId);
                    if (product != null)
                    {
                        detail.MRP = product.MRP;
                        detail.SaleRate = product.SaleRate;
                        detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                        detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);
                    }
                }

            }
        }
        public void CalculateExe(TransactionModel model)
        {
            foreach (var item in model.TranDetails.Where(x => x.ModeForm != 2))
            {
                item.GrossAmt = Math.Round(item.Rate * item.Qty, 2);
                item.GstAmt = Math.Round(item.GrossAmt * item.GstRate / 100, 2);
                item.SCRate = Math.Round(item.GstRate / 2, 2);
                item.SCAmt = Math.Round(item.GstAmt / 2, 2);
                item.NetAmt = Math.Round(item.GrossAmt + item.GstAmt, 2);
            }
        }

        public void setGridTotal(TransactionModel model)
        {
            model.GrossAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.GrossAmt), 2);
            model.TaxAmt = Math.Round(model.TranDetails.Where(x => x.ModeForm != 2).Sum(x => x.GstAmt), 2);
            model.CashDiscountAmt = 0;
            if (model.CashDiscType == "R" && model.CashDiscount > 0 && model.CashDiscount <= model.GrossAmt) { Math.Round(model.CashDiscountAmt = model.CashDiscount, 2); }
            else if (model.CashDiscType == "P" && model.CashDiscount > 0 && model.CashDiscount <= 100) { model.CashDiscountAmt = Math.Round((model.GrossAmt * model.CashDiscount / 100), 2); }
            else { model.CashDiscount = 0; }
            model.TotalDiscount = model.CashDiscountAmt;
            model.NetAmt = Math.Round(model.GrossAmt + model.TaxAmt + model.Shipping + model.OtherCharge - model.RoundOfDiff - model.TotalDiscount, 2);

        }

        public object FooterChange(TransactionModel model, string fieldName)
        {
            //if (fieldName == "ApplyPromo")
            //{
            //    //GEtApplyprome(model)
            //    CalculateExe(model);

            //}

            setGridTotal(model);
            return model;
        }

        public List<object> PartyList(int pageSize, int pageNo = 1, string search = "", string TranType = "")
        {
            if (TranType == "P")
            {
                VendorRepository rep = new VendorRepository(__dbContext);
                return rep.CustomList(pageSize, pageNo, search);
            }
            else
            {
                CustomerRepository rep = new CustomerRepository(__dbContext);
                return rep.CustomList(pageSize, pageNo, search);
            }
        }

        public object SetParty(TransactionModel model, long FkPartyId)
        {
            var vendor = new PartyModel();
            if(model.ExtProperties.TranAlias == "PINV")
                vendor = GetVendor(FkPartyId);
            else
                vendor = GetCustomer(FkPartyId);

            if (vendor != null)
            { 
                model.PartyAddress = vendor.Address == null ? "" : vendor.Address.ToString();  
                model.PartyName = vendor.Name;
                model.PartyGSTN = vendor.Gstno == null? "" : vendor.Gstno.ToString();
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
                                     FkBranchId= cou.FkBranchId
                                 }
                                )).FirstOrDefault();
            return data;
        }


        public List<ProdLotDtlModel> Get_ProductLotDtlList(int PKProductId,string Batch, string Color)
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
                                              FKUserId = cou.FKUserId,
                                              src = cou.Src,
                                              DateModified = cou.DateModified.ToString("dd-MMM-yyyy"),
                                              DateCreated = cou.DateCreated.ToString("dd-MMM-yyyy"),
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
                data.DateModified = entity.odr.DateModified.ToString("dd-MMM-yyyy");
                data.DateCreated = entity.odr.DateCreated.ToString("dd-MMM-yyyy");
                data.src = entity.odr.Src;
                data.FKUserId = entity.odr.FKUserId;
            }
            return data;
        }

        public List<ColumnStructure> TrandtlColumnList(string TranType)
        {
            var list = new List<ColumnStructure>();
            if (TranType == "P")
            {
                list = new List<ColumnStructure>
            {
               new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Product Name", Fields="ProductName_Text",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="C" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="MRP", Fields="MRP",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Price", Fields="Rate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="QTY", Fields="Qty",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Free Qty", Fields="FreeQty",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Gross Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="GST Rate", Fields="GstRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="GST Amount", Fields="GstAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Net Amount", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Batch", Fields="Batch",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="LN" },
                new ColumnStructure{ pk_Id=11, Orderby= 11, Heading ="Color", Fields="Color",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="LN" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="MfgDate", Fields="MfgDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="D1" },
                //new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="ExpiryDate", Fields="ExpiryDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               // new ColumnStructure{ pk_Id=14, Orderby =14, Heading ="MRP", Fields="MRP",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=15, Orderby =15, Heading ="SaleRate", Fields="SaleRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=16, Orderby =16, Heading ="Action", Fields="Delete",Width=10,IsActive=1, SearchType=0,Sortable=0,CtrlType="" },

            };
            }
            else
            {
                list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Product Name", Fields="ProductName_Text",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="C" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="MRP", Fields="MRP",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Price", Fields="Rate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="QTY", Fields="Qty",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Free Qty", Fields="FreeQty",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Gross Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="GST Rate", Fields="GstRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="GST Amount", Fields="GstAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Net Amount", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Batch", Fields="Batch",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="L" },
                 new ColumnStructure{ pk_Id=11, Orderby= 11, Heading ="Color", Fields="Color",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="L" },
               new ColumnStructure{ pk_Id=16, Orderby =16, Heading ="Action", Fields="Delete",Width=10,IsActive=1, SearchType=0,Sortable=0,CtrlType="" },

            };
            }
            return list.OrderBy(x => x.Orderby).ToList();
        }

       

        public List<ProductModel> ProductList()
        {
            ProductRepository rep = new ProductRepository(__dbContext);
            return rep.GetList(1000, 1);
        }
        public List<SeriesModel> SeriesList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "")
        {
            SeriesRepository rep = new SeriesRepository(__dbContext);
            return rep.GetList(pageSize, pageNo, search, TranAlias);
        }



    }
}
