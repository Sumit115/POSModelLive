using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class UnitRepository : Repository<TblUnitMas>, IUnitRepository
    {
        public UnitRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }
       
        public string isAlreadyExist(UnitModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.UnitName))
            {
                cnt = (from x in __dbContext.TblUnitMas
                       where x.UnitName == model.UnitName && x.PkUnitId != model.PkUnitId
                       select x).Count();
                if (cnt > 0)
                    error = "Unit Name Exits";
            }

             
            return error;
        }

        public List<UnitModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<UnitModel> data = (from cou in __dbContext.TblUnitMas
                                          // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                      orderby cou.PkUnitId
                                      select (new UnitModel
                                      {
                                          PkUnitId = cou.PkUnitId,
                                          FKUserId = cou.FKUserID,
                                          FKCreatedByID = cou.FKCreatedByID,
                                          ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                          CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                                          UnitName = cou.UnitName, 
                                      }
                                     )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public UnitModel GetSingleRecord(long PkUnitId)
        {

            UnitModel data = new UnitModel();
            data = (from cou in __dbContext.TblUnitMas
                    where cou.PkUnitId == PkUnitId
                    select (new UnitModel
                    {
                        PkUnitId = cou.PkUnitId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                        UnitName = cou.UnitName,
                       
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpUnit(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);

         //   result.Insert(0, new UnitModel { PkUnitId = 0, UnitName = "Select" });

            return (from r in result
                    select new
                    {
                        r.PkUnitId,
                        r.UnitName
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkUnitId)
        {
            string Error = "";
            UnitModel obj = GetSingleRecord(PkUnitId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkUnitId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblUnitMas
                           where x.PkUnitId == PkUnitId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblUnitMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkUnitId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkUnitId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetUnitID(), PkUnitId, obj.FKUnitID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            UnitModel model = (UnitModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            UnitModel model = (UnitModel)objmodel;
            TblUnitMas Tbl = new TblUnitMas();
            if (model.PkUnitId > 0)
            {
                var _entity = __dbContext.TblUnitMas.Find(model.PkUnitId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkUnitId = model.PkUnitId;
            Tbl.UnitName = model.UnitName;
         
            Tbl.ModifiedDate= DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.FKUserID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                UnitModel oldModel = GetSingleRecord(Tbl.PkUnitId);
                ID = Tbl.PkUnitId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKUnitID, oldModel.PkUnitId, oldModel.FKUnitID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKUnitID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Unit Name", Fields="UnitName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















