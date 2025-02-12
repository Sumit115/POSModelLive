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
                       where x.StateName == model.StateName && x.PkStateId != model.PkStateId
                       && x.FkCountryId == model.FkCountryId
                       select x).Count();
                if (cnt > 0)
                    error = "State Already Exits";
            }

            return error;
        }

        public List<StateModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<StateModel> data = (from cou in __dbContext.TblStateMas
                                     join catGrp in __dbContext.TblCountryMas on cou.FkCountryId equals catGrp.PkCountryId

                                     // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     orderby cou.PkStateId
                                     select (new StateModel
                                     {
                                         PkStateId = cou.PkStateId,
                                         
                                         StateName = cou.StateName,
                                         CapitalName = cou.CapitalName,
                                         StateType = cou.StateType,
                                         StateCode = cou.StateCode,
                                         FkCountryId = cou.FkCountryId,
                                         CountryName = catGrp.CountryName,
                                         FKUserID = cou.FKUserID,
                                         DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<StateModel> GetListByGroupId(long CountryId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<StateModel> data = (from cou in __dbContext.TblStateMas
                                     join catGrp in __dbContext.TblCountryMas on cou.FkCountryId equals catGrp.PkCountryId
                                     where cou.FkCountryId == CountryId
                                     // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     orderby cou.PkStateId
                                     select (new StateModel
                                     {
                                         PkStateId = cou.PkStateId,                                         
                                         StateName = cou.StateName,
                                         CapitalName = cou.CapitalName,
                                         StateType = cou.StateType,
                                         StateCode = cou.StateCode,
                                         FkCountryId = cou.FkCountryId,
                                         CountryName = catGrp.CountryName,
                                         FKUserID = cou.FKUserID,
                                         DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public StateModel GetSingleRecord(long PkStateId)
        {

            StateModel data = new StateModel();
            data = (from cou in __dbContext.TblStateMas
                    join catGrp in __dbContext.TblCountryMas on cou.FkCountryId equals catGrp.PkCountryId
                    where cou.PkStateId == PkStateId
                    select (new StateModel
                    {
                        PkStateId = cou.PkStateId,
                        StateName = cou.StateName,
                        CapitalName = cou.CapitalName,
                        StateType = cou.StateType,
                        StateCode = cou.StateCode,
                        FkCountryId = cou.FkCountryId,
                        CountryName = catGrp.CountryName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpState(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new StateModel { PkStateId = 0, StateName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkStateId,
                        r.StateName
                    }).ToList();
        }
        public object GetDrpTableState(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkStateId,
                        State = r.StateName,
                        Capital = r.CapitalName,
                        Code = r.StateCode,
                        r.StateType
                    }).ToList();
        }
        public object GetDrpStateByGroupId(long CountryId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(CountryId, pagesize, pageno, search);

            result.Insert(0, new StateModel { PkStateId = 0, StateName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkStateId,
                        r.StateName
                    }).ToList();
        }

        public string DeleteRecord(long PkStateId)
        {
            string Error = "";
            StateModel obj = GetSingleRecord(PkStateId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkStateId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblStateMas
                           where x.PkStateId == PkStateId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblStateMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkStateId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkStateId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetStateID(), PkStateId, obj.FKStateID, obj.DATE_MODIFIED, true);
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
            if (model.PkStateId > 0)
            {
                var _entity = __dbContext.TblStateMas.Find(model.PkStateId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkStateId = model.PkStateId;
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
                //AddMasterLog(oldModel, __FormID, tblCountry.FKStateID, oldModel.PkStateId, oldModel.FKStateID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKStateID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
              new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="State", Fields="StateName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
              new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Capital", Fields="CapitalName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
              new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Type", Fields="StateType",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
               new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Country", Fields="CountryName",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
               new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















