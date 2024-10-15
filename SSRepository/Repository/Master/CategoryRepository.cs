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
            if (!string.IsNullOrEmpty(model.Category))
            {
                cnt = (from x in __dbContext.TblCategoryMas
                       where x.CategoryName == model.Category && x.PkCategoryId != model.PkCategoryId
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
                                        join catGrp in __dbContext.TblCategoryGroupMas on cou.FkCategoryGroupId equals catGrp.PkCategoryGroupId
                                        where (EF.Functions.Like(cou.CategoryName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        && (CategoryGroupId == 0 || cou.FkCategoryGroupId == CategoryGroupId)
                                        orderby cou.PkCategoryId
                                        select (new CategoryModel
                                        {
                                            PkCategoryId = cou.PkCategoryId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                            CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                                            Category = cou.CategoryName,
                                            FkCategoryGroupId = cou.FkCategoryGroupId,
                                            GroupName = catGrp.CategoryGroupName,
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
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                        Category = cou.CategoryName,
                        FkCategoryGroupId = cou.FkCategoryGroupId,
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
            Tbl.CategoryName = model.Category;
            Tbl.FkCategoryGroupId = model.FkCategoryGroupId;
            Tbl.ModifiedDate = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.FKUserID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                Tbl.PkCategoryId = getIdOfSeriesByEntity("PkCategoryId", null, Tbl, "TblCategoryMas");
                AddData(Tbl, false);
            }
            else
            {

                CategoryModel oldModel = GetSingleRecord(Tbl.PkCategoryId);
                ID = Tbl.PkCategoryId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKCategoryID, oldModel.PkCategoryId, oldModel.FKCategoryID, oldModel.DATE_MODIFIED);
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
                        locObj.FKCreatedByID = model.FKCreatedByID;
                        locObj.FKUserID = model.FKUserId;
                        locObj.CreationDate = DateTime.Now;
                        locObj.ModifiedDate = DateTime.Now;
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
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Section Name", Fields="Category",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Section Group", Fields="GroupName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
               };
            return list;
        }


    }
}



















