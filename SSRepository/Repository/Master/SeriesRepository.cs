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
            if (model.PkSeriesId > 0)
            {
                if (model.TranAlias == "SORD") { error = __dbContext.TblSalesOrdertrn.Where(x => x.FKSeriesId == model.PkSeriesId).ToList().Count > 0 ? "Update Not Available" : ""; }
                if (model.TranAlias == "SINV") { error = __dbContext.TblSalesInvoicetrn.Where(x => x.FKSeriesId == model.PkSeriesId).ToList().Count > 0 ? "Update Not Available" : ""; }
                if (model.TranAlias == "SCHN") { error = __dbContext.TblSalesChallantrn.Where(x => x.FKSeriesId == model.PkSeriesId).ToList().Count > 0 ? "Update Not Available" : ""; }
                if (model.TranAlias == "PORD") { error = __dbContext.TblPurchaseOrdertrn.Where(x => x.FKSeriesId == model.PkSeriesId).ToList().Count > 0 ? "Update Not Available" : ""; }
                if (model.TranAlias == "PINV") { error = __dbContext.TblPurchaseInvoicetrn.Where(x => x.FKSeriesId == model.PkSeriesId).ToList().Count > 0 ? "Update Not Available" : ""; }
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
                            //join _tranAlias in GetDrpTranAlias().ToList() on cou.TranAlias equals _tranAlias.Value
                        where EF.Functions.Like(cou.Series.Trim().ToLower(), search + "%")
                        && (TranAlias == "" || cou.TranAlias == TranAlias)
                        && (DocumentType == "" || cou.DocumentType == DocumentType)
                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                        orderby cou.PkSeriesId
                        select (new SeriesModel
                        {
                            PkSeriesId = cou.PkSeriesId,
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
                            TaxType = cou.TaxType,
                            FKUserID = cou.FKUserID,
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
            if (EnCustomFlag == 1)//(int)Handler.en_CustomFlag.CustomDrop)
            {
                var key = SysDefaults_byLogin();

                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblSeriesMas
                             //join _tranAlias in GetDrpTranAlias().ToList() on cou.TranAlias equals _tranAlias.Value
                         where EF.Functions.Like(cou.Series.Trim().ToLower(), search + "%")
                         && (TranAlias == "" || cou.TranAlias == TranAlias)
                         && (DocumentType == "" || cou.DocumentType == DocumentType)
                         orderby cou.PkSeriesId
                         select (new
                         {
                             cou.PkSeriesId,
                             cou.Series,
                             cou.SeriesNo,
                             cou.BillingRate,
                             cou.TranAlias,

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
                    join location in __dbContext.TblLocationMas on cou.FKLocationID equals location.PkLocationID
                    join branch in __dbContext.TblBranchMas on location.FkBranchID equals branch.PkBranchId
                    where cou.PkSeriesId == PkSeriesId
                    select (new SeriesModel
                    {
                        PkSeriesId = cou.PkSeriesId,
                        Series = cou.Series,
                        SeriesNo = cou.SeriesNo,
                        // FkBranchId = cou.FkBranchId,
                        BillingRate = cou.BillingRate,
                        TranAlias = cou.TranAlias,
                        FormatName = cou.FormatName,
                        ResetNoFor = cou.ResetNoFor,
                        AllowWalkIn = cou.AllowWalkIn,
                        AutoApplyPromo = cou.AutoApplyPromo,
                        RoundOff = cou.RoundOff,
                        DefaultQty = cou.DefaultQty,
                        AllowZeroRate = cou.AllowZeroRate,
                        AllowFreeQty = cou.AllowFreeQty,
                        BranchName = branch.BranchName,
                        FKLocationID = cou.FKLocationID,
                        Location = location.Location,
                        TaxType = cou.TaxType,
                        DocumentType = cou.DocumentType,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkSeriesId)
        {
            string Error = "";
            SeriesModel obj = GetSingleRecord(PkSeriesId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkSeriesId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblSeriesMas
                           where x.PkSeriesId == PkSeriesId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblSeriesMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkSeriesId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkSeriesId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetSeriesID(), PkSeriesId, obj.FKSeriesID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {
            SeriesModel model = (SeriesModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            if (string.IsNullOrEmpty(error) && model.PkSeriesId > 0)
            {
                // error = isAvailableForEdit(model, Mode);
            }
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            SeriesModel model = (SeriesModel)objmodel;
            TblSeriesMas Tbl = new TblSeriesMas();
            if (model.PkSeriesId > 0)
            {
                var _entity = __dbContext.TblSeriesMas.Find(model.PkSeriesId);
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
            var list = new List<ColumnStructure>
            {
                    new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Series", Fields="Series",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Series No ", Fields="SeriesNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Billing Rate", Fields="BillingRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Transaction", Fields="TranAliasName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Format Name", Fields="FormatName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Reset No For", Fields="ResetNoFor",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Allow WalkIn", Fields="AllowWalkIn",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="AutoApply Promo", Fields="AutoApplyPromo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Round Off", Fields="RoundOff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Default Qty", Fields="DefaultQty",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Allow Zero Rate", Fields="AllowZeroRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    //new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Allow Free Qty", Fields="AllowFreeQty",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}



















