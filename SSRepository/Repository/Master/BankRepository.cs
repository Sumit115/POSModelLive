using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class BankRepository : Repository<TblBankMas>, IBankRepository
    {
        public BankRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(BankModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.BankName))
            {
                cnt = (from x in __dbContext.TblBankMas
                       where x.BankName == model.BankName && x.PkBankId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Bank Name Exits";
            }

            return error;
        }

        public List<BankModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<BankModel> data = (from cou in __dbContext.TblBankMas

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby cou.PkBankId
                                    select (new BankModel
                                    {
                                        PKID = cou.PkBankId,
                                        BankName = cou.BankName,
                                        IFSCCode = cou.IFSCCode,
                                        FKUserID = cou.FKUserID,
                                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                        UserName = cou.FKUser.UserId,
                                    }
                                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblBankMas
                         where (EF.Functions.Like(cou.BankName.Trim().ToLower(), search + "%"))
                         orderby cou.BankName
                         select (new
                         {
                             cou.PkBankId,
                             cou.BankName,
                             cou.IFSCCode,
                         }
                        )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public BankModel GetSingleRecord(long PkBankId)
        {

            BankModel data = new BankModel();
            data = (from cou in __dbContext.TblBankMas
                    where cou.PkBankId == PkBankId
                    select (new BankModel
                    {
                        PKID = cou.PkBankId,
                        BankName = cou.BankName,
                        IFSCCode = cou.IFSCCode,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpBank(int pageSize, int pageno, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageno, search);

            result.Insert(0, new BankModel { PKID = 0, BankName = "Select" });

            return (from r in result
                    select new
                    {
                        r.PKID,
                        r.BankName,
                        r.IFSCCode
                    }).ToList().OrderBy(x => x.PKID).ToList();
        }

        public string DeleteRecord(long PkBankId)
        {
            string Error = "";
            BankModel oldModel = GetSingleRecord(PkBankId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkBankId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblBankMas
                           where x.PkBankId == PkBankId
                           select x).ToList();
                if (lst.Count > 0)
                {
                    __dbContext.TblBankMas.RemoveRange(lst);

                    AddMasterLog((long)Handler.Form.Bank, oldModel.PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.BankName, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                }
                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkBankId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkBankId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetBankID(), PkBankId, obj.FKBankID, obj.DATE_MODIFIED, true);

                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            BankModel model = (BankModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            BankModel model = (BankModel)objmodel;
            TblBankMas Tbl = new TblBankMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblBankMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkBankId = model.PKID;
            Tbl.BankName = model.BankName;
            Tbl.IFSCCode = model.IFSCCode;
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

                BankModel oldModel = GetSingleRecord(Tbl.PkBankId);
                ID = Tbl.PkBankId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Bank, Tbl.PkBankId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.BankName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBankID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Bank Name", Fields="BankName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="IFSC Code", Fields="IFSCCode",Width=40,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
          };
            return list;
        }


    }
}



















