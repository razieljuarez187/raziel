using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DAO;
using BPMO.Security.BR;

namespace BPMO.Refacciones.BR {
    public class CotizacionNotaTallerBR : IBRBaseDocumento {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return this.ultimoIdGenerado; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Crea un registro de cotización de nota de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Cotización de Nota de taller que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO cotizacionNotaTaller, SeguridadBO firma) {
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firmaConexion = Guid.NewGuid();
            Guid firmaTransaccion = Guid.NewGuid();
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion

                #region Validar parámetros
                string mensajeError = String.Empty;
                if (dataContext == null)
                    mensajeError += " , DataContext";
                if (cotizacionNotaTaller == null || !(cotizacionNotaTaller is CotizacionNotaTallerBO))
                    mensajeError += " , CotizacionNotaTaller";
                if (mensajeError.Length > 0)
                    throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes parámetros no pueden ser nulos!!!");
                if (cotizacionNotaTaller.GetChildren().Count < 1)
                    throw new Exception("La cotización de nota de taller debe tener cuando menos un detalle!!!");
                #endregion Validar parámetros

                #region Apertura de transacción
                dataContext.OpenConnection(firmaConexion);
                dataContext.BeginTransaction(firmaTransaccion);
                #endregion

                CotizacionNotaTallerInsertarDAO insertarDAO = new CotizacionNotaTallerInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, cotizacionNotaTaller);
                registrosAfectados = insertarDAO.RegistrosAfectados;
                ultimoIdGenerado = insertarDAO.UltimoIdGenerado;
                cotizacionNotaTaller.Id = ultimoIdGenerado;
                bool esExitoDetalle = false;
                if (esExito) {
                    DetalleCotizacionNotaTallerBR detalleCotizacionNotaTallerBR = new DetalleCotizacionNotaTallerBR();
                    foreach (DetalleDocumentoBaseBO detalleDocumento in cotizacionNotaTaller.GetChildren()) {
                        esExitoDetalle = detalleCotizacionNotaTallerBR.Insertar(dataContext, cotizacionNotaTaller, detalleDocumento, firma);
                        if (!esExitoDetalle) {
                            dataContext.RollbackTransaction(firmaTransaccion);
                            throw new Exception("OCurrió un error al insertar el detalle de la cotización de la nota de taller!");
                        }
                    }
                } else {
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error desconocido al insertar la cotización de nota de taller!");
                }

