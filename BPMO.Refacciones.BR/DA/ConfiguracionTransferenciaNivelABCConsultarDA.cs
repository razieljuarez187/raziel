using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.DA {
    /// <summary>
    /// Acceso a datos para consulta Configuraciones de nivel ABC
    /// </summary>
    internal class ConfiguracionTransferenciaNivelABCConsultarDA {
        #region Métodos
        /// <summary>
        /// Obtiene un DataSet con las configuraciones asignadas
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="configRegla">Objeto con los parámetros de búsqueda</param>
        /// <returns></returns>
        public DataSet Consultar(IDataContext dataContext, int? configuracionId) {
            #region Validar parámetos
            string mensajeError = String.Empty;
            if (configuracionId == null)
                mensajeError += " , configuracionId";
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
            sCmd.Append(" SELECT conf.ConfiguracionId, conf.NivelABCId ,catNivABC.ClaveABC,catNivABC.Descripcion,catNivABC.CalculoABC");
            sCmd.Append(" FROM eRef_confTransferenciaNivelABC conf ");
            sCmd.Append("   INNER JOIN ref_CatNivABC catNivABC ON (catNivABC.NivelABCId = conf.NivelABCId) ");
            StringBuilder sWhere = new StringBuilder();

            #region Valores
            if (configuracionId!=null) {
                sWhere.Append(" AND conf.ConfiguracionId = @configuracion_Id");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Id", configuracionId, System.Data.DbType.Int32);
            }
            #endregion Valores

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
