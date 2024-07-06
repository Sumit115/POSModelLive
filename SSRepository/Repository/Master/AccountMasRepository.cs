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

namespace SSRepository.Repository.Master
{
    public class AccountMasRepository : Repository<TblAccountMas>, IAccountMasRepository
    {
        public AccountMasRepository(AppDbContext dbContext) : base(dbContext)
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
                                          where (EF.Functions.Like(cou.Account.Trim().ToLower(), search + "%"))
                                          orderby cou.PkAccountId
                                          select (new AccountMasModel
                                          {
                                              PkAccountId = cou.PkAccountId,
                                              FKUserId = cou.FKUserID,
                                              FKCreatedByID = cou.FKCreatedByID,
                                              Account = cou.Account,
                                              Alias = cou.Alias,
                                              FkAccountGroupId = cou.FkAccountGroupId,
                                              Address = cou.Address,
                                              Station = cou.Station,
                                              Locality = cou.Locality,
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
                                              Status = cou.Status,
                                              DiscDate = cou.DiscDate,

                                              AccountGroupName = AccGrp.AccountGroupName,
                                          }
                                         )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public AccountMasModel GetSingleRecord(long PkAccountId)
        {

            AccountMasModel data = new AccountMasModel();
            data = (from cou in __dbContext.TblAccountMas
                    join CatPGrp in __dbContext.TblAccountGroupMas on cou.FkAccountGroupId equals CatPGrp.PkAccountGroupId
                     into tempAccGrp
                    from AccGrp in tempAccGrp.DefaultIfEmpty()
                    where cou.PkAccountId == PkAccountId
                    select (new AccountMasModel
                    {
                        PkAccountId = cou.PkAccountId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        Account = cou.Account,
                        Alias = cou.Alias,
                        FkAccountGroupId = cou.FkAccountGroupId,
                        Address = cou.Address,
                        Station = cou.Station,
                        Locality = cou.Locality,
                        Pincode = cou.Pincode,
                        Phone1 = cou.Phone1,
                        Phone2 = cou.Phone2,
                        Email = cou.Email,
                        //ApplyCostCenter = cou.ApplyCostCenter == null ? false : Convert.ToBoolean(cou.ApplyCostCenter),
                        //ApplyTCS = cou.ApplyTCS == null ? false : Convert.ToBoolean(cou.ApplyTCS),
                        //ApplyTDS = cou.ApplyTDS == null ? false : Convert.ToBoolean(cou.ApplyTDS),
                        Status = cou.Status,
                        DiscDate = cou.DiscDate,
                        FKBankID = cou.FKBankID,
                        AccountNo = cou.AccountNo,
                        AccountLocation_lst = (from ad in __dbContext.TblAccountLocLnk
                                                   //  join loc in __dbContext.TblBranchMas on ad.FKLocationID equals loc.PkBranchId
                                               where (ad.FkAccountId == cou.PkAccountId)
                                               select (new AccountLocLnkModel
                                               {
                                                   PKAccountLocLnkId = ad.PKAccountLocLnkId,
                                                   FkAccountId = ad.FkAccountId,
                                                   FKLocationID = ad.FKLocationID,
                                                   Selected = true
                                               })).ToList(),
                        AccountDtl_lst = (from ad in __dbContext.TblAccountDtl
                                          join loc in __dbContext.TblBranchMas on ad.FKLocationID equals loc.PkBranchId
                                          where (ad.FkAccountId == cou.PkAccountId)
                                          select (new AccountDtlModel
                                          {
                                              PKAccountDtlId = ad.PKAccountDtlId,
                                              FkAccountId = ad.FkAccountId,
                                              Location = loc.BranchName,
                                              FKLocationID = ad.FKLocationID,
                                              OpBal = ad.OpBal == null ? 0 : Math.Abs(Convert.ToDecimal(ad.OpBal)),
                                              type = Convert.ToDecimal(ad.OpBal) >= 0 ? "Cr" : "Dr",
                                              Mode = 1
                                          })).ToList(),
                        AccountLicDtl_lst = (from ad in __dbContext.TblAccountLicDtl
                                                 // join loc in __dbContext.TblBranchMas on ad.FKLocationID equals loc.PkBranchId
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

                        AccountGroupName = AccGrp.AccountGroupName,

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpAccount(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);

            result.Insert(0, new AccountMasModel { PkAccountId = 0, Account = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkAccountId,
                        r.Account
                    }).ToList();
        }

        public string DeleteRecord(long PkAccountId)
        {
            string Error = "";
            using (var trans = __dbContext.Database.BeginTransaction())
            {
                try
                {

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

                    __dbContext.SaveChanges();
                    trans.Commit();
                    return "";
                }
                catch (Exception ex)
                {
                    //WriteLog(ex, "SaveData", "TranRepository", _contextAccessor.HttpContext.Session.Get<Int64>("UserID"));
                    trans.Rollback();
                    ResponseModel response = new ResponseModel();
                    response.ID = 0;
                    response.Response = "Error: " + ex.Message;
                    return JsonConvert.SerializeObject(response);
                }
            }
            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            AccountMasModel model = (AccountMasModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
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
            Tbl.Station = model.Station;
            Tbl.Locality = model.Locality;
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
            Tbl.FKUserID = model.FKUserId;
            if (Mode == "Create")
            {
                ID = Tbl.PkAccountId = getIdOfSeriesByEntity("PkcountryId", null, Tbl, "TblAccountMas");
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.CreationDate = DateTime.Now;

                AddData(Tbl, false);
            }
            else
            {

                AccountMasModel oldModel = GetSingleRecord(Tbl.PkAccountId);
                ID = Tbl.PkAccountId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FkAccountId, oldModel.PkAccountId, oldModel.FkAccountId, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FkAccountId, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());

            var ul = (from x in __dbContext.TblAccountLocLnk
                      where x.FkAccountId == Tbl.PkAccountId
                      select x).ToList();

            DeleteData(ul, true);

            //  List<TblAccountLocLnk> lstDelLocation = new List<TblAccountLocLnk>();
            List<TblAccountLocLnk> lstAddLocation = new List<TblAccountLocLnk>();
            List<TblAccountLocLnk> lstEditLocation = new List<TblAccountLocLnk>();

            if (model.AccountLocation_lst != null)
            {
                //lstDelLocation = (from x in __dbContext.TblAccountLocLnk
                //                  where x.FkAccountId == Tbl.PkAccountId && !model.AccountLocation_lst.ToList().Any(y => y.PKAccountLocLnkId == x.PKAccountLocLnkId)
                //                  select x).ToList();

                foreach (var lc in model.AccountLocation_lst)
                {
                    if (lc.Selected)
                    {
                        //TblUserLocLnk objLoc = new TblUserLocLnk();
                        //objLoc.FKUserID = GetUserID();
                        //objLoc.FKLocationID = lc.PkLocid;
                        //AddData(objLoc, false);

                        TblAccountLocLnk objLoc = new TblAccountLocLnk();
                        //   objLoc.PKAccountLocLnkId = getIdOfSeriesByEntity("PKAccountLocLnkId", null, Tbl, "TblAccountLocLnk");
                        objLoc.PKAccountLocLnkId = lc.PKAccountLocLnkId;
                        objLoc.FkAccountId = Tbl.PkAccountId;
                        objLoc.FKLocationID = lc.FKLocationID;
                        objLoc.FKCreatedByID = model.FKCreatedByID;
                        objLoc.FKUserID = model.FKUserId;
                        objLoc.CreationDate = DateTime.Now;
                        objLoc.ModifiedDate = DateTime.Now;
                        //if (objLoc.PKAccountLocLnkId > 0)
                        //    lstEditLocation.Add(objLoc);
                        //else
                        lstAddLocation.Add(objLoc);
                    }
                }
            }
            else
            {
                //lstDelLocation = (from x in __dbContext.TblAccountLocLnk
                //                  where x.FkAccountId == Tbl.PkAccountId 
                //                  select x).ToList();
            }
            //if (lstDelLocation.Count() > 0)
            //    DeleteData(lstDelLocation, true);
            //if (lstEditLocation.Count() > 0)
            //    UpdateData(lstEditLocation, true);
            if (lstAddLocation.Count() > 0)
                AddData(lstAddLocation, true);


            if (model.AccountDtl_lst != null)
            {
                List<TblAccountDtl> lstAdd = new List<TblAccountDtl>();
                List<TblAccountDtl> lstEdit = new List<TblAccountDtl>();
                List<TblAccountDtl> lstDel = new List<TblAccountDtl>();
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
                    if (item.Mode == 1)
                    {
                        locObj.ModifiedDate = DateTime.Now;
                        lstEdit.Add(locObj);
                    }
                    else if (item.Mode == 0)
                    {
                        //  locObj.PKAccountDtlId = getIdOfSeriesByEntity("PKAccountDtlId", null, Tbl, "TblAccountDtl");
                        locObj.FKCreatedByID = model.FKCreatedByID;
                        locObj.FKUserID = model.FKUserId;
                        locObj.CreationDate = DateTime.Now;
                        locObj.ModifiedDate = DateTime.Now;
                        lstAdd.Add(locObj);
                    }

                    else
                    {
                        var res1 = (from x in __dbContext.TblAccountDtl
                                    where x.FKLocationID == locObj.FKLocationID && x.FkAccountId == locObj.FkAccountId
                                    select x).Count();
                        if (res1 > 0)
                        {
                            lstDel.Add(locObj);
                        }
                    }

                }

                if (lstDel.Count() > 0)
                    DeleteData(lstDel, true);
                if (lstEdit.Count() > 0)
                    UpdateData(lstEdit, true);
                if (lstAdd.Count() > 0)
                    AddData(lstAdd, true);
            }

            if (model.AccountLicDtl_lst != null)
            {
                List<TblAccountLicDtl> lstAdd = new List<TblAccountLicDtl>();
                List<TblAccountLicDtl> lstEdit = new List<TblAccountLicDtl>();
                List<TblAccountLicDtl> lstDel = new List<TblAccountLicDtl>();

                lstDel = (from x in __dbContext.TblAccountLicDtl
                          where x.FkAccountId == Tbl.PkAccountId
                          select x).ToList();



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
                        locObj.FKCreatedByID = model.FKCreatedByID;
                        locObj.FKUserID = model.FKUserId;
                        locObj.CreationDate = DateTime.Now;
                        locObj.ModifiedDate = DateTime.Now;
                        lstAdd.Add(locObj);
                    }
                    else if (!string.IsNullOrEmpty(item.Description) && locObj.PKAccountLicDtlId > 0)
                    {
                        //  locObj.PKAccountLicDtlId = getIdOfSeriesByEntity("PKAccountLicDtlId", null, Tbl, "TblAccountLicDtl");
                        locObj.ModifiedDate = DateTime.Now;
                        lstAdd.Add(locObj);
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
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Account ", Fields="Account",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Account Group", Fields="AccountGroupName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Alias", Fields="Alias",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Address", Fields="Address",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Station", Fields="Station",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Locality", Fields="Locality",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Pincode", Fields="Pincode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Phone", Fields="Phone1",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Mobile", Fields="Phone2",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=10, Orderby =11, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }

        public object GetDrpAccountMas(int pageSize, int pageNo = 1, string search = "")
        {
            throw new NotImplementedException();
        }
    }
}



















