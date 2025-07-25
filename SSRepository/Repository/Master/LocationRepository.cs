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
                       where x.Location == model.Location && x.PkLocationID != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Location Name Already Exits";
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
                    orderby Loc.Location
                    select (new LocationModel
                    {
                        PKID = Loc.PkLocationID,
                        Location = Loc.Location,
                        Alias = Loc.Alias,
                        Address = Loc.Address,
                        Station = Loc.stationMas.StationName,
                        Locality = Loc.localityMas.LocalityName,
                        Branch = Loc.branchMas.BranchName,
                        Account = Loc.accountMas.Account,
                        IsAllAccount = Loc.IsAllAccount,
                        IsAllCostCenter = Loc.IsAllCostCenter,
                        IsAllCustomer = Loc.IsAllCustomer,
                        IsAllProduct = Loc.IsAllProduct,
                        IsAllVendor = Loc.IsAllVendor,
                        IsBillingLocation = Loc.IsBillingLocation,
                        IsDifferentTax = Loc.IsAllVendor,
                        Pincode = Loc.Pincode,
                        Phone1 = Loc.Phone1,
                        Phone2 = Loc.Phone2,
                        Fax = Loc.Fax,
                        Email = Loc.Email,
                        Website = Loc.Website,
                        UserName = Loc.FKUser.UserId,
                        DATE_MODIFIED = Loc.ModifiedDate.ToString("dd-MMM-yyyy")
                    }
                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblLocationMas
                         where (EF.Functions.Like(cou.Location.Trim().ToLower(), Convert.ToString(search) + "%"))
                          //&& BillingLocation.Contains(cou.PkLocationID.ToString())
                         orderby cou.Location
                         select (new
                         {
                             cou.PkLocationID,
                             cou.Location,
                             cou.Alias,
                             cou.Address,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else if (EnCustomFlag == (int)Handler.en_CustomFlag.Filter)
            {
                var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblLocationMas
                         where (EF.Functions.Like(cou.Location.Trim().ToLower(), Convert.ToString(search) + "%"))
                         && BillingLocation.Contains(cou.PkLocationID.ToString())
                         orderby cou.Location
                         select (new
                         {
                             cou.PkLocationID,
                             cou.Location,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public object GetDrpLocation(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;


            return (from Loc in __dbContext.TblLocationMas
                    where (EF.Functions.Like(Loc.Location.Trim().ToLower(), Convert.ToString(search) + "%"))
                    orderby Loc.Location
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
                        PKID = cou.PkLocationID,
                        Location = cou.Location,
                        Address = cou.Address,
                        Alias = cou.Alias,
                        IsBillingLocation = cou.IsBillingLocation,
                        IsAllProduct = cou.IsAllProduct,
                        IsAllCustomer = cou.IsAllCustomer,
                        IsAllVendor = cou.IsAllVendor,
                        FKStationID = cou.FkStationID,
                        Station = cou.stationMas.StationName,
                        FKLocalityID = cou.FkLocalityID,
                        Locality = cou.localityMas.LocalityName,
                        Pincode = cou.Pincode,
                        Phone1 = cou.Phone1,
                        Phone2 = cou.Phone2,
                        IsAllCostCenter = cou.IsAllCostCenter,
                        Fax = cou.Fax,
                        Email = cou.Email,
                        Website = cou.Website,
                        IsDifferentTax = cou.IsDifferentTax,
                        FKAccountID = cou.FkAccountID,
                        Account = cou.accountMas.Account,
                        FKBranchID = cou.FkBranchID,
                        Branch = cou.branchMas.BranchName,
                        IsAllAccount = cou.IsAllAccount,
                        UserName = cou.FKUser.UserId,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),

                    })).FirstOrDefault();
            if (data != null)
            {
                data.UserLoclnk = (from ad in __dbContext.TblUserLocLnk
                                   join user in __dbContext.TblUserMas on ad.FKUserID equals user.PkUserId
                                   where (ad.FKLocationID == data.PKID)
                                   select (new UserLocLnkModel
                                   {
                                       FkLocationID = ad.FKLocationID,
                                       FkUserID = ad.FKUserID,
                                       UserName = user.UserId,
                                   })).ToList();
            }
            return data;
        }

        public string DeleteRecord(long PkLocationId)
        {
            string Error = "";
            LocationModel oldModel = GetSingleRecord(PkLocationId);


            //
            //[]
            var prdqtyBarcode = (from cou in __dbContext.TblProductQTYBarcode
                                 where cou.FKLocationId == PkLocationId
                                 select cou).Count();
            if (prdqtyBarcode > 0)
                Error = "use in other transaction";
            if (Error == "")
            {
                //var prdStockdtl = (from cou in __dbContext.tblProdStock_Dtl
                //                   where cou.FKLocationId == PkLocationId
                //                   select cou).Count();
                //if (prdStockdtl > 0)
                //    Error = "use in other transaction";
                //if (Error == "")
                //{
                var lstDeluserlink = (from x in __dbContext.TblUserLocLnk
                                      where x.FKLocationID == PkLocationId
                                      select x).ToList();
                if (lstDeluserlink.Count > 0)
                    __dbContext.TblUserLocLnk.RemoveRange(lstDeluserlink);

                var lst = (from x in __dbContext.TblLocationMas
                           where x.PkLocationID == PkLocationId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblLocationMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Location, PkLocationId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.Location, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
                // }
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
            if (model.PKID > 0)
            {
                var _entity = GetSingleRecord(model.PKID);

                IList<TblUserLocLnk> userlnk = (from x in __dbContext.TblUserLocLnk
                                                where x.FKLocationID == model.PKID
                                                select x).ToList();

                if (userlnk.Any())
                {
                    DeleteData(userlnk, true);
                }
            }
            Tbl.PkLocationID = model.PKID;
            Tbl.Location = model.Location;
            Tbl.Address = model.Address;
            Tbl.Alias = model.Alias;
            Tbl.CreationDate = DateTime.Now;
            Tbl.IsBillingLocation = model.IsBillingLocation;
            Tbl.IsAllProduct = model.IsAllProduct;
            Tbl.IsAllCustomer = false;
            Tbl.IsAllVendor = false;
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
            Tbl.IsAllAccount = false;
            Tbl.IsAllCostCenter = false;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                Tbl.PkLocationID = getIdOfSeriesByEntity("PkLocationID", null, Tbl, "TblLocationMas");
                AddData(Tbl, false);
            }
            else
            {
                LocationModel oldModel = GetSingleRecord(Tbl.PkLocationID);
                ID = Tbl.PkLocationID;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Location, Tbl.PkLocationID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Location, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            if (model.UserLoclnk != null)
            {
                List<TblUserLocLnk> lstAdd = new List<TblUserLocLnk>();
                foreach (var item in model.UserLoclnk)
                {
                    TblUserLocLnk locObj = new TblUserLocLnk();
                    locObj.FKLocationID = Tbl.PkLocationID;
                    locObj.FKUserID = item.FkUserID;
                    if (item.ModeForm == 0)
                    {
                        lstAdd.Add(locObj);
                    }
                }
                if (lstAdd.Count > 0)
                    AddData(lstAdd, true);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKLocationID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Location", Fields="Location",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Alias",    Fields="Alias",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Branch",    Fields="Branch",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Account",    Fields="Account",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="IsBillingLocation", Fields="IsBillingLocation",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Address",  Fields="Address",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Pincode",  Fields="Pincode",Width=8,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Station", Fields="Station",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Locality",   Fields="Locality",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Phone2", Fields="Phone2",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Email", Fields="Email",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Fax", Fields="Fax",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Website", Fields="Website",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },

                        };
            return list;
        }


    }
}