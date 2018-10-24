using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;

namespace BPMO.Refacciones.DA {
    class ConfiguracionTransferenciaNaturalezaInsertarDA {
        #region Métodos
        /// <summary>
        /// Obtiene un DataSet con las configuraciones asignadas
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="configRegla">Objeto con los parámetros de búsqueda</param>
        /// <returns></returns>
        public DataSet Insertar(IDataContext dataContext, int? configuracionId, int? NaturalezaMovId) {
            #region Validar parámetos
            string mensajeError = String.Empty;
            if (configuracionId == null)
                mensajeError += " , ConfiguracionId";
            if (NaturalezaMovId == null)
                mensajeError += " , NaturalezaMovId";
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
            sCmd.Append(" INSERT INTO [eRef_confTransferenciaNaturalezaMovimiento]");
            sCmd.Append(" ([ConfiguracionId], [NaturalezaMovId]) ");
            sCmd.Append(" VALUES ('" + configuracionId + "', '" + NaturalezaMovId + "')");
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dataContext.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ConfiguracionesNivelABC");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            return ds;
        }
        #endregion /Métodos
    }
}
