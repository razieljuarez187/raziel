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
using System.Data;

namespace BPMO.Refacciones.Procesos.PRE {
    public class ConfiguracionTransferenciaPRE {
        #region Atributos
        private IBuscadorConfiguracionTransferenciasVIS vistaConsulta;
        private IMantenimientoConfiguracionTransferenciaVIS vistaMantto;
        private IDataContext dataContext = null;
        private ConfiguracionTransferenciaBR configuracionBr;
        ConfiguracionTransferenciaBO configuracion;
        public enum ECatalogoBuscador {
            Empresa = 1,
            Sucursal = 2,
            Almacen = 3,
            TipoPedido = 4,
        };
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor para consulta de Configuración
        /// </summary>
        /// <param name="vistaConsulta">Vista con la que trabajará el presentador</param>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        public ConfiguracionTransferenciaPRE(IBuscadorConfiguracionTransferenciasVIS vistaConsulta, List<DatosConexionBO> listadoCnx) {
            this.vistaConsulta = vistaConsulta;
            AgregarProviderDataContext(listadoCnx);
        }        
        /// <summary>
        /// Constructor para mantenimiento de Configuración
        /// </summary>
        /// <param name="vistaMantenimiento">Vista con la que trabajará el presentador</param>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        public ConfiguracionTransferenciaPRE(IMantenimientoConfiguracionTransferenciaVIS vistaMantenimiento, List<DatosConexionBO> listadoCnx) {
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
                List<ConfiguracionTransferenciaBO> configuraciones = this.Consultar(this.InterfazADatosConsulta());
                if (configuraciones.Count == 1) {
                    vistaConsulta.EnviarAEditarBO(configuraciones[0].Id.Value);
                } else {
                    vistaConsulta.ListadoConfiguracionTransferencia = configuraciones;
                    if (configuraciones.Count == 0)
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
            List<ConfiguracionTransferenciaBO> listaOrdenada = null;
            if (campo.ToUpper().Equals("ID"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.Id).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.Id).ToList();
            if (campo.ToUpper().Equals("EMPRESA"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.Empresa.Nombre).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.Empresa.NombreCorto).ToList();
            if (campo.ToUpper().Equals("SUCURSAL"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.Sucursal.Nombre).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.Sucursal.Nombre).ToList();
            if (campo.ToUpper().Equals("ALMACEN"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.Almacen.Nombre).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.Almacen.Nombre).ToList();
            if (campo.ToUpper().Equals("TIPOPEDIDO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.TipoPedido.Id).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.TipoPedido.Nombre).ToList();
            if (campo.ToUpper().Equals("MaximoArticulosLinea"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.MaximoArticulosLinea.ToString()).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.MaximoArticulosLinea.ToString()).ToList();
            if (campo.ToUpper().Equals("MaximoLineas"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.MaximoLineas).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.MaximoLineas).ToList();
            if (campo.ToUpper().Equals("NIVELESABC"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.NivelesABC).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.NivelesABC).ToList();
            if (campo.ToUpper().Equals("ACTIVO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoConfiguracionTransferencia.OrderBy(order => order.Activo).ToList() : vistaConsulta.ListadoConfiguracionTransferencia.OrderByDescending(order => order.Activo).ToList();
            vistaConsulta.ListadoConfiguracionTransferencia = listaOrdenada;
        }
        /// <summary>
        /// Obtiene los datos de la interfaz de Consulta
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionTransferenciaBO InterfazADatosConsulta() {
            ConfiguracionTransferenciaBO configuracion = new ConfiguracionTransferenciaBO();
            configuracion.Id = vistaConsulta.Id;
            configuracion.Empresa = new EmpresaLiderBO() { Id = vistaConsulta.EmpresaId };
            configuracion.Sucursal = new SucursalLiderBO() { Id = vistaConsulta.SucursalId };
            configuracion.Almacen = new AlmacenBO(){ Id = vistaConsulta.AlmacenId };
            configuracion.TipoPedido = new TipoPedidoBO() { Id = vistaConsulta.TipoPedidoId };
            configuracion.Activo = vistaConsulta.Activo;
            return configuracion;
        }
        /// <summary>
        /// Consultar Configuraciones Reglas
        /// </summary>
        /// <param name="configuracionBO">Objeto que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista de objetos que contiene la información que coincide con la consulta</returns>
        private List<ConfiguracionTransferenciaBO> Consultar(ConfiguracionTransferenciaBO configuracionBO) {
            configuracionBr = new ConfiguracionTransferenciaBR();
            List<AuditoriaBaseBO> lstReglas = configuracionBr.Consultar(dataContext, configuracionBO);
            return lstReglas.ConvertAll(item => (ConfiguracionTransferenciaBO)item);
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
                configuracionBr = new ConfiguracionTransferenciaBR();
                configuracion = this.InterfazADatosMantto();
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vistaMantto.UsuarioSesion, this.vistaMantto.Adscripcion);
                configuracionBr.InsertarCompleto(dataContext, configuracion, seguridadBO, vistaMantto.ConfNivelABCBO);
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
                configuracionBr = new ConfiguracionTransferenciaBR();
                
                ConfiguracionTransferenciaBO configuracion = this.InterfazADatosEditar();
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vistaMantto.UsuarioSesion, this.vistaMantto.Adscripcion);
                configuracionBr.ActualizarCompleto(dataContext, configuracion, seguridadBO, vistaMantto.ConfNivelABCBO);
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
        private ConfiguracionTransferenciaBO InterfazADatosEditar() {
            ConfiguracionTransferenciaBO configuracion = (ConfiguracionTransferenciaBO)vistaMantto.ConfiguracionReglaBase;
            configuracion.Empresa = new EmpresaLiderBO() { Id = vistaMantto.EmpresaId, Nombre = vistaMantto.NombreEmpresa };
            configuracion.Sucursal = new SucursalLiderBO() { Id = vistaMantto.SucursalId, Nombre = vistaMantto.NombreSucursal };
            configuracion.Almacen = new AlmacenBO() { Id = vistaMantto.AlmacenId, Nombre = vistaMantto.NombreAlmacen };
            configuracion.TipoPedido = new TipoPedidoBO() { Id = vistaMantto.TipoPedidoBO.Id, Nombre = vistaMantto.TipoPedidoBO.Nombre };
            configuracion.MaximoArticulosLinea = vistaMantto.maximoArticulosLinea;
            configuracion.MaximoLineas = vistaMantto.maximoLineas;
            configuracion.Activo = vistaMantto.Activo;
            configuracion.Auditoria.UUA = vistaMantto.UsuarioSesion.Id;

            configuracion.ConfiguracionCantidadTransferencia.Lunes = vistaMantto.confCantidadLunes;
            configuracion.ConfiguracionCantidadTransferencia.Martes = vistaMantto.confCantidadMartes;
            configuracion.ConfiguracionCantidadTransferencia.Miercoles = vistaMantto.confCantidadMiercoles;
            configuracion.ConfiguracionCantidadTransferencia.Jueves = vistaMantto.confCantidadJueves;
            configuracion.ConfiguracionCantidadTransferencia.Viernes = vistaMantto.confCantidadViernes;
            configuracion.ConfiguracionCantidadTransferencia.Sabado = vistaMantto.confCantidadSabado;
            configuracion.ConfiguracionCantidadTransferencia.Domingo = vistaMantto.confCantidadDomingo;
            configuracion.ConfiguracionCantidadTransferencia.Activo = vistaMantto.confCantidadActivo;

            configuracion.ConfiguracionHoraTransferencia.Lunes = vistaMantto.confHoraLunes;
            configuracion.ConfiguracionHoraTransferencia.Martes = vistaMantto.confHoraMartes;
            configuracion.ConfiguracionHoraTransferencia.Miercoles = vistaMantto.confHoraMiercoles;
            configuracion.ConfiguracionHoraTransferencia.Jueves = vistaMantto.confHoraJueves;
            configuracion.ConfiguracionHoraTransferencia.Viernes = vistaMantto.confHoraViernes;
            configuracion.ConfiguracionHoraTransferencia.Sabado = vistaMantto.confHoraSabado;
            configuracion.ConfiguracionHoraTransferencia.Domingo = vistaMantto.confHoraDomingo;
            configuracion.ConfiguracionHoraTransferencia.Activo = vistaMantto.confHoraActivo;

            configuracion.NivelesABC = vistaMantto.NivelABCBO;

            return configuracion;
        }
        /// <summary>
        /// Obtiene el nombre del usuario que tiene relación con la auditoria del proceso actual
        /// </summary>
        /// <param name="usuarioId">identificador del usuario a buscar</param>
        /// <returns>Retorna una cadena con el nombre del usuario, si no se encontró el usuario retorna null</returns>
        private string ObtenerNombreTipoPedido(int tipoPedidoId) {
            TipoPedidoBR transferenciaBR = new TipoPedidoBR();
            string nombreTipoPedido = null;
            CatalogoBaseBO tipoPedidoBO = transferenciaBR.Consultar(this.dataContext, new TipoPedidoBO { Id = tipoPedidoId }).FirstOrDefault();
            if (tipoPedidoBO != null) {
                nombreTipoPedido = (tipoPedidoBO as TipoPedidoBO).Nombre;
            }
            return nombreTipoPedido;
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
            this.DatosAInterfaz((ConfiguracionTransferenciaBO)this.vistaMantto.ConfiguracionReglaBase);
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
            this.DatosAInterfaz(ConsultarConfiguracion(new ConfiguracionTransferenciaBO { Id = configId }));
            List<NivelABCBO> confLstNivelABC;
            confLstNivelABC = configuracionBr.ConsultarDataSet(dataContext, configId);
            List<NivelABCBO> NoExisten = (from p in vistaMantto.NivelABCBO
                                          where !(from ex in confLstNivelABC
                                 select ex.Id)
                                 .Contains(p.Id)
                         select p).ToList();
            vistaMantto.ConfNivelABCBO = confLstNivelABC;
            vistaMantto.NivelABCBO = NoExisten;
            vistaMantto.PreparaUIDetalle();
        }
        public void DesplearcatalogoNivelABC() {
            List<CatalogoBaseBO> confLstNivelABC;
            NivelABCBR nivelABCBR =new NivelABCBR();
            NivelABCBO nivelABCBO = new NivelABCBO();
            confLstNivelABC = nivelABCBR.Consultar(dataContext, nivelABCBO);
            vistaMantto.NivelABCBO = confLstNivelABC.ConvertAll(x => (NivelABCBO)x);
        }
        /// <summary>
        /// Asocia los datos a la Interfaz
        /// </summary>
        /// <param name="configuracion">Objeto con la información a desplegar </param>
        private void DatosAInterfaz(ConfiguracionTransferenciaBO configuracion) {
            vistaMantto.ConfiguracionReglaBase = configuracion;
            vistaMantto.Id = configuracion.Id;
            vistaMantto.EmpresaId = configuracion.Empresa.Id;
            vistaMantto.NombreEmpresa = configuracion.Empresa.Nombre;
            vistaMantto.SucursalId = configuracion.Sucursal.Id;
            vistaMantto.NombreSucursal = configuracion.Sucursal.Nombre;
            vistaMantto.AlmacenId = configuracion.Almacen.Id;
            vistaMantto.NombreAlmacen = configuracion.Almacen.Nombre;
            vistaMantto.TipoPedidoId = configuracion.TipoPedido.Id;
            vistaMantto.NombreTipoPedido = configuracion.TipoPedido.Nombre;
            vistaMantto.maximoArticulosLinea = configuracion.MaximoArticulosLinea;
            vistaMantto.maximoLineas = configuracion.MaximoLineas;
            vistaMantto.Activo = configuracion.Activo;

            vistaMantto.confCantidadLunes = configuracion.ConfiguracionCantidadTransferencia.Lunes;
            vistaMantto.confCantidadMartes = configuracion.ConfiguracionCantidadTransferencia.Martes;
            vistaMantto.confCantidadMiercoles = configuracion.ConfiguracionCantidadTransferencia.Miercoles;
            vistaMantto.confCantidadJueves = configuracion.ConfiguracionCantidadTransferencia.Jueves;
            vistaMantto.confCantidadViernes = configuracion.ConfiguracionCantidadTransferencia.Viernes;
            vistaMantto.confCantidadSabado = configuracion.ConfiguracionCantidadTransferencia.Sabado;
            vistaMantto.confCantidadDomingo = configuracion.ConfiguracionCantidadTransferencia.Domingo;
            vistaMantto.confCantidadActivo = configuracion.ConfiguracionCantidadTransferencia.Activo;

            vistaMantto.confHoraLunes = configuracion.ConfiguracionHoraTransferencia.Lunes;
            vistaMantto.confHoraMartes = configuracion.ConfiguracionHoraTransferencia.Martes;
            vistaMantto.confHoraMiercoles = configuracion.ConfiguracionHoraTransferencia.Miercoles;
            vistaMantto.confHoraJueves = configuracion.ConfiguracionHoraTransferencia.Jueves;
            vistaMantto.confHoraViernes = configuracion.ConfiguracionHoraTransferencia.Viernes;
            vistaMantto.confHoraSabado = configuracion.ConfiguracionHoraTransferencia.Sabado;
            vistaMantto.confHoraDomingo = configuracion.ConfiguracionHoraTransferencia.Domingo;
            vistaMantto.confHoraActivo = configuracion.ConfiguracionHoraTransferencia.Activo;

            vistaMantto.NivelABCBO = configuracion.NivelesABC;

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
        private ConfiguracionTransferenciaBO ConsultarConfiguracion(ConfiguracionTransferenciaBO configuracion) {
            if (configuracion.Id == null)
                throw new Exception("El ID de configuración es requerido para obtener un único registro");
            configuracionBr = new ConfiguracionTransferenciaBR();
            ConfiguracionTransferenciaBO confTransferenciaBO = (ConfiguracionTransferenciaBO)configuracionBr.ConsultarCompleto(dataContext, configuracion)[0];
            return confTransferenciaBO;
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
            if (!vistaMantto.TipoPedidoId.HasValue)
                sError += " , TipoPedidoID";
            if (vistaMantto.Activo == null)
                sError += " , Activo";
            if (vistaMantto.UsuarioSesion == null || !vistaMantto.UsuarioSesion.Id.HasValue)
                sError += " , Usuario Auditoría";            
            if (sError.Length > 0)
                sError = "Los siguientes campos son obligatorios: " + sError.Substring(2);
            return sError;
        }
        /// <summary>
        /// Obtiene los datos de la interfaz Mantenimiento Sucursal, Taller, UnidadOperativa  y Empleado
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionTransferenciaBO InterfazADatosMantto() {
            ConfiguracionTransferenciaBO configuracion = new ConfiguracionTransferenciaBO();
            configuracion.Empresa = new EmpresaLiderBO() { Id = vistaMantto.EmpresaId, Nombre = vistaMantto.NombreEmpresa };
            configuracion.Sucursal = new SucursalLiderBO() { Id = vistaMantto.SucursalId, Nombre = vistaMantto.NombreSucursal };
            configuracion.Almacen = new AlmacenBO() { Id = vistaMantto.AlmacenId, Nombre = vistaMantto.NombreAlmacen };
            configuracion.TipoPedido = new TipoPedidoBO() { Id = vistaMantto.TipoPedidoId, Nombre = vistaMantto.NombreTipoPedido };
            configuracion.ConfiguracionCantidadTransferencia = new ConfiguracionCantidadTransferenciaBO() {};
            configuracion.ConfiguracionHoraTransferencia = new ConfiguracionHoraTransferenciaBO() { };
            configuracion.MaximoArticulosLinea = vistaMantto.maximoArticulosLinea;
            configuracion.MaximoLineas = vistaMantto.maximoLineas;
            configuracion.Activo = vistaMantto.Activo;
            configuracion.Auditoria = new AuditoriaBO();
            configuracion.Auditoria.UC = configuracion.Auditoria.UUA = vistaMantto.UsuarioSesion.Id;

            configuracion.ConfiguracionCantidadTransferencia.Lunes = vistaMantto.confCantidadLunes;
            configuracion.ConfiguracionCantidadTransferencia.Martes = vistaMantto.confCantidadMartes;
            configuracion.ConfiguracionCantidadTransferencia.Miercoles = vistaMantto.confCantidadMiercoles;
            configuracion.ConfiguracionCantidadTransferencia.Jueves = vistaMantto.confCantidadJueves;
            configuracion.ConfiguracionCantidadTransferencia.Viernes = vistaMantto.confCantidadViernes;
            configuracion.ConfiguracionCantidadTransferencia.Sabado = vistaMantto.confCantidadSabado;
            configuracion.ConfiguracionCantidadTransferencia.Domingo = vistaMantto.confCantidadDomingo;
            configuracion.ConfiguracionCantidadTransferencia.Activo = vistaMantto.confCantidadActivo;

            configuracion.ConfiguracionHoraTransferencia.Lunes = vistaMantto.confHoraLunes;
            configuracion.ConfiguracionHoraTransferencia.Martes = vistaMantto.confHoraMartes;
            configuracion.ConfiguracionHoraTransferencia.Miercoles = vistaMantto.confHoraMiercoles;
            configuracion.ConfiguracionHoraTransferencia.Jueves = vistaMantto.confHoraJueves;
            configuracion.ConfiguracionHoraTransferencia.Viernes = vistaMantto.confHoraViernes;
            configuracion.ConfiguracionHoraTransferencia.Sabado = vistaMantto.confHoraSabado;
            configuracion.ConfiguracionHoraTransferencia.Domingo = vistaMantto.confHoraDomingo;
            configuracion.ConfiguracionHoraTransferencia.Activo = vistaMantto.confHoraActivo;

            configuracion.NivelesABC = vistaMantto.NivelABCBO;

            return configuracion;
        }
        public void AgregarConfiguracionABC(int indiceOrigen) {
            try {
                List<NivelABCBO> lstConfABC;
                List<NivelABCBO> lstABC;
                if (this.vistaMantto.NivelABCBO == null)
                    lstABC = new List<NivelABCBO>();
                else
                    lstABC = this.vistaMantto.NivelABCBO;
                if (this.vistaMantto.ConfNivelABCBO == null)
                    lstConfABC = new List<NivelABCBO>();
                else
                    lstConfABC = this.vistaMantto.ConfNivelABCBO;
                lstConfABC.Add(vistaMantto.NivelABCBO[indiceOrigen]);
                lstABC.Remove(vistaMantto.NivelABCBO[indiceOrigen]);

                this.vistaMantto.NivelABCBO = lstABC;
                this.vistaMantto.ConfNivelABCBO = lstConfABC;
            } catch (Exception) {
                
                throw;
            }
        }
        public void QuitarConfiguracionABC(int indiceOrigen) {
            try {
                List<NivelABCBO> lstConfABC;
                List<NivelABCBO> lstABC;
                if (this.vistaMantto.NivelABCBO == null)
                    lstABC = new List<NivelABCBO>();
                else
                    lstABC = this.vistaMantto.NivelABCBO;
                if (this.vistaMantto.ConfNivelABCBO == null)
                    lstConfABC = new List<NivelABCBO>();
                else
                    lstConfABC = this.vistaMantto.ConfNivelABCBO;
                lstABC.Add(vistaMantto.ConfNivelABCBO[indiceOrigen]);
                lstConfABC.Remove(vistaMantto.ConfNivelABCBO[indiceOrigen]);

                this.vistaMantto.NivelABCBO = lstABC;
                this.vistaMantto.ConfNivelABCBO = lstConfABC;
            } catch (Exception) {

                throw;
            }
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
            IMantenimientoConfiguracionTransferenciaVIS vMantto = null;
            IBuscadorConfiguracionTransferenciasVIS vConsulta = null;
            bool esMantenimeinto = false;
            if (objetoVista is IMantenimientoConfiguracionTransferenciaVIS) {
                vMantto = (IMantenimientoConfiguracionTransferenciaVIS)objetoVista;
                esMantenimeinto = true;
            } else if (objetoVista is IBuscadorConfiguracionTransferenciasVIS) {
                vConsulta = (IBuscadorConfiguracionTransferenciasVIS)objetoVista;
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
                case ECatalogoBuscador.TipoPedido:
                    #region TipoPedido
                    TipoPedidoBO tipoPedido = new TipoPedidoBO();
                    string nombreTipoPedido = string.Empty;
                    if (esMantenimeinto) {
                        tipoPedido.Activo = true;
                        nombreTipoPedido = vMantto.NombreTipoPedido;
                    } else {
                        nombreTipoPedido = vConsulta.NombreTipoPedido;
                    }
                    esNumero = int.TryParse(nombreTipoPedido, out id);
                    if (esNumero)
                        tipoPedido.Id = id;
                    else
                        tipoPedido.Nombre = nombreTipoPedido;
                    objetoBuscador = tipoPedido;
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
            if (objetoVista is IMantenimientoConfiguracionTransferenciaVIS)
                esMantenimiento = true;
            else if (objetoVista is IBuscadorConfiguracionTransferenciasVIS)
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
                    case ECatalogoBuscador.TipoPedido:
                        #region Empleado
                        TipoPedidoBO tipoPedido = (TipoPedidoBO)objeto;
                        if (esMantenimiento) {
                            vistaMantto.TipoPedidoId = tipoPedido.Id;
                            vistaMantto.NombreTipoPedido = tipoPedido.Nombre;
                        } else {
                            vistaConsulta.TipoPedidoId = tipoPedido.Id;
                            vistaConsulta.NombreTipoPedido = tipoPedido.Nombre;
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
