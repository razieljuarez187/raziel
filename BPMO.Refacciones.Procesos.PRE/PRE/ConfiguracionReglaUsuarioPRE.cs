using System;
using System.Collections.Generic;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Generales.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Enumeradores;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.BR;
using BPMO.Refacciones.Procesos.VIS;

namespace BPMO.Refacciones.Procesos.PRE {
    /// <summary>
    /// Presentador
    /// </summary>
    public class ConfiguracionReglaUsuarioPRE {
        #region Atributos
        private IBuscadorConfiguracionReglaVIS vistaConsulta;
        private IMantenimientoConfiguracionReglaVIS vistaMantto;
        private IDataContext dataContext = null;
        private ConfiguracionReglaUsuarioBR configuracionBr;
        
        public enum ECatalogoBuscador {
            Empresa = 1,
            Sucursal = 2,
            Almacen = 3,
            Usuario = 4,
        };
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor para consulta de Configuración
        /// </summary>
        /// <param name="vistaConsulta">Vista con la que trabajará el presentador</param>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        public ConfiguracionReglaUsuarioPRE(IBuscadorConfiguracionReglaVIS vistaConsulta, List<DatosConexionBO> listadoCnx) {
            this.vistaConsulta = vistaConsulta;
            AgregarProviderDataContext(listadoCnx);
        }        
        /// <summary>
        /// Constructor para mantenimiento de Configuración
        /// </summary>
        /// <param name="vistaMantenimiento">Vista con la que trabajará el presentador</param>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        public ConfiguracionReglaUsuarioPRE(IMantenimientoConfiguracionReglaVIS vistaMantenimiento, List<DatosConexionBO> listadoCnx) {
            this.vistaMantto = vistaMantenimiento;
            AgregarProviderDataContext(listadoCnx);
        }
        /// <summary>
        /// Constructor para el reporte de Configuraciones
        /// </summary>
        /// <param name="vistaReporte"></param>
        /// <param name="listadoCnx"></param>
        //public ConfiguracionReglaUsuarioPRE(IReporteConfiguracionReglaVIS vistaDocRPT, List<DatosConexionBO> listadoCnx) {
        //    this.vistaDocRPT = vistaDocRPT;
        //    AgregarProviderDataContext(listadoCnx);
        //}

        /// <summary>
        /// Agrega provider al DataContext actual
        /// </summary>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        private void AgregarProviderDataContext(List<DatosConexionBO> listadoCnx) {
            if (listadoCnx == null)
                throw new ArgumentNullException("Se requiere proveer los parámetros de conexión.");
            foreach (DatosConexionBO cnx in listadoCnx) {
                if (this.dataContext == null) {
                    this.dataContext = new DataContext(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
                    cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
                } else {
                    this.dataContext.AddProvider(new DataProviderFactoryBPMO().GetProvider(cnx.TipoProveedor,
                    cnx.BaseDatos, cnx.Usuario, cnx.Servidor, cnx.ServidorLigado), cnx.NombreProveedor);
                }
            }
        }
        #endregion

        #region Metodos
        #region Consulta
        /// <summary>
        /// Consultar Listado de Configuraciones, si la búsqueda retorna 1 registro se presenta el detalle del elemento para su edición
        /// </summary>
        public void ObtenerConfiguraciones() {
            try {
                List<ConfiguracionReglaUsuarioBO> configuracionesReglas = this.Consultar(this.InterfazADatosConsulta());
                if (configuracionesReglas.Count == 1) {
                    vistaConsulta.EnviarAEditarBO(configuracionesReglas[0].Id.Value);
                } else {
                    vistaConsulta.ListadoConfiguracionRegla = configuracionesReglas;
                    if (configuracionesReglas.Count == 0)
                        vistaConsulta.MostrarMensaje("No existen registros que cumplan con la condición solicitada, favor de corregir.", ETipoMensajeIU.INFORMACION);
                }
            } catch (Exception ex) {
                vistaConsulta.MostrarMensaje("No existen registros que cumplan con la condición solicitada, favor de corregir.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Aplicar Ordenamiento a los resultados de búsqueda
        /// </summary>
        /// <param name="campo">Campo sobre el que se aplicará el ordenamiento</param>
        /// <param name="orden">Orden que desea aplicar ASC o DESC</param>
        public void OrdenarListado(string campo, string orden) {
            string asc = "ASC";
            List<ConfiguracionReglaUsuarioBO> listaOrdenada = null;
            if (campo.ToUpper().Equals("ID"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.Id).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.Id).ToList();
            if (campo.ToUpper().Equals("EMPRESA"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.Empresa.Nombre).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.Empresa.NombreCorto).ToList();
            if (campo.ToUpper().Equals("SUCURSAL"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.Sucursal.Nombre).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.Sucursal.Nombre).ToList();
            if (campo.ToUpper().Equals("ALMACEN"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.Almacen.Nombre).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.Almacen.Nombre).ToList();
            if (campo.ToUpper().Equals("USUARIO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.Usuario.Nombre).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.Usuario.Nombre).ToList();
            if (campo.ToUpper().Equals("TIPO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.TipoRegla.ToString()).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.TipoRegla.ToString()).ToList();
            if (campo.ToUpper().Equals("VALOR"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.ValorInicial).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.ValorInicial).ToList();
            if (campo.ToUpper().Equals("FINAL"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.ValorFinal).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.ValorFinal).ToList();
            if (campo.ToUpper().Equals("ACTIVO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionRegla.OrderBy(order => order.Activo).ToList() : vistaConsulta.ListadoConfiguracionRegla.OrderByDescending(order => order.Activo).ToList();
            vistaConsulta.ListadoConfiguracionRegla = listaOrdenada;
        }
        /// <summary>
        /// Obtiene los datos de la interfaz de Consulta
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionReglaUsuarioBO InterfazADatosConsulta() {
            ConfiguracionReglaUsuarioBO configuracion = new ConfiguracionReglaUsuarioBO();
            configuracion.Id = vistaConsulta.Id;
            configuracion.Empresa = new EmpresaLiderBO() { Id = vistaConsulta.EmpresaId };
            configuracion.Sucursal = new SucursalLiderBO() { Id = vistaConsulta.SucursalId };
            configuracion.Almacen = new AlmacenBO(){ Id = vistaConsulta.AlmacenId };
            configuracion.Usuario = new UsuarioLiderBO() { Id = vistaConsulta.UsuarioId };
            configuracion.TipoRegla = vistaConsulta.TipoRegla;
            configuracion.Activo = vistaConsulta.Activo;
            return configuracion;
        }
        /// <summary>
        /// Consultar Configuraciones Reglas
        /// </summary>
        /// <param name="configuracionBO">Objeto que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista de objetos que contiene la información que coincide con la consulta</returns>
        private List<ConfiguracionReglaUsuarioBO> Consultar(ConfiguracionReglaUsuarioBO configuracionBO) {
            configuracionBr = new ConfiguracionReglaUsuarioBR();
            List<AuditoriaBaseBO> lstReglas = configuracionBr.ConsultarCompleto(dataContext, configuracionBO);
            return lstReglas.ConvertAll(item => (ConfiguracionReglaUsuarioBO)item);
        }
        #endregion

        #region Mantenimiento
        /// <summary>
        /// Insertar Configuración
        /// </summary>
        public void Insertar() {
            try {
                string camposRequeridos = this.ValidaRequeridos();
                if (!String.IsNullOrEmpty(camposRequeridos)) {
                    vistaMantto.MostrarMensaje(camposRequeridos, ETipoMensajeIU.INFORMACION);
                    return;
                }
                configuracionBr = new ConfiguracionReglaUsuarioBR();
                ConfiguracionReglaUsuarioBO configuracion = this.InterfazADatosMantto();
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vistaMantto.UsuarioSesion, this.vistaMantto.Adscripcion);
                configuracionBr.Insertar(dataContext, configuracion, seguridadBO);
                vistaMantto.Id = configuracion.Id = configuracionBr.UltimoIdGenerado;
                this.DesplegarDetalleConfiguracionRegla(configuracion.Id);
                vistaMantto.MostrarMensaje("Guardado Exitoso", ETipoMensajeIU.EXITO);
            } catch (Exception ex) {
                vistaMantto.MostrarMensaje("Guardado no exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Actualizar Configuración
        /// </summary>
        public void Actualizar() {
            try {
                string camposRequeridos = this.ValidaRequeridos();
                if (!String.IsNullOrEmpty(camposRequeridos)) {
                    vistaMantto.MostrarMensaje(camposRequeridos, ETipoMensajeIU.INFORMACION);
                    return;
                }
                configuracionBr = new ConfiguracionReglaUsuarioBR();
                ConfiguracionReglaUsuarioBO configuracion = this.InterfazADatosEditar();
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vistaMantto.UsuarioSesion, this.vistaMantto.Adscripcion);
                configuracionBr.Actualizar(dataContext, configuracion, seguridadBO);
                this.DesplegarDetalleConfiguracionRegla(vistaMantto.Id);
                vistaMantto.MostrarMensaje("Guardado Exitoso", ETipoMensajeIU.EXITO);
            } catch (Exception ex) {
                vistaMantto.MostrarMensaje("Guardado no exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Obtiene los datos de la Interfaz, para su edición
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionReglaUsuarioBO InterfazADatosEditar() {
            ConfiguracionReglaUsuarioBO configuracion = (ConfiguracionReglaUsuarioBO)vistaMantto.ConfiguracionReglaBase;
            configuracion.Empresa = new EmpresaLiderBO() { Id = vistaMantto.EmpresaId, Nombre = vistaMantto.NombreEmpresa };
            configuracion.Sucursal = new SucursalLiderBO() { Id = vistaMantto.SucursalId, Nombre = vistaMantto.NombreSucursal };
            configuracion.Almacen = new AlmacenBO() { Id = vistaMantto.AlmacenId, Nombre = vistaMantto.NombreAlmacen };
            configuracion.Usuario = new UsuarioLiderBO() { Id = vistaMantto.UsuarioId, Nombre = vistaMantto.NombreUsuario };
            configuracion.TipoRegla = vistaMantto.TipoRegla;
            configuracion.ValorInicial = vistaMantto.ValorInicial;
            configuracion.ValorFinal = vistaMantto.ValorFinal;
            configuracion.Activo = vistaMantto.Activo;
            configuracion.Auditoria.UUA = vistaMantto.UsuarioSesion.Id;

            return configuracion;
        }
        /// <summary>
        /// Obtiene el nombre del usuario que tiene relación con la auditoria del proceso actual
        /// </summary>
        /// <param name="usuarioId">identificador del usuario a buscar</param>
        /// <returns>Retorna una cadena con el nombre del usuario, si no se encontró el usuario retorna null</returns>
        private string ObtenerNombreUsuario(int usuarioId) {
            UsuarioBR usuarioBR = new UsuarioBR();
            string nombreUsuario = null;
            CatalogoBaseBO usuarioBO = usuarioBR.Consultar(this.dataContext, new UsuarioBO { Id = usuarioId }).FirstOrDefault();
            if (usuarioBO != null) {
                nombreUsuario = (usuarioBO as UsuarioBO).Usuario;
            }
            return nombreUsuario;
        }
        /// <summary>
        /// Cancela la edición
        /// </summary>
        public void CancelarEdicion() {
            this.DatosAInterfaz((ConfiguracionReglaUsuarioBO)this.vistaMantto.ConfiguracionReglaBase);
            vistaMantto.PreparaUIDetalle();
        }
        /// <summary>
        /// Prepara la Interfaz para Edición, una vez seleccionado un registro
        /// </summary>
        public void PreparaUIEdicion() {
            try {
                vistaMantto.PreparaUIActualizar();
            } catch (Exception ex) {
                vistaMantto.MostrarMensaje("Inconsistencias al presentar la información.", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Despliega el Detalle de la Configuración Ragla
        /// </summary>
        public void DesplegarDetalleConfiguracionRegla(int? configId) {
            this.DatosAInterfaz(ConsultarConfiguracion(new ConfiguracionReglaUsuarioBO { Id = configId }));
            vistaMantto.PreparaUIDetalle();
        }
        /// <summary>
        /// Asocia los datos a la Interfaz
        /// </summary>
        /// <param name="configuracion">Objeto con la información a desplegar </param>
        private void DatosAInterfaz(ConfiguracionReglaUsuarioBO configuracion) {
            vistaMantto.ConfiguracionReglaBase = configuracion;
            vistaMantto.Id = configuracion.Id;
            vistaMantto.EmpresaId = configuracion.Empresa.Id;
            vistaMantto.NombreEmpresa = configuracion.Empresa.Nombre;
            vistaMantto.SucursalId = configuracion.Sucursal.Id;
            vistaMantto.NombreSucursal = configuracion.Sucursal.Nombre;
            vistaMantto.AlmacenId = configuracion.Almacen.Id;
            vistaMantto.NombreAlmacen = configuracion.Almacen.Nombre;
            vistaMantto.UsuarioId = configuracion.Usuario.Id;
            vistaMantto.NombreUsuario = configuracion.Usuario.Nombre;
            vistaMantto.TipoRegla = configuracion.TipoRegla;
            vistaMantto.ValorInicial = configuracion.ValorInicial;
            vistaMantto.ValorFinal = configuracion.ValorFinal;
            vistaMantto.Activo = configuracion.Activo;

            #region Información de la bitácora
            string nombreUsuarioCreacion = this.ObtenerNombreUsuario(configuracion.Auditoria.UC.Value);
            this.vistaMantto.UsuarioCreacionBitacora = nombreUsuarioCreacion;
            this.vistaMantto.FechaCreacionBitacora = configuracion.Auditoria.FC;
            if (configuracion.Auditoria.UC != configuracion.Auditoria.UUA) {
                this.vistaMantto.UsuarioActualizacionBitacora = this.ObtenerNombreUsuario(configuracion.Auditoria.UUA.Value);
            } else {
                this.vistaMantto.UsuarioActualizacionBitacora = nombreUsuarioCreacion;
            }
            this.vistaMantto.FechaActualizacionBitacora = configuracion.Auditoria.FUA;
            #endregion
        }        
        /// <summary>
        /// Consulta una Configuración específica, de acuerdo a un objeto seleccionado
        /// </summary>
        /// <param name="configuracion">Objeto que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista de objetos que contiene la información que coincide con la consulta</returns>
        private ConfiguracionReglaUsuarioBO ConsultarConfiguracion(ConfiguracionReglaUsuarioBO configuracion) {
            if (configuracion.Id == null)
                throw new Exception("El ID de configuración es requerido para obtener un único registro");
            configuracionBr = new ConfiguracionReglaUsuarioBR();
            List<AuditoriaBaseBO> lstConfiguraciones = configuracionBr.ConsultarCompleto(dataContext, configuracion);
            return lstConfiguraciones.ConvertAll(item => (ConfiguracionReglaUsuarioBO)item)[0];
        }
        /// <summary>
        /// Validar campos requeridos
        /// </summary>
        /// <returns>En caso de error retorna una cadena con el nombre de los campos que son obligatorios</returns>
        private string ValidaRequeridos() {
            string sError = string.Empty;
            if (!vistaMantto.EmpresaId.HasValue)
                sError += " , EmpresaId";
            if (!vistaMantto.SucursalId.HasValue)
                sError += " , SucursalId";
            if (!vistaMantto.AlmacenId.HasValue)
                sError += " , AlmacenId";
            if (!vistaMantto.UsuarioId.HasValue)
                sError += " , UsuarioID";
            if (!vistaMantto.TipoRegla.HasValue)
                sError += " , TipoRegla";
            if (!vistaMantto.ValorInicial.HasValue && !vistaMantto.ValorFinal.HasValue)
                sError += " , ValorInicial ó Valor Final";
            if (vistaMantto.Activo == null)
                sError += " , Activo";
            if (vistaMantto.UsuarioSesion == null || !vistaMantto.UsuarioSesion.Id.HasValue)
                sError += " , Usuario Auditoría";            
            if (sError.Length > 0)
                sError = "Los siguientes campos son obligatorios: " + sError.Substring(2);
            if (vistaMantto.ValorInicial.HasValue && vistaMantto.ValorFinal.HasValue && vistaMantto.ValorInicial > vistaMantto.ValorFinal)
                sError += (sError.Length > 0 ? "\n" : "") + "El valor inicial no debe ser mayor al valor final";
            return sError;
        }
        /// <summary>
        /// Obtiene los datos de la interfaz Mantenimiento Sucursal, Taller, UnidadOperativa  y Empleado
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionReglaUsuarioBO InterfazADatosMantto() {
            ConfiguracionReglaUsuarioBO configuracion = new ConfiguracionReglaUsuarioBO();
            configuracion.Empresa = new EmpresaLiderBO() { Id = vistaMantto.EmpresaId, Nombre = vistaMantto.NombreEmpresa };
            configuracion.Sucursal = new SucursalLiderBO() { Id = vistaMantto.SucursalId, Nombre = vistaMantto.NombreSucursal };
            configuracion.Almacen = new AlmacenBO() { Id = vistaMantto.AlmacenId, Nombre = vistaMantto.NombreAlmacen };
            configuracion.Usuario = new UsuarioLiderBO() { Id = vistaMantto.UsuarioId, Nombre = vistaMantto.NombreUsuario };
            configuracion.TipoRegla = vistaMantto.TipoRegla;
            configuracion.ValorInicial = vistaMantto.ValorInicial;
            configuracion.ValorFinal = vistaMantto.ValorFinal;
            configuracion.Activo = vistaMantto.Activo;
            configuracion.Auditoria = new AuditoriaBO();
            configuracion.Auditoria.UC = configuracion.Auditoria.UUA = vistaMantto.UsuarioSesion.Id;
            return configuracion;
        }
        #endregion

        #region Buscador
        /// <summary>
        /// Obtiene el objeto que se enviara al buscador
        /// </summary>
        /// <param name="tipoBusqueda">Tipo de busqueda</param>
        /// <param name="objetoVista">Vista quien llama al método</param>
        /// <returns>Objeto que se enviara</returns>
        public object ObtenerObjetoBusqueda(ECatalogoBuscador tipoBusqueda, object objetoVista) {
            #region Validar VIS
            object objetoBuscador = null;
            IMantenimientoConfiguracionReglaVIS vMantto = null;
            IBuscadorConfiguracionReglaVIS vConsulta = null;
            bool esMantenimeinto = false;
            if (objetoVista is IMantenimientoConfiguracionReglaVIS) {
                vMantto = (IMantenimientoConfiguracionReglaVIS)objetoVista;
                esMantenimeinto = true;
            } else if (objetoVista is IBuscadorConfiguracionReglaVIS) {
                vConsulta = (IBuscadorConfiguracionReglaVIS)objetoVista;
                esMantenimeinto = false;
            }
            #endregion
            int id;
            bool esNumero;
            switch (tipoBusqueda) {
                case ECatalogoBuscador.Empresa:
                    #region Empresa
                    EmpresaLiderBO bo = new EmpresaLiderBO();
                    string nombreEmpresa = string.Empty;
                    if (esMantenimeinto) {
                        bo.Activo = true;
                        nombreEmpresa = vMantto.NombreEmpresa;
                    } else {
                        nombreEmpresa = vConsulta.NombreEmpresa;
                    }
                    esNumero = int.TryParse(nombreEmpresa, out id);
                    if (esNumero)
                        bo.Id = id;
                    else
                        bo.Nombre = nombreEmpresa;
                    objetoBuscador = bo;
                    break;
                    #endregion
                case ECatalogoBuscador.Sucursal:
                    #region Sucursal
                    SucursalLiderBO sucursal = new SucursalLiderBO();
                    string nombreSucursal = string.Empty;
                    int? _empresaId = null;
                    if (esMantenimeinto) {
                        sucursal.Activo = true;
                        nombreSucursal = vMantto.NombreSucursal;
                        if (vMantto.EmpresaId == null) {
                            vMantto.MostrarMensaje("Es necesario que primero seleccione una empresa.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        _empresaId = vistaMantto.EmpresaId;
                    } else {
                        nombreSucursal = vConsulta.NombreSucursal;
                        if (vConsulta.EmpresaId == null) {
                            vConsulta.MostrarMensaje("Es necesario que primero seleccione una empresa.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        _empresaId = vConsulta.EmpresaId;
                    }
                    sucursal.Empresa = new EmpresaLiderBO() { Id = _empresaId };
                    esNumero = int.TryParse(nombreSucursal, out id);
                    if (esNumero)
                        sucursal.Id = id;
                    else
                        sucursal.Nombre = nombreSucursal;
                    objetoBuscador = sucursal;
                    break;
                    #endregion
                case ECatalogoBuscador.Almacen:
                    #region Almacén
                    AlmacenBO almacen = new AlmacenBO();
                    string nombreTaller = string.Empty;
                    int? _sucursalId = null;
                    if (esMantenimeinto) {
                        almacen.Activo = true;
                        nombreTaller = vMantto.NombreAlmacen;
                        if (vMantto.SucursalId == null) {
                            vMantto.MostrarMensaje("Es necesario que primero seleccione un almacén.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        _sucursalId = vistaMantto.SucursalId;
                    } else {
                        nombreTaller = vConsulta.NombreAlmacen;
                        if (vConsulta.SucursalId == null) {
                            vConsulta.MostrarMensaje("Es necesario que primero seleccione un almacén.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        _sucursalId = vConsulta.SucursalId;
                    }
                    almacen.Sucursal = new SucursalLiderBO() { Id = _sucursalId };
                    esNumero = int.TryParse(nombreTaller, out id);
                    if (esNumero)
                        almacen.Id = id;
                    else
                        almacen.Nombre = nombreTaller;
                    objetoBuscador = almacen;
                    break;
                    #endregion
                case ECatalogoBuscador.Usuario:
                    #region Empleado
                    UsuarioLiderBO usuario = new UsuarioLiderBO();
                    string nombreUsuario = string.Empty;
                    if (esMantenimeinto) {
                        usuario.Activo = true;
                        nombreUsuario = vMantto.NombreUsuario;
                    } else {
                        nombreUsuario = vConsulta.NombreUsuario;
                    }
                    esNumero = int.TryParse(nombreUsuario, out id);
                    if (esNumero)
                        usuario.Id = id;
                    else
                        usuario.Nombre = nombreUsuario;
                    objetoBuscador = usuario;
                    break;
                    #endregion
            }
            return objetoBuscador;
        }
        /// <summary>
        /// Despliega la información del objeto seleccionado
        /// </summary>
        /// <param name="tipoBusqueda">Tipo de busuqeda que se efectuara</param>
        /// <param name="objeto">Objeto que se desplegará</param>
        /// <param name="objetoVista">Vusta quien llama al método</param>
        public void DesplegarBusqueda(ECatalogoBuscador tipoBusqueda, object objeto, object objetoVista) {
            bool esMantenimiento = false;
            if (objetoVista is IMantenimientoConfiguracionReglaVIS)
                esMantenimiento = true;
            else if (objetoVista is IBuscadorConfiguracionReglaVIS)
                esMantenimiento = false;
            if (objeto != null) {
                switch (tipoBusqueda) {
                    case ECatalogoBuscador.Empresa:
                        #region Empresa
                        EmpresaLiderBO empresa = (EmpresaLiderBO)objeto;
                        if (esMantenimiento) {
                            vistaMantto.EmpresaId = empresa.Id;
                            vistaMantto.NombreEmpresa = empresa.Nombre;                            
                            vistaMantto.SucursalId = null;
                            vistaMantto.NombreSucursal = null;
                            vistaMantto.AlmacenId = null;
                            vistaMantto.NombreAlmacen = null;
                        } else {
                            vistaConsulta.EmpresaId = empresa.Id;
                            vistaConsulta.NombreEmpresa = empresa.Nombre;
                            vistaConsulta.SucursalId = null;
                            vistaConsulta.NombreSucursal = null;
                            vistaConsulta.AlmacenId = null;
                            vistaConsulta.NombreAlmacen = null;
                        }
                        break;
                        #endregion
                    case ECatalogoBuscador.Sucursal:
                        #region Sucursal
                        SucursalLiderBO sucursal = (SucursalLiderBO)objeto;
                        if (esMantenimiento) {
                            vistaMantto.SucursalId = sucursal.Id;
                            vistaMantto.NombreSucursal = sucursal.Nombre;
                            vistaMantto.AlmacenId = null;
                            vistaMantto.NombreAlmacen = null;
                        } else {
                            vistaConsulta.SucursalId = sucursal.Id;
                            vistaConsulta.NombreSucursal = sucursal.Nombre;
                            vistaConsulta.AlmacenId = null;
                            vistaConsulta.NombreAlmacen = null;
                        }
                        break;
                        #endregion
                    case ECatalogoBuscador.Almacen:
                        #region Almacén
                        AlmacenBO almacen = (AlmacenBO)objeto;
                        if (esMantenimiento) {
                            vistaMantto.AlmacenId = almacen.Id;
                            vistaMantto.NombreAlmacen = almacen.Nombre;
                        } else {
                            vistaConsulta.AlmacenId = almacen.Id;
                            vistaConsulta.NombreAlmacen = almacen.Nombre;
                        }
                        break;
                        #endregion
                    case ECatalogoBuscador.Usuario:
                        #region Empleado
                        UsuarioLiderBO usuario = (UsuarioLiderBO)objeto;
                        if (esMantenimiento) {
                            vistaMantto.UsuarioId = usuario.Id;
                            vistaMantto.NombreUsuario = usuario.Nombre;
                        } else {
                            vistaConsulta.UsuarioId = usuario.Id;
                            vistaConsulta.NombreUsuario = usuario.Nombre;
                        }
                        break;
                        #endregion
                }
            }
        }
        #endregion
        #endregion Metodos
    }
}
