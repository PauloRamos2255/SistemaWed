using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Productos = CapaEntidades.Producto;

namespace CapaDatos
{
    public class CD_Venta
    {

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;

                    var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int);
                    resultadoParam.Direction = ParameterDirection.Output;

                    var detallesVentaParam = new SqlParameter("@DetallesVenta", DetalleVenta);
                    detallesVentaParam.SqlDbType = SqlDbType.Structured;
                    detallesVentaParam.TypeName = "dbo.DetallesVentaTableType";

                    db.Database.ExecuteSqlCommand("usp_InsertarVenta @IdCliente, @TotalProducto, @MontoTotal, @Contacto, @IdDistrito, @Telefono, @Direccion, @IdTransaccion, @DetallesVenta, @Mensaje OUT, @Resultado OUT",
                        new SqlParameter("@IdCliente", obj.IdCliente),
                        new SqlParameter("@TotalProducto", obj.TotalProducto),
                        new SqlParameter("@MontoTotal", obj.MontoTotal),
                        new SqlParameter("@Contacto", obj.Contacto),
                        new SqlParameter("@IdDistrito", obj.IdDistrito),
                        new SqlParameter("@Telefono", obj.Telefono),
                        new SqlParameter("@Direccion", obj.Direccion),
                        new SqlParameter("@IdTransaccion", obj.IdTransaccion),
                        detallesVentaParam,
                        mensajeParam,
                        resultadoParam);

                    resultado = Convert.ToBoolean(resultadoParam.Value);
                    Mensaje = mensajeParam.Value.ToString();

                    //... código para utilizar los valores de mensaje y resultado aquí ...
                }
            }
            catch (Exception x)
            {
                resultado = false;
                Mensaje = x.Message;
            }

            return resultado;
        }



        public List<DetalleVenta> ListarCompras(int idCliente)
        {
            try
            {
                using (var context = new ecommerce2024Entities())
                {
                    var query = context.fn_ListarComprar(idCliente);
                    var lista = query.Select(x => new DetalleVenta
                    {
                        oProducto = new Productos
                        {
                            Nombre = x.Nombre,
                            Precio = (decimal)x.Precio,
                            RutaImagen = x.RutaImagen,
                            NombreImagen = x.NombreImagen
                        },
                        Cantidad = (int)x.Cantidad,
                        Total = (decimal)x.Total,
                        IdTransaccion = x.IdTransaccion
                    }).ToList();

                    return lista;
                }
            }
            catch
            {
                return new List<DetalleVenta>();
            }
        }




    }
}
