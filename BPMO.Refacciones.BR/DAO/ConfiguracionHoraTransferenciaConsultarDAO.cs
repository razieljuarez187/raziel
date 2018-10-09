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
    /// <summary>
    /// Acceso a Datos para Consultar registros de ConfiguracionHoraTransferencia
    /// </summary>
    class ConfiguracionHoraTransferenciaConsultarDAO : IDAOBaseConsultarAuditoriaDetalle {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ConfiguracionHoraTransferenciaBO en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <param name="objetoMaestro">Objeto padre que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        /// 
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro) {
            #region Validar parámetos
            ConfiguracionHoraTransferenciaBO config = null;
            ConfiguracionTransferenciaBO configMaestro = null;
            if (auditoriaBase is ConfiguracionHoraTransferenciaBO)
                config = (ConfiguracionHoraTransferenciaBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (config == null)
                mensajeError += " , ConfiguracionHora";
            if (objetoMaestro is ConfiguracionTransferenciaBO)
                configMaestro = (ConfiguracionTransferenciaBO)objetoMaestro;
            mensajeError = String.Empty;
            if (objetoMaestro == null)
                mensajeError += " , Configuracion";
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
            sCmd.Append(" SELECT ConfiguracionId, ConfiguracionHoraId, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo, ");
            sCmd.Append(" Activo, UC, FC, UA, FA ");
            sCmd.Append(" FROM eRef_confHoraTransferencia ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (configMaestro.Id.HasValue) {
                sWhere.Append(" AND ConfiguracionId = @Configuracion_Id");
                Utileria.AgregarParametro(sqlCmd, "Configuracion_Id", configMaestro.Id, DbType.Int32);
            }
            if (config.Id.HasValue) {
                sWhere.Append(" AND ConfiguracionHoraId = @configuracionHora_Id");
                Utileria.AgregarParametro(sqlCmd, "configuracionHora_Id", config.Id, DbType.Int32);
            }
            if (config.Lunes.HasValue) {
                sWhere.Append(" AND Lunes = @configuracion_Lunes");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Lunes", config.Lunes, DbType.Time);
            }
            if (config.Martes.HasValue) {
                sWhere.Append(" AND Martes = @configuracion_Martes");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Martes", config.Martes, DbType.Time);
            }
            if (config.Miercoles.HasValue) {
                sWhere.Append(" AND Miercoles = @configuracion_Miercoles");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Miercoles", config.Miercoles, DbType.Time);
            }
            if (config.Jueves.HasValue) {
                sWhere.Append(" AND Jueves = @configuracion_Jueves");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Jueves", config.Jueves, DbType.Time);
            }
            if (config.Viernes.HasValue) {
                sWhere.Append(" AND Viernes = @configuracion_Viernes");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Viernes", config.Viernes, DbType.Time);
            }
            if (config.Sabado.HasValue) {
                sWhere.Append(" AND Sabado = @configuracion_Sabado");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Sabado", config.Sabado, DbType.Time);
            }
            if (config.Domingo.HasValue) {
                sWhere.Append(" AND Domingo = @configuracion_Domingo");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Domingo", config.Domingo, DbType.Time);
            }
            if (config.Activo.HasValue) {
                sWhere.Append(" AND Activo = @configuracion_Activo");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Activo", config.Activo, DbType.Boolean);
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
                sqlAdapter.Fill(ds, "eRef_confHoraTransferencia");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstConfiguraciones = new List<AuditoriaBaseBO>();
            ConfiguracionHoraTransferenciaBO configuracionHora = null;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                configuracionHora = new ConfiguracionHoraTransferenciaBO();
                configuracionHora.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionHoraTransferenciaBO
                if (!row.IsNull("ConfiguracionHoraId"))
                    configuracionHora.Id = (Int32)Convert.ChangeType(row["ConfiguracionHoraId"], typeof(Int32));
                if (!row.IsNull("Lunes"))
                    configuracionHora.Lunes = (TimeSpan)Convert.ChangeType(row["Lunes"], typeof(TimeSpan));
                if (!row.IsNull("Martes"))
                    configuracionHora.Martes = (TimeSpan)Convert.ChangeType(row["Martes"], typeof(TimeSpan));
                if (!row.IsNull("Miercoles"))
                    configuracionHora.Miercoles = (TimeSpan)Convert.ChangeType(row["Miercoles"], typeof(TimeSpan));
                if (!row.IsNull("Jueves"))
                    configuracionHora.Jueves = (TimeSpan)Convert.ChangeType(row["Jueves"], typeof(TimeSpan));
                if (!row.IsNull("Viernes"))
                    configuracionHora.Viernes = (TimeSpan)Convert.ChangeType(row["Viernes"], typeof(TimeSpan));
                if (!row.IsNull("Sabado"))
                    configuracionHora.Sabado = (TimeSpan)Convert.ChangeType(row["Sabado"], typeof(TimeSpan));
                if (!row.IsNull("Domingo"))
                    configuracionHora.Domingo = (TimeSpan)Convert.ChangeType(row["Domingo"], typeof(TimeSpan));
                if (!row.IsNull("Activo"))
                    configuracionHora.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("UC"))
                    configuracionHora.Auditoria.UC = (Int32)Convert.ChangeType(row["UC"], typeof(Int32));
                if (!row.IsNull("FC"))
                    configuracionHora.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                if (!row.IsNull("UA"))
                    configuracionHora.Auditoria.UUA = (Int32)Convert.ChangeType(row["UA"], typeof(Int32));
                if (!row.IsNull("FA"))
                    configuracionHora.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FA"], typeof(DateTime));
                #endregion /ConfiguracionHoraTransferenciaBO

                lstConfiguraciones.Add(configuracionHora);
            }
            return lstConfiguraciones;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos

    }
}
