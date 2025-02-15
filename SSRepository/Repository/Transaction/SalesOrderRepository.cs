using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class SalesOrderRepository : TranBaseRepository, ISalesOrderRepository
    {
        public SalesOrderRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_SalesOrderAddUpd";
            SPList = "usp_SalesOrderList";
            SPById = "usp_SalesOrderById";
        }

        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;
            string error = "";
            return error;

        }


        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var obj = (from cou in __dbContext.TblSalesOrdertrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       orderby cou.PkId descending
                       select new
                       {
                           cou,
                           ser,
                       }).FirstOrDefault();
            if (obj != null)
            {
                model.FKSeriesId = obj.ser.PkSeriesId;
            }
            if (model.FKSeriesId == 0)
            {
                var _entity = (from cou in __dbContext.TblSeriesMas
                               where cou.TranAlias == TranAlias
                               && cou.DocumentType == DocumentType
                               select new
                               {
                                   cou
                               }).FirstOrDefault();
                if (_entity != null)
                {
                    model.FKSeriesId = _entity.cou.PkSeriesId;
                }
            }
            if (model.FKSeriesId != 0)
                SetSeries(model, model.FKSeriesId);
            return model;
        }
        public long GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            var obj = __dbContext.TblSalesOrdertrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("SORD");
            }
            else
            {
                list = new List<ColumnStructure>
                {
                     new ColumnStructure{ pk_Id=1,  Orderby =1,  Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=2,  Orderby =2,  Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=3,  Orderby =3,  Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=4,  Orderby =4,  Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=5,  Orderby =5,  Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=6,  Orderby =6,  Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=7,  Orderby =7,  Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=8,  Orderby =8,  Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=9,  Orderby =9,  Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Remark", Fields="Remark",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Status", Fields="TranStatus",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=14, Orderby =14, Heading ="Schedule Date", Fields="OrderScheduleDt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=15, Orderby =15, Heading ="Concern Person Name", Fields="ConcernPersonName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=16, Orderby =16, Heading ="Concern Person Mobile", Fields="ConcernPersonMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

                };
            }
            return list;
        }

        public void UpdateTrnSatus(long PkId, long FKSeriesId,string TrnStatus)
        {
            var entity = __dbContext.TblSalesOrdertrn.Where(x => x.PkId == PkId && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            entity.TrnStatus = TrnStatus;
            entity.DraftMode = true;
            __dbContext.Update(entity);
            __dbContext.SaveChanges();
        }


        public object FileUpload(TransactionModel model, DataTable dt)
        {
            try
            {
                model.NotFound = "";
                List<string> notFound_List = new List<string>();
                string BillingRate = !string.IsNullOrEmpty(model.BillingRate) ? model.BillingRate : (model.TranAlias == "PORD" || model.TranAlias == "PINV") ? "PurchaseRate" : "SaleRate";
                foreach (DataRow dr in dt.Rows)
                {
                    string ProductName = dr[0].ToString();
                    string Size = dr[1].ToString();
                    int Qty = Convert.ToInt32(dr[2].ToString());

                    DataTable dtProduct = GetProduct("", model.FKLocationID, 0, 0, ProductName, false,model.ExtProperties.TranAlias,model.FkPartyId, model.FKOrderID, model.FKOrderSrID);
                    if (dtProduct.Rows.Count > 0)
                    {

                        //check
                        var detail = new TranDetails();
                        detail.FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
                        detail.FkLotId = Convert.ToInt64(dtProduct.Rows[0]["PkLotId"].ToString()); ;

                        var _old = model.TranDetails.ToList().Where(x => x.FkProductId == detail.FkProductId && x.FkLotId == detail.FkLotId && x.ModeForm != 2 && x.Batch == Size).FirstOrDefault();
                        if (_old == null)
                        {
                            var _checkSrNo = model.TranDetails.ToList().Where(x => x.FkProductId > 0 && x.Qty > 0).ToList();
                            if (_checkSrNo.Count > 0)
                            {
                                detail.SrNo = _checkSrNo.Max(x => x.SrNo) + 1;

                            }
                            else { detail.SrNo = 1; }

                            detail.Barcode = "";
                            detail.FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
                            detail.FkBrandId = Convert.ToInt64(dtProduct.Rows[0]["FkBrandId"].ToString());
                            detail.FKProdCatgId = Convert.ToInt64(dtProduct.Rows[0]["FKProdCatgId"].ToString());
                            detail.Product = dtProduct.Rows[0]["Product"].ToString();
                            detail.CodingScheme = dtProduct.Rows[0]["CodingScheme"].ToString();
                            detail.Qty = Qty;
                            detail.ModeForm = 0;//0=Add,1=Edit,2=Delete 
                            detail.FKLocationID = model.FKLocationID;
                            detail.ReturnTypeID = 2;

                            detail.MRP = Convert.ToDecimal(dtProduct.Rows[0]["MRP"].ToString());
                            detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["SaleRate"].ToString());

                            if (BillingRate == "MRP") { detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["MRP"].ToString()); }
                            if (BillingRate == "SaleRate") { detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["SaleRate"].ToString()); }
                            if (BillingRate == "TradeRate") { detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["TradeRate"].ToString()); }
                            if (BillingRate == "DistributionRate") { detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["DistributionRate"].ToString()); }
                            if (BillingRate == "PurchaseRate") { detail.SaleRate = Convert.ToDecimal(dtProduct.Rows[0]["PurchaseRate"].ToString()); }

                            detail.FkLotId = Convert.ToInt64(dtProduct.Rows[0]["PkLotId"].ToString()); ;
                            detail.Color = dtProduct.Rows[0]["Color"].ToString();
                            detail.Batch = Size;//dtProduct.Rows[0]["Batch"].ToString();
                            detail.FKOrderID = Convert.ToInt64(dtProduct.Rows[0]["FkOrderId"].ToString()); ;
                            detail.FKOrderSrID = Convert.ToInt64(dtProduct.Rows[0]["FKOrderSrID"].ToString()); ;
                            detail.OrderSrNo = Convert.ToInt64(dtProduct.Rows[0]["OrderSrNo"].ToString()); ;

                            if (model.TranAlias == "PORD" || model.TranAlias == "PINV")
                            {
                                detail.FkLotId = 0;
                            }

                            detail.GstRate = (detail.SaleRate < 1000 ? 5 : 18);
                            detail.Rate = Math.Round(Convert.ToDecimal(detail.SaleRate) * (100 / (100 + detail.GstRate)), 2);

                            detail.TradeDisc = 0;
                            detail.TradeDiscAmt = 0;
                            detail.TradeDiscType = "";

                            if (model.FkPartyId > 0 && (model.TranAlias == "SORD" || model.TranAlias == "SINV"))
                            {
                                var _cust = new CustomerRepository(__dbContext, _contextAccessor).GetSingleRecord(model.FkPartyId);
                                detail.TradeDisc = _cust.Disc;

                            }
                            detail.TradeRate = detail.DistributionRate = detail.SaleRate;

                            CalculateExe(model, detail);
                            model.TranDetails.Add(detail);
                        }
                        else
                        {
                            int rowIndex = model.TranDetails.FindIndex(a => a.FkProductId == detail.FkProductId && a.Batch == Size);
                            model.TranDetails[rowIndex].Qty += 1;

                            CalculateExe(model, model.TranDetails[rowIndex]);
                        }



                        SetGridTotal(model);
                        SetPaymentDetail(model);
                    }
                    else { notFound_List.Add(ProductName); }
                }
                model.NotFound = string.Join(",", notFound_List.ToList());
                model.IsTranChange = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " Rep");
            }
            return model;
        }


    }
}
