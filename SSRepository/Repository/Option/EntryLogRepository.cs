using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Option;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Repository.Option
{
    public class EntryLogRepository : BaseRepository, IEntryLogRepository
    {
        public EntryLogRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {

        }

        public object GetList(DateTime FromDate, DateTime ToDate)
        {

           var data = (from cou in __dbContext.TblMasterLogDtl
                                            join user in __dbContext.TblUserMas on cou.FKUserId equals user.PkUserId
                                            join lastUser in __dbContext.TblUserMas on cou.FKLastUserId equals lastUser.PkUserId
                                            join form in __dbContext.TblFormMas on cou.FKFormID equals form.PKFormID
                                            where cou.ModifyDate.Value.Date >= FromDate.Date && cou.ModifyDate.Value.Date <= ToDate.Date
                                            orderby cou.ModifyDate
                       select (new 
                                            {
                                                PKMasterLogID = cou.PKMasterLogID,
                                                FKFormID = cou.FKFormID,
                                                FKID = cou.FKID,
                                                FKSeriseId = cou.FKSeriseId,
                                                DATE_ENTRY = cou.EntryDate.ToString("dd-MMM-yyyy"),
                                                TIME_ENTRY = cou.EntryDate.ToString("HH:mm"),
                                                IsDelete = cou.IsDelete,
                                                Status = cou.IsDelete ? "Delete" : "Update",
                                                // JsonDetail = cou.JsonDetail,
                                                Description = cou.Description,
                                                FKUserId = cou.FKUserId,
                                                UserName = user.UserId,
                                                DATE_MODIFIED = cou.ModifyDate.Value.ToString("dd-MMM-yyyy"),
                                                TIME_MODIFIED = cou.ModifyDate.Value.ToString("HH:mm"),
                                                FKLastUserId = cou.FKLastUserId,
                                                LastUserName = user.UserId,
                                                DATE_LASTMODIFIED = cou.LastModifyDate.Value.ToString("dd-MMM-yyyy"),
                                                TIME_LASTMODIFIED = cou.LastModifyDate.Value.ToString("HH:mm"),
                                                WebUrl = form.WebURL,
                                            }
                                           )).ToList();
            return data;
        }
    
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Master", Fields="Description",Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Entry Date", Fields="DATE_ENTRY",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Entry Time", Fields="TIME_ENTRY",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Status", Fields="Status",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Last User Name", Fields="LastUserName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Last Date Modified", Fields="DATE_LASTMODIFIED",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Last Time", Fields="TIME_LASTMODIFIED",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="User Name", Fields="UserName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Date Modified", Fields="DATE_MODIFIED",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Time", Fields="TIME_MODIFIED",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                      };
            return list;
        }

    }
}
