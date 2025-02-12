using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class RegionRepository : Repository<TblRegionMas>, IRegionRepository
    {
        public RegionRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(RegionModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.RegionName))
            {
                cnt = (from x in __dbContext.TblRegionMas
                       where x.RegionName == model.RegionName && x.PkRegionId != model.PkRegionId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<RegionModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<RegionModel> data = (from cou in __dbContext.TblRegionMas
                                        join catGrp in __dbContext.TblZoneMas on cou.FkZoneId equals catGrp.PkZoneId

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkRegionId
                                        select (new RegionModel
                                        {
                                            PkRegionId = cou.PkRegionId,
                                            RegionName = cou.RegionName,
                                            Description = cou.Description,
                                            FkZoneId = cou.FkZoneId,
                                            ZoneName = catGrp.ZoneName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<RegionModel> GetListByGroupId(long ZoneId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<RegionModel> data = (from cou in __dbContext.TblRegionMas
                                        join catGrp in __dbContext.TblZoneMas on cou.FkZoneId equals catGrp.PkZoneId
                                        where cou.FkZoneId == ZoneId
                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkRegionId
                                        select (new RegionModel
                                        {
                                            PkRegionId = cou.PkRegionId,
                                            RegionName = cou.RegionName,
                                            Description = cou.Description,
                                            FkZoneId = cou.FkZoneId,
                                            ZoneName = catGrp.ZoneName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public RegionModel GetSingleRecord(long PkRegionId)
        {

            RegionModel data = new RegionModel();
            data = (from cou in __dbContext.TblRegionMas
                    join catGrp in __dbContext.TblZoneMas on cou.FkZoneId equals catGrp.PkZoneId
                    where cou.PkRegionId == PkRegionId
                    select (new RegionModel
                    {
                        PkRegionId = cou.PkRegionId,
                        RegionName = cou.RegionName,
                        Description = cou.Description,
                        FkZoneId = cou.FkZoneId,
                        ZoneName = catGrp.ZoneName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpRegion(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new RegionModel { PkRegionId = 0, RegionName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkRegionId,
                        r.RegionName
                    }).ToList();
        }
        public object GetDrpRegionByZoneId(long ZoneId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(ZoneId, pagesize, pageno, search);

            result.Insert(0, new RegionModel { PkRegionId = 0, RegionName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkRegionId,
                        r.RegionName
                    }).ToList();
        }
        public object GetDrpTableRegion(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkRegionId,
                        Region = r.RegionName,
                        r.Description,
                        Zone = r.ZoneName,
                    }).ToList();
        }
        public string DeleteRecord(long PkRegionId)
        {
            string Error = "";
            RegionModel obj = GetSingleRecord(PkRegionId);

            //var Country = (from x in _context.TblZoneMas
            //               where x.FkcountryId == PkRegionId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  ZoneMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblRegionMas
                           where x.PkRegionId == PkRegionId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblRegionMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkRegionId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkRegionId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetRegionID(), PkRegionId, obj.FKRegionID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            RegionModel model = (RegionModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            RegionModel model = (RegionModel)objmodel;
            TblRegionMas Tbl = new TblRegionMas();
            if (model.PkRegionId > 0)
            {
                var _entity = __dbContext.TblRegionMas.Find(model.PkRegionId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkRegionId = model.PkRegionId;
            Tbl.RegionName = model.RegionName;
            Tbl.FkZoneId = model.FkZoneId;
            Tbl.Description = model.Description;

            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                RegionModel oldModel = GetSingleRecord(Tbl.PkRegionId);
                ID = Tbl.PkRegionId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKRegionID, oldModel.PkRegionId, oldModel.FKRegionID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKRegionID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Zone", Fields="ZoneName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Region", Fields="RegionName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Description", Fields="Description",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















