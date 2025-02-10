using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Data;
using System.Reflection;

namespace SSRepository.Repository
{
    public class Repository<T> : BaseRepository, IRepository<T> where T : class, IEntity
    {
        public Repository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }
        public async Task<string> CreateAsync(object tblmas, string Mode, Int64 ID, string dbType = "")
        {
            string ErrorMsg = ValidateData(tblmas, Mode);
            if (ErrorMsg == "")
            {
                SaveBaseData(ref tblmas, Mode, ref ID);
                ErrorMsg = SaveClientData();                
            }

            return ErrorMsg;
        }

        public virtual string ValidateData(object tblmas, string Mode)
        { return ""; }

        public virtual void SaveBaseData(ref object tblmas, string Mode, ref Int64 ID)
        { return; }

        public virtual void Delete(long ID)
        {
            __dbContext.Remove(__dbContext.Set<T>().Find(ID));
            SaveDataAsync();
        }

        public IQueryable<T> GetAll(int pageSize, int pageNo = 1)
        {
            return __dbContext.Set<T>().Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        //public IQueryable<T> GetAll(int pageSize, string predicate, int pageNo = 1)
        //{
        //    return __dbContext.Set<T>().OrderBy<T>(predicate).Skip((pageNo - 1) * pageSize).Take(pageSize);
        //}

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            //IQueryable<T> query = _context.Set<T>().Where(predicate);
            //return query;
            IQueryable<T> query = __dbContext.Set<T>().Where(predicate).AsNoTracking();

            return query;
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return __dbContext.Set<T>().Where(predicate);
        }

