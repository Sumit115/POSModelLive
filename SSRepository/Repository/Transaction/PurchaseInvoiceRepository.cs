﻿using Microsoft.EntityFrameworkCore;
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

namespace SSRepository.Repository.Transaction
{
    public class PurchaseInvoiceRepository : TranBaseRepository, IPurchaseInvoiceRepository
    {
        public PurchaseInvoiceRepository(AppDbContext dbContext) : base(dbContext)
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
                    { error = "Barcode Already exists :"+ string.Join(",", _exists.Select(x => x.Barcode).ToList()); }
                
                }
            }

            return error;
        }
        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var obj = (from cou in __dbContext.TblPurchaseInvoicetrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       join branch in __dbContext.TblBranchMas on ser.FkBranchId equals branch.PkBranchId
                       join location in __dbContext.TblLocationMas on ser.FKLocationID equals location.PkLocationID
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                        && ser.DocumentType == DocumentType
                       orderby cou.PkId descending
                       select new
                       {
                           cou,
                           ser,
                           branch,
                           location,
                       }).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ser != null)
                {
                    model.SeriesName = obj.ser.Series == null ? "" : obj.ser.Series.ToString();
                    model.FKLocationID = obj.location.PkLocationID;
                    model.FKSeriesId = obj.ser.PkSeriesId;
                    model.BillingRate = obj.ser.BillingRate;
                    model.BranchStateName = obj.location.State;

                }
            }
            if (model.FKSeriesId == 0)
            {
                var _entity = (from cou in __dbContext.TblSeriesMas
                               join branch in __dbContext.TblBranchMas on cou.FkBranchId equals branch.PkBranchId
                               join location in __dbContext.TblLocationMas on cou.FKLocationID equals location.PkLocationID
                               where cou.TranAlias == TranAlias
                               select new
                               {
                                   cou,
                                   branch,
                                   location
                               }).FirstOrDefault();
                if (_entity != null)
                {
                    model.SeriesName = _entity.cou.Series == null ? "" : _entity.cou.Series.ToString();
                    model.FKLocationID = _entity.location.PkLocationID;
                    model.FKSeriesId = _entity.cou.PkSeriesId;
                    model.BillingRate = _entity.cou.BillingRate;
                    model.BranchStateName = _entity.location.State;
                }
            }
            return model;
        }

        public long GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            var obj = __dbContext.TblPurchaseInvoicetrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
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
                list = new List<ColumnStructure>
            {
                 new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="#", Fields="sno",Width=5,IsActive=1, SearchType=0,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Entry Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Entry No", Fields="EntryNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Vendor", Fields="PartyName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Gross", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Tax", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Discount", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="RoundOf", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Shipping", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Net Amount", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

            };
            }
            return list;
        }



    }
}
