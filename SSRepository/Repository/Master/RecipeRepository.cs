using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class RecipeRepository : Repository<TblRecipeMas>, IRecipeRepository
    {
        public RecipeRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(RecipeModel model, string Mode)
        {
            dynamic cnt;
            string error = "";

            return error;
        }

        public List<RecipeModel> GetList(int pageSize, int pageNo = 1, string search = "", long RecipeGroupId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<RecipeModel> data = (from cou in __dbContext.TblRecipeMas
                                      where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                      orderby cou.PkRecipeId
                                      select (new RecipeModel
                                      {
                                          PkRecipeId = cou.PkRecipeId,
                                          Name = cou.Name,
                                          FKUserID = cou.FKUserID,
                                          DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                      }
                                     )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public RecipeModel GetSingleRecord(long PkRecipeId)
        {

            RecipeModel data = new RecipeModel();
            data = (from cou in __dbContext.TblRecipeMas
                    where cou.PkRecipeId == PkRecipeId
                    select (new RecipeModel
                    {
                        PkRecipeId = cou.PkRecipeId,
                        Name = cou.Name,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        Recipe_dtl = (from ad in __dbContext.TblRecipeDtl
                                      join prd in __dbContext.TblProductMas on ad.FkProductId equals prd.PkProductId
                                      where (ad.FkRecipeId == cou.PkRecipeId)
                                      select (new RecipeDtlModel
                                      {
                                          PkId = ad.PkId,
                                          FkRecipeId = ad.FkRecipeId,
                                          SrNo = ad.SrNo,
                                          TranType = ad.TranType,
                                          FkProductId = ad.FkProductId,
                                          Batch = ad.Batch,
                                          Color = ad.Color,
                                          Qty = ad.Qty,
                                          Product=prd.Product,
                                          Mode=1,
                                      })).ToList(),
                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkRecipeId)
        {
            string Error = "";
            RecipeModel obj = GetSingleRecord(PkRecipeId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkRecipeId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblRecipeMas
                           where x.PkRecipeId == PkRecipeId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblRecipeMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkRecipeId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkRecipeId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetRecipeID(), PkRecipeId, obj.FKRecipeID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            RecipeModel model = (RecipeModel)objmodel;
            string error = "";
            if (model.Recipe_dtl == null)
            {
                error = "Please Insert Product";
            }
            else
            {
                if (model.Recipe_dtl.ToList().Where(x => x.FkProductId == 0 || x.SrNo <= 0 || x.Qty <= 0 || string.IsNullOrEmpty(x.TranType) || string.IsNullOrEmpty(x.Batch) || string.IsNullOrEmpty(x.Color)).ToList().Count > 0)
                {
                    error = "Please Fill Product All Data.";
                }
                else
                {
                    error = isAlreadyExist(model, Mode);
                }
            }
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            RecipeModel model = (RecipeModel)objmodel;
            TblRecipeMas Tbl = new TblRecipeMas();
            if (model.PkRecipeId > 0)
            {
                var _entity = __dbContext.TblRecipeMas.Find(model.PkRecipeId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkRecipeId = model.PkRecipeId;
            Tbl.Name = model.Name;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                Tbl.PkRecipeId = getIdOfSeriesByEntity("PkRecipeId", null, Tbl, "TblRecipeMas");
                AddData(Tbl, false);
            }
            else
            {

                RecipeModel oldModel = GetSingleRecord(Tbl.PkRecipeId);
                ID = Tbl.PkRecipeId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKRecipeID, oldModel.PkRecipeId, oldModel.FKRecipeID, oldModel.DATE_MODIFIED);
            }


            if (model.Recipe_dtl != null)
            {
                List<TblRecipeDtl> lstAdd = new List<TblRecipeDtl>();
                List<TblRecipeDtl> lstEdit = new List<TblRecipeDtl>();
                List<TblRecipeDtl> lstDel = new List<TblRecipeDtl>();
                //List<TblRecipeDtl> lstDel = (from x in __dbContext.TblRecipeDtl
                //                             where x.FkRecipeId == Tbl.PkRecipeId
                //                             select x).ToList();


                foreach (var item in model.Recipe_dtl)
                {

                    TblRecipeDtl locObj = new TblRecipeDtl();
                    locObj.FkRecipeId = Tbl.PkRecipeId;
                    locObj.SrNo = item.SrNo;
                    locObj.TranType = item.TranType;
                    locObj.FkProductId = item.FkProductId;
                    locObj.Batch = item.Batch;
                    locObj.Color = item.Color;
                    locObj.Qty = item.Qty;
                    if (item.Mode == 0)
                    {
                        lstAdd.Add(locObj);
                    }
                    else if (item.Mode == 1 && item.PkId > 0)
                    {
                        locObj.PkId = item.PkId;
                        lstEdit.Add(locObj);
                    }
                    else if (item.Mode == 2 && item.PkId>0)
                    {
                        locObj.PkId = item.PkId;
                        lstDel.Add(locObj);
                    }
                }

                if (lstDel.Count() > 0)
                    DeleteData(lstDel, true);
                if (lstEdit.Count() > 0)
                    UpdateData(lstEdit, true);
                if (lstAdd.Count() > 0)
                    AddData(lstAdd, true);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKRecipeID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl" || GridName.ToString().ToLower() == "rtn")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "ArticalNo", Fields = "Product", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "CD" },
                    new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "Size", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "C" },
                    new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Color", Fields = "Color", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "L" },
                    new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "MRP", Fields = "MRP", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "F.2" },
                    new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "QTY", Fields = "Qty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "F.2" },
                    new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Del", Fields = "Delete", Width = 5, IsActive = 1, SearchType = 0, Sortable = 0, CtrlType = "BD" }
                };
            }
            else
            {
                list = new List<ColumnStructure>
                {
                 //new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 //new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Name", Fields="Name",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
               
            };
            }
            return list;
        }


    }
}



















