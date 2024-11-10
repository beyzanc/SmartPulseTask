using Microsoft.AspNetCore.Mvc;

namespace SmartPulseTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Error()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
            return View();
        }

    }
}