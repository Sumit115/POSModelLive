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
                       where x.ZoneName == model.ZoneName && x.PkZoneId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Zone Name Already Exits";
            }

            return error;
        }

        public List<ZoneModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ZoneModel> data = (from cou in __dbContext.TblZoneMas
                                    where (EF.Functions.Like(cou.ZoneName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby cou.ZoneName
                                    select (new ZoneModel
                                    {
                                        PKID = cou.PkZoneId,
                                        ZoneName = cou.ZoneName,
                                        Description = cou.Description,
                                        FKUserID = cou.FKUserID,
                                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                        UserName = cou.FKUser.UserId,
                                    }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public ZoneModel GetSingleRecord(long PkZoneId)
        {

            ZoneModel data = (from cou in __dbContext.TblZoneMas
                              where cou.PkZoneId == PkZoneId
                              select (new ZoneModel
                              {
                                  PKID = cou.PkZoneId,
                                  ZoneName = cou.ZoneName,
                                  Description = cou.Description,
                                  FKUserID = cou.FKUserID,
                                  DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                  UserName = cou.FKUser.UserId,
                              })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return (from cou in __dbContext.TblZoneMas
                        where (EF.Functions.Like(cou.ZoneName.Trim().ToLower(), Convert.ToString(search) + "%"))
                        orderby cou.ZoneName
                        select (new
                        {
                             cou.PkZoneId,
                             cou.ZoneName,
                            cou.Description, 
                        }
                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PkZoneId)
        {
            string Error = "";
            ZoneModel oldModel = GetSingleRecord(PkZoneId);
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblZoneMas
                           where x.PkZoneId == PkZoneId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblZoneMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Zone, PkZoneId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.ZoneName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
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
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblZoneMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkZoneId = model.PKID;
            Tbl.ZoneName = model.ZoneName;
            Tbl.Description = model.Description;
            Tbl.FKUserID = GetUserID();
            Tbl.ModifiedDate = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                //obj.PkZoneId = ID = getIdOfSeriesByEntity("PkZoneId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                ZoneModel oldModel = GetSingleRecord(Tbl.PkZoneId);
                ID = Tbl.PkZoneId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Zone, Tbl.PkZoneId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.ZoneName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkZoneId, obj.FKZoneID, tblZone.Images, tblZone.Remarks, tblZone.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
       
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Zone", Fields="ZoneName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Description", Fields="Description",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };

            return list;
        }


    }
}



















