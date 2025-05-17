using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class DistrictRepository : Repository<TblDistrictMas>, IDistrictRepository
    {
        public DistrictRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(DistrictModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.DistrictName))
            {
                cnt = (from x in __dbContext.TblDistrictMas
                       where x.DistrictName == model.DistrictName && x.PkDistrictId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "District Name Already Exits";
            }

            return error;
        }

        public List<DistrictModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkStateId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<DistrictModel> data = (from cou in __dbContext.TblDistrictMas
                                        where (EF.Functions.Like(cou.DistrictName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                          && (FkStateId == 0 || cou.FkStateId == FkStateId)
                                        orderby cou.DistrictName
                                        select (new DistrictModel
                                        {
                                            PKID = cou.PkDistrictId,
                                            DistrictName = cou.DistrictName,
                                            FkStateId = cou.FkStateId,
                                            StateName = cou.FKState.StateName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                            UserName = cou.FKUser.UserId,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FkStateId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return (from cou in __dbContext.TblDistrictMas
                        where (EF.Functions.Like(cou.DistrictName.Trim().ToLower(), Convert.ToString(search) + "%"))
                          && (FkStateId == 0 || cou.FkStateId == FkStateId)
                        orderby cou.DistrictName
                        select (new
                        {
                            cou.PkDistrictId,
                            cou.DistrictName, 
                            cou.FKState.StateName,
                        }
                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return null;
            }
        }


        public DistrictModel GetSingleRecord(long PkDistrictId)
        {

            DistrictModel data = new DistrictModel();
            data = (from cou in __dbContext.TblDistrictMas
                    where cou.PkDistrictId == PkDistrictId
                    select (new DistrictModel
                    {
                        PKID = cou.PkDistrictId,
                        DistrictName = cou.DistrictName,
                        FkStateId = cou.FkStateId,
                        StateName = cou.FKState.StateName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,
                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkDistrictId)
        {
            string Error = "";
            DistrictModel oldModel = GetSingleRecord(PkDistrictId);

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblDistrictMas
                           where x.PkDistrictId == PkDistrictId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblDistrictMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.District, PkDistrictId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.DistrictName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            DistrictModel model = (DistrictModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            DistrictModel model = (DistrictModel)objmodel;
            TblDistrictMas Tbl = new TblDistrictMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblDistrictMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkDistrictId = model.PKID;
            Tbl.DistrictName = model.DistrictName;
            Tbl.FkStateId = model.FkStateId;
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

                DistrictModel oldModel = GetSingleRecord(Tbl.PkDistrictId);
                ID = Tbl.PkDistrictId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.District, Tbl.PkDistrictId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.DistrictName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKDistrictID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="State", Fields="StateName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="District", Fields="DistrictName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };

            return list;
        }


    }
}



















