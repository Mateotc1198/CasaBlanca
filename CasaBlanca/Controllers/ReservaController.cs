using CasaBlanca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CasaBlanca.Controllers
{
    public class ReservaController : Controller
    {
        // El flujo de reservas depende de la sesión activa para asociar la reserva al usuario correcto.
        private bool UsuarioLogueado()
        {
            return Session["IdUsuario"] != null;
        }

        // Permite precargar la reserva desde el listado de hoteles para mejorar la experiencia del usuario.
        [HttpGet]
        public ActionResult Nueva(int? idHotel, string entrada, string salida, string huespedes)
        {
            if (!UsuarioLogueado())
            {
                return RedirectToAction("Login", "Usuario");
            }

            DateTime? fechaEntrada = null;
            DateTime? fechaSalida = null;

            if (DateTime.TryParse(entrada, out DateTime entradaParseada))
            {
                fechaEntrada = entradaParseada;
            }

            if (DateTime.TryParse(salida, out DateTime salidaParseada))
            {
                fechaSalida = salidaParseada;
            }

            var modelo = new Reserva
            {
                IdHotel = idHotel ?? 0,
                Entrada = fechaEntrada,
                Salida = fechaSalida,
                Huespedes = huespedes
            };

            if (idHotel.HasValue)
            {
                Hotel hotel = HotelDAO.ObtenerPorId(idHotel.Value);

                if (hotel != null)
                {
                    // Se copian los datos del hotel al modelo para evitar depender del formulario
                    // en cálculos de precio o en datos que no deben venir manipulados por el cliente.
                    modelo.NombreHotel = hotel.Nombre;
                    modelo.Destino = hotel.Ciudad;
                    modelo.PrecioPorNoche = hotel.PrecioPorNoche;
                    modelo.ImagenHotel = hotel.Imagen;

                    if (modelo.Entrada.HasValue && modelo.Salida.HasValue)
                    {
                        modelo.Noches = (modelo.Salida.Value - modelo.Entrada.Value).Days;
                        if (modelo.Noches < 1)
                        {
                            modelo.Noches = 1;
                        }

                        // El número de huéspedes se valida en la vista y en el modelo; aquí se hace
                        // una normalización mínima para no romper la precarga del formulario.
                        int personas = 1;
                        int.TryParse(modelo.Huespedes, out personas);
                        if (personas < 1)
                        {
                            personas = 1;
                        }

                        modelo.Total = modelo.Noches * hotel.PrecioPorNoche;
                    }
                }
            }

            ViewBag.Hoteles = HotelDAO.ListarHotelesActivos();
            return View(modelo);
        }

        // Recalcula los datos sensibles en servidor antes de guardar para no confiar en valores posteados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Reserva model)
        {
            if (!UsuarioLogueado())
            {
                return RedirectToAction("Login", "Usuario");
            }

            Hotel hotel = HotelDAO.ObtenerPorId(model.IdHotel);
            ViewBag.Hoteles = HotelDAO.ListarHotelesActivos();


            if (hotel != null)
            {
                model.NombreHotel = hotel.Nombre;
                model.Destino = hotel.Ciudad;
                model.PrecioPorNoche = hotel.PrecioPorNoche;
                model.ImagenHotel = hotel.Imagen;

                if (model.Entrada.HasValue && model.Salida.HasValue)
                {
                    model.Noches = (model.Salida.Value - model.Entrada.Value).Days;

                    if (model.Noches < 1)
                    {
                        ModelState.AddModelError("Salida", "La fecha de salida debe ser mayor que la fecha de entrada.");
                    }

                    model.Total = model.Noches * hotel.PrecioPorNoche;
                }
            }

            if (!ModelState.IsValid)
            {
                return View("Nueva", model);
            }

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
            bool ok = ReservaDAO.Guardar(model, idUsuario);

            if (!ok)
            {
                ViewBag.MensajeError = "No se pudo guardar la reserva.";
                return View("Nueva", model);
            }

            ViewBag.Mensaje = "Reserva guardada correctamente.";
            ViewBag.Hoteles = HotelDAO.ListarHotelesActivos();

            ModelState.Clear();
            return View("Nueva", new Reserva());
        }
        // Muestra solo las reservas del usuario autenticado para mantener aislamiento entre cuentas.
        public ActionResult MisReservas()
        {
            if (!UsuarioLogueado())
            {
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
            var lista = ReservaDAO.ListarReservasPorUsuario(idUsuario);

            return View(lista);
        }
        [HttpPost]
        public ActionResult Cancelar(int idReserva)
        {
            if (!UsuarioLogueado())
            {
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);
            // La validación final también se hace en la capa DAO para asegurar que solo se cancelen
            // reservas propias y en estado pendiente.
            bool ok = ReservaDAO.CancelarReservaUsuario(idReserva, idUsuario);

            if (ok)
            {
                TempData["MensajeExito"] = "Reserva cancelada correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cancelar la reserva. Solo puedes cancelar reservas pendientes.";
            }

            return RedirectToAction("MisReservas");
        }
    }
}
