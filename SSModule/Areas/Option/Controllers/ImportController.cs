using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository.Option;

namespace SSAdmin.Areas.Option.Controllers
{
    [Area("Option")]
    public class ImportController : Controller
    {
        private readonly IImportRepository _repository;

        public ImportController(IImportRepository repository)
        {
            _repository = repository;

        }

        public IActionResult Stock()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Stock(ImportFileModel model)
        {

            try
            {
                var file = model.File;
                //Handler.Log("UploadFile", "In Try");
                if (file != null)
                {
                    //Handler.Log("UploadFile", "File Not Null");

                    DataTable dt = new DataTable();
                    string path = "";
                    path = Path.Combine("wwwroot", "ExcelFile");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string rn = new Random().Next(0, 9999).ToString("D6");
                    string filename = "Stock_"+rn +"_"+ DateTime.Now.Ticks + file.FileName;
                    //Handler.Log("UploadFile", "File Name :"+ filename);

                    string filePath = Path.Combine(path, filename);
                    //Handler.Log("UploadFile", "File Path :" + filePath);

                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        //Handler.Log("UploadFile", "fileStream");

                        file.CopyToAsync(fileStream);
                        //Handler.Log("UploadFile", "fileStream copy");

                        fileStream.Close();

                        //Handler.Log("UploadFile", "fileStream close");

                    }
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        //Handler.Log("UploadFile", "StreamReader");

                        string[] headers = sr.ReadLine().Split(',');
                        //Handler.Log("UploadFile", "StreamReader headers:"+ headers);

                        foreach (string header in headers)
                        {
                            dt.Columns.Add(header.Trim());
                            //Handler.Log("UploadFile", "dt Column Added:" + header.Trim());

                        }
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < headers.Length; i++)
                            {
                                dr[i] = rows[i].Trim();
                                //Handler.Log("UploadFile", "dt Row Added:" + rows[i].Trim());
                            }
                            dt.Rows.Add(dr);
                            //Handler.Log("UploadFile", "dt Row Added Done");

                        }
                        sr.Close();
                        //Handler.Log("UploadFile", "StreamReader Close");


                    }
                    if (dt.Rows.Count > 0)
                    {
                        int n = 0;
                        decimal d = 0;
                        var lst = (from DataRow dr in dt.Rows
                                   select new TranDetails()
                                   {
                                       Barcode = dr["Barcode"].ToString(),
                                       Product = dr["Artical"].ToString(),
                                       SubCategoryName = dr["SubSection"].ToString(),
                                       Batch = dr["Size"].ToString(),
                                       Qty = int.TryParse(dr["Qty"].ToString(), out n) ? Convert.ToInt32(dr["Qty"].ToString()) : 0,
                                       MRP = int.TryParse(dr["MRP"].ToString(), out n) ? Convert.ToDecimal(dr["MRP"].ToString()) : 0,
                                        //MRP = Convert.ToDecimal(dr["MRP"].ToString())

                                   }).ToList();
                        if (lst.Where(x => x.Qty == 0).ToList().Count == 0 && lst.Where(x => x.MRP == 0).ToList().Count == 0)
                        {
                            var dublicatebarcodeList = lst.GroupBy(x => x.Barcode).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                            if (dublicatebarcodeList.Count == 0)
                            {
                                ViewBag.Msg = _repository.SaveData(lst);
                            }
                            else
                                throw new Exception("Dublicate Barcode fond:" + string.Join(",", dublicatebarcodeList));
                        }
                        else
                            throw new Exception("Invalid Data");
                        //Handler.Log("UploadFile", "dt Row Count");

                        //model.IsUploadExcelFile = 1;
                        //return Json(new
                        //{
                        //    status = "success",
                        //    data = _repository.FileUpload(model, dt)
                        //});
                    }
                    else
                        throw new Exception("Invalid Data");
                }
                else
                    throw new Exception("File Not Uploaded Please Retry after Some Time");
            }
            catch (Exception ex)
            {
                //return Json(new
                //{
                //    status = "error",
                //    msg = ex.Message + " controller",
                //});
                ViewBag.Msg = "Error : "+ex.Message;
            }

            return View(model);

        }
    }
}
