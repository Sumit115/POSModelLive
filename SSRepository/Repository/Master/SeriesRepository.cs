using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using System.Runtime.ConstrainedExecution;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;

namespace SSRepository.Repository.Master
{
    public class SeriesRepository : Repository<TblSeriesMas>, ISeriesRepository
    {
        public SeriesRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(SeriesModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            //if (!string.IsNullOrEmpty(model.Mobile))
            //{
            //    cnt = (from x in __dbContext.TblSeriesMas
            //           where x.Mobile == model.Mobile && x.PkSeriesId != model.PkSeriesId
            //           select x).Count();
            //    if (cnt > 0)
            //        error = "Mobile Already Exits";
            //}

            return error;
        }
        public string isAvailableForEdit(SeriesModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (model.PKID > 0)
            {
                //if (model.TranAlias == "SORD") { error = __dbContext.TblSalesOrdertrn.Where(x => x.FKSeriesId == model.PKID).ToList().Count > 0 ? "Update Not Available" : ""; }
                //if (model.TranAlias == "SINV") { error = __dbContext.TblSalesInvoicetrn.Where(x => x.FKSeriesId == model.PKID).ToList().Count > 0 ? "Update Not Available" : ""; }
                //if (model.TranAlias == "SCHN") { error = __dbContext.TblSalesChallantrn.Where(x => x.FKSeriesId == model.PKID).ToList().Count > 0 ? "Update Not Available" : ""; }
                //if (model.TranAlias == "PORD") { error = __dbContext.TblPurchaseOrdertrn.Where(x => x.FKSeriesId == model.PKID).ToList().Count > 0 ? "Update Not Available" : ""; }
                //if (model.TranAlias == "PINV") { error = __dbContext.TblPurchaseInvoicetrn.Where(x => x.FKSeriesId == model.PKID).ToList().Count > 0 ? "Update Not Available" : ""; }
            }

            return error;
        }

