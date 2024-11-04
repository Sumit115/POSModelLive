
using LMS.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace LMS.Repository
{
    public class BaseRepository 
    {
        protected readonly ssodbContext __dbContext;

        public BaseRepository(ssodbContext dbContext)
        {
            __dbContext = dbContext;
        }


        public void DetachEntity<TEntity>(TEntity entityToDetach) where TEntity : class
        {
            var entry = __dbContext.Entry(entityToDetach);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Detached;
            }
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


    }

}



















