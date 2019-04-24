using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.Enumeradores;
using BPMO.Refacciones.Master.UI;
using BPMO.Refacciones.Procesos.PRE;
using BPMO.Refacciones.Procesos.VIS;
using System.ComponentModel;

namespace BPMO.Refacciones.UI {
    /// <summary>
    /// UI para el manejo de Buscador de Cotizaciones
    /// </summary>
    public partial class BuscadorConfiguracionReglaUI : Page, IBuscadorConfiguracionReglaVIS {
        #region Atributos
        private ConfiguracionReglaUsuarioPRE presentador = null;
        #endregion
        #region Propiedades
        #region Variables de Session
        protected List<DatosConexionBO> ListadoDatosConexion {
            get { return (this.Session["DatosConexion"] == null) ? null : (List<DatosConexionBO>)this.Session["DatosConexion"]; }
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
                object objeto = null;
                if (Session[ViewState_Guid] != null)
                    objeto = (Session[ViewState_Guid] as object);

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
                object objeto = null;
                string nombreSession = String.Format("BOSELECTO_{0}", ViewState_Guid);
                if (Session[nombreSession] != null)
                    objeto = (Session[nombreSession] as object);

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
        protected int? Session_ConfiguracionId {
            set {
                if (value != null)
                    this.Session["ConfiguracionReglaId"] = value;
                else
                    this.Session.Remove("ConfiguracionReglaId");
            }
        }
        #endregion
        public AdscripcionBO Adscripcion {
            get {
                return (AdscripcionBO)this.Session["AdscripcionServicio"];
            }
        }
        public int? Id {
            get {
                int id;
                bool esNumero = int.TryParse(this.txtId.Text.Trim(), out id);
                if (esNumero == false) {
                    return null;
                }
                return id;
            }
            set {
                this.txtId.Text = value.ToString();
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
        }
        public bool? Activo {
            get {
                bool? estatus = null;
                if (!this.ddlEstatus.SelectedValue.Equals("null"))
                    estatus = bool.Parse(this.ddlEstatus.SelectedValue);
                return estatus;
            }
        }
        public List<ConfiguracionReglaUsuarioBO> ListadoConfiguracionRegla {
            get {
                return (Session["ConfiguracionesReglas"] != null) ? (List<ConfiguracionReglaUsuarioBO>)Session["ConfiguracionesReglas"] : null;
            }
            set {
                if (value != null)
                    this.Session.Add("ConfiguracionesReglas", value);
                else
                    this.Session.Remove("ConfiguracionesReglas");
                this.grvConfiguracionesReglas.DataSource = value;
                this.grvConfiguracionesReglas.DataBind();
            }
        }
        public string Orden {
            get {
                return (ViewState["SORT"] == null) ? "ASC" : (string)ViewState["SORT"];
            }
            set {
                ViewState.Add("SORT", value);
            }
        }
        #endregion
        #region Metodos
        /// <summary>
        /// Método Carga de la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            try{
                presentador = new ConfiguracionReglaUsuarioPRE(this, ListadoDatosConexion);
                if (!IsPostBack) {
                    #region Código de seguridad para las UI
                    //Verifica que el proceso actual exista en la lista de procesos permitidos para el usuario actual
                    SiteMaster miMaster = (SiteMaster)this.Master;
                    Security.BR.SecurityBR securityBR = new Security.BR.SecurityBR(new SeguridadBO(Guid.Empty, miMaster.Usuario, miMaster.Adscripcion));
                    miMaster.VerificarProcesoActual(securityBR.ProcesoActual);
                    #endregion

                    #region Aplicar el número de filas al grid
                    this.grvConfiguracionesReglas.PageSize = (int)miMaster.NumeroFilas;
                    #endregion

                    this.Session.Remove("ConfiguracionesReglas");
                    CargarTiposRegla();
                }
            } catch (Exception ex) {
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
            Lista.Insert(0, new KeyValuePair<int, string>(-1, "TODOS"));
            //Limpiar el DropDownList Actual
            ddlTipoRegla.Items.Clear();
            // Asignar Lista al DropDownList
            ddlTipoRegla.DataTextField = "Value";
            ddlTipoRegla.DataValueField = "Key";
            ddlTipoRegla.DataSource = Lista;
            ddlTipoRegla.DataBind();
        }
        
        #region Buscador
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
        /// Se ejecuta cuando se selecciona un elemento en el buscador y se encarga de agregar la información del elemento seleccionado 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResult_Click(object sender, EventArgs e) {
            try {
                presentador.DesplegarBusqueda(this.ViewState_Catalogo, this.Session_BOSelecto, this);
                this.Session_BOSelecto = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Ejecuta el buscador
        /// </summary>
        private void EjecutaBuscador(string catalogo, ConfiguracionReglaUsuarioPRE.ECatalogoBuscador tipoBusqueda) {
            this.ViewState_Catalogo = tipoBusqueda;
            this.Session_ObjetoBuscador = presentador.ObtenerObjetoBusqueda(tipoBusqueda, this);
            this.Session_BOSelecto = null;
            if (this.Session_ObjetoBuscador != null)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Events", "BtnBuscar('" + this.ViewState_Guid + "','" + catalogo + "');", true);

        } 
        #endregion

        /// <summary>
        /// Paginación del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionesReglas_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                this.grvConfiguracionesReglas.DataSource = this.ListadoConfiguracionRegla;
                this.grvConfiguracionesReglas.PageIndex = e.NewPageIndex;
                this.grvConfiguracionesReglas.DataBind();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias en el paginado de la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Seleccionar un registro del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionesReglas_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                string eCommandNameUpper = e.CommandName.ToString().ToUpper();
                if (eCommandNameUpper == "PAGE" || eCommandNameUpper == "SORT") return;
                int configReglaId = int.Parse(e.CommandArgument.ToString());
                if (configReglaId > -1) {
                    switch (eCommandNameUpper) {
                        case "CMDDETALLES":
                            this.Session_ConfiguracionId = configReglaId;
                            this.ListadoConfiguracionRegla = null;
                            Response.Redirect("~/Procesos.UI/MantenimientoConfiguracionReglaUI.aspx");
                            break;
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Aplicar Ordenamiento a los resultados de la búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionesReglas_Sorting(object sender, GridViewSortEventArgs e) {
            try {
                presentador.OrdenarListado(e.SortExpression, this.Orden);
                this.Orden = (this.Orden.Equals("ASC")) ? "DESC" : "ASC";
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Se le aplican datos al GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvConfiguracionesReglas_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    ConfiguracionReglaUsuarioBO configuracionRe = (ConfiguracionReglaUsuarioBO)e.Row.DataItem;
                    
                    Label lblEmpresa = (Label)e.Row.FindControl("lblEmpresa");
                    lblEmpresa.Text = configuracionRe.Empresa == null ? string.Empty : configuracionRe.Empresa.Nombre;
                    Label lblSucursal = (Label)e.Row.FindControl("lblSucursal");
                    lblSucursal.Text = configuracionRe.Sucursal == null ? string.Empty : configuracionRe.Sucursal.Nombre;
                    Label lblAlmacen = (Label)e.Row.FindControl("lblAlmacen");
                    lblAlmacen.Text = configuracionRe.Almacen == null ? string.Empty : configuracionRe.Almacen.Nombre;
                    Label lblUsuario = (Label)e.Row.FindControl("lblUsuario");
                    lblUsuario.Text = configuracionRe.Usuario == null ? string.Empty : configuracionRe.Usuario.Nombre;
                    if (configuracionRe.Activo.HasValue) {
                        CheckBox checkStatus = (CheckBox)e.Row.FindControl("chkActivo");
                        checkStatus.Checked = configuracionRe.Activo.Value;
                    }
                }
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Se envia a Editar la Configuración de Usuario
        /// </summary>
        /// <param name="configId">Identificador de la configuración</param>
        public void EnviarAEditarBO(int configId) {
            this.Session_ConfiguracionId = configId;
            this.ListadoConfiguracionRegla = null;
            Response.Redirect("~/Procesos.UI/MantenimientoConfiguracionReglaUI.aspx");
        }        
        /// <summary>
        /// Desplegar mensaje de Error con detalle
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="detalle"></param>
        /// 
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string msjDetalle) {
            SiteMaster masterMsj = (SiteMaster)Page.Master;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, msjDetalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }
        /// <summary>
        /// Botón activa realizar la búsqueda de Cotizacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e) {
            try {
                this.ListadoConfiguracionRegla = null;
                if (this.NombreEmpresa == null)
                    this.EmpresaId = null;
                if (this.NombreSucursal == null)
                    this.SucursalId = null;
                if (this.NombreAlmacen == null)
                    this.AlmacenId = null;
                if (this.NombreUsuario == null)
                    this.UsuarioId = null;
                presentador.ObtenerConfiguraciones();
            } catch (Exception ex) {
                MostrarMensaje("No existen registros que cumplan con la condición solicitada, favor de corregir.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion
    }
}