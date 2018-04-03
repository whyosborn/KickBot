using System.Linq;
using SlackAPI;
using System.Web.Mvc;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
    
            return View();
        }
    }
}
