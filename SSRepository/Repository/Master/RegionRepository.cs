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
                       where x.RegionName == model.RegionName && x.PkRegionId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Region Name Already Exits";
            }

            return error;
        }

        public List<RegionModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkZoneId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<RegionModel> data = (from cou in __dbContext.TblRegionMas
                                      where (EF.Functions.Like(cou.RegionName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        && (FkZoneId == 0 || cou.FkZoneId == FkZoneId)
                                      orderby cou.RegionName
                                      select (new RegionModel
                                      {
                                          PKID = cou.PkRegionId,
                                          RegionName = cou.RegionName,
                                          Description = cou.Description,
                                          FkZoneId = cou.FkZoneId,
                                          ZoneName = cou.FKZone.ZoneName,
                                          FKUserID = cou.FKUserID,
                                          DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                          UserName = cou.FKUser.UserId,
                                      }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public RegionModel GetSingleRecord(long PkRegionId)
        {

            RegionModel data = new RegionModel();
            data = (from cou in __dbContext.TblRegionMas
                      where cou.PkRegionId == PkRegionId
                    select (new RegionModel
                    {
                        PKID = cou.PkRegionId,
                        RegionName = cou.RegionName,
                        Description = cou.Description,
                        FkZoneId = cou.FkZoneId,
                        ZoneName = cou.FKZone.ZoneName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,
                    })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FkZoneId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblRegionMas
                         where (EF.Functions.Like(cou.RegionName.Trim().ToLower(), Convert.ToString(search) + "%"))
                           && (FkZoneId == 0 || cou.FkZoneId == FkZoneId)
                         orderby cou.RegionName
                         select (new
                        {
                             PkRegionId = cou.PkRegionId,
                             RegionName = cou.RegionName,
                             ZoneName = cou.FKZone.ZoneName, 
                         }
                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PkRegionId)
        {
            string Error = "";
            RegionModel oldModel = GetSingleRecord(PkRegionId);
             
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblRegionMas
                           where x.PkRegionId == PkRegionId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblRegionMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Region, PkRegionId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.RegionName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
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
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblRegionMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkRegionId = model.PKID;
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
                AddMasterLog((long)Handler.Form.Region, Tbl.PkRegionId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.RegionName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKRegionID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Zone", Fields="ZoneName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Region", Fields="RegionName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Description", Fields="Description",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          }; 
        
            return list;
        }


    }
}



















