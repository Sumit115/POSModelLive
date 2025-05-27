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
                       where x.AreaName == model.AreaName && x.PkAreaId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<AreaModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkRegionId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<AreaModel> data = (from cou in __dbContext.TblAreaMas
                                    where (EF.Functions.Like(cou.AreaName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     && (FkRegionId == 0 || cou.FkRegionId == FkRegionId)
                                    orderby cou.AreaName
                                    select (new AreaModel
                                    {
                                        PKID = cou.PkAreaId,
                                        AreaName = cou.AreaName,
                                        Description = cou.Description,
                                        FkRegionId = cou.FkRegionId,
                                        RegionName = cou.FKRegion.RegionName,
                                        FKUserID = cou.FKUserID,
                                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                        UserName = cou.FKUser.UserId,
                                    }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public AreaModel GetSingleRecord(long PkAreaId)
        {

            AreaModel data = new AreaModel();
            data = (from cou in __dbContext.TblAreaMas
                    where cou.PkAreaId == PkAreaId
                    select (new AreaModel
                    {
                        PKID = cou.PkAreaId,
                        AreaName = cou.AreaName,
                        Description = cou.Description,
                        FkRegionId = cou.FkRegionId,
                        RegionName = cou.FKRegion.RegionName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,
                    })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FkRegionId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblAreaMas
                         where (EF.Functions.Like(cou.AreaName.Trim().ToLower(), Convert.ToString(search) + "%"))
                          && (FkRegionId == 0 || cou.FkRegionId == FkRegionId)
                         orderby cou.AreaName
                         select (new
                         {
                             PkAreaId = cou.PkAreaId,
                             AreaName = cou.AreaName,
                             RegionName = cou.FKRegion.RegionName,
                         }
                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PkAreaId)
        {
            string Error = "";
            AreaModel oldModel = GetSingleRecord(PkAreaId);

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblAreaMas
                           where x.PkAreaId == PkAreaId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblAreaMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Area, PkAreaId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.AreaName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
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
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblAreaMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkAreaId = model.PKID;
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
                AddMasterLog((long)Handler.Form.Area, Tbl.PkAreaId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.AreaName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKAreaID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Region", Fields="RegionName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Area", Fields="AreaName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Description", Fields="Description",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };

          return list;
        }


    }
}



















