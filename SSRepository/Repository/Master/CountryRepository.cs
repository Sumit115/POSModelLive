using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class CountryRepository : Repository<TblCountryMas>, ICountryRepository
    {
        public CountryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(CountryModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.CountryName))
            {
                cnt = (from x in __dbContext.TblCountryMas
                       where x.CountryName == model.CountryName && x.PkCountryId != model.PkCountryId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<CountryModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CountryModel> data = (from cou in __dbContext.TblCountryMas
                                           // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                       orderby cou.PkCountryId
                                       select (new CountryModel
                                       {
                                           PkCountryId = cou.PkCountryId,
                                           FKUserId = cou.FKUserId,
                                           src = cou.Src,
                                           CountryName = cou.CountryName,
                                           CapitalName = cou.CapitalName,
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public CountryModel GetSingleRecord(long PkCountryId)
        {

            CountryModel data = new CountryModel();
            data = (from cou in __dbContext.TblCountryMas
                    where cou.PkCountryId == PkCountryId
                    select (new CountryModel
                    {
                        PkCountryId = cou.PkCountryId,
                        FKUserId = cou.FKUserId,
                        src = cou.Src,
                        CountryName = cou.CountryName,
                        CapitalName = cou.CapitalName,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpCountry(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new CountryModel { PkCountryId = 0, CountryName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkCountryId,
                        r.CountryName
                    }).ToList();
        }
        public object GetDrpTableCountry(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkCountryId,
                        r.CountryName,
                        r.CapitalName
                    }).ToList();
        }

        public string DeleteRecord(long PkCountryId)
        {
            string Error = "";
            CountryModel obj = GetSingleRecord(PkCountryId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkCountryId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCountryMas
                           where x.PkCountryId == PkCountryId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCountryMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkCountryId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkCountryId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetCountryID(), PkCountryId, obj.FKCountryID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            CountryModel model = (CountryModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CountryModel model = (CountryModel)objmodel;
            TblCountryMas Tbl = new TblCountryMas();
            if (model.PkCountryId > 0)
            {
                var _entity = __dbContext.TblCountryMas.Find(model.PkCountryId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCountryId = model.PkCountryId;
            Tbl.CountryName = model.CountryName;
            Tbl.CapitalName = model.CapitalName;
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

                CountryModel oldModel = GetSingleRecord(Tbl.PkCountryId);
                ID = Tbl.PkCountryId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKCountryID, oldModel.PkCountryId, oldModel.FKCountryID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCountryID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Country", Fields="CountryName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =1, Heading ="Capital", Fields="CapitalName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                        };
            return list;
        }


    }
}



















