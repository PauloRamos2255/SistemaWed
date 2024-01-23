using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GnerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }


        //encriptacion de Texto en SHA256
        public static string ConverttirSha256 (string texto)
        {
            StringBuilder Sb = new StringBuilder ();
            using(SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString ();
        }

        public static bool EnviarCorreo (string correo , string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("anuelitoramos@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;
                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("anuelitoramos@gmail.com", "bhywfqeoacmuaoyi"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }


        public static string CovertirBase64(string ruta , out bool convertion)
        {
            string textobase64 = string.Empty;
            convertion = true;
            try
            {
                Byte[] bytes = File.ReadAllBytes(ruta);
                textobase64 = Convert.ToBase64String(bytes);
            }
            catch 
            {
                convertion = false;
            }
            return textobase64;
        }


    }
}
