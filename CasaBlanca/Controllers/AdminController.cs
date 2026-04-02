using System;
using System.Web.Mvc;
using CasaBlanca.Models;

namespace CasaBlanca.Controllers
{
    public class AdminController : Controller
    {
        private bool EsAdmin()
        {
            return Session["EsAdmin"] != null && (bool)Session["EsAdmin"];
        }

        public ActionResult Index()
        {
            if (!EsAdmin())
            {
                return RedirectToAction("Login", "Usuario");
            }

            var modelo = new AdminDashboard
            {
                TotalUsuarios = UsuarioDAO.ContarUsuarios(),
                TotalReservas = ReservaDAO.ContarReservas(),
                ReservasPendientes = ReservaDAO.ContarReservasPorEstado("Pendiente"),
                ReservasConfirmadas = ReservaDAO.ContarReservasPorEstado("Confirmada"),
                ReservasCanceladas = ReservaDAO.ContarReservasPorEstado("Cancelada")
            };

            return View(modelo);
        }

        public ActionResult Hoteles()
        {
            if (!EsAdmin())
            {
                return RedirectToAction("Login", "Usuario");
            }

            var lista = HotelDAO.ListarTodos();
            return View(lista);
        }

        [HttpGet]
        public ActionResult CrearHotel()
        {
            if (!EsAdmin())
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View(new Hotel { Activo = true });
        }

        [HttpPost]
        public ActionResult CrearHotel(Hotel model)
        {
            if (!EsAdmin())
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool ok = HotelDAO.CrearHotel(model);

            if (ok)
            {
                TempData["MensajeExito"] = "Hotel creado correctamente.";
                return RedirectToAction("Hoteles");
            }

            ViewBag.MensajeError = "No se pudo crear el hotel.";
            return View(model);
        }

        [HttpGet]
        public ActionResult EditarHotel(int id)
        {
            if (!EsAdmin())
            {
                return RedirectToAction("Login", "Usuario");
            }

            var hotel = HotelDAO.ObtenerPorId(id);

            if (hotel == null)
            {
                return RedirectToAction("Hoteles");
            }

            return View(hotel);
        }

        [HttpPost]
        public ActionResult EditarHotel(Hotel model)
        {
            if (!EsAdmin())
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool ok = HotelDAO.ActualizarHotel(model);

            if (ok)
            {
                TempData["MensajeExito"] = "Hotel actualizado correctamente.";
                return RedirectToAction("Hoteles");
            }

            ViewBag.MensajeError = "No se pudo actualizar el hotel.";
            return View(model);
        }
    }
}