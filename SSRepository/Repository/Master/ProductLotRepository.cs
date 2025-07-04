﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class ProductLotRepository : Repository<TblProdLotDtl>, IProductLotRepository
    {
        public ProductLotRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(ProdLotDtlModel model, string Mode)
        {
            dynamic cnt;
            string error = "";

            return error;
        }

        public List<ProdLotDtlModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProdLotDtlModel> data = (from cou in __dbContext.TblProdLotDtl
                                          join prd in __dbContext.TblProductMas on cou.FKProductId equals prd.PkProductId
                                          join cat in __dbContext.TblCategoryMas on prd.FKProdCatgId equals cat.PkCategoryId
                                          join Pbrand in __dbContext.TblBrandMas on prd.FkBrandId equals Pbrand.PkBrandId
                                                                into tembrand
                                          from brand in tembrand.DefaultIfEmpty()
                                              // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                          orderby cou.PkLotId
                                          select (new ProdLotDtlModel
                                          {
                                              PkLotId = cou.PkLotId,
                                              //FKUserId = cou.FKUserID,
                                              //
                                              //ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                              //CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
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
                                              ProductName = prd.Product
                                          }
                                         )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public List<ProdLotDtlModel> GetListByProduct(long FkProductId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            // var data = new List<ProdLotDtlModel>();
            var data = (from cou in __dbContext.TblProdLotDtl
                        join prd in __dbContext.TblProductMas on cou.FKProductId equals prd.PkProductId
                        join cat in __dbContext.TblCategoryMas on prd.FKProdCatgId equals cat.PkCategoryId
                        //  join catgrop in __dbContext.TblCategoryGroupMas on prd.FkCatGroupId equals catgrop.PkCategoryGroupId
                        join Pbrand in __dbContext.TblBrandMas on prd.FkBrandId equals Pbrand.PkBrandId
                                              into tembrand
                        from brand in tembrand.DefaultIfEmpty()
                        //join Pstock in __dbContext.TblProdStockDtl on cou.FKProductId equals Pstock.FKProductId
                        //                    into temstock
                        //from stock in temstock.DefaultIfEmpty()

                        join Pstock in __dbContext.TblProdStockDtl on new { cou.FKProductId, cou.PkLotId } equals new { Pstock.FKProductId, PkLotId= Pstock.FKLotID }
                                         into temstock
                        from stock in temstock.DefaultIfEmpty()
                        where cou.FKProductId == FkProductId
                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                        // orderby cou.PkLotId
                        select new ProdLotDtlModel
                        {
                            PkLotId = cou.PkLotId,
                            //FKUserId = cou.FKUserID,
                            //
                            //ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                            //CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                            FKProductId = cou.FKProductId,
                            LotAlias = cou.LotAlias,
                            LotName = cou.LotName,
                            LotNo = cou.LotNo,
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
                            ProductName = prd.Product,
                            CurStock = stock.CurStock, 
                            DATE_MODIFIED = cou.StockDate.Value.ToString("dd-MMM-yyyy")
                        }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public ProdLotDtlModel GetSingleRecord(long PkLotId)
        {

            ProdLotDtlModel data = new ProdLotDtlModel();
            data = (from cou in __dbContext.TblProdLotDtl
                    where cou.PkLotId == PkLotId
                    select (new ProdLotDtlModel
                    {
                        PkLotId = cou.PkLotId,
                        //FKUserId = cou.FKUserID,
                        //
                        //ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        //CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                        FKProductId = cou.FKProductId,
                        LotAlias = cou.LotAlias,
                        LotName = cou.LotName,
                        LotNo = cou.LotNo,
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
                        //ProductName = prd.Product
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpProdLotDtl(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);
            result.Insert(0, new ProdLotDtlModel { PkLotId = 0, Batch = "Select" });


            return (from r in result
                    select new
                    {
                        r.PkLotId,
                        r.Batch
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkLotId)
        {
            string Error = "";
            ProdLotDtlModel obj = GetSingleRecord(PkLotId);


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblProdLotDtl
                           where x.PkLotId == PkLotId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblProdLotDtl.RemoveRange(lst);

                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            ProdLotDtlModel model = (ProdLotDtlModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            ProdLotDtlModel model = (ProdLotDtlModel)objmodel;
            TblProdLotDtl Tbl = new TblProdLotDtl();
            if (model.PkLotId > 0)
            {
                var _entity = __dbContext.TblProdLotDtl.Find(model.PkLotId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkLotId = model.PkLotId;
            Tbl.FKProductId = model.FKProductId;
            Tbl.LotAlias = model.LotAlias;
            Tbl.LotName = model.LotName;
            Tbl.LotNo = model.LotNo;
            Tbl.Barcode = model.Barcode;
            Tbl.Batch = model.Batch;
            Tbl.Color = model.Color;
            Tbl.MfgDate = model.MfgDate;
            Tbl.ExpiryDate = model.ExpiryDate;
            Tbl.ProdConv1 = model.ProdConv1;
            Tbl.MRP = model.MRP;
            Tbl.LtExtra = model.LtExtra;
            Tbl.AddLT = model.AddLT;
            Tbl.SaleRate = model.SaleRate;
            Tbl.PurchaseRate = model.PurchaseRate;
            Tbl.FkmfgGroupId = model.FkmfgGroupId;
            Tbl.TradeRate = model.TradeRate;
            Tbl.DistributionRate = model.DistributionRate;
            Tbl.PurchaseRateUnit = model.PurchaseRateUnit;
            Tbl.MRPSaleRateUnit = model.MRPSaleRateUnit;
            Tbl.InTrnId = model.InTrnId;
            Tbl.InTrnFKSeriesID = model.InTrnFKSeriesID;
            Tbl.InTrnsno = model.InTrnsno;
            Tbl.Remarks = model.Remarks;
            //Tbl.FKUserID = model.FKUserId;
            //Tbl.ModifiedDate= DateTime.Now;
            if (Mode == "Create")
            {
                //Tbl.FKCreatedByID = model.FKCreatedByID;
                //Tbl.FKUserID = model.FKUserId;
                //Tbl.CreationDate = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                ProdLotDtlModel oldModel = GetSingleRecord(Tbl.PkLotId);
                ID = Tbl.PkLotId;
                UpdateData(Tbl, false);
                //AddMasterLog((long)Handler.Form.ProductLot, Tbl.PkLotId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.LotName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKProductLotID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>
            {
                 //new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Article Name", Fields="LotName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                 // new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Article No", Fields="LotNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Barcode", Fields="Barcode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                //  new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Alias", Fields="LotAlias",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Size", Fields="Batch",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Color ", Fields="Color",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="MRP ", Fields="MRP",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="SaleRate", Fields="SaleRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="PurchaseRate ", Fields="PurchaseRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="TradeRate", Fields="TradeRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Distribution Rate", Fields="DistributionRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="F.2" },
                 //new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Purchase Rate Unit", Fields="PurchaseRateUnit",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 //new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="MRPSale Rate Unit ", Fields="MRPSaleRateUnit",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Remark", Fields="Remarks",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                 //new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Cur. Stock", Fields="CurStock",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

            };
            return list;
        }
        public string UpdateProdLotDtl(long PkLotId, long FKProductId, string ColumnName, decimal Value)
        {
            string error = "";
            try
            {
                string Qry = "Update tblProdLot_dtl set " + ColumnName + "='" + Value + "' where PkLotId=" + PkLotId + " and FKProductId=" + FKProductId + "";
                ExecuteQuery(Qry, ref error);
            }
            catch (Exception ex) { error = ex.Message; }
            return error;
        }


    }
}



















