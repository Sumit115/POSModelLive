using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using Azure;

namespace SSRepository.Repository.Master
{
    public class CustomerRepository : Repository<TblCustomerMas>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public List<PartyModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<PartyModel> data = (from cou in __dbContext.TblCustomerMas
                                     join _city in __dbContext.TblCityMas
                                    on new { User = cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                                    into _citytmp
                                     from city in _citytmp.DefaultIfEmpty()
                                     where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     orderby cou.PkCustomerId
                                     select (new PartyModel
                                     {
                                         PKID = cou.PkCustomerId,
                                         Code = cou.Code,
                                         Name = cou.Name,
                                         Marital = cou.Marital,
                                         Gender = cou.Gender,
                                         Dob = cou.Dob,
                                         Email = cou.Email,
                                         Mobile = cou.Mobile,
                                         Aadhar = cou.Aadhar,
                                         Panno = cou.Panno,
                                         Gstno = cou.Gstno,
                                         IsAadharVerify = cou.IsAadharVerify,
                                         IsPanVerify = cou.IsPanVerify,
                                         Status = cou.Status,
                                         Address = cou.Address,
                                         StateName = cou.StateName,
                                         FkCityId = cou.FkCityId,
                                         City = city.CityName,
                                         Pin = cou.Pin,
                                         FKUserID = cou.FKUserID,
                                         DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblCustomerMas
                         where EF.Functions.Like(cou.Name.Trim().ToLower(), search + "%")
                         orderby cou.Name
                         select (new
                         {
                             cou.PkCustomerId,
                             PkId = cou.PkCustomerId,
                             cou.Name,
                             cou.Code,
                             cou.Email,
                             cou.Mobile,
                             cou.Address,
                             cou.StateName,
                             cou.Pin,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else if (EnCustomFlag == (int)Handler.en_CustomFlag.Filter)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblCustomerMas
                         where EF.Functions.Like(cou.Name.Trim().ToLower(), search + "%")
                         orderby cou.Name
                         select (new
                         {
                             cou.PkCustomerId,
                             PkId = cou.PkCustomerId,
                             cou.Name,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public PartyModel GetSingleRecord(long PkCustomerId)
        {

            PartyModel data = (from cou in __dbContext.TblCustomerMas
                               where cou.PkCustomerId == PkCustomerId
                               select (new PartyModel
                               {
                                   PKID = cou.PkCustomerId,
                                   Code = cou.Code,
                                   Name = cou.Name,
                                   Marital = cou.Marital,
                                   Gender = cou.Gender,
                                   Dob = cou.Dob,
                                   Email = cou.Email,
                                   Mobile = cou.Mobile,
                                   Aadhar = cou.Aadhar,
                                   Panno = cou.Panno,
                                   Gstno = cou.Gstno,
                                   Passport = cou.Passport,
                                   AadharCardFront = cou.AadharCardFront,
                                   AadharCardBack = cou.AadharCardBack,
                                   PanCard = cou.PanCard,
                                   Signature = cou.Signature,
                                   IsAadharVerify = cou.IsAadharVerify,
                                   IsPanVerify = cou.IsPanVerify,
                                   Status = cou.Status,
                                   Address = cou.Address,
                                   StateName = cou.StateName,
                                   FkCityId = cou.FkCityId,
                                   // City = city.CityName,
                                   Pin = cou.Pin,
                                   Disc = cou.Disc,
                                   FKUserID = cou.FKUserID,
                                   DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")

                               })).SingleOrDefault();
            return data;
        }

        public object CustomDropDown(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            return (from cou in __dbContext.TblCustomerMas
                    join _city in __dbContext.TblCityMas
                   on new { User = cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                   into _citytmp
                    from city in _citytmp.DefaultIfEmpty()
                    where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                    orderby cou.PkCustomerId
                    select new
                    {
                        PkId = cou.PkCustomerId,
                        Name = cou.Name,
                        Code = cou.Code,
                        //Marital = cou.Marital,
                        //Gender = cou.Gender,
                        //Dob = cou.Dob,
                        Email = cou.Email,
                        Mobile = cou.Mobile,
                        //Aadhar = cou.Aadhar,
                        //Panno = cou.Panno,
                        //Gstno = cou.Gstno,
                        //IsAadharVerify = cou.IsAadharVerify,
                        //IsPanVerify = cou.IsPanVerify,
                        //Status = cou.Status,
                        Address = cou.Address,
                        //StateName = cou.StateName,
                        //FkCityId = cou.FkCityId,
                        City = city.CityName,
                        // Pin = cou.Pin,
                    }
                                     ).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        }

        public string DeleteRecord(long PKID)
        {
            string Error = "";
            PartyModel oldModel = GetSingleRecord(PKID);

            var saleOrderExist = (from cou in __dbContext.TblSalesOrdertrn
                                  join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                                  where cou.FkPartyId == PKID && ser.TranAlias == "SORD"
                                  select cou).Count();
            if (saleOrderExist > 0)
                Error += "use in other transaction";

            if (Error == "") {
                var saleInvoiceExist = (from cou in __dbContext.TblSalesInvoicetrn
                                      join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                                      where cou.FkPartyId == PKID && ser.TranAlias == "SINV"
                                      select cou).Count();
                if (saleInvoiceExist > 0)
                    Error += "use in other transaction";
            }
            if (Error == "")
            {
                var saleCrNoteExist = (from cou in __dbContext.TblSalesCrNotetrn
                                        join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                                        where cou.FkPartyId == PKID && (ser.TranAlias == "SCRN" || ser.TranAlias == "SRTN")
                                        select cou).Count();
                if (saleCrNoteExist > 0)
                    Error += "use in other transaction";
            }
            if (Error == "")
            {
                var saleChallanExist = (from cou in __dbContext.TblSalesChallantrn
                                        join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                                        where cou.FkPartyId == PKID && ser.TranAlias == "SPSL" 
                                        select cou).Count();
                if (saleChallanExist > 0)
                    Error += "use in other transaction";
            }

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCustomerMas
                           where x.PkCustomerId == PKID
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCustomerMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Customer, PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Name, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }

        public override string ValidateData(object objmodel, string Mode)
        {

            PartyModel model = (PartyModel)objmodel;
            string error = "";
            if (string.IsNullOrEmpty(model.Mobile))
            {
                error += "Enter Mobile";
            }
            if (string.IsNullOrEmpty(model.StateName))
            {
                error += "Select State";
            }
            if (error == "")
            {
                error += isAlreadyExist(model, Mode);
            }
            return error;

        }


        private string isAlreadyExist(PartyModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Mobile == model.Mobile && x.PkCustomerId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits !";
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Email == model.Email && x.PkCustomerId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits !";
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Code == model.Code && x.PkCustomerId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "ALIAS Already Exits !";
            }
            return error;
        }

        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            PartyModel model = (PartyModel)objmodel;
            TblCustomerMas Tbl = new TblCustomerMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblCustomerMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCustomerId = model.PKID;
            Tbl.Name = model.Name;
            Tbl.Code = model.Code;
            Tbl.Marital = model.Marital;
            Tbl.Gender = model.Gender;
            Tbl.Dob = model.Dob;
            Tbl.Email = model.Email;
            Tbl.Mobile = model.Mobile;
            Tbl.Aadhar = model.Aadhar;
            Tbl.Panno = model.Panno;
            Tbl.Gstno = model.Gstno;
            Tbl.Passport = model.Passport;
            Tbl.AadharCardFront = model.AadharCardFront;
            Tbl.AadharCardBack = model.AadharCardBack;
            Tbl.PanCard = model.PanCard;
            Tbl.Signature = model.Signature;
            Tbl.Address = model.Address;
            Tbl.FkCityId = model.FkCityId;
            Tbl.StateName = model.StateName;
            Tbl.Pin = model.Pin;
            Tbl.Disc = model.Disc;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                Tbl.Code = model.Code;
                Tbl.IsAadharVerify = 0;
                Tbl.IsPanVerify = 0;
                Tbl.Status = 1;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj); 
                Tbl.FkAccountID = SaveAndGetAccountId(model);
                AddData(Tbl, false);
            }
            else
            {

                PartyModel oldModel = GetSingleRecord(Tbl.PkCustomerId);
                ID = Tbl.PkCustomerId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Customer, Tbl.PkCustomerId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Name, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCustomerID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }

        private long SaveAndGetAccountId(PartyModel model)
        {
            object md = new AccountMasModel()
            {
                PKID = 0,
                Account = model.Name,
                FkAccountGroupId = 2,
                //Station = model.Station,
                //Locality = model.Locality,
                //Alias = model.Alias,
                Address = model.Address,
                Pincode = model.Pin,
                Phone1 = model.Mobile,
                //Phone2 = model.Phone2,
                Email = model.Email,
                //ApplyCostCenter = model.ApplyCostCenter,
                //ApplyTCS = model.ApplyTCS,
                //ApplyTDS = model.ApplyTDS,
                Status = "Continue",
                //DiscDate = model.DiscDate,
                //FKBankID = model.FKBankID,
                //AccountNo = model.AccountNo,
            };

            long ID = 0;
            try
            {
                new AccountMasRepository(__dbContext, _contextAccessor).SaveBaseData(ref md, "Create", ref ID);

            }
            catch (Exception ex) { }
            return ID;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=index++, Orderby =index++,  Heading ="Name", Fields="Name",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++,  Heading ="Code", Fields="Code",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Address", Fields="Address",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Email", Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Mobile", Fields="Mobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Aadhar", Fields="Aadhar",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Gstno", Fields="Gstno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Panno", Fields="Panno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="State", Fields="StateName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="City", Fields="City",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Dob", Fields="Dob",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="User Name", Fields="Create Date",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















