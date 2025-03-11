using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Azure;
using static Handler;

namespace SSRepository.Repository.Master
{
    public class RoleRepository : Repository<TblRoleMas>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
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

        public List<RoleModel> GetList(int pageSize, int pageNo = 1, string search = "", long FKUserID = 0)
        {
            if (FKUserID != 0 && IsAdmin() != 1)
            {
                FKUserID = GetUserID();
            }
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<RoleModel> data = (from cou in __dbContext.TblRoleMas
                                    where (EF.Functions.Like(cou.RoleName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    && (FKUserID == 0 || cou.FKUserID == FKUserID)
                                    orderby cou.PkRoleId
                                    select (new RoleModel
                                    {
                                        PkRoleId = cou.PkRoleId,
                                        RoleName = cou.RoleName,
                                        FKUserID = cou.FKUserID,
                                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                    }
                                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);

            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                return (from r in result
                        select new
                        {
                            r.PkRoleId,
                            r.RoleName
                        }).ToList();
            }
            else if (EnCustomFlag == (int)Handler.en_CustomFlag.Filter)
            {
                return (from r in result
                        select new
                        {
                            r.PkRoleId,
                            r.RoleName
                        }).ToList(); ;
            }
            else
            {
                return null;
            }
            
        }

        public RoleModel GetSingleRecord(long PkRoleId, bool IsAccess = false)
        {
            RoleModel data = (from cou in __dbContext.TblRoleMas
                              where cou.PkRoleId == PkRoleId
                              select (new RoleModel
                              {
                                  PkRoleId = cou.PkRoleId,
                                  RoleName = cou.RoleName,
                                  FKUserID = cou.FKUserID,
                                  DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                              })).FirstOrDefault();

            if (data != null)
            {
                data.RoleDtls = (from r in __dbContext.TblRoleDtl
                                 join f in __dbContext.TblFormMas on r.FKFormID equals f.PKFormID
                                 where r.FkRoleID == data.PkRoleId && (IsAccess == false || r.IsAccess == IsAccess)
                                 orderby f.SeqNo
                                 select (new RoleDtlModel
                                 {
                                     PkRoleDtlId = r.PkRoleDtlId,
                                     FkRoleID = r.FkRoleID,
                                     FKFormID = r.FKFormID,
                                     FKMasterFormID = f.FKMasterFormID,
                                     SeqNo = f.SeqNo,
                                     FormName = f.FormName,
                                     ShortName = f.FormName,
                                     ShortCut = f.ShortCut,
                                     ToolTip = f.ToolTip,
                                     Image = f.Image,
                                     WebURL = f.WebURL,
                                     IsAccess = r.IsAccess,
                                     IsEdit = r.IsEdit,
                                     IsCreate = r.IsCreate,
                                     IsPrint = r.IsPrint,
                                     IsBrowse = r.IsBrowse
                                 })).ToList();

                data.RoleDtls = BuildMenuTree(data.RoleDtls, null);
            }
            return data;
        }


        private List<RoleDtlModel> BuildMenuTree(List<RoleDtlModel>? allMenuItems, long? parentId)
        {
            if (allMenuItems != null)
            {
                return allMenuItems
               .Where(m => m.FKMasterFormID == parentId)
               .Select(m => new RoleDtlModel
               {
                   PkRoleDtlId = m.PkRoleDtlId,
                   FkRoleID = m.FkRoleID,
                   FKFormID = m.FKFormID,
                   FKMasterFormID = m.FKMasterFormID,
                   SeqNo = m.SeqNo,
                   FormName = m.FormName,
                   ShortName = m.FormName,
                   ShortCut = m.ShortCut,
                   ToolTip = m.ToolTip,
                   Image = m.Image,
                   WebURL = m.WebURL,
                   IsAccess = m.IsAccess,
                   IsEdit = m.IsEdit,
                   IsCreate = m.IsCreate,
                   IsPrint = m.IsPrint,
                   IsBrowse = m.IsBrowse,
                   SubMenu = BuildMenuTree(allMenuItems, m.FKFormID) // Recursive call for nested menus
               })
               .ToList();
            }
            else
            {
                return new List<RoleDtlModel>();
            }
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
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                Tbl.PkRoleId = getIdOfSeriesByEntity("PkRoleId", null, Tbl, "TblRoleMas");
                AddData(Tbl, false);
            }
            else
            {

                RoleModel oldModel = GetSingleRecord(Tbl.PkRoleId);
                ID = Tbl.PkRoleId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Role, Tbl.PkRoleId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.RoleName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }


            if (model.RoleDtls != null)
            {
                MenuAdd(model.RoleDtls, Tbl.PkRoleId);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKRoleID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }

        private bool MenuAdd(List<RoleDtlModel> RoleDtls, long PkRoleId)
        {
            bool flag = false;
            foreach (var item in RoleDtls)
            {
                TblRoleDtl tbl = new TblRoleDtl();
                tbl.PkRoleDtlId = item.PkRoleDtlId;
                tbl.FkRoleID = PkRoleId;
                tbl.FKFormID = item.FKFormID;
                tbl.IsEdit = item.IsEdit;
                tbl.IsCreate = item.IsCreate;
                tbl.IsPrint = item.IsPrint;
                tbl.IsBrowse = item.IsBrowse;
                if (item.SubMenu != null && item.SubMenu.Any())
                {
                    tbl.IsAccess = MenuAdd(item.SubMenu, PkRoleId);
                }
                else
                {
                    tbl.IsAccess = item.IsAccess;
                }
                if (tbl.IsAccess)
                {
                    flag = true;
                }
                if (tbl.PkRoleDtlId > 0)
                    UpdateData(tbl, false);
                else
                    AddData(tbl, false);

            }

            return flag;

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



















