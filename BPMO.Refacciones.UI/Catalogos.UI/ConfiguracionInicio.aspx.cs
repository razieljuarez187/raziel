using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BO;
using BPMO.Refacciones.Catalogos.PRE;
using BPMO.Refacciones.Catalogos.VIS;
using BPMO.Refacciones.Master.UI;

namespace BPMO.Refacciones.Catalogos.UI {
    /// <summary>
    /// Interfaz de Usuario para cargar la configuración de inicio
    /// </summary>
    public partial class PaginaInicio : System.Web.UI.Page, IConfiguracionInicio {
        
        #region Atributos
        /// <summary>
        /// Presentador de ConfiguracionInicio
        /// </summary>
        ConfiguracionInicioPRE presentador = null;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la Unidad Operativa
        /// </summary>
        public int? UnidadOperativa {
            get {
                int unidad;
                if (!int.TryParse(this.ddlUnidadOperativa.SelectedValue, out unidad)) {
                    return null;
                }
                return unidad;
            }
        }
        /// <summary>
        /// Obtiene la Sucursal
        /// </summary>
        public int? Sucursal {
            get {
                int sucursal;
                if (!int.TryParse(this.ddlSucursal.SelectedValue, out sucursal)) {
                    return null;
                }
                return sucursal;
            }
        }        
        /// <summary>
        /// Obtiene la lista de conexiones para la base de datos
        /// </summary>
        public List<DatosConexionBO> ListaDatosConexion {
            get { return ((SiteMaster)this.Master).ListadoDatosConexion; }
        }
        /// <summary>
        /// Obtiene el usuario logueado
        /// </summary>
        public UsuarioBO Usuario {
            get { return ((SiteMaster)this.Master).Usuario; }
        }
        /// <summary>
        /// Obtiene o establece la Adscripción del Servicio
        /// </summary>
        public AdscripcionBO Adscripcion {
            get {
                return ((SiteMaster)this.Master).Adscripcion;
            }
            set {
                ((SiteMaster)this.Master).Adscripcion = value;
            }
        }
        /// <summary>
        /// Obtiene o establece la lista de Adscripciones
        /// </summary>
        public List<AdscripcionBO> Adscripciones {
            get {
                return this.Session["lstAdscripciones"] != null ? (List<AdscripcionBO>)this.Session["lstAdscripciones"] : null;
            }
            set {
                this.Session["lstAdscripciones"] = value;
            }
        }
        /// <summary>
        /// Obtiene o estable la lista de Procesos del sistema
        /// </summary>
        public List<ProcesoBO> ListaProcesos {
            get {
                return ((SiteMaster)this.Master).ListadoProcesos;
            }
            set {
                ((SiteMaster)this.Master).ListadoProcesos = value;
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">1: Error, 2: Advertencia, 3: Información</param>
        /// <param name="msjDetalle">Desplega el detalle del mensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null) {
            SiteMaster masterMsj = (SiteMaster)Page.Master;
            masterMsj.MostrarMensaje(mensaje, tipoMensaje, msjDetalle);
        }
        /// <summary>
        /// Envia a la página de inicio del sistema
        /// </summary>
        public void EnviarAInicio() {
            this.Response.Redirect("~/Catalogos.UI/default.aspx?MenuSeleccionado=Inicio");
        }
        /// <summary>
        /// Método manejador del evento load del formulario web
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EventArgs</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.presentador = new ConfiguracionInicioPRE(this);
                if (!this.IsPostBack) {
                    SiteMaster miMaster = (SiteMaster)this.Master;
                    miMaster.MostrarMenu = false;
                    this.presentador.ObtenerDatosAdscripcion();
                }
            } catch (Exception) {
                this.MostrarMensaje("Error inesperado", ETipoMensajeIU.ERROR, "surgió un error inesperado, por favor póngase en contacto con el administrador del sistema");
            }
        }

        /// <summary>
        /// Este método carga los datos de las unidades operativas a las que el usuario tiene acceso
        /// </summary>
        public void CargarDatosAdscripcion() {
            try {
                this.ddlUnidadOperativa.Items.Clear();
                this.ddlSucursal.Items.Clear();
                var grupoUnidadOperativa = this.Adscripciones.GroupBy(ad => ad.UnidadOperativa.Id);
                foreach (var unidadId in grupoUnidadOperativa) {
                    UnidadOperativaBO unidadBO = this.Adscripciones.FirstOrDefault(ad => ad.UnidadOperativa.Id == unidadId.Key.Value).UnidadOperativa;
                    if (unidadBO.Id != null && !string.IsNullOrWhiteSpace(unidadBO.Nombre)) {
                        ListItem unidadItem = new ListItem(unidadBO.Nombre, unidadBO.Id.ToString());
                        this.ddlUnidadOperativa.Items.Add(unidadItem);
                    }
                }
                if (this.ddlUnidadOperativa.SelectedItem != null) {
                    this.ObtenerSucursales(int.Parse(this.ddlUnidadOperativa.SelectedValue));
                }
            } catch (Exception) {

                this.MostrarMensaje("Error al cargar listas de adscripciones", ETipoMensajeIU.ERROR, "Es posible que algunas unidades operativas obtenidas no sean válidas, " +
                                    "por favor contacte al administrador del sistema");
            }
        }
        /// <summary>
        /// Este método carga las sucursales en base a una unidad operativa
        /// </summary>
        /// <param name="UnidadOperativaId">Identificador de la unidad operativa</param>
        private void ObtenerSucursales(int UnidadOperativaId) {
            try {
                this.ddlSucursal.Items.Clear();
                var grupoUnidadOperativa = this.Adscripciones.GroupBy(ad => ad.UnidadOperativa.Id).FirstOrDefault(gu => gu.Key == UnidadOperativaId);
                var grupoSucursal = grupoUnidadOperativa.GroupBy(u => u.Sucursal.Id);
                foreach (var sucursalId in grupoSucursal) {
                    SucursalBO sucursalBO = this.Adscripciones.FirstOrDefault(ad => ad.Sucursal.Id == sucursalId.Key).Sucursal;
                    if (sucursalBO.Id != null && !string.IsNullOrWhiteSpace(sucursalBO.Nombre)) {
                        ListItem sucursalItem = new ListItem(sucursalBO.Nombre, sucursalBO.Id.ToString());
                        ddlSucursal.Items.Add(sucursalItem);
                    }
                }
            } catch (Exception) {

                this.MostrarMensaje("Error al cargar listas de adscripciones", ETipoMensajeIU.ERROR, "Es posible que algunas sucursales obtenidas no sean válidas, " +
                                    "por favor contacte al administrador del sistema");
            }
        }
        
        /// <summary>
        /// Método manejador del evento SelectedIndexChanged del control ddlUnidadOperativa
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EventArgs</param>
        protected void ddlUnidadOperativa_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (this.ddlUnidadOperativa.SelectedItem != null) {
                    this.ObtenerSucursales(int.Parse(this.ddlUnidadOperativa.SelectedValue));
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al seleccionar la Unidad Operativa.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }        
        /// <summary>
        /// Método manejador del evento click del control btnAceptar
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EventArgs</param>
        protected void btnAceptar_Click(object sender, EventArgs e) {
            try {
                presentador.SeleccionarAdscripcion();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cargar información de la adscripción servicio.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion        
    }
}