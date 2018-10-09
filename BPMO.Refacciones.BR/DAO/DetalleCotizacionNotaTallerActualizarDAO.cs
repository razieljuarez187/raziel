using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a datos para actualizar un Detalle Cotización Nota de Taller
    /// </summary>
    internal class DetalleCotizacionNotaTallerActualizarDAO : IDAOBaseActualizarDetalleDocumento {
        #region Atributos
        private int registrosAfectados;
        #endregion

        #region Constructores
        #endregion

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Actualizar DetalleCotizacionNotaTaller
        /// </summary>
        /// <param name="dataContext">Acceso a base de datos</param>
        /// <param name="documentoBase">CotizacionNotaTaller a actualizar</param>
        /// <param name="detalleDocumentoBase">DetalleCotizacionNotaTaller a actualizar</param>
        /// <returns>Indica si la actualización fue exitosa</returns>
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase) {
            #region Parámetros
            CotizacionNotaTallerBO cotizacionNotaTaller = null;
            DetalleCotizacionNotaTallerBO detalleCotizacionNotaTaller = null;
            if (documentoBase is CotizacionNotaTallerBO)
                cotizacionNotaTaller = (CotizacionNotaTallerBO)documentoBase;
            if (detalleDocumentoBase is DetalleCotizacionNotaTallerBO)
                detalleCotizacionNotaTaller = (DetalleCotizacionNotaTallerBO)detalleDocumentoBase;
            string mensajeError = String.Empty;
            if (cotizacionNotaTaller == null)
                mensajeError += " , CotizacionNotaTaller";
            if (detalleCotizacionNotaTaller == null)
                mensajeError += " , DetalleCotizacionNotaTaller";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (detalleCotizacionNotaTaller.Articulo == null || detalleCotizacionNotaTaller.Articulo.Id == null)
                mensajeError += " , DetalleCotizacionNotaTaller.Articulo.Id";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            #endregion

            #region Conexión a BD
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                manejadorDC.RegresaProveedorInicial(dataContext);
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" UPDATE inv_detCotizacionNT SET ");

            sValue.Append(" , CantidadSolicitada = @DetalleCotizacionNotaTaller_CantidadSolicitada");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CantidadSolicitada";
            sqlParam.Value = detalleCotizacionNotaTaller.CantidadSolicitada;
            sqlParam.DbType = DbType.Double;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(" , CantidadSurtida = @DetalleCotizacionNotaTaller_CantidadSurtida");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CantidadSurtida";
            sqlParam.Value = detalleCotizacionNotaTaller.CantidadSurtida;
            sqlParam.DbType = DbType.Double;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(" , Costo = @DetalleCotizacionNotaTaller_Costo");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_Costo";
            sqlParam.Value = detalleCotizacionNotaTaller.CostoUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(" , Precio = @DetalleCotizacionNotaTaller_Precio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_Precio";
            sqlParam.Value = detalleCotizacionNotaTaller.PrecioUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            if (detalleCotizacionNotaTaller.ArticuloCore != null && detalleCotizacionNotaTaller.ArticuloCore.Id != null) {
                sValue.Append(" , CoreId = @DetalleCotizacionNotaTaller_CoreId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CoreId";
                sqlParam.Value = detalleCotizacionNotaTaller.ArticuloCore.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            sValue.Append(" , CostoCore = @DetalleCotizacionNotaTaller_CostoCore");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CostoCore";
            sqlParam.Value = detalleCotizacionNotaTaller.CostoArticuloCore;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(" , PrecioCore = @DetalleCotizacionNotaTaller_PrecioCore");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_PrecioCore";
            sqlParam.Value = detalleCotizacionNotaTaller.PrecioArticuloCore;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            sValue.Append(" WHERE CotizacionNTID = @CotizacionNotaTaller_ID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_ID";
            sqlParam.Value = cotizacionNotaTaller.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(" AND ArticuloId = @DetalleCotizacionNotaTaller_ArticuloId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_ArticuloId";
            sqlParam.Value = detalleCotizacionNotaTaller.Articulo.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            int iRes = 0;
            string sValores = sValue.ToString().Trim();
            if (sValores.Length > 0) {
                if (sValores.StartsWith(","))
                    sValores = sValores.Substring(2);
            }
            sCmd.Append(sValores);
            #endregion

            #region Ejecución de Sentencia SQL
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            registrosAfectados = iRes;
            if (iRes < 1)
                throw new Exception("Hubo un error al actualizar el registro o fue modificado mientras era editado. ");
            else
                return true;
            #endregion
        }
        #endregion
    }
}
