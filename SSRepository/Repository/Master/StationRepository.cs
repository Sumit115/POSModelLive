using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class StationRepository : Repository<TblStationMas>, IStationRepository
    {
        public StationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(StationModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.StationName))
            {
                cnt = (from x in __dbContext.TblStationMas
                       where x.StationName == model.StationName && x.PkStationId != model.PkStationId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<StationModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<StationModel> data = (from cou in __dbContext.TblStationMas
                                        join catGrp in __dbContext.TblDistrictMas on cou.FkDistrictId equals catGrp.PkDistrictId

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkStationId
                                        select (new StationModel
                                        {
                                            PkStationId = cou.PkStationId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            StationName = cou.StationName,
                                            FkDistrictId = cou.FkDistrictId,
                                            DistrictName = catGrp.DistrictName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<StationModel> GetListByGroupId(long DistrictId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<StationModel> data = (from cou in __dbContext.TblStationMas
                                        join catGrp in __dbContext.TblDistrictMas on cou.FkDistrictId equals catGrp.PkDistrictId
                                        where cou.FkDistrictId == DistrictId
                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkStationId
                                        select (new StationModel
                                        {
                                            PkStationId = cou.PkStationId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            StationName = cou.StationName,
                                            FkDistrictId = cou.FkDistrictId,
                                            DistrictName = catGrp.DistrictName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public StationModel GetSingleRecord(long PkStationId)
        {

            StationModel data = new StationModel();
            data = (from cou in __dbContext.TblStationMas
                    join catGrp in __dbContext.TblDistrictMas on cou.FkDistrictId equals catGrp.PkDistrictId
                    where cou.PkStationId == PkStationId
                    select (new StationModel
                    {
                        PkStationId = cou.PkStationId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        StationName = cou.StationName,
                        FkDistrictId = cou.FkDistrictId,
                        DistrictName = catGrp.DistrictName,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpStation(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new StationModel { PkStationId = 0, StationName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkStationId,
                        r.StationName
                    }).ToList();
        }
        public object GetDrpStationByDistrictId(long DistrictId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(DistrictId, pagesize, pageno, search);

            result.Insert(0, new StationModel { PkStationId = 0, StationName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkStationId,
                        r.StationName
                    }).ToList();
        }

        public string DeleteRecord(long PkStationId)
        {
            string Error = "";
            StationModel obj = GetSingleRecord(PkStationId);

            //var Country = (from x in _context.TblDistrictMas
            //               where x.FkcountryId == PkStationId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  DistrictMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblStationMas
                           where x.PkStationId == PkStationId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblStationMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkStationId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkStationId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetStationID(), PkStationId, obj.FKStationID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            StationModel model = (StationModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            StationModel model = (StationModel)objmodel;
            TblStationMas Tbl = new TblStationMas();
            if (model.PkStationId > 0)
            {
                var _entity = __dbContext.TblStationMas.Find(model.PkStationId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkStationId = model.PkStationId;
            Tbl.StationName = model.StationName;
            Tbl.FkDistrictId = model.FkDistrictId;
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

                StationModel oldModel = GetSingleRecord(Tbl.PkStationId);
                ID = Tbl.PkStationId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKStationID, oldModel.PkStationId, oldModel.FKStationID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKStationID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="District", Fields="DistrictName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Station", Fields="StationName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















