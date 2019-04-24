using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.CatalogosLider.BO;
using BPMO.CatalogosLider.BR;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Generales.BR;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios generales de notas de taller del sistema Líder
    /// </summary>
    public class NotaTallerProcesosBR {
        /// <summary>
        /// Devuelve las notas de taller relacionadas a una orden de servicio
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller que se utilizará como filtro para la derivacion de la lista de notas a devolver</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        public void DevolverPorCancelacionServicio(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
            Guid firmaConexion = Guid.NewGuid();
            Guid firmaTransaccion = Guid.NewGuid();
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            try {
                #region Validar Parámetros
                NotaTallerBO notaTaller = null;
                if (documentoBase is NotaTallerBO)
                    notaTaller = (NotaTallerBO)documentoBase;
                if (notaTaller == null)
                    throw new ArgumentNullException("NotaTaller", "El parametro no puede ser nulo!!!");
                if (notaTaller.OrdenServicioId == null)
                    throw new ArgumentNullException("NotaTaller.OrdenServicioId", "El parametro no puede ser nulo!!!");
                #endregion Validar Parámetros

                #region Verificar existencia de notas de taller
                NotaTallerBO notaTallerFiltro = new NotaTallerBO();
                NotaTallerBR notaTallerBR = new NotaTallerBR();
                notaTallerFiltro.OrdenServicioId = notaTaller.OrdenServicioId;
                notaTallerFiltro.Estatus = new EstatusBO();
                notaTallerFiltro.Estatus.Id = 3;
                notaTallerFiltro.EsDevolucion = false;
                List<DocumentoBaseBO> lstNotasTallerRelacionadas = notaTallerBR.Consultar(dataContext, notaTallerFiltro);
                if (lstNotasTallerRelacionadas.Count < 1)
                    throw new Exception("La orden de servicio referenciada no tiene notas de taller aplicadas y relacionadas!!!");
                #endregion Verificar existencia de notas de taller

                #region Preparar devoluciones de notas de taller
                List<NotaTallerBO> lstDevolucionesNotasTaller = new List<NotaTallerBO>();
                NotaTallerBO devolucionNotaTaller = new NotaTallerBO();
                foreach (NotaTallerBO notaTallerRelacionada in lstNotasTallerRelacionadas) {
                    devolucionNotaTaller = (NotaTallerBO)notaTallerBR.ConsultarCompleto(dataContext, notaTallerRelacionada)[0];
                    devolucionNotaTaller.EsDevolucion = true;
                    if (devolucionNotaTaller.PuedeDevolverse) {
                        devolucionNotaTaller.MovimientoId = 0;
                        devolucionNotaTaller.Estatus.Id = 1;
                        devolucionNotaTaller.NumeroReferencia = notaTallerRelacionada.Id;
                        devolucionNotaTaller.TipoReferencia = "0";
                        devolucionNotaTaller.EstaImpreso = true;
                        foreach (DetalleNotaTallerBO detalleDevolucionNT in devolucionNotaTaller.GetChildren()) {
                            if (detalleDevolucionNT.CantidadReal > 0) {
                                detalleDevolucionNT.CantidadDevuelta = detalleDevolucionNT.CantidadReal;
                                detalleDevolucionNT.Cantidad = 0;
                                detalleDevolucionNT.CantidadSurtida = 0;
                                detalleDevolucionNT.CantidadCancelada = 0;
                                detalleDevolucionNT.CantidadReservada = 0;
                            } else {
                                devolucionNotaTaller.Remove(detalleDevolucionNT);
                            }
                        }
                    }
                    lstDevolucionesNotasTaller.Add(devolucionNotaTaller);
                }
                #endregion Preparar devoluciones de notas de taller

                #region Apertura de Transacción
                dataContext.OpenConnection(firmaConexion);
                dataContext.BeginTransaction(firmaTransaccion);
                #endregion Apertura de Transacción

                #region Registro de devolucion
                MovimientoRefaccionBR movimientoRefaccionBR = new MovimientoRefaccionBR();
                MovimientoCoreBR movimientoCoreBR = new MovimientoCoreBR();
                foreach (NotaTallerBO notaTallerADevolver in lstDevolucionesNotasTaller) {
                    #region Inserción de Devoluciones de Notas de Taller
                    if (!notaTallerBR.Insertar(dataContext, notaTallerADevolver, firma)) {
                        dataContext.RollbackTransaction(firmaTransaccion);
                        throw new Exception("Ocurrió un error al registrar la devolucion de la nota de taller " + notaTallerADevolver.Id + "!!!");
                    }
                    #endregion Inserción de devoluciones de Notas de Taller

                    #region Registro de Movimientos de Refacciones

                    #region Insercion de Movimiento
                    MovimientoRefaccionBO movimientoRefaccion = new MovimientoRefaccionBO();
                    movimientoRefaccion.EmpresaLiderId = notaTallerADevolver.EmpresaLiderId;
                    movimientoRefaccion.SucursalLiderId = notaTallerADevolver.SucursalLiderId;
                    movimientoRefaccion.Almacen = notaTallerADevolver.Almacen;
                    movimientoRefaccion.UsuarioLiderId = notaTallerADevolver.UsuarioLiderId;
                    movimientoRefaccion.ConceptoId = 144;
                    movimientoRefaccion.NumeroReferencia = notaTallerADevolver.Id;
                    movimientoRefaccion.TipoReferencia = "DEV. NOTA DE TALLER";
                    movimientoRefaccion.MonedaLiderId = 1;
                    movimientoRefaccion.Divisa = new DivisaBO();
                    movimientoRefaccion.Divisa.TipoCambio = 1;
                    movimientoRefaccion.ClienteInternoId = 0;
                    movimientoRefaccion.EsConsigna = false;
                    movimientoRefaccion.Serie = String.Empty;
                    movimientoRefaccion.Folio = notaTallerADevolver.Id.ToString();
                    movimientoRefaccion.RmClienteId = notaTallerADevolver.ClienteLiderId;
                    ClienteLiderBO cliente = new ClienteLiderBO();
                    cliente.Id = notaTallerADevolver.ClienteLiderId;
                    ClienteLiderBR clienteBR = new ClienteLiderBR();
                    List<CatalogoBaseBO> lstClientes = new List<CatalogoBaseBO>();
                    lstClientes = clienteBR.Consultar(dataContext, cliente);
                    movimientoRefaccion.RmCliente = lstClientes[0].Nombre;
                    movimientoRefaccion.RmReferenciaId = notaTallerADevolver.OrdenServicioId;
                    movimientoRefaccion.RmReferencia = "ORDEN DE SERVICIO";
                    movimientoRefaccion.RmFolio = notaTallerADevolver.Id;
                    foreach (DetalleNotaTallerBO detalleDevolucionNotaTaller in notaTallerADevolver.GetChildren()) {
                        DetalleMovimientoRefaccionBO detalleMovimiento = new DetalleMovimientoRefaccionBO();
                        detalleMovimiento.Articulo = new ArticuloBO();
                        detalleMovimiento.Articulo.Id = detalleDevolucionNotaTaller.Articulo.Id;
                        detalleMovimiento.Cantidad = detalleDevolucionNotaTaller.CantidadDevuelta;
                        detalleMovimiento.CostoUnitario = detalleDevolucionNotaTaller.CostoUnitario;
                        detalleMovimiento.PrecioUnitario = detalleDevolucionNotaTaller.PrecioUnitario;
                        #region Datos de Core
                        if (detalleDevolucionNotaTaller.ArticuloCore != null && detalleDevolucionNotaTaller.ArticuloCore.Id != null) {
                            detalleMovimiento.ArticuloCore = new ArticuloBO();
                            detalleMovimiento.ArticuloCore.Id = detalleDevolucionNotaTaller.ArticuloCore.Id;
                            detalleMovimiento.CostoCore = detalleDevolucionNotaTaller.CostoArticuloCore;
                            detalleMovimiento.PrecioCore = detalleDevolucionNotaTaller.PrecioArticuloCore;
                        }
                        #endregion /Datos de Core (20180410)
                        movimientoRefaccion.Add(detalleMovimiento);
                    }
                    if (!movimientoRefaccionBR.Insertar(dataContext, movimientoRefaccion, firma)) {
                        dataContext.RollbackTransaction(firmaTransaccion);
                        throw new Exception("Ocurrió un error al registrar el movimiento de la devolucion de la nota de taller " + notaTallerADevolver.Id + "!!!");
                    }
                    #endregion Insercion de Movimiento

                    #region Registro de relacion de Notas de Taller
                    NotaTallerMovimientoRefaccionBO notaTallerMovimiento = new NotaTallerMovimientoRefaccionBO();
                    NotaTallerMovimientoRefaccionBR notaTallerMovimientoBR = new NotaTallerMovimientoRefaccionBR();
                    notaTallerMovimiento.NotaTallerOriginal = notaTallerADevolver;
                    notaTallerMovimiento.NotaTallerNueva = new NotaTallerBO();
                    notaTallerMovimiento.NotaTallerNueva.Id = notaTallerBR.UltimoIdGenerado;
                    notaTallerMovimiento.Movimiento = new MovimientoRefaccionBO();
                    notaTallerMovimiento.Movimiento.Id = movimientoRefaccionBR.UltimoIdGenerado;
                    if (!notaTallerMovimientoBR.Insertar(dataContext, notaTallerMovimiento, firma)) {
                        dataContext.RollbackTransaction(firmaTransaccion);
                        throw new Exception("Ocurrió un error al registrar la relacion de los movimientos y las notas de taller!!!");
                    }
                    #endregion Registro de relacion de Notas de Taller

                    #endregion Registro de Movimientos de Refacciones
                    /*
                    #region Registro de Movimientos de Cores

                    #region Insercion de Movimiento Cores
                    if (notaTallerADevolver.TieneCores) {
                        MovimientoCoreBO movimientoCore = new MovimientoCoreBO();
                        foreach (DetalleNotaTallerBO detalleDevolucionNotaTaller in notaTallerADevolver.GetChildren()) {
                            if (detalleDevolucionNotaTaller.TieneCores) {
                                movimientoCore.EmpresaLiderId = notaTallerADevolver.EmpresaLiderId;
                                movimientoCore.SucursalLiderId = notaTallerADevolver.SucursalLiderId;
                                movimientoCore.Almacen = notaTallerADevolver.Almacen;
                                movimientoCore.NumeroReferencia = notaTallerBR.UltimoIdGenerado;
                                movimientoCore.TipoReferencia = "CS";
                                movimientoCore.ConceptoId = 155;
                                movimientoCore.UsuarioLiderId = notaTallerADevolver.UsuarioLiderId;
                                movimientoCore.Recon = detalleDevolucionNotaTaller.Articulo;
                                movimientoCore.Core = detalleDevolucionNotaTaller.ArticuloCore;
                                movimientoCore.Cantidad = detalleDevolucionNotaTaller.CantidadDevuelta;
                                movimientoCore.Costo = detalleDevolucionNotaTaller.CostoArticuloCore;
                                movimientoCore.CostoDolares = detalleDevolucionNotaTaller.PrecioArticuloCoreOriginal;
                                movimientoCore.Precio = detalleDevolucionNotaTaller.PrecioArticuloCore;
                                movimientoCore.MonedaLiderId = detalleDevolucionNotaTaller.MonedaLiderOriginalId;
                                movimientoCore.TipoCambio = 1;
                                if (!movimientoCoreBR.Insertar(dataContext, movimientoCore, firma)) {
                                    dataContext.RollbackTransaction(firmaTransaccion);
                                    throw new Exception("Ocurrió un error al registrar el movimiento de cores de la devolucion de la nota de taller " + notaTallerBR.UltimoIdGenerado + "!!!");
                                }
                            }
                        }

                    }
                    #endregion Insercion de Movimiento Cores

                    #endregion Registro de Movimientos de Cores
                    */
                }
                #endregion Registro de devolucion

                #region Cierre de Transacción
                dataContext.CommitTransaction(firmaTransaccion);
                #endregion Cierre de Transacción
            } catch {
                dataContext.RollbackTransaction(firmaTransaccion);
                throw;
            } finally {
                dataContext.CloseConnection(firmaConexion);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
        }
        /// <summary>
        /// Clona la nota de taller en base a la nota enviada, validando la existencia de las piezas relacionadas a la nota base
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller base de la que se copiarán los datos para la nueva nota</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        public void ClonarNotaTaller(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firmaConexion = Guid.NewGuid();
            Guid firmaTransaccion = Guid.NewGuid();
            try {
                #region Validar parámetros
                NotaTallerBO notaTaller = null;
                if (documentoBase is NotaTallerBO)
                    notaTaller = (NotaTallerBO)documentoBase;
                string mensajeError = string.Empty;
                if (dataContext == null)
                    mensajeError += " , DataContext";
                if (notaTaller == null || notaTaller.Id == null)
                    mensajeError += " , NotaTaller.Id";
                if (notaTaller == null || notaTaller.OrdenServicioId == null)
                    mensajeError += " , NotaTaller.OrdenServicioId";
                if (mensajeError.Length > 0)
                    throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes parámetros no pueden ser nulos!!!");
                #endregion Validar parámetros

                #region Recuperar nota de taller
                NotaTallerBO notaTallerFiltro = new NotaTallerBO();
                notaTallerFiltro.Id = notaTaller.Id;
                notaTallerFiltro.Estatus = new EstatusBO();
                notaTallerFiltro.Estatus.Id = 3;
                notaTallerFiltro.EsDevolucion = false;
                NotaTallerBR notaTallerBR = new NotaTallerBR();
                List<DocumentoBaseBO> lstNotasTaller = notaTallerBR.ConsultarCompleto(dataContext, notaTallerFiltro);
                if (lstNotasTaller.Count < 1)
                    throw new Exception("La nota de taller referenciada no existe o tiene un estatus incorrecto!!!");
                NotaTallerBO notaTallerRecuperada = (NotaTallerBO)lstNotasTaller[0];
                #endregion Recuperar nota de taller

                #region Recuperar porcentaje de IVA vigente
                CatalogosGeneralesLiderBR generalesBR = new CatalogosGeneralesLiderBR();
                decimal porcentajeIVA = generalesBR.ConsultaIVASucursalLider(dataContext, (int)notaTallerRecuperada.SucursalLiderId);
                if (porcentajeIVA == 0)
                    throw new ArgumentException("La sucursal de Lider no existe! Favor de revisar!");
                #endregion Recuperar porcentaje de IVA vigente

                #region Verificar existencia
                ExistenciaAlmacenRefaccionesBO existencia;
                ExistenciaAlmacenRefaccionesBR existenciaBR = new ExistenciaAlmacenRefaccionesBR();
                List<AuditoriaBaseBO> lstExistencias;
                List<DetalleNotaTallerBO> lstDetallesNotaTaller = new List<DetalleNotaTallerBO>();
                foreach (DetalleNotaTallerBO detalleNotaTaller in notaTallerRecuperada.GetChildren()) {
                    if (detalleNotaTaller.CantidadReal > 0) {
                        existencia = new ExistenciaAlmacenRefaccionesBO();
                        existencia.EmpresaLiderId = notaTallerRecuperada.EmpresaLiderId;
                        existencia.SucursalLiderId = notaTallerRecuperada.SucursalLiderId;
                        existencia.Almacen = notaTallerRecuperada.Almacen;
                        existencia.Articulo = detalleNotaTaller.Articulo;
                        lstExistencias = new List<AuditoriaBaseBO>();
                        lstExistencias = existenciaBR.Consultar(dataContext, existencia);
                        if (lstExistencias == null || lstExistencias.Count < 1)
                            throw new Exception("No se encontró la parte en el almacén relacionado!!!");
                        existencia = (ExistenciaAlmacenRefaccionesBO)lstExistencias[0];
                        if ((existencia.Disponible < detalleNotaTaller.CantidadReal))
                            throw new Exception("Una de las partes de la nota de taller " + notaTallerRecuperada.Id + " no tiene existencia!!!");
                        lstDetallesNotaTaller.Add(detalleNotaTaller);
                    }
                }
                #endregion Verificar existencia

                #region Preparar nota de taller a insertar
                NotaTallerBO nuevaNotaTaller = new NotaTallerBO();
                nuevaNotaTaller.EmpresaLiderId = notaTallerRecuperada.EmpresaLiderId;
                nuevaNotaTaller.SucursalLiderId = notaTallerRecuperada.SucursalLiderId;
                nuevaNotaTaller.Almacen = notaTallerRecuperada.Almacen;
                nuevaNotaTaller.AreaLiderId = notaTallerRecuperada.AreaLiderId;
                nuevaNotaTaller.MecanicoLiderId = notaTallerRecuperada.MecanicoLiderId;
                nuevaNotaTaller.UsuarioLiderId = notaTallerRecuperada.UsuarioLiderId;
                nuevaNotaTaller.OrdenServicioId = notaTaller.OrdenServicioId;
                nuevaNotaTaller.Estatus = new EstatusBO();
                nuevaNotaTaller.Estatus.Id = 1;
                nuevaNotaTaller.NumeroReferencia = notaTallerRecuperada.Id;
                nuevaNotaTaller.TipoReferencia = notaTallerRecuperada.TipoReferencia;
                nuevaNotaTaller.ClienteLiderId = notaTallerRecuperada.ClienteLiderId;
                nuevaNotaTaller.DiasCredito = notaTallerRecuperada.DiasCredito;
                nuevaNotaTaller.Observaciones = notaTallerRecuperada.Observaciones;
                nuevaNotaTaller.EsRescate = notaTallerRecuperada.EsRescate;
                nuevaNotaTaller.TipoPedidoId = notaTallerRecuperada.TipoPedidoId;
                nuevaNotaTaller.EsDevolucion = false;
                nuevaNotaTaller.EstaImpreso = true;
                nuevaNotaTaller.MonedaLiderId = notaTallerRecuperada.MonedaLiderId;
                nuevaNotaTaller.KitId = notaTallerRecuperada.KitId;
                nuevaNotaTaller.NotaTallerReferencia = notaTallerRecuperada.NotaTallerReferencia;
                foreach (DetalleNotaTallerBO detalleNotaTallerAInsertar in lstDetallesNotaTaller) {
                    detalleNotaTallerAInsertar.CantidadSurtida = detalleNotaTallerAInsertar.CantidadReal;
                    detalleNotaTallerAInsertar.Cantidad = detalleNotaTallerAInsertar.CantidadReal;
                    detalleNotaTallerAInsertar.CantidadDevuelta = 0;
                    detalleNotaTallerAInsertar.PorcentajeImpuesto = porcentajeIVA;
                    nuevaNotaTaller.Add(detalleNotaTallerAInsertar);
                }
                #endregion Preparar nota de taller a insertar

                #region Apertura de Transacción
                dataContext.OpenConnection(firmaConexion);
                dataContext.BeginTransaction(firmaTransaccion);
                #endregion Apertura de Transacción

                #region Insertar nueva nota de taller
                if (!notaTallerBR.Insertar(dataContext, nuevaNotaTaller, firma)) {
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error al insertar la nueva nota de taller!!!");
                }
                #endregion Insertar nueva nota de taller

                #region Reservar piezas en el almacen
                nuevaNotaTaller.Id = notaTallerBR.UltimoIdGenerado;
                foreach (DetalleNotaTallerBO detalleNotaTallerAActualizar in nuevaNotaTaller.GetChildren()) {
                    detalleNotaTallerAActualizar.CantidadReservada = detalleNotaTallerAActualizar.CantidadReal;
                }
                if (!notaTallerBR.Actualizar(dataContext, nuevaNotaTaller, firma)) {
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error al reservar las piezas de la nueva nota de taller!!!");
                }
                #endregion Reservar piezas en el almacen

                #region Cierre de Transacción
                dataContext.CommitTransaction(firmaTransaccion);
                #endregion Cierre de Transacción
            } catch {
                dataContext.RollbackTransaction(firmaTransaccion);
                throw;
            } finally {
                dataContext.CloseConnection(firmaConexion);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
        }
    }
}
