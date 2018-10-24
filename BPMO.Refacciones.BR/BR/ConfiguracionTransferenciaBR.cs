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
        /// Inserta un refgistro ConfiguracionTransferencia en la base de datos
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
        /// Actualiza un registro ConfiguracionTransferencia de la base de datos
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
                ConfiguracionTransferenciaActualizarDAO actualizarDAO = new ConfiguracionTransferenciaActualizarDAO();
                bool esExito = actualizarDAO.Actualizar(dataContext, auditoriaBase);
                this.registrosAfectados = actualizarDAO.RegistrosAfectados;

                return esExito;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Elimina un registro ConfiguracionTransferencia de la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <param name="firma">Objeto que contiene los permisos de la acción a realizar</param>
        /// <returns>Indica si la operación termino correctamente</returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Obtiene una lista de ConfiguracionTransferencia
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
        /// Obtiene una lista de ConfiguracionTransferencia
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Lista de resultados</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                List<CatalogoBaseBO> lstNivelABC;
                List<CatalogoBaseBO> lstNaturalezas;
                NivelABCBO nivelABCBO = new NivelABCBO();
                NivelABCBR nivelABCBR = new NivelABCBR();
                NaturalezasBO naturalezasBO = new NaturalezasBO();
                NaturalezaBR naturalezasBR = new NaturalezaBR();
                ConfiguracionCantidadTransferenciaBO configuracionCantidadTransferenciaBO = new ConfiguracionCantidadTransferenciaBO();
                ConfiguracionCantidadTransferenciaBR ConfiguracionCantidadTransferenciaBR = new ConfiguracionCantidadTransferenciaBR();
                ConfiguracionHoraTransferenciaBO configuracionHoraTransferenciaBO = new ConfiguracionHoraTransferenciaBO();
                ConfiguracionHoraTransferenciaBR ConfiguracionHoraTransferenciaBR = new ConfiguracionHoraTransferenciaBR();
                List<AuditoriaBaseBO> confTransferenciaBO = this.Consultar(dataContext, auditoriaBase);
                if (confTransferenciaBO != null) {
                    List<AuditoriaBaseBO> lstConfiguracionesCantidad = ConfiguracionCantidadTransferenciaBR.Consultar(dataContext, configuracionCantidadTransferenciaBO, confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0]);
                    if (lstConfiguracionesCantidad.Count > 1)
                        throw new Exception("Se encontraron mas de una configuración de cantidad de transferencias. Por favor verifique.");
                    else if (lstConfiguracionesCantidad.Count <= 0)
                        throw new Exception("No se encontró ninguna configuración de cantidad de transferencias. Por favor verifique.");
                    else
                        confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].ConfiguracionCantidadTransferencia = lstConfiguracionesCantidad.ConvertAll(item => (ConfiguracionCantidadTransferenciaBO)item)[0];

                    List<AuditoriaBaseBO> lstConfiguracionesHora = ConfiguracionHoraTransferenciaBR.Consultar(dataContext, configuracionHoraTransferenciaBO, confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0]);
                    if (lstConfiguracionesHora.Count > 1)
                        throw new Exception("Se encontraron mas de una configuración de hora de transferencias. Por favor verifique.");
                    else if (lstConfiguracionesHora.Count <= 0)
                        throw new Exception("No se encontró ninguna configuración de hora de transferencias. Por favor verifique.");
                    else
                        confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].ConfiguracionHoraTransferencia = lstConfiguracionesHora.ConvertAll(item => (ConfiguracionHoraTransferenciaBO)item)[0];

                    lstNivelABC = nivelABCBR.Consultar(dataContext, nivelABCBO);
                    confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].NivelesABC = lstNivelABC.ConvertAll(x => (NivelABCBO)x);
                    lstNaturalezas = naturalezasBR.Consultar(dataContext, naturalezasBO);
                    confTransferenciaBO.ConvertAll(x => (ConfiguracionTransferenciaBO)x)[0].Naturalezas = lstNaturalezas.ConvertAll(x => (NaturalezasBO)x);
                }
                return confTransferenciaBO;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Obtiene las ConfiguracionTransferencia asignadas
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="configuracionFiltro">Objeto que contiene los parámetros a buscar</param>
        /// <returns>Registros que coinciden con la búsqueda</returns>
        public DataSet ConsultarFiltro(IDataContext dataContext, AuditoriaBaseBO configuracionFiltro) {
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

                    #region ConfiguracionTransferencia
                    if (!row.IsNull("NivelABCId"))
                        configuracionNivelABC.Id = (Int32)Convert.ChangeType(row["NivelABCId"], typeof(Int32));
                    if (!row.IsNull("ClaveABC"))
                        configuracionNivelABC.NombreCorto = (String)Convert.ChangeType(row["ClaveABC"], typeof(String));
                    if (!row.IsNull("Descripcion"))
                        configuracionNivelABC.Nombre = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
                    #endregion /ConfiguracionTransferencia

                    lstNivelABC.Add(configuracionNivelABC);
                }
                return lstNivelABC;
            } catch {
                throw;
            }
        }
        public List<NaturalezasBO> ConsultarConfNaturalezas(IDataContext dataContext, int? configuracionId) {
            try {
                ConfiguracionTransferenciaNaturalezaConsultarDA consultarDA = new ConfiguracionTransferenciaNaturalezaConsultarDA();
                List<NaturalezasBO> lstNaturalezas = new List<NaturalezasBO>();
                NaturalezasBO configuracionlstNaturalezas = null;
                DataSet ds = consultarDA.Consultar(dataContext, configuracionId);
                foreach (DataRow row in ds.Tables[0].Rows) {
                    #region Inicializar BO
                    configuracionlstNaturalezas = new NaturalezasBO();
                    configuracionlstNaturalezas.Auditoria = new AuditoriaBO();
                    #endregion /Inicializar BO

                    #region ConfiguracionTransferencia
                    if (!row.IsNull("NaturalezaMovId"))
                        configuracionlstNaturalezas.Id = (Int32)Convert.ChangeType(row["NaturalezaMovId"], typeof(Int32));
                    if (!row.IsNull("Clave"))
                        configuracionlstNaturalezas.NombreCorto = (String)Convert.ChangeType(row["Clave"], typeof(String));
                    if (!row.IsNull("Nombre"))
                        configuracionlstNaturalezas.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
                    #endregion /ConfiguracionTransferencia

                    lstNaturalezas.Add(configuracionlstNaturalezas);
                }
                return lstNaturalezas;
            } catch {
                throw;
            }
        }
        public bool InsertarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                dataContext.OpenConnection(miFirma);
                dataContext.BeginTransaction(miFirma);
                configuracionCantidadBr = new ConfiguracionCantidadTransferenciaBR();
                configuracionHoraBr = new ConfiguracionHoraTransferenciaBR();
                bool esExito = this.Insertar(dataContext, auditoriaBase, firma);
                int configuracionId = this.ultimoIdGenerado;
                ConfiguracionTransferenciaBO configTransferencia = (ConfiguracionTransferenciaBO)auditoriaBase;
                configTransferencia.Id = configuracionId;
                configuracionCantidadBr.Insertar(dataContext, (ConfiguracionCantidadTransferenciaBO)configTransferencia.ConfiguracionCantidadTransferencia, configTransferencia, firma);
                configuracionHoraBr.Insertar(dataContext, (ConfiguracionHoraTransferenciaBO)configTransferencia.ConfiguracionHoraTransferencia, configTransferencia, firma);
                ConfiguracionTransferenciaNivelABCInsertarDA confNivelABCDAInsertar = new ConfiguracionTransferenciaNivelABCInsertarDA();
                ConfiguracionTransferenciaNaturalezaInsertarDA confNaturalezasInsertar = new ConfiguracionTransferenciaNaturalezaInsertarDA();
                if (configTransferencia.NivelesABC != null && configTransferencia.NivelesABC.Count > 0) {
                    foreach (NivelABCBO item in configTransferencia.NivelesABC) {
                        confNivelABCDAInsertar.Insertar(dataContext, configuracionId, item.Id);
                    }
                }
                if (configTransferencia.Naturalezas != null && configTransferencia.Naturalezas.Count > 0) {
                    foreach (NaturalezasBO item in configTransferencia.Naturalezas) {
                        confNaturalezasInsertar.Insertar(dataContext, configuracionId, item.Id);
                    }
                }
                dataContext.CommitTransaction(miFirma);
                return esExito;
            } catch {
                dataContext.RollbackTransaction(miFirma);
                throw;
            } finally {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(miFirma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
        }
        public bool ActualizarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion
                dataContext.OpenConnection(miFirma);
                dataContext.BeginTransaction(miFirma);
                ConfiguracionTransferenciaActualizarDAO actualizarDAO = new ConfiguracionTransferenciaActualizarDAO();
                ConfiguracionCantidadTransferenciaBR configuracionCantidadBr = new ConfiguracionCantidadTransferenciaBR();
                ConfiguracionHoraTransferenciaBR configuracionHoraBr = new ConfiguracionHoraTransferenciaBR();
                ConfiguracionTransferenciaNivelABCBorrarDA confNivelABCDABorrar = new ConfiguracionTransferenciaNivelABCBorrarDA();
                ConfiguracionTransferenciaNivelABCInsertarDA confNivelABCDAInsertar = new ConfiguracionTransferenciaNivelABCInsertarDA();
                ConfiguracionTransferenciaNaturalezaBorrarDA confNaturalezasDABorrar = new ConfiguracionTransferenciaNaturalezaBorrarDA();
                ConfiguracionTransferenciaNaturalezaInsertarDA confNaturalezasInsertar = new ConfiguracionTransferenciaNaturalezaInsertarDA();
                ConfiguracionTransferenciaBO configuracion = (ConfiguracionTransferenciaBO)auditoriaBase;
                bool esExito = actualizarDAO.Actualizar(dataContext, configuracion);
                this.registrosAfectados = actualizarDAO.RegistrosAfectados;
                configuracionCantidadBr.Actualizar(dataContext, configuracion.ConfiguracionCantidadTransferencia, configuracion, firma);
                configuracionHoraBr.Actualizar(dataContext, configuracion.ConfiguracionHoraTransferencia, configuracion, firma);
                confNivelABCDABorrar.Borrar(dataContext, configuracion.Id);
                if (configuracion.NivelesABC != null && configuracion.NivelesABC.Count > 0) {
                    foreach (NivelABCBO item in configuracion.NivelesABC) {
                        confNivelABCDAInsertar.Insertar(dataContext, configuracion.Id, item.Id);
                    }
                }
                confNaturalezasDABorrar.Borrar(dataContext, configuracion.Id);
                if (configuracion.Naturalezas != null && configuracion.Naturalezas.Count > 0) {
                    foreach (NaturalezasBO item in configuracion.Naturalezas) {
                        confNaturalezasInsertar.Insertar(dataContext, configuracion.Id, item.Id);
                    }
                }
                dataContext.CommitTransaction(miFirma);
                return esExito;
            } catch {
                dataContext.RollbackTransaction(miFirma);
                throw;
            } finally {
                if (dataContext.ConnectionState == ConnectionState.Open)
                    dataContext.CloseConnection(miFirma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
        }
        #endregion /Métodos
    }
}
