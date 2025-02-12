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
                                          DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
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
            BrandModel obj = GetSingleRecord(PkBrandId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkBrandId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblBrandMas
                           where x.PkBrandId == PkBrandId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblBrandMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkBrandId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkBrandId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetBrandID(), PkBrandId, obj.FKBrandID, obj.DATE_MODIFIED, true);
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
                //AddMasterLog(oldModel, __FormID, tblCountry.FKBrandID, oldModel.PkBrandId, oldModel.FKBrandID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBrandID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Brand Name", Fields="BrandName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















