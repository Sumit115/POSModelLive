﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace SSRepository.Repository.Master
{
    public class ProductRepository : Repository<TblProductMas>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
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
                    error = "Already Exits";
            }

            return error;
        }

        public List<ProductModel> GetList(int pageSize, int pageNo = 1, string search = "", long FkCatId = 0)
        {

            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProductModel> data = (from cou in __dbContext.TblProductMas
                                       join cat in __dbContext.TblCategoryMas on cou.FKProdCatgId equals cat.PkCategoryId
                                       join Pbrand in __dbContext.TblBrandMas on cou.FkBrandId equals Pbrand.PkBrandId
                                                             into tembrand
                                       from brand in tembrand.DefaultIfEmpty()
                                       where (EF.Functions.Like(cou.Product.Trim().ToLower(), Convert.ToString(search) + "%"))
                                       && cou.FKProdCatgId == (FkCatId > 0 ? FkCatId : cou.FKProdCatgId)
                                       orderby cou.PkProductId
                                       select (new ProductModel
                                       {
                                           PkProductId = cou.PkProductId,
                                           Product = cou.Product,
                                           NameToDisplay = cou.NameToDisplay,
                                           NameToPrint = cou.NameToPrint,
                                           Image = cou.Image,
                                           Alias = cou.Alias,
                                           Strength = cou.Strength,
                                           Barcode = cou.Barcode,
                                           Status = cou.Status ?? "",
                                           FKProdCatgId = cou.FKProdCatgId,
                                           HSNCode = cou.HSNCode,
                                           FkBrandId = cou.FkBrandId,
                                           ShelfID = cou.ShelfID,
                                           TradeDisc = cou.TradeDisc,
                                           MinStock = cou.MinStock,
                                           MaxStock = cou.MaxStock,
                                           MinDays = cou.MinDays,
                                           MaxDays = cou.MaxDays,
                                           CaseLot = cou.CaseLot ?? "",
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
                                           CategoryName = cat.CategoryName,
                                           BrandName = brand.BrandName,
                                           FKUserID = cou.FKUserID,
                                           DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblProductMas
                         join cat in __dbContext.TblCategoryMas on cou.FKProdCatgId equals cat.PkCategoryId
                         join Pbrand in __dbContext.TblBrandMas on cou.FkBrandId equals Pbrand.PkBrandId
                                               into tembrand
                         from brand in tembrand.DefaultIfEmpty()
                         where EF.Functions.Like(cou.Product.Trim().ToLower(), search + "%")
                         orderby cou.Product
                         select (new
                         {
                             PkProductId = cou.PkProductId,
                             Product = cou.Product,
                             NameToDisplay = cou.NameToDisplay,
                             NameToPrint = cou.NameToPrint,
                             CategoryName = cat.CategoryName,
                             BrandName = brand.BrandName,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else if (EnCustomFlag == (int)Handler.en_CustomFlag.Filter)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblProductMas
                         join cat in __dbContext.TblCategoryMas on cou.FKProdCatgId equals cat.PkCategoryId
                         join Pbrand in __dbContext.TblBrandMas on cou.FkBrandId equals Pbrand.PkBrandId
                                               into tembrand
                         from brand in tembrand.DefaultIfEmpty()
                         where EF.Functions.Like(cou.Product.Trim().ToLower(), search + "%")
                         orderby cou.Product
                         select (new
                         {
                             PkProductId = cou.PkProductId,
                             Product = cou.Product,
                             NameToDisplay = cou.NameToDisplay,
                         }
                       )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }
        public object CustomList_Batch(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FKProductId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblProdLotDtl
                         where cou.FKProductId == FKProductId
                         && EF.Functions.Like(cou.Batch.Trim().ToLower(), search + "%")
                         orderby cou.PkLotId
                         select (new
                         {
                             cou.PkLotId,
                             cou.Batch,
                             cou.Color,
                         }
                         )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }

        public object CustomList_Color(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", long FKProductId = 0,string Batch="")
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
                return ((from cou in __dbContext.TblProdLotDtl
                         where cou.FKProductId == FKProductId
                        && EF.Functions.Like(cou.Color.Trim().ToLower(), search + "%")
                          && (cou.Batch == Batch || Batch == "")
                         orderby cou.PkLotId
                         select (new 
                         {
                               cou.PkLotId,
                               cou.Color,
                               cou.Batch, 
                         }
                          )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList());
            }
            else
            {
                return null;
            }
        }


        public List<ProductModel> GetListByPartyId_InSaleInvoice(int pageSize, int pageNo = 1, string search = "", long FkPartyId = 0, long FkInvoiceId = 0, DateTime? InvoiceDate = null)
        {

            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<ProductModel> data = (from saledtl in __dbContext.TblSalesInvoicedtl
                                       join sale in __dbContext.TblSalesInvoicetrn on saledtl.FkId equals sale.PkId
                                       join cou in __dbContext.TblProductMas on saledtl.FkProductId equals cou.PkProductId
                                       join cat in __dbContext.TblCategoryMas on cou.FKProdCatgId equals cat.PkCategoryId
                                       join Pbrand in __dbContext.TblBrandMas on cou.FkBrandId equals Pbrand.PkBrandId
                                                             into tembrand
                                       from brand in tembrand.DefaultIfEmpty()
                                       where sale.FkPartyId == FkPartyId
                                       && sale.PkId == (FkInvoiceId > 0 ? FkInvoiceId : sale.PkId)
                                       && sale.EntryDate.Date == (InvoiceDate != null ? InvoiceDate.Value.Date : sale.EntryDate.Date)
                                       orderby cou.PkProductId
                                       select (new ProductModel
                                       {
                                           PkProductId = cou.PkProductId,
                                           Product = cou.Product,
                                           NameToDisplay = cou.NameToDisplay,
                                           NameToPrint = cou.NameToPrint,
                                           Image = cou.Image,
                                           Alias = cou.Alias,
                                           Strength = cou.Strength,
                                           Barcode = cou.Barcode,
                                           Status = cou.Status,
                                           FKProdCatgId = cou.FKProdCatgId,
                                           HSNCode = cou.HSNCode,
                                           FkBrandId = cou.FkBrandId,
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
                                           CategoryName = cat.CategoryName,
                                           BrandName = brand.BrandName,
                                           FKInvoiceID = sale.PkId,
                                           InvoiceSrNo = saledtl.SrNo,
                                           FKInvoiceSrID = sale.FKSeriesId,
                                           FKUserID = cou.FKUserID,
                                           DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                                       }
                                      )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        public ProductModel GetSingleRecord(long PkProductId)
        {

            ProductModel data = new ProductModel();
            data = (from cou in __dbContext.TblProductMas
                    join cat in __dbContext.TblCategoryMas on cou.FKProdCatgId equals cat.PkCategoryId
                    where cou.PkProductId == PkProductId
                    select (new ProductModel
                    {
                        PkProductId = cou.PkProductId,
                        Product = cou.Product,
                        NameToDisplay = cou.NameToDisplay,
                        NameToPrint = cou.NameToPrint,
                        Image = cou.Image,
                        Alias = cou.Alias,
                        Strength = cou.Strength,
                        Barcode = cou.Barcode,
                        Status = cou.Status,
                        FKProdCatgId = cou.FKProdCatgId,
                        FKTaxID = cou.FKTaxID,
                        HSNCode = cou.HSNCode,
                        FkBrandId = cou.FkBrandId,
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
                        Genration = cou.Genration,
                        CodingScheme = cou.CodingScheme,
                        FkUnitId = cou.FkUnitId,
                        CategoryName = cat.CategoryName,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();

            return data;
        }
        public object GetDrpProduct(int pageSize, int pageNo = 1, string search = "", long FkCatId = 0)
        {
            if (search != null) search = search.ToLower();
            if (search == null) search = "";

            var result = GetList(pageSize, pageNo, search);
            // result.Insert(0, new ProductModel { PkProductId = 0, Product = "Select" });


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
            ProductModel oldModel = GetSingleRecord(PkProductId);
             

            if (Error == "")
            {
                var lst = (from x in __dbContext.TblProductMas
                           where x.PkProductId == PkProductId
                           select x).ToList();
                if (lst.Count > 0)
                    __dbContext.TblProductMas.RemoveRange(lst); 

                AddMasterLog((long)Handler.Form.Product, PkProductId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), true, JsonConvert.SerializeObject(oldModel), oldModel.Product, GetUserID(), DateTime.Now, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
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
            Tbl.FKProdCatgId = model.FKProdCatgId;
            Tbl.FKTaxID = model.FKTaxID;
            Tbl.HSNCode = model.HSNCode;
            Tbl.FkBrandId = model.FkBrandId > 0 ? model.FkBrandId : null;
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
            Tbl.MRPSaleRateUnit = "";
            Tbl.SaleRate = model.SaleRate;
            Tbl.TradeRate = model.TradeRate;
            Tbl.DistributionRate = model.DistributionRate;
            Tbl.PurchaseRate = model.PurchaseRate;
            Tbl.PurchaseRateUnit = "";
            Tbl.KeepStock = model.KeepStock;
            Tbl.Genration = model.Genration;
            Tbl.CodingScheme = model.CodingScheme;
            Tbl.FkUnitId = model.FkUnitId;
            Tbl.ModifiedDate = DateTime.Now;
            Tbl.FKUserID = GetUserID();
            if (Mode == "Create")
            {

                Tbl.FKCreatedByID = Tbl.FKUserID;
                Tbl.CreationDate = Tbl.ModifiedDate;
                var data = __dbContext.TblProductMas.OrderByDescending(u => u.PkProductId).FirstOrDefault();
                if (data != null)
                {
                    Tbl.PkProductId = data.PkProductId + 1;
                }
                else
                {
                    Tbl.PkProductId = 1;
                }

                //obj.PkcountryId = ID = getIdOfSeriesByEntity("PkcountryId", null, obj);
                AddData(Tbl, false);
            }
            else
            {

                ProductModel oldModel = GetSingleRecord(Tbl.PkProductId);
                ID = Tbl.PkProductId;
                UpdateData(Tbl, false);
                AddMasterLog((long)Handler.Form.Product, Tbl.PkProductId, -1, Convert.ToDateTime(oldModel.DATE_MODIFIED), false, JsonConvert.SerializeObject(oldModel), oldModel.Product, Tbl.FKUserID, Tbl.ModifiedDate, oldModel.FKUserID, Convert.ToDateTime(oldModel.DATE_MODIFIED));
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKProductID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="Name", Fields="Product",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Alias", Fields="Alias",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Strength", Fields="Strength",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Barcode", Fields="Barcode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Status", Fields="Status",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Section", Fields="CategoryName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="HSNCode", Fields="HSNCode",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Brand", Fields="BrandName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="MRP", Fields="MRP",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=13, Orderby =13, Heading ="SaleRate", Fields="SaleRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=14, Orderby =14, Heading ="TradeRate", Fields="TradeRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=15, Orderby =15, Heading ="DistributionRate", Fields="DistributionRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=16, Orderby =16, Heading ="PurchaseRate	", Fields="PurchaseRate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id=12, Orderby =17, Heading ="Created", Fields="CreateDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },
                  new ColumnStructure{ pk_Id=13, Orderby =18, Heading ="Modified", Fields="ModifiDate",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

            };
            return list;
        }

        public List<CategoryModel> prodCatgList(int pageSize, int pageNo = 1, string search = "")
        {
            CategoryRepository rep = new CategoryRepository(__dbContext, _contextAccessor);
            return rep.GetList(pageSize, pageNo, search);
        }

        public ProductModel GetSingleRecord_ByBarcode(string Barcode)
        {
            //Barcode,ProductId
            //TblProduct>TblProdLot>tblProdQTYBarcode
            ProductModel data = new ProductModel();
            data = (from cou in __dbContext.TblProductMas
                    where cou.Barcode == Barcode
                    select (new ProductModel
                    {
                        PkProductId = cou.PkProductId,
                        Product = cou.Product,
                        NameToDisplay = cou.NameToDisplay,
                        NameToPrint = cou.NameToPrint,
                        Image = cou.Image,
                        Alias = cou.Alias,
                        Strength = cou.Strength,
                        Barcode = cou.Barcode,
                        Status = cou.Status,
                        FKProdCatgId = cou.FKProdCatgId,
                        FKTaxID = cou.FKTaxID,
                        HSNCode = cou.HSNCode,
                        FkBrandId = cou.FkBrandId,
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
                        Genration = cou.Genration,
                        CodingScheme = cou.CodingScheme,
                        FkUnitId = cou.FkUnitId,
                        FKUserID = cou.FKUserID,
                        DATE_MODIFIED = cou.ModifiedDate.ToString("dd-MMM-yyyy")
                    })).FirstOrDefault();

            return data;
        }

        //GetProductDetail ->@barcode='' ,@productID,=0,@lotId=0

        public string GetBarCode()
        {
            Int64 ProdBarcode;
            Int64 InitBarcode = 99900000000000;
            Int32 DefBarcodeLen;
            Int64 DefBarcode = 0;
            Int64 BranchNo = 0;
            Int64 MaxDefBarcode;
            string SKUDef = "";

            var lstSysDef = (from x in __dbContext.TblSysDefaults
                             where (x.SysDefKey.Contains("InitialProdBarcode") || x.SysDefKey.Contains("BranchNo") || x.SysDefKey.Contains("SKUDefinition"))
                             select x).ToList();

            foreach (var item in lstSysDef)
            {
                if (item.SysDefKey == "InitialProdBarcode")
                {
                    if (item.SysDefValue != null && item.SysDefValue != "")
                    {
                        InitBarcode = Convert.ToInt64(item.SysDefValue);
                    }
                }
                else if (item.SysDefKey == "BranchNo")
                {
                    if (item.SysDefValue != null && item.SysDefValue != "")
                    {
                        BranchNo = Convert.ToInt64(item.SysDefValue);
                    }
                }
                else if (item.SysDefKey == "SKUDefinition")
                {
                    if (item.SysDefValue != null && item.SysDefValue != "")
                    {
                        SKUDef = item.SysDefValue;
                    }
                }
            }
            DefBarcodeLen = InitBarcode.ToString().Length;

            if (BranchNo > 0)
            {
                DefBarcode = Convert.ToInt64(InitBarcode.ToString().Substring(0, 5));
            }

            string result = new String('9', DefBarcodeLen - DefBarcode.ToString().Length);
            MaxDefBarcode = Convert.ToInt64(DefBarcode.ToString() + result);

            dynamic OutParam;
            dynamic OutParam1;

            try
            {
                //if (BranchNo > 0)
                //{
                //    OutParam = (from b in __dbContext.TblProdLotDtl where b.Barcode >= InitBarcode && b.Barcode <= Convert.ToInt64(b.Barcode.ToString().Substring(0, DefBarcodeLen)) && Convert.ToInt64(b.Barcode.ToString().Substring(0, 5)) == DefBarcode select b.Barcode).Max();
                //}
                //else
                //{
                //    OutParam = (from b in __dbContext.TblProdLotDtl where b.Barcode >= InitBarcode && b.Barcode <= Convert.ToInt64(b.Barcode.ToString().Substring(0, DefBarcodeLen)) && Convert.ToInt64(b.Barcode.ToString().Substring(0, 1)) == DefBarcode select b.Barcode).Max();
                //}
            }
            catch
            {
                OutParam = InitBarcode.ToString();
            }

            //if (OutParam == null)
            OutParam = Convert.ToInt64(InitBarcode);

            try
            {
                //if (BranchNo > 0)
                //{
                //    ProdBarcode = (from b in __dbContext.TblProductMas where b.Barcode >= InitBarcode && b.Barcode <= Convert.ToInt64(b.Barcode.ToString().Substring(0, DefBarcodeLen)) && Convert.ToInt64(b.Barcode.ToString().Substring(0, 5)) == DefBarcode select (long)b.Barcode).Max();
                //}
                //else
                //{

                //    ProdBarcode = (from b in __dbContext.TblProductMas where b.Barcode >= InitBarcode && b.Barcode <= Convert.ToInt64(b.Barcode.ToString().Substring(0, DefBarcodeLen)) && Convert.ToInt64(b.Barcode.ToString().Substring(0, 1)) == DefBarcode select (long)b.Barcode).Max();
                //}

            }
            catch (Exception ex)
            {
                ProdBarcode = InitBarcode;
            }

            //if (InitBarcode > OutParam && InitBarcode > ProdBarcode)
            //{
            //    OutParam = InitBarcode;
            //}
            //else if (OutParam > ProdBarcode)
            //{
            //    OutParam = OutParam;
            //}
            //else
            //{
            //    OutParam = ProdBarcode;
            //}

            //if (OutParam >= MaxDefBarcode)
            //{
            //    OutParam = 0;
            //}
            //else
            //    OutParam = OutParam + 1;
            string ReturnVar = Convert.ToString(OutParam);

            return ReturnVar;

        }


    }
}



















