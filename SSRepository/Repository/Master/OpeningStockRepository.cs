using Azure;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Repository.Master
{
    public class OpeningStockRepository: Repository<TblProdStockDtl>, IOpeningStockRepository
    {
        public OpeningStockRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public string isAlreadyExist(TblProdStockDtlModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            //if (!string.IsNullOrEmpty(model.PKStockId))
            //{
            //    cnt = (from x in __dbContext.TblProductMas
            //           where x.Product == model.Product && x.PkProductId != model.PkProductId
            //           select x).Count();
            //    if (cnt > 0)
            //        error = "Already Exits";

            return error;
        }

        public List<TblProdStockDtlModel> GetList(long FkProdId, int pageSize, int pageNo = 1, string search = "")
        {

            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<TblProdStockDtlModel> data = (from cou in __dbContext.TblProdStockDtl
                                               join cat in __dbContext.TblProductMas on cou.FKProductId equals cat.PkProductId
                                       join Pbrand in __dbContext.TblProdLotDtl on cou.FKLotID equals Pbrand.PkLotId
                                                             into tembrand
                                       from brand in tembrand.DefaultIfEmpty()
                                       join tbllc in __dbContext.TblLocationMas on cou.FKLocationId equals tbllc.PkLocationID
                                               where cou.FKProductId== FkProdId// && cou.FKLotID==FkLotId 
                                       orderby cou.PkstockId
                                       select (new TblProdStockDtlModel
                                       {
                                           PKStockId = cou.PkstockId,
                                           FKProdID = cou.FKProductId,
                                           FKLocationID = cou.FKLocationId,
                                           FKLotID = cou.FKLotID,
                                           OpStock = cou.OpStock,
                                           InStock = cou.InStock,
                                           OutStock = cou.OutStock,
                                           CurStock = cou.CurStock,
                                           StockDate = cou.StockDate,
                                           LocationName= tbllc.Location
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public TblProdStockDtlModel GetSingleRecord(long PKStockId)
        {
            TblProdStockDtlModel data = new TblProdStockDtlModel();
            data = (from cou in __dbContext.TblProdStockDtl
                    where cou.PkstockId == PKStockId
                    select (new TblProdStockDtlModel
                    {
                        PKStockId = cou.PkstockId,
                        FKProdID = cou.FKProductId,
                        FKLocationID = cou.FKLocationId,
                        FKLotID = cou.FKLotID,
                        OpStock = cou.OpStock,
                        InStock = cou.InStock,
                        OutStock = cou.OutStock,
                        CurStock = cou.CurStock,
                        StockDate= cou.StockDate
                    })).FirstOrDefault();

            return data;
        }

        public override string ValidateData(object objmodel, string Mode)
        {

            TblProdStockDtlModel model = (TblProdStockDtlModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }

        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            TblProdStockDtlModel model = (TblProdStockDtlModel)objmodel;
            TblProdStockDtl Tbl = new TblProdStockDtl();
            if (model.PKStockId > 0)
            {
                var _entity = __dbContext.TblProdStockDtl.Find(model.PKStockId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }
            Tbl.PkstockId = model.PKStockId;
            Tbl.FKProductId = model.FKProdID;
            Tbl.FKLocationId = model.FKLocationID;
            Tbl.FKLotID = model.FKLotID;
            Tbl.OpStock = model.OpStock;
            Tbl.InStock = model.InStock;
            Tbl.OutStock = model.OutStock;
            Tbl.CurStock = model.CurStock;
            Tbl.StockDate = DateTime.Now;
            if (Mode == "Create")
            {
                AddData(Tbl, false);
            }
            else
            {

                TblProdStockDtlModel oldModel = GetSingleRecord(Tbl.PkstockId);
                ID = Tbl.PkstockId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKProductID, oldModel.PkProductId, oldModel.FKProductID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKProductID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }

        public string DeleteRecord(long PKID)
        {
            //not implemented till
            return "";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                 new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="PKStockId", Fields="PKStockId",Width=10,IsActive=0, SearchType=0,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="FKProdID", Fields="FKProductId",Width=10,IsActive=0, SearchType=0,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="FKLocationId", Fields="FKLocationId",Width=10,IsActive=0, SearchType=0,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="FKLotID", Fields="FKLotID",Width=10,IsActive=0, SearchType=0,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="StockDate", Fields="StockDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="OpStock ", Fields="OpStock",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="InStock ", Fields="InStock",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="OutStock ", Fields="OutStock",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="CurStock ", Fields="CurStock",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="AdjStock ", Fields="AdjStock",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="LocationName ", Fields="LocationName",Width=11,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
            };
            return list;
        }


        public string GetByLocationId(Int64 FkLcoationId,Int64 FkLotId)
        {
            string returndata = string.Empty;

            List<TblProdStockDtlModel> data = (from cou in __dbContext.TblProdStockDtl
                                               join cat in __dbContext.TblProductMas on cou.FKProductId equals cat.PkProductId
                                               join Pbrand in __dbContext.TblProdLotDtl on cou.FKLotID equals Pbrand.PkLotId
                                                                     into tembrand
                                               from brand in tembrand.DefaultIfEmpty()
                                               join tbllc in __dbContext.TblLocationMas on cou.FKLocationId equals tbllc.PkLocationID
                                               where brand.PkLotId == FkLotId && cou.FKLocationId== FkLcoationId
                                               orderby cou.PkstockId
                                               select (new TblProdStockDtlModel
                                               {
                                                   PKStockId = cou.PkstockId,
                                                   FKProdID = cou.FKProductId,
                                                   FKLocationID = cou.FKLocationId,
                                                   FKLotID = cou.FKLotID,
                                                   OpStock = cou.OpStock,
                                                   InStock = cou.InStock,
                                                   OutStock = cou.OutStock,
                                                   CurStock = cou.CurStock,
                                                   StockDate = cou.StockDate,
                                                   LocationName = tbllc.Location
                                               }
                                              )).ToList();

            //var data = __dbContext.TblProdStockDtl.Where(x => x.FKLocationId == FkLcoationId && x.FKLotID== FkLotId ).ToList();
        
            return JsonConvert.SerializeObject(data);   
        }
    }
}
