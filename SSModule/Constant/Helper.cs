using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;


namespace SSAdmin.Constant
{
    public class Helper
    {
        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.GetTypedHeaders().Referer == null ||
         filterContext.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != filterContext.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
                {
                    filterContext.HttpContext.Response.Redirect("/");
                }
            }
        }

        public enum FoodChoice
        {
            Veg = 1,
            NonVeg = 2,
            Egg = 3,
            Other = 4
        }


        public static string StringToBarcode(string str, int MaxWidth = 500, int MaxHeight = 150)
        {
            IronBarCode.GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(str, IronBarCode.BarcodeWriterEncoding.Code128);
            barcode.ResizeTo(MaxWidth, MaxHeight);
            System.Drawing.Font barcodeValueFont = new System.Drawing.Font("Cambria", 28);
            barcode.AddBarcodeValueTextBelowBarcode(barcodeValueFont, System.Drawing.Color.Black,5);
            // Styling a Barcode and adding annotation text
            barcode.ChangeBarCodeColor(System.Drawing.Color.Black);
            barcode.SetMargins(5);
            //System.Drawing.Bitmap QrBitmap = barcode.ToBitmap();
            //byte[] BitmapArray = barcode.ToGifBinaryData();
            //var aa = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
             
            string path = "";
            path = Path.Combine("wwwroot", "Barcode"); 

            Random generator = new Random();
            string rn = generator.Next(0, 9999).ToString("D6");
            string FilePath = "";
            string filename = str + rn + ".png";

            FilePath = Path.Combine(path, filename);
            barcode.SaveAsPng(FilePath);
            return "/Barcode/"+filename;
        }
    }
}
