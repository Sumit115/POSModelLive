﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Azure;

namespace SSRepository.Repository.Master
{
    public class CityRepository : Repository<TblCityMas>, ICityRepository
    {
        public CityRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor   )
        {
        }

        public string isAlreadyExist(CityModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.CityName))
            {
                cnt = (from x in __dbContext.TblCityMas
                       where x.CityName == model.CityName && x.PkCityId != model.PkCityId
                       && x.StateName == model.StateName
                       select x).Count();
                if (cnt > 0)
                    error = "City Name Exits";
            }

            return error;
        }

        public List<CityModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<CityModel> data = (from cou in __dbContext.TblCityMas

                                        // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby cou.PkCityId
                                    select (new CityModel
                                    {
                                        PkCityId = cou.PkCityId,
                                        CityName = cou.CityName,
                                        StateName = cou.StateName,
                                        FKUserID = cou.FKUserID,
                                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                    }
                                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public CityModel GetSingleRecord(long PkCityId)
        {

            CityModel data = new CityModel();
            data = (from cou in __dbContext.TblCityMas
                    where cou.PkCityId == PkCityId
                    select (new CityModel
                    {
                        PkCityId = cou.PkCityId,
                        CityName = cou.CityName,
                        StateName = cou.StateName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")

                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpCity(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkCityId,
                        r.CityName
                    }).ToList(); ;
        }
        public object GetDrpCity_ByState(string StateName)
        {
            return (from cou in __dbContext.TblCityMas
                    where cou.StateName == StateName
                    // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                    orderby cou.PkCityId
                    select new
                    {
                        cou.PkCityId,
                        cou.CityName
                    }).ToList();
        }

        public string DeleteRecord(long PkCityId)
        {
            string Error = "";
            CityModel obj = GetSingleRecord(PkCityId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkCityId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblCityMas
                           where x.PkCityId == PkCityId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblCityMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkCityId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkCityId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetCityID(), PkCityId, obj.FKCityID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            CityModel model = (CityModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CityModel model = (CityModel)objmodel;
            TblCityMas Tbl = new TblCityMas();
            if (model.PkCityId > 0)
            {
                var _entity = __dbContext.TblCityMas.Find(model.PkCityId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkCityId = model.PkCityId;
            Tbl.CityName = model.CityName;
            Tbl.StateName = model.StateName;

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

                CityModel oldModel = GetSingleRecord(Tbl.PkCityId);
                ID = Tbl.PkCityId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.City, Tbl.PkCityId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.CityName, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKCityID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                 // new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Date", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="State Name", Fields="StateName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="City Name", Fields="CityName",Width=50,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                      };
            return list;
        }


    }
}



















