using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta OBJCapaDato = new CD_Venta();
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            return OBJCapaDato.Registrar(obj , DetalleVenta , out Mensaje);
        }

        public List<DetalleVenta> ListarCompras(int idCliente)
        {
            return OBJCapaDato.ListarCompras(idCliente);
        }

    }
}
