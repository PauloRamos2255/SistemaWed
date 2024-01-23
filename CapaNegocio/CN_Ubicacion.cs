using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {

        private CD_Ubicacion OBJCapaDato = new CD_Ubicacion();
        public List<Departamento> ListarDepartamento()
        {
            return OBJCapaDato.ListarDepartamento();
        }


        public List<Provincia> ListarProvincia(String IdDepartamento)
        {
            return OBJCapaDato.ListarProvincia(IdDepartamento);
        }


        public List<Distrito> ListarDistrito(String IdDepartamento, String IdProvincia)
        {
            return OBJCapaDato.ListarDistrito(IdDepartamento, IdProvincia);
        }

    }
}
