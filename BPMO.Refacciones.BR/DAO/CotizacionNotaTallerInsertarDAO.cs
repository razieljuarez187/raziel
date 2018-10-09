using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a datos para insertar CotizacionNotaTaller
    /// </summary>
    internal class CotizacionNotaTallerInsertarDAO : IDAOBaseInsertarDocumento {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion

        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado {
            get { return this.ultimoIdGenerado; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Registrar CotizacionNotaTaller
        /// </summary>
        /// <param name="dataContext">Acceso a base de datos</param>
        /// <param name="documentoBase">CotizacionNotaTaller a insertar</param>
        /// <returns>Indica si la CotizacionNotaTaller se inserto correctamente</returns>
        public bool Insertar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            #region Validar parametros
            CotizacionNotaTallerBO cotizacionNotaTaller = null; ;
            if (documentoBase is CotizacionNotaTallerBO)
                cotizacionNotaTaller = (CotizacionNotaTallerBO)documentoBase;
            string mensajeError = string.Empty;
            if (cotizacionNotaTaller == null)
                mensajeError += " , CotizacionNotaTaller";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            if (cotizacionNotaTaller.EmpresaLiderId == null)
                mensajeError += " , CotizacionNotaTaller.EmpresaId";
            if (cotizacionNotaTaller.SucursalLiderId == null)
                mensajeError += " , CotizacionNotaTaller.SucursalId";
            if (cotizacionNotaTaller.AlmacenLiderId == null)
                mensajeError += " , CotizacionNotaTaller.AlmacenId";
            if (cotizacionNotaTaller.AreaId == null)
                mensajeError += " , CotizacionNotaTaller.AreaId";
            if (cotizacionNotaTaller.MecanicoId == null)
                mensajeError += " , CotizacionNotaTaller.TecnicoId";
            if (cotizacionNotaTaller.UsuarioLiderId == null)
                mensajeError += " , CotizacionNotaTaller.UsuarioLiderId";
            if (cotizacionNotaTaller.ClienteLiderId == null)
                mensajeError += " , CotizacionNotaTaller.ClienteId";
            if (cotizacionNotaTaller.MonedaId == null)
                mensajeError += " , CotizacionNotaTaller.MonedaId";
            if (cotizacionNotaTaller.Observaciones == null)
                mensajeError += " , CotizacionNotaTaller.Observaciones";
            if (cotizacionNotaTaller.StatusId == null)
                mensajeError += " , CotizacionNotaTaller.StatusId";
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
            sCmd.Append(" INSERT inv_encCotizacionNT (EmpresaId, SucursalId, AlmacenId, AreaId, TecnicoID, UsuarioId, ClienteID, MonedaID, CotizacionOrdenID,");
            sCmd.Append(" OrdenID, StatusID, Observaciones, FechaAutoriza, FechaAplica, FechaRechaza, FechaCaduca, NotaTallerId)");
            sCmd.Append(" VALUES(");
            #region Valores
            sValue.Append(", @CotizacionNotaTaller_EmpresaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_EmpresaId";
            sqlParam.Value = cotizacionNotaTaller.EmpresaLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_SucursalId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_SucursalId";
            sqlParam.Value = cotizacionNotaTaller.SucursalLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_AlmacenId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_AlmacenId";
            sqlParam.Value = cotizacionNotaTaller.AlmacenLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_AreaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_AreaId";
            sqlParam.Value = cotizacionNotaTaller.AreaId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_MecanicoId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_MecanicoId";
            sqlParam.Value = cotizacionNotaTaller.MecanicoId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_UsuarioId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_UsuarioId";
            sqlParam.Value = cotizacionNotaTaller.UsuarioLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_ClienteId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_ClienteId";
            sqlParam.Value = cotizacionNotaTaller.ClienteLiderId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @CotizacionNotaTaller_MonedaId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_MonedaId";
            sqlParam.Value = cotizacionNotaTaller.MonedaId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            if (cotizacionNotaTaller.CotizacionOrdenId != null) {
                sValue.Append(", @CotizacionNotaTaller_CotizacionOrdenId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "CotizacionNotaTaller_CotizacionOrdenId";
                sqlParam.Value = cotizacionNotaTaller.CotizacionOrdenId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL");
            }
            if (cotizacionNotaTaller.OrdenServicioId != null) {
                sValue.Append(", @CotizacionNotaTaller_OrdenId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "CotizacionNotaTaller_OrdenId";
                sqlParam.Value = cotizacionNotaTaller.OrdenServicioId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL");
            }
            sValue.Append(", @CotizacionNotaTaller_StatusId");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_StatusId";
            sqlParam.Value = cotizacionNotaTaller.StatusId;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            if (cotizacionNotaTaller.Observaciones != null) {
                sValue.Append(", @CotizacionNotaTaller_Observaciones");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "CotizacionNotaTaller_Observaciones";
                sqlParam.Value = cotizacionNotaTaller.Observaciones;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            } else {
                sValue.Append(", NULL");
            }
            if (cotizacionNotaTaller.FechaAutoriza != null)
                sValue.Append(", GETDATE()");
            else
                sValue.Append(", NULL");
            if (cotizacionNotaTaller.FechaAplica != null)
                sValue.Append(", GETDATE()");
            else
                sValue.Append(", NULL");
            if (cotizacionNotaTaller.FechaRechaza != null)
                sValue.Append(", GETDATE()");
            else
                sValue.Append(", NULL");
            if (cotizacionNotaTaller.FechaCaduca != null)
                sValue.Append(", GETDATE()");
            else
                sValue.Append(", NULL");

            if (cotizacionNotaTaller.NotaTallerId.HasValue)
                sValue.Append(", " + cotizacionNotaTaller.NotaTallerId.ToString());
            else
                sValue.Append(", NULL");

            #endregion Valores
            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");
            sCmd.Append(" SELECT @identity = SCOPE_IDENTITY()");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "identity";
            sqlParam.DbType = DbType.Int32;
            sqlParam.Direction = ParameterDirection.Output;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion

            #region Ejecución Sentecia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
                if (result > 0) {
                    object resultId = sqlCmd.Parameters["identity"].Value;
                    if (!(resultId is DBNull))
                        ultimoIdGenerado = Convert.ToInt32(resultId);
                    else
                        ultimoIdGenerado = null;
                }
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
        #endregion
    }
}
