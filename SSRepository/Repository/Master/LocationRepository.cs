using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using System.Net;

namespace SSRepository.Repository.Master
{
    public class LocationRepository : Repository<TblLocationMas>, ILocationRepository
    {
        public LocationRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(LocationModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Location))
            {
                cnt = (from x in __dbContext.TblLocationMas
                       where x.Location == model.Location && x.PkLocationID != model.PKLocationID
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<LocationModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<LocationModel> data = new List<LocationModel>();


            data = (from Loc in __dbContext.TblLocationMas
                    where (EF.Functions.Like(Loc.Location.Trim().ToLower(), Convert.ToString(search) + "%"))
                    orderby Loc.PkLocationID
                    select (new LocationModel
                    {
                        PKLocationID = Loc.PkLocationID,
                        Location = Loc.Location,
                        Alias = Loc.Alias,
                        Address = Loc.Address,
                        FKStationID = Loc.FkStationID,
                        Pincode = Loc.Pincode,
                        Phone1 = Loc.Phone1,
                        Phone2 = Loc.Phone2,
                        Fax = Loc.Fax,
                        Email = Loc.Email,
                        Website = Loc.Website,
                        FKUserID = Loc.FKUserID,
                        DATE_MODIFIED = Loc.ModifiedDate.ToString("dd-MMM-yyyy")
                    }
                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return data;
        }
        public object GetDrpLocation(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;


            return (from Loc in __dbContext.TblLocationMas
                    where (EF.Functions.Like(Loc.Location.Trim().ToLower(), Convert.ToString(search) + "%"))
                    orderby Loc.PkLocationID
                    select (new
                    {
                        Loc.PkLocationID,
                        Loc.Location,
                        //Loc.Alias,
                        //Loc.Address,
                        //Loc.FkStationID,
                        //Loc.Pincode,
                        //Loc.Phone1,
                        //Loc.Phone2,
                        //Loc.Fax,
                        //Loc.Email,
                        //Loc.Website,
                    }
                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();


        }

        public LocationModel GetSingleRecord(long PkLocationId)
        {

            LocationModel? data = new LocationModel();
            data = (from cou in __dbContext.TblLocationMas
                    where cou.PkLocationID == PkLocationId
                    select (new LocationModel
                    {
                        PKLocationID = cou.PkLocationID,
                        Location = cou.Location,
                       // PkLocationID = cou.PkLocationID,
                        Address = cou.Address,
                        Alias = cou.Alias,
                        IsBillingLocation = cou.IsBillingLocation,
                        IsAllProduct = cou.IsAllProduct,
                        IsAllCustomer = cou.IsAllCustomer,
                        IsAllVendor = cou.IsAllVendor,
                        FKStationID = cou.FkStationID,
                        FKLocalityID = cou.FkLocalityID,
                        Pincode = cou.Pincode,
                        Phone1 = cou.Phone1,
                        Phone2 = cou.Phone2,
                        IsAllCostCenter = cou.IsAllCostCenter,
                        Fax = cou.Fax,
                        Email = cou.Email,
                        Website = cou.Website,
                        IsDifferentTax = cou.IsDifferentTax,
                        FKAccountID = cou.FkAccountID,
                        FKBranchID = cou.FkBranchID,
                        IsAllAccount = cou.IsAllAccount,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")

                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkLocationId)
        {
            string Error = "";
            LocationModel obj = GetSingleRecord(PkLocationId);



            if (Error == "")
            {
                var lst = (from x in __dbContext.TblLocationMas
                           where x.PkLocationID == PkLocationId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblLocationMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkLocationId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkLocationId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetLocationID(), PkLocationId, obj.FKLocationID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            LocationModel model = (LocationModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            LocationModel model = (LocationModel)objmodel;
            TblLocationMas Tbl = new TblLocationMas();
            if (model.PKLocationID > 0)
            {
                var _entity = __dbContext.TblLocationMas.Find(model.PKLocationID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkLocationID = model.PKLocationID;
            Tbl.Location = model.Location;
            Tbl.Address = model.Address;
            Tbl.Alias = model.Alias;
            Tbl.CreationDate = DateTime.Now;
            Tbl.IsBillingLocation = model.IsBillingLocation;
            Tbl.IsAllProduct = model.IsAllProduct;
            Tbl.IsAllCustomer = model.IsAllCustomer;
            Tbl.IsAllVendor = model.IsAllVendor;
            Tbl.FkStationID = model.FKStationID;
            Tbl.FkLocalityID = model.FKLocalityID;
            Tbl.Pincode = model.Pincode;
            Tbl.Phone1 = model.Phone1;
            Tbl.Phone2 = model.Phone2;
            Tbl.Fax = model.Fax;
            Tbl.Email = model.Email;
            Tbl.Website = model.Website;
            Tbl.IsDifferentTax = model.IsDifferentTax;
            Tbl.FkAccountID = model.FKAccountID;
            Tbl.FkBranchID = model.FKBranchID;
            Tbl.IsAllAccount = model.IsAllAccount;
            Tbl.IsAllCostCenter = model.IsAllCostCenter;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                AddData(Tbl, false);
            }
            else
            {
                LocationModel oldModel = GetSingleRecord(Tbl.PkLocationID);
                ID = Tbl.PkLocationID;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Location, Tbl.PkLocationID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Location, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKLocationID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Location", Fields="Location",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Alias", Fields="Alias",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Address", Fields="Address",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Pincode", Fields="Pincode",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Phone1", Fields="Phone1",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Phone2", Fields="Phone2",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Phone2", Fields="Phone2",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Fax", Fields="Fax",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Email", Fields="Email",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Website", Fields="Website",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

                        };
            return list;
        }


    }
}