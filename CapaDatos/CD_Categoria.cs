using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace CapaDatos
{
    public class CD_Categoria
    {

        public List<Categoria> Listar()
        {
            try
            {
                List<Categoria> Lista = new List<Categoria>();
                ecommerce2024Entities Carrito = new ecommerce2024Entities();
                var query = Carrito.CATEGORIA.OrderBy(micarrito => micarrito.IdCategoria);
                foreach (var miObejto in query)
                {
                    Categoria objCategoria = new Categoria();
                    objCategoria.IdCategoria = miObejto.IdCategoria;
                    objCategoria.Descripcion = miObejto.Descripcion;
                    objCategoria.Activo = Convert.ToBoolean(miObejto.Activo);
                    
                    Lista.Add(objCategoria); // Agregar objeto Usuario a la lista
                }
                return Lista;
            }
            catch
            {
                List<Categoria> error = new List<Categoria>();
                return error;
            }

        }


        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
        

                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_RegistrarCategoria( obj.Descripcion,obj.Activo, mensajeParam, resultadoParam);

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




        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    
                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_EditarCategoria (obj.IdCategoria , obj.Descripcion, obj.Activo, mensajeParam, resultadoParam);

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



        public bool Eliminar(int idCategoria, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (var carrito = new ecommerce2024Entities())
                {
                    
                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    carrito.sp_EliminarCategoria(idCategoria, mensajeParam, resultadoParam);

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
