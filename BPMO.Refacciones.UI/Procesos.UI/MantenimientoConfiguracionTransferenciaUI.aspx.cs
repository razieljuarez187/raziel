using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.Enumeradores;
using BPMO.Refacciones.Master.UI;
using BPMO.Refacciones.Procesos.PRE;
using BPMO.Refacciones.Procesos.VIS;
using System.Web.UI.WebControls;
using System.ComponentModel;
using BPMO.Refacciones.BO;
using System.Collections;
using System.Text.RegularExpressions;

namespace BPMO.Refacciones.UI.Procesos.UI {
    public partial class MantenimientoConfiguracionTransferenciaUI : Page, IMantenimientoConfiguracionTransferenciaVIS {
        #region Atributos
        ConfiguracionTransferenciaPRE presentador = null;

        public ArrayList listaNivelABCRelacion = new ArrayList();
        public ArrayList listaNivelABCCatalogo = new ArrayList();
        #endregion
        #region Propiedades
        #region Variables de Session
        public UsuarioBO UsuarioSesion {
            get {
                return (this.Session["Usuario"] != null) ? (UsuarioBO)this.Session["Usuario"] : null;
            }
        }
        protected List<DatosConexionBO> ListadoDatosConexion {
            get {
                return (this.Session["DatosConexion"] == null) ? null : (List<DatosConexionBO>)this.Session["DatosConexion"];
            }
        }
        public AdscripcionBO Adscripcion {
            get {
                return ((SiteMaster)this.Master).Adscripcion;
            }
        }
        public string ViewState_Guid {
            get {
                if (ViewState["GuidSession"] == null) {
                    Guid guid = Guid.NewGuid();
                    ViewState["GuidSession"] = guid.ToString();
                }
                return ViewState["GuidSession"].ToString();
            }
        }
        public ConfiguracionTransferenciaPRE.ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ConfiguracionTransferenciaPRE.ECatalogoBuscador)ViewState["BUSQUEDA"];
            }
            set {
                ViewState["BUSQUEDA"] = value;
            }
        }
        protected object Session_ObjetoBuscador {
            get {
                object objeto;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);
                else objeto = null;
                return objeto;
            }
            set {
                if (value != null)
                    Session[ViewState_Guid] = value;
                else
                    Session.Remove(ViewState_Guid);
            }
        }
        protected object Session_BOSelecto {
            get {
                object objeto;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);
                else objeto = null;
                return objeto;
            }
            set {
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (value != null)
                    Session[nombreSession] = value;
                else
                    Session.Remove(nombreSession);
            }
        }
        #endregion
        public int? Id {
            get {
                return (String.IsNullOrEmpty(this.lblId.Text)) ? null : (int?)int.Parse(this.lblId.Text.Trim());
            }
            set {
                this.lblId.Text = value != null ? value.ToString() : string.Empty;
            }
        }
        public int? EmpresaId {
            get {
                int _empresaId;
                if (!int.TryParse(this.hdnEmpresaId.Value, out _empresaId)) {
                    return null;
                }
                return _empresaId;
            }
            set {
                this.hdnEmpresaId.Value = value != null ? value.ToString() : null;
            }
        }
        public string NombreEmpresa {
            get {
                return (String.IsNullOrEmpty(this.txtEmpresa.Text)) ? null : this.txtEmpresa.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtEmpresa.Text = value.ToUpper();
                else
                    this.txtEmpresa.Text = string.Empty;
            }
        }
        public int? SucursalId {
            get {
                int _sucursalId;
                if (!int.TryParse(this.hdnSucursalId.Value, out _sucursalId)) {
                    return null;
                }
                return _sucursalId;
            }
            set {
                this.hdnSucursalId.Value = value != null ? value.ToString() : null;
            }
        }
        public string NombreSucursal {
            get {
                return (String.IsNullOrEmpty(this.txtSucursal.Text)) ? null : this.txtSucursal.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtSucursal.Text = value.ToUpper();
                else
                    this.txtSucursal.Text = string.Empty;
            }
        }
        public int? AlmacenId {
            get {
                short _almacenId;
                if (!short.TryParse(this.hdnAlmacenId.Value, out _almacenId)) {
                    return null;
                }
                return _almacenId;
            }
            set {
                this.hdnAlmacenId.Value = value != null ? value.ToString() : null;
            }
        }
        public string NombreAlmacen {
            get {
                return (String.IsNullOrEmpty(this.txtAlmacen.Text)) ? null : this.txtAlmacen.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtAlmacen.Text = value.ToUpper();
                else
                    this.txtAlmacen.Text = string.Empty;
            }
        }
        public TipoPedidoBO TipoPedidoBO {
            get {
                int _tipoPedidoId;
                TipoPedidoBO tipoPedidoBO = new TipoPedidoBO();
                if (!int.TryParse(this.hdnTipoPedidoId.Value, out _tipoPedidoId)) {
                    return null;
                }
                tipoPedidoBO.Id = _tipoPedidoId;
                return tipoPedidoBO;
            }
            set {
                this.hdnTipoPedidoId.Value = value != null ? value.ToString() : null;
            }
        }
        public int? maximoArticulosLinea {
            get {
                int _maximoArticulosLinea;
                if (!int.TryParse(this.txtMaximoArticulosLinea.Text, out _maximoArticulosLinea)) {
                    return null;
                }
                return _maximoArticulosLinea;
            }
            set {
                this.txtMaximoArticulosLinea.Text = value != null ? value.ToString() : null;
            }
        }
        public int? maximoLineas {
            get {
                int _maximoLineas;
                if (!int.TryParse(this.txtMaximoLineas.Text, out _maximoLineas)) {
                    return null;
                }
                return _maximoLineas;
            }
            set {
                this.txtMaximoLineas.Text = value != null ? value.ToString() : null;
            }
        }
        public int? TipoPedidoId {
            get {
                int _tipoPedidoId;
                if (!int.TryParse(this.hdnTipoPedidoId.Value, out _tipoPedidoId)) {
                    return null;
                }
                return _tipoPedidoId;
            }
            set {
                this.hdnTipoPedidoId.Value = value != null ? value.ToString() : null;
            }
        }
        public string NombreTipoPedido {
            get {
                return (String.IsNullOrEmpty(this.txtTipoPedido.Text)) ? null : this.txtTipoPedido.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtTipoPedido.Text = value.ToUpper();
                else
                    this.txtTipoPedido.Text = string.Empty;
            }
        }
        public bool? Activo {
            get {
                bool? estatus = null;
                if (this.rbtnActivo.Checked)
                    estatus = true;
                else if (this.rbtnInactivo.Checked)
                    estatus = false;
                return estatus;
            }
            set {
                if (value != null) {
                    if (value.Value) {
                        this.rbtnActivo.Checked = true;
                        this.rbtnInactivo.Checked = false;
                    } else {
                        this.rbtnActivo.Checked = false;
                        this.rbtnInactivo.Checked = true;
                    }
                } else {
                    this.rbtnActivo.Checked = false;
                    this.rbtnInactivo.Checked = false;
                }
            }
        }
        public bool? confCantidadActivo {
            get {
                bool? estatus = null;
                if (this.chkCantidadActivo.Checked)
                    estatus = true;
                else if (!this.chkCantidadActivo.Checked)
                    estatus = false;
                return estatus;
            }
            set {
                if (value != null) {
                    if (value.Value) {
                        this.chkCantidadActivo.Checked = true;
                    } else {
                        this.chkCantidadActivo.Checked = false;
                    }
                } else {
                    this.chkCantidadActivo.Checked = false;
                }
            }
        }
        public bool? confHoraActivo {
            get {
                bool? estatus = null;
                if (this.chkHoraActivo.Checked)
                    estatus = true;
                else if (!this.chkHoraActivo.Checked)
                    estatus = false;
                return estatus;
            }
            set {
                if (value != null) {
                    if (value.Value) {
                        this.chkHoraActivo.Checked = true;
                    } else {
                        this.chkHoraActivo.Checked = false;
                    }
                } else {
                    this.chkHoraActivo.Checked = false;
                }
            }
        }
        public int? confCantidadLunes {
            get {
                int _confCantidadLunes;
                if (!int.TryParse(this.txtCantidadLunes.Text, out _confCantidadLunes)) {
                    return null;
                }
                return _confCantidadLunes;
            }
            set {
                this.txtCantidadLunes.Text = value != null ? value.ToString() : null;
            }
        }
        public int? confCantidadMartes {
            get {
                int _confCantidadMartes;
                if (!int.TryParse(this.txtCantidadMartes.Text, out _confCantidadMartes)) {
                    return null;
                }
                return _confCantidadMartes;
            }
            set {
                this.txtCantidadMartes.Text = value != null ? value.ToString() : null;
            }
        }
        public int? confCantidadMiercoles {
            get {
                int _confCantidadMiercoles;
                if (!int.TryParse(this.txtCantidadMiercoles.Text, out _confCantidadMiercoles)) {
                    return null;
                }
                return _confCantidadMiercoles;
            }
            set {
                this.txtCantidadMiercoles.Text = value != null ? value.ToString() : null;
            }
        }
        public int? confCantidadJueves {
            get {
                int _confCantidadJueves;
                if (!int.TryParse(this.txtCantidadJueves.Text, out _confCantidadJueves)) {
                    return null;
                }
                return _confCantidadJueves;
            }
            set {
                this.txtCantidadJueves.Text = value != null ? value.ToString() : null;
            }
        }
        public int? confCantidadViernes {
            get {
                int _confCantidadViernes;
                if (!int.TryParse(this.txtCantidadViernes.Text, out _confCantidadViernes)) {
                    return null;
                }
                return _confCantidadViernes;
            }
            set {
                this.txtCantidadViernes.Text = value != null ? value.ToString() : null;
            }
        }
        public int? confCantidadSabado {
            get {
                int _confCantidadSabado;
                if (!int.TryParse(this.txtCantidadSabado.Text, out _confCantidadSabado)) {
                    return null;
                }
                return _confCantidadSabado;
            }
            set {
                this.txtCantidadSabado.Text = value != null ? value.ToString() : null;
            }
        }
        public int? confCantidadDomingo {
            get {
                int _confCantidadDomingo;
                if (!int.TryParse(this.txtCantidadDomingo.Text, out _confCantidadDomingo)) {
                    return null;
                }
                return _confCantidadDomingo;
            }
            set {
                this.txtCantidadDomingo.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraLunes {
            get {
                TimeSpan _confHoraLunes;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraLunes.Text)) {
                    return null;
                } else {
                    _confHoraLunes = TimeSpan.Parse(this.txtHoraLunes.Text);
                }
                return _confHoraLunes;
            }
            set {
                this.txtHoraLunes.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraMartes {
            get {
                TimeSpan _confHoraMartes;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraMartes.Text)) {
                    return null;
                } else {
                    _confHoraMartes = TimeSpan.Parse(this.txtHoraMartes.Text);
                }
                return _confHoraMartes;
            }
            set {
                this.txtHoraMartes.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraMiercoles {
            get {
                TimeSpan _confHoraMiercoles;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraMiercoles.Text)) {
                    return null;
                } else {
                    _confHoraMiercoles = TimeSpan.Parse(this.txtHoraMiercoles.Text);
                }
                return _confHoraMiercoles;
            }
            set {
                this.txtHoraMiercoles.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraJueves {
            get {
                TimeSpan _confHoraJueves;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraJueves.Text)) {
                    return null;
                } else {
                    _confHoraJueves = TimeSpan.Parse(this.txtHoraJueves.Text);
                }
                return _confHoraJueves;
            }
            set {
                this.txtHoraJueves.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraViernes {
            get {
                TimeSpan _confHoraViernes;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraViernes.Text)) {
                    return null;
                } else {
                    _confHoraViernes = TimeSpan.Parse(this.txtHoraViernes.Text);
                }
                return _confHoraViernes;
            }
            set {
                this.txtHoraViernes.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraSabado {
            get {
                TimeSpan _confHoraSabado;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraSabado.Text)) {
                    return null;
                } else {
                    _confHoraSabado = TimeSpan.Parse(this.txtHoraSabado.Text);
                }
                return _confHoraSabado;
            }
            set {
                this.txtHoraSabado.Text = value != null ? value.ToString() : null;
            }
        }
        public TimeSpan? confHoraDomingo {
            get {
                TimeSpan _confHoraDomingo;
                Regex checartiempo = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]|:[0-5][0-9]$");
                if (!checartiempo.IsMatch(this.txtHoraDomingo.Text)) {
                    return null;
                } else {
                    _confHoraDomingo = TimeSpan.Parse(this.txtHoraDomingo.Text);
                }
                return _confHoraDomingo;
            }
            set {
                this.txtHoraDomingo.Text = value != null ? value.ToString() : null;
            }
        }
        #region DatosBitacora
        public string UsuarioCreacionBitacora {
            set { this.txtCreadoBitacora.Text = value ?? string.Empty; }
        }
        public DateTime? FechaCreacionBitacora {
            set { this.txtFechaCreacionBitacora.Text = value != null ? String.Format("{0} {1}", value.Value.ToShortDateString(), value.Value.ToLongTimeString()) : string.Empty; }
        }
        public string UsuarioActualizacionBitacora {
            set { this.txtActualizadoBitacora.Text = value ?? string.Empty; }
        }
        public DateTime? FechaActualizacionBitacora {
            set { this.txtFechaActualizacionBitacora.Text = value != null ? String.Format("{0} {1}", value.Value.ToShortDateString(), value.Value.ToLongTimeString()) : string.Empty; }
        }
        #endregion
        public List<NivelABCBO> NivelABCBO {
            get {
                return (Session["NivelABC"] != null) ? (List<NivelABCBO>)Session["NivelABC"] : null;
            }
            set {
                if (value != null)
                    this.Session.Add("NivelABC", value);
                else
                    this.Session.Remove("NivelABC");
                this.lbNivelABC.DataSource = value;                
                this.lbNivelABC.DataBind();
            }
        }
        public List<NivelABCBO> ConfNivelABCBO {
            get {
                return (Session["NivelABCconf"] != null) ? (List<NivelABCBO>)Session["NivelABCconf"] : null;
            }
            set {
                if (value != null)
                    this.Session.Add("NivelABCconf", value);
                else
                    this.Session.Remove("NivelABCconf");
                this.lbNivelABCRel.DataSource = value;
                this.lbNivelABCRel.DataBind();
            }
        }
        public object ConfiguracionTransferenciaBase {
            get {
                return this.Session["CONFIGTRANSBASE"] ?? null;
            }
            set {
                if (value != null)
                    this.Session.Add("CONFIGTRANSBASE", value);
                else
                    this.Session.Remove("CONFIGTRANSBASE");
            }
        }
        #endregion
        #region Métodos
        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                presentador = new ConfiguracionTransferenciaPRE(this, ListadoDatosConexion);
                if (!IsPostBack) {
                    #region Código de seguridad para las UI
                    //Verifica que el proceso actual exista en la lista de procesos permitidos para el usuario actual
                    SiteMaster miMaster = (SiteMaster)this.Master;
                    Security.BR.SecurityBR securityBR = new Security.BR.SecurityBR(new SeguridadBO(Guid.Empty, miMaster.Usuario, miMaster.Adscripcion));
                    miMaster.VerificarProcesoActual(securityBR.ProcesoActual);
                    #endregion
                    presentador.DesplegarcatalogoNivelABC();
                    if (this.Session["ConfiguracionId"] != null) {
                        int id = 0;
                        bool esNumero = int.TryParse(this.Session["ConfiguracionId"].ToString(), out id);
                        if (esNumero) {
                            Id = (id > 0) ? (int?)id : null;
                            presentador.DesplegarDetalleConfiguracionTransferencia(Id);
                            string tipoUI = Request.QueryString["ui"];
                            if (tipoUI != null && tipoUI.Equals("e"))
                                presentador.PreparaUIEdicion();
                        }
                        this.Session.Remove("ConfiguracionId");
                    } else {
                        this.Session.Remove("NivelABCconf");
                        PreparaUIInsertar();
                    }
                    System.Web.HttpBrowserCapabilities browser = Request.Browser;
                    string stringFormatoHora ="El formato de hora para cada campo es: ";
                    switch (browser.Browser) {
                        case "Chrome":
                            stringFormatoHora += " HH:mm a.m.|p.m.  (12 hrs)";
                            break;
                        case "InternetExplorer":
                            stringFormatoHora += " HH:mm:ss (24 hrs)";
                            break;
                        case "Firefox":
                            stringFormatoHora += " HH:mm (24 hrs)";
                            break;
                        case "Safari":
                            stringFormatoHora += " HH:mm (24 hrs)";
                            break;
                        default:
                            stringFormatoHora +=" Desconocido";
                            break;
                    }
                    txtFormatoHora.Text = stringFormatoHora;
                }
            } catch (Exception ex) {
                PreparaUIInsertar();
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
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
        /// Prepara la Interfaz para Inserción
        /// </summary>
        public void PreparaUIInsertar() {
            this.rbtnActivo.Checked = true;
            this.chkCantidadActivo.Checked = true;
            this.chkHoraActivo.Checked = true;
            this.trEstatus.Visible = false;
            this.btnEditar.Enabled = false;
            this.btnGuardar.Enabled = true;
            this.btnGuardar.CommandName = "INSERTAR";
            this.ConfiguracionTransferenciaBase = null;
            this.ibtnBuscaEmpresa.Visible = true;
            this.ibtnBuscaSucursal.Visible = true;
            this.ibtnBuscaAlmacen.Visible = true;
            this.ibtnBuscaTipoPedido.Visible = true;
            this.DesactivarTexts(false);
            this.btnCancelar.Enabled = true;
            this.lbNivelABC.Enabled = true;
            this.lbNivelABCRel.Enabled = true;
            this.hdnTipoAccion.Value = "INSERTAR";
            this.lblEncabezadoLeyenda.Text = "CATÁLOGOS - REGISTRAR CONFIGURACIÓN";
            this.AsignarClaseCss();
            this.tNumeroCatalogo.Visible = false;
        }
        /// <summary>
        /// Prepara la Interfaz para Actualización de la configuración
        /// </summary>
        public void PreparaUIActualizar() {
            this.rbtnActivo.Enabled = true;
            this.chkCantidadActivo.Enabled = true;
            this.chkHoraActivo.Enabled = true;
            this.rbtnInactivo.Enabled = true;
            this.trEstatus.Visible = true;
            this.btnGuardar.Enabled = true;
            this.btnEditar.Enabled = false;
            this.btnGuardar.CommandName = "EDITAR";
            this.DesactivarTexts(false);
            this.ibtnBuscaEmpresa.Visible = true;
            this.ibtnBuscaSucursal.Visible = true;
            this.ibtnBuscaAlmacen.Visible = true;
            this.ibtnBuscaTipoPedido.Visible = true;
            this.btnCancelar.Enabled = true;
            this.lbNivelABC.Enabled = true;
            this.lbNivelABCRel.Enabled = true;
            this.hdnTipoAccion.Value = "EDITAR";
            this.lblEncabezadoLeyenda.Text = "CATÁLOGOS - EDITAR CONFIGURACIÓN";
            this.AsignarClaseCss();
            this.tNumeroCatalogo.Visible = true;
        }
        /// <summary>
        /// Prepara la Interfaz para desplegar Detalle de la configuración
        /// </summary>
        public void PreparaUIDetalle() {
            this.rbtnActivo.Enabled = false;
            this.chkCantidadActivo.Enabled = false;
            this.chkHoraActivo.Enabled = false;
            this.rbtnInactivo.Enabled = false;
            this.trEstatus.Visible = true;
            this.btnGuardar.Enabled = false;
            this.btnEditar.Enabled = true;
            this.DesactivarTexts(true);
            this.ibtnBuscaEmpresa.Visible = false;
            this.ibtnBuscaSucursal.Visible = false;
            this.ibtnBuscaAlmacen.Visible = false;
            this.ibtnBuscaTipoPedido.Visible = false;
            this.btnCancelar.Enabled = false;
            this.lbNivelABC.Enabled = false;
            this.lbNivelABCRel.Enabled = false;
            this.hdnTipoAccion.Value = "EDITAR";
            this.lblEncabezadoLeyenda.Text = "CATÁLOGOS - CONSULTAR CONFIGURACIÓN";
            this.AsignarClaseCss(false, false, true);
            this.tNumeroCatalogo.Visible = true;
        }
        /// <summary>
        /// Asigna un clase css a los botones de acuerdo al tipo de operación que se va a realizar
        /// </summary>
        /// <param name="guardar">Configuración para el botón guardar</param>
        /// <param name="cancelar">Configuración para el botón cancelar</param>
        /// <param name="editar">Configuración para el botón editar</param>
        private void AsignarClaseCss(bool guardar = true, bool cancelar = true, bool editar = false) {
            this.btnGuardar.CssClass = guardar ? "BotonAceptar" : "BotonAceptarDeshabilitado";
            this.btnCancelar.CssClass = cancelar ? "BotonCancelar" : "BotonCancelarDeshabilitado";
            this.btnEditar.CssClass = editar ? "BotonEditar" : "BotonEditarDeshabilitado";
        }
        /// <summary>
        /// Desactiva los texts de la UI
        /// </summary>
        /// <param name="readOnly">Indica si el control será de solo lectura</param>
        private void DesactivarTexts(bool readOnly) {
            this.txtEmpresa.ReadOnly =
            this.txtSucursal.ReadOnly =
            this.txtAlmacen.ReadOnly =
            this.txtTipoPedido.ReadOnly =
            this.txtMaximoArticulosLinea.ReadOnly =
            this.txtMaximoLineas.ReadOnly =
            this.txtCantidadLunes.ReadOnly =
            this.txtCantidadMartes.ReadOnly =
            this.txtCantidadMiercoles.ReadOnly =
            this.txtCantidadJueves.ReadOnly =
            this.txtCantidadViernes.ReadOnly =
            this.txtCantidadSabado.ReadOnly =
            this.txtCantidadDomingo.ReadOnly =
            this.txtHoraLunes.ReadOnly =
            this.txtHoraMartes.ReadOnly =
            this.txtHoraMiercoles.ReadOnly =
            this.txtHoraJueves.ReadOnly =
            this.txtHoraViernes.ReadOnly =
            this.txtHoraSabado.ReadOnly =
            this.txtHoraDomingo.ReadOnly = readOnly;
            this.lbNivelABC.Enabled = !readOnly;
            this.lbNivelABCRel.Enabled = !readOnly;
            this.imgBtnAgregar.Enabled = !readOnly;
            this.imgBtnQuitar.Enabled = !readOnly;
            this.chkCantidadActivo.Enabled = !readOnly;
            this.chkHoraActivo.Enabled = !readOnly;
            this.txtEmpresa.CssClass =
            this.txtSucursal.CssClass =
            this.txtAlmacen.CssClass =
            this.txtTipoPedido.CssClass =
            this.txtMaximoArticulosLinea.CssClass =
            this.txtMaximoLineas.CssClass =
            this.txtCantidadLunes.CssClass =
            this.txtCantidadMartes.CssClass =
            this.txtCantidadMiercoles.CssClass =
            this.txtCantidadJueves.CssClass =
            this.txtCantidadViernes.CssClass =
            this.txtCantidadSabado.CssClass =
            this.txtCantidadDomingo.CssClass =
            this.txtHoraLunes.CssClass =
            this.txtHoraMartes.CssClass =
            this.txtHoraMiercoles.CssClass =
            this.txtHoraJueves.CssClass =
            this.txtHoraViernes.CssClass =
            this.txtHoraSabado.CssClass =
            this.txtHoraDomingo.CssClass =
            this.chkCantidadActivo.CssClass =
            this.chkHoraActivo.CssClass =
            this.lbNivelABC.CssClass =
            this.lbNivelABCRel.CssClass =
            this.imgBtnAgregar.CssClass =
            this.imgBtnQuitar.CssClass = (readOnly) ? "textBoxDisabled" : null;
        }
        #region Uso del Buscador
        /// <summary>
        /// Buscar Empresa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaEmpresa_Click(object sender, ImageClickEventArgs e) {
            try {
                string xml = "EmpresaLider";
                bool origenTxt = hdnBuscador.Value.Equals("1");
                bool limpiarTxt = false;
                if (origenTxt && string.IsNullOrEmpty(this.NombreEmpresa)) {
                    limpiarTxt = true;
                } else {
                    if (!origenTxt) xml += "&hidden=0";
                    this.EjecutaBuscador(xml, ConfiguracionTransferenciaPRE.ECatalogoBuscador.Empresa);
                }
                if (origenTxt || limpiarTxt) {
                    this.NombreEmpresa = null;
                    this.EmpresaId = null;
                    this.NombreSucursal = null;
                    this.SucursalId = null;
                    this.NombreAlmacen = null;
                    this.AlmacenId = null;
                }
                hdnBuscador.Value = "";
            } catch (Exception ex) {
                this.MostrarMensaje("No se pudo buscar la empresa", ETipoMensajeIU.ADVERTENCIA, ex.Message);
            }
        }
        /// <summary>
        /// Buscar Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaSucursal_Click(object sender, ImageClickEventArgs e) {
            try {
                string xml = "SucursalLider";
                bool origenTxt = hdnBuscador.Value.Equals("1");
                bool limpiarTxt = false;
                if (origenTxt && string.IsNullOrEmpty(this.NombreSucursal)) {
                    limpiarTxt = true;
                } else {
                    if (!origenTxt) xml += "&hidden=0";
                    this.EjecutaBuscador(xml, ConfiguracionTransferenciaPRE.ECatalogoBuscador.Sucursal);
                }
                if (origenTxt || limpiarTxt) {
                    this.NombreSucursal = null;
                    this.SucursalId = null;
                    this.NombreAlmacen = null;
                    this.AlmacenId = null;
                }
                hdnBuscador.Value = "";
            } catch (Exception ex) {
                this.MostrarMensaje("No se pudo buscar la Sucursal", ETipoMensajeIU.ADVERTENCIA, ex.Message);
            }
        }
        /// <summary>
        /// Buscar Almacén
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaAlmacen_Click(object sender, ImageClickEventArgs e) {
            try {
                string xml = "AlmacenLider";
                bool origenTxt = hdnBuscador.Value.Equals("1");
                bool limpiarTxt = false;
                if (origenTxt && string.IsNullOrEmpty(this.NombreAlmacen)) {
                    limpiarTxt = true;
                } else {
                    if (!origenTxt) xml += "&hidden=0";
                    this.EjecutaBuscador(xml, ConfiguracionTransferenciaPRE.ECatalogoBuscador.Almacen);
                }
                if (origenTxt || limpiarTxt) {
                    this.NombreAlmacen = null;
                    this.AlmacenId = null;
                }
                hdnBuscador.Value = "";
            } catch (Exception ex) {
                this.MostrarMensaje("No se pudo buscar el Almacén", ETipoMensajeIU.ADVERTENCIA, ex.Message);
            }
        }
        /// <summary>
        /// Buscar Tipo de Pedido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaTipoPedido_Click(object sender, ImageClickEventArgs e) {
            try {
                string xml = "TipoPedido";
                bool origenTxt = hdnBuscador.Value.Equals("1");
                bool limpiarTxt = false;
                if (origenTxt && string.IsNullOrEmpty(this.NombreTipoPedido)) {
                    limpiarTxt = true;
                } else {
                    if (!origenTxt) xml += "&hidden=0";
                    this.EjecutaBuscador(xml, ConfiguracionTransferenciaPRE.ECatalogoBuscador.TipoPedido);
                }
                if (origenTxt || limpiarTxt) {
                    this.NombreTipoPedido = null;
                    this.TipoPedidoId = null;
                }
                hdnBuscador.Value = "";
            } catch (Exception ex) {
                this.MostrarMensaje("No se pudo buscar el Tipo de Pedido", ETipoMensajeIU.ADVERTENCIA, ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del catálogo</param>
        /// <param name="tipoBusqueda">Tipo del catálogo</param>
        private void EjecutaBuscador(string catalogo, ConfiguracionTransferenciaPRE.ECatalogoBuscador tipoBusqueda) {
            this.ViewState_Catalogo = tipoBusqueda;
            this.Session_ObjetoBuscador = presentador.ObtenerObjetoBusqueda(tipoBusqueda, this);
            this.Session_BOSelecto = null;
            if (this.Session_ObjetoBuscador != null)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Events", "BtnBuscar('" + this.ViewState_Guid + "','" + catalogo + "');", true);
        }
        /// <summary>
        /// Se ejecuta cuando se selecciona un elemento en el buscador y se encarga de agregar la información del elemento seleccionado 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                presentador.DesplegarBusqueda(this.ViewState_Catalogo, this.Session_BOSelecto, this);
                this.Session_BOSelecto = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region Operaciones
        /// <summary>
        /// Botón para ejecutar el registro de la Configuración
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnGuardar_Click(object sender, EventArgs e) {
            try {
                if (this.btnGuardar.CommandName.Equals("INSERTAR"))
                    presentador.Insertar();
                if (this.btnGuardar.CommandName.Equals("EDITAR"))
                    presentador.Actualizar();
            } catch (Exception ex) {
                MostrarMensaje("Guardado no exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Botón para ejecutar la Actualización de la Configuración
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnEditar_Click(object sender, EventArgs e) {
            try {
                presentador.PreparaUIEdicion();
            } catch (Exception ex) {
                MostrarMensaje("Guardado no exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Cancela la edición
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnCancelar_Click(object sender, EventArgs e) {
            try {
                if (this.btnGuardar.CommandName.Equals("EDITAR")) {
                    presentador.CancelarEdicion();
                } else {
                    this.Response.Redirect("~/Procesos.UI/BuscadorConfiguracionTransferenciasUI.aspx");
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void btnAgregar_Click(object sender, ImageClickEventArgs e) {
            try {
                List<NivelABCBO> lstAux = new List<NivelABCBO>();
                for (int i = 0; i < lbNivelABC.Items.Count; i++) {
                    if (lbNivelABC.Items[i].Selected)
                        lstAux.Add(this.NivelABCBO[i]);
                }
                this.presentador.AgregarConfiguracionABC(lstAux);
            } catch (Exception ex) {
                MostrarMensaje("Error con la configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void btnQuitar_Click(object sender, ImageClickEventArgs e) {
            try {
                List<NivelABCBO> lstAux = new List<NivelABCBO>();
                for (int i = 0; i < lbNivelABCRel.Items.Count; i++) {
                    if (lbNivelABCRel.Items[i].Selected)
                        lstAux.Add(this.ConfNivelABCBO[i]);                        
                }
                this.presentador.QuitarConfiguracionABC(lstAux);
            } catch (Exception ex) {
                MostrarMensaje("Error con la configuración", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void lbtnCantidad_Click(object sender, EventArgs e) {
            try {
                this.pnlHora.Visible = false;
                this.pnlNivelABC.Visible = false;
                this.pnlCantidad.Visible = true;
                this.hdnMenuTaps.Value = "Cantidad";
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al despliegar la información de Unidad", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void lbtnHora_Click(object sender, EventArgs e) {
            try {
                this.pnlCantidad.Visible = false;
                this.pnlNivelABC.Visible = false;
                this.pnlHora.Visible = true;
                this.hdnMenuTaps.Value = "Hora";
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar las posiciones de trabajo.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        protected void lbtnNivelABC_Click(object sender, EventArgs e) {
            try {
                this.pnlCantidad.Visible = false;
                this.pnlHora.Visible = false;
                this.pnlNivelABC.Visible = true;
                this.hdnMenuTaps.Value = "NivelABC";
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar las posiciones de trabajo.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #endregion

    }
}