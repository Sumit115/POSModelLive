using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using Azure;

namespace SSRepository.Repository.Master
{
    public class CreditCardTypeRepository : Repository<TblCreditCardTypeMas>, ICreditCardTypeRepository
    {
        public CreditCardTypeRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public List<CreditCardTypeModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CreditCardTypeModel> data = (from cou in __dbContext.TblCreditCardTypeMas
                                              where (EF.Functions.Like(cou.CreditCardType.Trim().ToLower(), Convert.ToString(search) + "%"))
                                              orderby cou.PkCreditCardTypeId
                                              select (new CreditCardTypeModel
                                              {
                                                  PKID = cou.PkCreditCardTypeId,
                                                  CreditCardType = cou.CreditCardType,
                                                  FkAccountID = cou.FkAccountID,
                                                  Assembly = cou.Assembly,
                                                  Class = cou.Class,
                                                  Method = cou.Method,
                                                  AccountName = cou.FKAccount.Account,
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
                return ((from cou in __dbContext.TblCreditCardTypeMas
                         where EF.Functions.Like(cou.CreditCardType.Trim().ToLower(), search + "%")
                         orderby cou.CreditCardType
                         select (new
                         {
                             cou.PkCreditCardTypeId,
                             //PkId = cou.PkCreditCardTypeId,
                             cou.CreditCardType,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public CreditCardTypeModel GetSingleRecord(long PkCreditCardTypeId)
        {

            CreditCardTypeModel data = (from cou in __dbContext.TblCreditCardTypeMas
                                        where cou.PkCreditCardTypeId == PkCreditCardTypeId
                                        select (new CreditCardTypeModel
                                        {
                                            PKID = cou.PkCreditCardTypeId,
                                            CreditCardType = cou.CreditCardType,
                                            FkAccountID = cou.FkAccountID,
                                            Assembly = cou.Assembly,
                                            Class = cou.Class,
                                            Method = cou.Method,
                                            AccountName = cou.FKAccount.Account,
                                            FKUserID = cou.FKUserID,
                                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                                            UserName = cou.FKUser.UserId,
                                        })).SingleOrDefault();
            return data;
        }

   
        public string DeleteRecord(long PKID)
        {
            string Error = "";
            CreditCardTypeModel oldModel = GetSingleRecord(PKID);

            //var saleOrderExist = (from cou in __dbContext.TblSalesOrdertrn
            //                      join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
            //                      where cou.FkCreditCardTypeId == PKID && ser.TranAlias == "SORD"
            //                      select cou).Count();
            //if (saleOrderExist > 0)
            //    Error += "use in other transaction";

              

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCreditCardTypeMas
                           where x.PkCreditCardTypeId == PKID
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCreditCardTypeMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.CreditCardType, PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.CreditCardType, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }

        public override string ValidateData(object objmodel, string Mode)
        {

            CreditCardTypeModel model = (CreditCardTypeModel)objmodel;
            string error = "";
            if (string.IsNullOrEmpty(model.CreditCardType))
            {
                error += "Enter Credit Card Type";
            }
            if (model.FkAccountID==null)
            {
                error += "Select Account";
            }
            if (error == "")
            {
                error += isAlreadyExist(model, Mode);
            }
            return error;

        }


        private string isAlreadyExist(CreditCardTypeModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.CreditCardType))
            {
                cnt = (from x in __dbContext.TblCreditCardTypeMas
                       where x.CreditCardType == model.CreditCardType && x.PkCreditCardTypeId != model.PKID
                       select x).Count();
                if (cnt > 0)
                    error = "Credit Card Type Already Exits !";
            }
            
            return error;
        }

        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CreditCardTypeModel model = (CreditCardTypeModel)objmodel;
            TblCreditCardTypeMas Tbl = new TblCreditCardTypeMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblCreditCardTypeMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCreditCardTypeId = model.PKID;
            Tbl.CreditCardType = model.CreditCardType;
            Tbl.FkAccountID = model.FkAccountID;
            Tbl.Assembly = model.Assembly;
            Tbl.Class = model.Class;
            Tbl.Method = model.Method;
            Tbl.Parameter = model.Parameter; 
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {
                ID = Tbl.PkCreditCardTypeId = getIdOfSeriesByEntity("PkCreditCardTypeId", null, Tbl, "TblCreditCardTypeMas");
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                 AddData(Tbl, false);
            }
            else
            {
                Tbl.FkAccountID = model.FkAccountID;
                CreditCardTypeModel oldModel = GetSingleRecord(Tbl.PkCreditCardTypeId);
                ID = Tbl.PkCreditCardTypeId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.CreditCardType, Tbl.PkCreditCardTypeId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.CreditCardType, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCreditCardTypeID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }


        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Credit Card Type", Fields="CreditCardType",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++,  Heading ="Account", Fields="AccountName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Assembly", Fields="Assembly",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Class", Fields="Class",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Method", Fields="Method",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Parameter", Fields="Parameter",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
         };
            return list;
        }


    }
}



















