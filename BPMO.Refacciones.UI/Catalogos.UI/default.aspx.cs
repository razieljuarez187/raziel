using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Refacciones.Master.UI;
using BPMO.Utilerias.BO;
using BPMO.Utilerias.PRE;
using BPMO.Utilerias.VIS;


namespace BPMO.Refacciones.Catalogos.UI {
    /// <summary>
    /// Interfaz de la página predeterminada
    /// </summary>
    public partial class Default : System.Web.UI.Page, IConsultaNotificacion {

        #region Atributos
        /// <summary>
        /// Presentador de ConsultaNotificacion
        /// </summary>
        ConsultaNotificacionPRE presentador = null;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene los datos de conexión
        /// </summary>
        public DatosConexionBO DatosConexion {
            get {
                DatosConexionBO cnx = null;
                if (this.Session["DatosConexion"] != null)
                    cnx = ((List<DatosConexionBO>)this.Session["DatosConexion"]).Find(x => x.NombreProveedor == "LIDER");
                return cnx;
            }
        }
        /// <summary>
        /// Establece la lista de Notificaciones
        /// </summary>
        public List<NotificacionBO> lstNotificaciones {
            set { this.Session["Notificaciones"] = value; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carga la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (this.Session["Adscripcion"] == null) {
                this.Response.Redirect("~/Catalogos.UI/ConfiguracionInicio.aspx");
            }
            string msjCambioAdscripcion = Request.QueryString["pkt"];
            if (msjCambioAdscripcion != null)
                MostrarMensaje("Se le ha redirigido a la página de inicio debido a que ha cambiado su selección de Unidad Operativa-Sucursal-Taller para esta sesión.", Primitivos.Enumeradores.ETipoMensajeIU.INFORMACION);
            presentador = new ConsultaNotificacionPRE(this);
            if (!IsPostBack) {
                presentador.ObtenerNotificaciones(true);
            }
        }
        /// <summary>
        /// Despliega un mensaje en pantalla
        /// </summary>
        /// <param name="mensaje">Mensaje a desplegar</param>
        /// <param name="tipoMensaje">Tipo de mensaje a mostrar</param>
        /// <param name="msjDetalle">Detalles del mensaje</param>
        public void MostrarMensaje(string mensaje, Primitivos.Enumeradores.ETipoMensajeIU tipoMensaje, string msjDetalle = null) {
            SiteMaster miMaster = (SiteMaster)this.Master;
            if (!string.IsNullOrEmpty(msjDetalle)) {
                miMaster.MostrarMensaje(mensaje, tipoMensaje, msjDetalle);
            } else {
                miMaster.MostrarMensaje(mensaje, tipoMensaje);
            }

        }
        /// <summary>
        /// Lista las notificaciones para el sistema actual
        /// </summary>
        public void ListarNotificaciones() {
            List<NotificacionBO> notificaciones = this.Session["Notificaciones"] != null ? (List<NotificacionBO>)this.Session["Notificaciones"] : null;
            if (notificaciones != null && notificaciones.Count > 0) {
                notificaciones.OrderBy(n => n.FechaFin);
                this.rptNotificaciones.DataSource = notificaciones;
                this.rptNotificaciones.DataBind();
            }
        }
        #endregion

    }
}