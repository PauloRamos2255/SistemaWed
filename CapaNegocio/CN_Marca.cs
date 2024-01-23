using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {

        private CD_Marca OBJCapaDato = new CD_Marca();

        public List<Marca> Listar()
        {
            return OBJCapaDato.Listar();
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion de la Categoria no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return OBJCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }

        }


        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion de la Categoria no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return OBJCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return OBJCapaDato.Eliminar(id, out Mensaje);
        }

        public List<Marca> ListarMarcaProducto(int IdCategoria)
        {
            return OBJCapaDato.ListarMarcaProducto(IdCategoria);
        }

    }
}
