using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {

        private CD_Cliente OBJCapaDato = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return OBJCapaDato.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apelllidos) || string.IsNullOrWhiteSpace(obj.Apelllidos))
            {
                Mensaje = "El apellido del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del Cliente no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                //string clase = CN_Recursos.GnerarClave();
                string asunto = "Creacion de Cuenta";
                string Mensajecorreo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es : !clave!</p>";
                Mensajecorreo = Mensajecorreo.Replace("!clave!", obj.Clave);
                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, Mensajecorreo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConverttirSha256(obj.Clave);
                    return OBJCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        public bool CambiarClave(int IdCliente, string Nuevaclave, out string Mensaje)
        {
            return OBJCapaDato.CambiarClave(IdCliente, Nuevaclave, out Mensaje);
        }


        public bool ReestablecerClave(int IdCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuvaclase = CN_Recursos.GnerarClave();
            bool resultado = OBJCapaDato.ReestablecerClave(IdCliente, CN_Recursos.ConverttirSha256(nuvaclase), out Mensaje);

            if (resultado)
            {
                string asunto = "Restablecimiento de Contraseña";
                string Mensajecorreo = "<h3>Su cuenta fue restablecida correctamente</h3></br><p>Su contraseña para acceder ahoora es : !clave!</p>";
                Mensajecorreo = Mensajecorreo.Replace("!clave!", nuvaclase);
                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, Mensajecorreo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                return false;
            }


        }



    }
}
