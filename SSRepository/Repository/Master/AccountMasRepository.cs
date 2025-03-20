using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Drawing;
using System;
using System.Diagnostics;
using static Handler;

namespace SSRepository.Repository.Master
{
    public class AccountMasRepository : Repository<TblAccountMas>, IAccountMasRepository
    {
        public AccountMasRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(AccountMasModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Account))
            {
                cnt = (from x in __dbContext.TblAccountMas
                       where x.Account == model.Account && x.PkAccountId != model.PkAccountId
                       select x).Count();
                if (cnt > 0)
                    error = "Section  Name Already Exits";
            }

            return error;
        }

        public List<AccountMasModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<AccountMasModel> data = (from cou in __dbContext.TblAccountMas
                                          join CatPGrp in __dbContext.TblAccountGroupMas on cou.FkAccountGroupId equals CatPGrp.PkAccountGroupId
                                                          into tempAccGrp
                                          from AccGrp in tempAccGrp.DefaultIfEmpty()
                                          join bankrp in __dbContext.TblBankMas on cou.FKBankID equals bankrp.PkBankId
                                           into tempBank
                                          from bank in tempBank.DefaultIfEmpty()
                                          where (EF.Functions.Like(cou.Account.Trim().ToLower(), search + "%"))
                                          orderby cou.PkAccountId
                                          select (new AccountMasModel
                                          {
                                              PkAccountId = cou.PkAccountId,
                                              Account = cou.Account,
                                              Alias = cou.Alias,
                                              FkAccountGroupId = cou.FkAccountGroupId,
                                              Address = cou.Address,
                                              FkStationId = cou.FkStationId,
                                              Station = cou.FKStation.StationName,
                                              FkLocalityId = cou.FkLocalityId,
                                              Locality = cou.FKLocality.LocalityName,
                                              Pincode = cou.Pincode,
                                              Phone1 = cou.Phone1,
                                              Phone2 = cou.Phone2,
                                              Fax = cou.Fax,
                                              Email = cou.Email,
                                              Website = cou.Website,
                                              Dob = cou.Dob,
                                              Dow = cou.Dow,
                                              ApplyCostCenter = cou.ApplyCostCenter,
                                              ApplyTCS = cou.ApplyTCS,
                                              ApplyTDS = cou.ApplyTDS,
                                              PrintBrDtl = cou.PrintBrDtl,
                                              FKBankID = cou.FKBankID,
                                              AccountNo = cou.AccountNo,
                                              BankName = bank.BankName,
                                              IFSCCode = bank.IFSCCode,
                                              Status = cou.Status,
                                              DiscDate = cou.DiscDate,
                                              AccountGroupName = AccGrp.AccountGroupName,
                                              FKUserID = cou.FKUserID,
                                              DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                              UserName = cou.FKUser.UserId,
                                          }
                                         )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public AccountMasModel GetSingleRecord(long PkAccountId)
        {

            AccountMasModel data = new AccountMasModel();
            data = (from cou in __dbContext.TblAccountMas
                    //join CatPGrp in __dbContext.TblAccountGroupMas on cou.FkAccountGroupId equals CatPGrp.PkAccountGroupId
                    // into tempAccGrp
                    //from AccGrp in tempAccGrp.DefaultIfEmpty()
                    //join bankrp in __dbContext.TblBankMas on cou.FKBankID equals bankrp.PkBankId
                    //  into tempBank
                    //from bank in tempBank.DefaultIfEmpty()
                    where cou.PkAccountId == PkAccountId
                    select (new AccountMasModel
                    {
                        PkAccountId = cou.PkAccountId,
                        Account = cou.Account,
                        Alias = cou.Alias,
                        FkAccountGroupId = cou.FkAccountGroupId,
                        Address = cou.Address,
                        FkStationId = cou.FkStationId,
                        Station = cou.FKStation.StationName,
                        FkLocalityId = cou.FkLocalityId,
                        Locality = cou.FKLocality.LocalityName,
                        Pincode = cou.Pincode,
                        Phone1 = cou.Phone1,
                        Phone2 = cou.Phone2,
                        Email = cou.Email,
                        UserName = cou.FKUser.UserId,
                        //ApplyCostCenter = cou.ApplyCostCenter == null ? false : Convert.ToBoolean(cou.ApplyCostCenter),
                        //ApplyTCS = cou.ApplyTCS == null ? false : Convert.ToBoolean(cou.ApplyTCS),
                        //ApplyTDS = cou.ApplyTDS == null ? false : Convert.ToBoolean(cou.ApplyTDS),
                        Status = cou.Status,
                        DiscDate = cou.DiscDate,
                        FKBankID = cou.FKBankID,
                        AccountNo = cou.AccountNo,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        AccountLocation_lst = (from ad in __dbContext.TblAccountLocLnk
                                               join loc in __dbContext.TblLocationMas on ad.FKLocationID equals loc.PkLocationID
                                               where (ad.FkAccountId == cou.PkAccountId)
                                               select (new AccountLocLnkModel
                                               {
                                                   PKAccountLocLnkId = ad.PKAccountLocLnkId,
                                                   FkAccountId = ad.FkAccountId,
                                                   FKLocationID = ad.FKLocationID,
                                                   ModeForm = 1,
                                                   Location = loc.Location
                                               })).ToList(),
                        AccountDtl_lst = (from ad in __dbContext.TblAccountDtl
                                          join loc in __dbContext.TblLocationMas on ad.FKLocationID equals loc.PkLocationID
                                          where (ad.FkAccountId == cou.PkAccountId)
                                          select (new AccountDtlModel
                                          {
                                              PKAccountDtlId = ad.PKAccountDtlId,
                                              FkAccountId = ad.FkAccountId,
                                              Location = loc.Location,
                                              FKLocationID = ad.FKLocationID,
                                              OpBal = ad.OpBal == null ? 0 : Math.Abs(Convert.ToDecimal(ad.OpBal)),
                                              type = Convert.ToDecimal(ad.OpBal) >= 0 ? "Cr" : "Dr",
                                              ModeForm = 1
                                          })).ToList(),
                        AccountLicDtl_lst = (from ad in __dbContext.TblAccountLicDtl 
                                             where (ad.FkAccountId == cou.PkAccountId)
                                             select (new AccountLicDtlModel
                                             {
                                                 PKAccountLicDtlId = ad.PKAccountLicDtlId,
                                                 FkAccountId = ad.FkAccountId,
                                                 Description = ad.Description,
                                                 No = ad.No,
                                                 IssueDate = ad.IssueDate,
                                                 ValidTill = ad.ValidTill,
                                             })).ToList(),

                        AccountGroupName = cou.FKAccountGroupMas.AccountGroupName,
                        BankName = cou.FKBank.BankName,
                        IFSCCode = cou.FKBank.IFSCCode,
                    })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageNo, int pageSize, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                var result = GetList(pageSize, pageNo, search);
                return (from r in result
                        select new
                        {
                            r.PkAccountId,
                            r.Account
                        }).ToList();
            }
            else
            {
                return null;
            }

        }

        public string DeleteRecord(long PkAccountId)
        {
            string Error = "";
            using (var trans = __dbContext.Database.BeginTransaction())
            {
                try
                {
                    AccountMasModel oldModel = GetSingleRecord(PkAccountId);
                   
                    var lst = (from x in __dbContext.TblAccountMas
                               where x.PkAccountId == PkAccountId
                               select x).ToList();
                    if (lst.Count > 0)
                        __dbContext.TblAccountMas.RemoveRange(lst);

                    var AccLocLnklst = (from x in __dbContext.TblAccountLocLnk
                                        where x.FkAccountId == PkAccountId
                                        select x).ToList();
                    if (AccLocLnklst.Count > 0)
                        __dbContext.RemoveRange(AccLocLnklst);

                    var AccLocDtllst = (from x in __dbContext.TblAccountDtl
                                        where x.FkAccountId == PkAccountId
                                        select x).ToList();
                    if (AccLocDtllst.Count > 0)
                        __dbContext.RemoveRange(AccLocDtllst);

                    var AccLocLicDtllst = (from x in __dbContext.TblAccountLicDtl
                                           where x.FkAccountId == PkAccountId
                                           select x).ToList();
                    if (AccLocLicDtllst.Count > 0)
                        __dbContext.RemoveRange(AccLocLicDtllst);

                    AddMasterLog((long)Handler.Form.AccountMas, PkAccountId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.Account, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));

                    __dbContext.SaveChanges();
                    trans.Commit();
                    return "";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            AccountMasModel model = (AccountMasModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            if (string.IsNullOrEmpty(error))
            {
                if (model.AccountLicDtl_lst != null)
                {
                    var _checkDates = model.AccountLicDtl_lst.Where(x => x.ValidTill < x.IssueDate).ToList();
                    if (_checkDates.Count > 0)
                        error = "please enter valid Issue Date & Valid Till Date";
                }
            }
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            AccountMasModel model = (AccountMasModel)objmodel;
            TblAccountMas Tbl = new TblAccountMas();
            if (model.PkAccountId > 0)
            {
                var _entity = __dbContext.TblAccountMas.Find(model.PkAccountId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }


            }

            Tbl.PkAccountId = model.PkAccountId;
            Tbl.Account = model.Account;
            Tbl.FkAccountGroupId = model.FkAccountGroupId;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FkStationId = model.FkStationId;
            Tbl.FkLocalityId = model.FkLocalityId;
            Tbl.Alias = model.Alias;
            Tbl.Address = model.Address;
            Tbl.Pincode = model.Pincode;
            Tbl.Phone1 = model.Phone1;
            Tbl.Phone2 = model.Phone2;
            Tbl.Email = model.Email;
            Tbl.ApplyCostCenter = model.ApplyCostCenter;
            Tbl.ApplyTCS = model.ApplyTCS;
            Tbl.ApplyTDS = model.ApplyTDS;
            Tbl.Status = model.Status;
            Tbl.DiscDate = model.DiscDate;
            Tbl.FKBankID = model.FKBankID;
            Tbl.AccountNo = model.AccountNo;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                ID = Tbl.PkAccountId = getIdOfSeriesByEntity("PkcountryId", null, Tbl, "TblAccountMas");
                AddData(Tbl, false);
            }
            else
            {

                AccountMasModel oldModel = GetSingleRecord(Tbl.PkAccountId);
                ID = Tbl.PkAccountId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.AccountMas, Tbl.PkAccountId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Account, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FkAccountId, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());

            var ul = (from x in __dbContext.TblAccountLocLnk
                      where x.FkAccountId == Tbl.PkAccountId
                      select x).ToList();

            DeleteData(ul, true);

            //List<TblAccountLocLnk> lstDelLocation = new List<TblAccountLocLnk>();
            List<TblAccountLocLnk> lstAddLocation = new List<TblAccountLocLnk>();
            //  List<TblAccountLocLnk> lstEditLocation = new List<TblAccountLocLnk>();

            if (model.AccountLocation_lst != null)
            {

                foreach (var lc in model.AccountLocation_lst)
                {
                    TblAccountLocLnk objLoc = new TblAccountLocLnk();
                    //   objLoc.PKAccountLocLnkId = getIdOfSeriesByEntity("PKAccountLocLnkId", null, Tbl, "TblAccountLocLnk");
                    //objLoc.PKAccountLocLnkId = lc.PKAccountLocLnkId;
                    objLoc.FkAccountId = Tbl.PkAccountId;
                    objLoc.FKLocationID = lc.FKLocationID;
                    //if (lc.ModeForm == 1)
                    //{
                    //    //locObj.ModifiedDate = DateTime.Now;
                    //    //lstEdit.Add(locObj);
                    //}
                    //else
                    if (lc.ModeForm != 2)
                    {
                        //  locObj.PKAccountDtlId = getIdOfSeriesByEntity("PKAccountDtlId", null, Tbl, "TblAccountDtl");
                        objLoc.ModifiedDate = DateTime.Now;
                        objLoc.FKUserID = GetUserID();
                        objLoc.FKCreatedByID = Tbl.FKUserID;
                        objLoc.CreationDate = Tbl.ModifiedDate;
                        lstAddLocation.Add(objLoc);
                    }
                    //else
                    //{
                    //    var res1 = (from x in __dbContext.TblAccountLocLnk
                    //                where x.PKAccountLocLnkId == objLoc.PKAccountLocLnkId
                    //                && x.FkAccountId == objLoc.FkAccountId
                    //                && x.FKLocationID == objLoc.FKLocationID
                    //                select x).Count();
                    //    if (res1 > 0)
                    //    {
                    //        lstDelLocation.Add(objLoc);
                    //    }
                    //}
                }
            }
            //if (lstDelLocation.Count() > 0)
            //    DeleteData(lstDelLocation, true);
            //if (lstEditLocation.Count() > 0)
            //    UpdateData(lstEditLocation, true);
            if (lstAddLocation.Count() > 0)
                AddData(lstAddLocation, true);


            var ul_dtl = (from x in __dbContext.TblAccountDtl
                          where x.FkAccountId == Tbl.PkAccountId
                          select x).ToList();

            DeleteData(ul_dtl, true);

            if (model.AccountDtl_lst != null)
            {
                List<TblAccountDtl> lstAddAccDtl = new List<TblAccountDtl>();
                //List<TblAccountDtl> lstEdit = new List<TblAccountDtl>();
                // List<TblAccountDtl> lstDel = new List<TblAccountDtl>();
                foreach (var item in model.AccountDtl_lst)
                {
                    TblAccountDtl locObj = new TblAccountDtl();
                    if (item.type == "Dr")
                        locObj.CurrBal = -item.OpBal;
                    else
                        locObj.CurrBal = item.OpBal;

                    locObj.PKAccountDtlId = item.PKAccountDtlId;
                    locObj.OpBal = locObj.CurrBal;
                    locObj.CurrBalDate = DateTime.Now;
                    locObj.FKLocationID = item.FKLocationID;
                    locObj.FkAccountId = Tbl.PkAccountId;


                    //   lstAdd.Add(locObj);
                    //if (item.Mode == 1)
                    //{
                    //    locObj.ModifiedDate = DateTime.Now;
                    //    locObj.CreationDate = Tbl.CreationDate;
                    //    locObj.FKUserID = locObj.FKCreatedByID = Tbl.FKUserID;
                    //    lstEdit.Add(locObj);
                    //}
                    //else 
                    if (item.ModeForm != 2)
                    {
                        //  locObj.PKAccountDtlId = getIdOfSeriesByEntity("PKAccountDtlId", null, Tbl, "TblAccountDtl");
                        locObj.ModifiedDate = DateTime.Now;
                        locObj.FKUserID = GetUserID();
                        locObj.FKCreatedByID = Tbl.FKUserID;
                        locObj.CreationDate = Tbl.ModifiedDate;
                        lstAddAccDtl.Add(locObj);
                    }

                    //else
                    //{
                    //    var res1 = (from x in __dbContext.TblAccountDtl
                    //                where x.FKLocationID == locObj.FKLocationID && x.FkAccountId == locObj.FkAccountId
                    //                select x).Count();
                    //    if (res1 > 0)
                    //    {
                    //        lstDel.Add(locObj);
                    //    }
                    //}

                }

                //if (lstDel.Count() > 0)
                //    DeleteData(lstDel, true);
                //if (lstEdit.Count() > 0)
                //    UpdateData(lstEdit, true);
                if (lstAddAccDtl.Count() > 0)
                    AddData(lstAddAccDtl, true);
            }

            if (model.AccountLicDtl_lst != null)
            {
                List<TblAccountLicDtl> lstAdd = new List<TblAccountLicDtl>();
                List<TblAccountLicDtl> lstEdit = new List<TblAccountLicDtl>();
                List<TblAccountLicDtl> lstDel = new List<TblAccountLicDtl>();

                //lstDel = (from x in __dbContext.TblAccountLicDtl
                //          where x.FkAccountId == Tbl.PkAccountId
                //          select x).ToList();



                foreach (var item in model.AccountLicDtl_lst)
                {
                    TblAccountLicDtl locObj = new TblAccountLicDtl();
                    locObj.PKAccountLicDtlId = item.PKAccountLicDtlId;
                    locObj.Description = item.Description;
                    locObj.No = item.No;
                    locObj.IssueDate = item.IssueDate;
                    locObj.ValidTill = item.ValidTill;
                    locObj.FkAccountId = Tbl.PkAccountId;


                    //   lstAdd.Add(locObj);
                    if (!string.IsNullOrEmpty(item.Description) && locObj.PKAccountLicDtlId == 0)
                    {
                        //  locObj.PKAccountLicDtlId = getIdOfSeriesByEntity("PKAccountLicDtlId", null, Tbl, "TblAccountLicDtl");
                        locObj.ModifiedDate = DateTime.Now;
                        locObj.FKUserID = GetUserID();
                        locObj.FKCreatedByID = Tbl.FKUserID;
                        locObj.CreationDate = Tbl.ModifiedDate;
                        lstAdd.Add(locObj);
                    }
                    else if (!string.IsNullOrEmpty(item.Description) && locObj.PKAccountLicDtlId > 0)
                    {
                        //  locObj.PKAccountLicDtlId = getIdOfSeriesByEntity("PKAccountLicDtlId", null, Tbl, "TblAccountLicDtl");
                        locObj.ModifiedDate = DateTime.Now;
                        locObj.FKUserID = Tbl.FKUserID;
                        locObj.FKCreatedByID = Tbl.FKUserID;
                        locObj.CreationDate = Tbl.ModifiedDate;
                        lstEdit.Add(locObj);
                    }
                    else
                    {
                        if (locObj.PKAccountLicDtlId > 0)
                            lstDel.Add(locObj);
                    }
                }

                if (lstDel.Count() > 0)
                    DeleteData(lstDel, true);
                if (lstEdit.Count() > 0)
                    UpdateData(lstEdit, true);
                if (lstAdd.Count() > 0)
                    AddData(lstAdd, true);
            }
        }
        public string AutoGenerateAlias()
        {
            var str = "";
            try
            {
                var _max = __dbContext.TblAccountMas.Where(x => !string.IsNullOrEmpty(x.Alias)).Max(x => Convert.ToInt64(x.Alias));
                str = _max != null ? (_max + 1).ToString() : "1000";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Error converting data type nvarchar to bigint"))
                    throw new Exception("auto generate failed. because existing records contains character in alias.");
                else
                    str = "1000";
            }
            return str;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Account", Fields="Account",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Account Group",    Fields="AccountGroupName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Alias",    Fields="Alias",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Email",    Fields="Email",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Mobile",  Fields="Phone2",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Address",  Fields="Address",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Pincode",  Fields="Pincode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Station", Fields="Station",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Locality",   Fields="Locality",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Phone", Fields="Phone1",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },

                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Bank", Fields="BankName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Acc. No", Fields="AccountNo",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="IFSC", Fields="IFSCCode",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },

                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },

                        };
            return list;
        }

        public object GetDrpAccountMas(int pageSize, int pageNo = 1, string search = "")
        {
            throw new NotImplementedException();
        }
    }
}



















