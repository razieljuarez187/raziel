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

namespace BPMO.Refacciones.UI.Procesos.UI {
    public partial class MantenimientoConfiguracionReglaUI : Page, IMantenimientoConfiguracionReglaVIS {
        #region Atributos
        ConfiguracionReglaUsuarioPRE presentador = null;
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
        public ConfiguracionReglaUsuarioPRE.ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ConfiguracionReglaUsuarioPRE.ECatalogoBuscador)ViewState["BUSQUEDA"];
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
        public int? UsuarioId {
            get {
                int _usuarioId;
                if (!int.TryParse(this.hdnUsuarioId.Value, out _usuarioId)) {
                    return null;
                }
                return _usuarioId;
            }
            set {
                this.hdnUsuarioId.Value = value != null ? value.ToString() : null;
            }
        }
        public string NombreUsuario {
            get {
                return (String.IsNullOrEmpty(this.txtUsuario.Text)) ? null : this.txtUsuario.Text.Trim().ToUpper();
            }
            set {
                if (value != null)
                    this.txtUsuario.Text = value.ToUpper();
                else
                    this.txtUsuario.Text = string.Empty;
            }
        }
        public ETipoReglaUsuario? TipoRegla {
            get {
                if (ddlTipoRegla.SelectedValue != "-1")
                    return (ETipoReglaUsuario)int.Parse(ddlTipoRegla.SelectedValue);

                return null;
            }
            set {
                if (value != null) {
                    int valueIndex = (int)value.Value;
                    ddlTipoRegla.SelectedIndex = ddlTipoRegla.Items.IndexOf(ddlTipoRegla.Items.FindByValue(valueIndex.ToString()));
                }
            }
        }
        public decimal? ValorInicial {
            get {
                decimal _valorIni;
                if (!decimal.TryParse(this.txtValorInicial.Text, out _valorIni)) {
                    return null;
                }
                return _valorIni;
            }
            set {
                this.txtValorInicial.Text = value != null ? value.ToString() : null;
            }
        }
        public decimal? ValorFinal {
            get {
                decimal _valorFin;
                if (!decimal.TryParse(this.txtValorFinal.Text, out _valorFin)) {
                    return null;
                }
                return _valorFin;
            }
            set {
                this.txtValorFinal.Text = value != null ? value.ToString() : null;
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

        public object ConfiguracionReglaBase {
            get {
                return this.Session["CONFIGREGLABASE"] ?? null;
            }
            set {
                if (value != null)
                    this.Session.Add("CONFIGREGLABASE", value);
                else
                    this.Session.Remove("CONFIGREGLABASE");
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
                presentador = new ConfiguracionReglaUsuarioPRE(this, ListadoDatosConexion);
                if (!IsPostBack) {
                    #region Código de seguridad para las UI
                    //Verifica que el proceso actual exista en la lista de procesos permitidos para el usuario actual
                    SiteMaster miMaster = (SiteMaster)this.Master;
                    Security.BR.SecurityBR securityBR = new Security.BR.SecurityBR(new SeguridadBO(Guid.Empty, miMaster.Usuario, miMaster.Adscripcion));
                    miMaster.VerificarProcesoActual(securityBR.ProcesoActual);
                    #endregion
                    this.Session.Remove("CONFIGREGLABASE");
                    CargarTiposRegla();

                    if (this.Session["ConfiguracionReglaId"] != null) {
                        int id = 0;
                        bool esNumero = int.TryParse(this.Session["ConfiguracionReglaId"].ToString(), out id);
                        if (esNumero) {
                            Id = (id > 0) ? (int?)id : null;
                            presentador.DesplegarDetalleConfiguracionRegla(Id);
                            string tipoUI = Request.QueryString["ui"];
                            if (tipoUI != null && tipoUI.Equals("e"))
                                presentador.PreparaUIEdicion();
                        }
                        this.Session.Remove("ConfiguracionReglaId");
                    } else {
                        PreparaUIInsertar();
                    }
                }
            } catch (Exception ex) {
                PreparaUIInsertar();
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Carga los datos de los tipos de ConfiguracionRegla
        /// </summary>
        private void CargarTiposRegla() {
            List<ETipoReglaUsuario> listado = new List<ETipoReglaUsuario>(Enum.GetValues(typeof(ETipoReglaUsuario)).Cast<ETipoReglaUsuario>());
            var Lista = new List<KeyValuePair<int, string>>();
            if (listado != null) {
                Lista.AddRange(
                    from tipoR in listado
                    let tipo = ((DescriptionAttribute)tipoR.GetType().GetField(tipoR.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description
                    select new KeyValuePair<int, string>((int)tipoR, tipo));
            }

            // Agregar el Item de fachada
            Lista.Insert(0, new KeyValuePair<int, string>(-1, "NINGUNO"));
            //Limpiar el DropDownList Actual
            ddlTipoRegla.Items.Clear();
            // Asignar Lista al DropDownList
            ddlTipoRegla.DataTextField = "Value";
            ddlTipoRegla.DataValueField = "Key";
            ddlTipoRegla.DataSource = Lista;
            ddlTipoRegla.DataBind();
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
            this.trEstatus.Visible = false;
            this.btnEditar.Enabled = false;
            this.btnGuardar.Enabled = true;
            this.btnGuardar.CommandName = "INSERTAR";
            this.ConfiguracionReglaBase = null;
            this.ibtnBuscaEmpresa.Visible = true;
            this.ibtnBuscaSucursal.Visible = true;
            this.ibtnBuscaAlmacen.Visible = true;
            this.ibtnBuscaUsuario.Visible = true;
            this.DesactivarTexts(false);
            this.btnCancelar.Enabled = true;
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
            this.rbtnInactivo.Enabled = true;
            this.trEstatus.Visible = true;
            this.btnGuardar.Enabled = true;
            this.btnEditar.Enabled = false;
            this.btnGuardar.CommandName = "EDITAR";
            this.DesactivarTexts(false);
            this.ibtnBuscaEmpresa.Visible = true;
            this.ibtnBuscaSucursal.Visible = true;
            this.ibtnBuscaAlmacen.Visible = true;
            this.ibtnBuscaUsuario.Visible = true;
            this.btnCancelar.Enabled = true;
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
            this.rbtnInactivo.Enabled = false;
            this.trEstatus.Visible = true;
            this.btnGuardar.Enabled = false;
            this.btnEditar.Enabled = true;
            this.DesactivarTexts(true);
            this.ibtnBuscaEmpresa.Visible = false;
            this.ibtnBuscaSucursal.Visible = false;
            this.ibtnBuscaAlmacen.Visible = false;
            this.ibtnBuscaUsuario.Visible = false;
            this.btnCancelar.Enabled = false;
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
            this.txtUsuario.ReadOnly =
            this.txtValorInicial.ReadOnly =
            this.txtValorFinal.ReadOnly = readOnly;
            this.ddlTipoRegla.Enabled = !readOnly;
            this.txtEmpresa.CssClass =
            this.txtSucursal.CssClass =
            this.txtAlmacen.CssClass =
            this.txtUsuario.CssClass =
            this.txtValorInicial.CssClass =
            this.txtValorFinal.CssClass =
            this.ddlTipoRegla.CssClass = (readOnly) ? "textBoxDisabled" : null;
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
                    this.EjecutaBuscador(xml, ConfiguracionReglaUsuarioPRE.ECatalogoBuscador.Empresa);
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
                    this.EjecutaBuscador(xml, ConfiguracionReglaUsuarioPRE.ECatalogoBuscador.Sucursal);
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
                    this.EjecutaBuscador(xml, ConfiguracionReglaUsuarioPRE.ECatalogoBuscador.Almacen);
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
        /// Buscar Usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnBuscaUsuario_Click(object sender, ImageClickEventArgs e) {
            try {
                string xml = "UsuarioLider";
                bool origenTxt = hdnBuscador.Value.Equals("1");
                bool limpiarTxt = false;
                if (origenTxt && string.IsNullOrEmpty(this.NombreUsuario)) {
                    limpiarTxt = true;
                } else {
                    if (!origenTxt) xml += "&hidden=0";
                    this.EjecutaBuscador(xml, ConfiguracionReglaUsuarioPRE.ECatalogoBuscador.Usuario);
                }
                if (origenTxt || limpiarTxt) {
                    this.NombreUsuario = null;
                    this.UsuarioId = null;
                }
                hdnBuscador.Value = "";
            } catch (Exception ex) {
                this.MostrarMensaje("No se pudo buscar el Usuario", ETipoMensajeIU.ADVERTENCIA, ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        /// <param name="catalogo">Nombre del catálogo</param>
        /// <param name="tipoBusqueda">Tipo del catálogo</param>
        private void EjecutaBuscador(string catalogo, ConfiguracionReglaUsuarioPRE.ECatalogoBuscador tipoBusqueda) {
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
                    this.Response.Redirect("~/Procesos.UI/BuscadorConfiguracionReglaUI.aspx");
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al cancelar", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #endregion
    }
}