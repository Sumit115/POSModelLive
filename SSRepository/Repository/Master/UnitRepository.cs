using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class UnitRepository : Repository<TblUnitMas>, IUnitRepository
    {
        public UnitRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }
       
        public string isAlreadyExist(UnitModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.UnitName))
            {
                cnt = (from x in __dbContext.TblUnitMas
                       where x.UnitName == model.UnitName && x.PkUnitId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Unit Name Exits";
            }

             
            return error;
        }

        public List<UnitModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<UnitModel> data = (from cou in __dbContext.TblUnitMas
                                          // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                      orderby cou.PkUnitId
                                      select (new UnitModel
                                      {
                                          PKID = cou.PkUnitId,
                                          UnitName = cou.UnitName, 
                                          FKUserID = cou.FKUserID,
                                          DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                          UserName = cou.FKUser.UserId,
                                      }
                                     )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public UnitModel GetSingleRecord(long PkUnitId)
        {

            UnitModel data = new UnitModel();
            data = (from cou in __dbContext.TblUnitMas
                    where cou.PkUnitId == PkUnitId
                    select (new UnitModel
                    {
                        PKID = cou.PkUnitId,
                        UnitName = cou.UnitName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpUnit(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);

         //   result.Insert(0, new UnitModel { PkUnitId = 0, UnitName = "Select" });

            return (from r in result
                    select new
                    {
                        r.PKID,
                        r.UnitName
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkUnitId)
        {
            string Error = "";
            UnitModel oldModel = GetSingleRecord(PkUnitId); 
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblUnitMas
                           where x.PkUnitId == PkUnitId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblUnitMas.RemoveRange(lst);
              
                AddMasterLog((long)Handler.Form.Unit, PkUnitId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.UnitName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            UnitModel model = (UnitModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            UnitModel model = (UnitModel)objmodel;
            TblUnitMas Tbl = new TblUnitMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblUnitMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkUnitId = model.PKID;
            Tbl.UnitName = model.UnitName;
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

                UnitModel oldModel = GetSingleRecord(Tbl.PkUnitId);
                ID = Tbl.PkUnitId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Unit, Tbl.PkUnitId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.UnitName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKUnitID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
             {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Name", Fields="UnitName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
             };
             
            return list;
        }


    }
}



















