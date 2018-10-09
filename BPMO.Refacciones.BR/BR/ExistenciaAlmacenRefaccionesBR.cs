using System.Collections.Generic;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DA;

namespace BPMO.Refacciones.BR {
    /// <summary>
    /// Servicios para el manejo de existencias de almacenes de refacciones del sistema Líder
    /// </summary>
    public class ExistenciaAlmacenRefaccionesBR : IBRBaseAuditoria {
        #region Atributos
        private int registrosAfectados;
        private int ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Métodos
        /// <summary>
        /// Crear un registro de existencia de almacén de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Existencia de almacén de refacciones que se desea crear</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Insertar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Actualiza un registro de existencia de almacén de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Existencia de almacén de refacciones que se desea actualizar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Actualizar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Elimina un registro de existencia de almacén de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Existencia de almacén de refacciones que se desea eliminar</param>
        /// <param name="firma">Clase para el manejo de seguridad</param>
        /// <returns>Verdadero si la operación se realizó con éxito; falso en caso contrario</returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, SeguridadBO firma) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Consulta registros de existencias de almacén de refacciones
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Existencia de almacén que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las existencias de almacén de refacciones recuperados por la consulta</returns>
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            try {
                ExistenciaAlmacenRefaccionesConsultarDAO consultarDAO = new ExistenciaAlmacenRefaccionesConsultarDAO();
                return consultarDAO.Consultar(dataContext, auditoriaBase);
            } catch {
                throw;
            }
        }
        /// <summary>
        /// Consulta registros de existencias de almacén de refacciones hasta su primer nivel de asociación y composición
        /// </summary>
        /// <param name="dataContext">DataContext que proveerá acceso a la base de datos</param>
        /// <param name="auditoriaBase">Existencia de almacén que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Lista que contiene la información de las existencias de almacén de refacciones y sus relaciones a primer nivel, recuperados por la consulta</returns>
        public List<AuditoriaBaseBO> ConsultarCompleto(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Obtiene el precio de la refacción usando la matriz de promociones y descuentos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="refaccion">Objeto que contiene los datos del artículo</param>
        /// <param name="cliente">Cliente al que se le venderá el artículo</param>
        /// <param name="tipoCliente">Tipo de cliente</param>
        /// <param name="direccion">Dirección del cliente</param>
        /// <param name="tipoPedido">Tipo de pedido: 
        ///   1 = NORMAL, 2 = MOSTRADOR, 3 = PROGRAMADO, 5 = EMERGENCIA, 6 = STOCK, 8 = CONSIGNA, 9 = CONSIGNA A FACTURAR, 10 = ESPECIAL,
        ///   11 = ENVIO DIRECTO, 12 = PRIORITARIO, 13 = FLEETCHARGE, 19 = WILL CALL, 20 = PEDIDOS INTERNET, 21 = TRANSFER PARTS </param>
        /// <param name="esContado">Indica si la venta es de contado</param>
        /// <returns></returns>
        public decimal ObtenerPrecio(IDataContext dataContext, ExistenciaAlmacenRefaccionesBO refaccion, ClienteBO cliente, TipoClienteBO tipoCliente,
            DireccionClienteBO direccion, int tipoPedido = 1, bool esContado = true) {
            try {
                ObtenerPrecioRefaccionActualDA precioDA = new ObtenerPrecioRefaccionActualDA();
                return precioDA.Consultar(dataContext, refaccion, cliente, tipoCliente, direccion, tipoPedido, esContado);
            } catch {
                throw;
            }
        }
        #endregion Métodos
    }
}
