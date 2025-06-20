using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminDyanamoEnterprises.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductsHome()
        {
            return View();
        }
        
    }
}
