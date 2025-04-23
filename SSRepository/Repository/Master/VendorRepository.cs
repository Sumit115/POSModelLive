using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Azure;

namespace SSRepository.Repository.Master
{
    public class VendorRepository : Repository<TblVendorMas>, IVendorRepository
    {
        public VendorRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(PartyModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblVendorMas
                       where x.Mobile == model.Mobile && x.PkVendorId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblVendorMas
                       where x.Email == model.Email && x.PkVendorId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits";
            }
            return error;
        }

        public List<PartyModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<PartyModel> data = (from cou in __dbContext.TblVendorMas
                                         //join _city in __dbContext.TblCityMas
                                         // on new { User = (int?)cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                                         // into _citytmp
                                         //from city in _citytmp.DefaultIfEmpty()
                                          where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                     orderby cou.PkVendorId
                                     select (new PartyModel
                                     {
                                         PKID = cou.PkVendorId,
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
                                         //Passport = cou.Passport,
                                         //AadharCardFront = cou.AadharCardFront,
                                         //AadharCardBack = cou.AadharCardBack,
                                         //PanCard = cou.PanCard,
                                         //Signature = cou.Signature,
                                         IsAadharVerify = cou.IsAadharVerify,
                                         IsPanVerify = cou.IsPanVerify,
                                         Status = cou.Status,
                                         Address = cou.Address,
                                         StateName = cou.StateName,
                                         FkCityId = cou.FkCityId,
                                         City = cou.FKCity.CityName,
                                         Pin = cou.Pin,
                                         FKUserID = cou.FKUserID,
                                         DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                         UserName = cou.FKUser.UserId,
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public PartyModel GetSingleRecord(long PkVendorId)
        {

            PartyModel data = new PartyModel();
            data = (from cou in __dbContext.TblVendorMas
                    where cou.PkVendorId == PkVendorId
                    select (new PartyModel
                    {
                        PKID = cou.PkVendorId,
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
                        City = cou.FKCity.CityName,
                        Pin = cou.Pin,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpVendor(int pageSize, int pageNo = 1, string search = "")
        {
            if (search == null) search = "";
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            return (from cou in __dbContext.TblVendorMas
                    join _city in __dbContext.TblCityMas
                     on new { User = (int?)cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                     into _citytmp
                    from city in _citytmp.DefaultIfEmpty()
                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                    orderby cou.PkVendorId
                    select (new
                    {
                        cou.PkVendorId,
                        cou.Name,
                        cou.Code,
                        cou.Email,
                        cou.Mobile,
                        //cou.Aadhar,
                        //cou.Panno,
                        //cou.Gstno,
                        //cou.IsAadharVerify,
                        //cou.IsPanVerify,
                        //cou.Status,
                        cou.Address,
                        cou.StateName,
                        // cou.FkCityId, 
                        cou.Pin,
                    }
                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblVendorMas
                         where EF.Functions.Like(cou.Name.Trim().ToLower(), search + "%")
                         orderby cou.Name
                         select (new
                         {
                             cou.PkVendorId,
                             PkId = cou.PkVendorId,
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
                return ((from cou in __dbContext.TblVendorMas
                         where EF.Functions.Like(cou.Name.Trim().ToLower(), search + "%")
                         orderby cou.Name
                         select (new
                         {
                             cou.PkVendorId,
                             PkId = cou.PkVendorId,
                             cou.Name,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public string DeleteRecord(long PKID)
        {
            string Error = "";
            PartyModel oldModel = GetSingleRecord(PKID);
            var purchaseOrderExist = (from cou in __dbContext.TblPurchaseOrdertrn
                                      join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                                      where cou.FkPartyId == PKID && ser.TranAlias == "PORD"
                                      select cou).Count();
            if (purchaseOrderExist > 0)
                Error += "use in other transaction";

            if (Error == "")
            {
                var purchaseInvoiceExist = (from cou in __dbContext.TblPurchaseInvoicetrn
                                            join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                                            where cou.FkPartyId == PKID && ser.TranAlias == "PINV"
                                            select cou).Count();
                if (purchaseInvoiceExist > 0)
                    Error += "use in other transaction";
            }
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblVendorMas
                           where x.PkVendorId == PKID
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblVendorMas.RemoveRange(lst);


                AddMasterLog((long)Handler.Form.Vendor, PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.Name, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            PartyModel model = (PartyModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            PartyModel model = (PartyModel)objmodel;
            TblVendorMas Tbl = new TblVendorMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblVendorMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkVendorId = model.PKID;
            Tbl.Code = model.Code;
            Tbl.Name = model.Name;
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

                PartyModel oldModel = GetSingleRecord(Tbl.PkVendorId);
                ID = Tbl.PkVendorId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Vendor, Tbl.PkVendorId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Name, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKVendorID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Name", Fields="Name",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Code", Fields="Code",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Address", Fields="Address",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Email", Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Mobile", Fields="Mobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Aadhar", Fields="Aadhar",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Gstno", Fields="Gstno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Panno", Fields="Panno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="State", Fields="StateName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="City", Fields="City",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Dob", Fields="Dob",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
         };
            return list;
        }


        private long SaveAndGetAccountId(PartyModel model)
        {
            object md = new AccountMasModel()
            {
                PKID = 0,
                Account = model.Name,
                FkAccountGroupId = 1,
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
                //ModifiedDate = DateTime.Now,
                FKUserID = GetUserID(),
            };

            long ID = 0;
            try
            {
                new AccountMasRepository(__dbContext, _contextAccessor).SaveBaseData(ref md, "Create", ref ID);

            }
            catch (Exception ex) { }
            return ID;
        }



    }
}



















