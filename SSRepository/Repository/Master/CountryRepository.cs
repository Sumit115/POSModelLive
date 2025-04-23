using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class CountryRepository : Repository<TblCountryMas>, ICountryRepository
    {
        public CountryRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(CountryModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.CountryName))
            {
                cnt = (from x in __dbContext.TblCountryMas
                       where x.CountryName == model.CountryName && x.PkCountryId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<CountryModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CountryModel> data = (from cou in __dbContext.TblCountryMas
                                           // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                       orderby cou.PkCountryId
                                       select (new CountryModel
                                       {
                                           PKID = cou.PkCountryId,
                                           CountryName = cou.CountryName,
                                           CapitalName = cou.CapitalName,
                                           FKUserID = cou.FKUserID,
                                           DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                           UserName = cou.FKUser.UserId,
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        } 
        public CountryModel GetSingleRecord(long PkCountryId)
        {

            CountryModel data = new CountryModel();
            data = (from cou in __dbContext.TblCountryMas
                    where cou.PkCountryId == PkCountryId
                    select (new CountryModel
                    {
                        PKID = cou.PkCountryId,
                        CountryName = cou.CountryName,
                        CapitalName = cou.CapitalName,
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
                return ((from cou in __dbContext.TblCountryMas
                         where (EF.Functions.Like(cou.CountryName.Trim().ToLower(), search + "%")) 
                         orderby cou.CountryName
                         select (new
                         {
                             cou.PkCountryId,
                             cou.CountryName,
                             cou.CapitalName,
                         }
                        )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PKID)
        {
            string Error = "";
            CountryModel oldModel = GetSingleRecord(PKID);
             
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCountryMas
                           where x.PkCountryId == PKID
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCountryMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Country, PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.CountryName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            CountryModel model = (CountryModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CountryModel model = (CountryModel)objmodel;
            TblCountryMas Tbl = new TblCountryMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblCountryMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCountryId = model.PKID;
            Tbl.CountryName = model.CountryName;
            Tbl.CapitalName = model.CapitalName;
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

                CountryModel oldModel = GetSingleRecord(Tbl.PkCountryId);
                ID = Tbl.PkCountryId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Country, Tbl.PkCountryId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.CountryName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCountryID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Country", Fields="CountryName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Capital", Fields="CapitalName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };
            return list;
        } 
    }
}



















