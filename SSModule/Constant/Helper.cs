using IronBarCode;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using ZXing;
using ZXing.Common;

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


        public static string StringToBarcodeNew(string barcodeText)
        {
            string path = "";
            path = Path.Combine("wwwroot", "Barcode");

            Random generator = new Random();
            string rn = generator.Next(0, 9999).ToString("D6");
            string FilePath = "";
            string filename = barcodeText + ".png";
            try
            {
                var barcodeWriter = new BarcodeWriter<Bitmap>
                {
                    Format = BarcodeFormat.CODE_39, // Specify Code 39 format
                    Options = new EncodingOptions
                    {
                        Height = 100,  // Barcode image height
                        Width = 300,   // Barcode image width
                        Margin = 0     // Remove extra margins
                    }
                };

                //// Generate barcode as a Bitmap
                using Bitmap barcodeBitmap = barcodeWriter.Write(barcodeText);

                Image barcodeImage = GenerateBarcode(barcodeText,300,100);

                var barcode = BarcodeWriter.CreateBarcode(barcodeText, BarcodeEncoding.Code39);

                // Customize the barcode (optional)
                barcode.SetMargins(0);
                barcode.ResizeTo(900, 300);



                FilePath = Path.Combine(path, filename);

                FilePath = Path.Combine(path, barcodeText + "static.png");
                // if (!Directory.Exists(FilePath))
                barcode.SaveAsPng(FilePath);

                FilePath = Path.Combine(path, barcodeText + "ZXing.png");
                barcodeBitmap.Save(FilePath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
            }
            return "/Barcode/" + filename;
        }

        public static string StringToBarcode(string str)
        {
            Image barcodeImage = GenerateBarcode(str, 900, 300);

            string path = "";
            path = Path.Combine("wwwroot", "Barcode");

            Random generator = new Random();
            string rn = generator.Next(0, 9999).ToString("D6");
            string FilePath = "";
            string filename = str + ".png";

            FilePath = Path.Combine(path, filename);
            // if (!Directory.Exists(FilePath))
            barcodeImage.Save(FilePath, System.Drawing.Imaging.ImageFormat.Png);

            return "/Barcode/" + filename;
        }
        static Image GenerateBarcode(string data, int width, int height)
        {
            // Define the patterns for Code 39 characters
            Dictionary<char, string> code39Patterns = new Dictionary<char, string>
        {
            { '0', "101001101101" }, { '1', "110100101011" }, { '2', "101100101011" },
            { '3', "110110010101" }, { '4', "101001101011" }, { '5', "110100110101" },
            { '6', "101100110101" }, { '7', "101001011011" }, { '8', "110100101101" },
            { '9', "101100101101" }, { 'A', "110101001011" }, { 'B', "101101001011" },
            { 'C', "110110100101" }, { 'D', "101011001011" }, { 'E', "110101100101" },
            { 'F', "101101100101" }, { 'G', "101010011011" }, { 'H', "110101001101" },
            { 'I', "101101001101" }, { 'J', "101011001101" }, { 'K', "110101010011" },
            { 'L', "101101010011" }, { 'M', "110110101001" }, { 'N', "101011010011" },
            { 'O', "110101101001" }, { 'P', "101101101001" }, { 'Q', "101010110011" },
            { 'R', "110101011001" }, { 'S', "101101011001" }, { 'T', "101011011001" },
            { 'U', "110010101011" }, { 'V', "100110101011" }, { 'W', "110011010101" },
            { 'X', "100101101011" }, { 'Y', "110010110101" }, { 'Z', "100110110101" },
            { '-', "100101011011" }, { '.', "110010101101" }, { ' ', "100110101101" },
            { '*', "100101101101" }, { '$', "100100100101" }, { '/', "100100101001" },
            { '+', "100101001001" }, { '%', "101001001001" }
        };

            // Prepend and append the start/stop character
            data = $"*{data.ToUpper()}*";

            // Validate the data
            foreach (char c in data)
            {
                if (!code39Patterns.ContainsKey(c))
                {
                    throw new ArgumentException($"Invalid character in data: {c}");
                }
            }

            // Create a bitmap with fixed size
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                // Calculate barcode dimensions
                int barWidth = Math.Min(width / (data.Length * 10), 5); // Approx. 12 bars per character, max width per bar
                int barcodeWidth = data.Length * 12 * barWidth; // Total barcode width
                int leftMargin = (width - barcodeWidth) / 2; // Auto-adjust left and right margin
                int topMargin = height / 10; // Dynamic top margin (10% of total height)
                int barcodeHeight = height - (2 * topMargin); // Height with top and bottom margin

                int x = leftMargin;
                string enn = "";
                // Draw the barcode
                foreach (char c in data)
                {
                    string pattern = code39Patterns[c];
                    enn += pattern;
                    DrawPattern(graphics, pattern, ref x, barcodeHeight, barWidth, topMargin);
                }
            }

            return bitmap;
        }

        static void DrawPattern(Graphics graphics, string pattern, ref int x, int height, int barWidth, int topMargin)
        {
            foreach (char p in pattern)
            {
                int width = (p == '1') ? barWidth : barWidth; // Wide or narrow bar
                Brush brush = (p == '1') ? Brushes.Black : Brushes.White;
                graphics.FillRectangle(brush, x, topMargin, width, height); // Adjust to include top margin
                x += width;
            }

            x += barWidth; // Gap between characters
        }
    }
}
