using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class BrandRepository : Repository<TblBrandMas>, IBrandRepository
    {
        public BrandRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(BrandModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.BrandName))
            {
                cnt = (from x in __dbContext.TblBrandMas
                       where x.BrandName == model.BrandName && x.PkBrandId != model.PkBrandId
                       select x).Count();
                if (cnt > 0)
                    error = "Brand Name Exits";
            }


            return error;
        }

        public List<BrandModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<BrandModel> data = (from cou in __dbContext.TblBrandMas
                                         // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     orderby cou.PkBrandId
                                     select (new BrandModel
                                     {
                                         PkBrandId = cou.PkBrandId,
                                         BrandName = cou.BrandName,
                                         FKUserID = cou.FKUserID,
                                         DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                         UserName = cou.FKUser.UserId,
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public BrandModel GetSingleRecord(long PkBrandId)
        {

            BrandModel data = new BrandModel();
            data = (from cou in __dbContext.TblBrandMas
                    where cou.PkBrandId == PkBrandId
                    select (new BrandModel
                    {
                        PkBrandId = cou.PkBrandId,
                        BrandName = cou.BrandName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpBrand(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);

            //   result.Insert(0, new BrandModel { PkBrandId = 0, BrandName = "Select" });

            return (from r in result
                    select new
                    {
                        r.PkBrandId,
                        r.BrandName
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkBrandId)
        {
            string Error = "";
            BrandModel oldModel = GetSingleRecord(PkBrandId);

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblBrandMas
                           where x.PkBrandId == PkBrandId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblBrandMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Brand, oldModel.PkBrandId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.BrandName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            BrandModel model = (BrandModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            BrandModel model = (BrandModel)objmodel;
            TblBrandMas Tbl = new TblBrandMas();
            if (model.PkBrandId > 0)
            {
                var _entity = __dbContext.TblBrandMas.Find(model.PkBrandId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkBrandId = model.PkBrandId;
            Tbl.BrandName = model.BrandName;
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

                BrandModel oldModel = GetSingleRecord(Tbl.PkBrandId);
                ID = Tbl.PkBrandId;
                UpdateData(Tbl, false);

                AddMasterLog((long)Handler.Form.Brand, Tbl.PkBrandId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.BrandName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBrandID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Brand Name", Fields="BrandName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };

            return list;
        }


    }
}



















