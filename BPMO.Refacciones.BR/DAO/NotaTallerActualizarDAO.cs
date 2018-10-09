using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;
using BPMO.Primitivos.Utilerias;

namespace BPMO.Refacciones.DAO
{
    internal class NotaTallerActualizarDAO : IDAOBaseActualizarDocumento
    {
        #region Atributos
        public int registrosAfectados;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados
        {
            get { return this.registrosAfectados; }
        }
        #endregion Propiedades

        #region Métodos
        public bool Actualizar(IDataContext dataContext, DocumentoBaseBO documentoBase)
        {
            #region Validar Parámetos
            NotaTallerBO notaTaller = null;
            if (documentoBase is NotaTallerBO)
                notaTaller = (NotaTallerBO)documentoBase;
            string mensajeError = String.Empty;
            if (notaTaller == null || notaTaller.Id == null)
                mensajeError += " , NotaTaller.Id";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
			#endregion Validar Parámetros

            #region Conexión a BD
            ManejadorDataContext manejadorDC = new ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try
            {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            }
            catch
            {
                throw;
            }
            #endregion

			#region Armado de Sentencia SQL
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder sValue = new StringBuilder();
            sCmd.Append(" UPDATE inv_encNotaTaller SET ");
            if (notaTaller.Observaciones != null)
            {
                sValue.Append(" , Observaciones = @NotaTaller_Observaciones");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_Observaciones";
                sqlParam.Value = notaTaller.Observaciones;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notaTaller.Estatus != null && notaTaller.Estatus.Id != null)
            {
                sValue.Append(" , Status = @NotaTaller_Status");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "NotaTaller_Status";
                sqlParam.Value = notaTaller.Estatus.Id;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            sValue.Append(" WHERE NotaTallerID = @NotaTaller_ID");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTaller_ID";
            sqlParam.Value = notaTaller.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            int iRes = 0;
            string sValores = sValue.ToString().Trim();
            if (sValores.Length > 0) {
                if (sValores.StartsWith(","))
                    sValores = sValores.Substring(2);
            }
            sCmd.Append(sValores);
			#endregion Armado de Sentencia SQL

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
            #endregion Ejecución Sentencia SQL
        }
        #endregion Métodos
    }
}
