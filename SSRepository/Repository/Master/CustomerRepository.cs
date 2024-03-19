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
        public string isAlreadyExist(CustomerModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Mobile == model.Mobile && x.PkCustomerId != model.PkCustomerId
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblCustomerMas
                       where x.Email == model.Email && x.PkCustomerId != model.PkCustomerId
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits";
            }
            return error;
        }

        public List<CustomerModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CustomerModel> data = (from cou in __dbContext.TblCustomerMas
                                        join _city in __dbContext.TblCityMas
                                       on new { User = cou.FkCityId } equals new { User = (int?)_city.PkCityId }
                                       into _citytmp
                                        from city in _citytmp.DefaultIfEmpty()
                                            // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkCustomerId
                                        select (new CustomerModel
                                        {
                                            PkCustomerId = cou.PkCustomerId,
                                            Code = cou.Code,
                                            Name = cou.Name,
                                            FKUserId = cou.FKUserId,
                                            src = cou.Src,
                                            DATE_MODIFIED = cou.DateModified,
                                            DATE_CREATED = cou.DateCreated,
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


        public CustomerModel GetSingleRecord(long PkCustomerId)
        {

            CustomerModel data = new CustomerModel();
            data = (from cou in __dbContext.TblCustomerMas
                    where cou.PkCustomerId == PkCustomerId
                    select (new CustomerModel
                    {
                        PkCustomerId = cou.PkCustomerId,
                        Code = cou.Code,
                        Name = cou.Name,
                        FKUserId = cou.FKUserId,
                        src = cou.Src,
                        DATE_MODIFIED = cou.DateModified,
                        DATE_CREATED = cou.DateCreated,
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

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpCustomer(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkCustomerId,
                        r.Name
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkCustomerId)
        {
            string Error = "";
            CustomerModel obj = GetSingleRecord(PkCustomerId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkCustomerId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCustomerMas
                           where x.PkCustomerId == PkCustomerId
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

            CustomerModel model = (CustomerModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CustomerModel model = (CustomerModel)objmodel;
            TblCustomerMas Tbl = new TblCustomerMas();
            if (model.PkCustomerId > 0)
            {
                var _entity = __dbContext.TblCustomerMas.Find(model.PkCustomerId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCustomerId = model.PkCustomerId;
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
            Tbl.DateModified = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.Src = model.src;
                Tbl.Code = model.Code;
                Tbl.FKUserId = model.FKUserId;
                Tbl.IsAadharVerify = 0;
                Tbl.IsPanVerify = 0;
                Tbl.Status = 1;
                Tbl.DateCreated = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                CustomerModel oldModel = GetSingleRecord(Tbl.PkCustomerId);
                ID = Tbl.PkCustomerId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKCustomerID, oldModel.PkCustomerId, oldModel.FKCustomerID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCustomerID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
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
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="DateCreated",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Code", Fields="Code",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Name", Fields="Name",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                //new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Father Name", Fields="FatherName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                //new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Mother Name", Fields="MotherName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                //new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Marital", Fields="Marital",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                //new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Gender", Fields="Gender",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Dob", Fields="Dob",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Email", Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Mobile", Fields="Mobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Aadhar", Fields="Aadhar",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Panno", Fields="Panno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Gstno", Fields="Gstno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
 };
            return list;
        }


    }
}



















