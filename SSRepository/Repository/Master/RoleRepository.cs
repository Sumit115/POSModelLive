using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Linq;

namespace SSRepository.Repository.Master
{
    public class RoleRepository : Repository<TblRoleMas>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(RoleModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                cnt = (from x in __dbContext.TblRoleMas
                       where x.RoleName == model.RoleName && x.PkRoleId != model.PkRoleId
                       select x).Count();
                if (cnt > 0)
                    error = "Section Name Already Exits";
            }

            return error;
        }

        public List<RoleModel> GetList(int pageSize, int pageNo = 1, string search = "", long RoleGroupId = 0)
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<RoleModel> data = (from cou in __dbContext.TblRoleMas
                                    where (EF.Functions.Like(cou.RoleName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby cou.PkRoleId
                                    select (new RoleModel
                                    {
                                        PkRoleId = cou.PkRoleId,
                                        FKUserId = cou.FKUserID,
                                        FKCreatedByID = cou.FKCreatedByID,
                                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                                        RoleName = cou.RoleName,
                                    }
                                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object GetDrpRole(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkRoleId,
                        r.RoleName
                    }).ToList(); ;
        }

        public RoleModel GetSingleRecord(long PkRoleId)
        {

            RoleModel data = new RoleModel();
            data = (from cou in __dbContext.TblRoleMas
                    where cou.PkRoleId == PkRoleId
                    select (new RoleModel
                    {
                        PkRoleId = cou.PkRoleId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                        RoleName = cou.RoleName,
                        RoleDtl_lst = (from ad in __dbContext.TblRoleDtl
                                           //  join loc in __dbContext.TblBranchMas on ad.FKLocationID equals loc.PkBranchId
                                       where (ad.FkRoleID == cou.PkRoleId)
                                       select (new RoleDtlModel
                                       {
                                           PkRoleDtlId = ad.PkRoleDtlId,
                                           FKFormID = ad.FKFormID,
                                           IsAccess = ad.IsAccess,
                                           IsEdit = ad.IsEdit,
                                           IsCreate = ad.IsCreate,
                                           IsPrint = ad.IsPrint,
                                           IsBrowse = ad.IsBrowse,
                                           //FKUserId = ad.FKUserID,
                                           //ModifiDate = ad.ModifiedDate.ToString("dd-MMM-yyyy"),
                                           //FKCreatedByID = ad.FKCreatedByID,
                                           //CreateDate = ad.CreationDate.ToString("dd-MMM-yyyy"),
                                           FkRoleID = ad.FkRoleID,
                                       })).ToList(),
                    })).FirstOrDefault();

            
            return data;
        }

        public string DeleteRecord(long PkRoleId)
        {
            string Error = "";
            RoleModel obj = GetSingleRecord(PkRoleId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkRoleId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblRoleMas
                           where x.PkRoleId == PkRoleId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblRoleMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkRoleId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkRoleId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetRoleID(), PkRoleId, obj.FKRoleID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            RoleModel model = (RoleModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            RoleModel model = (RoleModel)objmodel;
            TblRoleMas Tbl = new TblRoleMas();
            if (model.PkRoleId > 0)
            {
                var _entity = __dbContext.TblRoleMas.Find(model.PkRoleId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkRoleId = model.PkRoleId;
            Tbl.RoleName = model.RoleName;
            Tbl.ModifiedDate = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = model.FKCreatedByID;
                Tbl.FKUserID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                Tbl.PkRoleId = getIdOfSeriesByEntity("PkRoleId", null, Tbl, "TblRoleMas");
                AddData(Tbl, false);
            }
            else
            {

                RoleModel oldModel = GetSingleRecord(Tbl.PkRoleId);
                ID = Tbl.PkRoleId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKRoleID, oldModel.PkRoleId, oldModel.FKRoleID, oldModel.DATE_MODIFIED);
            }


            if (model.RoleDtl_lst != null)
            {
                List<TblRoleDtl> lstAdd = new List<TblRoleDtl>();
                List<TblRoleDtl> lstEdit = new List<TblRoleDtl>();
                List<TblRoleDtl> lstDel = new List<TblRoleDtl>();
                foreach (var item in model.RoleDtl_lst)
                {
                    TblRoleDtl locObj = new TblRoleDtl();
                    locObj.FKFormID = item.FKFormID;
                    locObj.IsAccess = item.IsAccess;
                    locObj.IsEdit = item.IsEdit;
                    locObj.IsCreate = item.IsCreate;
                    locObj.IsPrint = item.IsPrint;
                    locObj.IsBrowse = item.IsBrowse;
                    locObj.FKUserID = Tbl.FKUserID;
                    locObj.ModifiedDate = Tbl.ModifiedDate;
                    locObj.FKCreatedByID = Tbl.FKCreatedByID;
                    locObj.CreationDate = Tbl.CreationDate;
                    locObj.FkRoleID = Tbl.PkRoleId;



                    //   lstAdd.Add(locObj);
                    if (item.PkRoleDtlId >0)
                    {
                        locObj.PkRoleDtlId = item.PkRoleDtlId;
                        locObj.ModifiedDate = DateTime.Now;
                        lstEdit.Add(locObj);
                    }
                    else if (item.PkRoleDtlId == 0)
                    {
                        //  locObj.PKAccountDtlId = getIdOfSeriesByEntity("PKAccountDtlId", null, Tbl, "TblAccountDtl");
                        locObj.FKCreatedByID = Tbl.FKCreatedByID;
                        locObj.FKUserID = Tbl.FKUserID;
                        locObj.CreationDate = DateTime.Now;
                        locObj.ModifiedDate = DateTime.Now;
                        lstAdd.Add(locObj);
                    }
                    else
                    {
                        var res1 = (from x in __dbContext.TblRoleDtl
                                    where x.FkRoleID == Tbl.PkRoleId
                                    && x.PkRoleDtlId != item.PkRoleDtlId
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
            //AddImagesAndRemark(obj.PkcountryId, obj.FKRoleID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Name", Fields="RoleName",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
               };
            return list;
        }


    }
}



















