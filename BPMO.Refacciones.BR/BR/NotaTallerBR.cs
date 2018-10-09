using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DAO;
using BPMO.Security.BR;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para el manejo de notas de taller del sistema Líder
    /// </summary>
    public class NotaTallerBR : IBRBaseDocumento {
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

        #region Metodos
        /// <summary>
        /// Crea un registro de nota de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
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
                if (documentoBase == null || !(documentoBase is NotaTallerBO))
                    mensajeError += " , NotaTaller";
                if (mensajeError.Length > 0)
                    throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes parámetros no pueden ser nulos!!!");
                if (documentoBase.GetChildren().Count < 1)
                    throw new Exception("La nota de taller debe tener cuando menos un detalle!!!");
                #endregion Validar parámetros

                #region Apertura de transacción
                dataContext.OpenConnection(firmaConexion);
                dataContext.BeginTransaction(firmaTransaccion);
                #endregion Apertura de transacción

                NotaTallerInsertarDAO insertarDAO = new NotaTallerInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, documentoBase);
                registrosAfectados = insertarDAO.RegistrosAfectados;
                ultimoIdGenerado = insertarDAO.UltimoIdGenerado;
                documentoBase.Id = ultimoIdGenerado;
                bool esExitoDetalle = false;
                if (esExito) {
                    DetalleNotaTallerBR detalleNotaTallerBR = new DetalleNotaTallerBR();
                    foreach (DetalleDocumentoBaseBO detalleDocumento in documentoBase.GetChildren()) {
                        esExitoDetalle = detalleNotaTallerBR.Insertar(dataContext, documentoBase, detalleDocumento, firma);
                        if (!esExitoDetalle) {
                            dataContext.RollbackTransaction(firmaTransaccion);
                            throw new Exception("Ocurrió un error al insertar el detalle de la nota de taller!!!");
                        }
                    }
                } else {
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error desconocido al insertar la nota de taller!!!");
                }

                #region Cierre de transacción
                dataContext.CommitTransaction(firmaTransaccion);
                if (esExito && esExitoDetalle)
                    return true;
                else
                    return false;
                #endregion Cierre de transacción
            } catch {
                dataContext.RollbackTransaction(firmaTransaccion);
                throw;
            } finally {
                dataContext.CloseConnection(firmaConexion);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
        }
        /// <summary>
        /// Actualiza un registro de nota de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller que se desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
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
                #endregion Apertura de conexión y transacción

                #region Modificación de los datos
                NotaTallerActualizarDAO actualizarDAO = new NotaTallerActualizarDAO();
                bool esExito = actualizarDAO.Actualizar(dataContext, documentoBase);
                bool esExitoDetalle = false;
                if (esExito) {
                    DetalleNotaTallerBR detalleActualizarDAO = new DetalleNotaTallerBR();
                    foreach (DetalleNotaTallerBO detalle in documentoBase.GetChildren()) {
                        esExitoDetalle = detalleActualizarDAO.Actualizar(dataContext, documentoBase, detalle,firma);
                        if (!esExitoDetalle) {
                            dataContext.RollbackTransaction(firmaTransaccion);
                            throw new Exception("Ocurrió un error al actualizar el detalle de la nota de taller!!!");
                        }
                    }
                }else{
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error al actualizar la nota de taller!!!");
                }
                #endregion Modificación de los datos

                #region Cierre de conexión y transacción
                dataContext.CommitTransaction(firmaTransaccion);
                return true;
                #endregion Cierre de conexión y transacción
            } catch {
                dataContext.RollbackTransaction(firmaTransaccion);
                throw;
            } finally {
                dataContext.CloseConnection(firmaConexion);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
        }
        /// <summary>
        /// Elimina un registro de nota de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Nota de taller que se desea eliminar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de notas de taller
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Nota de taller que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las notas de taller recuperados por la consulta</returns>
        public List<DocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            try {
                NotaTallerConsultarDAO consultarDAO = new NotaTallerConsultarDAO();
                return consultarDAO.Consultar(dataContext, documentoBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Consulta registros de notas de taller hasta su primer nivel de asociación y composición
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Nota de taller que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las notas de taller y sus relaciones a primer nivel, recuperados por la consulta</returns>
        public List<DocumentoBaseBO> ConsultarCompleto(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            try {
                List<DocumentoBaseBO> lstDocumentosBase = this.Consultar(dataContext, documentoBase);
                if (lstDocumentosBase.Count > 0) {
                    if (documentoBase.Id != null) {
                        DetalleNotaTallerBR detNotaTallerBR = new DetalleNotaTallerBR();
                        List<DetalleDocumentoBaseBO> lstDetalleNotaTaller = detNotaTallerBR.Consultar(dataContext, documentoBase);
                        foreach (DetalleDocumentoBaseBO detalleNotaTaller in lstDetalleNotaTaller) {
                            lstDocumentosBase[0].Add(detalleNotaTaller);
                        }
                    }
                }
                return lstDocumentosBase;
            } catch {
                throw;
            }
        }
        #endregion Metodos
    }
}
