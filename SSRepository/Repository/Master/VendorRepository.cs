using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Data;

namespace SSRepository.Repository.Master
{
    public class VendorRepository : Repository<TblVendorMas>, IVendorRepository
    {
        public VendorRepository(AppDbContext dbContext) : base(dbContext)
        {
            __FormID = (long)en_Form.Vendor;
        }
       
        public string isAlreadyExist(VendorModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                cnt = (from x in __dbContext.TblVendorMas
                       where x.Mobile == model.Mobile && x.PkVendorId != model.PkVendorId
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits";
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                cnt = (from x in __dbContext.TblVendorMas
                       where x.Email == model.Email && x.PkVendorId != model.PkVendorId
                       select x).Count();
                if (cnt > 0)
                    error = "Email Already Exits";
            }
            return error;
        }

        public List<VendorModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<VendorModel> data = (from cou in __dbContext.TblVendorMas
                                            // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                        orderby cou.PkVendorId
                                        select (new VendorModel
                                        {
                                            PkVendorId = cou.PkVendorId,
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
                                        }
                                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public VendorModel GetSingleRecord(long PkVendorId)
        {

            VendorModel data = new VendorModel();
            data = (from cou in __dbContext.TblVendorMas
                    where cou.PkVendorId == PkVendorId
                    select (new VendorModel
                    {
                        PkVendorId = cou.PkVendorId,
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

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpVendor(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkVendorId,
                        r.Name
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkVendorId)
        {
            string Error = "";
            VendorModel obj = GetSingleRecord(PkVendorId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkVendorId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblVendorMas
                           where x.PkVendorId == PkVendorId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblVendorMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkVendorId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkVendorId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetVendorID(), PkVendorId, obj.FKVendorID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            VendorModel model = (VendorModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            VendorModel model = (VendorModel)objmodel;
            TblVendorMas Tbl = new TblVendorMas();
            if (model.PkVendorId > 0)
            {
                var _entity = __dbContext.TblVendorMas.Find(model.PkVendorId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkVendorId = model.PkVendorId;
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
            Tbl.DateModified = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.Src = model.src;
                Tbl.Code = model.Code;
                Tbl.FKUserId = model.FKUserId;
                Tbl.IsAadharVerify = 0;
                Tbl.IsPanVerify = 0;
                Tbl.Status = 1;
                Tbl.DateCreated= DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {
                
                 VendorModel oldModel = GetSingleRecord(Tbl.PkVendorId);
                ID = Tbl.PkVendorId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKVendorID, oldModel.PkVendorId, oldModel.FKVendorID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKVendorID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public DataTable AutoDropDown()
        {
            DataTable record = new DataTable("record");
            var data = (from cou in __dbContext.TblVendorMas
                            // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                        orderby cou.PkVendorId
                        select (new
                        {
                            pk_Id = cou.PkVendorId,
                            Field1 = cou.Name,
                            Field2 = cou.Mobile,
                            //  Field3 = "",
                        }
                       )).ToList();

            record = ToDataTable(data);
            return record;
        }

        public List<ColumnStructure> ColumnList()
        {
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="DateCreated",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Code", Fields="Code",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Name", Fields="Name",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                //new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Father Name", Fields="FatherName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                //new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Mother Name", Fields="MotherName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Marital", Fields="Marital",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Gender", Fields="Gender",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
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



















