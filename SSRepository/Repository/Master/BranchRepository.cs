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
        public BranchRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            ;
        }

        public string isAlreadyExist(BranchModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblBranchMas
                       where x.Mobile == model.Mobile && x.PkBranchId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblBranchMas
                       where x.Email == model.Email && x.PkBranchId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.BranchCode))
            {
                cnt = (from x in __dbContext.TblBranchMas
                       where x.BranchCode == model.BranchCode && x.PkBranchId != model.PKID
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
                                      where (EF.Functions.Like(cou.BranchName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                      orderby cou.BranchName
                                      select (new BranchModel
                                      {
                                          PKID = cou.PkBranchId,
                                          BranchName = cou.BranchName,
                                          ContactPerson = cou.ContactPerson,
                                          Email = cou.Email,
                                          Mobile = cou.Mobile,
                                          Address = cou.Address,
                                          FkCityId = cou.FkCityId,
                                          City = cou.FKCity.CityName,
                                          State = cou.State,
                                          Pin = cou.Pin,
                                          Country = cou.Country,
                                          FkRegId = cou.FkRegId,
                                          BranchCode = cou.BranchCode,
                                          Location = cou.Location,
                                          Image1 = cou.Image1,
                                          FKUserID = cou.FKUserID,
                                          UserName = cou.FKUser.UserId,
                                          DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
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
                        PKID = cou.PkBranchId,
                        BranchName = cou.BranchName,
                        ContactPerson = cou.ContactPerson,
                        Email = cou.Email,
                        Mobile = cou.Mobile,
                        Address = cou.Address,
                        FkCityId = cou.FkCityId,
                        City = cou.FKCity.CityName,
                        State = cou.State,
                        Pin = cou.Pin,
                        Country = cou.Country,
                        FkRegId = cou.FkRegId,
                        BranchCode = cou.BranchCode,
                        Location = cou.Location,
                        Image1 = cou.Image1,
                        FKUserID = cou.FKUserID,
                        UserName = cou.FKUser.UserId,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
               
                return (from cou in __dbContext.TblBranchMas
                        where (EF.Functions.Like(cou.BranchName.Trim().ToLower(), Convert.ToString(search) + "%"))
                        orderby cou.BranchName
                        select new
                        {
                            cou.PkBranchId,
                            cou.BranchName
                        }).ToList();
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PkBranchId)
        {
            string Error = "";
            BranchModel oldModel = GetSingleRecord(PkBranchId);

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

                AddMasterLog((long)Handler.Form.Branch, PkBranchId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.BranchName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            BranchModel model = (BranchModel)objmodel;
            string error = "nbbnn";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            BranchModel model = (BranchModel)objmodel;
            TblBranchMas Tbl = new TblBranchMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblBranchMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkBranchId = model.PKID;
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
            Tbl.Image1 = model.Image1;
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

                BranchModel oldModel = GetSingleRecord(Tbl.PkBranchId);
                ID = Tbl.PkBranchId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Branch, Tbl.PkBranchId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.BranchName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBranchID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Branch Code", Fields="BranchCode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Branch Name", Fields="BranchName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Contact Person", Fields="ContactPerson",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Email", Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Mobile", Fields="Mobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Location", Fields="Location",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Address", Fields="Address",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="State", Fields="State",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="City", Fields="City",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Pin", Fields="Pin",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
      };
            return list;
        }


    }
}



















