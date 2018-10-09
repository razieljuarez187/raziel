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
    internal class DetalleNotaTallerConsultarDAO : IDAOBaseConsultarDetalleDocumento {
        public List<DetalleDocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            #region Validar parámetros
            NotaTallerBO notaTaller = null;
            if (documentoBase is NotaTallerBO)
                notaTaller = (NotaTallerBO)documentoBase;
            string msjError = string.Empty;
            if (notaTaller == null)
                msjError += " , NotaTaller";
            if (notaTaller.Id == null)
                msjError += " , NotaTaller.Id";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (dataContext == null)
                throw new ArgumentNullException("DataContext", "El parametro no puede ser nulo!!!");
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
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT dn.RenglonId AS Id, dn.ArticuloId AS ArticuloId, dn.CantSol AS Cantidad, dn.CantSurt AS CantidadSurtida, dn.CantCancel AS CantidadCancelada,");
            sCmd.Append(" dn.CantReserv AS CantidadReservada, dn.CantDevuelta AS CantidadDevuelta, dn.Costo AS CostoArticulo, dn.Precio AS PrecioArticulo, dn.CoreId AS ArticuloCoreId,");
            sCmd.Append(" dn.PrecioCore AS PrecioArticuloCore, dn.CostoCore AS CostoArticuloCore, dn.IVA AS PorcentajeImpuesto, dn.EmpReservaID AS EmpresaLiderReservaId,");
            sCmd.Append(" dn.SucReservaId AS SucursalLiderReservaId, dn.AlmReservaID AS AlmacenReservaId, dn.TipoRemision AS TipoRemision, dn.MonedaIdOri AS MonedaLiderOriginalId,");
            sCmd.Append(" dn.PrecioOri AS PrecioArticuloOriginal, dn.PrecioOriCore AS PrecioArticuloCoreOriginal");
            sCmd.Append(" FROM inv_detNotaTaller dn");
            StringBuilder sWhere = new StringBuilder();
            sWhere.Append(" dn.NotaTallerId = @NotaTaller_ID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_ID";
            sqlParam.Value = notaTaller.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            if (notaTaller.GetChildren().Count > 0) {
                DetalleNotaTallerBO detalleNotaTaller = (DetalleNotaTallerBO)notaTaller.GetChild(0);
                #region DetalleNotaTaller
                if (detalleNotaTaller.Id != null) {
                    sWhere.Append(" dn.RenglonId = @DetalleNotaTaller_ID");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "DetalleNotaTaller_ID";
                    sqlParam.Value = detalleNotaTaller.Id;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                #region Articulo
                if (detalleNotaTaller.Articulo != null) {
                    if (detalleNotaTaller.Articulo.Id != null) {
                        sWhere.Append(" dn.ArticuloId = @detalleNotaTaller_ArticuloID");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "detalleNotaTaller_ArticuloID";
                        sqlParam.Value = detalleNotaTaller.Articulo.Id;
                        sqlParam.DbType = DbType.Int32;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                }
                #endregion Articulo
                #region Core
                if (detalleNotaTaller.ArticuloCore != null) {
                    if (detalleNotaTaller.ArticuloCore.Id != null) {
                        sWhere.Append(" dn.CoreId = @detalleNotaTaller_CoreID");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "detalleNotaTaller_CoreID";
                        sqlParam.Value = detalleNotaTaller.ArticuloCore.Id;
                        sqlParam.DbType = DbType.Int32;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
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
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "DetalleNotaTaller");
            } catch (Exception ex) {
                throw ex;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<DetalleDocumentoBaseBO> lstDocumentos = new List<DetalleDocumentoBaseBO>();
            DetalleNotaTallerBO detalleNotaTallerResultado;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                detalleNotaTallerResultado = new DetalleNotaTallerBO();
                detalleNotaTallerResultado.Articulo = new ArticuloBO();
                detalleNotaTallerResultado.ArticuloCore = new ArticuloBO();
                detalleNotaTallerResultado.AlmacenReserva = new AlmacenBO();
                #endregion Inicializar BO

                #region DetalleNotaTaller
                if (!row.IsNull("Id"))
                    detalleNotaTallerResultado.Id = (int)Convert.ChangeType(row["Id"], typeof(int));
                if (!row.IsNull("Cantidad"))
                    detalleNotaTallerResultado.Cantidad = (int)Convert.ChangeType(row["Cantidad"], typeof(int));
                if (!row.IsNull("CantidadSurtida"))
                    detalleNotaTallerResultado.CantidadSurtida = (int)Convert.ChangeType(row["CantidadSurtida"], typeof(int));
                if (!row.IsNull("CantidadCancelada"))
                    detalleNotaTallerResultado.CantidadCancelada = (int)Convert.ChangeType(row["CantidadCancelada"], typeof(int));
                if (!row.IsNull("CantidadReservada"))
                    detalleNotaTallerResultado.CantidadReservada = (int)Convert.ChangeType(row["CantidadReservada"], typeof(int));
                if (!row.IsNull("CantidadDevuelta"))
                    detalleNotaTallerResultado.CantidadDevuelta = (int)Convert.ChangeType(row["CantidadDevuelta"], typeof(int));
                if (!row.IsNull("CostoArticulo"))
                    detalleNotaTallerResultado.CostoUnitario = (decimal)Convert.ChangeType(row["CostoArticulo"], typeof(decimal));
                if (!row.IsNull("PrecioArticulo"))
                    detalleNotaTallerResultado.PrecioUnitario = (decimal)Convert.ChangeType(row["PrecioArticulo"], typeof(decimal));
                if (!row.IsNull("PrecioArticuloCore"))
                    detalleNotaTallerResultado.PrecioArticuloCore = (decimal)Convert.ChangeType(row["PrecioArticuloCore"], typeof(decimal));
                if (!row.IsNull("CostoArticuloCore"))
                    detalleNotaTallerResultado.CostoArticuloCore = (decimal)Convert.ChangeType(row["CostoArticuloCore"], typeof(decimal));
                if (!row.IsNull("PorcentajeImpuesto"))
                    detalleNotaTallerResultado.PorcentajeImpuesto = (decimal)Convert.ChangeType(row["PorcentajeImpuesto"], typeof(decimal));
                if (!row.IsNull("EmpresaLiderReservaId"))
                    detalleNotaTallerResultado.EmpresaLiderReservaId = (int)Convert.ChangeType(row["EmpresaLiderReservaId"], typeof(int));
                if (!row.IsNull("SucursalLiderReservaId"))
                    detalleNotaTallerResultado.SucursalLiderReservaId = (int)Convert.ChangeType(row["SucursalLiderReservaId"], typeof(int));
                if (!row.IsNull("AlmacenReservaId"))
                    detalleNotaTallerResultado.AlmacenReserva.Id = (int)Convert.ChangeType(row["AlmacenReservaId"], typeof(int));
                if (!row.IsNull("TipoRemision"))
                    detalleNotaTallerResultado.TipoRemisionId = (int)Convert.ChangeType(row["TipoRemision"], typeof(int));
                if (!row.IsNull("MonedaLiderOriginalId"))
                    detalleNotaTallerResultado.MonedaLiderOriginalId = (int)Convert.ChangeType(row["MonedaLiderOriginalId"], typeof(int));
                if (!row.IsNull("PrecioArticuloOriginal"))
                    detalleNotaTallerResultado.PrecioArticuloOriginal = (decimal)Convert.ChangeType(row["PrecioArticuloOriginal"], typeof(decimal));
                if (!row.IsNull("PrecioArticuloCoreOriginal"))
                    detalleNotaTallerResultado.PrecioArticuloCoreOriginal = (decimal)Convert.ChangeType(row["PrecioArticuloCoreOriginal"], typeof(decimal));
                #endregion DetalleNotaTaller

                #region Articulo
                if (!row.IsNull("ArticuloId"))
                    detalleNotaTallerResultado.Articulo.Id = (int)Convert.ChangeType(row["ArticuloId"], typeof(int));
                if (!row.IsNull("ArticuloCoreId"))
                    detalleNotaTallerResultado.ArticuloCore.Id = (int)Convert.ChangeType(row["ArticuloCoreId"], typeof(int));
                #endregion Articulo

                lstDocumentos.Add(detalleNotaTallerResultado);
            }
            return lstDocumentos;
            #endregion
        }
    }
}
