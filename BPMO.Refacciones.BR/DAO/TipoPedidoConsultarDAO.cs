using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Consultar registros de TipoPedidoConsultar
    /// </summary>
    internal class TipoPedidoConsultarDAO : IDAOBaseConsultarCatalogo {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        public List<CatalogoBaseBO> Consultar(IDataContext dataContext, CatalogoBaseBO catalogoBase) {
            #region Validar parámetos
            TipoPedidoBO tipoPedido = null;
            if (catalogoBase is TipoPedidoBO)
                tipoPedido = (TipoPedidoBO)catalogoBase;
            string mensajeError = String.Empty;
            if (tipoPedido == null)
                mensajeError += " , TipoPedido";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2));
            #endregion Validar parámetros

            #region Conexión a BD
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                throw;
            }
            #endregion Conexión a BD

            #region Armado de Sentencia SQL
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT transferencia, TipoPedidoID, TipoPedido, Clave");
            sCmd.Append(" FROM ref_TiposPedido ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (tipoPedido.Id.HasValue) {
                sWhere.Append(" AND TipoPedidoID = @TipoPedido_ID");
                Utileria.AgregarParametro(sqlCmd, "TipoPedido_ID", tipoPedido.Id, System.Data.DbType.Int32);
            }
            if (!String.IsNullOrWhiteSpace(tipoPedido.Nombre)) {
                sWhere.Append(" AND TipoPedido LIKE @Tipo_Pedido");
                Utileria.AgregarParametro(sqlCmd, "Tipo_Pedido", tipoPedido.Nombre, System.Data.DbType.String);
            }
            if (!String.IsNullOrWhiteSpace(tipoPedido.NombreCorto)) {
                sWhere.Append(" AND Clave LIKE @_Clave");
                Utileria.AgregarParametro(sqlCmd, "_Clave", tipoPedido.Nombre, System.Data.DbType.String);
            }
            if (tipoPedido.AplicaTransferencia.HasValue) {
                sWhere.Append(" AND transferencia = @_Transferencia");
                Utileria.AgregarParametro(sqlCmd, "_Transferencia", tipoPedido.AplicaTransferencia, System.Data.DbType.Boolean);
            }
            if (tipoPedido.AplicaVenta.HasValue) {
                sWhere.Append(" AND Ventas = @_Ventas");
                Utileria.AgregarParametro(sqlCmd, "_Ventas", tipoPedido.AplicaVenta, System.Data.DbType.Boolean);
            }
            #endregion Valores

            string where = sWhere.ToString().Trim();
            if (where.Length > 0) {
                if (where.StartsWith("AND "))
                    where = where.Substring(4);
                sCmd.Append(" WHERE " + where);
            }
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "TipoPedido");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<CatalogoBaseBO> lstConfiguraciones = new List<CatalogoBaseBO>();
            TipoPedidoBO tipo_Pedido = null;

            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                tipo_Pedido = new TipoPedidoBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("TipoPedidoID"))
                    tipo_Pedido.Id = (Int32)Convert.ChangeType(row["TipoPedidoID"], typeof(Int32));
                if (!row.IsNull("transferencia"))
                    tipo_Pedido.AplicaTransferencia = (bool)Convert.ChangeType(row["transferencia"], typeof(bool));
                if (!row.IsNull("TipoPedido"))
                    tipo_Pedido.Nombre = (string)Convert.ChangeType(row["TipoPedido"], typeof(string));
                if (!row.IsNull("Clave"))
                    tipo_Pedido.NombreCorto = (string)Convert.ChangeType(row["Clave"], typeof(string));
                #endregion /ConfiguracionesReglas

                lstConfiguraciones.Add(tipo_Pedido);
            }
            return lstConfiguraciones;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos
    }
}
