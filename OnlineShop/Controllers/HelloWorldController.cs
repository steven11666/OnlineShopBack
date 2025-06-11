using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.mystring1 = "say hi";
            ViewData["mystring2"] = "yy";
            return View();
        }
        public string Welcome(string Name)
        {
            return "This is the Welcome Action : Hello, " + Name + " !!";
        }
    }
}
