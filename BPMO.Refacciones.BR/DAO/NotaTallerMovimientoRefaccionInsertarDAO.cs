using System;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO
{
    internal class NotaTallerMovimientoRefaccionInsertarDAO
    {
        #region Atributos
        private int registrosAfectados;
        private int? ultimoIdGenerado;
        #endregion Atributos

        #region Propiedades
        public int RegistrosAfectados
        {
            get { return this.registrosAfectados; }
        }
        public int? UltimoIdGenerado
        {
            get { return this.ultimoIdGenerado; }
        }
        #endregion Propiedades

        #region Métodos
        public bool Insertar(IDataContext dataContext, NotaTallerMovimientoRefaccionBO movimiento)
        {
            #region Validar parametros
            string mensajeError = String.Empty;
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (movimiento.NotaTallerOriginal == null || movimiento.NotaTallerOriginal.Id == null)
                mensajeError += " , NotaTallerOriginal";
            if (movimiento.NotaTallerNueva == null || movimiento.NotaTallerNueva.Id == null)
                mensajeError += " , NotaTallerNueva";
            if (movimiento.Movimiento == null || movimiento.Movimiento.Id == null)
                mensajeError += " , Movimiento";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2), "Los siguientes datos no pueden ser nulos!!!");
            #endregion Validar parametros

            #region Conexión a BD
            if (!dataContext.CheckProviderByName("LIDER"))
                throw new ArgumentNullException("LIDER", "No se ha definido un proveedor de conexiones para la base de datos requerida!!!");
            string incomingDataContext = dataContext.CurrentProvider;
            if (dataContext.CurrentProvider != "LIDER")
                dataContext.SetCurrentProvider("LIDER");
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
            sCmd.Append(" INSERT inv_MovtosNotaTaller (NotaTallerOriID, NotaTallerID, MovimientoID)");
            sCmd.Append(" VALUES(");
            #region Valores
            sValue.Append(", @NotaTallerOriginal_Id");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTallerOriginal_Id";
            sqlParam.Value = movimiento.NotaTallerOriginal.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @NotaTallerNueva_Id");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "NotaTallerNueva_Id";
            sqlParam.Value = movimiento.NotaTallerNueva.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sValue.Append(", @Movimiento_Id");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "Movimiento_Id";
            sqlParam.Value = movimiento.Movimiento.Id;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            #endregion Valores
            string cmd = sValue.ToString().Trim();
            if (cmd.StartsWith(","))
                cmd = cmd.Substring(1);
            sCmd.Append(cmd);
            sCmd.Append(")");
            #endregion Armado de Sentencia SQL

            #region Ejecución Sentecia SQL
            int result = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                dataContext.CloseConnection(firma);
                if (dataContext.CurrentProvider != incomingDataContext)
                    dataContext.SetCurrentProvider(incomingDataContext);
            }
            registrosAfectados = result;

            if (result < 1)
                throw new Exception("Hubo un error al crear el registro.");
            else
                return true;
            #endregion Ejecución Sentencia SQL

        }
        #endregion Métodos
    }
}
