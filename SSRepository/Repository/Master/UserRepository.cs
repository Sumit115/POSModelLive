﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;

namespace SSRepository.Repository.Master
{
    public class UserRepository : Repository<TblUserMas>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }
       
        public string isAlreadyExist(UserModel model, string Mode)
        {
            dynamic cnt; string error = "";
            cnt = (from x in __dbContext.TblUserMas
                   where x.UserId == model.UserId && x.Pwd == model.Pwd && x.PkUserId != model.PkUserId
                   select x).Count();
            if (cnt > 0)
                error = "User Already Exits";
            return error;
        }

        public List<UserModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<UserModel> data = (from cou in __dbContext.TblUserMas
                                    join emp in __dbContext.TblEmployeeMas on cou.FkEmployeeId equals emp.PkEmployeeId
                                    join brch in __dbContext.TblBranchMas on cou.FkBranchId equals brch.PkBranchId
                                    join compny in __dbContext.TblCompanies on cou.FkRegId equals compny.PkCompanyId
                                    // where (EF.Functions.Like(cou.UserId.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby cou.PkUserId
                                    select (new UserModel
                                    {
                                        PkUserId = cou.PkUserId,
                                        UserId = cou.UserId,
                                        Pwd = cou.Pwd,
                                        FkRegId = cou.FkRegId,
                                        Usertype = cou.Usertype,
                                        FkBranchId = cou.FkBranchId,
                                        FkRoleId = cou.FkRoleId,
                                        Expiredt = cou.Expiredt,
                                        ExpirePwddt = cou.ExpirePwddt,
                                        FkEmployeeId = cou.FkEmployeeId,
                                        IsAdmin = cou.IsAdmin,
                                        //EmployeeVM = new EmployeeModel
                                        //{
                                        //    PkEmployeeId = emp.PkEmployeeId,
                                        EmployeeName = emp.Name,
                                        //},
                                        //BranchVM = new BranchModel
                                        //{
                                        //    PkBranchId = brch.PkBranchId,
                                        BranchName = brch.BranchName,
                                        //}
                                        FKUserID = cou.FKUserID,
                                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                    }
                                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public UserModel GetSingleRecord(long PkUserId)
        {

            UserModel data = new UserModel();
            data = (from cou in __dbContext.TblUserMas
                    where cou.PkUserId == PkUserId
                    select (new UserModel
                    {
                        PkUserId = cou.PkUserId,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        UserId = cou.UserId,
                        Pwd = cou.Pwd,
                        FkRegId = cou.FkRegId,
                        Usertype = cou.Usertype,
                        FkBranchId = cou.FkBranchId,
                        FkRoleId = cou.FkRoleId,
                        Expiredt = cou.Expiredt,
                        ExpirePwddt = cou.ExpirePwddt,
                        FkEmployeeId = cou.FkEmployeeId,
                        IsAdmin = cou.IsAdmin,
                    })).FirstOrDefault();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1,  string search = "")
        {

            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                var result = GetList(pageSize, pageNo, search);

                return (from r in result
                        select new
                        {
                            PkId = r.PkUserId,
                            Name =r.UserId
                        }).ToList();
            }
            else
            {
                return null;
            }
        }
        public string DeleteRecord(long PkUserId)
        {
            string Error = "";
            UserModel obj = GetSingleRecord(PkUserId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkUserId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblUserMas
                           where x.PkUserId == PkUserId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblUserMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkUserId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkUserId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetUserID(), PkUserId, obj.FKUserID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            UserModel model = (UserModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            UserModel model = (UserModel)objmodel;
            TblUserMas Tbl = new TblUserMas();
            if (model.PkUserId > 0)
            {
                var _entity = __dbContext.TblUserMas.Find(model.PkUserId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }
            Tbl.PkUserId = model.PkUserId;
            Tbl.UserId = model.UserId;
            Tbl.Pwd = model.Pwd;
            Tbl.FkRegId = model.FkRegId;
            Tbl.Usertype = model.Usertype;
            Tbl.FkBranchId = model.FkBranchId;
            Tbl.FkRoleId = model.FkRoleId;
            Tbl.Expiredt = model.Expiredt;
            Tbl.ExpirePwddt = model.ExpirePwddt;
            Tbl.FkEmployeeId = model.FkEmployeeId;
            Tbl.IsAdmin = model.IsAdmin;
            Tbl.ModifiedDate= DateTime.Now;
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
                UserModel oldModel = GetSingleRecord(Tbl.PkUserId);
                ID = Tbl.PkUserId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.User, Tbl.PkUserId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.UserName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKUserID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Branch", Fields="BranchName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Employee", Fields="EmployeeName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
     new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="User Id", Fields="UserId",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
     new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Password", Fields="Pwd",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
     new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

            };
            return list;
        }

    }
}



















