
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC_App
{
    public class ProgressController : Controller
    {
        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
