using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DAO;
using BPMO.Primitivos.Utilerias;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para el manejo de movimientos de refacciones del sistema Líder
    /// </summary>
    public class MovimientoRefaccionBR : IBRBaseDocumento {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Metodos
        /// <summary>
        /// Crea un registro de movimiento de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Movimiento de refacciones que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firmaConexion = Guid.NewGuid();
            Guid firmaTransaccion = Guid.NewGuid();
            try {
                #region Validación de parámetros
                string mensajeError = String.Empty;
                if (dataContext == null)
                    mensajeError += " , DataContext";
                if (documentoBase == null || !(documentoBase is MovimientoRefaccionBO))
                    mensajeError += " , MovimientoRefaccion";
                if (mensajeError.Length > 0)
                    throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes parámetros no pueden ser nulos!!!");
                if (documentoBase.GetChildren().Count < 1)
                    throw new Exception("El movimiento debe tener cuando menos un detalle!!!");
                #endregion Validación de parámetros

                #region Apertura de transacción
                dataContext.OpenConnection(firmaConexion);
                dataContext.BeginTransaction(firmaTransaccion);
                #endregion Apertura de transacción

                MovimientoRefaccionInsertarDAO insertarDAO = new MovimientoRefaccionInsertarDAO();
                bool esExito = insertarDAO.Insertar(dataContext, documentoBase);
                registrosAfectados = insertarDAO.RegistrosAfectados;
                ultimoIdGenerado = insertarDAO.UltimoIdGenerado;
                documentoBase.Id = ultimoIdGenerado;
                bool esExitoDetalle = false;
                if (esExito) {
                    DetalleMovimientoRefaccionInsertarDAO insertarDetalleDAO = new DetalleMovimientoRefaccionInsertarDAO();
                    foreach (DetalleDocumentoBaseBO detalleDocumento in documentoBase.GetChildren()) {
                        esExitoDetalle = insertarDetalleDAO.Insertar(dataContext, documentoBase, detalleDocumento);
                        if (!esExitoDetalle) {
                            dataContext.RollbackTransaction(firmaTransaccion);
                            throw new Exception("Ocurrió un error desconocido al insertar el detalle del movimiento de refacciones!!!");
                        }
                    }
                } else {
                    dataContext.RollbackTransaction(firmaTransaccion);
                    throw new Exception("Ocurrió un error desconocido al insertar el movimiento de refacciones!!!");
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
        /// Actualiza un registro de movimiento de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Movimiento de refacciones que se desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Elimina un registro de movimiento de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de refacciones que se desea eliminar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, DocumentoBaseBO documentoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de movimientos de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de refacciones que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los movimientos de refacciones recuperados por la consulta</returns>
        public List<DocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de movimientos de refacciones hasta su primer nivel de asociación y composición
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Movimiento de refacciones que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los movimientos de refacciones y sus relaciones a primer nivel, recuperados por la consulta</returns>
        public List<DocumentoBaseBO> ConsultarCompleto(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            throw new NotImplementedException();
        }
        #endregion Metodos
    }
}
