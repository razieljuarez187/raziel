using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Security.BR;
using System.Data;
using BPMO.Refacciones.DA;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.BR.DAO;
using System;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Reglas de negocio para ConfiguracionHoraTransferencia
    /// </summary>
    public class ConfiguracionHoraTransferenciaBR : IBRBaseAuditoriaDetalle {
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
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //TODO:
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                //SecurityBR seguridadBR = new SecurityBR(firma);
                //firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionHoraTransferenciaInsertarDAO insertarDAO = new ConfiguracionHoraTransferenciaInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, auditoriaBase, objetoMaestro);
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
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                //TODO:
                //SecurityBR seguridadBR = new SecurityBR(firma);
                //firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionTransferenciaBO config = (ConfiguracionTransferenciaBO)auditoriaBase;
                ConfiguracionHoraTransferenciaActualizarDAO actualizarDAO = new ConfiguracionHoraTransferenciaActualizarDAO();
                bool esExito = actualizarDAO.Actualizar(dataContext, config.ConfiguracionHoraTransferencia, objetoMaestro);
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
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtiene una lista de ConfiguracionReglaUsuario
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro) {
            try {
                ConfiguracionHoraTransferenciaConsultarDAO consultarDAO = new ConfiguracionHoraTransferenciaConsultarDAO();
                return consultarDAO.Consultar(dataContext, auditoriaBase, objetoMaestro);
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
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtiene las ConfiguracionReglaUsuario asignadas
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="configuracionFiltro">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Registros que coinciden con la búsqueda</returns>
        public DataSet ConsultarFiltro(IDataContext dataContext, ConfiguracionReglaUsuarioFiltroBO configuracionFiltro) {
            throw new NotImplementedException();
        }
        #endregion /Métodos
    }
}
