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
using BPMO.Refacciones.Enumeradores;

namespace BPMO.Refacciones.Procesos.PRE {
    public class ExcepcionEtapaEmbalajePRE {
        #region Atributos
        private IBuscadorExcepcionEtapaEmbalajeVIS vistaConsulta;
        private IMantenimientoExcepcionEtapaEmbalajeVIS vistaMantto;
        private IDataContext dataContext = null;
        private ExcepcionEtapaEmbalajeBR excepcionBR;
        private ConfiguracionEtapaEmbalajeBO excepcionBO;
        public enum ECatalogoBuscador {
            Empresa = 1,
            Sucursal = 2,
            Almacen = 3,
            TipoPedido = 4,
        };
        #endregion Atributos
        #region Constructores
        /// <summary>
        /// Constructor para consulta de Configuración
        /// </summary>
        /// <param name="vistaConsulta">Vista con la que trabajará el presentador</param>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        public ExcepcionEtapaEmbalajePRE(IBuscadorExcepcionEtapaEmbalajeVIS vistaConsulta, List<DatosConexionBO> listadoCnx) {
            this.vistaConsulta = vistaConsulta;
            AgregarProviderDataContext(listadoCnx);
        }
        /// <summary>
        /// Constructor para mantenimiento de Configuración
        /// </summary>
        /// <param name="vistaMantenimiento">Vista con la que trabajará el presentador</param>
        /// <param name="listadoCnx"> List DatosConexionBO provee parámetros de conexión a agregar</param>
        public ExcepcionEtapaEmbalajePRE(IMantenimientoExcepcionEtapaEmbalajeVIS vistaMantenimiento, List<DatosConexionBO> listadoCnx) {
            this.vistaMantto = vistaMantenimiento;
            AgregarProviderDataContext(listadoCnx);
        }
        #endregion Constructores
        #region Métodos
        #region Consulta
        /// <summary>
        /// Consultar Listado de excepciones, si la búsqueda retorna 1 registro se presenta el detalle del elemento para su edición
        /// </summary>
        public void ObtenerExcepciones() {
            try {
                List<ConfiguracionEtapaEmbalajeBO> excepciones = this.Consultar(this.InterfazADatosConsulta());
                if (excepciones.Count == 1) {
                    vistaConsulta.EnviarAEditarBO(excepciones[0].Id.Value);
                } else {
                    vistaConsulta.ListadoExcepcionesEmbalaje = excepciones;
                    if (excepciones.Count == 0)
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
            List<ConfiguracionEtapaEmbalajeBO> listaOrdenada = null;
            if (campo.ToUpper().Equals("ID"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.Id).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.Id).ToList();
            if (campo.ToUpper().Equals("EMPRESA"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.Empresa.Nombre).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.Empresa.NombreCorto).ToList();
            if (campo.ToUpper().Equals("SUCURSAL"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.Sucursal.Nombre).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.Sucursal.Nombre).ToList();
            if (campo.ToUpper().Equals("ALMACEN"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.Almacen.Nombre).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.Almacen.Nombre).ToList();
            if (campo.ToUpper().Equals("TIPOMOVIMIENTO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.TipoMovimiento.ToString()).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.TipoMovimiento.ToString()).ToList();
            if (campo.ToUpper().Equals("TIPOPEDIDO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.TipoPedido.Id).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.TipoPedido.Nombre).ToList();
            if (campo.ToUpper().Equals("ACTIVO"))
                listaOrdenada = (orden.Equals(asc)) ? vistaConsulta.ListadoExcepcionesEmbalaje.OrderBy(order => order.Activo).ToList() : vistaConsulta.ListadoExcepcionesEmbalaje.OrderByDescending(order => order.Activo).ToList();
            vistaConsulta.ListadoExcepcionesEmbalaje = listaOrdenada;
        }
        /// <summary>
        /// Obtiene los datos de la interfaz de Consulta
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionEtapaEmbalajeBO InterfazADatosConsulta() {
            ConfiguracionEtapaEmbalajeBO excepcion = new ConfiguracionEtapaEmbalajeBO();
            excepcion.Id = vistaConsulta.Id;
            excepcion.Empresa = new EmpresaLiderBO() { Id = vistaConsulta.EmpresaId };
            excepcion.Sucursal = new SucursalLiderBO() { Id = vistaConsulta.SucursalId };
            excepcion.Almacen = new AlmacenBO() { Id = vistaConsulta.AlmacenId };
            excepcion.TipoMovimiento = vistaConsulta.TipoMovimiento;
            excepcion.TipoPedido = new TipoPedidoBO() { Id = vistaConsulta.TipoPedidoId };
            excepcion.Activo = vistaConsulta.Activo;
            return excepcion;
        }
        /// <summary>
        /// Consultar excepciones de embalaje
        /// </summary>
        /// <param name="excepcionBO">Objeto que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista de objetos que contiene la información que coincide con la consulta</returns>
        private List<ConfiguracionEtapaEmbalajeBO> Consultar(ConfiguracionEtapaEmbalajeBO excepcionBO) {
            excepcionBR = new ExcepcionEtapaEmbalajeBR();
            List<AuditoriaBaseBO> lstTransferencias = excepcionBR.Consultar(dataContext, excepcionBO);
            return lstTransferencias.ConvertAll(item => (ConfiguracionEtapaEmbalajeBO)item);
        }
        #endregion Consulta
        #region Mantenimiento
        /// <summary>
        /// Insertar excepción
        /// </summary>
        public void Insertar() {
            try {
                string camposRequeridos = this.ValidaRequeridos();
                if (!String.IsNullOrEmpty(camposRequeridos)) {
                    vistaMantto.MostrarMensaje(camposRequeridos, ETipoMensajeIU.INFORMACION);
                    return;
                }
                excepcionBR = new ExcepcionEtapaEmbalajeBR();
                excepcionBO = this.InterfazADatosMantto();
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vistaMantto.UsuarioSesion, this.vistaMantto.Adscripcion);
                excepcionBR.Insertar(dataContext, excepcionBO, seguridadBO);
                vistaMantto.Id = excepcionBO.Id = excepcionBR.UltimoIdGenerado;
                this.DesplegarDetalleExcepcionEtapaEmbalaje(excepcionBO.Id);
                vistaMantto.MostrarMensaje("Guardado Exitoso", ETipoMensajeIU.EXITO);
            } catch (Exception ex) {
                vistaMantto.MostrarMensaje("Guardado no exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Actualizar excepción
        /// </summary>
        public void Actualizar() {
            try {
                string camposRequeridos = this.ValidaRequeridos();
                if (!String.IsNullOrEmpty(camposRequeridos)) {
                    vistaMantto.MostrarMensaje(camposRequeridos, ETipoMensajeIU.INFORMACION);
                    return;
                }
                excepcionBR = new ExcepcionEtapaEmbalajeBR();
                ConfiguracionEtapaEmbalajeBO excepcion = this.InterfazADatosEditar();
                SeguridadBO seguridadBO = new SeguridadBO(Guid.Empty, this.vistaMantto.UsuarioSesion, this.vistaMantto.Adscripcion);
                excepcionBR.Actualizar(dataContext, excepcion, seguridadBO);
                this.DesplegarDetalleExcepcionEtapaEmbalaje(vistaMantto.Id);
                vistaMantto.MostrarMensaje("Guardado Exitoso", ETipoMensajeIU.EXITO);
            } catch (Exception ex) {
                vistaMantto.MostrarMensaje("Guardado no exitoso", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        /// <summary>
        /// Obtiene los datos de la Interfaz, para su edición
        /// </summary>
        /// <returns>Objeto que contiene los datos obtenidos de la interfaz</returns>
        private ConfiguracionEtapaEmbalajeBO InterfazADatosEditar() {
            ConfiguracionEtapaEmbalajeBO excepcion = (ConfiguracionEtapaEmbalajeBO)vistaMantto.ExcepcionEtapaEmbalajeBase;
            excepcion.Empresa = new EmpresaLiderBO() { Id = vistaMantto.EmpresaId, Nombre = vistaMantto.NombreEmpresa };
            excepcion.Sucursal = new SucursalLiderBO() { Id = vistaMantto.SucursalId, Nombre = vistaMantto.NombreSucursal };
            excepcion.Almacen = new AlmacenBO() { Id = vistaMantto.AlmacenId, Nombre = vistaMantto.NombreAlmacen };
            excepcion.TipoMovimiento = vistaMantto.TipoMovimiento;
            excepcion.TipoPedido = new TipoPedidoBO() { Id = vistaMantto.TipoPedidoId, Nombre = vistaMantto.NombreTipoPedido };
            excepcion.Activo = vistaMantto.Activo;
            excepcion.Auditoria.UUA = vistaMantto.UsuarioSesion.Id;
            return excepcion;
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
            this.DatosAInterfaz((ConfiguracionEtapaEmbalajeBO)this.vistaMantto.ExcepcionEtapaEmbalajeBase);
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
        /// Despliega el Detalle de la excepción
        /// </summary>
        public void DesplegarDetalleExcepcionEtapaEmbalaje(int? excepcionId) {
            this.DatosAInterfaz(ConsultarExcepcion(new ConfiguracionEtapaEmbalajeBO { Id = excepcionId }));
            vistaMantto.PreparaUIDetalle();
        }
        /// <summary>
        /// Asocia los datos a la Interfaz
        /// </summary>
        /// <param name="excepcion">Objeto con la información a desplegar </param>
        private void DatosAInterfaz(ConfiguracionEtapaEmbalajeBO excepcion) {
            vistaMantto.ExcepcionEtapaEmbalajeBase = excepcion;
            vistaMantto.Id = excepcion.Id;
            vistaMantto.EmpresaId = excepcion.Empresa.Id;
            vistaMantto.NombreEmpresa = excepcion.Empresa.Nombre;
            vistaMantto.SucursalId = excepcion.Sucursal.Id;
            vistaMantto.NombreSucursal = excepcion.Sucursal.Nombre;
            vistaMantto.AlmacenId = excepcion.Almacen.Id;
            vistaMantto.NombreAlmacen = excepcion.Almacen.Nombre;
            vistaMantto.TipoMovimiento = excepcion.TipoMovimiento;
            vistaMantto.TipoPedidoId = excepcion.TipoPedido.Id;
            vistaMantto.NombreTipoPedido = excepcion.TipoPedido.Nombre;
            vistaMantto.Activo = excepcion.Activo;
            #region Información de la bitácora
            string nombreUsuarioCreacion = this.ObtenerNombreUsuario(excepcion.Auditoria.UC.Value);
            this.vistaMantto.UsuarioCreacionBitacora = nombreUsuarioCreacion;
            this.vistaMantto.FechaCreacionBitacora = excepcion.Auditoria.FC;
            if (excepcion.Auditoria.UC != excepcion.Auditoria.UUA) {
                this.vistaMantto.UsuarioActualizacionBitacora = this.ObtenerNombreUsuario(excepcion.Auditoria.UUA.Value);
            } else {
                this.vistaMantto.UsuarioActualizacionBitacora = nombreUsuarioCreacion;
            }
            this.vistaMantto.FechaActualizacionBitacora = excepcion.Auditoria.FUA;
            #endregion
        }
        /// <summary>
        /// Consulta una Configuración específica, de acuerdo a un objeto seleccionado
        /// </summary>
        /// <param name="excepcion">Objeto que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista de objetos que contiene la información que coincide con la consulta</returns>
        private ConfiguracionEtapaEmbalajeBO ConsultarExcepcion(ConfiguracionEtapaEmbalajeBO excepcion) {
            if (excepcion.Id == null)
                throw new Exception("El ID de excepción es requerido para obtener un único registro");
            this.excepcionBR = new ExcepcionEtapaEmbalajeBR();
            ConfiguracionEtapaEmbalajeBO excepcionBO = (ConfiguracionEtapaEmbalajeBO)this.excepcionBR.Consultar(dataContext, excepcion)[0];
            return excepcionBO;
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
            if (!vistaMantto.TipoMovimiento.HasValue)
                sError += " , TipoMovimiento";
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
        private ConfiguracionEtapaEmbalajeBO InterfazADatosMantto() {
            ConfiguracionEtapaEmbalajeBO excepcion = new ConfiguracionEtapaEmbalajeBO();
            excepcion.Empresa = new EmpresaLiderBO() { Id = vistaMantto.EmpresaId, Nombre = vistaMantto.NombreEmpresa };
            excepcion.Sucursal = new SucursalLiderBO() { Id = vistaMantto.SucursalId, Nombre = vistaMantto.NombreSucursal };
            excepcion.Almacen = new AlmacenBO() { Id = vistaMantto.AlmacenId, Nombre = vistaMantto.NombreAlmacen };
            excepcion.TipoMovimiento = vistaMantto.TipoMovimiento;
            excepcion.TipoPedido = new TipoPedidoBO() { Id = vistaMantto.TipoPedidoId, Nombre = vistaMantto.NombreTipoPedido };
            excepcion.Activo = vistaMantto.Activo;
            excepcion.Auditoria = new AuditoriaBO();
            excepcion.Auditoria.UC = excepcion.Auditoria.UUA = vistaMantto.UsuarioSesion.Id;
            return excepcion;
        }
        #endregion Mantenimiento
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
            IMantenimientoExcepcionEtapaEmbalajeVIS vMantto = null;
            IBuscadorExcepcionEtapaEmbalajeVIS vConsulta = null;
            bool esMantenimeinto = false;
            if (objetoVista is IMantenimientoExcepcionEtapaEmbalajeVIS) {
                vMantto = (IMantenimientoExcepcionEtapaEmbalajeVIS)objetoVista;
                esMantenimeinto = true;
            } else if (objetoVista is IBuscadorExcepcionEtapaEmbalajeVIS) {
                vConsulta = (IBuscadorExcepcionEtapaEmbalajeVIS)objetoVista;
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
                            vMantto.MostrarMensaje("Es necesario que primero seleccione una sucursal.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        _sucursalId = vistaMantto.SucursalId;
                    } else {
                        nombreTaller = vConsulta.NombreAlmacen;
                        if (vConsulta.SucursalId == null) {
                            vConsulta.MostrarMensaje("Es necesario que primero seleccione una sucursal.", ETipoMensajeIU.ADVERTENCIA);
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
                        if (vMantto.TipoMovimiento == null) {
                            vMantto.MostrarMensaje("Es necesario que primero seleccione un tipo de movimiento.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        switch(vMantto.TipoMovimiento){
                            case ETipoMovimiento.TRANSFERENCIA:
                                tipoPedido.AplicaTransferencia = true;
                                break;
                            case ETipoMovimiento.VENTA:
                                tipoPedido.AplicaVenta = true;
                                break;
                        }
                    } else {
                        nombreTipoPedido = vConsulta.NombreTipoPedido;
                        if (vConsulta.TipoMovimiento == null) {
                            vConsulta.MostrarMensaje("Es necesario que primero seleccione un tipo de movimiento.", ETipoMensajeIU.ADVERTENCIA);
                            return null;
                        }
                        switch(vConsulta.TipoMovimiento){
                            case ETipoMovimiento.TRANSFERENCIA:
                                tipoPedido.AplicaTransferencia = true;
                                break;
                            case ETipoMovimiento.VENTA:
                                tipoPedido.AplicaVenta = true;
                                break;
                        }
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
            if (objetoVista is IMantenimientoExcepcionEtapaEmbalajeVIS)
                esMantenimiento = true;
            else if (objetoVista is IBuscadorExcepcionEtapaEmbalajeVIS)
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
        #endregion Buscador
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
        #endregion Métodos
    }
}
