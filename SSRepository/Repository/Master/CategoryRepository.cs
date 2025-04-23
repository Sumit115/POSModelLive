using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class CategoryRepository : Repository<TblCategoryMas>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(CategoryModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Category))
            {
                cnt = (from x in __dbContext.TblCategoryMas
                       where x.CategoryName == model.Category && x.PkCategoryId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<CategoryModel> GetList(int pageSize, int pageNo = 1, string search = "", long CategoryGroupId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CategoryModel> data = (from cou in __dbContext.TblCategoryMas
                                        //join catGrp in __dbContext.TblCategoryGroupMas on cou.FkCategoryGroupId equals catGrp.PkCategoryGroupId
                                        where (EF.Functions.Like(cou.CategoryName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        && (CategoryGroupId == 0 || cou.FkCategoryGroupId == CategoryGroupId)
                                        orderby cou.PkCategoryId
                                        select (new CategoryModel
                                        {
                                            PKID = cou.PkCategoryId,
                                            Category = cou.CategoryName,
                                            FkCategoryGroupId = cou.FkCategoryGroupId,
                                            GroupName = cou.FKCategoryGroupMas.CategoryGroupName,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                            UserName=cou.FKUser.UserId,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                 if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblCategoryMas
                             //join _tranAlias in GetDrpTranAlias().ToList() on cou.TranAlias equals _tranAlias.Value
                         where EF.Functions.Like(cou.CategoryName.Trim().ToLower(), search + "%")
                         orderby cou.CategoryName
                         select (new
                         {
                             cou.PkCategoryId,
                             cou.CategoryName,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public object GetDrpCategory(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);

            //   result.Insert(0, new CategoryModel { PkCategoryId = 0, CategoryName = "Select" });

            return (from r in result
                    select new
                    {
                        r.PKID,
                        r.Category,
                        r.GroupName,
                    }).ToList(); ;
        }

        public CategoryModel GetSingleRecord(long PkCategoryId)
        {

            CategoryModel data = new CategoryModel();
            data = (from cou in __dbContext.TblCategoryMas
                    where cou.PkCategoryId == PkCategoryId
                    select (new CategoryModel
                    {
                        PKID = cou.PkCategoryId,
                        Category = cou.CategoryName,
                        FkCategoryGroupId = cou.FkCategoryGroupId,
                        GroupName = cou.FKCategoryGroupMas.CategoryGroupName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CategorySize_lst = (from ad in __dbContext.TblCategorySizeLnk
                                                //  join loc in __dbContext.TblBranchMas on ad.FKLocationID equals loc.PkBranchId
                                            where (ad.FkCategoryId == cou.PkCategoryId)
                                            select (new CategorySizeLnkModel
                                            {
                                                PkId = ad.PkId,
                                                Size = ad.Size,
                                                FkCategoryId = ad.FkCategoryId,
                                            })).ToList(),
                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkCategoryId)
        {
            string Error = "";
            CategoryModel oldModel = GetSingleRecord(PkCategoryId);
             
            if (Error == "")
            {
                var sizelst = (from x in __dbContext.TblCategorySizeLnk
                              where x.FkCategoryId == PkCategoryId  
                              select x).ToList();
                if (sizelst.Count > 0)
                    __dbContext.RemoveRange(sizelst);

                var lst = (from x in __dbContext.TblCategoryMas
                           where x.PkCategoryId == PkCategoryId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCategoryMas.RemoveRange(lst);

                
                AddMasterLog((long)Handler.Form.Category, oldModel.PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.Category, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));

                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            CategoryModel model = (CategoryModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CategoryModel model = (CategoryModel)objmodel;
            TblCategoryMas Tbl = new TblCategoryMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblCategoryMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCategoryId = model.PKID;
            Tbl.CategoryName = model.Category;
            Tbl.FkCategoryGroupId = model.FkCategoryGroupId;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                Tbl.PkCategoryId = getIdOfSeriesByEntity("PkCategoryId", null, Tbl, "TblCategoryMas");
                AddData(Tbl, false);
            }
            else
            {

                CategoryModel oldModel = GetSingleRecord(Tbl.PkCategoryId);
                ID = Tbl.PkCategoryId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Category, Tbl.PkCategoryId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Category, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }


            if (model.CategorySize_lst != null)
            {
                List<TblCategorySizeLnk> lstAdd = new List<TblCategorySizeLnk>();
                // List<TblCategorySizeLnk> lstEdit = new List<TblCategorySizeLnk>();
                List<TblCategorySizeLnk> lstDel = new List<TblCategorySizeLnk>();
                foreach (var item in model.CategorySize_lst)
                {
                    TblCategorySizeLnk locObj = new TblCategorySizeLnk();
                    locObj.Size = item.Size;
                    locObj.FkCategoryId = Tbl.PkCategoryId;
                    locObj.PkId = item.PkId;


                    //   lstAdd.Add(locObj);
                    if (item.Mode == 1)
                    {
                        //locObj.ModifiedDate = DateTime.Now;
                        //lstEdit.Add(locObj);
                    }
                    else if (item.Mode == 0)
                    {
                        //  locObj.PKAccountDtlId = getIdOfSeriesByEntity("PKAccountDtlId", null, Tbl, "TblAccountDtl");
                        locObj.ModifiedDate = DateTime.Now;
                        locObj.FKUserID = GetUserID();
                        locObj.FKCreatedByID = Tbl.FKUserID;
                        locObj.CreationDate = Tbl.ModifiedDate;
                        lstAdd.Add(locObj);
                    }

                    else
                    {
                        var res1 = (from x in __dbContext.TblCategorySizeLnk
                                    where x.FkCategoryId == locObj.FkCategoryId && x.Size == locObj.Size
                                    && x.PkId == locObj.PkId
                                    select x).Count();
                        if (res1 > 0)
                        {
                            lstDel.Add(locObj);
                        }
                    }

                }

                if (lstDel.Count() > 0)
                    DeleteData(lstDel, true);
                //if (lstEdit.Count() > 0)
                //    UpdateData(lstEdit, true); 
                if (lstAdd.Count() > 0)
                    AddData(lstAdd, true);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCategoryID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Section Name", Fields="Category",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Section Group", Fields="GroupName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















