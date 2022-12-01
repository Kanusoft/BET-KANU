using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    public class NamesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
