using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using BPMO.Basicos.BO;
using BPMO.Patterns.Creational.DataContext;
using BPMO.Primitivos.Utilerias;
using BPMO.Refacciones.BO;
using BPMO.Refacciones.DAO;

namespace BPMO.Refacciones.BR.DAO {
    /// <summary>
    /// Acceso a Datos para Consultar registros de ConfiguracionCantidadTransferencia
    /// </summary>
    /// </summary>
    class ConfiguracionCantidadTransferenciaConsultarDAO : IDAOBaseConsultarAuditoriaDetalle {
        #region Atributos
        #endregion /Atributos

        #region Métodos
        /// <summary>
        /// Consulta una lista de ConfiguracionReglaUsuario en la base de datos
        /// </summary>
        /// <param name="dataContext">Objeto que provee acceso a la base de datos</param>
        /// <param name="auditoriaBase">Objeto que provee los parámetros de búsqueda</param>
        /// <returns>Lista de Configuraciones de Reglas</returns>
        /// 
        public List<AuditoriaBaseBO> Consultar(IDataContext dataContext, AuditoriaBaseBO auditoriaBase, AuditoriaBaseBO objetoMaestro) {
            #region Validar parámetos
            ConfiguracionCantidadTransferenciaBO config = null;
            ConfiguracionTransferenciaBO configMaestro = null;
            if (auditoriaBase is ConfiguracionCantidadTransferenciaBO)
                config = (ConfiguracionCantidadTransferenciaBO)auditoriaBase;
            string mensajeError = String.Empty;
            if (config == null)
                mensajeError += " , Configuracion Cantidad";
            if (objetoMaestro is ConfiguracionTransferenciaBO)
                configMaestro = (ConfiguracionTransferenciaBO)objetoMaestro;
            mensajeError = String.Empty;
            if (objetoMaestro == null)
                mensajeError += " , Configuracion";
            if (dataContext == null)
                mensajeError += " , DataContext";
            if (mensajeError.Length > 0)
                throw new ArgumentNullException(mensajeError.Substring(2));
            if (configMaestro.Id== null)
                mensajeError += " , ConfiguracionId";
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
            sCmd.Append(" SELECT ConfiguracionId, ConfiguracionCantidadId, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo ");
            sCmd.Append(" , Activo, UC, FC, UA, FA ");
            sCmd.Append(" FROM eRef_confCantidadTransferencia ");
            StringBuilder sWhere = new StringBuilder();
            #region Valores
            if (configMaestro.Id.HasValue) {
                sWhere.Append(" AND ConfiguracionId = @Configuracion_Id");
                Utileria.AgregarParametro(sqlCmd, "Configuracion_Id", configMaestro.Id, DbType.Int32);
            }
            if (config.Id.HasValue) {
                sWhere.Append(" AND ConfiguracionCantidadId = @configuracionCantidad_Id");
                Utileria.AgregarParametro(sqlCmd, "configuracionCantidad_Id", config.Id, DbType.Int32);
            }
            if (config.Lunes != null && config.Lunes.HasValue) {
                sWhere.Append(" AND Lunes = @configuracion_Lunes");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Lunes", config.Lunes, DbType.Int32);
            }
            if (config.Martes != null && config.Martes.HasValue) {
                sWhere.Append(" AND Martes = @configuracion_Martes");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Martes", config.Martes, DbType.Int32);
            }
            if (config.Miercoles != null && config.Miercoles.HasValue) {
                sWhere.Append(" AND Miercoles = @configuracion_Miercoles");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Miercoles", config.Miercoles, DbType.Int32);
            }
            if (config.Jueves != null && config.Jueves.HasValue) {
                sWhere.Append(" AND Jueves = @configuracion_Jueves");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Jueves", config.Jueves, DbType.Int32);
            }
            if (config.Viernes != null && config.Viernes.HasValue) {
                sWhere.Append(" AND Viernes = @configuracion_Viernes");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Viernes", config.Viernes, DbType.Int32);
            }
            if (config.Sabado != null && config.Sabado.HasValue) {
                sWhere.Append(" AND Sabado = @configuracion_Sabado");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Sabado", config.Sabado, DbType.Int32);
            }
            if (config.Domingo != null && config.Domingo.HasValue) {
                sWhere.Append(" AND Domingo = @configuracion_Domingo");
                Utileria.AgregarParametro(sqlCmd, "configuracion_Domingo", config.Domingo, DbType.Int32);
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
                sqlAdapter.Fill(ds, "ConfiguracionesReglas");
            } catch {
                throw;
            } finally {
                dataContext.CloseConnection(firma);
                manejadorDC.RegresaProveedorInicial(dataContext);
            }
            #endregion

            #region Mapeo DataSet a BO
            List<AuditoriaBaseBO> lstConfiguraciones = new List<AuditoriaBaseBO>();
            ConfiguracionCantidadTransferenciaBO configuracionCantidad = null;
            foreach (DataRow row in ds.Tables[0].Rows) {
                #region Inicializar BO
                configuracionCantidad = new ConfiguracionCantidadTransferenciaBO();
                configuracionCantidad.Auditoria = new AuditoriaBO();
                #endregion /Inicializar BO

                #region ConfiguracionesReglas
                if (!row.IsNull("ConfiguracionCantidadId"))
                    configuracionCantidad.Id = (Int32)Convert.ChangeType(row["ConfiguracionCantidadId"], typeof(Int32));
                if (!row.IsNull("Lunes"))
                    configuracionCantidad.Lunes = (Int32)Convert.ChangeType(row["Lunes"], typeof(Int32));
                if (!row.IsNull("Martes"))
                    configuracionCantidad.Martes = (Int32)Convert.ChangeType(row["Martes"], typeof(Int32));
                if (!row.IsNull("Miercoles"))
                    configuracionCantidad.Miercoles = (Int32)Convert.ChangeType(row["Miercoles"], typeof(Int32));
                if (!row.IsNull("Jueves"))
                    configuracionCantidad.Jueves = (Int32)Convert.ChangeType(row["Jueves"], typeof(Int32));
                if (!row.IsNull("Viernes"))
                    configuracionCantidad.Viernes = (Int32)Convert.ChangeType(row["Viernes"], typeof(Int32));
                if (!row.IsNull("Sabado"))
                    configuracionCantidad.Sabado = (Int32)Convert.ChangeType(row["Sabado"], typeof(Int32));
                if (!row.IsNull("Domingo"))
                    configuracionCantidad.Domingo = (Int32)Convert.ChangeType(row["Domingo"], typeof(Int32));
                if (!row.IsNull("Activo"))
                    configuracionCantidad.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
                if (!row.IsNull("UC"))
                    configuracionCantidad.Auditoria.UC = (Int32)Convert.ChangeType(row["UC"], typeof(Int32));
                if (!row.IsNull("FC"))
                    configuracionCantidad.Auditoria.FC = (DateTime)Convert.ChangeType(row["FC"], typeof(DateTime));
                if (!row.IsNull("UA"))
                    configuracionCantidad.Auditoria.UUA = (Int32)Convert.ChangeType(row["UA"], typeof(Int32));
                if (!row.IsNull("FA"))
                    configuracionCantidad.Auditoria.FUA = (DateTime)Convert.ChangeType(row["FA"], typeof(DateTime));
                #endregion /ConfiguracionesReglas

                lstConfiguraciones.Add(configuracionCantidad);
            }
            return lstConfiguraciones;
            #endregion Mapeo DataSet a BO
        }
        #endregion /Métodos

    }
}
