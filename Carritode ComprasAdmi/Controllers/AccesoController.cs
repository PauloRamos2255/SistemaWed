using CapaEntidades;
using CapaNegocio;
using Microsoft.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;


namespace Carritode_ComprasAdmi.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apelllidos) ? "" : objeto.Apelllidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
             }

            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if(resultado > 0)
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

        [HttpPost]

        public ActionResult Index(string correo , string clave)
        {
            Cliente oCliente = new Cliente();

            //string tokenRecaptcha = Request.Form["g-recaptcha-response"];

            //// Verificar el token reCAPTCHA
            //bool isValidRecaptcha = VerifyRecaptcha(tokenRecaptcha);

            //if (!isValidRecaptcha)
            //{
            //    ViewBag.Error = "Por favor, completa la verificación reCAPTCHA.";
            //    return View();
            //}

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConverttirSha256(clave)).FirstOrDefault();
            
            if(oCliente == null)
            {
                ViewBag.Error ="Correo o contraseña no son correctas";
                return View();
            }
            else
            {
                if (oCliente.Restablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }


        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Cliente oCliente = new Cliente();
            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (oCliente == null)
            {
                ViewBag.Error = "No se encontro un Cliente relacionado con el correo";
                return View();
            }
            string mensaje = string.Empty;
            bool repuesta = new CN_Cliente().ReestablecerClave(oCliente.IdCliente, correo, out mensaje);
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

        [HttpPost]
        public ActionResult CambiarClave(string IdCliente , string clave, string nuevaclave, string comfirmarclave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(IdCliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConverttirSha256(clave))
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es la correcta ";
                return View();
            }
            else if (nuevaclave != comfirmarclave)
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vclave"] = clave;
                ViewBag.Error = "Las contraseñas no coninciden ";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConverttirSha256(nuevaclave);
            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(IdCliente), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                TempData["IdCliente"] = IdCliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }


        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }


        private bool VerifyRecaptcha(string token)
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            string recaptchaUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(recaptchaUrl);
                dynamic jsonResponse = JsonConvert.DeserializeObject(response);

                // Verificar si la respuesta es válida
                if (jsonResponse.success == "true")
                {
                    return true;
                }
            }

            return false;
        }


    }
}