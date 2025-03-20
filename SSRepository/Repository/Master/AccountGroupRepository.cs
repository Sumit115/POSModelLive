using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Drawing;

namespace SSRepository.Repository.Master
{
    public class AccountGroupRepository : Repository<TblAccountGroupMas>, IAccountGroupRepository
    {
        public AccountGroupRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(AccountGroupModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.AccountGroupName))
            {
                cnt = (from x in __dbContext.TblAccountGroupMas
                       where x.AccountGroupName == model.AccountGroupName && x.PkAccountGroupId != model.PkAccountGroupId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Group Name Already Exits";
            }

            return error;
        }

        public List<AccountGroupModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<AccountGroupModel> data = (from cou in __dbContext.TblAccountGroupMas
                                            //join CatPGrp in __dbContext.TblAccountGroupMas on cou.FkAccountGroupId equals CatPGrp.PkAccountGroupId
                                            //                into tempAccGrp
                                            //from AccGrp in tempAccGrp.DefaultIfEmpty()
                                            where (EF.Functions.Like(cou.AccountGroupName.Trim().ToLower(), search + "%"))
                                            orderby cou.PkAccountGroupId
                                            select (new AccountGroupModel
                                            {
                                                PkAccountGroupId = cou.PkAccountGroupId,
                                                AccountGroupName = cou.AccountGroupName,
                                                FkAccountGroupId = cou.FkAccountGroupId,
                                                PAccountGroupName = cou.FKAccountGroupMas.AccountGroupName,
                                                GroupType = cou.GroupType,
                                                GroupAlias = cou.GroupAlias,
                                                NatureOfGroup = cou.NatureOfGroup,
                                                PrintDtl = cou.PrintDtl,
                                                NetCrDrBalanceForRpt = cou.NetCrDrBalanceForRpt,
                                                FKUserID = cou.FKUserID,
                                                DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                                UserName = cou.FKUser.UserId,
                                            }
                                           )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
              
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblAccountGroupMas 
                         where (EF.Functions.Like(cou.AccountGroupName.Trim().ToLower(), search + "%"))
                         orderby cou.PkAccountGroupId
                         select (new
                         {
                             cou.PkAccountGroupId,
                             cou.AccountGroupName, 
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            } 
            else
            {
                return null;
            }
        }

        public AccountGroupModel GetSingleRecord(long PkAccountGroupId)
        {

            AccountGroupModel data = new AccountGroupModel();
            data = (from cou in __dbContext.TblAccountGroupMas
                    where cou.PkAccountGroupId == PkAccountGroupId
                    select (new AccountGroupModel
                    {
                        PkAccountGroupId = cou.PkAccountGroupId,
                        AccountGroupName = cou.AccountGroupName,
                        FkAccountGroupId = cou.FkAccountGroupId,
                        GroupType = cou.GroupType,
                        GroupAlias = cou.GroupAlias,
                        NatureOfGroup = cou.NatureOfGroup,
                        PrintDtl = cou.PrintDtl,
                        NetCrDrBalanceForRpt = cou.NetCrDrBalanceForRpt,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpAccountGroup(int pageSize, int pageNo, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);

            result.Insert(0, new AccountGroupModel { PkAccountGroupId = 0, AccountGroupName = "Select" });
            return (from r in result
                    select new
                    {
                        r.PkAccountGroupId,
                        r.AccountGroupName
                    }).ToList();
        }

        public string DeleteRecord(long PkAccountGroupId)
        {
            string Error = "";
            AccountGroupModel oldModel = GetSingleRecord(PkAccountGroupId);
             
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblAccountGroupMas
                           where x.PkAccountGroupId == PkAccountGroupId
                           select x).ToList();
                if (lst.Count > 0)
                {
                    __dbContext.TblAccountGroupMas.RemoveRange(lst);
                    AddMasterLog((long)Handler.Form.AccountGroup, oldModel.PkAccountGroupId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.AccountGroupName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                }
                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkAccountGroupId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkAccountGroupId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetAccountGroupID(), PkAccountGroupId, obj.FKAccountGroupID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            AccountGroupModel model = (AccountGroupModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            AccountGroupModel model = (AccountGroupModel)objmodel;
            TblAccountGroupMas Tbl = new TblAccountGroupMas();
            if (model.PkAccountGroupId > 0)
            {
                var _entity = __dbContext.TblAccountGroupMas.Find(model.PkAccountGroupId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkAccountGroupId = model.PkAccountGroupId;
            Tbl.FkAccountGroupId = model.FkAccountGroupId;
            Tbl.GroupType = model.GroupType;
            Tbl.AccountGroupName = model.AccountGroupName;
            Tbl.GroupAlias = model.GroupAlias;
            Tbl.NatureOfGroup = model.NatureOfGroup;
            Tbl.PrintDtl = model.PrintDtl;
            Tbl.NetCrDrBalanceForRpt = model.NetCrDrBalanceForRpt;
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

                AccountGroupModel oldModel = GetSingleRecord(Tbl.PkAccountGroupId);
                ID = Tbl.PkAccountGroupId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.AccountGroup, Tbl.PkAccountGroupId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.AccountGroupName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKAccountGroupID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Account Group", Fields="AccountGroupName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Alias", Fields="GroupAlias",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Master Group", Fields="PAccountGroupName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Group Type", Fields="GroupType_FullName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Nature Of Group", Fields="NatureOfGroup_FullName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Show Details In Report", Fields="PrintDtl_Status",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Net Cr/Dr Balance For Report", Fields="NetCrDrBalanceForRpt_Status",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
   };
            return list;
        }


    }
}



















