using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public DashBoard Listar()
        {
            try
            {
                ecommerce2024Entities carrito = new ecommerce2024Entities();
                var result = carrito.sp_ReporteDashboard().FirstOrDefault();

                DashBoard objDashBoard = new DashBoard();
                objDashBoard.TotalCliente = Convert.ToInt32(result.TotalCliente);
                objDashBoard.TotalVenta = Convert.ToInt32(result.TotalVenta);
                objDashBoard.TotalProducto = Convert.ToInt32(result.TotalProducto);

                return objDashBoard;
            }
            catch
            {
                return null;
            }
        }




        public List<Reporte> ListarReporte(string fechaInicio, string fechaFin, string idTransaccion)
        {
            try
            {
                using (var carrito = new ecommerce2024Entities()) // Replace "YourEntities" with the name of your EF context
                {
                    var result = carrito.sp_ReporteVentas(fechaInicio, fechaFin, idTransaccion).ToList();

                    // Convert the result to a list of ReporteobjReporte objects
                    List<Reporte> objListaReportes = new List<Reporte>();
                    foreach (var miObjeto in result)
                    {

                        Reporte objReporte = new Reporte();
                        objReporte.FechaVenta = miObjeto.FechaVenta;
                        objReporte.Cliente = miObjeto.Cliente;
                        objReporte.Producto = miObjeto.Producto;
                        objReporte.Precio = Convert.ToDecimal(miObjeto.Precio);
                        objReporte.Cantidad = Convert.ToInt32(miObjeto.Cantidad);
                        objReporte.Total = Convert.ToDecimal(miObjeto.Total);
                        objReporte.IdTransaccion = miObjeto.IdTransaccion;
                        objListaReportes.Add(objReporte);
                    }

                    return objListaReportes;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

