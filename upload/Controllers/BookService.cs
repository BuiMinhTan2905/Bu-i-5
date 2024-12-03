using Microsoft.AspNetCore.Mvc;

namespace upload.Controllers
{
    public class BookService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
