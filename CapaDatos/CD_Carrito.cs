using Productos = CapaEntidades.Producto;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int IdCliente , int IdCarrito)
        {
            bool Resultado = true;

            try
            {
                using (var db = new ecommerce2024Entities())
                {   
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_ExisteCarrito(IdCliente, IdCarrito  , resultadoParam);

                    Resultado = Convert.ToBoolean(resultadoParam.Value);
                 
                    //... código para utilizar los valores de mensaje y resultado aquí ...
                }
            }
            catch (Exception)
            {
                Resultado = false;
            }

            return Resultado;
        }

        public bool OperacionCarrito(int IdCliente, int IdCarrito, bool Sumar, out string Mensaje)
        {
            bool Resultado = true;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_OperacionCarrito(IdCliente, IdCarrito, Sumar, mensajeParam, resultadoParam);

                    Resultado = Convert.ToBoolean(resultadoParam.Value);
                    Mensaje = mensajeParam.Value.ToString();
                }
            }
            catch (Exception x)
            {
                Resultado = false;
                Mensaje = x.Message;
            }
            return Resultado;
        }


        public int CantidadCarrito(int IdCliente)
        {
            int resultado = 0;
            try
            {
                using (var carrito = new ecommerce2024Entities())
                {
                    var result = carrito.sp_CantidadCarrito(IdCliente).FirstOrDefault();
                    if (result != null)
                    {
                        resultado = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception)
            {
                resultado = 0;
            }

            return resultado;
        }


        public List<Carrito> ListarProducto(int idCliente)
        {
            try
            {
                using (var context = new ecommerce2024Entities())
                {
                    var query = context.fn_ObtenerCarritoCliente(idCliente);
                    var lista = query.Select(x => new Carrito
                    {
                        oProducto = new Productos
                        {
                            IdProducto = x.IdProducto,
                            Nombre = x.Nombre,
                            Precio = (decimal)x.Precio,
                            RutaImagen = x.RutaImagen,
                            NombreImagen = x.NombreImagen,
                            Marca = new Marca { Descripcion = x.DesMarca },
                        },
                        Cantidad = (int)x.Cantidad
                    }).ToList();

                    return lista;
                }
            }
            catch
            {
                return new List<Carrito>();
            }
        }





        public bool EliminarCarrito(int IdCliente, int IdProducto)
        {
            bool Resultado = true;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_EliminarCarrito(IdCliente, IdProducto, resultadoParam);

                    Resultado = Convert.ToBoolean(resultadoParam.Value);

                    //... código para utilizar los valores de mensaje y resultado aquí ...
                }
            }
            catch (Exception)
            {
                Resultado = false;
            }

            return Resultado;
        }




    }
}
