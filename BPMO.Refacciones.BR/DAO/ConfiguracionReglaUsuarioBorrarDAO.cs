using System;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Refacciones.BO;

namespace BPMO.Refacciones.DAO {
    /// <summary>
    /// Acceso a Datos para Eliminar registros de ConfiguracionReglaUsuario
    /// </summary>
    internal class ConfiguracionReglaUsuarioBorrarDAO : IDAOBaseBorrarAuditoria {
        #region Atributos
        private int registrosAfectados;
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la cantidad de registros afectados despues de realizar la operación.
        /// </summary>
        /// <remarks>Tiene valor solo cuando se utilizan los métodos Insertar,Actualizar y Borrar.</remarks>
        public int RegistrosAfectados {
            get { return this.registrosAfectados; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Elimina un registro ConfiguracionReglaUsuario de la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a base de datos</param>
        /// <param name="auditoriaBase">Objeto con los parámetros del registro a borrar</param>
        /// <returns></returns>
        public bool Borrar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase) {
            #region Validar Filtros
            ConfiguracionReglaUsuarioBO configRegla = null;
            if (auditoriaBase is ConfiguracionReglaUsuarioBO)
                configRegla = (ConfiguracionReglaUsuarioBO)auditoriaBase;
            string msjError = string.Empty;
            if (configRegla == null)
                msjError += " , ConfiguracionRegla";
            if (dataContext == null)
                msjError += " , DataContext";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!configRegla.Id.HasValue)
                msjError += " , Id";
            if (configRegla.Auditoria == null)
                msjError += " , Auditoria";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            if (!configRegla.Auditoria.FUA.HasValue)
                msjError += " , Auditoria.FUA";
            if (msjError.Length > 0)
                throw new ArgumentNullException(msjError.Substring(2));
            #endregion

            #region Conexión a BD
            BPMO.Primitivos.Utilerias.ManejadorDataContext manejadorDctx = new Primitivos.Utilerias.ManejadorDataContext(dataContext, "LIDER");
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
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM eRef_confReglasUsuarios WHERE");
            // Id
            sCmd.Append(" ConfiguracionReglaId = @configuracion_Id");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Id", configRegla.Id, System.Data.DbType.Int32);
            // Fecha Última Modificación
            sCmd.Append(" AND FA = @configuracion_Old_FA");
            Utileria.AgregarParametro(sqlCmd, "configuracion_Old_FA", configRegla.Auditoria.FUA, System.Data.DbType.DateTime);
            #endregion

            #region Ejecución Sentecia SQL
            int result = 0;
            try {
                sqlCmd.CommandText = sCmd.Replace("@", dataContext.ParameterSymbol).ToString();
                result = sqlCmd.ExecuteNonQuery();
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDctx.RegresaProveedorInicial(dataContext);
            }
            registrosAfectados = result;
            if (result < 1)
                throw new Exception("Hubo un error al eliminar el registro.");
            else
                return true;
            #endregion
        } 
        #endregion /Métodos
    }
}
