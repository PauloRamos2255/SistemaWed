
using Productos = CapaEntidades.Producto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Productos> Listar()
        {
            try
            {
                List<Productos> objLista = new List<Productos>();
                ecommerce2024Entities Prestamos = new ecommerce2024Entities();

                var query = Prestamos.ListarProducto().ToList();

                foreach (var miObjeto in query)
                {
                    Productos objProducto = new Productos();
                    objProducto.IdProducto = miObjeto.IdProducto;
                    objProducto.Nombre = miObjeto.Nombre;
                    objProducto.Descripcion = miObjeto.Descripcion;
                    objProducto.Marca = new Marca { IdMarca = miObjeto.IdMarca, Descripcion = miObjeto.DesMarca };
                    objProducto.Categoria = new Categoria { IdCategoria = miObjeto.IdCategoria, Descripcion = miObjeto.DesCategoria };
                    objProducto.Precio = Convert.ToDecimal(miObjeto.Precio);
                    objProducto.Stock = Convert.ToInt16(miObjeto.Stock);
                    objProducto.RutaImagen = miObjeto.RutaImagen;
                    objProducto.NombreImagen = miObjeto.NombreImagen;
                    objProducto.Activo = Convert.ToBoolean(miObjeto.Activo);

                    objLista.Add(objProducto);
                }

                return objLista;
            }
            catch (EntityException ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public int Registrar(Productos obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {


                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_RegistrarProducto(obj.Nombre, obj.Descripcion, obj.Marca.IdMarca,  obj.Categoria.IdCategoria, obj.Precio, obj.Stock, obj.Activo, mensajeParam, resultadoParam);

                    idautogenerado = (int)resultadoParam.Value;
                    Mensaje = mensajeParam.Value.ToString();


                    //... código para utilizar los valores de mensaje y resultado aquí ...
                }
            }
            catch (Exception x)
            {
                idautogenerado = 0;
                Mensaje = x.Message;
            }

            return idautogenerado;
        }



        ecommerce2024Entities carrito = new ecommerce2024Entities();

        public bool GuardarDatosImagen(Productos obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                Producto objProductos = ( from miProductos in carrito.Producto.Where( miProductos => miProductos.IdProducto == obj.IdProducto)
                                                 select miProductos).FirstOrDefault();
                objProductos.IdProducto = obj.IdProducto;
                objProductos.NombreImagen = obj.NombreImagen;
                objProductos.RutaImagen = obj.RutaImagen;

                if (carrito.SaveChanges() > 0)
                {
                    resultado = true;
                }
                else
                {
                    Mensaje = "No se pudo actulizar imagen";
                }
       
            }
            catch (Exception X)
            {

                resultado = false;
                Mensaje = X.Message;
            }
            return resultado;

        }

        public bool Editar(Productos obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(int));

                    db.usp_EditarProducto(obj.IdProducto, obj.Nombre, obj.Descripcion, obj.Marca.IdMarca, obj.Categoria.IdCategoria, obj.Precio, obj.Stock, obj.Activo, mensajeParam, resultadoParam);

                    Mensaje = mensajeParam.Value.ToString();
                    resultado = (int)(resultadoParam.Value)== 1; 
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }





        public bool Eliminar(int idProductos, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (var carrito = new ecommerce2024Entities())
                {

                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    carrito.sp_EliminarProducto(idProductos, mensajeParam, resultadoParam);

                    mensaje = mensajeParam.Value.ToString();
                    resultado = (bool)resultadoParam.Value;
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = "Ocurrió un error al eliminar el usuario: " + ex.Message;
            }

            return resultado;
        }
    }

}

