using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Marca
    {

        public List<Marca> Listar()
        {
            try
            {
                List<Marca> Lista = new List<Marca>();
                ecommerce2024Entities Carrito = new ecommerce2024Entities();
                var query = Carrito.MARCA.OrderBy(micarrito => micarrito.IdMarca);
                foreach (var miObejto in query)
                {
                    Marca objMarca = new Marca();
                    objMarca.IdMarca = miObejto.IdMarca;
                    objMarca.Descripcion = miObejto.Descripcion;
                    objMarca.Activo = Convert.ToBoolean(miObejto.Activo);

                    Lista.Add(objMarca); // Agregar objeto Usuario a la lista
                }
                return Lista;
            }
            catch
            {
                List<Marca> error = new List<Marca>();
                return error;
            }
        }


        public int Registrar(Marca obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {


                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_RegistrarMarca(obj.Descripcion, obj.Activo, mensajeParam, resultadoParam);

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




        public bool Editar(Marca obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {

                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_EditarMarca(obj.IdMarca, obj.Descripcion, obj.Activo, mensajeParam, resultadoParam);

                    Mensaje = mensajeParam.Value.ToString();
                    resultado = (bool)resultadoParam.Value;

                    //... código para utilizar los valores de mensaje y resultado aquí ...
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }



        public bool Eliminar(int idMarca, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (var carrito = new ecommerce2024Entities())
                {

                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    carrito.sp_EliminarMarca(idMarca, mensajeParam, resultadoParam);

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


        public List<Marca> ListarMarcaProducto(int idCategoria)
        {
            try
            {
                List<Marca> Lista = new List<Marca>();
                ecommerce2024Entities Carrito = new ecommerce2024Entities();
                var query = Carrito.ListarMarcaProducto(idCategoria).ToList();
                foreach (var miObejto in query)
                {
                    Marca objMarca = new Marca();
                    objMarca.IdMarca = miObejto.IdMarca;
                    objMarca.Descripcion = miObejto.Descripcion;

                    Lista.Add(objMarca); // Agregar objeto Usuario a la lista
                }
                return Lista;
            }
            catch
            {
                List<Marca> error = new List<Marca>();
                return error;
            }

        }


    }
}
