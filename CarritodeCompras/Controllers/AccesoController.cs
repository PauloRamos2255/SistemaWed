using CapaEntidades;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security;
using System.Web.Security;

namespace CarritodeCompras.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }


        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConverttirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcto";
                return View();
            }
            else
            {
                if (oUsuario.Restablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpPost]
        public ActionResult CambiarClave(string IdUsuario, string clave, string nuevaclave, string comfirmarclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(IdUsuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConverttirSha256(clave))
            {
                TempData["IdUsuario"] = IdUsuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es la correcta ";
                return View();
            }
            else if (nuevaclave != comfirmarclave)
            {
                TempData["IdUsuario"] = IdUsuario;
                ViewData["vclave"] = clave;
                ViewBag.Error = "Las contraseñas no coninciden ";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConverttirSha256(nuevaclave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(IdUsuario), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                TempData["IdUsuario"] = IdUsuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario ousuario = new Usuario();
            ousuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if(ousuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado con el correo";
                return View();
            }
            string mensaje = string.Empty;
            bool repuesta = new CN_Usuarios().ReestablecerClave(ousuario.IdUsuario, correo, out mensaje);
            if (repuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }


    }
}