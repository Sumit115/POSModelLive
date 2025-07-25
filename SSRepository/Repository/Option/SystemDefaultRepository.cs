using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SSRepository.Data;
using SSRepository.IRepository.Options;
using SSRepository.Models;
using System.Text;

namespace SSRepository.Repository.Options
{
    public class SystemDefaultRepository : ISystemDefaultRepository
    {

        protected readonly AppDbContext __dbContext;

        protected readonly IHttpContextAccessor _contextAccessor;
        public SystemDefaultRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            __dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }

        public async Task<SysDefaults> GetSingleRecord()
        {
            var defaults = await __dbContext.TblSysDefaults.ToListAsync();

            var model = new SysDefaults();

            foreach (var def in defaults)
            {
                var prop = typeof(SysDefaults).GetProperty(def.SysDefKey);
                if (prop != null && prop.CanWrite)
                {
                    try
                    {
                        object? value = Convert.ChangeType(def.SysDefValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        prop.SetValue(model, value);
                    }
                    catch
                    {
                        // Optional: log or skip invalid conversions
                    }
                }
            }

            return model;
        }


        public string GetSingleKey(string Key)
        {
            string value = "";
            var obj = __dbContext.TblSysDefaults.Where(X => X.SysDefKey == Key).SingleOrDefault();
            if (obj != null)
                value = obj.SysDefValue;
            return value;
        }

        public async Task SaveBaseAsync(SysDefaults model)
        {
            var props = typeof(SysDefaults).GetProperties()// Skip file uploads
                        .ToList();

            foreach (var prop in props)
            {
                string key = prop.Name;
                object? value = prop.GetValue(model);
                string valueStr = value?.ToString() ?? "";

                var entity = await __dbContext.TblSysDefaults.FirstOrDefaultAsync(x => x.SysDefKey == key);

                if (entity != null)
                {
                    if (entity.SysDefValue != valueStr)
                    {
                        entity.SysDefValue = valueStr?? "";
                        entity.DATE_MODIFIED = DateTime.Now;
                        entity.FKUserID = Convert.ToInt64(_contextAccessor.HttpContext.Session.GetString("UserID"));
                        __dbContext.Update(entity);
                    }
                }
                else
                {
                    entity = new TblSysDefaults();
                    entity.PKSysDefID = 0;
                    entity.SysDefKey = key;
                    entity.SysDefValue = valueStr ?? "";
                    entity.FKUserID = Convert.ToInt64(_contextAccessor.HttpContext.Session.GetString("UserID"));
                    entity.DATE_MODIFIED = DateTime.Now;
                    __dbContext.Add(entity);
                }
            }

            await __dbContext.SaveChangesAsync();
        }


    }
}
