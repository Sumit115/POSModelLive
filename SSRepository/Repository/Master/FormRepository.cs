using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.X509Certificates;

namespace SSRepository.Repository.Master
{
    public class FormRepository : Repository<TblFormMas>, IFormRepository
    {
        public FormRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public string isAlreadyExist(FormModel model, string Mode)
        {
            dynamic cnt;
            string error = "";
            


            return error;
        }

        public List<FormModel> GetList(int pageSize, int pageNo = 1, string search = "")
        {
            if (search != null) search = search.ToLower();
            pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;
            List<FormModel> data = (from l in __dbContext.TblFormMas
                                        //where (EF.Functions.Like(cou.FormName.Trim().ToLower(), Convert.ToString(search) + "%"))
                                    orderby l.PKFormID
                                    select (new FormModel
                                    {
                                        PKID = l.PKFormID,
                                        FormName = l.FormName,
                                        FKMasterFormID = l.FKMasterFormID,
                                        MasterForm = l.ParentForm.FormName,
                                        SeqNo = l.SeqNo,
                                        ShortName = l.ShortName,
                                        ShortCut = l.ShortCut,
                                        ToolTip = l.ToolTip,
                                        Image = l.Image,
                                        FormType = l.FormType,
                                        WebURL = l.WebURL,
                                        IsActive = l.IsActive

                                    }
                                   )).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }


        public FormModel? GetSingleRecord(long PKID)
        {

            var data = (from l in __dbContext.TblFormMas
                        where l.PKFormID == PKID
                        select (new FormModel
                        {

                            PKID = l.PKFormID,
                            FormName = l.FormName,
                            FKMasterFormID = l.FKMasterFormID,
                            MasterForm = l.ParentForm.FormName,
                            SeqNo = l.SeqNo,
                            ShortName = l.ShortName,
                            ShortCut = l.ShortCut,
                            ToolTip = l.ToolTip,
                            Image = l.Image,
                            FormType = l.FormType,
                            WebURL = l.WebURL,
                            IsActive = l.IsActive

                        })).FirstOrDefault();
            return data;
        }

        public string DeleteRecord(long PkId)
        {
            string Error = "";

            var lst = (from x in __dbContext.TblFormMas
                       where x.PKFormID == PkId
                       select x).ToList();
            if (lst.Count > 0)
                __dbContext.TblFormMas.RemoveRange(lst);

            __dbContext.SaveChanges();


            return Error;
        }
        public override string ValidateData(object objmodel, string Mode)
        {

            FormModel model = (FormModel)objmodel;
            string error = "";
            return error;

        }
        public override void SaveBaseData(ref object objmodel, string Mode, ref Int64 ID)
        {
            FormModel model = (FormModel)objmodel;
            TblFormMas Tbl = new TblFormMas();
            if (model.PKID > 0)
            {
                var _entity = __dbContext.TblFormMas.Find(model.PKID);
                if (_entity != null) { Tbl = _entity; }
                else { throw new Exception("data not found"); }
            }

            Tbl.PKFormID = model.PKID;
            Tbl.FormName = model.FormName;
            Tbl.FKMasterFormID = model.FKMasterFormID;
            Tbl.SeqNo = model.SeqNo;
            Tbl.ShortName = model.ShortName;
            Tbl.ShortCut = model.ShortCut;
            Tbl.ToolTip = model.ToolTip;
            Tbl.Image = model.Image;
            Tbl.FormType = model.FormType;
            Tbl.WebURL = model.WebURL;
            Tbl.IsActive = model.IsActive;
            if (Mode == "Create")
            {
                AddData(Tbl, false);
            }
            else
            {

                FormModel? oldModel = GetSingleRecord(Tbl.PKFormID);
                ID = Tbl.PKFormID;
                UpdateData(Tbl, false);
                //AddMasterLog(oldModel, __FormID, tblCountry.FKBrandID, oldModel.PkBrandId, oldModel.FKBrandID, oldModel.DATE_MODIFIED);
            }
            //AddImagesAndRemark(obj.PkcountryId, obj.FKBrandID, tblCountry.Images, tblCountry.Remarks, tblCountry.ImageStatus.ToString().ToLower(), __FormID, Mode.Trim());
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            var list = new List<ColumnStructure>
            {
                new ColumnStructure{ pk_Id= index++, Orderby = index++, Heading ="FormName", Fields= "FormName", Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure{ pk_Id= index++, Orderby = index++, Heading ="MasterForm", Fields= "MasterForm", Width=20,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                new ColumnStructure { pk_Id = index++, Orderby = index++, Heading = "SeqNo", Fields = "SeqNo", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = index++, Heading = "ShortName", Fields = "ShortName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = index++, Heading = "ShortCut", Fields = "ShortCut", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = index++, Heading = "ToolTip", Fields = "ToolTip", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = index++, Heading = "FormType", Fields = "FormType", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                  new ColumnStructure{ pk_Id=index++, Orderby =index++, Heading = "IsActive", Fields= "IsActive", Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="" },

            };
            return list;
        }


    }
}



















