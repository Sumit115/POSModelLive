﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class PromotionRepository : Repository<TblPromotionMas>, IPromotionRepository
    {
        public PromotionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(PromotionModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Promotion))
            {
                cnt = (from x in __dbContext.TblPromotionMas
                       where x.PromotionName == model.Promotion && x.PkPromotionId != model.PkPromotionId
                       select x).Count();
                if (cnt > 0)
                    error = "Name Already Exits";
            }

            return error;
        }

        public List<PromotionModel> GetList(int pageSize, int pageNo = 1, string PromotionDuring = "")
        {
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<PromotionModel> data = (from cou in __dbContext.TblPromotionMas
                                             //join catGrp in __dbContext.TblPromotionGroupMas on cou.FkPromotionGroupId equals catGrp.PkPromotionGroupId
                                         where cou.PromotionDuring == PromotionDuring
                                         //&& (PromotionGroupId == 0 || cou.FkPromotionGroupId == PromotionGroupId)
                                         orderby cou.PkPromotionId
                                         select (new PromotionModel
                                         {
                                             PkPromotionId = cou.PkPromotionId,
                                             FKUserId = cou.FKUserID,
                                             FKCreatedByID = cou.FKCreatedByID,
                                             ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                             CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                                             PromotionDuring = cou.PromotionDuring,
                                             PromotionName = cou.PromotionName,
                                             PromotionFromDt = cou.PromotionFromDt,
                                             PromotionToDt = cou.PromotionToDt,
                                             PromotionFromTime = cou.PromotionFromTime,
                                             PromotionToTime = cou.PromotionToTime,
                                             FKLocationId = cou.FKLocationId,
                                             FkVendorId = cou.FkVendorId,
                                             FkCustomerId = cou.FkCustomerId,
                                             FkReferById = cou.FkReferById,
                                             PromotionApplyOn = cou.PromotionApplyOn,
                                             Promotion = cou.Promotion,
                                             PromotionApplyAmt = cou.PromotionApplyAmt,
                                             PromotionApplyQty = cou.PromotionApplyQty,
                                             FkPromotionApplyUnitId = cou.FkPromotionApplyUnitId,
                                             FKLotID = cou.FKLotID,
                                             FkPromotionProdId = cou.FkPromotionProdId,
                                             PromotionAmt = cou.PromotionAmt,
                                             PromotionQty = cou.PromotionQty,
                                             FkPromotionUnitId = cou.FkPromotionUnitId,
                                             FKProdID = cou.FKProdID,
                                             FkProdCatgId = cou.FkProdCatgId,
                                             FkBrandId = cou.FkBrandId,
                                             //FkPromotionGroupId = cou.FkPromotionGroupId,
                                             //GroupName = catGrp.PromotionGroupName,
                                         }
                                        )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public PromotionModel GetSingleRecord(long PkPromotionId)
        {
            
            PromotionModel data = new PromotionModel();
            data = (from cou in __dbContext.TblPromotionMas
                    join _prd in __dbContext.TblProductMas on cou.FKProdID equals (int?)_prd.PkProductId into _prdmp
                    from prd in _prdmp.DefaultIfEmpty()
                    join _cat in __dbContext.TblCategoryMas on cou.FkProdCatgId equals (int?)_cat.PkCategoryId into _catmp
                    from cat in _catmp.DefaultIfEmpty()
                    join _brand in __dbContext.TblBrandMas on cou.FkBrandId equals (int?)_brand.PkBrandId into _brandmp
                    from brand in _brandmp.DefaultIfEmpty()

                    join _location in __dbContext.TblLocationMas on cou.FKLocationId equals (int?)_location.PkLocationID into _locationmp
                    from location in _locationmp.DefaultIfEmpty()
                    join _cust in __dbContext.TblCustomerMas on cou.FkCustomerId equals (int?)_cust.PkCustomerId into _custmp
                    from cust in _custmp.DefaultIfEmpty()
                    join _vendor in __dbContext.TblVendorMas on cou.FkVendorId equals (int?)_vendor.PkVendorId into _vendormp
                    from vendor in _vendormp.DefaultIfEmpty()

                    join _freePrd in __dbContext.TblProductMas on cou.FkPromotionProdId equals (int?)_freePrd.PkProductId into _freePrdmp
                    from freePrd in _freePrdmp.DefaultIfEmpty()


                    where cou.PkPromotionId == PkPromotionId
                    select (new PromotionModel
                    {
                        PkPromotionId = cou.PkPromotionId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                        PromotionDuring = cou.PromotionDuring,
                        PromotionName = cou.PromotionName,
                        PromotionFromDt = cou.PromotionFromDt,
                        PromotionToDt = cou.PromotionToDt,
                        PromotionFromTime = cou.PromotionFromTime,
                        PromotionToTime = cou.PromotionToTime,
                        FKLocationId = cou.FKLocationId,
                        FkVendorId = cou.FkVendorId,
                        FkCustomerId = cou.FkCustomerId,
                        FkReferById = cou.FkReferById,
                        PromotionApplyOn = cou.PromotionApplyOn,
                        Promotion = cou.Promotion,
                        PromotionApplyAmt = cou.PromotionApplyAmt,
                        PromotionApplyQty = cou.PromotionApplyQty,
                        FkPromotionApplyUnitId = cou.FkPromotionApplyUnitId,
                        FKLotID = cou.FKLotID,
                        FkPromotionProdId = cou.FkPromotionProdId,
                        PromotionAmt = cou.PromotionAmt,
                        PromotionQty = cou.PromotionQty,
                        FkPromotionUnitId = cou.FkPromotionUnitId,
                        FKProdID = cou.FKProdID,
                        FkProdCatgId = cou.FkProdCatgId,
                        FkBrandId = cou.FkBrandId,
                        //FkPromotionGroupId = cou.FkPromotionGroupId,
                        //PromotionSize_lst = (from ad in __dbContext.TblPromotionSizeLnk
                        //                        //  join loc in __dbContext.TblBranchMas on ad.FKLocationID equals loc.PkBranchId
                        //                    where (ad.FkPromotionId == cou.PkPromotionId)
                        //                    select (new PromotionSizeLnkModel
                        //                    {
                        //                        PkId = ad.PkId,
                        //                        Size = ad.Size,
                        //                        FkPromotionId = ad.FkPromotionId,
                        //                    })).ToList(),
                        ProductName = prd.Product,
                        CategoryName = cat.CategoryName,
                        BrandName=brand.BrandName,
                       CustomerName = cust.Name ,
                       VendorName = vendor.Name,
                       LocationName=location.Location,
                       PromotionProductName=freePrd.Product,
                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkPromotionId)
        {
            string Error = "";
            PromotionModel obj = GetSingleRecord(PkPromotionId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkPromotionId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblPromotionMas
                           where x.PkPromotionId == PkPromotionId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblPromotionMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkPromotionId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkPromotionId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetPromotionID(), PkPromotionId, obj.FKPromotionID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            PromotionModel model = (PromotionModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            PromotionModel model = (PromotionModel)objmodel;
            TblPromotionMas Tbl = new TblPromotionMas();
            if (model.PkPromotionId > 0)
            {
                var _entity = __dbContext.TblPromotionMas.Find(model.PkPromotionId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkPromotionId = model.PkPromotionId;
            Tbl.PromotionDuring = model.PromotionDuring;
            Tbl.PromotionName = model.PromotionName;
            Tbl.PromotionFromDt = model.PromotionFromDt;
            Tbl.PromotionToDt = model.PromotionToDt;
            Tbl.PromotionFromTime = model.PromotionFromTime;
            Tbl.PromotionToTime = model.PromotionToTime;
            Tbl.FKLocationId = model.FKLocationId > 0 ? model.FKLocationId : null;
            Tbl.FkVendorId = model.FkVendorId > 0 ? model.FkVendorId : null;
            Tbl.FkCustomerId = model.FkCustomerId > 0 ? model.FkCustomerId : null;
            Tbl.FkReferById = model.FkReferById > 0 ? model.FkReferById : null;
            Tbl.PromotionApplyOn = model.PromotionApplyOn;
            Tbl.Promotion = model.Promotion;
            Tbl.PromotionApplyAmt = model.PromotionApplyAmt;
            Tbl.PromotionApplyQty = model.PromotionApplyQty;
            Tbl.FkPromotionApplyUnitId = model.FkPromotionApplyUnitId > 0 ? model.FkPromotionApplyUnitId : null;
            Tbl.FKLotID = model.FKLotID > 0 ? model.FKLotID : null;
            Tbl.FkPromotionProdId = model.FkPromotionProdId > 0 ? model.FkPromotionProdId : null;
            Tbl.PromotionAmt = model.PromotionAmt;
            Tbl.PromotionQty = model.PromotionQty;
            Tbl.FkPromotionUnitId = model.FkPromotionUnitId > 0 ? model.FkPromotionUnitId : null;
            Tbl.FKProdID = model.FKProdID > 0 ? model.FKProdID : null;
            Tbl.FkProdCatgId = model.FkProdCatgId > 0 ? model.FkProdCatgId : null;
            Tbl.FkBrandId = model.FkBrandId > 0 ? model.FkBrandId : null;
            Tbl.ModifiedDate = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.FKUserID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                Tbl.PkPromotionId = getIdOfSeriesByEntity("PkPromotionId", null, Tbl, "TblPromotionMas");
                AddData(Tbl, false);
            }
            else
            {

                PromotionModel oldModel = GetSingleRecord(Tbl.PkPromotionId);
                ID = Tbl.PkPromotionId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKPromotionID, oldModel.PkPromotionId, oldModel.FKPromotionID, oldModel.DATE_MODIFIED);
            }


            //AddImagesAndRemark(obj.PkcountryId, obj.FKPromotionID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Name", Fields="PromotionName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="From Date", Fields="PromotionFromDt_str",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="To Date", Fields="PromotionToDt_str",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="From Time", Fields="PromotionFromTime_str",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="To Time", Fields="PromotionToTime_str",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Apply On", Fields="PromotionApplyOn",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Promotion", Fields="Promotion",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },


            };
            if (GridName.ToString() == "S")
            {
                list.Add(new ColumnStructure { pk_Id = 20, Orderby = 20, Heading = "Customer", Fields = "CustomerName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else if (GridName.ToString() == "P")
            {
                list.Add(new ColumnStructure { pk_Id = 20, Orderby = 20, Heading = "Vendor", Fields = "VendorName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            return list;
        }


    }
}


















