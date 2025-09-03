using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using SSRepository.Repository.Master;
using System.Xml.Linq;
using System.Runtime.ConstrainedExecution;
using System;
using System.Diagnostics;
using SSRepository.IRepository;

namespace SSRepository.Repository.Transaction
{
    public class PurchaseInvoiceRepository : TranBaseRepository, IPurchaseInvoiceRepository
    {
        public PurchaseInvoiceRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_PurchaseInvoiceAddUpd";
            SPList = "usp_PurchaseInvoiceList";
            SPById = "usp_PurchaseInvoiceById";
        }

        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;
            string error = "";


            error = isAlreadyExist(model, "");
            return error;

        }
        public string isAlreadyExist(TransactionModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (model.UniqIdDetails != null)
            {
                if (model.PkId > 0)
                {
                    var _exists = __dbContext.TblProductQTYBarcode.Where(x => (x.TranInId != model.PkId && x.TranInSeriesId != model.FKSeriesId)).ToList().Where(x => model.UniqIdDetails.Any(y => y.Barcode == x.Barcode)).ToList();
                    if (_exists.Count > 0)
                    { error = "Barcode Already exists :" + string.Join(",", _exists.Select(x => x.Barcode).ToList()); }

                }
                else
                {
                    var _exists = __dbContext.TblProductQTYBarcode.ToList().Where(x => model.UniqIdDetails.Any(y => y.Barcode == x.Barcode)).ToList();
                    if (_exists.Count > 0)
                    { error = "Barcode Already exists :" + string.Join(",", _exists.Select(x => x.Barcode).ToList()); }

                }
            }

            return error;
        }
        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
            var obj = (from cou in __dbContext.TblPurchaseInvoicetrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       && BillingLocation.Contains(ser.FKLocationID.ToString())
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
                               where cou.TranAlias == TranAlias && cou.DocumentType == DocumentType
                               && BillingLocation.Contains(cou.FKLocationID.ToString())
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
            var obj = __dbContext.TblPurchaseInvoicetrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }

        public List<TranDetails> Get_ProductInfo_FromFile(string filePath, List<string> validationErrors)
        {
            List<TranDetails> tranList = new List<TranDetails>();

            DataTable dt = new DataTable();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Trim());
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i].Trim();
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
                    string error = "";
                    int srNo = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        srNo++;

                        if (!int.TryParse(dr["Qty"]?.ToString().Trim(), out int qty))
                            error += $" Qty is zero.";

                        if (!decimal.TryParse(dr["MRP"]?.ToString().Trim(), out decimal mrp))
                            error += $" MRP is zero.";


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
                            error += $" Duplicate Barcode. ";
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
                            });
                        }
                    }
                }
                else
                    throw new Exception("Please Update CodingScheme From System Default");


                if (validationErrors.Count == 0)
                {
                    tranList = new List<TranDetails>();
                    int srNo = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        srNo++;
                        int qty = Convert.ToInt32(dr["Qty"].ToString());
                        decimal mrp = Convert.ToDecimal(dr["MRP"].ToString());
                        string Product = dr["Artical"]?.ToString().Trim();

                        var item = new TranDetails
                        {
                            SrNo = srNo,
                            Barcode = dr["Barcode"]?.ToString().Trim(),
                            Product = Product,
                            ProductDisplay = Product,
                            Batch = dr["Size"]?.ToString().Trim(),
                            Color = dr["Color"]?.ToString().Trim(),
                            Qty = qty,
                            MRP = mrp,
                            SaleRate = mrp,
                            TradeRate = mrp,
                            DistributionRate = mrp,
                            FkLotId = 0
                        };
                        var _exists = tranList.Where(x => x.Product == Product && x.FkProductId > 0).FirstOrDefault();
                        if (_exists != null)
                        {
                            item.FkProductId = _exists.FkProductId;
                            item.FkBrandId = _exists.FkBrandId;
                            item.FKProdCatgId = _exists.FKProdCatgId;
                            item.SubCategoryName = _exists.SubCategoryName;
                            item.Product = item.ProductDisplay = _exists.Product;
                            item.CodingScheme = _exists.CodingScheme;
                        }
                        else
                        {
                            DataTable dtProduct = GetProduct("", 0, 0, 0, dr["Artical"]?.ToString(), false, "");
                            if (dtProduct.Rows.Count > 0)
                            {

                                item.FkProductId = Convert.ToInt64(dtProduct.Rows[0]["PkProductId"].ToString());
                                item.FkBrandId = Convert.ToInt64(dtProduct.Rows[0]["FkBrandId"].ToString());
                                item.FKProdCatgId = Convert.ToInt64(dtProduct.Rows[0]["FKProdCatgId"].ToString());
                                item.SubCategoryName = dtProduct.Rows[0]["CategoryName"].ToString();
                                item.Product = dtProduct.Rows[0]["Product"].ToString();
                                item.CodingScheme = dtProduct.Rows[0]["CodingScheme"].ToString();
                            }
                            else
                            {
                                var Category = __dbContext.TblCategoryMas.Where(x => x.CategoryName == dr["SubSection"].ToString()).SingleOrDefault();
                                if (Category != null)
                                {
                                    item.FKProdCatgId = Category.PkCategoryId;
                                    item.SubCategoryName = Category.CategoryName;
                                }
                                else
                                    validationErrors.Add($"Row {srNo} Category : {dr["SubSection"].ToString()}");

                            }
                        }
                        tranList.Add(item);

                    }
                }
            }
            else
                throw new Exception("Invalid Data");
            return tranList;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("P");
            }
            else
            {
                int index = 1;
                int Orderby = 1;
                list = new List<ColumnStructure>
            {
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="#", Fields="sno",Width=5,IsActive=1, SearchType=0,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Entry Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Entry No", Fields="EntryNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Vendor", Fields="PartyName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Gross", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="GrossAmt"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Tax", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="TaxAmt"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Discount", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="TotalDiscount"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="RoundOf", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="RoundOfDiff"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Shipping", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="Shipping"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Net Amount", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="NetAmt"},

            };
            }
            return list;
        }



    }
}
