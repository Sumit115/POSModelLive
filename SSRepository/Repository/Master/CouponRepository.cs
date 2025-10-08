using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSRepository.Repository.Master
{
    public class CouponRepository : Repository<TblCouponMas>, ICouponRepository
    {
        public CouponRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
            : base(dbContext, contextAccessor)
        {
        }

        public string isAlreadyExist(CouponModel model, string Mode)
        {
            string error = "";

             

            return error;
        }

        public List<CouponModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (!string.IsNullOrEmpty(search)) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;

            var data = (from cou in __dbContext.TblCouponMas
                        where string.IsNullOrEmpty(search) || EF.Functions.Like(cou.PkCouponId.ToString(), search + "%")
                        orderby cou.PkCouponId
                        select new CouponModel
                        {
                            PKID = cou.PkCouponId,
                            Amount = cou.Amount,
                            NoOfCoupon = cou.NoOfCoupon,
                            FKUserID = cou.FKUserID,
                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                            UserName = cou.FKUser.UserId,

                            //CouponCodes = (from cc in __dbContext.TblCouponCodeLnk
                            //               where cc.FkCouponId == c.PkCouponId
                            //               select new CouponCodeLnkModel
                            //               {
                            //                   PkId = cc.PkId,
                            //                   CouponCode = cc.CouponCode,
                            //                   FkCouponId = cc.FkCouponId,
                            //                   FKUserID = cc.FKUserID
                            //               }).ToList()
                        }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return data;
        }

        public CouponModel GetSingleRecord(long PkCouponId)
        {
            var data = (from cou in __dbContext.TblCouponMas
                        where cou.PkCouponId == PkCouponId
                        select new CouponModel
                        {
                            PKID = cou.PkCouponId,
                            Amount = cou.Amount,
                            NoOfCoupon = cou.NoOfCoupon,
                            FKUserID = cou.FKUserID,
                            DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy"),
                            UserName = cou.FKUser.UserId,
                            CouponCodes = (from cc in __dbContext.TblCouponCodeLnk
                                           where cc.FkCouponId == cou.PkCouponId
                                           select new CouponCodeLnkModel
                                           {
                                               PkId = cc.PkId,
                                               CouponCode = cc.CouponCode,
                                               FkCouponId = cc.FkCouponId,
                                               FKUserID = cc.FKUserID
                                           }).ToList()
                        }).FirstOrDefault();

            return data;
        }

        public override string ValidateData(object objmodel, string Mode)
        {
            var model = (CouponModel)objmodel;
            return isAlreadyExist(model, Mode);
        }

        public override void SaveBaseData(ref object objmodel, string Mode, ref long ID)
        {
            var model = (CouponModel)objmodel;
            TblCouponMas Tbl = new TblCouponMas();

            if (model.PKID > 0)
            {
                Tbl = __dbContext.TblCouponMas.Find(model.PKID);
                if (Tbl == null)
                    throw new Exception("Coupon not found");
            }

            Tbl.PkCouponId = model.PKID;
            Tbl.Amount = model.Amount;
            Tbl.NoOfCoupon = model.NoOfCoupon;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();

            if (Mode == "Create")
            {
                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                Tbl.PkCouponId = getIdOfSeriesByEntity("PkCouponId", null, Tbl, "TblCouponMas");
                AddData(Tbl, false);

                // Generate unique 12-digit coupon codes
                var couponList = new List<TblCouponCodeLnk>();
                for (int i = 0; i < Tbl.NoOfCoupon; i++)
                {
                    couponList.Add(new TblCouponCodeLnk
                    {
                        FkCouponId = Tbl.PkCouponId,
                        CouponCode = GenerateUnique12DigitCode(),
                        FKUserID = Tbl.FKUserID,
                        FKCreatedByID = Tbl.FKUserID,
                        ModifiedDate = DateTime.Now,
                        CreationDate = DateTime.Now
                    });
                }

                if (couponList.Count > 0)
                    AddData(couponList, true);
            }
            else
            {
                CouponModel oldModel = GetSingleRecord(Tbl.PkCouponId);
                ID = Tbl.PkCouponId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Coupon, Tbl.PkCouponId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.NoOfCoupon.ToString(), Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
        }

        private string GenerateUnique12DigitCode()
        {
            // Generate random 12-digit numeric code
            Random rnd = new Random();
            string code;
            do
            {
                code = rnd.Next(0, 999999999).ToString("D9") + rnd.Next(100, 999).ToString(); // total 12 digits
            } while (__dbContext.TblCouponCodeLnk.Any(x => x.CouponCode == code));
            return code;
        }

        public string DeleteRecord(long PkCouponId)
        {
            string error = "";
            var oldModel = GetSingleRecord(PkCouponId);

            if (oldModel != null)
            {
                // Delete coupon codes
                var codes = __dbContext.TblCouponCodeLnk.Where(x => x.FkCouponId == PkCouponId).ToList();
                if (codes.Count > 0)
                    __dbContext.RemoveRange(codes);

                // Delete coupon master
                var master = __dbContext.TblCouponMas.Where(x => x.PkCouponId == PkCouponId).FirstOrDefault();
                if (master != null)
                    __dbContext.TblCouponMas.Remove(master);


                AddMasterLog((long)Handler.Form.Coupon, oldModel.PKID, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.NoOfCoupon.ToString(), GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
                __dbContext.SaveChanges();
            }

            return error;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>
            {
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="No Of Coupon", Fields="NoOfCoupon",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Amount", Fields="Amount",Width=30,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="User", Fields="UserName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=index++,Orderby =Orderby++, Heading ="Modified", Fields="DATE_MODIFIED",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="" },
            };
            return list;
        }


    }
}
