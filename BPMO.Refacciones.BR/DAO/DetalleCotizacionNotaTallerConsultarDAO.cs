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
    /// Acceso a datos para consultar DetalleCotizacionNotaTaller
    /// </summary>
    internal class DetalleCotizacionNotaTallerConsultarDAO : IDAOBaseConsultarDetalleDocumento {
        /// <summary>
        /// Consultar DetalleCotizacionNotaTaller
        /// </summary>
        /// <param name="dataContext">Acceso a base de datos</param>
        /// <param name="documentoBase">CotizacionNotaTaller a consultar</param>
        /// <returns>Listado de DetalleCotizacionNotaTaller</returns>
        public List<DetalleDocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            #region Validar Parámetros
            CotizacionNotaTallerBO cotizacionNotaTaller = null;
            if (documentoBase is CotizacionNotaTallerBO)
                cotizacionNotaTaller = (CotizacionNotaTallerBO)documentoBase;
            string msjError = string.Empty;
            if (cotizacionNotaTaller == null)
                msjError += " , CotizacionNotaTaller";
            if (cotizacionNotaTaller.Id == null)
                msjError += " , CotizacionNotaTaller.Id";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (dataContext == null)
                throw new ArgumentNullException("DataContext", "El parámetro no puede ser nulo!!!");
            #endregion

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
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT dn.RenglonID AS Id, dn.ArticuloId AS ArticuloId, dn.CantidadSolicitada AS CantidadSolicitada, dn.CantidadSurtida AS CantidadSurtida,");
            sCmd.Append(" dn.Costo AS CostoArticulo, dn.Precio AS PrecioArticulo, dn.CoreID AS ArticuloCoreId,");
            sCmd.Append(" dn.PrecioCore AS PrecioArticuloCore, dn.CostoCore AS CostoArticuloCore, dn.IVA AS PorcentajeImpuesto, dn.EmpresaId AS EmpresaLiderReservaId,");
            sCmd.Append(" dn.SucursalId AS SucursalLiderReservaId, dn.AlmacenId AS AlmacenReservaId");
            sCmd.Append(" FROM inv_detCotizacionNT dn");
            StringBuilder sWhere = new StringBuilder();
            sWhere.Append(" dn.CotizacionNTID = @CotizacionNotaTaller_ID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_ID";
            sqlParam.Value = cotizacionNotaTaller.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            if (cotizacionNotaTaller.GetChildren().Count > 0) {
                DetalleCotizacionNotaTallerBO detalleCotizacionNotaTaller = (DetalleCotizacionNotaTallerBO)cotizacionNotaTaller.GetChild(0);
                #region DetalleNotaTaller
                if (detalleCotizacionNotaTaller.Id != null) {
                    sWhere.Append(" dn.RenglonID = @DetalleCotizacionNotaTaller_ID");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "DetalleCotizacionNotaTaller_ID";
                    sqlParam.Value = detalleCotizacionNotaTaller.Id;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                #region Articulo
                if (detalleCotizacionNotaTaller.Articulo != null && detalleCotizacionNotaTaller.Articulo.Id != null) {
                    sWhere.Append(" dn.ArticuloID = @detalleCotizacionNotaTaller_ArticuloID");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "detalleCotizacionNotaTaller_ArticuloID";
                    sqlParam.Value = detalleCotizacionNotaTaller.Articulo.Id;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                #endregion Articulo
                #region Core
                if (detalleCotizacionNotaTaller.ArticuloCore != null && detalleCotizacionNotaTaller.ArticuloCore.Id != null) {
                    sWhere.Append(" dn.CoreID = @detalleCotizacionNotaTaller_CoreID");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "detalleCotizacionNotaTaller_CoreID";
                    sqlParam.Value = detalleCotizacionNotaTaller.ArticuloCore.Id;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                #endregion Core
                #endregion DetalleFacturaLider
            }
            string where = sWhere.ToString().Trim();
            if (where.Length > 0) {
                if (where.StartsWith("AND "))
                    where = where.Substring(4);
                sCmd.Append(" WHERE " + where);
            }
            #endregion

            #region Ejecución de Sentencia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "DetalleCotizacionNotaTaller");
            } catch (Exception ex) {
                throw ex;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<DetalleDocumentoBaseBO> lstDocumentos = new List<DetalleDocumentoBaseBO>();
            DetalleCotizacionNotaTallerBO detalleCotizacionNotaTallerResultado;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                detalleCotizacionNotaTallerResultado = new DetalleCotizacionNotaTallerBO();
                detalleCotizacionNotaTallerResultado.Articulo = new ArticuloBO();
                detalleCotizacionNotaTallerResultado.ArticuloCore = new ArticuloBO();
                #endregion Inicializar BO

                #region DetalleCotizacionNotaTaller
                if (!row.IsNull("Id"))
                    detalleCotizacionNotaTallerResultado.Id = (int)Convert.ChangeType(row["Id"], typeof(int));
                if (!row.IsNull("ArticuloId"))
                    detalleCotizacionNotaTallerResultado.Articulo.Id = (int)Convert.ChangeType(row["ArticuloId"], typeof(int));
                if (!row.IsNull("CantidadSolicitada"))
                    detalleCotizacionNotaTallerResultado.CantidadSolicitada = (int)Convert.ChangeType(row["CantidadSolicitada"], typeof(int));
                if (!row.IsNull("CantidadSurtida"))
                    detalleCotizacionNotaTallerResultado.CantidadSurtida = (int)Convert.ChangeType(row["CantidadSurtida"], typeof(int));
                if (!row.IsNull("CostoArticulo"))
                    detalleCotizacionNotaTallerResultado.CostoUnitario = (decimal)Convert.ChangeType(row["CostoArticulo"], typeof(decimal));
                if (!row.IsNull("PrecioArticulo"))
                    detalleCotizacionNotaTallerResultado.PrecioUnitario = (decimal)Convert.ChangeType(row["PrecioArticulo"], typeof(decimal));
                if (!row.IsNull("ArticuloCoreId"))
                    detalleCotizacionNotaTallerResultado.ArticuloCore.Id = (int)Convert.ChangeType(row["ArticuloCoreId"], typeof(int));
                if (!row.IsNull("PrecioArticuloCore")) {
                    detalleCotizacionNotaTallerResultado.PrecioArticuloCore = (decimal)Convert.ChangeType(row["PrecioArticuloCore"], typeof(decimal));
                    detalleCotizacionNotaTallerResultado.PrecioArticuloCoreOriginal = (decimal)Convert.ChangeType(row["PrecioArticuloCore"], typeof(decimal));
                }
                if (!row.IsNull("CostoArticuloCore"))
                    detalleCotizacionNotaTallerResultado.CostoArticuloCore = (decimal)Convert.ChangeType(row["CostoArticuloCore"], typeof(decimal));
                if (!row.IsNull("PorcentajeImpuesto"))
                    detalleCotizacionNotaTallerResultado.PorcentajeImpuesto = (decimal)Convert.ChangeType(row["PorcentajeImpuesto"], typeof(decimal));
                if (!row.IsNull("EmpresaLiderReservaId"))
                    detalleCotizacionNotaTallerResultado.EmpresaLiderReservaId = (int)Convert.ChangeType(row["EmpresaLiderReservaId"], typeof(int));
                if (!row.IsNull("SucursalLiderReservaId"))
                    detalleCotizacionNotaTallerResultado.SucursalLiderReservaId = (int)Convert.ChangeType(row["SucursalLiderReservaId"], typeof(int));
                if (!row.IsNull("AlmacenReservaId"))
                    detalleCotizacionNotaTallerResultado.AlmacenId = (int)Convert.ChangeType(row["AlmacenReservaId"], typeof(int));
                #endregion DetalleNotaTaller

                lstDocumentos.Add(detalleCotizacionNotaTallerResultado);
            }
            return lstDocumentos;
            #endregion
        }
    }
}
