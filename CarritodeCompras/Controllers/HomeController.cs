using CapaEntidades;
using CapaNegocio;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarritodeCompras.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuario()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if(objeto.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public JsonResult ListarReporte()
        {
            DashBoard oLista = new CN_Reporte().Listar();
            return Json(new { resultado = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarReporteVenta( string fechainicio , string fechafin , string transaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().ListarReporte(fechainicio , fechafin , transaccion);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string transaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().ListarReporte(fechainicio, fechafin, transaccion);
            DataTable dt = new DataTable();
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach (Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
            rp.FechaVenta,
            rp.Cliente,
            rp.Producto,
            rp.Precio,
            rp.Cantidad,
            rp.Total,
            rp.IdTransaccion
                });
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Reporte");

                // Escribe los datos en la hoja de trabajo
                worksheet.Cells.LoadFromDataTable(dt, true);

                // Verifica si hay datos antes de acceder a la tabla
                if (dt.Rows.Count > 0)
                {
                    // Aplica formato a la tabla
                    worksheet.Tables[0].TableStyle = TableStyles.Medium9;
                }

                // Establece el ancho de las columnas
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(3).AutoFit();
                worksheet.Column(4).AutoFit();
                worksheet.Column(5).AutoFit();
                worksheet.Column(6).AutoFit();
                worksheet.Column(7).AutoFit();

                // Configura el encabezado
                worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Reporte de Ventas";


                // Guarda el archivo
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta.xlsx");
            }

        }





    }
}