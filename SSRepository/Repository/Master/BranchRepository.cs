using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class BranchRepository : Repository<TblBranchMas>, IBranchRepository
    {
        public BranchRepository(AppDbContext dbContext) : base(dbContext)
        {;
        }

        public string isAlreadyExist(BranchModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblBranchMas
                       where x.Mobile == model.Mobile && x.PkBranchId != model.PkBranchId
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblBranchMas
                       where x.Email == model.Email && x.PkBranchId != model.PkBranchId
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.BranchCode))
            {
                cnt = (from x in __dbContext.TblBranchMas
                       where x.BranchCode == model.BranchCode && x.PkBranchId != model.PkBranchId
                       select x).Count();
                if (cnt > 0)
                    error = "BranchCode Already Exits";
            }
            return error;
        }

        public List<BranchModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<BranchModel> data = (from cou in __dbContext.TblBranchMas
                                      join _city in __dbContext.TblCityMas
                                       on new { User = cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                                       into _citytmp from city in _citytmp.DefaultIfEmpty()
                                          // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                      orderby cou.PkBranchId
                                      select (new BranchModel
                                      {
                                          PkBranchId = cou.PkBranchId,
                                          FKUserId = cou.FKUserId,
                                          src = cou.Src,
                                          DATE_MODIFIED = cou.DateModified,
                                          DATE_CREATED = cou.DateCreated,
                                          BranchName = cou.BranchName,
                                          ContactPerson = cou.ContactPerson,
                                          Email = cou.Email,
                                          Mobile = cou.Mobile,
                                          Address = cou.Address,
                                          FkCityId = cou.FkCityId,
                                          City = city.CityName,
                                          State = cou.State,
                                          Pin = cou.Pin,
                                          Country = cou.Country,
                                          FkRegId = cou.FkRegId,
                                          BranchCode = cou.BranchCode,
                                          Location = cou.Location, 
                                      }
                                     )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public BranchModel GetSingleRecord(long PkBranchId)
        {

            BranchModel data = new BranchModel();
            data = (from cou in __dbContext.TblBranchMas
                    where cou.PkBranchId == PkBranchId
                    select (new BranchModel
                    {
                        PkBranchId = cou.PkBranchId,
                        FKUserId = cou.FKUserId,
                        src = cou.Src,
                        DATE_MODIFIED = cou.DateModified,
                        DATE_CREATED = cou.DateCreated,
                        BranchName = cou.BranchName,
                        ContactPerson = cou.ContactPerson,
                        Email = cou.Email,
                        Mobile = cou.Mobile,
                        Address = cou.Address,
                        FkCityId = cou.FkCityId,
                        State = cou.State,
                        Pin = cou.Pin,
                        Country = cou.Country,
                        FkRegId = cou.FkRegId,
                        BranchCode = cou.BranchCode,
                        Location = cou.Location,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpBranch(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkBranchId,
                        r.BranchName
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkBranchId)
        {
            string Error = "";
            BranchModel obj = GetSingleRecord(PkBranchId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkBranchId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblBranchMas
                           where x.PkBranchId == PkBranchId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblBranchMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkBranchId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkBranchId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetBranchID(), PkBranchId, obj.FKBranchID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            BranchModel model = (BranchModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            BranchModel model = (BranchModel)objmodel;
            TblBranchMas Tbl = new TblBranchMas();
            if (model.PkBranchId > 0)
            {
                var _entity = __dbContext.TblBranchMas.Find(model.PkBranchId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkBranchId = model.PkBranchId;
            Tbl.BranchName = model.BranchName;
            Tbl.ContactPerson = model.ContactPerson;
            Tbl.Email = model.Email;
            Tbl.Mobile = model.Mobile;
            Tbl.Address = model.Address;
            Tbl.FkCityId = model.FkCityId;
            Tbl.State = model.State;
            Tbl.Pin = model.Pin;
            Tbl.Country = model.Country;
            Tbl.FkRegId = model.FkRegId;
            Tbl.BranchCode = model.BranchCode;
            Tbl.Location = model.Location;
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

                BranchModel oldModel = GetSingleRecord(Tbl.PkBranchId);
                ID = Tbl.PkBranchId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKBranchID, oldModel.PkBranchId, oldModel.FKBranchID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBranchID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="DateCreated",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Branch Code", Fields="BranchCode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Branch Name", Fields="BranchName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Contact Person", Fields="ContactPerson",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Email", Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Mobile", Fields="Mobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=55, Orderby =6, Heading ="Location", Fields="Location",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=6, Orderby =7, Heading ="Address", Fields="Address",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="State", Fields="State",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=7, Orderby =9, Heading ="City", Fields="City",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=9, Orderby =10, Heading ="Pin", Fields="Pin",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  //new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Country", Fields="Country",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     };
            return list;
        }


    }
}



















