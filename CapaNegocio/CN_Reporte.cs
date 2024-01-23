using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {

        private CD_Reporte objCapaDato = new CD_Reporte();

        public DashBoard Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Reporte> ListarReporte(string fechainicio, string fechafin, string idTransaccion)
        {
            return objCapaDato.ListarReporte( fechainicio, fechafin, idTransaccion);
        }


    }
}
