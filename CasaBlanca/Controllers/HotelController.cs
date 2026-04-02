using System.Web.Mvc;
using CasaBlanca.Models;

namespace CasaBlanca.Controllers
{
    public class HotelController : Controller
    {
        public ActionResult Lista()
        {
            var hoteles = HotelDAO.ListarHotelesActivos();
            return View(hoteles);
        }
    }
}