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
        public BankRepository(AppDbContext dbContext) : base(dbContext)
        {
            __FormID = (long)en_Form.Bank;
        }
       
        public string isAlreadyExist(BankModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.BankName))
            {
                cnt = (from x in __dbContext.TblBankMas
                       where x.BankName == model.BankName && x.PkBankId != model.PkBankId
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
                                          PkBankId = cou.PkBankId,
                                          FKUserId = cou.FKUserId,
                                          src = cou.Src,
                                          DATE_MODIFIED = cou.DateModified,
                                          DATE_CREATED = cou.DateCreated,
                                          BankName = cou.BankName, 
                                      }
                                     )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public BankModel GetSingleRecord(long PkBankId)
        {

            BankModel data = new BankModel();
            data = (from cou in __dbContext.TblBankMas
                    where cou.PkBankId == PkBankId
                    select (new BankModel
                    {
                        PkBankId = cou.PkBankId,
                        FKUserId = cou.FKUserId,
                        src = cou.Src,
                        DATE_MODIFIED = cou.DateModified,
                        DATE_CREATED = cou.DateCreated,
                        BankName = cou.BankName,
                       
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpBank(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkBankId,
                        r.BankName
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkBankId)
        {
            string Error = "";
            BankModel obj = GetSingleRecord(PkBankId);

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
                    __dbContext.TblBankMas.RemoveRange(lst);

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
            if (model.PkBankId > 0)
            {
                var _entity = __dbContext.TblBankMas.Find(model.PkBankId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkBankId = model.PkBankId;
            Tbl.BankName = model.BankName;
         
            Tbl.DateModified = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.Src = model.src;
                Tbl.FKUserId = model.FKUserId;
                Tbl.DateCreated = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                BankModel oldModel = GetSingleRecord(Tbl.PkBankId);
                ID = Tbl.PkBankId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKBankID, oldModel.PkBankId, oldModel.FKBankID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBankID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList()
        {
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="DateCreated",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Bank Name", Fields="BankName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                      };
            return list;
        }


    }
}



