        public Int64 getIdOfSeriesByEntity(string ColumnName, object FKSeriesID, T entity, string TableName)
        {
            try
            {
                long MaxID = 0;

                //int? BranchNo = 0;
                //var SysDefValue = __dbContext.TblSysDefaults.FirstOrDefault(a => a.SysDefKey == "BranchNo").SysDefValue;


                //if (SysDefValue != null)
                //{
                //    try
                //    {
                //        //BranchNo = _context.TblBranchMas.FirstOrDefault(a => a.PkbranchId == Convert.ToInt64(SysDefValue)).No;
                //        BranchNo = __dbContext.TblBranchMas.Where(a => a.PkBranchId == Convert.ToInt64(SysDefValue)).Select(a => a.No).First();
                //    }
                //    catch
                //    {
                //        BranchNo = 1;
                //    }
                //}

                //Int64 BranchID = 10000000 * Convert.ToInt32(BranchNo);
                //Int64 MaxBranchID = 10000000 * Convert.ToInt32(BranchNo + 1);

                //IQueryable queryableData = __dbContext.Query(entity.GetType());

                //var tbl = Expression.Parameter(entity.GetType(), "tbl");
                //var prop = Expression.Property(tbl, ColumnName);
                //var MinValue = Expression.Constant(BranchID);
                //var MaxValue = Expression.Constant(MaxBranchID);
                //var Greater = Expression.GreaterThanOrEqual(prop, MinValue);
                //var Less = Expression.LessThan(prop, MaxValue);
                //var And = Expression.And(Greater, Less);

                //MethodCallExpression whereCallExpression = Expression.Call(
                //    typeof(Queryable),
                //    "Where",
                //    new Type[] { queryableData.ElementType },
                //    queryableData.Expression,
                //    Expression.Lambda<Func<T, bool>>(And, new ParameterExpression[] { tbl }));
                ////var abc = queryableData.Provider.CreateQuery(whereCallExpression);
                //var lstMaxID = queryableData.Provider.CreateQuery(whereCallExpression).ToDynamicList();//.Aggregate("Max", ColumnName);

                ////long MaxID = 0;
                //if (lstMaxID.Count == 0)
                //{
                //    MaxID = BranchID;
                //}
                //else
                //{
                //    var asd = queryableData.Provider.CreateQuery(whereCallExpression).DefaultIfEmpty();
                //    MaxID = Convert.ToInt64(queryableData.Provider.CreateQuery(whereCallExpression).DefaultIfEmpty().Aggregate("Max", ColumnName));
                //}

                //return Convert.ToInt64(MaxID) + 1;

                //Static bt Table Name 

                if (TableName == "TblAccountMas")
                {
                    MaxID = __dbContext.TblAccountMas.ToList().Count > 0 ? __dbContext.TblAccountMas.ToList().Max(x => x.PkAccountId) : 0;
                }
                else if (TableName == "TblAccountLocLnk")
                {
                    MaxID = __dbContext.TblAccountLocLnk.ToList().Count > 0 ? __dbContext.TblAccountLocLnk.ToList().Max(x => x.PKAccountLocLnkId) : 0;
                }
                else if (TableName == "TblAccountDtl")
                {
                    MaxID = __dbContext.TblAccountDtl.ToList().Count > 0 ? __dbContext.TblAccountDtl.ToList().Max(x => x.PKAccountDtlId) : 0;
                }
                else if (TableName == "TblAccountLicDtl")
                {
                    MaxID = __dbContext.TblAccountLicDtl.ToList().Count > 0 ? __dbContext.TblAccountLicDtl.ToList().Max(x => x.PKAccountLicDtlId) : 0;
                }
                else if (TableName == "TblCategoryMas")
                {
                    MaxID = __dbContext.TblCategoryMas.ToList().Count > 0 ? __dbContext.TblCategoryMas.ToList().Max(x => x.PkCategoryId) : 0;
                }
                else if (TableName == "TblPromotionMas")
                {
                    MaxID = __dbContext.TblPromotionMas.ToList().Count > 0 ? __dbContext.TblPromotionMas.ToList().Max(x => x.PkPromotionId) : 0;
                }
                else if (TableName == "TblRecipeMas")
                {
                    MaxID = __dbContext.TblRecipeMas.ToList().Count > 0 ? __dbContext.TblRecipeMas.ToList().Max(x => x.PkRecipeId) : 0;
                }
                else if (TableName == "TblRoleMas")
                {
                    MaxID = __dbContext.TblRoleMas.ToList().Count > 0 ? __dbContext.TblRoleMas.ToList().Max(x => x.PkRoleId) : 0;
                }

                return Convert.ToInt64(MaxID) + 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


    public class BaseRepository //: IBaseRepository
    {
        protected readonly AppDbContext __dbContext;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly int __MaxPageSize = 1000;
        protected readonly int __PageSize = 30;


        public BaseRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            __dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }

        public long GetUserID()
        {
            return Convert.ToInt64(_contextAccessor.HttpContext.Session.GetString("UserID"));
        }

        public long GetRoleID()
        {
            return Convert.ToInt64(_contextAccessor.HttpContext.Session.GetString("RoleId"));
        }
        public long IsAdmin()
        {
            return Convert.ToInt64(_contextAccessor.HttpContext.Session.GetString("IsAdmin"));
        }



        #region Add/Update/Delete Data
        public virtual void AddData(object entity, bool IsRange)
        {
            if (IsRange == true)
            {
                var typ = entity.GetType();
                System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typ);
                list = (System.Collections.IList)entity;
                for (int i = 0; i < list.Count; i++)
                {
                    __dbContext.Add(list[i]);
                }
            }
            else
                __dbContext.Add(entity);
        }

        public virtual void DeleteData(object entity, bool IsRange)
        {
            if (IsRange == true)
            {
                var typ = entity.GetType();
                System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typ);
                list = (System.Collections.IList)entity;
                for (int i = 0; i < list.Count; i++)
                {
                    __dbContext.Remove(list[i]);
                }
            }
            else
                __dbContext.Remove(entity);
        }

