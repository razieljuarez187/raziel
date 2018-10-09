using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para el manejo de movimientos de cores del sistema Líder
    /// </summary>
    public class MovimientoCoreBR : IBRBaseAuditoria {
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

        #region Metodos
        /// <summary>
        /// Crea un registro de movimiento de cores
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de cores que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            try {
                MovimientoCoreInsertarDAO insertarDAO = new MovimientoCoreInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, auditoriaBase);
                registrosAfectados = insertarDAO.RegistrosAfectados;
                ultimoIdGenerado = insertarDAO.UltimoIdGenerado;
                return esExito;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Actualiza un registro de movimiento de cores
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de cores que se desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Elimina un registro de movimiento de cores
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de cores que se desea eliminar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de movimientos de cores
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de cores que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los movimientos de cores recuperados por la consulta</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Consulta movimientos de cores hasta su primer nivel de asociación y composición
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de core que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los movimientos de core y sus relaciones a primer nivel, recuperados por la consulta</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            throw new System.NotImplementedException();
        }
        #endregion Metodos
    }
}
