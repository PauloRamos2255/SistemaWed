using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Ubicacion
    {

        public List<Departamento> ListarDepartamento()
        {
            try
            {
                // Codifique
                //Creamos una lista de la entidad de negocio....
                List<Departamento> objLsita = new List<Departamento>();
                ecommerce2024Entities Prestamo = new ecommerce2024Entities();

                //Con LNQ obtenemos el llistado de categorias.....
                var query = Prestamo.DEPARTAMENTO.OrderBy(miPrestamo => miPrestamo.IdDepartamento);

                foreach (var miObjeto in query)
                {
                    Departamento objDepartamento = new Departamento();

                    objDepartamento.IdDeaprtamneto = miObjeto.IdDepartamento;
                    objDepartamento.Descripcion = miObjeto.Descripcion;
                    //aGREGAMOS LA INMSTACIA A LA LISTA
                    objLsita.Add(objDepartamento);
                }
                return objLsita;
            }
            catch 
            {
                return new List<Departamento>();
            }

        }


        public List<Provincia> ListarProvincia(String IdDepartamento)
        {
            try
            {
                // Codifique

                // Codifique
                //Creamos una lista de la entidad de negocio....
                List<Provincia> objLsita = new List<Provincia>();
                ecommerce2024Entities Prestamo = new ecommerce2024Entities();

                //Con LNQ obtenemos el llistado de categorias.....
                var query = Prestamo.PROVINCIA.OrderBy(miPresatmo => miPresatmo.IdProvincia)
                     .Where(miPresatmo => miPresatmo.IdDepartamento.Contains(IdDepartamento));

                foreach (var miObjeto in query)
                {
                    Provincia objProvincia = new Provincia();

                    objProvincia.IdProvincia = miObjeto.IdProvincia;
                    objProvincia.Descripcion = miObjeto.Descripcion;

                    //aGREGAMOS LA INMSTACIA A LA LISTA
                    objLsita.Add(objProvincia);
                }
                return objLsita;
            }
            catch 
            {
                return new List<Provincia>();
            }
        }


        public List<Distrito> ListarDistrito(String IdDepartamento , String IdProvincia)
        {
            try
            {
                // Codifique

                // Codifique
                //Creamos una lista de la entidad de negocio....
                List<Distrito> objLsita = new List<Distrito>();
                ecommerce2024Entities Prestamo = new ecommerce2024Entities();

                //Con LNQ obtenemos el llistado de categorias.....
                var query = Prestamo.DISTRITO.OrderBy(miPresatmo => miPresatmo.IdProvincia)
                     .Where(miPresatmo => miPresatmo.IdDepartamento.Contains(IdDepartamento) && miPresatmo.IdProvincia.Contains(IdProvincia));

                foreach (var miObjeto in query)
                {
                    Distrito objProvincia = new Distrito();

                    objProvincia.IdDistrito= miObjeto.IdDistrito;
                    objProvincia.Descripcion = miObjeto.Descripcion;

                    //aGREGAMOS LA INMSTACIA A LA LISTA
                    objLsita.Add(objProvincia);
                }
                return objLsita;
            }
            catch
            {
                return new List<Distrito>();
            }
        }

    }
}
