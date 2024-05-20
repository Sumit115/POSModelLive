using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class DistrictRepository : Repository<TblDistrictMas>, IDistrictRepository
    {
        public DistrictRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(DistrictModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.DistrictName))
            {
                cnt = (from x in __dbContext.TblDistrictMas
                       where x.DistrictName == model.DistrictName && x.PkDistrictId != model.PkDistrictId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<DistrictModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<DistrictModel> data = (from cou in __dbContext.TblDistrictMas
                                        join catGrp in __dbContext.TblStateMas on cou.FkStateId equals catGrp.PkStateId

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkDistrictId
                                        select (new DistrictModel
                                        {
                                            PkDistrictId = cou.PkDistrictId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            DistrictName = cou.DistrictName,
                                            FkStateId = cou.FkStateId,
                                            StateName = catGrp.StateName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<DistrictModel> GetListByGroupId(long StateId, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<DistrictModel> data = (from cou in __dbContext.TblDistrictMas
                                        join catGrp in __dbContext.TblStateMas on cou.FkStateId equals catGrp.PkStateId
                                        where cou.FkStateId == StateId
                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkDistrictId
                                        select (new DistrictModel
                                        {
                                            PkDistrictId = cou.PkDistrictId,
                                            FKUserId = cou.FKUserID,
                                            FKCreatedByID = cou.FKCreatedByID,
                                            DistrictName = cou.DistrictName,
                                            FkStateId = cou.FkStateId,
                                            StateName = catGrp.StateName,
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public DistrictModel GetSingleRecord(long PkDistrictId)
        {

            DistrictModel data = new DistrictModel();
            data = (from cou in __dbContext.TblDistrictMas
                    join catGrp in __dbContext.TblStateMas on cou.FkStateId equals catGrp.PkStateId
                    where cou.PkDistrictId == PkDistrictId
                    select (new DistrictModel
                    {
                        PkDistrictId = cou.PkDistrictId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        DistrictName = cou.DistrictName,
                        FkStateId = cou.FkStateId,
                        StateName = catGrp.StateName,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpDistrict(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new DistrictModel { PkDistrictId = 0, DistrictName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkDistrictId,
                        r.DistrictName
                    }).ToList();
        }
        public object GetDrpDistrictByStateId(long StateId, int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetListByGroupId(StateId, pagesize, pageno, search);

            result.Insert(0, new DistrictModel { PkDistrictId = 0, DistrictName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkDistrictId,
                        r.DistrictName
                    }).ToList();
        }
        public object GetDrpTableDistrict(int pagesize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            return (from r in result
                    select new
                    {
                        r.PkDistrictId,
                        District = r.DistrictName,
                        State = r.StateName, 
                    }).ToList();
        }

        public string DeleteRecord(long PkDistrictId)
        {
            string Error = "";
            DistrictModel obj = GetSingleRecord(PkDistrictId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkDistrictId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblDistrictMas
                           where x.PkDistrictId == PkDistrictId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblDistrictMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkDistrictId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkDistrictId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetDistrictID(), PkDistrictId, obj.FKDistrictID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            DistrictModel model = (DistrictModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            DistrictModel model = (DistrictModel)objmodel;
            TblDistrictMas Tbl = new TblDistrictMas();
            if (model.PkDistrictId > 0)
            {
                var _entity = __dbContext.TblDistrictMas.Find(model.PkDistrictId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkDistrictId = model.PkDistrictId;
            Tbl.DistrictName = model.DistrictName;
            Tbl.FkStateId = model.FkStateId;
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

                DistrictModel oldModel = GetSingleRecord(Tbl.PkDistrictId);
                ID = Tbl.PkDistrictId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKDistrictID, oldModel.PkDistrictId, oldModel.FKDistrictID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKDistrictID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="State", Fields="StateName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="District", Fields="DistrictName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                        };
            return list;
        }


    }
}



















