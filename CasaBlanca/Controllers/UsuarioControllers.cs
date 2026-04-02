using System;
using System.Linq;
using System.Web.Mvc;
using CasaBlanca.Models;

namespace CasaBlanca.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }
        // El registro delega en el DAO la verificación de duplicados y el hash de la contraseña.

        [HttpPost]
        public ActionResult Registrar(Usuario usuario)
        {
            if (usuario == null)
            {
                ViewBag.MensajeError = "Datos de usuario no recibidos.";
                return View();
            }

            if (string.IsNullOrEmpty(usuario.Telefono) || !usuario.Telefono.All(char.IsDigit))
            {
                ViewBag.MensajeError = "El teléfono solo debe contener números.";
                return View(usuario);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool registrado = UsuarioDAO.Registrar(usuario);

                    if (registrado)
                    {
                        TempData["MensajeExito"] = "¡Registro exitoso! Ahora puedes iniciar sesión.";
                        return RedirectToAction("RegistroExitoso");
                    }
                    else
                    {
                        ViewBag.MensajeError = "Error al registrar usuario. El usuario o correo ya existen.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.MensajeError = "Error en el sistema: " + ex.Message;
                }
            }

            return View(usuario);
        }

        //Muestra formulario de inicio de sesión
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        // La sesión guarda únicamente los datos necesarios para navegación y autorización por rol.
        [HttpPost]
        public ActionResult Login(string user, string clave)
        {
            try
            {
                Usuario usuario = UsuarioDAO.ValidarLogin(user, clave);

                if (usuario != null)
                {
                    Session["IdUsuario"] = usuario.IdUsuario;
                    Session["NombreUsuario"] = usuario.Nombre;
                    Session["User"] = usuario.User;
                    Session["EsAdmin"] = usuario.EsAdmin;

                    if (usuario.EsAdmin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.MensajeError = "Usuario o contraseña incorrectos.";
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = "Error en el sistema: " + ex.Message;
            }

            return View();
        }

        // Limpia por completo la sesión para evitar reutilizar datos del usuario anterior.
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult RegistroExitoso()
        {
            return View();
        }
    }
}
