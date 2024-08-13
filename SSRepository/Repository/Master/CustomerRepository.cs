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
        public CustomerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public string isAlreadyExist(PartyModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Mobile == model.Mobile && x.PkCustomerId != model.PkId
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits !";
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Email == model.Email && x.PkCustomerId != model.PkId
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits !";
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Code == model.Code && x.PkCustomerId != model.PkId
                       select x).Count();
                if (cnt > 0)
                    error = "ALIAS Already Exits !";
            }
            return error;
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
                                         PkId = cou.PkCustomerId,
                                         Code = cou.Code,
                                         Name = cou.Name,
                                         FKUserId = cou.FKUserID,
                                         FKCreatedByID = cou.FKCreatedByID,
                                         ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                         CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
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
                                         City = city.CityName,
                                         Pin = cou.Pin,
                                     }
                                    )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public List<object> CustomList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            var data = (from cou in __dbContext.TblCustomerMas
                        join _city in __dbContext.TblCityMas
                       on new { User = cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                       into _citytmp
                        from city in _citytmp.DefaultIfEmpty()
                        where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                        orderby cou.PkCustomerId
                        select (new
                        {
                            cou.PkCustomerId,
                            cou.Code,
                            cou.Name,
                            cou.StateName,
                            cou.Address,
                            cou.Mobile,
                            cou.Email
                        }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data.Select(x => (object)x).ToList();
        }

        public PartyModel GetSingleRecord(long PkCustomerId)
        {

            PartyModel data = new PartyModel();
            data = (from cou in __dbContext.TblCustomerMas
                    where cou.PkCustomerId == PkCustomerId
                    select (new PartyModel
                    {
                        PkId = cou.PkCustomerId,
                        Code = cou.Code,
                        Name = cou.Name,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
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

                    })).FirstOrDefault();
            return data;
        }
          public object GetDrpCustomer(int pageSize, int pageNo = 1, string search = "")
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
                                     select  new
                                     {
                                         PkId = cou.PkCustomerId,
                                         Name = cou.Name,
                                         Code = cou.Code,
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
                                     }
                                    ).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
             
        }

        public string DeleteRecord(long PkId)
        {
            string Error = "";
            PartyModel obj = GetSingleRecord(PkId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkCustomerId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCustomerMas
                           where x.PkCustomerId == PkId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCustomerMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkCustomerId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkCustomerId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetCustomerID(), PkCustomerId, obj.FKCustomerID, obj.DATE_MODIFIED, true);
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
            TblCustomerMas Tbl = new TblCustomerMas();
            if (model.PkId > 0)
            {
                var _entity = __dbContext.TblCustomerMas.Find(model.PkId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCustomerId = model.PkId;
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
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.Code = model.Code;
                Tbl.FKUserID = model.FKUserId;
                Tbl.IsAadharVerify = 0;
                Tbl.IsPanVerify = 0;
                Tbl.Status = 1;
                Tbl.CreationDate = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj); 
                Tbl.FkAccountID = SaveAndGetAccountId(model); 
                AddData(Tbl, false);
            }
            else
            {

                PartyModel oldModel = GetSingleRecord(Tbl.PkCustomerId);
                ID = Tbl.PkCustomerId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKCustomerID, oldModel.PkCustomerId, oldModel.FKCustomerID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCustomerID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        private long SaveAndGetAccountId(PartyModel model)
        {
            object md = new AccountMasModel()
            {
                PkAccountId = 0,
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
                //ModifiedDate = DateTime.Now,
                FKUserId = model.FKUserId,
                FKCreatedByID = model.FKCreatedByID,
            };

            long ID = 0;
            try
            {
                new AccountMasRepository(__dbContext).SaveBaseData(ref md, "Create", ref ID);

            }
            catch (Exception ex) { }
            return ID;
        }
        public DataTable AutoDropDown()
        {
            DataTable record = new DataTable("record");
            var data = (from cou in __dbContext.TblCustomerMas
                            // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                        orderby cou.PkCustomerId
                        select (new
                        {
                            pk_Id = cou.PkCustomerId,
                            Field1 = cou.Name,
                            Field2 = cou.Mobile,
                            //  Field3 = "",
                        }
                       )).ToList();

            record = ToDataTable(data);
            return record;
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Code", Fields="Code",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Name", Fields="Name",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Dob", Fields="Dob",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Email", Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Mobile", Fields="Mobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Aadhar", Fields="Aadhar",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Panno", Fields="Panno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Gstno", Fields="Gstno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
 };
            return list;
        }


    }
}



















