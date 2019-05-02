using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Security.BR;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Reglas de negocio para Excepciones de etapa de embalaje
    /// </summary>
    public class ExcepcionEtapaEmbalajeBR : IBRBaseAuditoria {
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
        /// Inserta un refgistro ExcepcionEtapaEmbalaje en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <param name="firma">Objeto que contiene los permisos de la acción a realizar</param>
        /// <returns>Indica si la operación termino correctamente</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionEtapaEmbalajeInsertarDAO insertarDAO = new ConfiguracionEtapaEmbalajeInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, auditoriaBase);
                this.registrosAfectados = insertarDAO.RegistrosAfectados;
                this.ultimoIdGenerado = insertarDAO.UltimoIdGenerado.Value;
                return esExito;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Actualiza un registro ExcepcionEtapaEmbalaje de la base de datos
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
                ConfiguracionEtapaEmbalajeActualizarDAO actualizarDAO = new ConfiguracionEtapaEmbalajeActualizarDAO();
                bool esExito = actualizarDAO.Actualizar(dataContext, auditoriaBase);
                this.registrosAfectados = actualizarDAO.RegistrosAfectados;

                return esExito;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Elimina un registro ExcepcionEtapaEmbalaje de la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <param name="firma">Objeto que contiene los permisos de la acción a realizar</param>
        /// <returns>Indica si la operación termino correctamente</returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtiene una lista de ExcepcionEtapaEmbalaje
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                ConfiguracionEtapaEmbalajeConsultarDAO consultarDAO = new ConfiguracionEtapaEmbalajeConsultarDAO();
                return consultarDAO.Consultar(dataContext, auditoriaBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Obtiene una lista de ExcepcionEtapaEmbalaje
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            throw new NotImplementedException();
        }
        #endregion /Métodos
    }
}
