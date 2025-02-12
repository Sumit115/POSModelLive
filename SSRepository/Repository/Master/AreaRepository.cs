using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class AreaRepository : Repository<TblAreaMas>, IAreaRepository
    {
        public AreaRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(AreaModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.AreaName))
            {
                cnt = (from x in __dbContext.TblAreaMas
                       where x.AreaName == model.AreaName && x.PkAreaId != model.PkAreaId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<AreaModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<AreaModel> data = (from cou in __dbContext.TblAreaMas
                                        join catGrp in __dbContext.TblRegionMas on cou.FkRegionId equals catGrp.PkRegionId

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkAreaId
                                        select (new AreaModel
                                        {
                                            PkAreaId = cou.PkAreaId,
                                            AreaName = cou.AreaName,
                                            Description = cou.Description,
                                            FkRegionId = cou.FkRegionId,
                                            RegionName = catGrp.RegionName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<AreaModel> GetListByGroupId(long RegionId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<AreaModel> data = (from cou in __dbContext.TblAreaMas
                                        join catGrp in __dbContext.TblRegionMas on cou.FkRegionId equals catGrp.PkRegionId
                                        where cou.FkRegionId == RegionId
                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkAreaId
                                        select (new AreaModel
                                        {
                                            PkAreaId = cou.PkAreaId,
                                            AreaName = cou.AreaName,
                                            Description = cou.Description,
                                            FkRegionId = cou.FkRegionId,
                                            RegionName = catGrp.RegionName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public AreaModel GetSingleRecord(long PkAreaId)
        {

            AreaModel data = new AreaModel();
            data = (from cou in __dbContext.TblAreaMas
                    join catGrp in __dbContext.TblRegionMas on cou.FkRegionId equals catGrp.PkRegionId
                    where cou.PkAreaId == PkAreaId
                    select (new AreaModel
                    {
                        PkAreaId = cou.PkAreaId,
                        AreaName = cou.AreaName,
                        Description = cou.Description,
                        FkRegionId = cou.FkRegionId,
                        RegionName = catGrp.RegionName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpArea(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new AreaModel { PkAreaId = 0, AreaName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkAreaId,
                        r.AreaName
                    }).ToList();
        }
        public object GetDrpAreaByRegionId(long RegionId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(RegionId, pagesize, pageno, search);

            result.Insert(0, new AreaModel { PkAreaId = 0, AreaName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkAreaId,
                        r.AreaName
                    }).ToList();
        }
        public object GetDrpTableArea(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkAreaId,
                        Area = r.AreaName,
                        r.Description,
                        Region = r.RegionName,
                    }).ToList();
        }
        public string DeleteRecord(long PkAreaId)
        {
            string Error = "";
            AreaModel obj = GetSingleRecord(PkAreaId);

            //var Country = (from x in _context.TblRegionMas
            //               where x.FkcountryId == PkAreaId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  RegionMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblAreaMas
                           where x.PkAreaId == PkAreaId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblAreaMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkAreaId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkAreaId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetAreaID(), PkAreaId, obj.FKAreaID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            AreaModel model = (AreaModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            AreaModel model = (AreaModel)objmodel;
            TblAreaMas Tbl = new TblAreaMas();
            if (model.PkAreaId > 0)
            {
                var _entity = __dbContext.TblAreaMas.Find(model.PkAreaId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkAreaId = model.PkAreaId;
            Tbl.AreaName = model.AreaName;
            Tbl.FkRegionId = model.FkRegionId;
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

                AreaModel oldModel = GetSingleRecord(Tbl.PkAreaId);
                ID = Tbl.PkAreaId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKAreaID, oldModel.PkAreaId, oldModel.FKAreaID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKAreaID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Region", Fields="RegionName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Area", Fields="AreaName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Description", Fields="Description",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=1, Orderby =4, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=1, Orderby =5, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















