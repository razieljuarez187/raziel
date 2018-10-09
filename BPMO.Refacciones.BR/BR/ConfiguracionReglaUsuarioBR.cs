using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Security.BR;
using System.Data;
using BPMO.Refacciones.DA;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Reglas de negocio para ConfiguracionReglaUsuario
    /// </summary>
    public class ConfiguracionReglaUsuarioBR : IBRBaseAuditoria {
        #region Atributos
        private int registrosAfectados;
        private int ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Métodos
        /// <summary>
        /// Inserta un refgistro ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <param name="firma">Objeto que contiene los permisos de la acción a realizar</param>
        /// <returns>Indica si la operación termino correctamente</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionReglaUsuarioInsertarDAO insertarDAO = new ConfiguracionReglaUsuarioInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, auditoriaBase);
                this.registrosAfectados = insertarDAO.RegistrosAfectados;
                this.ultimoIdGenerado = insertarDAO.UltimoIdGenerado.Value;
                return esExito;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Actualiza un registro ConfiguracionReglaUsuario de la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <param name="firma">Objeto que contiene los permisos de la acción a realizar</param>
        /// <returns>Indica si la operación termino correctamente</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionReglaUsuarioActualizarDAO actualizarDAO = new ConfiguracionReglaUsuarioActualizarDAO();
                bool esExito = actualizarDAO.Actualizar(dataContext, auditoriaBase);
                this.registrosAfectados = actualizarDAO.RegistrosAfectados;
                return esExito;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Elimina un registro ConfiguracionReglaUsuario de la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <param name="firma">Objeto que contiene los permisos de la acción a realizar</param>
        /// <returns>Indica si la operación termino correctamente</returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionReglaUsuarioBorrarDAO borrarDAO = new ConfiguracionReglaUsuarioBorrarDAO();
                return borrarDAO.Borrar(dataContext, auditoriaBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Obtiene una lista de ConfiguracionReglaUsuario
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            try {
                ConfiguracionReglaUsuarioConsultarDAO consultarDAO = new ConfiguracionReglaUsuarioConsultarDAO();
                return consultarDAO.Consultar(dataContext, auditoriaBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Obtiene una lista de ConfiguracionReglaUsuario
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            try {
                ConfiguracionReglaUsuarioConsultarDAO consultarDAO = new ConfiguracionReglaUsuarioConsultarDAO();
                return consultarDAO.ConsultarCompleto(dataContext, auditoriaBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Obtiene las ConfiguracionReglaUsuario asignadas
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="configuracionFiltro">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Registros que coinciden con la búsqueda</returns>
        public DataSet ConsultarFiltro(IDataContext dataContext, ConfiguracionReglaUsuarioFiltroBO configuracionFiltro) {
            try {
                ObtenerConfiguracionesReglasAsignadasDA consultarDA = new ObtenerConfiguracionesReglasAsignadasDA();
                return consultarDA.ConsultarConfiguracionesAsignadas(dataContext, configuracionFiltro);
            } catch {
                throw;
            }
        }
        #endregion /Métodos
    }
}
