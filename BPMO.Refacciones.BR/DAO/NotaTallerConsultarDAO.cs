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
    internal class NotaTallerConsultarDAO : IDAOBaseConsultarDocumento {
        public List<DocumentoBaseBO> Consultar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            #region Validar parámetros
            NotaTallerBO notaTaller = null;
            if (documentoBase is NotaTallerBO)
                notaTaller = (NotaTallerBO)documentoBase;
            string msjError = string.Empty;
            if (notaTaller == null)
                msjError += " , NotaTaller";
            if (msjError.Length > 0)
                throw new ArgumentNullException("NotaTaller", "El parametro no puede ser nulo!!!");
            if (dataContext == null)
                throw new ArgumentNullException("DataConext", "El parametro no puede ser nulo!!!");
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
            sCmd.Append(" SELECT en.NotaTallerID AS ID, en.EmpresaId AS EmpresaLiderID, uo.OracleUO AS UONombreCorto, en.SucursalId AS SucursalLiderID,");
            sCmd.Append(" LTRIM(RTRIM(su.NombreCorto)) AS SucursalNombreCorto, en.AlmacenId AS AlmacenID, en.AreaId AS AreaLiderID, en.MecanicoId AS MecanicoLiderID,");
            sCmd.Append(" en.FCaptura AS FC, en.UsuarioId AS UsuarioLiderID, en.OrdenID AS OrdenServicioID, en.MovimientoId AS MovimientoID,");
            sCmd.Append(" en.Status AS EstatusID, en.Referencia AS NumeroReferencia, en.TipoRef AS TipoReferencia, en.ClienteId AS clienteLiderLiderID,");
            sCmd.Append(" en.DiasCredito AS DiasCredito, en.Observaciones AS Observaciones, en.Rescate AS EsRescate, en.TipoPedidoId AS TipoPedidoID,");
            sCmd.Append(" en.Devolucion AS EsDevolucion, en.NotaTallerIdref AS NotaTallerReferencia, en.Impreso AS EstaImpreso, en.KitID AS KitID, en.MonedaIdVta AS MonedaLiderID,");
            sCmd.Append(" mo.Abreviatura AS MonedaDestinoCodigo, en.FechaAplica AS FechaAplicacion");
            sCmd.Append(" FROM inv_encNotaTaller en INNER JOIN grl_catSucursales su ON su.SucursalID = en.SucursalID");
            sCmd.Append(" INNER JOIN grl_catOracleUO uo ON uo.OracleUOID = su.OracleUOID INNER JOIN tes_catmonedas mo ON mo.MonedaID = en.MonedaIdVta");
            StringBuilder sWhere = new StringBuilder();

            #region NotaTaller
            if (notaTaller.Id != null) {
                sWhere.Append(" en.NotaTallerID = @notaTaller_ID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notaTaller_ID";
                sqlParam.Value = notaTaller.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notaTaller.Adscripcion != null) {
                if (notaTaller.Adscripcion.UnidadOperativa != null) {
                    if (!String.IsNullOrEmpty(notaTaller.Adscripcion.UnidadOperativa.NombreCorto)) {
                        sWhere.Append(" AND uo.OracleUO = @notaTaller_UONombreCorto");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "notaTaller_UONombreCorto";
                        sqlParam.Value = notaTaller.Adscripcion.UnidadOperativa.NombreCorto;
                        sqlParam.DbType = DbType.String;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                }
                if (notaTaller.Adscripcion.Sucursal != null) {
                    if (!String.IsNullOrEmpty(notaTaller.Adscripcion.Sucursal.NombreCorto)) {
                        sWhere.Append(" AND su.NombreCorto = @notaTaller_SucursalNombreCorto");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "notaTaller_SucursalNombreCorto";
                        sqlParam.Value = notaTaller.Adscripcion.Sucursal.NombreCorto;
                        sqlParam.DbType = DbType.String;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                }
            }
            if (notaTaller.OrdenServicioId != null) {
                sWhere.Append(" AND en.OrdenId = @notaTaller_OrdenServicioID");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notaTaller_OrdenServicioID";
                sqlParam.Value = notaTaller.OrdenServicioId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notaTaller.EsDevolucion != null) {
                sWhere.Append(" AND en.Devolucion = @notaTaller_EsDevolucion");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notaTaller_EsDevolucion";
                sqlParam.Value = notaTaller.EsDevolucion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notaTaller.Estatus != null && notaTaller.Estatus.Id != null) {
                sWhere.Append(" AND en.Status = @notaTaller_Status");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notaTaller_Status";
                sqlParam.Value = notaTaller.Estatus.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            #endregion NotaTaller

            #region Auditoria
            if (notaTaller.Auditoria != null) {
                if (notaTaller.Auditoria.FC != null) {
                    sWhere.Append(" AND en.FCaptura LIKE @notaTaller_FC");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "notaTaller_FC";
                    sqlParam.Value = notaTaller.Auditoria.FC;
                    sqlParam.DbType = DbType.DateTime;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (notaTaller.Auditoria.UC != null) {
                    sWhere.Append(" AND en.UC LIKE @notaTaller_UC");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "notaTaller_UC";
                    sqlParam.Value = notaTaller.Auditoria.UC;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (notaTaller.Auditoria.UUA != null) {
                    sWhere.Append(" AND UA LIKE @facturaLider_UA");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "facturaLider_UA";
                    sqlParam.Value = notaTaller.Auditoria.UUA;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (notaTaller.Auditoria.FUA != null) {
                    sWhere.Append(" AND FA LIKE @facturaLider_FA");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "facturaLider_FA";
                    sqlParam.Value = notaTaller.Auditoria.FUA;
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
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "NotaTaller");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<DocumentoBaseBO> lstDocumento = new List<DocumentoBaseBO>();
            NotaTallerBO notaTallerLider;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                notaTallerLider = new NotaTallerBO();
                notaTallerLider.Auditoria = new AuditoriaBO();
                notaTallerLider.Adscripcion = new AdscripcionBO();
                notaTallerLider.Adscripcion.UnidadOperativa = new UnidadOperativaBO();
                notaTallerLider.Adscripcion.Sucursal = new SucursalBO();
                notaTallerLider.Almacen = new AlmacenBO();
                notaTallerLider.Estatus = new EstatusBO();
                notaTallerLider.Divisa = new DivisaBO();
                notaTallerLider.Divisa.MonedaDestino = new MonedaBO();
                #endregion Inicializar BO

                #region NotaTaller
                if (!row.IsNull("Id"))
                    notaTallerLider.Id = (int)Convert.ChangeType(row["Id"], typeof(int));
                if (!row.IsNull("EmpresaLiderID"))
                    notaTallerLider.EmpresaLiderId = (int)Convert.ChangeType(row["EmpresaLiderID"], typeof(int));
                if (!row.IsNull("UONombreCorto"))
                    notaTallerLider.Adscripcion.UnidadOperativa.NombreCorto = (string)Convert.ChangeType(row["UONombreCorto"], typeof(string));
                if (!row.IsNull("SucursalLiderID"))
                    notaTallerLider.SucursalLiderId = (int)Convert.ChangeType(row["SucursalLiderID"], typeof(int));
                if (!row.IsNull("SucursalNombreCorto"))
                    notaTallerLider.Adscripcion.Sucursal.NombreCorto = (string)Convert.ChangeType(row["SucursalNombreCorto"], typeof(string));
                if (!row.IsNull("AlmacenID"))
                    notaTallerLider.Almacen.Id = (int)Convert.ChangeType(row["AlmacenID"], typeof(int));
                if (!row.IsNull("AreaLiderID"))
                    notaTallerLider.AreaLiderId = (int)Convert.ChangeType(row["AreaLiderID"], typeof(int));
                if (!row.IsNull("MecanicoLiderID"))
                    notaTallerLider.MecanicoLiderId = (int)Convert.ChangeType(row["MecanicoLiderID"], typeof(int));
                if (!row.IsNull("UsuarioLiderID"))
                    notaTallerLider.UsuarioLiderId = (int)Convert.ChangeType(row["UsuarioLiderID"], typeof(int));
                if (!row.IsNull("OrdenServicioID"))
                    notaTallerLider.OrdenServicioId = (int)Convert.ChangeType(row["OrdenServicioID"], typeof(int));
                if (!row.IsNull("MovimientoId"))
                    notaTallerLider.MovimientoId = (int)Convert.ChangeType(row["MovimientoID"], typeof(int));
                if (!row.IsNull("EstatusID"))
                    notaTallerLider.Estatus.Id = (int)Convert.ChangeType(row["EstatusID"], typeof(int));
                if (!row.IsNull("NumeroReferencia"))
                    notaTallerLider.NumeroReferencia = (int)Convert.ChangeType(row["NumeroReferencia"], typeof(int));
                if (!row.IsNull("TipoReferencia"))
                    notaTallerLider.TipoReferencia = (string)Convert.ChangeType(row["TipoReferencia"], typeof(string));
                if (!row.IsNull("clienteLiderLiderID"))
                    notaTallerLider.ClienteLiderId = (int)Convert.ChangeType(row["clienteLiderLiderID"], typeof(int));
                if (!row.IsNull("DiasCredito"))
                    notaTallerLider.DiasCredito = (int)Convert.ChangeType(row["DiasCredito"], typeof(int));
                if (!row.IsNull("Observaciones"))
                    notaTallerLider.Observaciones = (string)Convert.ChangeType(row["Observaciones"], typeof(string));
                if (!row.IsNull("EsRescate"))
                    notaTallerLider.EsRescate = (bool)Convert.ChangeType(row["EsRescate"], typeof(bool));
                if (!row.IsNull("TipoPedidoID"))
                    notaTallerLider.TipoPedidoId = (int)Convert.ChangeType(row["TipoPedidoID"], typeof(int));
                if (!row.IsNull("EsDevolucion"))
                    notaTallerLider.EsDevolucion = (bool)Convert.ChangeType(row["EsDevolucion"], typeof(bool));
                if (!row.IsNull("NotaTallerReferencia"))
                    notaTallerLider.NotaTallerReferencia = (int)Convert.ChangeType(row["NotaTallerReferencia"], typeof(int));
                if (!row.IsNull("EstaImpreso"))
                    notaTallerLider.EstaImpreso = (bool)Convert.ChangeType(row["EstaImpreso"], typeof(bool));
                if (!row.IsNull("KitID"))
                    notaTallerLider.KitId = (int)Convert.ChangeType(row["KitID"], typeof(int));
                if (!row.IsNull("MonedaLiderId"))
                    notaTallerLider.MonedaLiderId = (int)Convert.ChangeType(row["MonedaLiderId"], typeof(int));
                if (!row.IsNull("MonedaDestinoCodigo"))
                    notaTallerLider.Divisa.MonedaDestino.Codigo = (string)Convert.ChangeType(row["MonedaDestinoCodigo"], typeof(string));
                if (!row.IsNull("FechaAplicacion"))
                    notaTallerLider.FechaAplicacion = (DateTime)Convert.ChangeType(row["FechaAplicacion"], typeof(DateTime));
                if (!row.IsNull("FC"))
                    notaTallerLider.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                #endregion NotaTaller

                lstDocumento.Add(notaTallerLider);
            }
            return lstDocumento;
            #endregion
        }
    }
}
