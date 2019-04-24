using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using BPMO.Basicos.BO;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BO;
using BPMO.Refacciones.Catalogos.PRE;
using BPMO.Refacciones.Catalogos.VIS;

namespace BPMO.Refacciones.Master.UI {
    /// <summary>
    /// Master page
    /// </summary>
    public partial class SiteMaster : System.Web.UI.MasterPage, IMDI {
        
        #region Atributos
        /// <summary>
        /// Presentador de MDI
        /// </summary>
        MDIPRE presentador = null;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene o establece la lista de conexiones a la base de datos
        /// </summary>
        public List<DatosConexionBO> ListadoDatosConexion {
            get {
                return this.Session["DatosConexion"] != null ? (List<DatosConexionBO>)this.Session["DatosConexion"] : null;
            }
            set {
                this.Session["DatosConexion"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el Usuario
        /// </summary>
        public UsuarioBO Usuario {
            get {
                return this.Session["Usuario"] != null ? (UsuarioBO)this.Session["Usuario"] : null;
            }
            set {
                this.Session["Usuario"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece la Adscripción del Servicio
        /// </summary>
        public AdscripcionBO Adscripcion {
            get {
                return this.Session["Adscripcion"] != null ? (AdscripcionBO)this.Session["Adscripcion"] : null;
            }
            set {
                this.Session["Adscripcion"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el listado de Procesos
        /// </summary>
        public List<ProcesoBO> ListadoProcesos {
            get {
                return this.Session["lstProcesos"] != null ? (List<ProcesoBO>)this.Session["lstProcesos"] : null;
            }
            set {
                this.Session["lstProcesos"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el ambiente de ejecución para nombre del estilo CSS
        /// </summary>
        public string Ambiente {
            get {
                return this.Session["EstiloCss"] != null ? this.Session["EstiloCss"].ToString() : null;
            }
            set {
                this.Session["EstiloCss"] = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el NumeroFilas de los Grids
        /// </summary>
        public int? NumeroFilas {
            get {
                return (this.Session["NUMERO_FILAS"] == null) ? null : (int?)this.Session["NUMERO_FILAS"];
            }
            set {
                if (value != null)
                    this.Session.Add("NUMERO_FILAS", value);
                else
                    this.Session.Remove("NUMERO_FILAS");
            }
        }
        /// <summary>
        /// Establece el valor si Muestra Menu
        /// </summary>
        public bool MostrarMenu {
            set {
                this.hdnMostrarMenu.Value = value.ToString();
            }
        }
        /// <summary>
        /// Obtiene el Logueo
        /// </summary>
        private string Logueo {
            get { return ConfigurationManager.AppSettings["Logueo"]; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método manejador del evento load de la master principal
        /// </summary>
        /// <param name="sender">Representa el objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Representa un objeto de tipo EventArgs</param>
        protected void Page_Load(object sender, EventArgs e) {
            try {
                this.VerificaSession();
                this.LeerYSubirNumeroFilas();
                if (this.Usuario != null && this.ListadoDatosConexion != null && this.ListadoDatosConexion.Count > 0) {
                    this.EvitarCache();
                    this.presentador = new MDIPRE(this);
                    if (!IsPostBack) {
                        hdnIniContFinSession.Value = (int.Parse(ConfigurationManager.AppSettings["timeoutIniContador"].ToString()) * 60).ToString();
                        if (bool.Parse(this.hdnMostrarMenu.Value)) {
                            if (this.ListadoProcesos == null) {
                                this.presentador.ObtenerProcesos();
                            } else {
                                this.CargarProcesos();
                            }
                        }
                    }
                } else {
                    this.Response.Redirect(this.Logueo);
                }
            } catch (Exception) {
                this.MostrarMensaje("Error inesperado", ETipoMensajeIU.ERROR, "Surgió un error inesperado, por favor contacte a su administrador de sistemas");
            }
        }
        /// <summary>
        /// Asigna el valor del menú seleccionado 
        /// </summary>
        private void AsignaMenuSeleccionado() {
            try {
                if (this.Request.QueryString["MenuSeleccionado"] != null) {
                    this.hdnMenuSeleccionado.Value = this.Request.QueryString["MenuSeleccionado"];
                    this.Session["MenuSeleccionado"] = this.hdnMenuSeleccionado.Value;
                    var posicion = this.Request.Url.ToString().IndexOf("MenuSeleccionado");
                    if (posicion != -1) {
                        this.Response.Redirect(this.Request.Url.ToString().Substring(0, posicion - 1), true);
                    }
                } else {
                    this.hdnMenuSeleccionado.Value = this.Session["MenuSeleccionado"] != null ? this.Session["MenuSeleccionado"].ToString() : string.Empty;
                }
            } catch {
                this.MostrarMensaje("Error al asignar el menú seleccionado", ETipoMensajeIU.INFORMACION);
            }
        }
        /// <summary>
        /// Método manejador del evento init de la página
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EvenArgs</param>
        protected void Page_Init(object sender, EventArgs e) {
            try {
                if (!IsPostBack) {
                    this.AsignaMenuSeleccionado();
                    //TODO: QUITAR ESTA LINEA DE CODIGO
                    //if (Request.UrlReferrer == null)
                    if (Request.QueryString["WDN"] != null)
                        this.InicializarConfiguracionPrueba();
                    else//ELIMINAR HASTA AQUÍ
                        this.InicializarConfiguracion();
                    //Registramos el primer post
                    Session["UltimoPost"] = DateTime.Now;
                }
            } catch (Exception) {
                this.MostrarMensaje("Inconsistencias al carga la página", ETipoMensajeIU.ERROR, "Surgió un error inesperado, por favor contacte a su administrador de sistemas");
            }
        }
        /// <summary>
        /// Método manejador del evento click del bóton lkCerrarSesión
        /// </summary>
        /// <param name="sender">Objeto sobre el cual se esta trabajando</param>
        /// <param name="e">Objeto de tipo EventArgs</param>
        protected void lkCerrarSesion_Click(object sender, EventArgs e) {
            FinalizarSession();
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">1: Error, 2: Advertencia, 3: Información</param>
        /// <param name="msjDetalle">Desplega el detalle del mensaje</param>
        public void MostrarMensaje(string mensaje, ETipoMensajeIU tipoMensaje, string msjDetalle = null) {
            string sError = string.Empty;
            if (tipoMensaje == ETipoMensajeIU.ERROR) {
                if (this.hdnMensaje == null)
                    sError += " , hdnDetalle";
                this.hdnDetalle.Value = msjDetalle;
            }
            if (hdnMensaje == null)
                sError += " , hdnMensaje";
            if (hdnTipoMensaje == null)
                sError += " , hdnTipoMensaje";
            if (sError.Length > 0)
                throw new Exception("No se pudo desplegar correctamente el error. No se encontró el control: " + sError.Substring(2) + " en la MasterPage.");

            this.hdnMensaje.Value = mensaje;
            this.hdnTipoMensaje.Value = ((int)tipoMensaje).ToString();
        }
        /// <summary>
        /// Carga la hoja de estilo de acuerdo al ambiente
        /// </summary>
        private void CargaHojaEstilo() {
            if (this.Ambiente == null) {
                this.ltEstilo.Text = "<link href='../CSS/EstiloProduccion.css' rel='Stylesheet' type='text/css'/>";
            } else {
                this.ltEstilo.Text = "<link rel='stylesheet' type='text/css' href='../CSS/" + this.Ambiente + ".css'/>";
            }
            if (this.Adscripcion != null) {
                this.lblAdscripcion.Text = this.Adscripcion.UnidadOperativa.Nombre + " | " + this.Adscripcion.Sucursal.Nombre;
                if (Adscripcion.UnidadOperativa != null && Adscripcion.UnidadOperativa.NombreCorto != null)
                    this.imgLogo.ImageUrl = "~/Imagenes/logos/" + Adscripcion.UnidadOperativa.NombreCorto.ToLower() + ".png";
                else
                    this.imgLogo.ImageUrl = "~/Imagenes/logos/bmo.png";
            } else
                this.imgLogo.ImageUrl = "~/Imagenes/logos/bmo.png";
        }
        /// <summary>
        /// Inicializa las variables de configuración del sistema
        /// </summary>
        private void InicializarConfiguracion() {
            try {
                if (this.ListadoDatosConexion == null) {
                    XDocument xmlDocumento = XDocument.Load(this.Request.PhysicalApplicationPath + "/Conexiones.xml");
                    string ambienteId = Request.Form["Ambiente"];
                    if (string.IsNullOrEmpty(ambienteId))
                        throw new Exception("El ambiente no es válido");
                    this.presentador = new MDIPRE(this);
                    this.presentador.ObtenerDatosDeConexion(xmlDocumento, ambienteId);
                }
                CargaHojaEstilo();
                if (this.Usuario == null) {
                    string id = this.Request.Form["UsuarioId"];
                    string nombreUsuario = this.Request.Form["NombreUsuario"];
                    if (string.IsNullOrEmpty(id)) {
                        throw new NullReferenceException("Usuario no válido");
                    } else {
                        UsuarioBO usuario = new UsuarioBO
                        {
                            Id = int.Parse(id),
                            Usuario = string.IsNullOrEmpty(nombreUsuario) ? "" : nombreUsuario,
                        };
                        this.Usuario = usuario;
                        this.lblNombre.Text = this.Usuario.Usuario;
                    }
                } else {
                    this.lblNombre.Text = this.Usuario.Usuario;
                }
                if (this.Adscripcion != null) {
                    this.lblAdscripcion.Text = this.Adscripcion.UnidadOperativa.Nombre + " | " + this.Adscripcion.Sucursal.Nombre;
                }
            } catch (Exception ex) {
                this.MostrarMensaje("No se pudo inicializar la configuración correctamente", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        //HACK: Método de prueba borrar al final: InicializarConfiguracionPrueba()
        public void InicializarConfiguracionPrueba() {
            this.Usuario = new UsuarioBO { Id = 9329, Usuario = "" };//6604, 7270, 
            if (this.ListadoDatosConexion == null) {
                XDocument xmlDocumento = XDocument.Load(this.Request.PhysicalApplicationPath + "/Conexiones.xml");
                this.presentador = new MDIPRE(this);
                this.presentador.ObtenerDatosDeConexion(xmlDocumento, "3");
            }
            this.lblNombre.Text = this.Usuario.Usuario;
            CargaHojaEstilo();
        }
        /// <summary>
        /// Carga los procesos a los que el usuario tiene acceso
        /// </summary>
        public void CargarProcesos() {
            try {
                var list = this.ListadoProcesos;
                var query = list.Where(p => p.UI == "#" || p.MenuPrincipal == true);
                foreach (var e in query) {
                    list = list.Where(p => p.ProcesoPadre != e.NombreCorto).ToList();
                }
                List<string> procesosRaiz = list.Where(p => p.ProcesoPadre != "#").Select(p => p.ProcesoPadre).Distinct().ToList();
                this.mnuPrincipal.Items.Clear();
                foreach (string proceso in procesosRaiz) {
                    if (!string.IsNullOrEmpty(proceso)) {
                        MenuItem elementoPadre = new MenuItem(proceso, proceso);
                        elementoPadre.ToolTip = "Menu " + proceso;
                        this.mnuPrincipal.Items.Add(elementoPadre);
                        this.AgregarSubMenusRecursivo(ref elementoPadre, this.ListadoProcesos);
                    }
                }
                this.MenuPredeterminado();
            } catch (Exception ex) {

                throw ex;
            }
        }
        /// <summary>
        /// Agrega los submenús y las elementos hijos de los menús padre
        /// </summary>
        /// <param name="mnuPadre">Elemento padre</param>
        /// <param name="listaProcesos">lista de procesos</param>
        private void AgregarSubMenusRecursivo(ref MenuItem mnuPadre, List<Security.BO.ProcesoBO> listaProcesos) {
            foreach (var proceso in listaProcesos) {
                if (proceso.ProcesoPadre == mnuPadre.Value && proceso.NombreCorto != mnuPadre.Value && proceso.MenuPrincipal == true) {
                    MenuItem elementoHijo = new MenuItem
                    {
                        Text = proceso.UI == "#" ? proceso.NombreCorto + "  »" : proceso.NombreCorto,
                        Value = proceso.UI,
                        NavigateUrl = proceso.UI != "#" ? proceso.Ruta : "",
                        ToolTip = proceso.Nombre,
                    };
                    mnuPadre.ChildItems.Add(elementoHijo);
                    this.AgregarSubMenusRecursivo(ref elementoHijo, listaProcesos);
                }
            }
        }
        /// <summary>
        /// Construye un ménu con opciones predeterminadas
        /// </summary>
        public void MenuPredeterminado() {
            MenuItem mnuOpciones = new MenuItem
            {
                Text = "Opciones",
                ToolTip = "Menú opciones"
            };
            MenuItem Inicio = new MenuItem
            {
                Text = "Inicio",
                Value = "Inicio",
                ToolTip = "Ir a la página de inicio",
                NavigateUrl = "~/Catalogos.UI/Default.aspx"
            };
            this.mnuPrincipal.Items.AddAt(0, Inicio);
            if (this.Adscripcion != null) {
                MenuItem cambiarAdscripcion = new MenuItem
                {
                    Text = "Cambiar Adscripción",
                    Value = "Adscripcion",
                    NavigateUrl = "~/Catalogos.UI/ConfiguracionInicio.aspx",
                    ToolTip = "Ir a la página de configuración"
                };
                mnuOpciones.ChildItems.Add(cambiarAdscripcion);
            }
            if (mnuOpciones.ChildItems.Count > 0) {
                this.mnuPrincipal.Items.Add(mnuOpciones);
            }
        }
        /// <summary>
        /// Comprueba la existencia del proceso solicitado con la lista de procesos a los que el usuario tiene acceso
        /// </summary>
        /// <param name="procesoActual">Nombre del proceso actual</param>
        public void VerificarProcesoActual(string procesoActual) {
            try {
                if (this.ListadoProcesos != null) {
                    ProcesoBO proceso = this.ListadoProcesos.FirstOrDefault(p => p.UI == procesoActual);
                    if (proceso == null) {
                        this.Response.Redirect("~/Catalogos.UI/default.aspx");
                    }
                } else {
                    this.Response.Redirect("~/Catalogos.UI/ConfiguracionInicio.aspx");
                }
            } catch (Exception) {
                this.MostrarMensaje("El proceso seleccionado no es válido", ETipoMensajeIU.ERROR,
                    "Ha surgido un error no controlado, si el problema persiste contacte al administrador del sistema");
            }
        }
        /// <summary>
        /// Este método permite evitar el almacenamiento en cache de la página
        /// </summary>
        private void EvitarCache() {
            this.Response.AddHeader("Cache-Control", "no-cache");
            this.Response.AddHeader("Pragma", "no-cache");
            this.Response.Expires = 0;
            this.Response.Cache.SetNoStore();
        }
        /// <summary>
        /// Verifica si el último Post registrado en la página ha superado el timeout de la session
        /// si así fue invoca a finalizar la sesión del usuario
        /// </summary>
        protected void VerificaSession() {
            if (Session["UltimoPost"] == null) {
                FinalizarSession();
            } else {
                DateTime ultimoPost = ((DateTime)Session["UltimoPost"]).AddMinutes(Session.Timeout);
                if (DateTime.Compare(ultimoPost, DateTime.Now) < 0) {
                    FinalizarSession();
                } else {
                    Session["UltimoPost"] = DateTime.Now;
                }
            }
            VerificarCambioAdscripcion();
        }
        /// <summary>
        /// Finaliza la sesion del usuario
        /// </summary>
        private void FinalizarSession() {
            this.Session.RemoveAll();
            this.Response.Redirect(this.Logueo);
        }
        /// <summary>
        /// Verifica si la adscripción servicio ha sido cambiada
        /// </summary>
        protected void VerificarCambioAdscripcion() {
            hdnContadorSession.Value = (Session.Timeout * 60).ToString();
            if (string.IsNullOrEmpty(hdnSessionKey.Value)) {
                if (Adscripcion != null)
                    hdnSessionKey.Value = Adscripcion.GetHashCode().ToString();
            } else {
                if (hdnSessionKey.Value != Adscripcion.GetHashCode().ToString()) {
                    Response.Redirect("~/Catalogos.UI/default.aspx?pkt=1", true);
                }
            }
        }
        /// <summary>
        /// Lee el número de filas del web.config y lo sube a sesión
        /// </summary>
        private void LeerYSubirNumeroFilas() {
            string numberRows = ConfigurationManager.AppSettings["NumberRows"];
            int rows;
            if (!int.TryParse(numberRows, out rows)) rows = 10;
            this.NumeroFilas = rows;
        }
        #endregion
    }
}
