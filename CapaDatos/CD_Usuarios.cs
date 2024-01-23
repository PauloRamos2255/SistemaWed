using CapaEntidades;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity.Core;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Data.Entity.Core.Objects;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            try
            {
                List<Usuario> Lista = new List<Usuario>();
                ecommerce2024Entities Carrito = new ecommerce2024Entities();
                var query = Carrito.USUARIO.OrderBy(micarrito => micarrito.IdUsuario);
                foreach (var miObejto in query)
                {
                    Usuario objUsuario = new Usuario();
                    objUsuario.IdUsuario = Convert.ToInt16(miObejto.IdUsuario);
                    objUsuario.Nombres = miObejto.Nombres;
                    objUsuario.Apellidos = miObejto.Apellidos;
                    objUsuario.Correo = miObejto.Correo;
                    objUsuario.Clave = miObejto.Clave;
                    objUsuario.Restablecer = Convert.ToBoolean(miObejto.Reestablecer);
                    objUsuario.Activo = Convert.ToBoolean(miObejto.Activo);
                    Lista.Add(objUsuario); // Agregar objeto Usuario a la lista
                }
                return Lista;
            }
            catch
            {
                List<Usuario> error = new List<Usuario>();
                return error;
            }

        }

        ecommerce2024Entities carrito = new ecommerce2024Entities();

        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    var resultado = new ObjectParameter("Resultado", typeof(int));
                    var mensaje = new ObjectParameter("Mensaje", typeof(string));

                    db.sp_RegistrarUsuario(
                        obj.Nombres,
                        obj.Apellidos,
                        obj.Correo,
                        obj.Clave,
                        obj.Activo,
                        obj.id_roles,
                        obj.numero,
                        mensaje,
                        resultado
                    );
                    idautogenerado = (int)resultado.Value;
                    Mensaje = (string)mensaje.Value;
                }
            }
            catch (Exception x)
            {
                idautogenerado = 0;
                Mensaje = x.Message;
            }

            return idautogenerado;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (var db = new ecommerce2024Entities())
                {
                    ObjectParameter mensajeParam = new ObjectParameter("Mensaje", typeof(string));
                    ObjectParameter resultadoParam = new ObjectParameter("Resultado", typeof(bool));

                    db.sp_EditarUsuario(obj.IdUsuario, obj.Nombres, obj.Apellidos, obj.Correo, obj.Activo, obj.id_roles,
                        obj.numero, mensajeParam, resultadoParam);

                    Mensaje = mensajeParam.Value.ToString();
                    resultado = (bool)resultadoParam.Value;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }


        public bool Eliminar(int idUsuario, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using(var db = new ecommerce2024Entities())
                {
                    var usuarioAEliminar = db.USUARIO.FirstOrDefault(u => u.IdUsuario == idUsuario);
                    if (usuarioAEliminar != null)
                    {
                        db.USUARIO.Remove(usuarioAEliminar);
                        db.SaveChanges();
                        resultado = true;
                        mensaje = "Usuario eliminado correctamente";
                    }
                    else
                    {
                        mensaje = "No se encontró el usuario a eliminar";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = "Ocurrió un error al eliminar el usuario: " + ex.Message;
            }

            return resultado;
        }




        public bool CambiarClave(int IdUsuario, string Nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(Nuevaclave))
                {
                    throw new ArgumentException("La nueva clave no puede estar vacía");
                }

                using (ecommerce2024Entities carrito = new ecommerce2024Entities())
                {
                    USUARIO Usuario = carrito.USUARIO.FirstOrDefault(u => u.IdUsuario == IdUsuario);
                    if (Usuario == null)
                    {
                        throw new ArgumentException($"No se encontró ningún usuario con el Id {IdUsuario}");
                    }

                    Usuario.Clave = Nuevaclave;
                    Usuario.Reestablecer = false;

                    if (carrito.SaveChanges() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la clave del usuario";
                    }
                }
            }
            catch (Exception x)
            {
                resultado = false;
                Mensaje = x.Message;
            }
            return resultado;
        }


        public bool ReestablecerClave(int IdUsuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(clave))
                {
                    throw new ArgumentException("La nueva clave no puede estar vacía");
                }

                using (ecommerce2024Entities carrito = new ecommerce2024Entities())
                {
                    USUARIO Usuario = carrito.USUARIO.FirstOrDefault(u => u.IdUsuario == IdUsuario);
                    if (Usuario == null)
                    {
                        throw new ArgumentException($"No se encontró ningún usuario con el Id {IdUsuario}");
                    }

                    Usuario.Clave = clave;
                    Usuario.Reestablecer = true;

                    if (carrito.SaveChanges() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la clave del usuario";
                    }
                }
            }
            catch (Exception x)
            {
                resultado = false;
                Mensaje = x.Message;
            }
            return resultado;
        }







        //public bool Eliminar( int id ,out string Mensaje)
        //{
        //    bool resultado =false;
        //    Mensaje = string.Empty;
        //    try
        //    {
        //        using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
        //        {
        //            SqlCommand cnd = new SqlCommand("delete top (1) from usuario where IdUsuario @id", oconexion);
        //            cnd.Parameters.AddWithValue("@id", id);
        //            cnd.CommandType = CommandType.Text;
        //            oconexion.Open();
        //            resultado = cnd.ExecuteNonQuery() > 0 ? true : false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //        Mensaje = ex.Message;
        //    }
        //    return resultado;
    }



    //public bool Editar(Usuario obj, out string Mensaje)
    //{
    //    bool resultado = false;
    //    Mensaje  = string.Empty;
    //    try
    //    {
    //        using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
    //        {
    //            SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
    //            cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
    //            cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
    //            cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
    //            cmd.Parameters.AddWithValue("Correo", obj.Correo); 
    //            cmd.Parameters.AddWithValue("Activo", obj.Activo);
    //            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
    //            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection. Output;
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            oconexion.Open();

    //            cmd.ExecuteNonQuery();
    //            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
    //            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
    //        }
    //    }
    //    catch (Exception x)
    //    {

    //        resultado = false;
    //        Mensaje = x.Message;

    //    }
    //    return resultado;
    //}






    //public int Registrar(Usuario obj, out string Mensaje)
    //{
    //    int idautogenerado = 0;
    //    Mensaje = string.Empty;
    //    try
    //    {
    //        using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
    //        {
    //            SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
    //            cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
    //            cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
    //            cmd.Parameters.AddWithValue("Correo", obj.Correo);
    //            cmd.Parameters.AddWithValue("Clave", obj.Clave);
    //            cmd.Parameters.AddWithValue("Activo", obj.Activo);
    //            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
    //            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

    //            oconexion.Open();
    //            cmd.ExecuteNonQuery();

    //            idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value); 
    //            Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

    //        }

    //    }
    //    catch (Exception x )
    //    {

    //       idautogenerado = 0;
    //        Mensaje = x.Message;
    //    }
    //    return idautogenerado;
    //}
}









