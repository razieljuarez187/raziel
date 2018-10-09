using System.Data;
using System.Data.Common;

namespace BPMO.Refacciones.DAO {
    internal class Utileria {
        /// <summary>
        /// Parametrización de campos
        /// </summary>
        /// <param name="sqlCmd">DbCommand</param>
        /// <param name="nombre">Nombre del parámetro</param>
        /// <param name="valor">Valor</param>
        /// <param name="tipo">Tipo del parámetro</param>
        internal static void AgregarParametro(DbCommand sqlCmd, string nombre, object valor, DbType tipo) {
            DbParameter sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = nombre;
            sqlParam.Value = valor;
            sqlParam.DbType = tipo;
            sqlCmd.Parameters.Add(sqlParam);
        }
    }
}
