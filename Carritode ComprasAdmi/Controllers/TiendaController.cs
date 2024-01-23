using CapaEntidades;
using CapaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CapaEntidades.PayPal;
using Carritode_ComprasAdmi.Filter;

namespace Carritode_ComprasAdmi.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult DetalleProducto(int IdProducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;
            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == IdProducto).FirstOrDefault();
            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.CovertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }
            return View(oProducto);
        }


        [HttpGet]
        public ActionResult ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ListarMarcaProducto(int IdCategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaProducto(IdCategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult ListarProducto(int IdCategoria, int IdMarca)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

           
            lista = new CN_Producto().Listar()
            .Where(p => (IdCategoria == 0 || p.Categoria.IdCategoria == IdCategoria) &&
                (IdMarca == 0 || p.Marca.IdMarca == IdMarca) &&
                p.Stock > 0 && p.Activo)
                .Select(p => new Producto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Marca = p.Marca,
                    Categoria = p.Categoria,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    RutaImagen = p.RutaImagen,
                    Base64 = CN_Recursos.CovertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                    Extension = Path.GetExtension(p.NombreImagen),
                    Activo = p.Activo
                })
           .ToList();


            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;

        }


        [HttpPost]
        public ActionResult AgregarCarrito(int IdProducto)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool Exixte = new CN_Carrito().ExisteCarrito(IdCliente, IdProducto);
            bool Respuesta = false;
            string nensaje = string.Empty;

            if (Exixte)
            {
                nensaje = "El producto ya existe en el carrito";
            }
            else
            {
                Respuesta = new CN_Carrito().OperacionCarrito(IdCliente, IdProducto, true, out nensaje);
            }

            return Json(new { Respuesta = Respuesta, Mensaje = nensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult CantidadCarrito()
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            int Cantidad = new CN_Carrito().CantidadCarrito(IdCliente);
            return Json(new { Cantidad = Cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarProductosCarrito()
        {
            List<Carrito> listaCarrito = new List<Carrito>();

            if (Session["Cliente"] != null)
            {
                int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

                listaCarrito = new CN_Carrito().ListarProducto(idCliente)
                    .Select(oc => new Carrito()
                    {
                        oProducto = new Producto()
                        {
                            IdProducto = oc.oProducto.IdProducto,
                            Nombre = oc.oProducto.Nombre,
                            Marca = oc.oProducto.Marca,
                            Precio = oc.oProducto.Precio,
                            RutaImagen = oc.oProducto.RutaImagen,
                            Base64 = CN_Recursos.CovertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out bool conversion),
                            Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                        },
                        Cantidad = oc.Cantidad
                    })
                    .ToList();
            }

            return Json(new { data = listaCarrito }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult OperacionCarrito(int IdProducto, bool Sumar)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool Respuesta = false;
            string nensaje = string.Empty;
            Respuesta = new CN_Carrito().OperacionCarrito(IdCliente, IdProducto, true, out nensaje);
            return Json(new { Respuesta = Respuesta, Mensaje = nensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult EliminarCarrito(int IdProducto)
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool Respuesta = false;
            string nensaje = string.Empty;
            Respuesta = new CN_Carrito().EliminarCarrito(IdCliente, IdProducto);
            return Json(new { Respuesta = Respuesta, Mensaje = nensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();
            oLista = new CN_Ubicacion().ListarDepartamento();
            return Json(new { Lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ObtenerProvincia(String IdDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = new CN_Ubicacion().ListarProvincia(IdDepartamento);
            return Json(new { Lista = oLista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult ObtenerDistrito(String IdDepartamento, String IdProvincia)
        {
            List<Distrito> oLista = new List<Distrito>();
            oLista = new CN_Ubicacion().ListarDistrito(IdDepartamento, IdProvincia);
            return Json(new { Lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [ValidarSesionAtribute]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;
            DataTable listaDetalleVenta = new DataTable();
            listaDetalleVenta.Locale = new CultureInfo("es-PE");
            listaDetalleVenta.Columns.Add("IdProducto", typeof(string));
            listaDetalleVenta.Columns.Add("Cantidad", typeof(int));
            listaDetalleVenta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();

            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal subTotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.Precio;
                total += subTotal;

                oListaItem.Add(new Item() {
                    name = oCarrito.oProducto.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = oCarrito.oProducto.Precio.ToString("G", new CultureInfo("es-PE"))
                    }
                });

                listaDetalleVenta.Rows.Add(new object[]
                {
                    oCarrito.oProducto.IdProducto,
                    oCarrito.Cantidad,
                    subTotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-PE")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-PE")),
                        }
                    }
                },
                description = "compra de articulo de mi tienda",
                items = oListaItem
            };

            Checkout_Order ocheckout_Order = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>()
                {
                    purchaseUnit
                },
                application_context = new ApplicationContext
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44315/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44315/Tienda/Carrito"

                }
            };

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = listaDetalleVenta;

            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Checkout> response_Paypal = new Response_Paypal<Response_Checkout>();
            response_Paypal = await opaypal.CrearSolicitud(ocheckout_Order);


            

            return Json(response_Paypal, JsonRequestBehavior.AllowGet);

        }



        [ValidarSesionAtribute]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();
            response_Paypal = await opaypal.AprobarPago(token);
            

            ViewData["Status"] = response_Paypal.Status;

            if (response_Paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable DetalleVenta = (DataTable)TempData["DetalleVenta"];
                oVenta.IdTransaccion = response_Paypal.Response.purchase_units[0].payments.captures[0].id;
                string mensaje = string.Empty;
                bool repuesta = new CN_Venta().Registrar(oVenta, DetalleVenta, out mensaje);
                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return await Task.FromResult(View());
        }


        [ValidarSesionAtribute]
        [Authorize]
        public ActionResult MisCompras()
        {
            List<DetalleVenta> listaventa = new List<DetalleVenta>();

            if (Session["Cliente"] != null)
            {
                int idCliente = ((Cliente)Session["Cliente"]).IdCliente;

                listaventa = new CN_Venta().ListarCompras(idCliente)
                    .Select(oc => new DetalleVenta()
                    {
                        oProducto = new Producto()
                        {
                            
                            Nombre = oc.oProducto.Nombre,
                            Precio = oc.oProducto.Precio,
                            Base64 = CN_Recursos.CovertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out bool conversion),
                            Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                        },
                        Cantidad = oc.Cantidad,
                        Total = oc.Total,
                        IdTransaccion = oc.IdTransaccion,
                    })
                    .ToList();
            }

            return View(listaventa);
        }

    }
}