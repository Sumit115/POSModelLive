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
        public StationRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(StationModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.StationName))
            {
                cnt = (from x in __dbContext.TblStationMas
                       where x.StationName == model.StationName && x.PkStationId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<StationModel> GetList(int pageSize, int pageNo = 1, string search = "",int FkDistrictId=0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<StationModel> data = (from cou in __dbContext.TblStationMas
                                       where (EF.Functions.Like(cou.StationName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                         && (FkDistrictId == 0 || cou.FkDistrictId == FkDistrictId)
                                       orderby cou.StationName 
                                       select (new StationModel
                                       {
                                           PKID = cou.PkStationId,
                                           StationName = cou.StationName,
                                           FkDistrictId = cou.FkDistrictId,
                                           DistrictName = cou.FKDistrict.DistrictName,
                                           FKUserID = cou.FKUserID,
                                           DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                           UserName = cou.FKUser.UserId,
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public StationModel GetSingleRecord(long PkStationId)
        {

            StationModel data = new StationModel();
            data = (from cou in __dbContext.TblStationMas
                     where cou.PkStationId == PkStationId
                    select (new StationModel
                    {
                        PKID = cou.PkStationId,
                        StationName = cou.StationName,
                        FkDistrictId = cou.FkDistrictId,
                        DistrictName = cou.FKDistrict.DistrictName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,
                    })).FirstOrDefault();
            return data;
        }

        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FkDistrictId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return (from cou in __dbContext.TblStationMas
                        where (EF.Functions.Like(cou.StationName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        && (FkDistrictId == 0 || cou.FkDistrictId == FkDistrictId)
                        orderby cou.StationName
                        select (new
                        {
                            cou.PkStationId,
                            cou.StationName, 
                            cou.FKDistrict.DistrictName
                        }
       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PkStationId)
        {
            string Error = "";
            StationModel oldModel = GetSingleRecord(PkStationId);

            
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblStationMas
                           where x.PkStationId == PkStationId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblStationMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Station, PkStationId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.StationName,GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
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
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblStationMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkStationId = model.PKID;
            Tbl.StationName = model.StationName;
            Tbl.FkDistrictId = model.FkDistrictId;

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

                StationModel oldModel = GetSingleRecord(Tbl.PkStationId);
                ID = Tbl.PkStationId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Station, Tbl.PkStationId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.StationName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKStationID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="District", Fields="DistrictName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Station", Fields="StationName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };

         return list;
        }


    }
}



















