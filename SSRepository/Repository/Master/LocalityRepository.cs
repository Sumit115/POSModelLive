using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Azure;

namespace SSRepository.Repository.Master
{
    public class LocalityRepository : Repository<TblLocalityMas>, ILocalityRepository
    {
        public LocalityRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(LocalityModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.LocalityName))
            {
                cnt = (from x in __dbContext.TblLocalityMas
                       where x.LocalityName == model.LocalityName && x.PkLocalityId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<LocalityModel> GetList(int pageSize, int pageNo = 1, string search = "",int FkAreaId=0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<LocalityModel> data = (from cou in __dbContext.TblLocalityMas
                                        where (EF.Functions.Like(cou.LocalityName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     && (FkAreaId == 0 || cou.FkAreaId == FkAreaId)
                                        orderby cou.LocalityName
                                        select (new LocalityModel
                                        {
                                            PKID = cou.PkLocalityId,
                                            LocalityName = cou.LocalityName,
                                            Description = cou.Description,
                                            FkAreaId = cou.FkAreaId,
                                            AreaName = cou.FKArea.AreaName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                            UserName = cou.FKUser.UserId,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

       
        public LocalityModel GetSingleRecord(long PkLocalityId)
        {

            LocalityModel data = new LocalityModel();
            data = (from cou in __dbContext.TblLocalityMas
                     where cou.PkLocalityId == PkLocalityId
                    select (new LocalityModel
                    {
                        PKID = cou.PkLocalityId,
                        LocalityName = cou.LocalityName,
                        Description = cou.Description,
                        FkAreaId = cou.FkAreaId,
                        AreaName = cou.FKArea.AreaName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,
                    })).FirstOrDefault();
            return data;
        }
       
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long AreaId=0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return (from cou in __dbContext.TblLocalityMas
                                             where (AreaId==0 || cou.FkAreaId == AreaId)
                                            orderby cou.LocalityName
                                            select (new 
                                            {
                                                cou.PkLocalityId,
                                                cou.LocalityName,
                                                cou.Description,
                                                cou.FkAreaId,
                                                cou.FKArea.AreaName,
                                                
                                            }
                                           )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PkLocalityId)
        {
            string Error = "";
            LocalityModel oldModel = GetSingleRecord(PkLocalityId); 
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblLocalityMas
                           where x.PkLocalityId == PkLocalityId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblLocalityMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Locality, PkLocalityId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.LocalityName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            LocalityModel model = (LocalityModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            LocalityModel model = (LocalityModel)objmodel;
            TblLocalityMas Tbl = new TblLocalityMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblLocalityMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkLocalityId = model.PKID;
            Tbl.LocalityName = model.LocalityName;
            Tbl.FkAreaId = model.FkAreaId;
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

                LocalityModel oldModel = GetSingleRecord(Tbl.PkLocalityId);
                ID = Tbl.PkLocalityId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Locality, Tbl.PkLocalityId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.LocalityName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKLocalityID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Area", Fields="AreaName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Locality", Fields="LocalityName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Description", Fields="Description",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };
            
            return list;
        }


    }
}



















