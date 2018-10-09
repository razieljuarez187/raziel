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
using BPMO.Refacciones.BR.DA;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Reglas de negocio para ConfiguracionTransferencia
    /// </summary>
    public class ConfiguracionTransferenciaBR : IBRBaseAuditoria {
        #region Atributos
        private int registrosAfectados;
        private int ultimoIdGenerado;
        ConfiguracionCantidadTransferenciaBR configuracionCantidadBr;
        ConfiguracionHoraTransferenciaBR configuracionHoraBr;
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
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                //TODO:
                //SecurityBR seguridadBR = new SecurityBR(firma);
                //firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionTransferenciaInsertarDAO insertarDAO = new ConfiguracionTransferenciaInsertarDAO();
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
                //TODO: SecurityBR seguridadBR = new SecurityBR(firma);
                //firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                ConfiguracionTransferenciaActualizarDAO actualizarDAO = new ConfiguracionTransferenciaActualizarDAO();
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
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtiene una lista de ConfiguracionReglaUsuario
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                ConfiguracionTransferenciaConsultarDAO consultarDAO = new ConfiguracionTransferenciaConsultarDAO();
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
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                ConfiguracionTransferenciaConsultarDAO consultarDAO = new ConfiguracionTransferenciaConsultarDAO();
                List<CatalogoBaseBO> lstNivelABCBO;
                NivelABCBO nivelABCBO = new NivelABCBO();
                NivelABCBR nivelABCBR = new NivelABCBR();
                ConfiguracionCantidadTransferenciaBO configuracionCantidadTransferenciaBO = new ConfiguracionCantidadTransferenciaBO();
                ConfiguracionCantidadTransferenciaBR ConfiguracionCantidadTransferenciaBR = new ConfiguracionCantidadTransferenciaBR();
                ConfiguracionHoraTransferenciaBO configuracionHoraTransferenciaBO = new ConfiguracionHoraTransferenciaBO();
                ConfiguracionHoraTransferenciaBR ConfiguracionHoraTransferenciaBR = new ConfiguracionHoraTransferenciaBR();

                List<AuditoriaBaseBO> confTransferenciaBO = consultarDAO.Consultar(dataContext, auditoriaBase);
                List<AuditoriaBaseBO> lstConfiguracionesCantidad = ConfiguracionCantidadTransferenciaBR.Consultar(dataContext, configuracionCantidadTransferenciaBO, confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0]);
                List<AuditoriaBaseBO> lstConfiguracionesHora = ConfiguracionHoraTransferenciaBR.Consultar(dataContext, configuracionHoraTransferenciaBO, confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0]);
                lstNivelABCBO = nivelABCBR.Consultar(dataContext, nivelABCBO);
                if (lstConfiguracionesCantidad.Count <= 1 && lstConfiguracionesHora.Count <= 1) {
                    confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].ConfiguracionCantidadTransferencia = lstConfiguracionesCantidad.ConvertAll(item => (ConfiguracionCantidadTransferenciaBO)item)[0];
                    confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].ConfiguracionHoraTransferencia = lstConfiguracionesHora.ConvertAll(item => (ConfiguracionHoraTransferenciaBO)item)[0];
                }
                confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].NivelesABC = lstNivelABCBO.ConvertAll(x => (NivelABCBO)x);
                return confTransferenciaBO;
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
            throw new NotImplementedException();
        }
        public List<NivelABCBO> ConsultarDataSet(IDataContext dataContext, int? configuracionId) {
            try {
                ConfiguracionTransferenciaNivelABCConsultarDA consultarDA = new ConfiguracionTransferenciaNivelABCConsultarDA();
                List<NivelABCBO> lstNivelABC = new List<NivelABCBO>();
                NivelABCBO configuracionNivelABC = null;
                DataSet ds = consultarDA.Consultar(dataContext, configuracionId);
                foreach (DataRow row in ds.Tables[0].Rows) {
                    #region Inicializar BO
                    configuracionNivelABC = new NivelABCBO();
                    configuracionNivelABC.Auditoria = new AuditoriaBO();
                    #endregion /Inicializar BO

                    #region ConfiguracionesReglas
                    if (!row.IsNull("NivelABCId"))
                        configuracionNivelABC.Id = (Int32)Convert.ChangeType(row["NivelABCId"], typeof(Int32));
                    if (!row.IsNull("ClaveABC"))
                        configuracionNivelABC.NombreCorto = (String)Convert.ChangeType(row["ClaveABC"], typeof(String));
                    if (!row.IsNull("Descripcion"))
                        configuracionNivelABC.Nombre = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
                    #endregion /ConfiguracionesReglas

                    lstNivelABC.Add(configuracionNivelABC);
                }
                return lstNivelABC;
            } catch {
                throw;
            }
        }
        public bool InsertarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma, List<NivelABCBO> confNivelABC) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                //TODO:
                //SecurityBR seguridadBR = new SecurityBR(firma);
                //firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                dataContext.OpenConnection(miFirma);
                dataContext.BeginTransaction(miFirma);
                ConfiguracionTransferenciaInsertarDAO insertarDAO = new ConfiguracionTransferenciaInsertarDAO();
                configuracionCantidadBr = new ConfiguracionCantidadTransferenciaBR();
                configuracionHoraBr = new ConfiguracionHoraTransferenciaBR();
                bool esExito = insertarDAO.Insertar(dataContext, auditoriaBase);
                this.registrosAfectados = insertarDAO.RegistrosAfectados;
                this.ultimoIdGenerado = insertarDAO.UltimoIdGenerado.Value;
                int configuracionId = this.ultimoIdGenerado;
                ConfiguracionTransferenciaBO configTransferencia = (ConfiguracionTransferenciaBO)auditoriaBase;
                configTransferencia.Id = configuracionId;
                configuracionCantidadBr.Insertar(dataContext, (ConfiguracionCantidadTransferenciaBO) configTransferencia.ConfiguracionCantidadTransferencia, configTransferencia, firma);
                configuracionHoraBr.Insertar(dataContext, (ConfiguracionHoraTransferenciaBO) configTransferencia.ConfiguracionHoraTransferencia, configTransferencia, firma);
                ConfiguracionTransferenciaNivelABCInsertarDA confNivelABCDAInsertar = new ConfiguracionTransferenciaNivelABCInsertarDA();
                foreach (NivelABCBO item in confNivelABC) {
                    confNivelABCDAInsertar.Insertar(dataContext, configuracionId, item.Id);
                }
                dataContext.CommitTransaction(miFirma);
                return esExito;
            } catch {
                dataContext.RollbackTransaction(miFirma);
                throw;
            } finally {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(miFirma);
            }
        }
        public bool ActualizarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma, List<NivelABCBO> confNivelABC) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                //TODO: SecurityBR seguridadBR = new SecurityBR(firma);
                //firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                dataContext.OpenConnection(miFirma);
                dataContext.BeginTransaction(miFirma);
                ConfiguracionTransferenciaActualizarDAO actualizarDAO = new ConfiguracionTransferenciaActualizarDAO();
                ConfiguracionCantidadTransferenciaBR configuracionCantidadBr = new ConfiguracionCantidadTransferenciaBR();
                ConfiguracionHoraTransferenciaBR configuracionHoraBr = new ConfiguracionHoraTransferenciaBR();
                ConfiguracionTransferenciaNivelABCBorrarDA confNivelABCDABorrar = new ConfiguracionTransferenciaNivelABCBorrarDA();
                ConfiguracionTransferenciaNivelABCConsultarDA confNivelABCDAConsultar= new ConfiguracionTransferenciaNivelABCConsultarDA();
                ConfiguracionTransferenciaNivelABCInsertarDA confNivelABCDAInsertar = new ConfiguracionTransferenciaNivelABCInsertarDA();
                bool esExito = actualizarDAO.Actualizar(dataContext, auditoriaBase);
                this.registrosAfectados = actualizarDAO.RegistrosAfectados;
                ConfiguracionTransferenciaBO configuracion=(ConfiguracionTransferenciaBO)auditoriaBase;
                configuracionCantidadBr.Actualizar(dataContext, auditoriaBase, auditoriaBase, firma);
                configuracionHoraBr.Actualizar(dataContext, auditoriaBase, auditoriaBase, firma);
                confNivelABCDABorrar.Borrar(dataContext,configuracion.Id);
                foreach (NivelABCBO item in confNivelABC) {
                    confNivelABCDAInsertar.Insertar(dataContext, configuracion.Id, item.Id);
                }
                dataContext.CommitTransaction(miFirma);
                return esExito;
            } catch {
                dataContext.RollbackTransaction(miFirma);
                throw;
            } finally {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(miFirma);
            }
        }
        #endregion /Métodos
    }
}
