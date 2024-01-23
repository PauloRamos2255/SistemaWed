using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Carrito
    {

        private CD_Carrito OBJCapaDato = new CD_Carrito();

        public bool ExisteCarrito(int IdCliente, int IdProducto)
        {
            return OBJCapaDato.ExisteCarrito(IdCliente, IdProducto);
        }

        public bool OperacionCarrito(int IdCliente, int IdProducto, bool Sumar, out string Mensaje)
        {
            return OBJCapaDato.OperacionCarrito(IdCliente, IdProducto , Sumar , out Mensaje);
        }

        public int CantidadCarrito(int IdCliente)
        {
            return OBJCapaDato.CantidadCarrito(IdCliente);
        }

        public List<Carrito> ListarProducto(int idCliente)
        {
            return OBJCapaDato.ListarProducto(idCliente);
        }

        public bool EliminarCarrito(int IdCliente, int IdProducto)
        {
            return OBJCapaDato.EliminarCarrito(IdCliente, IdProducto);
        }

    }
}
