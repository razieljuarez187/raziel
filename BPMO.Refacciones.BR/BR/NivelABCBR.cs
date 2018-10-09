using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Refacciones.BR.DA;
using System.Data;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Reglas de negocio para NivelABC
    /// </summary>
    public class NivelABCBR : IBRBaseCatalogo {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return this.ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Métodos
        /// <summary>
        /// Crea un registro de Refaccion en la base de datos
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="catalogoBase">Refaccion que desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, CatalogoBaseBO catalogoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Actualiza un registro de Refaccion en la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="catalogoBase">Refaccion que tiene los datos ha actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, CatalogoBaseBO catalogoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Elimina un registro de Refaccion en la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="catalogoBase">Refaccion que tiene los datos ha Borrar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, CatalogoBaseBO catalogoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de Refacciones de la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="catalogoBase">Refaccion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Un List de CatalogoBase que contiene la información de Ciudad generada por la consulta</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            try {
                NivelABCConsultarDAO consultarDAO = new NivelABCConsultarDAO();
                return consultarDAO.Consultar(dataContext, catalogoBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Consulta Completo, hasta primer nivel, registros de Refacciones de Cores en la base de datos.
        /// </summary>
        /// <param name="dataContext">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="catalogoBase">Refaccion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Un List de CatalogoBase que contiene la información de las refaccion sus relaciones a primer nivel, generada por la consulta</returns>
        public List<CatalogoBaseBO> ConsultarCompleto(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            throw new NotImplementedException();
        }
        #endregion Métodos


        
    }
}