                #region Cierre de transacción
                dataContext.CommitTransaction(firmaTransaccion);
                if (esExito && esExitoDetalle)
                    return true;
                else
                    return false;
                #endregion
            } catch {
                dataContext.RollbackTransaction(firmaTransaccion);
                throw;
            } finally {
                dataContext.CloseConnection(firmaConexion);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
        }
        /// <summary>
        /// Actualiza un registro de cotización de nota de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cotizacionNotaTaller">Cotización de Nota de taller que se desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO cotizacionNotaTaller, SeguridadBO firma) {
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firmaConexion = Guid.NewGuid();
            Guid firmaTransaccion = Guid.NewGuid();
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion

                #region Apertura de conexión y transacción
                dataContext.OpenConnection(firmaConexion);
                dataContext.BeginTransaction(firmaTransaccion);
                #endregion

                #region Modificación de los datos
                CotizacionNotaTallerActualizarDAO actualizarDAO = new CotizacionNotaTallerActualizarDAO();
                bool esExito = actualizarDAO.Actualizar(dataContext, cotizacionNotaTaller);
                bool esExitoDetalle = false;
                if (esExito) {
                    DetalleCotizacionNotaTallerBR detalleActualizarDAO = new DetalleCotizacionNotaTallerBR();
                    foreach (DetalleDocumentoBaseBO detalle in cotizacionNotaTaller.GetChildren()) {
                        esExitoDetalle = detalleActualizarDAO.Actualizar(dataContext, cotizacionNotaTaller, detalle, firma);
                        if (!esExitoDetalle) {
                            dataContext.RollbackTransaction(firmaTransaccion);
                            throw new Exception("Ocurrión un error al actualizar el detalle de la cotización de nota de taller!");
                        }
                    }
                } else {
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error al actualizar la nota de taller!!!");
                }
                #endregion

                #region Cierre de conexión y transacción
                dataContext.CommitTransaction(firmaTransaccion);
                return true;
                #endregion
            } catch {
                dataContext.RollbackTransaction(firmaTransaccion);
                throw;
            } finally {
                dataContext.CloseConnection(firmaConexion);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
        }
        /// <summary>
        /// Elimina un registro de cotización de nota de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cotizacionNotaTaller">Cotización de Nota de taller que se desea eliminar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, DocumentoBaseBO cotizacionNotaTaller, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de cotización de notas de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cotizacionNotaTaller">Cotización de Nota de taller que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las cotizaciones de notas de taller recuperados por la consulta</returns>
        public List<DocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO cotizacionNotaTaller) {
            try {
                CotizacionNotaTallerConsultarDAO consultarDAO = new CotizacionNotaTallerConsultarDAO();
                return consultarDAO.Consultar(dataContext, cotizacionNotaTaller);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Consulta registros de cotizaciones de notas de taller hasta su primer nivel de asociación y composición
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cotizacionNotaTaller">Cotización de Nota de taller que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las cotizaciones de notas de taller y sus relaciones a primer nivel, recuperados por la consulta</returns>
        public List<DocumentoBaseBO> ConsultarCompleto(IDataContext dataContext, DocumentoBaseBO cotizacionNotaTaller) {
            try {
                List<DocumentoBaseBO> lstDocumentosBase = this.Consultar(dataContext, cotizacionNotaTaller);
                if (lstDocumentosBase.Count > 0) {
                    if (cotizacionNotaTaller.Id != null) {
                        DetalleCotizacionNotaTallerBR detCotNotaTallerBR = new DetalleCotizacionNotaTallerBR();
                        List<DetalleDocumentoBaseBO> lstDetalleCotizacionNotaTaller = detCotNotaTallerBR.Consultar(dataContext, cotizacionNotaTaller);
                        foreach (DetalleDocumentoBaseBO detalleCotizacionNotaTaller in lstDetalleCotizacionNotaTaller) {
                            lstDocumentosBase[0].Add(detalleCotizacionNotaTaller);
                        }
                    }
                }
                return lstDocumentosBase;
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Generar una Nota de Taller a partir de una Cotización de Nota de Taller
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="documentoBase"></param>
        /// <param name="firma"></param>
        /// <returns></returns>
        public NotaTallerBO GenerarNotaTaller(IDataContext dataContext, CotizacionNotaTallerBO cotizacion, SeguridadBO firma) {
            NotaTallerBO result = null;
            Guid miFirma = Guid.NewGuid();
            ManejadorDataContext manejadorDctx = new ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion

                #region Validar parametros
                string mensajeError = string.Empty;
                if (dataContext == null)
                    mensajeError += " , DataContext";
                if (cotizacion == null)
                    mensajeError += " , CotizacionNotaTaller";
                if (firma == null)
                    mensajeError += " , SeguridadBO";
                if (cotizacion.GetChildren().Count == 0)
                    mensajeError += " , Detalles de Cotizacion";
                if (mensajeError.Length > 0)
                    throw new ArgumentNullException(mensajeError.Substring(2));
                #endregion

                #region Mapeo Encabezado
                NotaTallerBR notaBr = new NotaTallerBR();
                result = new NotaTallerBO();
                result.EmpresaLiderId = cotizacion.EmpresaLiderId;
                result.SucursalLiderId = cotizacion.SucursalLiderId;
                result.Almacen = new AlmacenBO() { Id = cotizacion.AlmacenLiderId };
                result.AreaLiderId = cotizacion.AreaId;
                result.MecanicoLiderId = cotizacion.MecanicoId;
                result.UsuarioLiderId = cotizacion.UsuarioLiderId;
                result.OrdenServicioId = cotizacion.OrdenServicioId;
                result.MovimientoId = cotizacion.MovimientoId ?? 0; // Si es null, se asigna 0
                result.Estatus = new EstatusBO() { Id = 1 };
                result.NumeroReferencia = cotizacion.NumeroReferencia;
                result.TipoReferencia = cotizacion.TipoReferencia;
                result.NotaTallerReferencia = 0; // Ya que no es devolución, se asigna 0
                result.ClienteLiderId = cotizacion.ClienteLiderId;
                result.DiasCredito = cotizacion.DiasCredito;
                result.Observaciones = cotizacion.Observaciones;
                result.EsRescate = cotizacion.EsRescate;
                result.TipoPedidoId = cotizacion.TipoPedidoId;
                result.EsDevolucion = cotizacion.EsDevolucion;
                result.EstaImpreso = cotizacion.EstaImpreso;
                result.MonedaLiderId = cotizacion.MonedaId;
                result.KitId = cotizacion.KitId;
                #endregion

                #region Mapeo de Detalles
                foreach (DetalleCotizacionNotaTallerBO detCotiza in cotizacion.GetChildren()) {
                    if (detCotiza.Articulo == null) continue;
                    // Obtener Existencia
                    ExistenciaAlmacenRefaccionesBR existenciaBR = new ExistenciaAlmacenRefaccionesBR();
                    ExistenciaAlmacenRefaccionesBO refaccion = new ExistenciaAlmacenRefaccionesBO()
                    {
                        EmpresaLiderId = cotizacion.EmpresaLiderId,
                        SucursalLiderId = cotizacion.SucursalLiderId,
                        Almacen = new AlmacenBO() { Id = cotizacion.AlmacenLiderId },
                        Articulo = new ArticuloBO() { Id = detCotiza.Articulo.Id }
                    };
                    refaccion = existenciaBR.Consultar(dataContext, refaccion).ConvertAll(r => r as ExistenciaAlmacenRefaccionesBO).FirstOrDefault();

                    // Si no tiene existencia, no lo agrego a la lista de detalles de la N.T.
                    if (refaccion.Disponible <= 0) continue;

                    // Ajustar cantidad a sutir
                    detCotiza.CantidadSurtida = refaccion.Disponible < detCotiza.CantidadSolicitada ? refaccion.Disponible : detCotiza.CantidadSolicitada;
                    // Mapear el detalle de la NotaTaller
                    DetalleNotaTallerBO detalle = new DetalleNotaTallerBO();
                    detalle.Articulo = new ArticuloBO() { Id = detCotiza.Articulo.Id };
                    detalle.Cantidad = detCotiza.CantidadSurtida;
                    detalle.CantidadSurtida = 0;
                    detalle.CantidadCancelada = 0;
                    detalle.CantidadReservada = detCotiza.CantidadSurtida; //La cantidad 'Surtida' de la Cotización, es Reservada en NT, ya que aún no se aplica, a diferencia de la cotización.
                    detalle.CantidadDevuelta = 0;
                    if (detCotiza.CostoUnitario.HasValue && detCotiza.CostoUnitario != 0)
                        detalle.CostoUnitario = detCotiza.CostoUnitario;
                    else
                        detalle.CostoUnitario = refaccion.CostoPromedio;
                    detalle.PrecioUnitario = detCotiza.PrecioUnitario;
                    detalle.PrecioArticuloOriginal = detCotiza.PrecioUnitario;
                    detalle.PorcentajeImpuesto = detCotiza.PorcentajeImpuesto;
                    detalle.EmpresaLiderReservaId = detCotiza.EmpresaLiderReservaId;
                    detalle.SucursalLiderReservaId = detCotiza.SucursalLiderReservaId;
                    detalle.AlmacenReserva = new AlmacenBO() { Id = detCotiza.AlmacenId };
                    detalle.MonedaLiderOriginalId = cotizacion.MonedaId;
                    detalle.TipoRemisionId = detCotiza.TipoRemisionId;                    

                    if (detCotiza.ArticuloCore != null && detCotiza.ArticuloCore.Id != null) {
                        detalle.ArticuloCore = new ArticuloBO() { Id = detCotiza.ArticuloCore.Id };
                        if (detCotiza.CostoArticuloCore.HasValue && detCotiza.CostoArticuloCore != 0)
                            detalle.CostoArticuloCore = detCotiza.CostoArticuloCore;
                        else
                            detalle.CostoArticuloCore = refaccion.CostoPromedioCore;
                        detalle.PrecioArticuloCore = detCotiza.PrecioArticuloCore;
                        detalle.PrecioArticuloCoreOriginal = detCotiza.PrecioArticuloCoreOriginal;
                    }
                    result.Add(detalle);
                }
                #endregion

                // Si no se agregó ningún detalle (por falta de existencia) no se crea la N.T. y se devuelve null
                if (result.GetChildren().Count > 0) {
                    // Inserción de la Nota de Taller
                    notaBr.Insertar(dataContext, result, firma);
                    result.Id = notaBr.UltimoIdGenerado;
                    // Se asigna la NotaTallerId relacionada a la Cotización recibida
                    cotizacion.NotaTallerId = notaBr.UltimoIdGenerado;
                } else {
                    result = null;
                }
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(miFirma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }

            return result;
        }
        #endregion
    }
}
