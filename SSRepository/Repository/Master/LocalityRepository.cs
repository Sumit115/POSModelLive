﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class LocalityRepository : Repository<TblLocalityMas>, ILocalityRepository
    {
        public LocalityRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(LocalityModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.LocalityName))
            {
                cnt = (from x in __dbContext.TblLocalityMas
                       where x.LocalityName == model.LocalityName && x.PkLocalityId != model.PkLocalityId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<LocalityModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<LocalityModel> data = (from cou in __dbContext.TblLocalityMas
                                        join catGrp in __dbContext.TblAreaMas on cou.FkAreaId equals catGrp.PkAreaId

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkLocalityId
                                        select (new LocalityModel
                                        {
                                            PkLocalityId = cou.PkLocalityId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            LocalityName = cou.LocalityName,
                                            Description = cou.Description,
                                            FkAreaId = cou.FkAreaId,
                                            AreaName = catGrp.AreaName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<LocalityModel> GetListByGroupId(long AreaId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<LocalityModel> data = (from cou in __dbContext.TblLocalityMas
                                        join catGrp in __dbContext.TblAreaMas on cou.FkAreaId equals catGrp.PkAreaId
                                        where cou.FkAreaId == AreaId
                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkLocalityId
                                        select (new LocalityModel
                                        {
                                            PkLocalityId = cou.PkLocalityId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            LocalityName = cou.LocalityName,
                                            Description = cou.Description,
                                            FkAreaId = cou.FkAreaId,
                                            AreaName = catGrp.AreaName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public LocalityModel GetSingleRecord(long PkLocalityId)
        {

            LocalityModel data = new LocalityModel();
            data = (from cou in __dbContext.TblLocalityMas
                    join catGrp in __dbContext.TblAreaMas on cou.FkAreaId equals catGrp.PkAreaId
                    where cou.PkLocalityId == PkLocalityId
                    select (new LocalityModel
                    {
                        PkLocalityId = cou.PkLocalityId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        LocalityName = cou.LocalityName,
                        Description = cou.Description,
                        FkAreaId = cou.FkAreaId,
                        AreaName = catGrp.AreaName,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpLocality(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new LocalityModel { PkLocalityId = 0, LocalityName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkLocalityId,
                        r.LocalityName
                    }).ToList();
        }
        public object GetDrpLocalityByAreaId(long AreaId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(AreaId, pagesize, pageno, search);

            result.Insert(0, new LocalityModel { PkLocalityId = 0, LocalityName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkLocalityId,
                        r.LocalityName
                    }).ToList();
        }
        public object GetDrpTableLocality(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkLocalityId,
                        Locality = r.LocalityName,
                        r.Description,
                        Area = r.AreaName,
                    }).ToList();
        }
        public string DeleteRecord(long PkLocalityId)
        {
            string Error = "";
            LocalityModel obj = GetSingleRecord(PkLocalityId);

            //var Country = (from x in _context.TblAreaMas
            //               where x.FkcountryId == PkLocalityId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  AreaMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblLocalityMas
                           where x.PkLocalityId == PkLocalityId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblLocalityMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkLocalityId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkLocalityId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetLocalityID(), PkLocalityId, obj.FKLocalityID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            LocalityModel model = (LocalityModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            LocalityModel model = (LocalityModel)objmodel;
            TblLocalityMas Tbl = new TblLocalityMas();
            if (model.PkLocalityId > 0)
            {
                var _entity = __dbContext.TblLocalityMas.Find(model.PkLocalityId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkLocalityId = model.PkLocalityId;
            Tbl.LocalityName = model.LocalityName;
            Tbl.FkAreaId = model.FkAreaId;
            Tbl.Description = model.Description;
            Tbl.ModifiedDate= DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.FKUserID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                LocalityModel oldModel = GetSingleRecord(Tbl.PkLocalityId);
                ID = Tbl.PkLocalityId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKLocalityID, oldModel.PkLocalityId, oldModel.FKLocalityID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKLocalityID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Area", Fields="AreaName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Locality", Fields="LocalityName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Description", Fields="Description",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