        public List<SeriesModel> GetList(int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {
            List<SeriesModel> data = new List<SeriesModel>();
            try
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                data = (from cou in __dbContext.TblSeriesMas
                        where EF.Functions.Like(cou.Series.Trim().ToLower(), search + "%")
                       && (TranAlias == "" || cou.TranAlias == TranAlias)
                       && (DocumentType == "" || cou.DocumentType == DocumentType)
                        orderby cou.Series
                        select (new SeriesModel
                        {
                            PKID = cou.PkSeriesId,
                            Series = cou.Series,
                            SeriesNo = cou.SeriesNo,
                            //FkBranchId = cou.FkBranchId,
                            BillingRate = cou.BillingRate,
                            TranAlias = cou.TranAlias ?? "",
                            FormatName = cou.FormatName,
                            ResetNoFor = cou.ResetNoFor,
                            AllowWalkIn = cou.AllowWalkIn,
                            AutoApplyPromo = cou.AutoApplyPromo,
                            RoundOff = cou.RoundOff,
                            DefaultQty = cou.DefaultQty,
                            AllowZeroRate = cou.AllowZeroRate,
                            AllowFreeQty = cou.AllowFreeQty,
                            DocumentType = cou.DocumentType ?? "",
                            FKLocationID = cou.FKLocationID,
                            Location = cou.FKLocation.Location,
                            TaxType = cou.TaxType,
                            PaymentMode = cou.PaymentMode,
                            FKUserID = cou.FKUserID,
                            UserName = cou.FKUser.UserId,
                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                            //TranAliasName= GetTranAliasName(cou.TranAlias),
                        }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {

            }
            return data.ToList();
        }


        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", string TranAlias = "", string DocumentType = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblSeriesMas
                         where EF.Functions.Like(cou.Series.Trim().ToLower(), search + "%")
                        && (TranAlias == "" || cou.TranAlias == TranAlias)
                        && (DocumentType == "" || cou.DocumentType == DocumentType)
                        && BillingLocation.Contains(cou.FKLocationID.ToString())
                         orderby cou.PkSeriesId
                         select (new
                         {
                             cou.PkSeriesId,
                             cou.Series,
                             Location = cou.FKLocation.Location,
                             cou.SeriesNo,
                             cou.BillingRate,
                             cou.TranAlias,

                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else if (EnCustomFlag == (int)Handler.en_CustomFlag.Filter)
            {
                var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblSeriesMas
                             //join _tranAlias in GetDrpTranAlias().ToList() on cou.TranAlias equals _tranAlias.Value
                         where EF.Functions.Like(cou.Series.Trim().ToLower(), search + "%")
                          && (TranAlias == "" || cou.TranAlias == TranAlias)
                         && (DocumentType == "" || cou.DocumentType == DocumentType)
                        && BillingLocation.Contains(cou.FKLocationID.ToString())
                         orderby cou.PkSeriesId
                         select (new
                         {
                             cou.PkSeriesId,
                             cou.Series,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }
        public SeriesModel GetSingleRecord(long PkSeriesId)
        {

            SeriesModel data = new SeriesModel();
            data = (from cou in __dbContext.TblSeriesMas
                        //join location in __dbContext.TblLocationMas on cou.FKLocationID equals location.PkLocationID
                        //join branch in __dbContext.TblBranchMas on location.FkBranchID equals branch.PkBranchId
                    where cou.PkSeriesId == PkSeriesId
                    select (new SeriesModel
                    {
                        PKID = cou.PkSeriesId,
                        Series = cou.Series,
                        SeriesNo = cou.SeriesNo,
                        //FkBranchId = cou.FkBranchId,
                        BillingRate = cou.BillingRate,
                        TranAlias = cou.TranAlias ?? "",
                        FormatName = cou.FormatName,
                        ResetNoFor = cou.ResetNoFor,
                        AllowWalkIn = cou.AllowWalkIn,
                        AutoApplyPromo = cou.AutoApplyPromo,
                        RoundOff = cou.RoundOff,
                        DefaultQty = cou.DefaultQty,
                        AllowZeroRate = cou.AllowZeroRate,
                        AllowFreeQty = cou.AllowFreeQty,
                        DocumentType = cou.DocumentType ?? "",
                        FKLocationID = cou.FKLocationID,
                        Location = cou.FKLocation.Location,
                        TaxType = cou.TaxType,
                        PaymentMode = cou.PaymentMode,
                        FKUserID = cou.FKUserID,
                        UserName = cou.FKUser.UserId,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkSeriesId)
        {
            string Error = "";
            SeriesModel oldModel = GetSingleRecord(PkSeriesId);

            var saleOrderExist = (from cou in __dbContext.TblSalesOrdertrn
                                  where cou.FKSeriesId == PkSeriesId
                                  select cou).Count();
            if (saleOrderExist > 0)
                Error += "use in other transaction";

            if (Error == "")
            {
                var saleInvoiceExist = (from cou in __dbContext.TblSalesInvoicetrn
                                        where cou.FKSeriesId == PkSeriesId
                                        select cou).Count();
                if (saleInvoiceExist > 0)
                    Error += "use in other transaction";
            }
            if (Error == "")
            {
                var saleCrNoteExist = (from cou in __dbContext.TblSalesCrNotetrn
                                        where cou.FKSeriesId == PkSeriesId
                                       select cou).Count();
                if (saleCrNoteExist > 0)
                    Error += "use in other transaction";
            }
            //if (Error == "")
            //{
            //    var saleChallanExist = (from cou in __dbContext.TblSalesChallantrn
            //                             where cou.FKSeriesId == PkSeriesId
            //                            select cou).Count();
            //    if (saleChallanExist > 0)
            //        Error += "use in other transaction";
            //}
            if (Error == "")
            {

                var purchaseOrderExist = (from cou in __dbContext.TblPurchaseOrdertrn
                                           where cou.FKSeriesId == PkSeriesId
                                          select cou).Count();
                if (purchaseOrderExist > 0)
                    Error += "use in other transaction";
            }
            if (Error == "")
            {
                var purchaseInvoiceExist = (from cou in __dbContext.TblPurchaseInvoicetrn
                                             where cou.FKSeriesId == PkSeriesId
                                            select cou).Count();
                if (purchaseInvoiceExist > 0)
                    Error += "use in other transaction";
            }
            if (Error == "")
            {
                var lst = (from x in __dbContext.TblSeriesMas
                           where x.PkSeriesId == PkSeriesId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblSeriesMas.RemoveRange(lst);

                AddMasterLog((long)Handler.Form.Series, PkSeriesId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.Series, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {
            SeriesModel model = (SeriesModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            if (string.IsNullOrEmpty(error) && model.PKID > 0)
            {
                // error = isAvailableForEdit(model, Mode);
            }
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            SeriesModel model = (SeriesModel)objmodel;
            TblSeriesMas Tbl = new TblSeriesMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblSeriesMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.Series = model.Series;
            //Tbl.FkBranchId = model.FkBranchId;
            Tbl.BillingRate = model.BillingRate;
            Tbl.FormatName = model.FormatName;
            Tbl.ResetNoFor = model.ResetNoFor;
            Tbl.AllowWalkIn = model.AllowWalkIn;
            Tbl.AutoApplyPromo = model.AutoApplyPromo;
            Tbl.RoundOff = model.RoundOff;
            Tbl.DefaultQty = model.DefaultQty;
            Tbl.AllowZeroRate = model.AllowZeroRate;
            Tbl.AllowFreeQty = model.AllowFreeQty;
            Tbl.TaxType = model.TaxType;
            Tbl.DocumentType = string.IsNullOrEmpty(model.DocumentType) ? "B" : model.DocumentType;
            Tbl.FKLocationID = model.FKLocationID == 0 ? 11 : model.FKLocationID;
            Tbl.PaymentMode = model.PaymentMode;

            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {
                Tbl.SeriesNo = model.SeriesNo;
                Tbl.TranAlias = model.TranAlias;
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate; ;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                SeriesModel oldModel = GetSingleRecord(Tbl.PkSeriesId);
                ID = Tbl.PkSeriesId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Series, Tbl.PkSeriesId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Series, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKSeriesID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Series", Fields="Series",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="SeriesNo",    Fields="SeriesNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Billing Rate",    Fields="BillingRate",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Transaction",    Fields="TranAliasName",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },

            };
            return list;
        }


    }
}



















