using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class ZoneRepository : Repository<TblZoneMas>, IZoneRepository
    {
        public ZoneRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(ZoneModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.ZoneName))
            {
                cnt = (from x in __dbContext.TblZoneMas
                       where x.ZoneName == model.ZoneName && x.PkZoneId != model.PkZoneId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<ZoneModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ZoneModel> data = (from cou in __dbContext.TblZoneMas
                                           // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                       orderby cou.PkZoneId
                                       select (new ZoneModel
                                       {
                                           PkZoneId = cou.PkZoneId,
                                           FKUserId = cou.FKUserID,
                                           FKCreatedByID = cou.FKCreatedByID,
                                           ZoneName = cou.ZoneName,
                                           Description = cou.Description,
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public ZoneModel GetSingleRecord(long PkZoneId)
        {

            ZoneModel data = new ZoneModel();
            data = (from cou in __dbContext.TblZoneMas
                    where cou.PkZoneId == PkZoneId
                    select (new ZoneModel
                    {
                        PkZoneId = cou.PkZoneId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ZoneName = cou.ZoneName,
                        Description = cou.Description,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpZone(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new ZoneModel { PkZoneId = 0, ZoneName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkZoneId,
                        r.ZoneName
                    }).ToList();
        }
        public object GetDrpTableZone(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkZoneId,
                        r.ZoneName,
                        r.Description
                    }).ToList();
        }

        public string DeleteRecord(long PkZoneId)
        {
            string Error = "";
            ZoneModel obj = GetSingleRecord(PkZoneId);

            //var Zone = (from x in _context.TblStateMas
            //               where x.FkZoneId == PkZoneId
            //               select x).Count();
            //if (Zone > 0)
            //    Error += "Table Name -  StateMas : " + Zone + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblZoneMas
                           where x.PkZoneId == PkZoneId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblZoneMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkZoneId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkZoneId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetZoneID(), PkZoneId, obj.FKZoneID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            ZoneModel model = (ZoneModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            ZoneModel model = (ZoneModel)objmodel;
            TblZoneMas Tbl = new TblZoneMas();
            if (model.PkZoneId > 0)
            {
                var _entity = __dbContext.TblZoneMas.Find(model.PkZoneId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkZoneId = model.PkZoneId;
            Tbl.ZoneName = model.ZoneName;
            Tbl.Description = model.Description;
            Tbl.ModifiedDate= DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.FKUserID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                //obj.PkZoneId = ID = getIdOfSeriesByEntity("PkZoneId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                ZoneModel oldModel = GetSingleRecord(Tbl.PkZoneId);
                ID = Tbl.PkZoneId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblZone.FKZoneID, oldModel.PkZoneId, oldModel.FKZoneID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkZoneId, obj.FKZoneID, tblZone.Images, tblZone.Remarks, tblZone.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Zone", Fields="ZoneName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =1, Heading ="Description", Fields="Description",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















