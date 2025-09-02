using Microsoft.AspNetCore.Mvc;

namespace RFIDApi.controller
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
