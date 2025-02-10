using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Net;
using System.Reflection;

namespace SSRepository.Repository.Master
{
    public class CompanyRepository : Repository<TblCompany>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }

        public CompanyModel GetSingleRecord()
        {

            CompanyModel data = new CompanyModel();
            data = (from cou in __dbContext.TblCompanies
                    select (new CompanyModel
                    {
                        PkCompanyId = cou.PkCompanyId,
                        FKUserId = cou.FKUserID,
                        FKCreatedByID = cou.FKCreatedByID,
                        ModifiDate = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                        CreateDate = cou.CreationDate.ToString("dd-MMM-yyyy"),
                        CompanyName = cou.CompanyName,
                        ContactPerson = cou.ContactPerson,
                        Email = cou.Email,
                        Mobile = cou.Mobile,
                        Address = cou.Address,
                        City = cou.City,
                        State = cou.State,
                        Pin = cou.Pin,
                        Country = cou.Country,
                        Gstn = cou.Gstn,
                        LogoImg = cou.LogoImg,
                        ThumbnailImg = cou.ThumbnailImg
                    })).FirstOrDefault();

            return data;
        }

        public override string ValidateData(object objmodel, string Mode)
        {

            CompanyModel model = (CompanyModel)objmodel;
            string error = "";
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            CompanyModel model = (CompanyModel)objmodel;
            TblCompany Tbl = new TblCompany();
            if (model.PkCompanyId > 0)
            {
                var _entity = __dbContext.TblCompanies.Find(model.PkCompanyId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.CompanyName = model.CompanyName;
            Tbl.ContactPerson = model.ContactPerson;
            Tbl.Email = model.Email;
            Tbl.Mobile = model.Mobile;
            Tbl.Address = model.Address;
            Tbl.City = model.City;
            Tbl.State = model.State;
            Tbl.Pin = model.Pin;
            Tbl.Country = model.Country;
            Tbl.Gstn = model.Gstn;
            Tbl.LogoImg = model.LogoImg;
            Tbl.ThumbnailImg = model.ThumbnailImg;
            Tbl.FKUserID = model.FKUserId;
            Tbl.ModifiedDate = DateTime.Now;
            if (Mode == "Create")
            {                
                Tbl.FKCreatedByID = model.FKUserId;
                Tbl.CreationDate = DateTime.Now;
                AddData(Tbl, false);
            }
            else
            {

                CompanyModel oldModel = GetSingleRecord();
               
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKProductID, oldModel.PkProductId, oldModel.FKProductID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKProductID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }

    }
}



















