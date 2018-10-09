using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.Procesos.REPORTESVIS;
using BPMO.Basicos.BO;
using BPMO.Refacciones.BO;
using System.Data;
using BPMO.Refacciones.BR;
using BPMO.Primitivos.Enumeradores;

namespace BPMO.Refacciones.Procesos.REPORTESPRE {
    public class ConfiguracionesAsignadasPRE {
        #region Atributos
        private IConfiguracionesAsignadasVIS vista;
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
        /// Constructor Presentador para obtener la productividad del usuario (Técnico, Controlista, JefeTaller)
        /// </summary>
        /// <param name="vista">IProductividadUsuarioVIS que provee el manejo de la UI</param>
        /// <param name="listadoCnx">Listado de DatosConexionBO que provee los parámetros de conexión a la Base de Datos</param>
        public ConfiguracionesAsignadasPRE(IConfiguracionesAsignadasVIS vista, List<DatosConexionBO> listadoCnx) {
            this.vista = vista;
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

        #region Métodos
        /// <summary>
        /// Obtiene la información contenida en el reporte en base a los filtros definidos por el usuario
        /// </summary>
        public void ConsultarProductividadUsuario() {
            try {
                ConfiguracionReglaUsuarioFiltroBO configFiltro = new ConfiguracionReglaUsuarioFiltroBO();
                configFiltro.Empresa = new EmpresaLiderBO() { Id = vista.EmpresaId };
                configFiltro.Sucursal = new SucursalLiderBO() { Id = vista.SucursalId };
                configFiltro.Almacen = new AlmacenBO() { Id = vista.AlmacenId };
                configFiltro.Usuario = new UsuarioLiderBO() { Id = vista.UsuarioId };
                configFiltro.TipoRegla = vista.TipoRegla;
                configFiltro.ValorInicial = vista.ValorInicialA;
                configFiltro.ValorInicialFin = vista.ValorInicialB;
                configFiltro.ValorFinal = vista.ValorFinalA;
                configFiltro.ValorFinalFin = vista.ValorFinalB;
                configFiltro.Activo = vista.Activo;

                configuracionBr = new ConfiguracionReglaUsuarioBR();
                DataSet dsConfAsignadas = configuracionBr.ConsultarFiltro(dataContext, configFiltro);
                if (dsConfAsignadas.Tables[0].Rows.Count < 1)
                    this.vista.MostrarMensaje("No se encontraron registros para la consulta especificada", ETipoMensajeIU.INFORMACION);
                vista.DesplegarConfiguracionesAsignadas(dsConfAsignadas);
            } catch (Exception ex) {
                this.vista.MostrarMensaje("Error al general el reporte", ETipoMensajeIU.ERROR, ex.Message);
            }
        }
        
        #region Buscador
        /// <summary>
        /// Obtiene el objeto que se enviara al buscador
        /// </summary>
        /// <param name="tipoBusqueda">Tipo de busqueda</param>
        /// <param name="objetoVista">Vista quien llama al método</param>
        /// <returns>Objeto que se enviara</returns>
        public object ObtenerObjetoBusqueda(ECatalogoBuscador tipoBusqueda) {
            #region Validar VIS
            object objetoBuscador = null;
            #endregion
            int id;
            bool esNumero;
            switch (tipoBusqueda) {
                case ECatalogoBuscador.Empresa:
                    #region Empresa
                    EmpresaLiderBO bo = new EmpresaLiderBO();
                    string nombreEmpresa = string.Empty;
                    nombreEmpresa = vista.NombreEmpresa;

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
                    nombreSucursal = vista.NombreSucursal;
                    if (vista.EmpresaId == null) {
                        vista.MostrarMensaje("Es necesario que primero seleccione una empresa.", ETipoMensajeIU.ADVERTENCIA);
                        return null;
                    }
                    _empresaId = vista.EmpresaId;
                    
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
                    
                    nombreTaller = vista.NombreAlmacen;
                    if (vista.SucursalId == null) {
                        vista.MostrarMensaje("Es necesario que primero seleccione un almacén.", ETipoMensajeIU.ADVERTENCIA);
                        return null;
                    }
                    _sucursalId = vista.SucursalId;

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
                    nombreUsuario = vista.NombreUsuario;
                    
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
        public void DesplegarBusqueda(ECatalogoBuscador tipoBusqueda, object objeto) {
            if (objeto != null) {
                switch (tipoBusqueda) {
                    case ECatalogoBuscador.Empresa:
                        #region Empresa
                        EmpresaLiderBO empresa = (EmpresaLiderBO)objeto;
                        vista.EmpresaId = empresa.Id;
                        vista.NombreEmpresa = empresa.Nombre;
                        vista.SucursalId = null;
                        vista.NombreSucursal = null;
                        vista.AlmacenId = null;
                        vista.NombreAlmacen = null;
                        break;
                        #endregion
                    case ECatalogoBuscador.Sucursal:
                        #region Sucursal
                        SucursalLiderBO sucursal = (SucursalLiderBO)objeto;
                        vista.SucursalId = sucursal.Id;
                        vista.NombreSucursal = sucursal.Nombre;
                        vista.AlmacenId = null;
                        vista.NombreAlmacen = null;
                        break;
                        #endregion
                    case ECatalogoBuscador.Almacen:
                        #region Almacén
                        AlmacenBO almacen = (AlmacenBO)objeto;
                        vista.AlmacenId = almacen.Id;
                        vista.NombreAlmacen = almacen.Nombre;
                        break;
                        #endregion
                    case ECatalogoBuscador.Usuario:
                        #region Empleado
                        UsuarioLiderBO usuario = (UsuarioLiderBO)objeto;
                        vista.UsuarioId = usuario.Id;
                        vista.NombreUsuario = usuario.Nombre;
                        break;
                        #endregion
                }
            }
        }
        #endregion
        
        #endregion
    }
}