        public virtual void UpdateData(object entity, bool IsRange)
        {
            if (IsRange == true)
            {
                var typ = entity.GetType();

                System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typ);
                list = (System.Collections.IList)entity;
                for (int i = 0; i < list.Count; i++)
                {
                    __dbContext.Update(list[i]);
                }
            }
            else
                __dbContext.Update(entity);
        }

        public async Task<string> SaveDataAsync()
        {
            using (var trans = __dbContext.Database.BeginTransaction())
            {
                try
                {
                    await __dbContext.SaveChangesAsync();
                    trans.Commit();
                    return "";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public virtual void AddClientData(object entity, bool IsRange)
        {
            //Save on Localsys
            if (IsRange == true)
            {
                var typ = entity.GetType();
                System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typ);
                list = (System.Collections.IList)entity;
                for (int i = 0; i < list.Count; i++)
                {
                    __dbContext.Add(list[i]);
                }
            }
            else
                __dbContext.Add(entity);
        }

        public virtual void DeleteClientData(object entity, bool IsRange)
        {
            //Delete on Localsys
            if (IsRange == true)
            {
                var typ = entity.GetType();
                System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typ);
                list = (System.Collections.IList)entity;
                for (int i = 0; i < list.Count; i++)
                {
                    __dbContext.Remove(list[i]);
                }
            }
            else
                __dbContext.Remove(entity);
        }

        public virtual void UpdateClientData(object entity, bool IsRange)
        {
            //Delete on Localsys
            if (IsRange == true)
            {
                var typ = entity.GetType();
                System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typ);
                list = (System.Collections.IList)entity;
                for (int i = 0; i < list.Count; i++)
                {
                    __dbContext.Update(list[i]);
                }
            }
            else
                __dbContext.Update(entity);
        }

        public string SaveClientData()
        {
            using (var trans = __dbContext.Database.BeginTransaction())
            {
                try
                {
                    __dbContext.SaveChanges();
                    trans.Commit();
                    return "";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return ex.Message;
                }
            }
        }
        #endregion

        public void DetachEntity<TEntity>(TEntity entityToDetach) where TEntity : class
        {
            var entry = __dbContext.Entry(entityToDetach);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Detached;
            }
        }

        #region API_Method_Begin            

        public void WriteLog(Exception ex, string Method, string Controller, long UserID = 0)
        {
            try
            {
                // Common.ErrorHandling(ex, Method, Controller, GetErrorPath(Common.RootPath, UserID), "", UserID);
            }
            catch (Exception ex1)
            { }
        }

        public string GetErrorPath(string path, long userid)
        {
            try
            {
                //path += "\\log\\" + GetCompanyName() + "\\" + GetUserName(userid);
            }
            catch { }
            return path;
        }

        //public string GetCompanyName()
        //{
        //    string name = "";
        //    try
        //    {
        //        var lst = (from d in __dbContext.TblSysDefaults
        //                   where (d.SysDefKey == "CompanyName")
        //                   select d).FirstOrDefault();
        //        if (lst != null)
        //            name = lst.SysDefValue.Trim();
        //        else
        //            name = "Common";
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw;
        //    }
        //    return name;
        //}

        //public string GetUserName(long userid)
        //{
        //    string name = "";
        //    var lst = (from d in _context.TblEmpUserMas
        //               where (d.PkuserId == userid)
        //               select d.UserName).FirstOrDefault();
        //    if (lst != null)
        //    {
        //        name = lst;
        //    }
        //    return name;
        //}


        public DataTable GetFilterData(string strFilter)
        {

            DataTable dtFilter = new DataTable();
            dtFilter.Columns.Add("PKID");
            dtFilter.AcceptChanges();
            if (strFilter != null && strFilter != "[]")
            {

                dtFilter = JsonConvert.DeserializeObject<DataTable>(strFilter);

            }
            return dtFilter;
        }

        public DataTable GetFilterDataString(string strFilter)
        {

            DataTable dtFilter = new DataTable();
            dtFilter.Columns.Add("Text");
            dtFilter.AcceptChanges();
            if (strFilter != null && strFilter != "[]")
            {

                dtFilter = JsonConvert.DeserializeObject<DataTable>(strFilter);

            }
            return dtFilter;
        }



        #endregion API_Method_End

        public static DataTable GetDataTableFromObjects(object o)
        {
            Type t = o.GetType();
            DataTable dt = new DataTable(t.Name);
            foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
            {
                //dt.Columns.Add(new DataColumn(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType));
                var column = new DataColumn
                {
                    ColumnName = pi.Name,
                    DataType = pi.PropertyType.Name.Contains("Nullable") ? typeof(string) : pi.PropertyType
                };

                dt.Columns.Add(column);
            }

            DataRow dr = dt.NewRow();
            foreach (DataColumn dc in dt.Columns)
            {
                dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);
            }
            dt.Rows.Add(dr);
            return dt;
        }

        public DataTable ObjectToDataTable(object o)
        {
            DataTable dt = new DataTable("OutputData");
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            o.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(o, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(o, null);
                }
                catch { }
            });
            return dt;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
       
        //Sql Conn
        #region sql Connection
        public string conn
        {
            get
            {

                return __dbContext.Database.GetConnectionString() ?? "";
            }
        }
        public DataTable ExecDataTable(string cmdText)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            return dt;
        }

        public int ExecNonQuery(string cmdText)
        {
            SqlConnection Con = new SqlConnection(conn);
            Con.Open();
            SqlCommand cmd = new SqlCommand(cmdText, Con);
            int nUpdatedRows = cmd.ExecuteNonQuery();
            cmd.Dispose();
            if (Con.State == ConnectionState.Open)
                Con.Close();
            Con.Dispose();
            return nUpdatedRows;
        }

        public string ExecScalarStr(string cmdText)
        {
            string Result = string.Empty;
            SqlConnection Con = new SqlConnection(conn);
            Con.Open();
            SqlCommand cmd = new SqlCommand(cmdText, Con);
            object obj = cmd.ExecuteScalar();
            cmd.Dispose();
            if (Con.State == ConnectionState.Open)
                Con.Close();

            Con.Dispose();
            return ToStr(obj);
        }
        public int ExecScalarInt(string cmdText)
        {
            return ToInt(ExecScalarStr(cmdText));
        }

        public decimal ExecScalarDec(string cmdText)
        {
            return Convert.ToDecimal(ExecScalarStr(cmdText));
        }

        public int ToInt(object Val)
        {
            int Result = 0;
            try
            {
                Result = Convert.ToInt32(Convert.ToDecimal(Val));
            }
            catch (Exception ex)
            {
                Result = 0;
            }
            return Result;
        }

        public string ToStr(object Val)
        {
            string Result = string.Empty;
            try
            {
                Result = Val.ToString();
            }
            catch (Exception ex)
            {
                Result = string.Empty;
            }
            return Result;
        }

        #endregion




        public string GetSysDefaultsByKey(string SysDefKey)
        {

            var _entity = (from x in __dbContext.TblSysDefaults
                           where x.SysDefKey == SysDefKey
                           select x).FirstOrDefault();

            return _entity != null ? _entity.SysDefValue : "";
        }

        public List<SysDefaultsModel> GetSysDefaultsList(string search = "")
        {
            if (search != null) search = search.ToLower();
            List<SysDefaultsModel> data = (from cou in __dbContext.TblSysDefaults
                                           where (EF.Functions.Like(cou.SysDefKey.Trim().ToLower(), Convert.ToString(search) + "%"))
                                           select (new SysDefaultsModel
                                           {
                                               PKSysDefID = cou.PKSysDefID,
                                               SysDefKey = cou.SysDefKey,
                                               SysDefValue = cou.SysDefValue,
                                               FKTableName = cou.FKTableName,
                                               FKColumnName = cou.FKColumnName,
                                               FKUserID = cou.FKUserID,
                                               DATE_MODIFIED = cou.DATE_MODIFIED,
                                           }
                                        )).ToList();
            return data;
        }
        public SysDefaults GetSysDefaults()
        {

            SysDefaults model = new SysDefaults();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetSysDefaults", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PkId", PkId);
                //cmd.Parameters.AddWithValue("@FkSeriesId", FkSeriesId);
                //cmd.Parameters.Add(new SqlParameter("@JsonData", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "JsonData", DataRowVersion.Default, null));
                //cmd.Parameters.Add(new SqlParameter("@ErrMsg", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "ErrMsg", DataRowVersion.Default, null));
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
                con.Close();
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    foreach (DataColumn column in dr.Table.Columns)
                    {
                        foreach (PropertyInfo pro in typeof(SysDefaults).GetProperties())
                        {
                            if (pro.Name == column.ColumnName)
                                pro.SetValue(model, dr[column.ColumnName], null);
                            else
                                continue;
                        }
                    }
                }
            }
            return model;
        }

        public void UpdateSysDefaults(object objmodel)
        {
            List<SysDefaultsModel> model = (List<SysDefaultsModel>)objmodel;
            try
            {
                foreach (var item in model)
                {
                    var _entity = __dbContext.TblSysDefaults.Where(x => x.SysDefKey == item.SysDefKey).FirstOrDefault();
                    if (_entity != null)
                    {
                        TblSysDefaults Tbl = new TblSysDefaults();
                        Tbl = _entity;
                        Tbl.SysDefValue = item.SysDefValue;
                        UpdateData(Tbl, false);

                    }
                }
                __dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }

        }
        public void InsertUpdateSysDefaults(object objmodel)
        {
            List<SysDefaultsModel> model = (List<SysDefaultsModel>)objmodel;
            try
            {
                foreach (var item in model.ToList().Where(x=>!string.IsNullOrEmpty(x.SysDefValue)))
                {

                    var _entity = __dbContext.TblSysDefaults.Where(x => x.SysDefKey == item.SysDefKey).FirstOrDefault();
                    if (_entity != null)
                    {
                        TblSysDefaults Tbl = new TblSysDefaults();
                        Tbl = _entity;
                        Tbl.SysDefValue = item.SysDefValue;
                        UpdateData(Tbl, false);
                        __dbContext.SaveChanges();
                    }
                    else
                    {

                        TblSysDefaults Tbl = new TblSysDefaults();
                        var data = __dbContext.TblSysDefaults.OrderByDescending(u => u.PKSysDefID).FirstOrDefault();
                        if (data != null)
                        {
                            Tbl.PKSysDefID = data.PKSysDefID + 1;
                        }
                        else
                        {
                            Tbl.PKSysDefID = 1;
                        }
                        Tbl.SysDefKey = item.SysDefKey;
                        Tbl.SysDefValue = item.SysDefValue;
                        Tbl.FKUserID = 1;
                        Tbl.DATE_MODIFIED = DateTime.Now;
                        AddData(Tbl, false);
                        __dbContext.SaveChanges();

                    }
                }
  
            }
            catch (Exception ex)
            {

            }

        }

        public void InsertUpdateSysDefaults(string SysDefKey, string SysDefValue)
        {
            try
            {
                var _entity = __dbContext.TblSysDefaults.Where(x => x.SysDefKey == SysDefKey).FirstOrDefault();
                if (_entity != null)
                {
                    TblSysDefaults Tbl = new TblSysDefaults();
                    Tbl = _entity;
                    Tbl.SysDefValue = SysDefValue;
                    UpdateData(Tbl, false);
                }
                else
                {


                    TblSysDefaults Tbl = new TblSysDefaults();
                    var data = __dbContext.TblSysDefaults.OrderByDescending(u => u.PKSysDefID).FirstOrDefault();
                    if (data != null)
                    {
                        Tbl.PKSysDefID = data.PKSysDefID + 1;
                    }
                    else
                    {
                        Tbl.PKSysDefID = 1;
                    }
                    Tbl.SysDefKey = SysDefKey;
                    Tbl.SysDefValue = SysDefValue;
                    Tbl.FKUserID = 1;
                    Tbl.DATE_MODIFIED = DateTime.Now;
                    AddData(Tbl, false);

                }

                __dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }

        }

        public List<BarcodePrintPreviewModel> BarcodePrintList(List<BarcodeDetails> model)
        {
            var _lst = new List<BarcodePrintPreviewModel>();

            foreach (var item in model)
            {
                if (item.TranInId > 0)
                {
                    var _d = (from cou in __dbContext.TblProductQTYBarcode
                              join prdLot in __dbContext.TblProdLotDtl on cou.FkLotID equals prdLot.PkLotId
                              join prd in __dbContext.TblProductMas on prdLot.FKProductId equals prd.PkProductId
                              join loc in __dbContext.TblLocationMas on item.FkLocationId equals loc.PkLocationID
                              join branch in __dbContext.TblBranchMas on loc.FkBranchID equals branch.PkBranchId
                              join city in __dbContext.TblCityMas on branch.FkCityId equals city.PkCityId
                              where cou.TranInId == item.TranInId && cou.TranInSeriesId == item.TranInSeriesId
                              && cou.TranInSrNo == item.TranInSrNo && prdLot.FKProductId == item.FKProductId
                              select new BarcodePrintPreviewModel()
                              {
                                  Barcode = cou.Barcode,
                                  MRP = prdLot.MRP,
                                  SaleRate = prdLot.SaleRate,
                                  Batch = prdLot.Batch,
                                  Product = prd.Product,
                                  StockDate = prdLot.StockDate,
                                  BranchName = branch.BranchName,
                                  Address = branch.Address,
                                  CityName = city.CityName,
                                  Pin = branch.Pin,
                                  IsPrint = cou.IsPrint > 0 ? false : true,
                                  PrintMode = 1,
                              }).ToList();
                    _lst.AddRange(_d);
                }
                else if (item.TranOutId > 0)
                {
                    var _d = (from cou in __dbContext.TblProductQTYBarcode
                              join prdLot in __dbContext.TblProdLotDtl on cou.FkLotID equals prdLot.PkLotId
                              join prd in __dbContext.TblProductMas on prdLot.FKProductId equals prd.PkProductId
                              join loc in __dbContext.TblLocationMas on item.FkLocationId equals loc.PkLocationID
                              join branch in __dbContext.TblBranchMas on loc.FkBranchID equals branch.PkBranchId
                              join city in __dbContext.TblCityMas on branch.FkCityId equals city.PkCityId
                              where cou.TranOutId == item.TranOutId && cou.TranOutSeriesId == item.TranOutSeriesId
                               && cou.TranOutSrNo == item.TranOutSrNo && prdLot.FKProductId == item.FKProductId
                              select new BarcodePrintPreviewModel()
                              {
                                  Barcode = cou.Barcode,
                                  MRP = prdLot.MRP,
                                  SaleRate = prdLot.SaleRate,
                                  Batch = prdLot.Batch,
                                  Product = prd.Product,
                                  StockDate = prdLot.StockDate,
                                  BranchName = branch.BranchName,
                                  Address = branch.Address,
                                  CityName = city.CityName,
                                  Pin = branch.Pin,
                                  IsPrint = cou.IsPrint > 1 ? false : true,
                                  PrintMode = 2,
                              }).ToList();
                    _lst.AddRange(_d);
                }

            }

            return _lst;
        }
        public void UpdatePrintBarcode(object objmodel)
        {
            List<BarcodePrintPreviewModel> model = (List<BarcodePrintPreviewModel>)objmodel;
            try
            {
                foreach (var item in model)
                {
                    var _entity = __dbContext.TblProductQTYBarcode.Where(x => x.Barcode == item.Barcode).FirstOrDefault();
                    if (_entity != null)
                    {
                        TblProductQTYBarcode Tbl = new TblProductQTYBarcode();
                        Tbl = _entity;
                        Tbl.IsPrint = item.PrintMode;
                        UpdateData(Tbl, false);

                    }
                }
                __dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }

        }



        public void ExecuteQuery(string Qry, ref string ErrMsg)
        {


            using (SqlConnection con = new SqlConnection(conn))
            {
                //con.Open();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("usp_ExecuteQuery", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Qry", Qry);
                cmd.Parameters.Add(new SqlParameter("@ErrMsg", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Output, false, 0, 10, "ErrMsg", DataRowVersion.Default, null));
                cmd.ExecuteNonQuery();
                ErrMsg = Convert.ToString(cmd.Parameters["@ErrMsg"].Value);
                con.Close();
            }
        }
        public List<FormModel> GetFormList(long? FKMasterFormID = null)
        {
            List<FormModel> data = (from cou in __dbContext.TblFormMas
                                    where cou.IsActive == true &&
                                    cou.FKMasterFormID == (FKMasterFormID > 0 ? FKMasterFormID : cou.FKMasterFormID)
                                    select (new FormModel
                                    {
                                        PKID = cou.PKFormID,
                                        FKMasterFormID = cou.FKMasterFormID,
                                        SeqNo = cou.SeqNo,
                                        FormName = cou.FormName,
                                        ShortName = cou.ShortName,
                                        ShortCut = cou.ShortCut,
                                        ToolTip = cou.ToolTip,
                                        Image = cou.Image,
                                        FormType = cou.FormType,
                                        WebURL = cou.WebURL,
                                        IsActive = cou.IsActive,
                                    }
                                        )).ToList();
            return data;
        }

        public DashboardSummaryModel usp_DashboardSummary()
        {
            DashboardSummaryModel model = new DashboardSummaryModel();
            DataSet ds = new DataSet();
            string data = "";
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_DashboardSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PkUserId", UserId);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                data = Convert.ToString(ds.Tables[0].Rows[0]["JsonData"]);
                con.Close();
            }
            if (data != null)
            {
                List<DashboardSummaryModel> aa = JsonConvert.DeserializeObject<List<DashboardSummaryModel>>(data);
                if (aa != null)
                {
                    model = aa[0];
                }
            }
            return model;
        }

     
    }

}



















