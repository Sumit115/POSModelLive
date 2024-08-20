using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Drawing;

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


        public static string StringToBarcode(string str)
        {
            Image barcodeImage = GenerateBarcode(str);

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

        static Image GenerateBarcode(string data)
        {
            // Define the patterns for Code 39 characters (N: narrow, W: wide)
            Dictionary<char, string> code39Patterns = new Dictionary<char, string>
            {
                {'0', "NnNwWnWnN"},
                {'1', "WnNwNnNnW"},
                {'2', "NnWwNnNnW"},
                {'3', "WnWwNnNnN"},
                {'4', "NnNwWnNnW"},
                {'5', "WnNwWnNnN"},
                {'6', "NnWwWnNnN"},
                {'7', "NnNwNnWnW"},
                {'8', "WnNwNnWnN"},
                {'9', "NnWwNnWnN"},
                // Add more characters as needed
                {'A', "WnNnWnNnN"},
                {'B', "NnWnWnNnN"},
                {'C', "WnWnWnNnN"},
                {'D', "NnNnWnWnN"},
                {'E', "WnNnWnWnN"},
                {'F', "NnWnWnWnN"},
                {'G', "NnNnNnWnW"},
                {'H', "WnNnNnWnN"},
                {'I', "NnWnNnWnN"},
                {'J', "NnNnWnWnN"},
                {'K', "WnNnNnWnN"},
                {'L', "NnWnNnWnN"},
                {'M', "WnWnNnWnN"},
                {'N', "NnNnWnWnN"},
                {'O', "WnNnWnWnN"},
                {'P', "NnWnWnWnN"},
                {'Q', "NnNnNnWnW"},
                {'R', "WnNnNnWnN"},
                {'S', "NnWnNnWnN"},
                {'T', "NnNnWnWnN"},
                {'U', "WnNnNnWnN"},
                {'V', "NnWnNnWnN"},
                {'W', "WnWnNnWnN"},
                {'X', "NnNnWnWnN"},
                {'Y', "WnNnWnWnN"},
                {'Z', "NnWnWnWnN"},
                {'-', "NnNnNnWnW"},
                {'.', "WnNnNnWnN"},
                {' ', "NnWnNnWnN"},
                {'*', "NnNnWnWnN"} // Start/Stop character
            };

            // Start/stop character for Code 39
            const char startStop = '*';

            // Create a bitmap to draw the barcode
            int width = data.Length * 20 + 40; // Approximate width
            int height = 100; // Barcode height
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(System.Drawing.Color.White);
                int x = 10;

                // Draw the start character
                DrawPattern(graphics, code39Patterns[startStop], ref x, height);

                // Draw each character in the data
                foreach (char c in data)
                {
                    if (code39Patterns.ContainsKey(c))
                    {
                        DrawPattern(graphics, code39Patterns[c], ref x, height);
                    }
                }

                // Draw the stop character
                DrawPattern(graphics, code39Patterns[startStop], ref x, height);
            }

            return bitmap;
        }

        static void DrawPattern(Graphics graphics, string pattern, ref int x, int height)
        {
            foreach (char p in pattern)
            {
                int width = (p == 'W') ? 6 : 2;
                graphics.FillRectangle((p == 'n' || p == 'N') ? Brushes.Black : Brushes.White, x, 0, width, height);
                x += width;
            }
            x += 2; // Gap between characters
        }


    }
}
