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
    /// Acceso a datos para consultar la Cotización Nota de Taller
    /// </summary>
    internal class CotizacionNotaTallerConsultarDAO : IDAOBaseConsultarDocumento {
        /// <summary>
        /// Consultar CotizacionNotaTaller
        /// </summary>
        /// <param name="dataContext">Acceso a base de datos</param>
        /// <param name="documentoBase">CotizacionNotaTaller a consultar</param>
        /// <returns>Listado de CotizacionNotaTaller</returns>
        public List<DocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            #region Validar Parámetros
            string msjError = string.Empty;
            if (dataContext == null)
                msjError += " , dataContext";
            if (documentoBase == null)
                msjError += " , CotizacionNotaTaller";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            CotizacionNotaTallerBO cotizacionNotaTaller = null;
            if (documentoBase is CotizacionNotaTallerBO)
                cotizacionNotaTaller = (CotizacionNotaTallerBO)documentoBase;           
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
            sCmd.Append(" SELECT en.CotizacionNTID AS ID, en.EmpresaId AS EmpresaLiderID, uo.OracleUO AS UONombreCorto, en.SucursalId AS SucursalLiderID,");
            sCmd.Append(" LTRIM(RTRIM(su.NombreCorto)) AS SucursalNombreCorto, en.AlmacenId AS AlmacenID, en.AreaId, en.TecnicoID AS TecnicoLiderID, en.UsuarioId,");
            sCmd.Append(" en.ClienteID AS clienteLiderID, en.MonedaID AS MonedaID, mo.Abreviatura AS MonedaDestinoCodigo, en.CotizacionOrdenID AS CotizacionOrdenID, en.OrdenID AS OrdenID,");
            sCmd.Append(" en.StatusID AS EstatusID, en.Observaciones AS Observaciones, en.FechaAutoriza, en.FechaAplica, en.FechaRechaza, en.FechaCaduca, en.NotaTallerId AS NotaTallerID");
            sCmd.Append(" FROM inv_encCotizacionNT en INNER JOIN grl_catSucursales su ON su.SucursalID = en.SucursalId");
            sCmd.Append(" INNER JOIN grl_catOracleUO uo ON uo.OracleUOID = su.OracleUOID INNER JOIN tes_catmonedas mo ON mo.MonedaID = en.MonedaID");
            StringBuilder sWhere = new StringBuilder();

            #region CotizacionNotaTaller
            if (cotizacionNotaTaller.Id != null) {
                sWhere.Append(" en.CotizacionNTID = @cotizacionNotaTaller_ID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "cotizacionNotaTaller_ID";
                sqlParam.Value = cotizacionNotaTaller.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.Adscripcion != null) {
                if (cotizacionNotaTaller.Adscripcion.UnidadOperativa != null && !string.IsNullOrEmpty(cotizacionNotaTaller.Adscripcion.UnidadOperativa.NombreCorto)) {
                    sWhere.Append(" AND uo.OracleUO = @cotizacionNotaTaller_UONombreCorto");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "cotizacionNotaTaller_UONombreCorto";
                    sqlParam.Value = cotizacionNotaTaller.Adscripcion.UnidadOperativa.NombreCorto;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (cotizacionNotaTaller.Adscripcion.Sucursal != null && !string.IsNullOrEmpty(cotizacionNotaTaller.Adscripcion.Sucursal.NombreCorto)) {
                    sWhere.Append(" AND su.NombreCorto = @cotizacionNotaTaller_SucursalNombreCorto");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "cotizacionNotaTaller_SucursalNombreCorto";
                    sqlParam.Value = cotizacionNotaTaller.Adscripcion.Sucursal.NombreCorto;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (cotizacionNotaTaller.OrdenServicioId != null) {
                sWhere.Append(" AND en.OrdenId = @cotizacionNotaTaller_OrdenServicioID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "cotizacionNotaTaller_OrdenServicioID";
                sqlParam.Value = cotizacionNotaTaller.OrdenServicioId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.CotizacionOrdenId != null) {
                sWhere.Append(" AND en.CotizacionOrdenID = @cotizacionNotaTaller_CotizacionOrdenID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "cotizacionNotaTaller_CotizacionOrdenID";
                sqlParam.Value = cotizacionNotaTaller.CotizacionOrdenId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.StatusId != null) {
                sWhere.Append(" AND en.StatusID = @cotizacionNotaTaller_StatusID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "cotizacionNotaTaller_StatusID";
                sqlParam.Value = cotizacionNotaTaller.StatusId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            #endregion NotaTaller

            #region Auditoria
            if (cotizacionNotaTaller.Auditoria != null) {
                if (cotizacionNotaTaller.Auditoria.FC != null) {
                    sWhere.Append(" AND en.FCaptura LIKE @cotizacionNotaTaller_FC");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "cotizacionNotaTaller_FC";
                    sqlParam.Value = cotizacionNotaTaller.Auditoria.FC;
                    sqlParam.DbType = DbType.DateTime;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (cotizacionNotaTaller.Auditoria.UC != null) {
                    sWhere.Append(" AND en.UC LIKE @cotizacionNotaTaller_UC");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "cotizacionNotaTaller_UC";
                    sqlParam.Value = cotizacionNotaTaller.Auditoria.UC;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (cotizacionNotaTaller.Auditoria.UUA != null) {
                    sWhere.Append(" AND UA LIKE @facturaLider_UA");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "facturaLider_UA";
                    sqlParam.Value = cotizacionNotaTaller.Auditoria.UUA;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (cotizacionNotaTaller.Auditoria.FUA != null) {
                    sWhere.Append(" AND FA LIKE @facturaLider_FA");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "facturaLider_FA";
                    sqlParam.Value = cotizacionNotaTaller.Auditoria.FUA;
                    sqlParam.DbType = DbType.DateTime;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            #endregion Auditoria

            string where = sWhere.ToString().Trim();
            if (where.Length > 0) {
                if (where.StartsWith("AND "))
                    where = where.Substring(4);
                sCmd.Append(" WHERE " + where);
            }
            #endregion

            #region Ejecución Sentecia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "CotizacionNotaTaller");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<DocumentoBaseBO> lstDocumento = new List<DocumentoBaseBO>();
            CotizacionNotaTallerBO cotizacionNotaTallerLider;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                cotizacionNotaTallerLider = new CotizacionNotaTallerBO();
                cotizacionNotaTallerLider.Auditoria = new AuditoriaBO();
                cotizacionNotaTallerLider.Adscripcion = new AdscripcionBO();
                cotizacionNotaTallerLider.Adscripcion.UnidadOperativa = new UnidadOperativaBO();
                cotizacionNotaTallerLider.Adscripcion.Sucursal = new SucursalBO();
                cotizacionNotaTallerLider.Divisa = new DivisaBO();
                cotizacionNotaTallerLider.Divisa.MonedaDestino = new MonedaBO();
                #endregion Inicializar BO
                #region CotizacionNotaTaller
                if (!row.IsNull("ID"))
                    cotizacionNotaTallerLider.Id = (int)Convert.ChangeType(row["ID"], typeof(int));
                if (!row.IsNull("EmpresaLiderID"))
                    cotizacionNotaTallerLider.EmpresaLiderId = (int)Convert.ChangeType(row["EmpresaLiderID"], typeof(int));
                if (!row.IsNull("UONombreCorto"))
                    cotizacionNotaTallerLider.Adscripcion.UnidadOperativa.NombreCorto = (string)Convert.ChangeType(row["UONombreCorto"], typeof(string));
                if (!row.IsNull("SucursalLiderID"))
                    cotizacionNotaTallerLider.SucursalLiderId = (int)Convert.ChangeType(row["SucursalLiderID"], typeof(int));
                if (!row.IsNull("SucursalNombreCorto"))
                    cotizacionNotaTallerLider.Adscripcion.Sucursal.NombreCorto = (string)Convert.ChangeType(row["SucursalNombreCorto"], typeof(string));
                if (!row.IsNull("AlmacenID"))
                    cotizacionNotaTallerLider.AlmacenLiderId = (int)Convert.ChangeType(row["AlmacenID"], typeof(int));
                if (!row.IsNull("AreaId"))
                    cotizacionNotaTallerLider.AreaId = (int)Convert.ChangeType(row["AreaId"], typeof(int));
                if (!row.IsNull("TecnicoLiderID"))
                    cotizacionNotaTallerLider.MecanicoId = (int)Convert.ChangeType(row["TecnicoLiderID"], typeof(int));
                if (!row.IsNull("UsuarioId"))
                    cotizacionNotaTallerLider.UsuarioLiderId = (int)Convert.ChangeType(row["UsuarioId"], typeof(int));
                if (!row.IsNull("clienteLiderID"))
                    cotizacionNotaTallerLider.ClienteLiderId = (int)Convert.ChangeType(row["clienteLiderID"], typeof(int));
                if (!row.IsNull("MonedaID"))
                    cotizacionNotaTallerLider.MonedaId = (int)Convert.ChangeType(row["MonedaID"], typeof(int));
                if (!row.IsNull("MonedaDestinoCodigo"))
                    cotizacionNotaTallerLider.Divisa.MonedaDestino.Codigo = (string)Convert.ChangeType(row["MonedaDestinoCodigo"], typeof(string));
                if (!row.IsNull("CotizacionOrdenID"))
                    cotizacionNotaTallerLider.CotizacionOrdenId = (int)Convert.ChangeType(row["CotizacionOrdenID"], typeof(int));
                if (!row.IsNull("OrdenID"))
                    cotizacionNotaTallerLider.OrdenServicioId = (int)Convert.ChangeType(row["OrdenID"], typeof(int));
                if (!row.IsNull("EstatusID"))
                    cotizacionNotaTallerLider.StatusId = (int)Convert.ChangeType(row["EstatusID"], typeof(int));
                if (!row.IsNull("Observaciones"))
                    cotizacionNotaTallerLider.Observaciones = (string)Convert.ChangeType(row["Observaciones"], typeof(string));
                if (!row.IsNull("FechaAutoriza"))
                    cotizacionNotaTallerLider.FechaAutoriza = (DateTime)Convert.ChangeType(row["FechaAutoriza"], typeof(DateTime));
                if (!row.IsNull("FechaAplica"))
                    cotizacionNotaTallerLider.FechaAplica = (DateTime)Convert.ChangeType(row["FechaAplica"], typeof(DateTime));
                if (!row.IsNull("FechaRechaza"))
                    cotizacionNotaTallerLider.FechaRechaza = (DateTime)Convert.ChangeType(row["FechaRechaza"], typeof(DateTime));
                if (!row.IsNull("FechaCaduca"))
                    cotizacionNotaTallerLider.FechaCaduca = (DateTime)Convert.ChangeType(row["FechaCaduca"], typeof(DateTime));
                if (!row.IsNull("NotaTallerID"))
                    cotizacionNotaTallerLider.NotaTallerId = (int)Convert.ChangeType(row["NotaTallerID"], typeof(int));
                #endregion NotaTaller
                lstDocumento.Add(cotizacionNotaTallerLider);
            }
            return lstDocumento;
            #endregion
        }
    }
}
