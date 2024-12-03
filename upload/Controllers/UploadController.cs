using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace upload.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file selected or file is empty.";
                return View();
            }

            // Define the uploads folder path
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

            // Ensure the folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Construct the full file path
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            // Save the file
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                ViewBag.Message = "File uploaded successfully!";
            }
            catch
            {
                ViewBag.Message = "An error occurred while uploading the file.";
            }

            return View();
        }
    }
}
