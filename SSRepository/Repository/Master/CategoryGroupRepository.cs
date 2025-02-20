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
                       where x.CategoryGroupName == model.CategoryGroupName && x.PkCategoryGroupId != model.PkCategoryGroupId
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
                                             join CatPGrp in __dbContext.TblCategoryGroupMas on cou.FkCategoryGroupId equals CatPGrp.PkCategoryGroupId
                                                             into tempcatGrp
                                             from catGrp in tempcatGrp.DefaultIfEmpty()
                                             where (EF.Functions.Like(cou.CategoryGroupName.Trim().ToLower(), search + "%"))
                                             orderby cou.PkCategoryGroupId
                                             select (new CategoryGroupModel
                                             {
                                                 PkCategoryGroupId = cou.PkCategoryGroupId,
                                                 CategoryGroupName = cou.CategoryGroupName,
                                                 FkCategoryGroupId = cou.FkCategoryGroupId,
                                                 PCategoryGroupName = catGrp.CategoryGroupName,
                                                 FKUserID = cou.FKUserID,
                                                 DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                             }
                                            )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public CategoryGroupModel GetSingleRecord(long PkCategoryGroupId)
        {

            CategoryGroupModel data = new CategoryGroupModel();
            data = (from cou in __dbContext.TblCategoryGroupMas
                    where cou.PkCategoryGroupId == PkCategoryGroupId
                    select (new CategoryGroupModel
                    {
                        PkCategoryGroupId = cou.PkCategoryGroupId,
                        CategoryGroupName = cou.CategoryGroupName,
                        FkCategoryGroupId = cou.FkCategoryGroupId,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpCategoryGroup(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new CategoryGroupModel { PkCategoryGroupId = 0, CategoryGroupName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkCategoryGroupId,
                        r.CategoryGroupName
                    }).ToList();
        }

        public string DeleteRecord(long PkCategoryGroupId)
        {
            string Error = "";
            CategoryGroupModel obj = GetSingleRecord(PkCategoryGroupId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkCategoryGroupId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCategoryGroupMas
                           where x.PkCategoryGroupId == PkCategoryGroupId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCategoryGroupMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkCategoryGroupId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkCategoryGroupId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetCategoryGroupID(), PkCategoryGroupId, obj.FKCategoryGroupID, obj.DATE_MODIFIED, true);
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
            if (model.PkCategoryGroupId > 0)
            {
                var _entity = __dbContext.TblCategoryGroupMas.Find(model.PkCategoryGroupId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCategoryGroupId = model.PkCategoryGroupId;
            Tbl.CategoryGroupName = model.CategoryGroupName;
            Tbl.FkCategoryGroupId = model.FkCategoryGroupId;
            Tbl.ModifiedDate= DateTime.Now;
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
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Section Group", Fields="CategoryGroupName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Parent Group", Fields="PCategoryGroupName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















