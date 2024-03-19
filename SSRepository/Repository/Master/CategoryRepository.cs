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
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(CategoryModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.CategoryName))
            {
                cnt = (from x in __dbContext.TblCategoryMas
                       where x.CategoryName == model.CategoryName && x.PkCategoryId != model.PkCategoryId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<CategoryModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CategoryModel> data = (from cou in __dbContext.TblCategoryMas
                                        join catGrp in __dbContext.TblCategoryGroupMas on cou.FkCategoryGroupId equals catGrp.PkCategoryGroupId

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkCategoryId
                                        select (new CategoryModel
                                        {
                                            PkCategoryId = cou.PkCategoryId,
                                            FKUserId = cou.FKUserId,
                                            src = cou.Src,
                                            DATE_MODIFIED = cou.DateModified,
                                            DATE_CREATED = cou.DateCreated,
                                            CategoryName = cou.CategoryName,
                                            FkCategoryGroupId = cou.FkCategoryGroupId,
                                            PCategoryGroupName = catGrp.CategoryGroupName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<CategoryModel> GetListByGroupId(long CategoryGroupId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CategoryModel> data = (from cou in __dbContext.TblCategoryMas
                                        join catGrp in __dbContext.TblCategoryGroupMas on cou.FkCategoryGroupId equals catGrp.PkCategoryGroupId
                                        where cou.FkCategoryGroupId == CategoryGroupId
                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkCategoryId
                                        select (new CategoryModel
                                        {
                                            PkCategoryId = cou.PkCategoryId,
                                            FKUserId = cou.FKUserId,
                                            src = cou.Src,
                                            DATE_MODIFIED = cou.DateModified,
                                            DATE_CREATED = cou.DateCreated,
                                            CategoryName = cou.CategoryName,
                                            FkCategoryGroupId = cou.FkCategoryGroupId,
                                            PCategoryGroupName = catGrp.CategoryGroupName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public CategoryModel GetSingleRecord(long PkCategoryId)
        {

            CategoryModel data = new CategoryModel();
            data = (from cou in __dbContext.TblCategoryMas
                    where cou.PkCategoryId == PkCategoryId
                    select (new CategoryModel
                    {
                        PkCategoryId = cou.PkCategoryId,
                        FKUserId = cou.FKUserId,
                        src = cou.Src,
                        DATE_MODIFIED = cou.DateModified,
                        DATE_CREATED = cou.DateCreated,
                        CategoryName = cou.CategoryName,
                        FkCategoryGroupId = cou.FkCategoryGroupId,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpCategory( int pagesize, int pageno,string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new CategoryModel { PkCategoryId = 0, CategoryName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkCategoryId,
                        r.CategoryName
                    }).ToList();
        }
        public object GetDrpCategoryByGroupId(long CategoryGroupId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(CategoryGroupId,pagesize, pageno, search);

            result.Insert(0, new CategoryModel { PkCategoryId = 0, CategoryName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkCategoryId,
                        r.CategoryName
                    }).ToList();
        }

        public string DeleteRecord(long PkCategoryId)
        {
            string Error = "";
            CategoryModel obj = GetSingleRecord(PkCategoryId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkCategoryId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCategoryMas
                           where x.PkCategoryId == PkCategoryId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCategoryMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkCategoryId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkCategoryId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetCategoryID(), PkCategoryId, obj.FKCategoryID, obj.DATE_MODIFIED, true);
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
            if (model.PkCategoryId > 0)
            {
                var _entity = __dbContext.TblCategoryMas.Find(model.PkCategoryId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCategoryId = model.PkCategoryId;
            Tbl.CategoryName = model.CategoryName;
            Tbl.FkCategoryGroupId = model.FkCategoryGroupId;
            Tbl.DateModified = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.Src = model.src;
                Tbl.FKUserId = model.FKUserId;
                Tbl.DateCreated = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                CategoryModel oldModel = GetSingleRecord(Tbl.PkCategoryId);
                ID = Tbl.PkCategoryId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKCategoryID, oldModel.PkCategoryId, oldModel.FKCategoryID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCategoryID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Section Group Name", Fields="PCategoryGroupName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Section Name", Fields="CategoryName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                        };
            return list;
        }


    }
}



















