using System.Web.Mvc;
using CasaBlanca.Models;

namespace CasaBlanca.Controllers
{
    public class HomeController : Controller
    {
        // Muestra la página principal con los hoteles activos
        public ActionResult Index()
        {
            ViewBag.Hoteles = HotelDAO.ListarHotelesActivos();
            return View();
        }

        // Muestra la página principal con los hoteles activos
        public ActionResult Nosotros()
        {
            return View();
        }
    }
}