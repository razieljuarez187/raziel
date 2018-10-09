using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.Security.BO;
using BPMO.Security.BR;
using BPMO.Refacciones.Catalogos.VIS;

namespace BPMO.Refacciones.Catalogos.PRE {
    /// <summary>
    /// Presentador para el manejo de MDI
    /// </summary>
    public class MDIPRE {
        
        #region Atributos
        /// <summary>
        /// Vista de MDI
        /// </summary>
        private IMDI vista;
        /// <summary>
        /// Contexto que contiene la información para acceso a base de datos
        /// </summary>
        private IDataContext dataContext = null;       
        #endregion

        #region Constructores
        /// <summary>
        /// Crea una instancia de la presentadora de la MDI principal
        /// </summary>
        /// <param name="vistaActual">vistaActual</param>
        public MDIPRE(IMDI vistaActual) {
            this.vista = vistaActual;
            if (this.vista.ListadoDatosConexion != null) {
                dataContext = CargarProviderDataContext(vista.ListadoDatosConexion);
            }
        }
        #endregion

        #region Métodos 
        /// <summary>
        /// Agrega provider al DataContext actual y lo retorna
        /// </summary>
        /// <param name="listadoCnx">List de DatosConexionBO que provee los parámetros de los provider a agregar al DataContext</param>
        /// <returns>IDataContext</returns>
        public static IDataContext CargarProviderDataContext(List<DatosConexionBO> listadoCnx) {
            IDataContext dataContext = null;
            if (listadoCnx == null)
                throw new ArgumentNullException("Se requiere proveer los parámetros de conexión.");
            foreach (DatosConexionBO cnx in listadoCnx) {
                if (dataContext == null) {
                    dataContext = new DataContext(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
                    cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
                } else {
                    dataContext.AddProvider(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
                    cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
                }
            }
            return dataContext;
        }
        /// <summary>
        /// Este método permite obtener los datos de conexión según el ambiente seleccionado por el usuario
        /// </summary>
        /// <param name="xmlAmbientes">Documento que contiene los ambientes</param>
        /// <param name="ambienteId">Número del ambiente a cargar</param>
        /// <returns>Retorna un valor verdadero si la operación se realizó con éxito</returns>
        public void ObtenerDatosDeConexion(XDocument xmlAmbientes, string ambienteId) {
            try {
                XElement Ambiente = xmlAmbientes.Root.Elements("Ambiente").FirstOrDefault(a => a.Attribute("id").Value == ambienteId);
                if (Ambiente != null) {
                    this.vista.Ambiente = Ambiente.Attribute("Estilo").Value;
                    this.vista.ListadoDatosConexion = new List<DatosConexionBO>();
                    foreach (var conexion in Ambiente.Elements("Conexion")) {
                        byte valorProveedor;
                        if (!string.IsNullOrEmpty(conexion.Attribute("TipoProveedor").Value) && byte.TryParse(conexion.Attribute("TipoProveedor").Value, out valorProveedor)) {
                            DatosConexionBO dato = this.vista.ListadoDatosConexion.FirstOrDefault(c => c.NombreProveedor == conexion.Attribute("Nombre").Value);
                            if (dato == null) {
                                DatosConexionBO datosConexion = new DatosConexionBO(conexion.Attribute("BaseDatos").Value, conexion.Attribute("UsuarioDB").Value,
                                    conexion.Attribute("Servidor").Value, conexion.Attribute("LinkedServer").Value, conexion.Attribute("Nombre").Value,
                                    (DataProviderTypeBPMO)valorProveedor);
                                this.vista.ListadoDatosConexion.Add(datosConexion);
                            }
                        }
                    }
                } else {
                    this.vista.MostrarMensaje("Ambiente no válido", ETipoMensajeIU.ERROR, 
                        "No se encontró el ambiente seleccionado, por favor contacte al administrador del sistema");
                }
            } catch (Exception) {
                this.vista.MostrarMensaje("No se pudieron obtener los datos de conexión", ETipoMensajeIU.ERROR,
                    "No se encontraron los parametros de conexión en la fuente de datos, póngase en contacto con el administrador del sistema");
            }
        }
        /// <summary>
        /// Este método permite obtener los procesos a los que el usuario tiene permiso
        /// </summary>
        public void ObtenerProcesos() {
            try {
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vista.Usuario, this.vista.Adscripcion);
                SecurityBR securityBR = new SecurityBR(seguridadBO);
                Security.BO.ProcesoBO procesoBO = new Security.BO.ProcesoBO();
                securityBR.InsertarAccesoBitacora(this.dataContext);
                List<CatalogoBaseBO> listaPermisos = securityBR.ProcesoConsultar(this.dataContext, procesoBO);
                if (listaPermisos.Count > 0) {
                    this.vista.ListadoProcesos = listaPermisos.ConvertAll(p => (ProcesoBO)p);
                    this.vista.CargarProcesos();
                } else {
                    this.vista.MenuPredeterminado();
                    this.vista.MostrarMensaje("Lo sentimos, usted no tiene permiso para ejecutar ninguna operación.", ETipoMensajeIU.ERROR,
                        "Usted no cuenta con persmisos. Para mas información póngase en contacto con el administrador del sistema");
                }
            } catch (Exception ex) {
                this.vista.MostrarMensaje("La base de datos produjo una Excepción al intentar accederla. " +
                    "Intente de nuevo y si el problema persiste póngase en contacto con el administrador del sistema", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        #endregion
    }
}
