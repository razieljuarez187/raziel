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
    /// Acceso a datos para actualizar una Cotización de Nota de Taller
    /// </summary>
    internal class CotizacionNotaTallerActualizarDAO : IDAOBaseActualizarDocumento {
        #region Atributos
        public int registrosAfectados;
        #endregion
        #region Propiedades
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion
        #region Métodos
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO documentoBase) {
            #region Validar Parámetos
            CotizacionNotaTallerBO cotizacionNotaTaller = null;
            if (documentoBase is CotizacionNotaTallerBO)
                cotizacionNotaTaller = (CotizacionNotaTallerBO)documentoBase;
            string mensajeError = String.Empty;
            if (cotizacionNotaTaller == null || cotizacionNotaTaller.Id == null)
                mensajeError += " , cotizacionNotaTaller.CotizacionNTId";
            if (dataContext == null)
                mensajeError += " , DataContext";
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
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" UPDATE inv_encCotizacionNT SET ");
            if (cotizacionNotaTaller.Observaciones != null) {
                sValue.Append(" , Observaciones = @CotizacionNotaTaller_Observaciones");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "CotizacionNotaTaller_Observaciones";
                sqlParam.Value = cotizacionNotaTaller.Observaciones;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.StatusId != null) {
                sValue.Append(" , StatusID = @NotaTaller_Status");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_Status";
                sqlParam.Value = cotizacionNotaTaller.StatusId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.FechaAutoriza != null) {
                sValue.Append(", FechaAutoriza = @NotaTaller_FechaAutoriza");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_FechaAutoriza";
                sqlParam.Value = cotizacionNotaTaller.FechaAutoriza;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.FechaAplica != null) {
                sValue.Append(", FechaAplica = @NotaTaller_FechaAplica");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_FechaAplica";
                sqlParam.Value = cotizacionNotaTaller.FechaAplica;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.FechaRechaza != null) {
                sValue.Append(", FechaRechaza = @NotaTaller_FechaRechaza");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_FechaRechaza";
                sqlParam.Value = cotizacionNotaTaller.FechaRechaza;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.FechaCaduca != null) {
                sValue.Append(", FechaCaduca = @NotaTaller_FechaCaduca");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_FechaCaduca";
                sqlParam.Value = cotizacionNotaTaller.FechaCaduca;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cotizacionNotaTaller.NotaTallerId != null) {
                sValue.Append(" , NotaTallerId = @NotaTaller_NotaTallerId");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_NotaTallerId";
                sqlParam.Value = cotizacionNotaTaller.NotaTallerId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            
            sValue.Append(" WHERE CotizacionNTID = @CotizacionNotaTaller_ID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "CotizacionNotaTaller_ID";
            sqlParam.Value = cotizacionNotaTaller.Id;
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

            #region Ejecución Sentencia SQL
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
