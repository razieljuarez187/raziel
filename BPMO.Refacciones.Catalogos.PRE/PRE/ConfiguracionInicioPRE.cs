using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Generales.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BR;
using BPMO.Refacciones.Catalogos.VIS;

namespace BPMO.Refacciones.Catalogos.PRE {
    /// <summary>
    /// Presentador para el manejo de ConfiguracionInicio
    /// </summary>
    public class ConfiguracionInicioPRE {


        #region Atributos
        /// <summary>
        /// Vista de ConfiguracionInicio
        /// </summary>
        private IConfiguracionInicio vista;
        /// <summary>
        /// Contexto que contiene la información para acceso a base de datos
        /// </summary>
        private IDataContext dataContext = null;
        #endregion

        #region Constructores
        /// <summary>
        /// Método constructor de la presentadora de la página de configuración inicial
        /// </summary>
        /// <param name="vistaActual">Vista sobre la cual se va a trabajar</param>
        public ConfiguracionInicioPRE(IConfiguracionInicio vistaActual) {
            try {
                this.vista = vistaActual;
                dataContext = MDIPRE.CargarProviderDataContext(vista.ListaDatosConexion);
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Inconsistencias en los parametros de configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Obtiene una lista de adscripciones a las cuales el usuario tiene acceso
        /// </summary>
        public void ObtenerDatosAdscripcion() {
            Guid miFirma = Guid.NewGuid();
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new BPMO.Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            try {
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vista.Usuario, this.vista.Adscripcion);
                SecurityBR securityBR = new SecurityBR(seguridadBO);
                dataContext.OpenConnection(miFirma);
                List<AdscripcionBO> lstAdscripcion = securityBR.AdscripcionSegConsultar(this.dataContext);
                if (lstAdscripcion.Count > 0) {
                    dataContext.SetCurrentProvider("ORACLE");
                    dataContext.OpenConnection(miFirma);
                    if (lstAdscripcion.Count == 1) {
                        this.SeleccionarAdscripcion(lstAdscripcion[0]);
                    } else {
                        List<AdscripcionBO> listaAdscripcionBO = new List<AdscripcionBO>();
                        foreach (AdscripcionBO adscripcionBO in lstAdscripcion) {
                            if (adscripcionBO.UnidadOperativa != null && adscripcionBO.UnidadOperativa.Id != null &&
                                adscripcionBO.Sucursal != null && adscripcionBO.Sucursal.Id != null) {
                                bool AdscripcionValida = true;                              
                                                                
                                if (AdscripcionValida) {
                                    listaAdscripcionBO.Add(adscripcionBO);
                                }
                            }
                        }
                        this.vista.Adscripciones = listaAdscripcionBO;
                        this.vista.CargarDatosAdscripcion();
                    }
                } else {
                    this.vista.MostrarMensaje("No tiene permisos configurados para trabajar con el sistema de Servicio o sus permisos han caducado." +
                       "Para mas información póngase en contacto con el administrador del sistema.", ETipoMensajeIU.INFORMACION);
                }
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Surgió un error al obtener las adscripciones, si el problema persiste, contacte al administrador del sistema",
                     ETipoMensajeIU.ERROR, ex.Message);
            } finally {
                dataContext.SetCurrentProvider("ORACLE");
                dataContext.CloseConnection(miFirma);
                dataContext.SetCurrentProvider("LIDER");
                dataContext.CloseConnection(miFirma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
        }
        /// <summary>
        /// Carga la adscripción seleccionada y retorna el control a la página de inicio del sistema
        /// </summary>
        /// <param name="adscripcion">Adscripción</param>
        public void SeleccionarAdscripcion(AdscripcionBO adscripcion = null) {
            try {
                if (adscripcion == null) {
                    if (this.vista.UnidadOperativa != null && this.vista.Sucursal != null) {
                        #region Instanciar Adscripción Servicio
                        AdscripcionBO _adscripcion = new AdscripcionBO
                        {
                            UnidadOperativa = new UnidadOperativaBO { Id = this.vista.UnidadOperativa },
                            Sucursal = new SucursalBO { Id = this.vista.Sucursal }
                        };
                        #endregion
                        #region Cargar datos: Unidad Operativa y Sucursal
                        #region Verificar si la Adscripcion Servicio cambio
                        if (!VerificarCambioAdscripcion(_adscripcion)) {
                            this.vista.Adscripciones = null;
                            this.vista.EnviarAInicio();
                            return;
                        }
                        #endregion
                        UnidadOperativaBR unidadBR = new UnidadOperativaBR();
                        _adscripcion.UnidadOperativa = (UnidadOperativaBO)unidadBR.Consultar(this.dataContext, _adscripcion.UnidadOperativa).FirstOrDefault();
                        _adscripcion.Sucursal = this.ObtenerDatosSucursal(_adscripcion.Sucursal);
                        //Verifica que los datos de la adscripción servicio estén correctos antes del re-direccionamiento a la página de inicio
                        if (_adscripcion.UnidadOperativa != null && _adscripcion.UnidadOperativa.Id != null && _adscripcion.Sucursal != null &&
                            _adscripcion.Sucursal.Id != null) {
                            this.vista.Adscripciones = null;
                            this.vista.ListaProcesos = null;
                            this.vista.Adscripcion = _adscripcion;
                            this.vista.EnviarAInicio();
                        } else {
                            this.vista.MostrarMensaje("Adscripción no válida", ETipoMensajeIU.ERROR,
                                "Los datos de la adscripción seleccionada no se recuperaron de forma correcta, Intente de nuevo y si el problema persiste, por favor " +
                                "contacte al administrador del sistema");
                        }
                        #endregion
                    } else {
                        this.vista.MostrarMensaje("Los campos unidad operativa, sucursal y taller son obligatorios", ETipoMensajeIU.INFORMACION);
                    }
                } else {
                    if (adscripcion.UnidadOperativa != null && adscripcion.Sucursal != null) {
                        #region Instanciar Adscripción Servicio
                        AdscripcionBO adscripcionBo = new AdscripcionBO
                        {
                            UnidadOperativa = new UnidadOperativaBO { Id = adscripcion.UnidadOperativa.Id },
                            Sucursal = new SucursalBO { Id = adscripcion.Sucursal.Id }
                        };
                        #endregion
                        #region Cargar datos: Unidad Operativa, Sucursal, Taller
                        #region Verificar si la Adscripcion Servicio cambio
                        if (!VerificarCambioAdscripcion(adscripcionBo)) {
                            this.vista.Adscripciones = null;
                            this.vista.EnviarAInicio();
                            return;
                        }
                        #endregion
                        UnidadOperativaBR unidadBR = new UnidadOperativaBR();
                        adscripcionBo.UnidadOperativa = (UnidadOperativaBO)unidadBR.Consultar(this.dataContext, adscripcionBo.UnidadOperativa).FirstOrDefault();
                        adscripcionBo.Sucursal = this.ObtenerDatosSucursal(adscripcionBo.Sucursal);
                        //Verifica que los datos de la adscripción servicio estén correctos antes del redireccionamiento a la página de inicio
                        if (adscripcionBo.UnidadOperativa != null && adscripcionBo.UnidadOperativa.Id != null && adscripcionBo.Sucursal != null &&
                            adscripcionBo.Sucursal.Id != null) {
                            this.vista.Adscripciones = null;
                            this.vista.ListaProcesos = null;
                            this.vista.Adscripcion = adscripcionBo;
                            this.vista.EnviarAInicio();
                        } else {
                            this.vista.MostrarMensaje("Adscripcion no válida", ETipoMensajeIU.ERROR,
                                "Es probable que usted no cuente con una adscripción válida, Si el problema persiste, por favor contacte al administrador del sistema");
                        }
                        #endregion
                    } else {
                        this.vista.MostrarMensaje("Los campos unidad operativa, sucursal y taller son obligatorios", ETipoMensajeIU.INFORMACION);
                    }
                }
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Surgió un problema al cargar la unidad operativa, sucursal y/o taller, por favor refresque la página.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Verifica si la adscripción servicio ha cambiado
        /// </summary>
        /// <param name="adscripcion">AdscripcionServicioBO adscripción seleccionada</param>
        /// <returns>retorna true si la adscripción servicio ha cambiado</returns>
        private bool VerificarCambioAdscripcion(AdscripcionBO adscripcion) {
            if (this.vista.Adscripcion != null) {
                if (vista.Adscripcion.UnidadOperativa.Id == adscripcion.UnidadOperativa.Id
                    && vista.Adscripcion.Sucursal.Id == adscripcion.Sucursal.Id)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Realiza una consulta de la sucursal especificada
        /// </summary>
        /// <param name="sucursal">Sucursal</param>
        private SucursalBO ObtenerDatosSucursal(SucursalBO sucursal) {
            try {
                SucursalBR sucursalBR = new SucursalBR();
                sucursal = sucursalBR.Consultar(this.dataContext, sucursal)[0] as SucursalBO;
                return sucursal;
            } catch {
                throw;
            }
        }
        #endregion
    }
}
