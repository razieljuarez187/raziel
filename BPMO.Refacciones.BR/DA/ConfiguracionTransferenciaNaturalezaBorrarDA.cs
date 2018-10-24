using System;
using System.Data.Common;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.DA {
    /// <summary>
    /// Acceso a datos para borrado de Configuraciones de Naturaleza
    /// </summary>
    internal class ConfiguracionTransferenciaNaturalezaBorrarDA {
        #region Atributos
        #endregion

        #region Propiedades
        #endregion

        #region Métodos

        internal decimal Borrar(IDataContext dataContext, int? configuracionId) {
            decimal result = 0;
            #region Validar Filtros
            string msjError = string.Empty;
            if (configuracionId == null)
                msjError += " , ID";
            if (msjError.Length > 0)
                throw new ArgumentNullException("Los siguientes campos no pueden ser vacios " + msjError.Substring(2));
            #endregion

            #region Conexión a BD
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
            Guid firma = Guid.NewGuid();
            DbCommand sqlCmd = null;
            try {
                dataContext.OpenConnection(firma);
                sqlCmd = dataContext.CreateCommand();
            } catch {
                manejadorDctx.RegresaProveedorInicial(dataContext);
                throw;
            }
            #endregion

            #region Armado de Sentencia SQL
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE eRef_confTransferenciaNaturalezaMovimiento ");
            StringBuilder sWhere = new StringBuilder();

            #region Valores
            if (configuracionId != null) {
                sWhere.Append(" AND ConfiguracionId = @configuracion_Id");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Id", configuracionId, System.Data.DbType.Int32);
            }
            #endregion Valores

            string where = sWhere.ToString().Trim();
            if (where.Length > 0) {
                if (where.StartsWith("AND "))
                    where = where.Substring(4);
                sCmd.Append(" WHERE " + where);
            }
            #endregion

            #region Ejecución Sentecia SQL
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                sqlCmd.ExecuteNonQuery();
                if (sqlCmd.Parameters["configuracion_Id"].Value != DBNull.Value) {
                    result = (decimal)Convert.ChangeType(sqlCmd.Parameters["configuracion_Id"].Value, typeof(decimal));
                }
            } catch (Exception ex) {
                throw ex;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
            #endregion

            return result;
        }
        #endregion
    }
}
