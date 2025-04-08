using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Drawing;

namespace SSRepository.Repository.Master
{
    public class CategoryGroupRepository : Repository<TblCategoryGroupMas>, ICategoryGroupRepository
    {
        public CategoryGroupRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(CategoryGroupModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.CategoryGroupName))
            {
                cnt = (from x in __dbContext.TblCategoryGroupMas
                       where x.CategoryGroupName == model.CategoryGroupName && x.PkCategoryGroupId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Group Name Already Exits";
            }

            return error;
        }

        public List<CategoryGroupModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CategoryGroupModel> data = (from cou in __dbContext.TblCategoryGroupMas
                                                 //join CatPGrp in __dbContext.TblCategoryGroupMas on cou.FkCategoryGroupId equals CatPGrp.PkCategoryGroupId
                                                 //                into tempcatGrp
                                                 //from catGrp in tempcatGrp.DefaultIfEmpty()
                                             where (EF.Functions.Like(cou.CategoryGroupName.Trim().ToLower(), search + "%"))
                                             orderby cou.PkCategoryGroupId
                                             select (new CategoryGroupModel
                                             {
                                                 PKID = cou.PkCategoryGroupId,
                                                 CategoryGroupName = cou.CategoryGroupName,
                                                 FkCategoryGroupId = cou.FkCategoryGroupId,
                                                 PCategoryGroupName = cou.FKCategoryGroupMas.CategoryGroupName,
                                                 FKUserID = cou.FKUserID,
                                                 DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                                 UserName = cou.FKUser.UserId,
                                             }
                                            )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblCategoryGroupMas
                             //join _tranAlias in GetDrpTranAlias().ToList() on cou.TranAlias equals _tranAlias.Value
                         where EF.Functions.Like(cou.CategoryGroupName.Trim().ToLower(), search + "%")
                         orderby cou.CategoryGroupName
                         select (new
                         {
                             cou.PkCategoryGroupId,
                             cou.CategoryGroupName,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }


        public CategoryGroupModel GetSingleRecord(long PkCategoryGroupId)
        {

            CategoryGroupModel data = new CategoryGroupModel();
            data = (from cou in __dbContext.TblCategoryGroupMas
                    where cou.PkCategoryGroupId == PkCategoryGroupId
                    select (new CategoryGroupModel
                    {
                        PKID = cou.PkCategoryGroupId,
                        CategoryGroupName = cou.CategoryGroupName,
                        FkCategoryGroupId = cou.FkCategoryGroupId,
                        PCategoryGroupName = cou.FKCategoryGroupMas.CategoryGroupName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserName = cou.FKUser.UserId,

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpCategoryGroup(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new CategoryGroupModel { PKID = 0, CategoryGroupName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PKID,
                        r.CategoryGroupName
                    }).ToList();
        }

        public string DeleteRecord(long PkCategoryGroupId)
        {
            string Error = "";
            CategoryGroupModel oldModel = GetSingleRecord(PkCategoryGroupId);

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCategoryGroupMas
                           where x.PkCategoryGroupId == PkCategoryGroupId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCategoryGroupMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.CategoryGroup, oldModel.PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.CategoryGroupName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            CategoryGroupModel model = (CategoryGroupModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CategoryGroupModel model = (CategoryGroupModel)objmodel;
            TblCategoryGroupMas Tbl = new TblCategoryGroupMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblCategoryGroupMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCategoryGroupId = model.PKID;
            Tbl.CategoryGroupName = model.CategoryGroupName;
            Tbl.FkCategoryGroupId = model.FkCategoryGroupId;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKCreatedByID = 1;
            if (Mode == "Create")
            {
                Tbl.FKUserID = 1;
                Tbl.CreationDate = DateTime.Now;
                AddData(Tbl, false);
            }
            else
            {

                CategoryGroupModel oldModel = GetSingleRecord(Tbl.PkCategoryGroupId);
                ID = Tbl.PkCategoryGroupId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.CategoryGroup, Tbl.PkCategoryGroupId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.CategoryGroupName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCategoryGroupID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
               new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Parent Group", Fields="PCategoryGroupName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Section Group", Fields="CategoryGroupName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















