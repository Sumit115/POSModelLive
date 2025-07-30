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
                   where x.UserId == model.UserId && x.Pwd == model.Pwd && x.PkUserId != model.PKID
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
                                       PKID = cou.PkUserId,
                                       FKUserID = cou.FKUserID,
                                       UserName = cou.FKUser.UserId,
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
                                       EditBatch = cou.EditBatch??false,
                                       EditColor = cou.EditColor ?? false,
                                       EditDiscount = cou.EditDiscount ?? false,
                                       EditRate = cou.EditRate ?? false,
                                       EditMRP = cou.EditMRP ?? false,
                                       EditPurRate = cou.EditPurRate ?? false,
                                       EditPurDiscount = cou.EditPurDiscount ?? false,
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
                        PKID = cou.PkUserId,
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
                        EditBatch = cou.EditBatch ?? false,
                        EditColor = cou.EditColor ?? false,
                        EditDiscount = cou.EditDiscount ?? false,
                        EditRate = cou.EditRate ?? false,
                        EditMRP = cou.EditMRP ?? false,
                        EditPurRate = cou.EditPurRate ?? false,
                        EditPurDiscount = cou.EditPurDiscount ?? false,
                    })).FirstOrDefault();
            if (data != null)
            {
                data.UserLoclnk = (from ad in __dbContext.TblUserLocLnk
                                   where (ad.FKUserID == data.PKID)
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
                            PkId = r.PKID,
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
            UserModel oldModel = GetSingleRecord(PkUserId);

           
            if (Error == "")
            {
                var lstLoclnk = (from x in __dbContext.TblUserLocLnk
                           where x.FKUserID == PkUserId
                           select x).ToList();
                if (lstLoclnk.Count > 0)
                    __dbContext.TblUserLocLnk.RemoveRange(lstLoclnk);


                var lst = (from x in __dbContext.TblUserMas
                           where x.PkUserId == PkUserId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblUserMas.RemoveRange(lst);

            
                  AddMasterLog((long)Handler.Form.User, PkUserId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.UserName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
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
            if (model.PKID > 0)
            {
                var _entity = GetSingleRecord(model.PKID);                
                IList<TblUserLocLnk> userlnk = (from x in __dbContext.TblUserLocLnk
                                                where x.FKUserID == model.PKID
                                                select x).ToList();

                if (userlnk.Any())
                {
                    DeleteData(userlnk, true);
                }
            }
            Tbl.PkUserId = model.PKID;
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
            Tbl.EditBatch = model.EditBatch;
            Tbl.EditColor = model.EditColor;
            Tbl.EditDiscount = model.EditDiscount;
            Tbl.EditRate = model.EditRate;
            Tbl.EditMRP = model.EditMRP;
            Tbl.EditPurRate = model.EditPurRate;
            Tbl.EditPurDiscount = model.EditPurDiscount;
            Tbl.ModifiedDate= DateTime.Now;
            Tbl.FKUserID = GetUserID();
             
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
                        lstAdd.Add(locObj);
                    }
                }
                if (lstAdd.Count > 0)
                    AddData(lstAdd, true);
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



















