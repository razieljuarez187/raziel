using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPMO.Refacciones.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Basicos.BO;
using BPMO.Refacciones.Procesos.REPORTESPRE;
using BPMO.Refacciones.Procesos.REPORTESVIS;
using System.Data;
using DevExpress.XtraReports.UI;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.Master.UI;
using DevExpress.XtraReports.Web;
using System.IO;
using System.ComponentModel;

namespace BPMO.Refacciones.Reportes {
    public partial class ConfiguracionesReglasAsignadasUI : Page, IConfiguracionesAsignadasVIS {
        #region Atributos
        private ConfiguracionesAsignadasPRE presentador = null;
        #endregion
        #region Propiedades
        #region Variables de Session
        public UsuarioBO UsuarioSesion {
            get {
                return (this.Session["Usuario"] != null) ? (UsuarioBO)this.Session["Usuario"] : null;
            }
        }
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
        public ConfiguracionesAsignadasPRE.ECatalogoBuscador ViewState_Catalogo {
            get {
                return (ConfiguracionesAsignadasPRE.ECatalogoBuscador)ViewState["BUSQUEDA"];
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
        public decimal? ValorInicialA {
            get {
                decimal _valorIni;
                if (!decimal.TryParse(this.txtValorInicialA.Text, out _valorIni)) {
                    return null;
                }
                return _valorIni;
            }
        }
        public decimal? ValorInicialB {
            get {
                decimal _valorIniB;
                if (!decimal.TryParse(this.txtValorInicialB.Text, out _valorIniB)) {
                    return null;
                }
                return _valorIniB;
            }
        }
        public decimal? ValorFinalA {
            get {
                decimal _valorFin;
                if (!decimal.TryParse(this.txtValorFinalA.Text, out _valorFin)) {
                    return null;
                }
                return _valorFin;
            }
        }
        public decimal? ValorFinalB {
            get {
                decimal _valorFinB;
                if (!decimal.TryParse(this.txtValorFinalB.Text, out _valorFinB)) {
                    return null;
                }
                return _valorFinB;
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
        private XtraReport Session_ReporteRpt {
            get { return (this.Session["CONFIGURACIONESASIGNADASRPT"] != null) ? (XtraReport)this.Session["CONFIGURACIONESASIGNADASRPT"] : null; }
            set { this.Session["CONFIGURACIONESASIGNADASRPT"] = value; }
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
                this.AsignarImagenBoton();
                presentador = new ConfiguracionesAsignadasPRE(this, ListadoDatosConexion);
                if (!IsPostBack) {
                    this.Session_ReporteRpt = null;
                    this.hdnMostrarReporte.Value = "0";
                    #region Código de seguridad para las UI
                    //Verifica que el proceso actual exista en la lista de procesos permitidos para el usuario actual
                    SiteMaster miMaster = (SiteMaster)this.Master;
                    Security.BR.SecurityBR securityBR = new Security.BR.SecurityBR(new SeguridadBO(Guid.Empty, miMaster.Usuario, miMaster.Adscripcion));
                    miMaster.VerificarProcesoActual(securityBR.ProcesoActual);
                    #endregion
                    CargarTiposRegla();
                }
                this.rptViewer.Report = this.Session_ReporteRpt;
            } catch (Exception ex) {
                this.MostrarMensaje("Inconsistencias al presentar la informacion", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Asigna la imagen correspondiente al botón en base al estatus guardado en el campo oculto hdnEstadoDatosOrden
        /// </summary>
        private void AsignarImagenBoton() {
            if (this.hdnEstadoDatos.Value == "false") {
                this.btnSHConsultar.ImageUrl = "../Imagenes/Arriba.jpg";
            } else {
                this.btnSHConsultar.ImageUrl = "../Imagenes/Abajo.jpg";
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
        /// <summary>
        /// Realiza la consulta de las productividades
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Informacion del evento generado</param>
        protected void btnBuscar_Click(object sender, EventArgs e) {
            try {
                presentador.ConsultarProductividadUsuario();
            } catch (Exception ex) {
                this.MostrarMensaje("No existen registros que cumplan con la condición solicitada, favor de corregir", ETipoMensajeIU.ERROR, ex.Message);
            }
        }        
        /// <summary>
        /// Genera el reporte de la productividad del jefe de taller
        /// </summary>
        /// <param name="listaDatos">Dataset con la información del reporte a desplegar</param>
        public void DesplegarConfiguracionesAsignadas(DataSet listaDatos) {
            if (listaDatos != null && listaDatos.Tables.Count > 0 && listaDatos.Tables[0].Rows.Count > 0) {
                ConfiguracionesReglasAsignadasRpt reporte = new ConfiguracionesReglasAsignadasRpt();
                reporte.pUsuario.Value = this.UsuarioSesion.Usuario;
                reporte.DataSource = listaDatos;
                this.Session_ReporteRpt = reporte;
                this.hdnMostrarReporte.Value = "1";
            } else {
                this.Session_ReporteRpt = null;
                this.hdnMostrarReporte.Value = "0";
            }
            this.rptViewer.Report = this.Session_ReporteRpt;
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
                    this.EjecutaBuscador(xml, ConfiguracionesAsignadasPRE.ECatalogoBuscador.Empresa);
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
                    this.EjecutaBuscador(xml, ConfiguracionesAsignadasPRE.ECatalogoBuscador.Sucursal);
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
                    this.EjecutaBuscador(xml, ConfiguracionesAsignadasPRE.ECatalogoBuscador.Almacen);
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
                    this.EjecutaBuscador(xml, ConfiguracionesAsignadasPRE.ECatalogoBuscador.Usuario);
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
        private void EjecutaBuscador(string catalogo, ConfiguracionesAsignadasPRE.ECatalogoBuscador tipoBusqueda) {
            this.ViewState_Catalogo = tipoBusqueda;
            this.Session_ObjetoBuscador = presentador.ObtenerObjetoBusqueda(tipoBusqueda);
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
                presentador.DesplegarBusqueda(this.ViewState_Catalogo, this.Session_BOSelecto);
                this.Session_BOSelecto = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion

        #region Métodos para el manejo del reporte
        /// <summary>
        /// Almacena el reporte en la cache
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CacheReportDocumentEventArgs</param>
        protected void rptViewer_CacheReportDocument(object sender, CacheReportDocumentEventArgs e) {
            try {
                e.Key = Guid.NewGuid().ToString();
                Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar el reporte.", Primitivos.Enumeradores.ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Restablece el reporte obteniéndolo de la cache
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RestoreReportDocumentFromCacheEventArgs</param>
        protected void rptViewer_RestoreReportDocumentFromCache(object sender, RestoreReportDocumentFromCacheEventArgs e) {
            try {
                Stream stream = (Stream)Page.Session[e.Key];
                if (stream != null)
                    e.RestoreDocumentFromStream(stream);
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar el reporte.", Primitivos.Enumeradores.ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Se desencadena cuando se descarga la página
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        protected void rptViewer_Unload(object sender, EventArgs e) {
            try {
                ((ReportViewer)sender).Report = null;
            } catch (Exception ex) {
                MostrarMensaje("Inconsistencias al desplegar el reporte.", Primitivos.Enumeradores.ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion
        
        /// <summary>
        /// Configura la interfaz de usuario para búsqueda
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnRegresar_Click(object sender, EventArgs e) {
            this.hdnMostrarReporte.Value = "0";
            this.Session_ReporteRpt = null;
            this.rptViewer.Report = this.Session_ReporteRpt;
        }
        /// <summary>
        /// Desplegar mensaje segun el tipo de de mensaje
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="tipo">Tipo de mensaje a mostrar</param>
        /// <param name="detalle">detalle el mensaje mostrado</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipo, string detalle = null) {
            SiteMaster masterMsj = (SiteMaster)Page.Master;
            if (masterMsj == null) return;
            if (tipo == ETipoMensajeIU.ERROR)
                masterMsj.MostrarMensaje(mensaje, tipo, detalle);
            else
                masterMsj.MostrarMensaje(mensaje, tipo);
        }
        #endregion
    }
}