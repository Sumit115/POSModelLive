using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using LMS.Data;

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
                                    where (EF.Functions.Like(cou.UserId.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby cou.UserId
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
                                       BranchName = cou.FkBranch.BranchName,
                                       FkRoleId = cou.FkRoleId,
                                       Role = cou.FkRole.RoleName,
                                       Expiredt = cou.Expiredt,
                                       ExpirePwddt = cou.ExpirePwddt,
                                       FkEmployeeId = cou.FkEmployeeId,
                                       EmployeeName = cou.FkEmployee.Name,
                                       IsAdmin = cou.IsAdmin,
                                   })).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
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
                        BranchName = cou.FkBranch.BranchName,
                        FkRoleId = cou.FkRoleId,
                        Role = cou.FkRole.RoleName,
                        Expiredt = cou.Expiredt,
                        ExpirePwddt = cou.ExpirePwddt,
                        FkEmployeeId = cou.FkEmployeeId,
                        EmployeeName= cou.FkEmployee.Name,
                        IsAdmin = cou.IsAdmin,
                    })).FirstOrDefault();
            if (data != null)
            {
                data.UserLoclnk = (from ad in __dbContext.TblUserLocLnk
                                   where (ad.FKUserID == data.PkUserId)
                                   select (new UserLocLnkModel
                                   {
                                       FkLocationID = ad.FKLocationID,
                                       FkUserID = ad.FKUserID,
                                       LocationName = ad.FKLocation.Location,
                                   })).ToList();
            }
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
                var _entity = GetSingleRecord(model.PkUserId);                
                IList<TblUserLocLnk> userlnk = (from x in __dbContext.TblUserLocLnk
                                                where x.FKUserID == model.PkUserId
                                                select x).ToList();

                if (userlnk.Any())
                {
                    DeleteData(userlnk, true);
                }
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
            if (model.UserLoclnk != null)
            {
                List<TblUserLocLnk> lstAdd = new List<TblUserLocLnk>();
                foreach (var item in model.UserLoclnk)
                {
                    TblUserLocLnk locObj = new TblUserLocLnk();
                    locObj.FKLocationID = item.FkLocationID;
                    locObj.FKUserID = Tbl.PkUserId;
                    if (item.ModeForm == 0)
                    {
                        Tbl.LocationUsers.Add(locObj);
                    }
                }
            }
            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                //Tbl.PkUserId = ID = getIdOfSeriesByEntity("PkUserId", null, Tbl, "TblUserMas");
                AddData(Tbl, false);
            }
            else
            {
                UserModel oldModel = GetSingleRecord(Tbl.PkUserId);
                ID = Tbl.PkUserId;

                Tbl.FKCreatedByID = oldModel.FKUserID;
                Tbl.CreationDate = Convert.ToDateTime(oldModel.DATE_MODIFIED);
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.User, Tbl.PkUserId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.UserName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKUserID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
            
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="User", Fields="UserId",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Branch", Fields="BranchName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Employee", Fields="EmployeeName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Role", Fields="Role",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Expire date", Fields="Expiredt",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
               new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },

            };
            return list;
        }

    }
}



















