using Microsoft.AspNetCore.Mvc;

namespace SSAdmin.Areas.ImportExport.Controllers
{
    [Area("ImportExport")]
    public class Import : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile SingleFile)
        {
            if (SingleFile == null || SingleFile.Length == 0)
            {
                ModelState.AddModelError("", "File not selected");
                return View("Index");
            }

            var permittedExtensions = new[] { ".jpg", ".png", ".gif" };
            var extension = Path.GetExtension(SingleFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Invalid file type.");
            }

            // Optional: Validate MIME type as well
            var mimeType = SingleFile.ContentType;
            var permittedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!permittedMimeTypes.Contains(mimeType))
            {
                ModelState.AddModelError("", "Invalid MIME type.");
            }

            //Validating the File Size
            if (SingleFile.Length > 10000000) // Limit to 10 MB
            {
                ModelState.AddModelError("", "The file is too large.");
            }

            if (ModelState.IsValid)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", SingleFile.FileName);

                //Using Streaming
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await SingleFile.CopyToAsync(stream);
                }

                // Process the file here (e.g., save to the database, storage, etc.)
                return View("UploadSuccess");
            }

            return View("Index");
        }
    }
}
