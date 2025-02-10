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
                                            join CatPGrp in __dbContext.TblAccountGroupMas on cou.FkAccountGroupId equals CatPGrp.PkAccountGroupId
                                                            into tempAccGrp
                                            from AccGrp in tempAccGrp.DefaultIfEmpty()
                                            where (EF.Functions.Like(cou.AccountGroupName.Trim().ToLower(), search + "%"))
                                            orderby cou.PkAccountGroupId
                                            select (new AccountGroupModel
                                            {
                                                PkAccountGroupId = cou.PkAccountGroupId,
                                                FKUserId = cou.FKUserID,
                                                FKCreatedByID = cou.FKCreatedByID,
                                                AccountGroupName = cou.AccountGroupName,
                                                FkAccountGroupId = cou.FkAccountGroupId,
                                                PAccountGroupName = AccGrp.AccountGroupName,
                                                GroupType = cou.GroupType,
                                                GroupAlias = cou.GroupAlias,
                                                NatureOfGroup = cou.NatureOfGroup,
                                                PrintDtl = cou.PrintDtl,
                                                NetCrDrBalanceForRpt = cou.NetCrDrBalanceForRpt,
                                            }
                                           )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public AccountGroupModel GetSingleRecord(long PkAccountGroupId)
        {

            AccountGroupModel data = new AccountGroupModel();
            data = (from cou in __dbContext.TblAccountGroupMas
                    where cou.PkAccountGroupId == PkAccountGroupId
                    select (new AccountGroupModel
                    {
                        PkAccountGroupId = cou.PkAccountGroupId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        AccountGroupName = cou.AccountGroupName,
                        FkAccountGroupId = cou.FkAccountGroupId,
                        GroupType = cou.GroupType,
                        GroupAlias = cou.GroupAlias,
                        NatureOfGroup = cou.NatureOfGroup,
                        PrintDtl = cou.PrintDtl,
                        NetCrDrBalanceForRpt = cou.NetCrDrBalanceForRpt,
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
            AccountGroupModel obj = GetSingleRecord(PkAccountGroupId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkAccountGroupId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblAccountGroupMas
                           where x.PkAccountGroupId == PkAccountGroupId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblAccountGroupMas.RemoveRange(lst);

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
            Tbl.FKUserID = model.FKUserId;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.CreationDate = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                AccountGroupModel oldModel = GetSingleRecord(Tbl.PkAccountGroupId);
                ID = Tbl.PkAccountGroupId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKAccountGroupID, oldModel.PkAccountGroupId, oldModel.FKAccountGroupID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKAccountGroupID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                   new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Account Group", Fields="AccountGroupName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Alias", Fields="GroupAlias",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Master Group", Fields="PAccountGroupName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Group Type", Fields="GroupType_FullName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                   new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Nature Of Group", Fields="NatureOfGroup_FullName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Show Details In Report", Fields="PrintDtl_Status",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Net Cr/Dr Balance For Report", Fields="NetCrDrBalanceForRpt_Status",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















