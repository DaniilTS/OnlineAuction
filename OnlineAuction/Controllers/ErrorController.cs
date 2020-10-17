using Microsoft.AspNetCore.Mvc;

namespace OnlineAuction.Controllers
{
    public class ErrorController : Controller
    {
        // GET
        public IActionResult Index(int? statuscode = null)
        {
            ViewData["Error"] = statuscode;
            return View();
        }
    }
}