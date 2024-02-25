using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;

namespace SSRepository.Repository.Master
{
    public class ProductRepository : Repository<TblProductMas>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            __FormID = (long)en_Form.Product;
        }
     
        public string isAlreadyExist(ProductModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            if (!string.IsNullOrEmpty(model.Product))
            {
                cnt = (from x in __dbContext.TblProductMas
                       where x.Product == model.Product && x.PkProductId != model.PkProductId
                       select x).Count();
                if (cnt > 0)
                    error = "Mobile Already Exits";
            }

            return error;
        }

        public List<ProductModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProductModel> data = (from cou in __dbContext.TblProductMas
                                       join cat in __dbContext.TblCategoryMas on cou.FkprodCatgId equals cat.PkCategoryId
                                           // where (EF.Functions.Like(cou.Name.Trim().ToLower(), Convert.ToString(search) + "%"))
                                       orderby cou.PkProductId
                                       select (new ProductModel
                                       {
                                           PkProductId = cou.PkProductId,
                                           FKUserId = cou.FKUserId,
                                           src = cou.Src,
                                           DATE_MODIFIED = cou.DateModified,
                                           DATE_CREATED = cou.DateCreated,
                                           Product = cou.Product,
                                           NameToDisplay = cou.NameToDisplay,
                                           NameToPrint = cou.NameToPrint,
                                           Image = cou.Image,
                                           Alias = cou.Alias,
                                           Strength = cou.Strength,
                                           Barcode = cou.Barcode,
                                           Status = cou.Status,
                                           FkprodCatgId = cou.FkprodCatgId,
                                           FKTaxID = cou.FKTaxID,
                                           HSNCode = cou.HSNCode,
                                           Brand = cou.Brand,
                                           ShelfID = cou.ShelfID,
                                           TradeDisc = cou.TradeDisc,
                                           MinStock = cou.MinStock,
                                           MaxStock = cou.MaxStock,
                                           MinDays = cou.MinDays,
                                           MaxDays = cou.MaxDays,
                                           CaseLot = cou.CaseLot,
                                           BoxSize = cou.BoxSize,
                                           Description = cou.Description,
                                           Unit1 = cou.Unit1,
                                           ProdConv1 = cou.ProdConv1,
                                           Unit2 = cou.Unit2,
                                           ProdConv2 = cou.ProdConv2,
                                           Unit3 = cou.Unit3,
                                           MRP = cou.MRP,
                                           SaleRate = cou.SaleRate,
                                           TradeRate = cou.TradeRate,
                                           DistributionRate = cou.DistributionRate,
                                           PurchaseRate = cou.PurchaseRate,
                                           KeepStock = cou.KeepStock,
                                           CategoryName=cat.CategoryName,
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public ProductModel GetSingleRecord(long PkProductId)
        {

            ProductModel data = new ProductModel();
            data = (from cou in __dbContext.TblProductMas
                    where cou.PkProductId == PkProductId
                    select (new ProductModel
                    {
                        PkProductId = cou.PkProductId,
                        FKUserId = cou.FKUserId,
                        src = cou.Src,
                        DATE_MODIFIED = cou.DateModified,
                        DATE_CREATED = cou.DateCreated,
                        Product = cou.Product,
                        NameToDisplay = cou.NameToDisplay,
                        NameToPrint = cou.NameToPrint,
                        Image = cou.Image,
                        Alias = cou.Alias,
                        Strength = cou.Strength,
                        Barcode = cou.Barcode,
                        Status = cou.Status,
                        FkprodCatgId = cou.FkprodCatgId,
                        FKTaxID = cou.FKTaxID,
                        HSNCode = cou.HSNCode,
                        Brand = cou.Brand,
                        ShelfID = cou.ShelfID,
                        TradeDisc = cou.TradeDisc,
                        MinStock = cou.MinStock,
                        MaxStock = cou.MaxStock,
                        MinDays = cou.MinDays,
                        MaxDays = cou.MaxDays,
                        CaseLot = cou.CaseLot,
                        BoxSize = cou.BoxSize,
                        Description = cou.Description,
                        Unit1 = cou.Unit1,
                        ProdConv1 = cou.ProdConv1,
                        Unit2 = cou.Unit2,
                        ProdConv2 = cou.ProdConv2,
                        Unit3 = cou.Unit3,
                        MRP = cou.MRP,
                        SaleRate = cou.SaleRate,
                        TradeRate = cou.TradeRate,
                        DistributionRate = cou.DistributionRate,
                        PurchaseRate = cou.PurchaseRate,
                        KeepStock = cou.KeepStock,
                    })).FirstOrDefault();
            return data;
        }
        public object GetDrpProduct(int pageno, int pagesize, string search = "")
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pagesize, pageno, search);


            return (from r in result
                    select new
                    {
                        r.PkProductId,
                        r.Product
                    }).ToList(); ;
        }

        public string DeleteRecord(long PkProductId)
        {
            string Error = "";
            ProductModel obj = GetSingleRecord(PkProductId);

            //var Country = (from x in _context.TblStateMas
            //               where x.FkcountryId == PkProductId
            //               select x).Count();
            //if (Country > 0)
            //    Error += "Table Name -  StateMas : " + Country + " Records Exist";


            if (Error == "")
            {
                var lst = (from x in __dbContext.TblProductMas
                           where x.PkProductId == PkProductId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblProductMas.RemoveRange(lst);

                //var imglst = (from x in _context.TblImagesDtl
                //              where x.Fkid == PkProductId && x.FKSeriesID == __FormID
                //              select x).ToList();
                //if (imglst.Count > 0)
                //    _context.RemoveRange(imglst);

                //var remarklst = (from x in _context.TblRemarksDtl
                //                 where x.Fkid == PkProductId && x.FormId == __FormID
                //                 select x).ToList();
                //if (remarklst.Count > 0)
                //    _context.RemoveRange(remarklst);
                //AddMasterLog(obj, __FormID, GetProductID(), PkProductId, obj.FKProductID, obj.DATE_MODIFIED, true);
                __dbContext.SaveChanges();
            }

            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            ProductModel model = (ProductModel)objmodel;
            string error = "";
            error = isAlreadyExist(model, Mode);
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            ProductModel model = (ProductModel)objmodel;
            TblProductMas Tbl = new TblProductMas();
            if (model.PkProductId > 0)
            {
                var _entity = __dbContext.TblProductMas.Find(model.PkProductId);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PkProductId = model.PkProductId;
            Tbl.Product = model.Product;
            Tbl.NameToDisplay = model.NameToDisplay;
            Tbl.NameToPrint = model.NameToPrint;
            Tbl.Image = model.Image;
            Tbl.Alias = model.Alias;
            Tbl.Strength = model.Strength;
            Tbl.Barcode = model.Barcode;
            Tbl.Status = model.Status;
            Tbl.FkprodCatgId = model.FkprodCatgId;
            Tbl.FKTaxID = model.FKTaxID;
            Tbl.HSNCode = model.HSNCode;
            Tbl.Brand = model.Brand;
            Tbl.ShelfID = model.ShelfID;
            Tbl.TradeDisc = model.TradeDisc;
            Tbl.MinStock = model.MinStock;
            Tbl.MaxStock = model.MaxStock;
            Tbl.MinDays = model.MinDays;
            Tbl.MaxDays = model.MaxDays;
            Tbl.CaseLot = model.CaseLot;
            Tbl.BoxSize = model.BoxSize;
            Tbl.Description = model.Description;
            Tbl.Unit1 = model.Unit1;
            Tbl.ProdConv1 = model.ProdConv1;
            Tbl.Unit2 = model.Unit2;
            Tbl.ProdConv2 = model.ProdConv2;
            Tbl.Unit3 = model.Unit3;
            Tbl.MRP = model.MRP;
            Tbl.SaleRate = model.SaleRate;
            Tbl.TradeRate = model.TradeRate;
            Tbl.DistributionRate = model.DistributionRate;
            Tbl.PurchaseRate = model.PurchaseRate;
            Tbl.KeepStock = model.KeepStock;
            Tbl.DateModified = DateTime.Now;
            if (Mode == "Create")
            {
                Tbl.Src = model.src;
                Tbl.FKUserId = model.FKUserId;
                Tbl.DateCreated = DateTime.Now;
                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                ProductModel oldModel = GetSingleRecord(Tbl.PkProductId);
                ID = Tbl.PkProductId;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKProductID, oldModel.PkProductId, oldModel.FKProductID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKProductID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList()
        {
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Product", Fields="Product",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Strength", Fields="Strength",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Barcode", Fields="Barcode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Status", Fields="Status",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="CategoryName", Fields="CategoryName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="HSNCode", Fields="HSNCode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Brand", Fields="Brand",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="MRP", Fields="MRP",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="SaleRate", Fields="SaleRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="TradeRate", Fields="TradeRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="DistributionRate", Fields="DistributionRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="PurchaseRate	", Fields="PurchaseRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
              
            };
            return list;
        }


    }
}



















