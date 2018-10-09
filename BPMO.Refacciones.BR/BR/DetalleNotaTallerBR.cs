using System;
using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Security.BR;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para el manejo de detalles de notas de taller del sistema Líder
    /// </summary>
    internal class DetalleNotaTallerBR : IBRBaseDetalleDocumento {
        #region Atributos
        private int registrosAfectados;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion Propiedades

        #region Metodos
        /// <summary>
        /// Crea un registro de detalle de nota de taller del sistema Líder
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller a la que pertenece el detalle que se desea crear</param>
        /// <param name="detalleDocumentoBase">Detalle de nota de taller que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion

                DetalleNotaTallerInsertarDAO insertarDAO = new DetalleNotaTallerInsertarDAO();
                return insertarDAO.Insertar(dataContext, documentoBase, detalleDocumentoBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Actualiza un detalle de nota de taller del sistema Líder
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller a la que pertenece el detalle que se desea actualizar</param>
        /// <param name="detalleDocumentoBase">Detalle de nota de taller que se desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase, SeguridadBO firma) {
            try {
                #region Código de seguridad
                //Verifica si el usuario tiene permisos para ejecutar la siguiente operación
                SecurityBR seguridadBR = new SecurityBR(firma);
                firma = seguridadBR.ConsultarPermisos(dataContext);
                #endregion

                DetalleNotaTallerActualizarDAO actualizarDAO = new DetalleNotaTallerActualizarDAO();
                return actualizarDAO.Actualizar(dataContext, documentoBase, detalleDocumentoBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Elimina un detalle de nota de taller del sistema Líder
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller a la que pertenece el detalle que se desea eliminar</param>
        /// <param name="detalleDocumentoBase">Detalle de nota de taller que se desea eliminar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase, SeguridadBO firma) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Consulta detalles de notas de taller del sistema Líder
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los detalles de la nota de taller recuperados por la consulta</returns>
        public List<DetalleDocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            try {
                DetalleNotaTallerConsultarDAO consultarDAO = new DetalleNotaTallerConsultarDAO();
                return consultarDAO.Consultar(dataContext, documentoBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Consulta detalles de notas de taller del sistema Líder hasta su primer nivel de asociación y composición
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="documentoBase">Nota de taller que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de los detalles de la nota de taller y sus relaciones a primer nivel, recuperados por la consulta</returns>
        public List<DetalleDocumentoBaseBO> ConsultarCompleto(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            throw new NotImplementedException();
        }
        #endregion Metodos
    }
}
