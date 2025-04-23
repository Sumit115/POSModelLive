using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class StateRepository : Repository<TblStateMas>, IStateRepository
    {
        public StateRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(StateModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.StateName))
            {
                cnt = (from x in __dbContext.TblStateMas
                       where x.StateName == model.StateName && x.PkStateId != model.PKID
                       && x.FkCountryId == model.FkCountryId
                       select x).Count();
                if (cnt > 0)
                    error = "State Already Exits";
            }

            return error;
        }

        public List<StateModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkCountryId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<StateModel> data = (from cou in __dbContext.TblStateMas
                                     where (EF.Functions.Like(cou.StateName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                       && (FkCountryId == 0 || cou.FkCountryId == FkCountryId)
                                     orderby cou.PkStateId
                                     select (new StateModel
                                     {
                                         PKID = cou.PkStateId, 
                                         StateName = cou.StateName,
                                         CapitalName = cou.CapitalName,
                                         StateType = cou.StateType,
                                         StateCode = cou.StateCode,
                                         FkCountryId = cou.FkCountryId,
                                         CountryName = cou.FKCountry.CountryName,
                                         FKUserID = cou.FKUserID,
                                         DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                         UserName = cou.FKUser.UserId,
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
         
        public StateModel GetSingleRecord(long PkStateId)
        {

            StateModel data = new StateModel();
            data = (from cou in __dbContext.TblStateMas
                   // join catGrp in __dbContext.TblCountryMas on cou.FkCountryId equals catGrp.PkCountryId
                    where cou.PkStateId == PkStateId
                    select (new StateModel
                    {
                        PKID = cou.PkStateId,
                        StateName = cou.StateName,
                        CapitalName = cou.CapitalName,
                        StateType = cou.StateType,
                        StateCode = cou.StateCode,
                        FkCountryId = cou.FkCountryId,
                        CountryName = cou.FKCountry.CountryName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,
                    })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FkCountryId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            { 

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return (from cou in __dbContext.TblStateMas
                        where (EF.Functions.Like(cou.StateName.Trim().ToLower(), Convert.ToString(search) + "%"))
                          && (FkCountryId == 0 || cou.FkCountryId == FkCountryId)
                         orderby cou.StateName
                         select (new
                         {
                             cou.PkStateId,
                             cou.StateName,
                             cou.CapitalName,
                             cou.FKCountry.CountryName,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PKID)
        {
            string Error = "";
            StateModel oldModel = GetSingleRecord(PKID); 

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblStateMas
                           where x.PkStateId == PKID
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblStateMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.State, PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.StateName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            StateModel model = (StateModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            StateModel model = (StateModel)objmodel;
            TblStateMas Tbl = new TblStateMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblStateMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkStateId = model.PKID;
            Tbl.StateName = model.StateName;
            Tbl.FkCountryId = model.FkCountryId;
            Tbl.CapitalName = model.CapitalName;
            Tbl.StateType = model.StateType;
            Tbl.StateCode = model.StateCode;
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

                StateModel oldModel = GetSingleRecord(Tbl.PkStateId);
                ID = Tbl.PkStateId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.State, Tbl.PkStateId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.StateName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKStateID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="State", Fields="StateName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Capital", Fields="CapitalName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Type", Fields="StateType",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Country", Fields="CountryName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };
             
            return list;
        }


    }
}
 