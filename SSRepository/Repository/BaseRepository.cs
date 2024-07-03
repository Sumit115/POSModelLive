using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SSRepository.Repository
{
    public class Repository<T> : BaseRepository, IRepository<T> where T : class, IEntity
    {


        public Repository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<string> CreateAsync(object tblmas, string Mode, Int64 ID, string dbType = "")
        {
            string ErrorMsg = ValidateData(tblmas, Mode);
            if (ErrorMsg == "")
            {
                SaveBaseData(ref tblmas, Mode, ref ID);
                ErrorMsg = SaveClientData();
                //if (dbType == "client")
                //    ErrorMsg = SaveClientData();
                //else
                //    ErrorMsg = await SaveDataAsync();
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

        protected readonly int __MaxPageSize = 1000;
        protected readonly int __PageSize = 30;


        public BaseRepository(AppDbContext dbContext)
        {
            __dbContext = dbContext;
        }

        protected long __UserId
        {
            get
            {
                return 0;// _contextAccessor.HttpContext.Session.GetString("FkUserId");
            }
        }

        /* public bool IsValidToken(string Token, Int64 UserID)
         {
             string ConnectionString = "";
             ApiToken obj = new ApiToken(); Int64 RGUserID = 0; bool result = true; Int64 ClientUserID = 0; string Remarks = "MSSQL";
             if (!String.IsNullOrEmpty(Token))
             {
                 var Reg = (from c in _ERPcontext.tblClientToken where c.AuthKey == Token select new ApiToken { UserID = c.RGUserID, ConnectionString = c.FkUser.FkclientReg.ConnectionString  = Token, ClientUserID = c.FKUserID, Remarks = c.FkUser.FkclientReg.Remarks }).FirstOrDefault();
                 if (Reg != null)
                 {
                     ConnectionString = Reg.ConnectionString;
                     RGUserID = Reg.UserID;
                     ClientUserID = Reg.ClientUserID;
                     Remarks = Reg.Remarks;
                 }
             }

             if (String.IsNullOrEmpty(ConnectionString) || UserID == 0 || RGUserID != UserID)
             {
                 result = false;
             }
             else
             {
                 result = true;
                 _contextAccessor.HttpContext.Session.SetString("DBType", Remarks);
                 _contextAccessor.HttpContext.Session.SetString("ConnectionString", ConnectionString);
                 _contextAccessor.HttpContext.Session.Set<Int64>("UserID", UserID);
                 _contextAccessor.HttpContext.Session.Set<Int64>("ClientUserID", ClientUserID);
                 objSystemDef = GetSysDefaultJson();
                 objReturnTypes = SetReturnType();
             }

             return result;
         }*/


        //protected SystemDef SetSysDefault()
        //{
        //    SystemDef sDef = new SystemDef();
        //    try
        //    {
        //        if (_contextAccessor.HttpContext.Session.Get<List<SysDefaultModel>>("SysDefault") == null || _contextAccessor.HttpContext.Session.Get<List<SysDefaultModel>>("SysDefault").Count == 0)
        //        {
        //            string sysData = CommonCore.ReadJson("SysDefault" + GetUserID(), GetErrorLogParam(), "data");
        //            if (!String.IsNullOrEmpty(sysData))
        //            {
        //                sDef = JsonConvert.DeserializeObject<SystemDef>(sysData);
        //            }
        //            _contextAccessor.HttpContext.Session.Set("SysDefault", sDef);
        //        }
        //        else
        //        {
        //            sDef = _contextAccessor.HttpContext.Session.Get<SystemDef>("SysDefault");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        string sysData = CommonCore.ReadJson("SysDefault" + GetUserID(), GetErrorLogParam(), "data");
        //        if (!String.IsNullOrEmpty(sysData))
        //        {
        //            sDef = JsonConvert.DeserializeObject<SystemDef>(sysData);
        //        }
        //        _contextAccessor.HttpContext.Session.Set("SysDefault", sDef);
        //    }
        //    return sDef;
        //}



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
                    //WriteLog(ex, "SaveData", "TranRepository", _contextAccessor.HttpContext.Session.Get<Int64>("UserID"));
                    trans.Rollback();
                    ResponseModel response = new ResponseModel();
                    response.ID = 0;
                    response.Response = "Error: " + ex.Message;
                    return JsonConvert.SerializeObject(response);
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
                    //WriteLog(ex, "SaveClientData", "step1", _contextAccessor.HttpContext.Session.Get<Int64>("UserID"));
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

                return __dbContext.Database.GetDbConnection().ConnectionString;
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

            return (from x in __dbContext.TblSysDefaults
                    where x.SysDefKey == SysDefKey
                    select x).FirstOrDefault().SysDefValue;
        }

    }

}



















