using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a datos para insertar DetalleCotizacionNotaTaller
    /// </summary>
    internal class DetalleCotizacionNotaTallerInsertarDAO : IDAOBaseInsertarDetalleDocumento {
        #region Atributos
        private int registrosAfectados;
        #endregion

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion
        /// <summary>
        /// Registrar DetalleCotizacionNotaTaller
        /// </summary>
        /// <param name="dataContext">Acceso a base de datos</param>
        /// <param name="documentoBase">CotizacionNotaTaller a insertar</param>
        /// <param name="detalleDocumentoBase">DetalleCotizacionNotaTaller a insertar</param>
        /// <returns>Indica si el registro fue exitoso</returns>
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase, DetalleDocumentoBaseBO detalleDocumentoBase) {
            #region Validar Parámetros
            CotizacionNotaTallerBO cotizacionNotaTaller = null;
            DetalleCotizacionNotaTallerBO detalleCotizacionNotaTaller = null;
            if (documentoBase is CotizacionNotaTallerBO)
                cotizacionNotaTaller = (CotizacionNotaTallerBO)documentoBase;
            if (detalleDocumentoBase is DetalleCotizacionNotaTallerBO)
                detalleCotizacionNotaTaller = (DetalleCotizacionNotaTallerBO)detalleDocumentoBase;
            string mensajeError = string.Empty;
            if (cotizacionNotaTaller == null)
                mensajeError += " , CotizacionNotaTaller";
            if (detalleCotizacionNotaTaller == null)
                mensajeError += " , DetalleCotizacionNotaTaller";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (cotizacionNotaTaller.EmpresaLiderId == null)
                mensajeError += " , CotizacionNotaTaller.EmpresaId";
            if (cotizacionNotaTaller.SucursalLiderId == null)
                mensajeError += " , CotizacionNotaTaller.SucursalId";
            if (cotizacionNotaTaller.Id == null)
                mensajeError += " , CotizacionNotaTaller.Id";
            if (cotizacionNotaTaller.AlmacenLiderId == null)
                mensajeError += " , CotizacionNotaTaller.AlmacenId";
            if (cotizacionNotaTaller.Id == null)
                mensajeError += " , DetalleCotizacionNotaTaller.CotizacionNotaTallerId";
            if (detalleCotizacionNotaTaller.Articulo == null || detalleCotizacionNotaTaller.Articulo.Id == null)
                mensajeError += " , DetalleCotizacionNotaTaller.Articulo.Id";
            if (detalleCotizacionNotaTaller.CantidadSolicitada == null)
                mensajeError += " , DetalleCotizacionNotaTaller.CantidadSolicitada";
            if (detalleCotizacionNotaTaller.CantidadSurtida == null)
                mensajeError += " , DetalleCotizacionNotaTaller.CantidadSurtida";
            if (detalleCotizacionNotaTaller.CostoUnitario == null)
                mensajeError += " , DetalleCotizacionNotaTaller.CostoUnitario";
            if (detalleCotizacionNotaTaller.PrecioUnitario == null)
                mensajeError += " , DetalleCotizacionNotaTaller.PrecioUnitario";
            if (detalleCotizacionNotaTaller.CostoArticuloCore == null)
                mensajeError += " , DetalleCotizacionNotaTaller.CostoCore";
            if (detalleCotizacionNotaTaller.PrecioArticuloCore == null)
                mensajeError += " , DetalleCotizacionNotaTaller.PrecioCore";
            if (detalleCotizacionNotaTaller.PorcentajeImpuesto == null)
                mensajeError += " , DetalleCotizacionNotaTaller.Iva";
            if (detalleCotizacionNotaTaller.EmpresaLiderReservaId == null)
                mensajeError += " , DetalleCotizacionNotaTaller.EmpresaId";
            if (detalleCotizacionNotaTaller.SucursalLiderReservaId == null)
                mensajeError += " , DetalleCotizacionNotaTaller.SucursalId";
            if (detalleCotizacionNotaTaller.AlmacenId == null)
                mensajeError += " , DetalleCotizacionNotaTaller.AlmacenId";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            #endregion

            #region Conexión a BD
            if (!dataContext.CheckProviderByName("LIDER"))
                throw new ArgumentNullException("LIDER", "No se ha definido un proveedor de conexiones para la base de datos requerida!!!");
            string incomingDataContext = dataContext.CurrentProvider;
            if (dataContext.CurrentProvider != "LIDER")
                dataContext.SetCurrentProvider("LIDER");
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
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" INSERT inv_detCotizacionNT (EmpresaId, SucursalId, AlmacenId, CotizacionNTID, ArticuloID, CantidadSolicitada, CantidadSurtida,");
            sCmd.Append(" Costo, Precio, Iva, CoreID, CostoCore, PrecioCore)");
            sCmd.Append("  VALUES(");
            #region Valores
            sValue.Append(", @DetalleCotizacionNotaTaller_EmpresaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_EmpresaId";
            sqlParam.Value = cotizacionNotaTaller.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_SucursalId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_SucursalId";
            sqlParam.Value = cotizacionNotaTaller.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_AlmacenId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_AlmacenId";
            sqlParam.Value = cotizacionNotaTaller.AlmacenLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_Id");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_Id";
            sqlParam.Value = cotizacionNotaTaller.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_ArticuloId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_ArticuloId";
            sqlParam.Value = detalleCotizacionNotaTaller.Articulo.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_CantidadSolicitada");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CantidadSolicitada";
            sqlParam.Value = detalleCotizacionNotaTaller.CantidadSolicitada;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_CantidadSurtida");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CantidadSurtida";
            sqlParam.Value = detalleCotizacionNotaTaller.CantidadSurtida;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_Costo");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_Costo";
            sqlParam.Value = detalleCotizacionNotaTaller.CostoUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_Precio");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_Precio";
            sqlParam.Value = detalleCotizacionNotaTaller.PrecioUnitario;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_IVA");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_IVA";
            sqlParam.Value = detalleCotizacionNotaTaller.PorcentajeImpuesto;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            if (detalleCotizacionNotaTaller.ArticuloCore != null && detalleCotizacionNotaTaller.ArticuloCore.Id != null) {
                sValue.Append(", @DetalleCotizacionNotaTaller_CoreId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CoreId";
                sqlParam.Value = detalleCotizacionNotaTaller.ArticuloCore.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL ");
            }
            sValue.Append(", @DetalleCotizacionNotaTaller_CostoCore");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_CostoCore";
            sqlParam.Value = detalleCotizacionNotaTaller.CostoArticuloCore;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @DetalleCotizacionNotaTaller_PrecioCore");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "DetalleCotizacionNotaTaller_PrecioCore";
            sqlParam.Value = detalleCotizacionNotaTaller.PrecioArticuloCore;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion Valores
            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");
            #endregion

            #region Ejecución de Sentencia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                if (dataContext.CurrentProvider != incomingDataContext)
                    dataContext.SetCurrentProvider(incomingDataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Hubo un error al crear el registro.");
            else
                return true;
            #endregion
        }
    }
}
